using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class AwesomeSma : BaseStrategy
	{
		public override string Name { get; } = "Awesome SMA";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 40;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal?> ao = candles.AwesomeOscillator();
			List<decimal?> smaShort = candles.Sma(20);
			List<decimal?> smaLong = candles.Sma(40);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else if (ao[i] > 0 && ao[i - 1] < 0 && smaShort[i] > smaLong[i] ||
					ao[i] > 0 && smaShort[i] > smaLong[i] && smaShort[i - 1] < smaLong[i - 1])
				{
					result.Add(TradingAdviceCode.BUY);
				}
				else if (smaShort[i] < smaLong[i] && smaShort[i - 1] > smaLong[i - 1])
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