using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class BbandRsi : BaseStrategy
	{
		public override string Name { get; } = "BBand RSI";

		public override int MinNumberOfCandles { get; } = 20;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			RsiBbandsOptions options = Options.GetOptions<RsiBbandsOptions>();

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal> currentPrices = candles.Select(x => x.Close).ToList();
			BbandItem bbands = candles.Bbands(options?.Bbands ?? 20);
			List<decimal?> rsi = candles.Rsi(options?.Rsi ?? 16);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (rsi[i] < 30 && currentPrices[i] < bbands.LowerBand[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (rsi[i] > 70)
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