using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Abstractions.Grains;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyNatsClient;
using MyNatsClient.Rx;
using Orleans;
using Persistence.Entities;

namespace Silo
{
	public class TradesMonitor : BackgroundService
	{
		private readonly IOptions<AppSettings> _settings;
		private readonly ILogger<TradesMonitor> _logger;
		private readonly IClusterClient _orleansClient;

		public TradesMonitor(IOptions<AppSettings> settings, ILogger<TradesMonitor> logger, IClusterClient orleansClient)
		{
			_settings = settings;
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
					
					using (NatsClient client = new NatsClient(new ConnectionInfo(_settings.Value.NatsHost, _settings.Value.NatsPort)))
					{
						await client.ConnectAsync();

						ISubscription subscription = await client.SubAsync(nameof(Trade), stream => stream.SubscribeSafe(msg =>
						{
							string payload = msg.GetPayloadAsString();
							Trade trade = JsonSerializer.Deserialize<Trade>(payload[1..]);
							
							ITradeProcessingGrain grain = _orleansClient.GetGrain<ITradeProcessingGrain>((long)trade.Exchange);
							grain.Process(trade);
						}));

						while (!stoppingToken.IsCancellationRequested)
						{
							await Task.Delay(_settings.Value.DefaultTimeoutSeconds, stoppingToken);
						}

						await client.UnsubAsync(subscription);
						client.Disconnect();
					}
				}
				catch (Exception ex)
				{
					_logger.LogError($"Worker failed: {ex.Message}", ex);
				}
			}
		}
	}
}