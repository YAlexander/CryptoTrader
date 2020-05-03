using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class SmaGoldenCrossStrategy : BaseStrategy<EmptyStrategyOptions>
	{
		public override string Name { get; } = "SMA 50/200 Golden Cross Strategy";

		public override int MinNumberOfCandles { get; } = 200;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			Validate(candles, null);

			decimal?[] sma50 = candles.Sma(50, CandleVariables.CLOSE).Result;
			decimal?[] sma200 = candles.Sma(200, CandleVariables.CLOSE).Result;
			bool[] crossUnder = sma50.Crossunder(sma200).ToArray();
			bool[] crossOver = sma50.Crossover(sma200).ToArray();

			for (int i = 0; i < candles.Length; i++)
			{
				if (i == 0)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (crossUnder[i])
				{
					Result.Add((candles[i], TradingAdvices.SELL));
				}
				else if (crossOver[i])
				{
					Result.Add((candles[i], TradingAdvices.BUY));
				}
				else
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
			}

			return Result;
		}

		public SmaGoldenCrossStrategy(EmptyStrategyOptions options) : base(options)
		{
		}
	}
}