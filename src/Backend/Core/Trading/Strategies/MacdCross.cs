using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class MacdCross : BaseStrategy
	{
		public override string Name { get; } = "MACD X";

		public override int MinNumberOfCandles { get; } = 50;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			MacdCrossOptions options = Options.GetOptions<MacdCrossOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			MacdItem macd = candles.Macd(options?.MacdFast, options?.MacdSlow, options?.MacdSignal);
			for (int i = 0; i < candles.Count(); i++)
			{
				var crossUnder = macd.Macd.Crossunder(macd.Signal);
				var crossOver = macd.Macd.Crossover(macd.Signal);
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (macd.Macd[i] > 0 && crossUnder[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else if (macd.Macd[i] < 0 && crossOver[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
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
