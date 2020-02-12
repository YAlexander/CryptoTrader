using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;

namespace Core.Trading.Strategies
{
	public class CloudBreakout : BaseStrategy
	{
		public override string Name => "Cloud Breakout";

		public override int MinNumberOfCandles { get; } = 120;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

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
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				// Upward cloud break from the bottom
				else if (ichiMoku.SenkouSpanA[i] > ichiMoku.SenkouSpanB[i] && cloudBreakUpB[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (ichiMoku.SenkouSpanA[i] < ichiMoku.SenkouSpanB[i] && cloudBreakUpA[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				// Downward cloud break from the top
				else if (ichiMoku.SenkouSpanA[i] > ichiMoku.SenkouSpanB[i] && cloudBreakDownA[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else if (ichiMoku.SenkouSpanA[i] < ichiMoku.SenkouSpanB[i] && cloudBreakDownB[i])
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