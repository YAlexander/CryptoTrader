using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;

namespace Core.Trading.Strategies
{
	public class SmaSar : BaseStrategy
	{
		public override string Name { get; } = "SMA SAR";

		public override int MinNumberOfCandles { get; } = 60;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> sma = candles.Sma(60);
			List<decimal?> sar = candles.Sar();
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

					if (closes[i] > sma[i] && fsar == 1)
					{
						result.Add((candles.ElementAt(i), TradingAdvices.BUY));
					}
					else if (closes[i] < sma[i] && fsar == -1)
					{
						result.Add((candles.ElementAt(i), TradingAdvices.SELL));
					}
					else
					{
						result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
					}
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
			}

			return result;
		}
	}
}