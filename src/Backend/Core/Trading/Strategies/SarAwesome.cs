using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;

namespace Core.Trading.Strategies
{
	public class SarAwesome : BaseStrategy
	{
		public override string Name { get; } = "SAR Awesome";
		
		public override int MinNumberOfCandles { get; } = 35;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> sar = candles.Sar();
			List<decimal?> ema = candles.Ema(5);
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

					if (currentSar > lastHigh && priorSar > lastHigh && earlierSar > lastHigh && ao[i] > 0 && ema[i] < close[i])
					{
						result.Add((candles.ElementAt(i), TradingAdvices.BUY));
					}
					else if (currentSar < lastLow && priorSar < lastLow && earlierSar < lastLow && ao[i] < 0 && ema[i] > close[i])
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
