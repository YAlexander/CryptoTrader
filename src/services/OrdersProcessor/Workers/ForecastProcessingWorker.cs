using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using core.Abstractions.Database;
using core.Abstractions.TypeCodes;
using core.Infrastructure.BL;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Notifications;
using core.Trading.Models;
using core.TypeCodes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyNatsClient;
using MyNatsClient.Encodings.Json;
using MyNatsClient.Events;
using MyNatsClient.Ops;
using Newtonsoft.Json;

namespace OrdersProcessor.Workers
{
	public class OrdersProcessingWorker : BackgroundService
	{
		private readonly ILogger<OrdersProcessingWorker> _logger;
		private IOptions<AppSettings> _settings;
		private ExchangeConfigProcessor _exchangeConfigProcessor;
		private AutoTradingProcessor _autoTradingProcessor;

		public OrdersProcessingWorker (
			ILogger<OrdersProcessingWorker> logger,
			IOptions<AppSettings> settings,
			AutoTradingProcessor autoTradingProcessor,
			ExchangeConfigProcessor exchangeConfigProcessor)
		{
			_exchangeConfigProcessor = exchangeConfigProcessor;
			_logger = logger;
			_settings = settings;
			_autoTradingProcessor = autoTradingProcessor;
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
								_logger.LogError($"OrdersProcessor failed with message: {ex.Message}", ex);
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
			Notification<TradingForecast> response = JsonConvert.DeserializeObject<Notification<TradingForecast>>(payload[1..]);

			if (response.Code == ActionCode.FORECAST.Code)
			{
				TradingForecast forecast = response.Payload;
				IExchangeCode exchange = ExchangeCode.Create(forecast.ExchangeCode);

				ExchangeConfig config = await _exchangeConfigProcessor.GetExchangeConfig(exchange.Code);
				PairConfig pairConfig = config.Pairs.FirstOrDefault(x => x.Symbol.Equals(forecast.Symbol));

				Deal deal = null;
				if (forecast.ForecastCode == TradingAdviceCode.BUY.Code)
				{
					deal = await _autoTradingProcessor.Buy(forecast, pairConfig);
					_logger.LogInformation($"Is BUY: {ExchangeCode.Create(forecast.ExchangeCode).Description} - {forecast.Symbol}");
				}
				else if (forecast.ForecastCode == TradingAdviceCode.SELL.Code)
				{
					deal = await _autoTradingProcessor.Sell(forecast, pairConfig);
					_logger.LogInformation($"Is SELL: {ExchangeCode.Create(forecast.ExchangeCode).Description} - {forecast.Symbol}");
				}
				else
				{
					_logger.LogInformation($"Is HOLD: {ExchangeCode.Create(forecast.ExchangeCode).Description} - {forecast.Symbol}");
				}

				try
				{
					await natsClient.PubAsJsonAsync(_settings.Value.OrdersQueueName, new Notification<Deal>() { Code = ActionCode.CREATED.Code, Payload = deal });
				}
				catch (Exception ex)
				{
					_logger.LogError("Can't send Nata notification", ex);
				}
			}
		}
	}
}
