using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class DoubleVolatility : BaseStrategy
	{
		public override string Name { get; } = "Double Volatility";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 20;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal?> sma5High = candles.Sma(5, CandleVariableCode.HIGH);
			List<decimal?> sma20High = candles.Sma(20, CandleVariableCode.HIGH);
			List<decimal?> sma20Low = candles.Sma(20, CandleVariableCode.LOW);
			List<decimal> closes = candles.Select(x => x.Close).ToList();
			List<decimal> opens = candles.Select(x => x.Open).ToList();
			List<decimal?> rsi = candles.Rsi(11);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i < 1)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else if (sma5High[i] > sma20High[i] && rsi[i] > 65 && Math.Abs(opens[i - 1] - closes[i - 1]) > 0 && Math.Abs(opens[i] - closes[i]) / Math.Abs(opens[i - 1] - closes[i - 1]) < 2)
				{
					result.Add(TradingAdviceCode.BUY);
				}
				else if (sma5High[i] < sma20Low[i] && rsi[i] < 35)
				{
					result.Add(TradingAdviceCode.SELL);
				}
				else
				{
					result.Add(TradingAdviceCode.HOLD);
				}
			}

			return result.LastOrDefault();
		}
	}
}
