using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class CciRsiStrategy : BaseStrategy<CciRsiOptions>
	{
		public override string Name { get; } = "CCI RSI";

		public override int MinNumberOfCandles { get; } = 15;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			CciRsiOptions options = GetOptions;
			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			decimal?[] cci = candles.Cci(options.CciPeriod).Result;
			decimal?[] rsi = candles.Rsi(options.RsiPeriod).Result;

			for (int i = 0; i < candles.Length; i++)
			{
				if (i == 0)
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (rsi[i] < 30 && cci[i] < -100)
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (rsi[i] > 70 && cci[i] > 100)
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

		public CciRsiStrategy(CciRsiOptions options) : base(options)
		{
		}
	}
}
