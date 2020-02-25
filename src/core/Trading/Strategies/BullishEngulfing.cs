using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class BullishEngulfing : BaseStrategy
	{
		public override string Name { get; } = "Bullish Engulfing";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 11;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> rsi = candles.Rsi(11);
			List<decimal> close = candles.Select(x => x.Close).ToList();
			List<decimal> high = candles.Select(x => x.High).ToList();
			List<decimal> low = candles.Select(x => x.Low).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				else if (rsi[i] < 40 && low[i - 1] < close[i] && high[i - 1] < high[i] && high[i - 1] < close[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				else if (high[i - 1] > close[i] && low[i - 1] < low[i] && close[i - 1] < low[i] && rsi[i] > 60)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
			}

			return result;
		}

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			IEnumerable<(ICandle candle, ITradingAdviceCode forecast)> forecasts = AllForecasts(candles);
			return forecasts.Last().forecast;
		}
	}
}
