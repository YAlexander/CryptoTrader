using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Abstractions.Grains;
using Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyNatsClient;
using MyNatsClient.Rx;
using Orleans;
using Persistence.Entities;

namespace Silo
{
	public class CandlesMonitor : BackgroundService
	{
		private IOptions<AppSettings> _settings;
		private readonly ILogger<CandlesMonitor> _logger;
		private readonly IClusterClient _orleansClient;

		public CandlesMonitor(IOptions<AppSettings> settings, ILogger<CandlesMonitor> logger, IClusterClient orleansClient)
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