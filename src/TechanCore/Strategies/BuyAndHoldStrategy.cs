using System;
using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class BuyAndHoldStrategy : BaseStrategy<EmptyStrategyOptions>
	{
		public override string Name { get; } = "Buy and Hold";

		public override int MinNumberOfCandles { get; } = 20;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			throw new NotImplementedException();
		}

		public override TradingAdvices Forecast (ICandle[] candles)
		{
			Validate(candles, null);

			List<TradingAdvices> result = new List<TradingAdvices> { TradingAdvices.BUY };
			TradingAdvices[] holdAdvices = new TradingAdvices[candles.Count() - 2];

			result.AddRange(holdAdvices);
			result.Add(TradingAdvices.SELL);

			return result.LastOrDefault();
		}

		public BuyAndHoldStrategy(EmptyStrategyOptions options) : base(options)
		{
		}
	}
}
