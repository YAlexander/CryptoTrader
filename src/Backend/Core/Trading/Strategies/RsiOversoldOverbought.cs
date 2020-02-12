using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class RsiOversoldOverbought : BaseStrategy
	{
		public override string Name { get; } = "RSI Oversold/Overbought";
		
		public override int MinNumberOfCandles { get; } = 200;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			RsiOversoldOverboughtOptions options = Options.GetOptions<RsiOversoldOverboughtOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> rsi = candles.Rsi(options?.Rsi ?? 14);
			List<bool> crossOver = rsi.Crossover(options?.Low ?? 30);
			List<bool> crossUnder = rsi.Crossunder(options?.High ?? 70);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (crossUnder[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else if (crossOver[i])
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
