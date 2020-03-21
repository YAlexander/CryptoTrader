using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class BullishEngulfingStrategy : BaseStrategy<BullishEngulfingOptions>
	{
		public override string Name { get; } = "Bullish Engulfing Strategy";

		public override int MinNumberOfCandles { get; } = 11;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			BullishEngulfingOptions options = GetOptions;
			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			decimal?[] rsi = candles.Rsi(11).Result;
			decimal[] close = candles.Close();
			decimal[] high = candles.High();
			decimal[] low = candles.Low();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (rsi[i] < 40 && low[i - 1] < close[i] && high[i - 1] < high[i] && high[i - 1] < close[i])
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (high[i - 1] > close[i] && low[i - 1] < low[i] && close[i - 1] < low[i] && rsi[i] > 60)
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

		public BullishEngulfingStrategy(BullishEngulfingOptions options) : base(options)
		{
		}
	}
}
