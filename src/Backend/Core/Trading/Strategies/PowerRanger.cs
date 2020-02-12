using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;

namespace Core.Trading.Strategies
{
	public class PowerRanger : BaseStrategy
	{
		public override string Name { get; } = "Power Ranger";

		public override int MinNumberOfCandles { get; } = 10;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();
			StochItem stoch = candles.Stoch(10);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 1)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else
				{
					if (stoch.K[i] > 20 && stoch.K[i - 1] < 20 || stoch.D[i] > 20 && stoch.D[i - 1] < 20)
					{
						result.Add((candles.ElementAt(i), TradingAdvices.BUY));
					}
					else if (stoch.K[i] < 80 && stoch.K[i - 1] > 80 || stoch.D[i] < 80 && stoch.D[i - 1] > 80)
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
