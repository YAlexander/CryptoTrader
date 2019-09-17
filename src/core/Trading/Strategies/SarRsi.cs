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
	public class SarRsi : BaseStrategy
	{
		public override string Name { get; } = "SAR RSI";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 15;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal?> sar = candles.Sar();
			List<decimal?> rsi = candles.Rsi();

			List<decimal> highs = candles.Select(x => x.High).ToList();
			List<decimal> lows = candles.Select(x => x.Low).ToList();
			List<decimal> closes = candles.Select(x => x.Close).ToList();
			List<decimal> opens = candles.Select(x => x.Open).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i > 2)
				{
					decimal? currentSar = sar[i];
					decimal? priorSar = sar[i - 1];
					decimal lastHigh = highs[i];
					decimal lastLow = lows[i];
					decimal lastOpen = opens[i];
					decimal lastClose = closes[i];
					decimal priorHigh = highs[i - 1];
					decimal priorLow = lows[i - 1];
					decimal priorOpen = opens[i - 1];
					decimal priorClose = closes[i - 1];
					decimal prevOpen = opens[i - 2];
					decimal prevClose = closes[i - 2];

					bool below = currentSar < lastLow;
					bool above = currentSar > lastHigh;
					bool redCandle = lastOpen < lastClose;
					bool greenCandle = lastOpen > lastClose;
					bool priorBelow = priorSar < priorLow;
					bool priorAbove = priorSar > priorHigh;
					bool priorRedCandle = priorOpen < priorClose;
					bool priorGreenCandle = priorOpen > priorClose;
					bool prevRedCandle = prevOpen < prevClose;
					bool prevGreenCandle = prevOpen > prevClose;

					priorRedCandle = prevRedCandle || priorRedCandle;
					priorGreenCandle = prevGreenCandle || priorGreenCandle;

					int fsar = 0;

					if (priorAbove && priorRedCandle && below && greenCandle)
					{
						fsar = 1;
					}
					else if (priorBelow && priorGreenCandle && above && redCandle)
					{
						fsar = -1;
					}

					if (rsi[i] > 70 && fsar == -1)
					{
						result.Add(TradingAdviceCode.SELL);
					}
					else if (rsi[i] < 30 && fsar == 1)
					{
						result.Add(TradingAdviceCode.BUY);
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