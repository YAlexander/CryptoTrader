using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;

namespace Core.Trading.Strategies
{
	public class BuyAndHold : BaseStrategy
	{
		public override string Name { get; } = "Buy and Hold";

		public override int MinNumberOfCandles { get; } = 20;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			throw new NotImplementedException();
		}

		public override TradingAdvices Forecast (ICandle[] candles)
		{
			Validate(candles, default);

			List<TradingAdvices> result = new List<TradingAdvices> { TradingAdvices.BUY };
			TradingAdvices[] holdAdvices = new TradingAdvices[candles.Count() - 2];

			result.AddRange(holdAdvices);
			result.Add(TradingAdvices.SELL);

			return result.LastOrDefault();
		}
	}
}
