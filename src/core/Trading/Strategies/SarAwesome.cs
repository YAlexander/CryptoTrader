using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.TypeCodes;
using Core.Trading.Extensions;

namespace core.Trading.Strategies
{
	public class SarAwesome : BaseStrategy
	{
		public override string Name { get; } = "SAR Awesome";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 35;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal?> sar = candles.Sar();
			List<decimal?> ema5 = candles.Ema(5);
			List<decimal?> ao = candles.AwesomeOscillator();

			List<decimal> close = candles.Select(x => x.Close).ToList();
			List<decimal> highs = candles.Select(x => x.High).ToList();
			List<decimal> lows = candles.Select(x => x.Low).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i >= 2)
				{
					decimal? currentSar = sar[i];
					decimal? priorSar = sar[i - 1];
					decimal? earlierSar = sar[i - 2];
					decimal lastHigh = highs[i];
					decimal lastLow = lows[i];

					if (currentSar > lastHigh && priorSar > lastHigh && earlierSar > lastHigh && ao[i] > 0 && ema5[i] < close[i])
					{
						result.Add(TradingAdviceCode.BUY);
					}
					else if (currentSar < lastLow && priorSar < lastLow && earlierSar < lastLow && ao[i] < 0 && ema5[i] > close[i])
					{
						result.Add(TradingAdviceCode.SELL);
					}
					else
					{
						result.Add(TradingAdviceCode.HOLD);
					}
				}
				else
				{
					result.Add(TradingAdviceCode.HOLD);
				}
			}

			return result.LastOrDefault();
		}
	}
}
