using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Helpers;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class QuickSmaStrategy : BaseStrategy<QuickSmaStrategyOptions>
	{
		public override string Name { get; } = "Quick SMA Strategy";
		
		public override int MinNumberOfCandles { get; } = 20;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{			
			QuickSmaStrategyOptions options = GetOptions;
			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			decimal?[] smaFast = candles.Sma(options.FastSmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] smaSlow = candles.Sma(options.SlowSmaPeriod, CandleVariables.CLOSE).Result;

			decimal?[] closes = candles.Close();
			bool[] crossOver = smaFast.Crossover(smaSlow).ToArray();
			bool[] crossUnder = smaSlow.Crossunder(closes).ToArray();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (crossOver[i])
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (crossUnder[i])
				{
					result.Add((candles[i], TradingAdvices.SELL));
				}
				else
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
			}

			return result;
		}

		public QuickSmaStrategy(QuickSmaStrategyOptions options) : base(options)
		{
		}
	}
}
