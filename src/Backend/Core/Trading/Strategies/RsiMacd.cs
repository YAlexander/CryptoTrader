using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class RsiMacd : BaseStrategy
	{
		public override string Name { get; } = "RSI MACD";
		
		public override int MinNumberOfCandles { get; } = 52;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			RsiMacdOptions options = Options.GetOptions<RsiMacdOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			MacdItem macd = candles.Macd(options?.MacdFast ?? 12, options?.MacdSlow ?? 26, options?.MacdSignal ?? 9);
			List<decimal?> rsi = candles.Rsi(options?.Rsi ?? 14);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (rsi[i] > 70 && macd.Macd[i] - macd.Signal[i] < 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else if (rsi[i] < 30 && macd.Macd[i] - macd.Signal[i] > 0)
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
