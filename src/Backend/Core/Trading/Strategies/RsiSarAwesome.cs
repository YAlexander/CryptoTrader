using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class RsiSarAwesome : BaseStrategy
	{
		public override string Name { get; } = "RSI SAR Awesome";
		
		public override int MinNumberOfCandles { get; } = 35;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			RsiSarAwesomeOptions options = Options.GetOptions<RsiSarAwesomeOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> sar = candles.Sar();
			List<decimal?> rsi = candles.Rsi(options?.Rsi ?? 5);
			List<decimal?> ao = candles.AwesomeOscillator();

			List<decimal> close = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i >= 2)
				{
					decimal? currentSar = sar[i];
					decimal? priorSar = sar[i - 1];

					if (currentSar < close[i] && priorSar > close[i] && ao[i] > 0 && rsi[i] > 50)
					{
						result.Add((candles.ElementAt(i), TradingAdvices.BUY));
					}
					else if (currentSar > close[i] && priorSar < close[i] && ao[i] < 0 && rsi[i] < 50)
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
