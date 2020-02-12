using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;

namespace Core.Trading.Strategies
{
	public class Base150 : BaseStrategy
	{
		public override string Name { get; } = "Base 150";

		public override int MinNumberOfCandles { get; } = 365;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> sma6 = candles.Sma(6);
			List<decimal?> sma25 = candles.Sma(25);
			List<decimal?> sma150 = candles.Sma(150);
			List<decimal?> sma365 = candles.Sma(365);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else
				{
					if (sma6[i] > sma150[i] && sma6[i] > sma365[i] && sma25[i] > sma150[i] && sma25[i] > sma365[i] && (sma6[i - 1] < sma150[i] || sma6[i - 1] < sma365[i] || sma25[i - 1] < sma150[i] || sma25[i - 1] < sma365[i]))
					{
						result.Add((candles.ElementAt(i), TradingAdvices.BUY));
					}
					if (sma6[i] < sma150[i] && sma6[i] < sma365[i] && sma25[i] < sma150[i] && sma25[i] < sma365[i] && (sma6[i - 1] > sma150[i] || sma6[i - 1] > sma365[i] || sma25[i - 1] > sma150[i] || sma25[i - 1] > sma365[i]))
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
