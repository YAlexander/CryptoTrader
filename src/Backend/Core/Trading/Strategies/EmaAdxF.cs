using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class EmaAdxF : BaseStrategy
	{
		public override string Name { get; } = "EMA ADX F";

		public override int MinNumberOfCandles { get; } = 15;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			EmaAdxFOptions options = Options.GetOptions<EmaAdxFOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal> closes = candles.Select(x => x.Close).ToList();
			List<decimal?> ema = candles.Ema(options?.Ema ?? 9);
			List<decimal?> adx = candles.Adx(options?.Adx ?? 14);
			List<decimal?> minusDi = candles.MinusDi(options?.MinusDi ?? 14);
			List<decimal?> plusDi = candles.PlusDi(options?.PlusDi ?? 14);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (ema[i] < closes[i] && plusDi[i] > 20 && plusDi[i] > minusDi[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (ema[i] > closes[i] && minusDi[i] > 20 && plusDi[i] < minusDi[i])
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
