using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;

namespace Core.Trading.Strategies
{
	public class FreqTradeEvo : BaseStrategy
	{
		public override string Name { get; } = "Freq Trade Evo";

		public override int MinNumberOfCandles { get; } = 40;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> rsi = candles.Rsi(5);
			StochItem fast = candles.StochFast();
			
			for (int i = 0; i < candles.Count(); i++)
			{
				if (rsi[i] < 22 && fast.K[i] < 25 && fast.D[i - 1] > fast.K[i - 1] && fast.D[i] - fast.K[i] < 0.3m)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (rsi[i] > 70 && fast.K[i] > 50)
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