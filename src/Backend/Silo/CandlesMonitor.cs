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
using Persistence.Helpers;
using TechanCore;

namespace Silo
{
	public class CandlesMonitor : BackgroundService
	{
		private IOptions<AppSettings> _settings;
		private readonly ILogger<CandlesMonitor> _logger;
		private readonly OrleansClient _orleansClient;

		public CandlesMonitor(IOptions<AppSettings> settings, ILogger<CandlesMonitor> logger, OrleansClient orleansClient)
		{
			_settings = settings;
			_logger = logger;
			_orleansClient = orleansClient;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (IClusterClient orleansClient = _orleansClient.Client)
				{
					try
					{
						using (NatsClient client = new NatsClient(new ConnectionInfo(_settings.Value.NatsHost, _settings.Value.NatsPort)))
						{
							await client.ConnectAsync();

							ISubscription subscription = await client.SubAsync(nameof(Candle), stream => stream.SubscribeSafe(async msg =>
							{
								string payload = msg.GetPayloadAsString();
								Candle candle = JsonSerializer.Deserialize<Candle>(payload[1..]);

								GrainKeyExtension key = candle.ToExtendedKey();

								ICandleGrain grain = orleansClient.GetGrain<ICandleGrain>((long)key.Exchange, key.ToString());
								await grain.Set((ICandle)candle);
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
}