using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using Abstractions.Enums;
using Binance.Helpers;
using Binance.Net;
using Common;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Persistence;
using Persistence.Entities;

namespace Binance
{
    public class CandlesWorker : BackgroundService
    {
        private readonly ILogger<CandlesWorker> _logger;
        private readonly ISettingsProcessor _exchangeSettingsProcessor;
        private readonly IOptions<Settings> _settings;
        private readonly IClusterClient _orleansClient;

        public CandlesWorker(
	        ILogger<CandlesWorker> logger,
	        ISettingsProcessor exchangeSettingsProcessor,
	        IOptions<Settings> settings,
	        OrleansClient orleansClient)
        {
            _logger = logger;
            _exchangeSettingsProcessor = exchangeSettingsProcessor;
            _settings = settings;
            _orleansClient = orleansClient.Client;
        }

		protected override async Task ExecuteAsync (CancellationToken stoppingToken)
		{
			IEnumerable<IExchangeSettings> exchangeSettings = await _exchangeSettingsProcessor.Get(Exchanges.BINANCE);
			
			List<Task> tasks = new List<Task>();
			foreach (IExchangeSettings pair in exchangeSettings)
			{
				if (pair.IsEnabled)
				{
					Task t = Task.Run(() => DoWork(pair, stoppingToken), stoppingToken);
					tasks.Add(t);
				}
			}

			await Task.WhenAll(tasks.ToArray());
		}

		private async Task DoWork (IExchangeSettings pair, CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					using (var client = new BinanceSocketClient())
					{
						CallResult<UpdateSubscription> successKline = client.SubscribeToKlineUpdates(pair.ToPair(), pair.Timeframe.Map(), async (data) =>
						{
								if (data.Data.Final)
								{
									Candle candle = data.Data.Map(pair);
									
									GrainKeyExtension keyExtension = new GrainKeyExtension();
									keyExtension.Asset1 = candle.Asset1;
									keyExtension.Asset2 = candle.Asset2;
									keyExtension.Time = candle.Time;
									
									ICandleGrain grain = _orleansClient.GetGrain<ICandleGrain>((long)pair.Exchange, keyExtension.ToString());
									await grain.Set(candle);
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