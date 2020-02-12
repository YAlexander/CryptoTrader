using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;

namespace Core.Trading.Strategies
{
	public class AwesomeMacd : BaseStrategy
	{
		public override string Name { get; } = "Awesome MACD";

		public override int MinNumberOfCandles { get; } = 40;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> ao = candles.AwesomeOscillator();
			MacdItem macd = candles.Macd(5, 7, 4);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (ao[i] < 0 && ao[i - 1] > 0 && macd.Hist[i] < 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else if (ao[i] > 0 && ao[i - 1] < 0 && macd.Hist[i] > 0)
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