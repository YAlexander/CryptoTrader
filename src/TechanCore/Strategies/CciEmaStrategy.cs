using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class CciEmaStrategy : BaseStrategy<CciEmaStrategyOptions>
	{
		public override string Name { get; } = "CCI EMA Strategy";

		public override int MinNumberOfCandles { get; } = 30;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			CciEmaStrategyOptions options = GetOptions;
			Validate(candles, options);

			decimal?[] cci = candles.Cci(options.CciPeriod).Result;
			decimal?[] emaFast = candles.Ema(options.EmaFast, CandleVariables.CLOSE).Result;
			decimal?[] emaSlow = candles.Ema(options.EmaSlow, CandleVariables.CLOSE).Result;

			for (int i = 0; i < candles.Length; i++)
			{
				if (i == 0)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (cci[i] < -100 && emaFast[i] > emaSlow[i] && emaFast[i - 1] < emaSlow[i])
				{
					Result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (cci[i] > 100 && emaFast[i] < emaSlow[i] && emaFast[i - 1] > emaSlow[i])
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

		public CciEmaStrategy(CciEmaStrategyOptions options) : base(options)
		{
		}
	}
}
