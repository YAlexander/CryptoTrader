using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;

namespace Core.Trading.Strategies
{
	public class Replex : BaseStrategy
	{
		public override string Name { get; } = "Replex";
		
		public override int MinNumberOfCandles { get; } = 20;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

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
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else if (rsi[i] < 30 && stoch.K[i] < 20 && close[i] < open[i] && stochRsi.K[i] < 20 && stoch.K[i] <= stoch.D[i] && stochRsi.K[i] <= stochRsi.D[i] && close[i] < bbands.LowerBand[i] - (bbands.MiddleBand[i] - bbands.LowerBand[i]) * 0.05m)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
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