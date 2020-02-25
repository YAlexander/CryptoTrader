using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class BuyAndHold : BaseStrategy
	{
		public override string Name { get; } = "Buy and Hold";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.QUARTER_HOUR;

		public override int MinNumberOfCandles { get; } = 20;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			throw new NotImplementedException();
		}

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode> { TradingAdviceCode.BUY };
			TradingAdviceCode[] holdAdvices = new TradingAdviceCode[candles.Count() - 2];

			result.AddRange(holdAdvices);
			result.Add(TradingAdviceCode.SELL);

			return result.LastOrDefault();
		}
	}
}
