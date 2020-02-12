using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class SmaStochRsi : BaseStrategy
	{
		public override string Name { get; } = "SMA Stoch RSI";

		public override int MinNumberOfCandles { get; } = 150;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			SmaStochRsiOptions options = Options.GetOptions<SmaStochRsiOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();
			List<decimal> price = candles.Select(x => x.Close).ToList();

			StochItem stoch = candles.Stoch(options?.Stoch ?? 8);
			List<decimal?> sma = candles.Sma(options?.Sma ?? 150);
			List<decimal?> rsi = candles.Rsi(options?.Rsi ?? 3);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 1)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else
				{
					if (price[i] > sma[i] && stoch.K[i] > 70 && rsi[i] < 20 && stoch.K[i] > stoch.D[i])
					{
						result.Add((candles.ElementAt(i), TradingAdvices.BUY));
					}
					else if (price[i] < sma[i] && stoch.K[i] > 70 && rsi[i] > 80 && stoch.K[i] < stoch.D[i])
					{
						result.Add((candles.ElementAt(i), TradingAdvices.SELL));
					}
					else
					{
						result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
					}
				}
			}

			return result;
		}
	}
}

