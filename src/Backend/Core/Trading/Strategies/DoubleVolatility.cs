using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.TAIndicators.Extensions;
using Core.TypeCodes;

namespace Core.Trading.Strategies
{
	public class DoubleVolatility : BaseStrategy
	{
		public override string Name { get; } = "Double Volatility";

		public override int MinNumberOfCandles { get; } = 20;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> sma5High = candles.Sma(5, CandleVariableCode.High);
			List<decimal?> sma20High = candles.Sma(20, CandleVariableCode.High);
			List<decimal?> sma20Low = candles.Sma(20, CandleVariableCode.Low);
			List<decimal> closes = candles.Select(x => x.Close).ToList();
			List<decimal> opens = candles.Select(x => x.Open).ToList();
			List<decimal?> rsi = candles.Rsi(11);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 1)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (sma5High[i] > sma20High[i] && rsi[i] > 65 && Math.Abs(opens[i - 1] - closes[i - 1]) > 0 && Math.Abs(opens[i] - closes[i]) / Math.Abs(opens[i - 1] - closes[i - 1]) < 2)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (sma5High[i] < sma20Low[i] && rsi[i] < 35)
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
