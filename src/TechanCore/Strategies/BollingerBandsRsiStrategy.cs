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

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal> currentPrices = candles.Select(x => x.Close).ToList();
			BollingerBandsResult bbands = candles.BollingerBands(options.BollingerPeriod, options.BollingerDeviationUp, options.BollingerDeviationDown);
			decimal?[] rsi = candles.Rsi(options.RsiPeriod).Result;

			for (int i = 0; i < candles.Length; i++)
			{
				if (i == 0)
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (rsi[i] < 30 && currentPrices[i] < bbands.LowerBand[i])
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (rsi[i] > 70)
				{
					result.Add((candles[i], TradingAdvices.SELL));
				}
				else
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
			}

			return result;
		}

		public BollingerBandsRsiStrategy(BollingerBandsRsiStrategyOptions options) : base(options)
		{
		}
	}
}