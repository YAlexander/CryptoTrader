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
    public class TradesWorker : BackgroundService
    {
        private readonly ILogger<TradesWorker> _logger;
        private readonly ISettingsProcessor _exchangeSettingsProcessor;
        private readonly IOptions<AppSettings> _settings;
        private readonly IClusterClient _orleansClient;

        public TradesWorker(
	        ILogger<TradesWorker> logger,
	        ISettingsProcessor exchangeSettingsProcessor,
	        IOptions<AppSettings> settings,
	        IClusterClient orleansClient)
        {
            _logger = logger;
            _exchangeSettingsProcessor = exchangeSettingsProcessor;
            _settings = settings;
            _orleansClient = orleansClient;
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
						CallResult<UpdateSubscription> successTrades  = client.SubscribeToTradeUpdates(pair.ToPair(), async (data) =>
						{
							GrainKeyExtension keyExtension = new GrainKeyExtension();
							keyExtension.Asset1 = pair.Asset1;
							keyExtension.Asset2 = pair.Asset2;

							Trade trade = data.Map(pair);
							ITradeProcessingGrain grain = _orleansClient.GetGrain<ITradeProcessingGrain>((long)pair.Exchange, keyExtension.ToString());
							await grain.Set(trade);						
						});

						if (!successTrades .Success)
						{
							_logger.LogError($"{successTrades .Error.Message} for {pair.Exchange} {pair.Asset1}{pair.Asset2}" );
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