using Binance.Net;
using Binance.Net.Objects;
using CandlesWorker.Extensions;
using core;
using core.Abstractions.TypeCodes;
using core.Infrastructure.BL;
using core.Infrastructure.Database.Entities;
using core.TypeCodes;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CandlesWorker.Workers
{
	public class BinanceCandlesMonitor : Worker
	{
		private ILogger<BinanceCandlesMonitor> _logger;
		private CandleProcessor _candleProcessor;

		public BinanceCandlesMonitor (
			IOptions<AppSettings> settings,
			ILogger<BinanceCandlesMonitor> logger,
			ExchangeConfigProcessor exchangeConfigProcessor,
			CandleProcessor candleProcessor
			) : base(logger, exchangeConfigProcessor)
		{
			_logger = logger;
			_candleProcessor = candleProcessor;
		}

		public override IExchangeCode Exchange { get; } = ExchangeCode.BINANCE;
		protected override IPeriodCode DefaultCandleInterval { get; } = PeriodCode.HOUR;

		protected override async Task DoWork (PairConfig config, CancellationToken stoppingToken)
		{
			_logger.LogInformation($"{Exchange.Description} Candles monitor is started");

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					using (BinanceClient client = new BinanceClient())
					{
						DateTime now = DateTime.UtcNow;
						IPeriodCode candlePeriod = config.Timeframe.HasValue ? PeriodCode.Create(config.Timeframe.Value) : DefaultCandleInterval;

						WebCallResult<BinanceKline[]> klines = client.GetKlines(config.Symbol, candlePeriod.ToPeriodCode(), now.AddDays(-1), now, 500);

						List<Candle> candles = klines.Data.Select(x => x.ToCandle(config.Symbol, candlePeriod)).ToList();
						foreach (Candle candle in candles)
						{
							await _candleProcessor.Create(candle);
						}
					}

					await Task.Delay(TimeSpan.FromHours(1));
				}
				catch (Exception ex)
				{
					_logger.LogError($"{Exchange.Description} Candles service failed with message: {ex.Message}", ex);
				}
			}
		}
	}
}
