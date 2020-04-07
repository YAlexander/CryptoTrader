using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyNatsClient;
using MyNatsClient.Rx;
using Orleans;

namespace Silo
{
	public class CandlesMonitor : BackgroundService
	{
		private readonly ILogger<CandlesMonitor> _logger;
		private readonly IClusterClient _orleansClient;

		public CandlesMonitor(ILogger<CandlesMonitor> logger, IClusterClient orleansClient)
		{
			_logger = logger;
			_orleansClient = orleansClient;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					if (!_orleansClient.IsInitialized)
					{
						await _orleansClient.Connect();
					}

					using (NatsClient client = new NatsClient(new MyNatsClient.ConnectionInfo("", 222)))
					{
						await client.ConnectAsync();

						 ISubscription subscription = await client.SubAsync("trades", stream => stream.Subscribe(msg =>
						{
							// TODO: Call Candle Grain
						}));

						while (!stoppingToken.IsCancellationRequested)
						{
							await Task.Delay(1000, stoppingToken);
						}

						await client.UnsubAsync(subscription);
						client.Disconnect();
					}
				}
				catch (Exception ex)
				{
					_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
				}
			}
		}
	}
}