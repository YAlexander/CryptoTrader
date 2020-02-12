using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class RsiMacdMfi : BaseStrategy
	{
		public override string Name { get; } = "RSI MACD MFI";
		
		public override int MinNumberOfCandles { get; } = 35;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			RsiMacdMfiOptions options = Options.GetOptions<RsiMacdMfiOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			MacdItem macd = candles.Macd(options?.MacdFast ?? 5, options?.MacdSlow ?? 10, options?.MacdSignal ?? 4);
			List<decimal?> rsi = candles.Rsi(options?.Rsi ?? 16);
			List<decimal?> mfi = candles.Mfi(options?.Mfi);
			List<decimal?> ao = candles.AwesomeOscillator();

			List<decimal> close = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (mfi[i] < 30 && rsi[i] < 45 && ao[i] > 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (mfi[i] > 30 && rsi[i] > 45 && ao[i] < 0)
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