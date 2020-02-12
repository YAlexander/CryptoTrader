using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class RsiBbands : BaseStrategy
	{
		public override string Name { get; } = "RSI Bbands";
		
		public override int MinNumberOfCandles { get; } = 200;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			RsiBbandsOptions options = Options.GetOptions<RsiBbandsOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> rsi = candles.Rsi(options?.Rsi ?? 6);
			BbandItem bbands = candles.Bbands(options?.Bbands ?? 200);
			List<decimal> closes = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 1)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (rsi[i - 1] > 50 && rsi[i] <= 50 && closes[i - 1] < bbands.UpperBand[i - 1] && closes[i] > bbands.UpperBand[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else if (rsi[i - 1] < 50 && rsi[i] >= 50 && closes[i - 1] < bbands.LowerBand[i - 1] && closes[i] > bbands.LowerBand[i])
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