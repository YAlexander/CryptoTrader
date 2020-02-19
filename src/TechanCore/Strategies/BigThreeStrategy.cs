using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class BigThreeStrategy : BaseStrategy<BigTreeStrategyOptions>
	{
		public override string Name { get; } = "Big Three Strategy";

		public override int MinNumberOfCandles { get; } = 100;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			BigTreeStrategyOptions options = GetOptions;
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			decimal?[] sma20 = candles.Sma(options.VeryFastSmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] sma40 = candles.Sma(options.FastSmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] sma80 = candles.Sma(options.SlowSmaPeriod, CandleVariables.CLOSE).Result;

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 2)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else
				{
					ICandle[] candlesArray = candles.ToArray();

					bool lastIsGreen = candlesArray[i].Close > candlesArray[i].Open;
					bool previousIsRed = candlesArray[i - 1].Close < candlesArray[i - 1].Open;
					bool beforeIsGreen = candlesArray[i - 2].Close > candlesArray[i - 2].Open;

					decimal? highestSma = new List<decimal?> { sma20[i], sma40[i], sma80[i] }.Max();

					bool lastAboveSma = candlesArray[i].Close > highestSma && candlesArray[i].High > highestSma && candlesArray[i].Low > highestSma && candlesArray[i].Open > highestSma;

					bool previousAboveSma = candlesArray[i - 1].Close > highestSma && candlesArray[i - 1].High > highestSma && candlesArray[i - 1].Low > highestSma && candlesArray[i - 1].Open > highestSma;

					bool beforeAboveSma = candlesArray[i - 2].Close > highestSma && candlesArray[i - 2].High > highestSma && candlesArray[i - 2].Low > highestSma && candlesArray[i - 2].Open > highestSma;

					bool allAboveSma = lastAboveSma && previousAboveSma && beforeAboveSma;
					bool hitsAnSma = sma80[i] < candlesArray[i].High && sma80[i] > candlesArray[i].Low;

					if (lastIsGreen && previousIsRed && beforeIsGreen && allAboveSma && sma20[i] > sma40[i] && sma20[i] > sma80[i])
					{
						result.Add((candles[i], TradingAdvices.BUY));
					}
					else if (hitsAnSma)
					{
						result.Add((candles[i], TradingAdvices.SELL));
					}
					else
					{
						result.Add((candles[i], TradingAdvices.HOLD));
					}
				}
			}

			return result;
		}

		public BigThreeStrategy(BigTreeStrategyOptions options) : base(options)
		{
		}
	}
}
