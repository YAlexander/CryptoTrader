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
	public class EmaAdxMacdStrategy : BaseStrategy<EmaAdxMacdOptions>
	{
		public override string Name { get; } = "EMA ADX MACD";
		
		public override int MinNumberOfCandles { get; } = 30;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			EmaAdxMacdOptions options = GetOptions;
			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			decimal?[] emaFast = candles.Ema(options.FastEmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] emaSlow = candles.Ema(options.SlowEmaPeriod, CandleVariables.CLOSE).Result;
			AdxResult adx = candles.Adx(options.AdxEmaPeriod);
			
			MacdIndicatorResult macd = candles.Macd(options.MacdFastPeriod, options.MacdSlowPeriod, options.MacdSignalPeriod);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (emaFast[i] < emaSlow[i] && emaFast[i - 1] > emaSlow[i] && macd.Macd[i] < 0 && adx.PlusDi[i] > adx.MinusDi[i])
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (emaFast[i] > emaSlow[i] && emaFast[i - 1] < emaSlow[i] && macd.Macd[i] > 0 && adx.PlusDi[i] < adx.MinusDi[i])
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

		public EmaAdxMacdStrategy(EmaAdxMacdOptions options) : base(options)
		{
		}
	}
}
