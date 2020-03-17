using System;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using Common;
using Contracts.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;

namespace Binance
{
    public class TradingMonitor : BackgroundService
    {
        private readonly ILogger<TradingMonitor> _logger;
        private readonly IClusterClient _orleansClient;
        private IOrderNotificator _notificator;
        
        public TradingMonitor(
	        ILogger<TradingMonitor> logger,
	        OrleansClient orleansClient,
	        IOrderNotificator notificator)
        {
            _logger = logger;
            _orleansClient = orleansClient.Client;
            _notificator = notificator;
        }

		protected override async Task ExecuteAsync (CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					INotificationGrain grain = _orleansClient.GetGrain<INotificationGrain>((int)Exchanges.BINANCE);

					IOrderNotificator obj = await _orleansClient.CreateObjectReference<IOrderNotificator>(_notificator);
					await grain.Subscribe(obj);

					while (!stoppingToken.IsCancellationRequested)
					{
						await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex.Message, ex);
				}
			}
		}
    }
}