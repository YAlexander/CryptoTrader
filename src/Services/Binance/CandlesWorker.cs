using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Abstractions.Enums;
using Binance.Helpers;
using Binance.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyNatsClient;
using MyNatsClient.Encodings.Json;
using Persistence;
using Persistence.Entities;

namespace Binance
{
    public class CandlesWorker : BackgroundService
    {
        private readonly ILogger<CandlesWorker> _logger;
        private readonly ISettingsProcessor _exchangeSettingsProcessor;
        private readonly IOptions<AppSettings> _settings;

        public CandlesWorker(
	        ILogger<CandlesWorker> logger,
	        ISettingsProcessor exchangeSettingsProcessor,
	        IOptions<AppSettings> settings
	        )
        {
            _logger = logger;
            _exchangeSettingsProcessor = exchangeSettingsProcessor;
            _settings = settings;
        }

		protected override async Task ExecuteAsync (CancellationToken stoppingToken)
		{
			IEnumerable<ExchangeSettings> exchangeSettings = await _exchangeSettingsProcessor.Get(Exchanges.BINANCE);
			
			List<Task> tasks = new List<Task>();
			foreach (ExchangeSettings pair in exchangeSettings)
			{
				if (pair.IsEnabled)
				{
					Task t = Task.Run(() => DoWork(pair, stoppingToken), stoppingToken);
					tasks.Add(t);
				}
			}

			await Task.WhenAll(tasks.ToArray());
		}

		private async Task DoWork (ExchangeSettings pair, CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					using (BinanceSocketClient client = new BinanceSocketClient())
					{
						CallResult<UpdateSubscription> successKline = client.SubscribeToKlineUpdates(pair.ToPair(), pair.Timeframe.Map(), async (data) =>
						{ 
							if (data.Data.Final)
							{
								using NatsClient natsClient = new NatsClient(new ConnectionInfo(_settings.Value.NatsHost, _settings.Value.NatsPort));
								{
									await natsClient.ConnectAsync();

									CandleEntity candle = data.Data.Map(pair);
									await natsClient.PubAsJsonAsync(nameof(CandleEntity), candle);
									_logger.LogTrace($"Received candle {candle.Exchange}, {candle.Asset1}/{candle.Asset2}, {candle.TimeFrame}");

									natsClient.Disconnect();
								}
							}
						});
							
						if (!successKline.Success)
						{
							_logger.LogError($"{successKline.Error.Message} for {pair.Exchange} {pair.Asset1}{pair.Asset2}" );
						}

						while (!stoppingToken.IsCancellationRequested)
						{
							await Task.Delay(TimeSpan.FromSeconds(pair.UpdatingInterval), stoppingToken);
						}
					}
				}
				catch(Exception ex)
				{
					_logger.LogError($"{ex.Message} for {pair.Exchange} {pair.Asset1}{pair.Asset2}", ex);
				}
			}
		}
    }
}