using Binance.Net;
using CandlesWorker.Extensions;
using core;
using core.Abstractions.TypeCodes;
using core.Infrastructure.BL;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Notifications;
using core.TypeCodes;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyNatsClient;
using MyNatsClient.Encodings.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TradesWorker.Workers
{
	public class BinanceWorker : Worker
	{
		private IOptions<AppSettings> _settings;
		private ILogger<Worker> _logger;
		private TradesProcessor _tradesProcessor;

		public BinanceWorker (IOptions<AppSettings> settings, ILogger<Worker> logger, ExchangeConfigProcessor exchangeConfigProcessor, TradesProcessor tradesProcessor) : base(logger, exchangeConfigProcessor)
		{
			_logger = logger;
			_settings = settings;
			_tradesProcessor = tradesProcessor;
		}

		public override IExchangeCode Exchange { get; set; } = ExchangeCode.BINANCE;

		protected override async Task DoWork (PairConfig pair, CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					using (BinanceSocketClient client = new BinanceSocketClient())
					{
						ConnectionInfo cnInfo = new ConnectionInfo(_settings.Value.BusConnectionString);
						using (NatsClient natsClient = new NatsClient(cnInfo))
						{
							if (!natsClient.IsConnected)
							{
								natsClient.Connect();
							}

							CallResult<UpdateSubscription> successKline = null;
							successKline = client.SubscribeToTradesStream(pair.Symbol, async (data) =>
							{
								Trade trade = data.ToEntity();

								if (!_settings.Value.DisadleDealsSaving)
								{
									long id = await _tradesProcessor.Create(trade);
								}

								await natsClient.PubAsJsonAsync(_settings.Value.TradesQueueName, new Notification<Trade>() { Code = ActionCode.CREATED.Code, Payload = trade });
							});

							successKline.Data.ConnectionLost += () => { _logger.LogError($"Connection to {Exchange} is lost"); };
							successKline.Data.ConnectionRestored += (data) => { _logger.LogError($"Connection to {Exchange} is Restored"); };

							while (!stoppingToken.IsCancellationRequested)
							{
							}

							natsClient.Disconnect();
							await client.Unsubscribe(successKline.Data);
						}
					}
				}
				catch (Exception ex)
				{
					_logger.LogError($"{Exchange.Description} Trades service failed with message {ex.Message}", ex);
				}
			}
		}
	}
}
