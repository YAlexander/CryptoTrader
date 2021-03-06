using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Extensions;
using core.Trading.Models;
using core.Trading.Strategies;
using core.TypeCodes;

namespace core.Trading.Extensions
{
	public class CloudBreakout : BaseStrategy
	{
		public override string Name => "Cloud Breakout";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 120;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			IchimokuItem ichiMoku = candles.Ichimoku();
			List<decimal> close = candles.Close();

			List<bool> cloudBreakUpA = close.Crossover(ichiMoku.SenkouSpanA);
			List<bool> cloudBreakDownA = close.Crossunder(ichiMoku.SenkouSpanA);
			List<bool> cloudBreakUpB = close.Crossover(ichiMoku.SenkouSpanB);
			List<bool> cloudBreakDownB = close.Crossunder(ichiMoku.SenkouSpanB);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				// Upward cloud break from the bottom
				else if (ichiMoku.SenkouSpanA[i] > ichiMoku.SenkouSpanB[i] && cloudBreakUpB[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				else if (ichiMoku.SenkouSpanA[i] < ichiMoku.SenkouSpanB[i] && cloudBreakUpA[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				// Downward cloud break from the top
				else if (ichiMoku.SenkouSpanA[i] > ichiMoku.SenkouSpanB[i] && cloudBreakDownA[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
				}
				else if (ichiMoku.SenkouSpanA[i] < ichiMoku.SenkouSpanB[i] && cloudBreakDownB[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
			}

			return result;
		}
	}
}