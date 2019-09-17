using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.Trading.Models;
using core.TypeCodes;

namespace core.Trading.Strategies
{

	public class StochAdx : BaseStrategy
	{
		public override string Name { get; } = "Stoch ADX";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 15;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			StochItem stoch = candles.Stoch(13);
			List<decimal?> adx = candles.Adx(14);
			List<decimal?> bearBull = candles.BearBull();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (adx[i] > 50 && (stoch.K[i] > 90 || stoch.D[i] > 90) && bearBull[i] == -1)
				{
					result.Add(TradingAdviceCode.SELL);
				}
				else if (adx[i] < 20 && (stoch.K[i] < 10 || stoch.D[i] < 10) && bearBull[i] == 1)
				{
					result.Add(TradingAdviceCode.BUY);
				}
				else
				{
					result.Add(TradingAdviceCode.HOLD);
				}
			}

			return result.LastOrDefault(); ;
		}
	}
}