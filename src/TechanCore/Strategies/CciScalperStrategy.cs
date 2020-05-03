using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class CciScalperStrategy : BaseStrategy<CciScalperStrategyOptions>
	{
		public override string Name { get; } = "CCI Scalper";

		public override int MinNumberOfCandles { get; } = 14;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			CciScalperStrategyOptions options = GetOptions;
			Validate(candles, options);

			decimal?[] cci = candles.Cci(options.CciPeriod).Result;
			decimal?[] emaFast = candles.Ema(options.FastEmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] emaNormal = candles.Ema(options.NormalEmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] emaSlow = candles.Ema(options.SlowEmaPeriod, CandleVariables.CLOSE).Result;

			for (int i = 0; i < candles.Count(); i++)
			{
				if (cci[i] < -100 && emaFast[i] > emaNormal[i] && emaFast[i] > emaSlow[i])
				{
					Result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (cci[i] > 100 && emaFast[i] < emaNormal[i] && emaFast[i] < emaSlow[i])
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

		public CciScalperStrategy(CciScalperStrategyOptions options) : base(options)
		{
		}
	}
}
