using System;
using System.Threading;
using System.Threading.Tasks;
using core;
using core.Abstractions.TypeCodes;
using core.Infrastructure.BL;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Notifications;
using core.Infrastructure.Notifications.Telegram;
using core.Trading;
using core.Trading.Models;
using core.TypeCodes;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyNatsClient;
using MyNatsClient.Encodings.Json;
using MyNatsClient.Events;
using MyNatsClient.Extensions;
using MyNatsClient.Ops;
using Newtonsoft.Json;

namespace TradingProcessor.Workers
{
	public class ForecastProcessor : BackgroundService
	{
		private readonly ILogger<ForecastProcessor> _logger;
		private IOptions<AppSettings> _settings;
		private TradingContextBuilder _contextBuilder;
		private TelegramClient _telegramClient;
		private IMemoryCache _cache;

		public ForecastProcessor (
			IOptions<AppSettings> settings,
			ILogger<ForecastProcessor> logger,
			TradingContextBuilder contextBuilder,
			TelegramClient telegramClient,
			IMemoryCache cache)
		{
			_logger = logger;
			_settings = settings;
			_contextBuilder = contextBuilder;
			_telegramClient = telegramClient;
			_cache = cache;
		}

		protected override async Task ExecuteAsync (CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					ConnectionInfo cnInfo = new ConnectionInfo(_settings.Value.BusConnectionString);
					using (NatsClient natsClient = new NatsClient(cnInfo))
					{
						if (!natsClient.IsConnected)
						{
							natsClient.Connect();
						}

						await natsClient.SubAsync(_settings.Value.CandlesQueueName, stream => stream.Subscribe(msg =>
						{
							try
							{
								Task.Run(async () => await Process(msg, natsClient));
							}
							catch (Exception ex)
							{
								_logger.LogError($"TradingProcessor failed with message: {ex.Message}", ex);
							}
						}));

						natsClient.Events.OfType<ClientDisconnected>().Subscribe(ev =>
						{
							Console.WriteLine($"Client was disconnected due to reason '{ev.Reason}'");

							if (!cnInfo.AutoReconnectOnFailure)
							{
								ev.Client.Connect();
							}
						});

						while (!stoppingToken.IsCancellationRequested)
						{
							await Task.Delay(TimeSpan.FromSeconds(1));
						}

						natsClient.Disconnect();
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex.Message);
				}
			}
		}

		private async Task Process (MsgOp msg, NatsClient natsClient)
		{
			ExchangeCode code = ExchangeCode.UNKNOWN;
			string payload = msg.GetPayloadAsString();
			Notification<Candle> response = JsonConvert.DeserializeObject<Notification<Candle>>(payload[1..]);

			if (response.Code == ActionCode.CREATED.Code)
			{
				Candle receivedCandle = response.Payload;
				code = ExchangeCode.Create(receivedCandle.ExchangeCode);

				TradingContext context = await _contextBuilder.Build(code, receivedCandle.Symbol);
				ITradingAdviceCode forecast = context.GetForecast();

				_logger.LogInformation($"{DateTime.UtcNow} - {code.Description} - {receivedCandle.Symbol} - Forecast: {forecast.Description}");

				if (_settings.Value.SendTelegramNotifacation)
				{
					string message = $"{DateTime.UtcNow} UTC - Exchange: {code.Description} {receivedCandle.Symbol} - Forecaset {forecast.Description}";
					await _telegramClient.SendMessage(message);
				}

				TradingForecast forecastInfo = new TradingForecast();
				forecastInfo.ExchangeCode = receivedCandle.ExchangeCode;
				forecastInfo.Symbol = receivedCandle.Symbol;
				forecastInfo.ForecastCode = forecast.Code;

				await natsClient.PubAsJsonAsync(_settings.Value.OrdersQueueName, new Notification<TradingForecast>() { Code = ActionCode.FORECAST.Code, Payload = forecastInfo });
			}
		}
	}
}
