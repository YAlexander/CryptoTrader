using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;

namespace Core.Trading.Strategies
{
	public class TheScalper : BaseStrategy
	{
		public override string Name { get; } = "The Scalper";
		
		public override int MinNumberOfCandles { get; } = 200;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			StochItem stoch = candles.Stoch();
			List<decimal?> sma200 = candles.Sma(200);

			List<decimal> closes = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 1)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else
				{
					if (sma200[i] < closes[i] &&
						stoch.K[i - 1] <= stoch.D[i - 1] &&
						stoch.K[i] > stoch.D[i] &&
						stoch.D[i - 1] < 20 &&
						stoch.K[i - 1] < 20)
					{
						result.Add((candles.ElementAt(i), TradingAdvices.BUY));
					}
					else if (stoch.K[i - 1] <= stoch.D[i - 1] &&
						stoch.K[i] > stoch.D[i] &&
						stoch.D[i - 1] > 80 &&
						stoch.K[i - 1] > 80)
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