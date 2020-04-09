using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Abstractions.Grains;
using Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyNatsClient;
using MyNatsClient.Rx;
using Orleans;
using Persistence.Entities;

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

						ISubscription subscription = await client.SubAsync(nameof(Candle), stream => stream.SubscribeSafe(msg =>
						{
							string payload = msg.GetPayloadAsString();
							Candle candle = JsonSerializer.Deserialize<Candle>(payload[1..]);
							
							GrainKeyExtension ext = new GrainKeyExtension();
							ext.Exchange = candle.Exchange;
							ext.Asset1 = candle.Asset1;
							ext.Asset2 = candle.Asset2;
							
							ICandleGrain grain = _orleansClient.GetGrain<ICandleGrain>((int)candle.Exchange, ext.ToString());
							grain.Set(candle);
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