using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Extensions;
using core.Trading.Extensions;
using core.Trading.Models;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class Replex : BaseStrategy
	{
		public override string Name { get; } = "Replex";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 20;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal?> rsi = candles.Rsi(14);
			BbandItem bbands = candles.Bbands(20);
			StochItem stoch = candles.Stoch();
			StochItem stochRsi = candles.StochRsi(fastKPeriod: 3);
			List<decimal> close = candles.Close();
			List<decimal> open = candles.Open();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (rsi[i] > 70 && stoch.K[i] > 80 && close[i] > open[i] && stochRsi.K[i] > 80 && stoch.K[i] >= stoch.D[i] && stochRsi.K[i] >= stochRsi.D[i] && close[i] > bbands.UpperBand[i] + (bbands.UpperBand[i] - bbands.MiddleBand[i]) * 0.05m)
				{
					result.Add(TradingAdviceCode.SELL);
				}
				else if (rsi[i] < 30 && stoch.K[i] < 20 && close[i] < open[i] && stochRsi.K[i] < 20 && stoch.K[i] <= stoch.D[i] && stochRsi.K[i] <= stochRsi.D[i] && close[i] < bbands.LowerBand[i] - (bbands.MiddleBand[i] - bbands.LowerBand[i]) * 0.05m)
				{
					result.Add(TradingAdviceCode.BUY);
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