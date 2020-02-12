using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class QuickSma : BaseStrategy
	{
		public override string Name { get; } = "Quick SMA";
		
		public override int MinNumberOfCandles { get; } = 20;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{			
			QuickSmaOptions options = Options.GetOptions<QuickSmaOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> smaFast = candles.Sma(options?.SmaFast ?? 12);
			List<decimal?> smaSlow = candles.Sma(options?.SmaSlow ?? 18);

			List<decimal> closes = candles.Close();
			List<bool> crossOver = smaFast.Crossover(smaSlow);
			List<bool> crossUnder = smaSlow.Crossunder(closes);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (crossOver[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (crossUnder[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
			}

			return result;
		}
	}
}
