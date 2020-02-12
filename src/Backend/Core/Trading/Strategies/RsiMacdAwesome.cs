using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class RsiMacdAwesome : BaseStrategy
	{
		public override string Name { get; } = "RSI MACD Awesome";

		public override int MinNumberOfCandles { get; } = 35;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			RsiMacdAwesomeOptions options = Options.GetOptions<RsiMacdAwesomeOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			MacdItem macd = candles.Macd(options?.MacdFast ?? 5, options?.MacdSlow ?? 10, options?.MacdSignal ?? 4);
			List<decimal?> rsi = candles.Rsi(options?.Rsi ?? 16);
			List<decimal?> ao = candles.AwesomeOscillator();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (macd.Hist[i] < 0 && ao[i] > 0 && rsi[i] < 45)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (macd.Hist[i] > 0 && ao[i] < 0 && rsi[i] > 45)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
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