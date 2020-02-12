using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class TripleMa : BaseStrategy
	{
		public override string Name { get; } = "Triple MA";

		public override int MinNumberOfCandles { get; } = 50;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			TripleMaOptions options = Options.GetOptions<TripleMaOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> sma1 = candles.Sma(options?.Sma1 ?? 20);
			List<decimal?> sma2 = candles.Sma(options?.Sma2 ?? 50);
			List<decimal?> ema = candles.Ema(options?.Ema ?? 11);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (ema[i] > sma2[i] && ema[i - 1] < sma2[i - 1])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY)); // A cross of the EMA and long SMA is a buy signal.
				}
				else if (ema[i] < sma2[i] && ema[i - 1] > sma2[i - 1] || ema[i] < sma1[i] && ema[i - 1] > sma1[i - 1])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL)); // As soon as our EMA crosses below an SMA its a sell signal.
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
