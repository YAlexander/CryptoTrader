using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Results;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class MacdTemaStrategy : BaseStrategy<MacdTemaStrategyOptions>
	{
		public override string Name { get; } = "MACD TEMA Strategy";
		
		public override int MinNumberOfCandles { get; } = 50;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			MacdTemaStrategyOptions options = GetOptions;
			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();
			MacdIndicatorResult macd = candles.Macd(options.MacdFastPeriod, options.MacdSlowPeriod, options.MacdSignalPeriod);
			decimal?[] tema = candles.Tema(options.TemaPeriod, CandleVariables.CLOSE).Result;

			List<decimal> close = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Length; i++)
			{
				if (i == 0)
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (tema[i] < close[i] && tema[i - 1] > close[i - 1] && macd.Macd[i] > 0 && macd.Macd[i - 1] < 0)
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (tema[i] > close[i] && tema[i - 1] < close[i - 1] && macd.Macd[i] < 0 && macd.Macd[i - 1] > 0)
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

		public MacdTemaStrategy(MacdTemaStrategyOptions options) : base(options)
		{
		}
	}
}