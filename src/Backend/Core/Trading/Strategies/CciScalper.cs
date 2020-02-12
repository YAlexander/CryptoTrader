using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;

namespace Core.Trading.Strategies
{
	public class CciScalper : BaseStrategy
	{
		public override string Name { get; } = "CCI Scalper";

		public override int MinNumberOfCandles { get; } = 14;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> cci = candles.Cci();
			List<decimal?> ema10 = candles.Ema(10);
			List<decimal?> ema21 = candles.Ema(21);
			List<decimal?> ema50 = candles.Ema(50);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (cci[i] < -100 && ema10[i] > ema21[i] && ema10[i] > ema50[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (cci[i] > 100 && ema10[i] < ema21[i] && ema10[i] < ema50[i])
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
