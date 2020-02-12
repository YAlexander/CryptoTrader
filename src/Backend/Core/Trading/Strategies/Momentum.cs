using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;

namespace Core.Trading.Strategies
{
	public class Momentum : BaseStrategy
	{
		public override string Name { get; } = "Momentum";

		public override int MinNumberOfCandles { get; } = 30;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> smaFast = candles.Sma(11);
			List<decimal?> smaSlow = candles.Sma(21);
			List<decimal?> mom = candles.Mom(30);
			List<decimal?> rsi = candles.Rsi();
			List<decimal> closes = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (rsi[i] < 30 && mom[i] > 0 && smaFast[i] > smaSlow[i] && closes[i] > smaSlow[i] && closes[i] > smaFast[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (rsi[i] > 70 && mom[i] < 0 && smaFast[i] < smaSlow[i] && closes[i] < smaSlow[i] && closes[i] < smaFast[i])
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