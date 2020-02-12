using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;

namespace Core.Trading.Strategies
{
	public class FreqTrade : BaseStrategy
	{
		public override string Name { get; } = "Freq Trade";
		
		public override int MinNumberOfCandles { get; } = 40;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> rsi = candles.Rsi(14);
			List<decimal?> adx = candles.Adx(14);
			List<decimal?> plusDi = candles.PlusDi(14);
			List<decimal?> minusDi = candles.MinusDi(14);
			StochItem fast = candles.StochFast();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (rsi[i] < 25 && fast.D[i] < 30 && adx[i] > 30 && plusDi[i] > 5)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (adx[i] > 0 && minusDi[i] > 0 && fast.D[i] > 65)
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
