using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Results;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class BollingerBandsRsiStrategy : BaseStrategy<BollingerBandsRsiStrategyOptions>
	{
		public override string Name { get; } = "Bollinger Bands RSI Strategy";

		public override int MinNumberOfCandles { get; } = 20;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			BollingerBandsRsiStrategyOptions options = GetOptions;
			Validate(candles, default);

			List<decimal> currentPrices = candles.Select(x => x.Close).ToList();
			BollingerBandsResult bbands = candles.BollingerBands(options.BollingerPeriod, options.BollingerDeviationUp, options.BollingerDeviationDown);
			decimal?[] rsi = candles.Rsi(options.RsiPeriod).Result;

			for (int i = 0; i < candles.Length; i++)
			{
				if (i == 0)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (rsi[i] < 30 && currentPrices[i] < bbands.LowerBand[i])
				{
					Result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (rsi[i] > 70)
				{
					Result.Add((candles[i], TradingAdvices.SELL));
				}
				else
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
			}

			return Result;
		}

		public BollingerBandsRsiStrategy(BollingerBandsRsiStrategyOptions options) : base(options)
		{
		}
	}
}