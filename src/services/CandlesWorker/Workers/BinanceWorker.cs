using Binance.Net;
using Binance.Net.Objects;
using CandlesWorker.Extensions;
using core;
using core.Abstractions.TypeCodes;
using core.Infrastructure.BL;
using core.Infrastructure.Database.Entities;
using core.TypeCodes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using MyNatsClient;
using MyNatsClient.Encodings.Json;
using core.Infrastructure.Notifications;

namespace CandlesWorker.Workers
{
	public class BinanceWorker : Worker
	{
		IOptions<AppSettings> _settings;
		private CandleProcessor _candlesProcessor;
		private ExchangeConfigProcessor _exchangeConfigProcessor;
		private ILogger<Worker> _logger;

		public BinanceWorker (ILogger<Worker> logger, IOptions<AppSettings> settings, CandleProcessor candlesProcessor, ExchangeConfigProcessor exchangeConfigProcessor) : base(logger, exchangeConfigProcessor)
		{
			_settings = settings;
			_logger = logger;
			_candlesProcessor = candlesProcessor;
			_exchangeConfigProcessor = exchangeConfigProcessor;
		}

		public override IExchangeCode Exchange { get; } = ExchangeCode.BINANCE;
		protected override IPeriodCode DefaultCandleInterval { get; } = PeriodCode.MINUTE;

		protected override async Task DoWork (PairConfig pair, CancellationToken stoppingToken)
		{
			_logger.LogInformation($"{Exchange.Description} Candles worker is started");

			IPeriodCode candlePeriod = pair.Timeframe.HasValue ? PeriodCode.Create(pair.Timeframe.Value) : DefaultCandleInterval;

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					using (BinanceSocketClient client = new BinanceSocketClient())
					{
						ConnectionInfo cnInfo = new ConnectionInfo(_settings.Value.BusConnectionString);
						using (NatsClient natsClient = new NatsClient(cnInfo))
						{
							CallResult<UpdateSubscription> successKline = client.SubscribeToKlineStream(pair.Symbol, candlePeriod.ToPeriodCode(), async (data) =>
							{
								await SaveCandle(data.Data, natsClient);
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
					_logger.LogError($"{Exchange.Description} Candles service failed with message: {ex.Message}", ex);
				}
			}
		}

		private async Task SaveCandle (BinanceStreamKline kline, NatsClient natsClient)
		{
			if (kline.Final)
			{
				Candle candle = kline.ToCandle();
				long? id = await _candlesProcessor.Create(candle);

				// TODO: Move to client
				if (!natsClient.IsConnected)
				{
					natsClient.Connect();
				}

				await natsClient.PubAsJsonAsync(_settings.Value.CandlesQueueName, new Notification<Candle>() { Code = ActionCode.CREATED.Code, Payload = candle });
			}
		}
	}
}
