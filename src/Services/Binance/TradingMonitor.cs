using System;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using Abstractions.Enums;
using Abstractions.Grains;
using Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;

namespace Binance
{
    public class TradingMonitor : BackgroundService
    {
        private readonly ILogger<TradingMonitor> _logger;
        private readonly IClusterClient _orleansClient;
        private readonly IOrderNotificator _notificator;
        
        public TradingMonitor(
	        ILogger<TradingMonitor> logger,
	        IClusterClient orleansClient,
	        IOrderNotificator notificator)
        {
            _logger = logger;
            _orleansClient = orleansClient;
            _notificator = notificator;
        }

		protected override async Task ExecuteAsync (CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					if (!_orleansClient.IsInitialized)
					{
						await _orleansClient.Connect();
					}

					IOrderProcessingGrain grain = _orleansClient.GetGrain<IOrderProcessingGrain>((int)Exchanges.BINANCE);
					IOrderNotificator obj = await _orleansClient.CreateObjectReference<IOrderNotificator>(_notificator);
					await grain.Subscribe(obj);
					
					// TODO: Move all communications with backend to NATS queue
					// ConnectionInfo cnInfo = new ConnectionInfo(new MyNatsClient.Host("192.168.1.250", 4242));
					// cnInfo.PubFlushMode = PubFlushMode.Auto;
					//
					// using (NatsClient client = new NatsClient(cnInfo))
					// {
					// 	client.Connect();
					//
					// 	while (!stoppingToken.IsCancellationRequested)
					// 	{
					// 		await client.SubAsync("candles", stream => stream.Subscribe(msg =>
					// 		{
					// 			// TODO: Create and call orders management
					// 			// Create Order, add Operation command
					//
					// 			//var context = Contexts[msg.Payload].Pair;
					// 		}));
					// 	}
					//
					// 	client.Disconnect();
					// }


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