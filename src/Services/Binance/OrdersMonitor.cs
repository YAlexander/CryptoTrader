using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Abstractions.Enums;
using Binance.Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Persistence;
using Persistence.Entities;

namespace Binance
{
	public class OrdersMonitor : BackgroundService
	{
		private readonly ILogger<TradesWorker> _logger;
		private readonly ISettingsProcessor _exchangeSettingsProcessor;
		private readonly IOptions<AppSettings> _settings;
		private readonly IClusterClient _orleansClient;

		public OrdersMonitor(
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
		
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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
					using (var client = new BinanceSocketClient())
					{
						// TODO: and process Get order updates
						
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