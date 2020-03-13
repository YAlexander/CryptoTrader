using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Binance.Helpers;
using Binance.Net;
using Contracts.Enums;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Persistence;
using Persistence.Entities;

namespace Binance
{
    public class CandlesWorker : BackgroundService
    {
        private readonly ILogger<CandlesWorker> _logger;
        private readonly ISettingsProcessor _exchangeSettingsProcessor;
        private readonly IOptions<Settings> _settings;

        public CandlesWorker(ILogger<CandlesWorker> logger, ISettingsProcessor exchangeSettingsProcessor, IOptions<Settings> settings)
        {
            _logger = logger;
            _exchangeSettingsProcessor = exchangeSettingsProcessor;
            _settings = settings;
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
						CallResult<UpdateSubscription> successKline = client.SubscribeToKlineUpdates(pair.ToPair(), pair.Timeframe.Map(), (data) =>
						{
								if (data.Data.Final)
								{
									// TODO: Add Orleans call
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