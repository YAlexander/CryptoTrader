using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
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

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, null);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			decimal?[] sma50 = candles.Sma(50, CandleVariables.CLOSE).Result;
			decimal?[] sma200 = candles.Sma(200, CandleVariables.CLOSE).Result;
			bool[] crossUnder = sma50.Crossunder(sma200).ToArray();
			bool[] crossOver = sma50.Crossover(sma200).ToArray();

			for (int i = 0; i < candles.Length; i++)
			{
				if (i == 0)
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (crossUnder[i])
				{
					result.Add((candles[i], TradingAdvices.SELL));
				}
				else if (crossOver[i])
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
			}

			return result;
		}

		public SmaGoldenCrossStrategy(EmptyStrategyOptions options) : base(options)
		{
		}
	}
}