using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;

namespace Core.Trading.Strategies
{

	public class StochAdx : BaseStrategy
	{
		public override string Name { get; } = "Stoch ADX";
		
		public override int MinNumberOfCandles { get; } = 15;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			StochItem stoch = candles.Stoch(13);
			List<decimal?> adx = candles.Adx(14);
			List<decimal?> bearBull = candles.BearBull();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (adx[i] > 50 && (stoch.K[i] > 90 || stoch.D[i] > 90) && bearBull[i] == -1)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else if (adx[i] < 20 && (stoch.K[i] < 10 || stoch.D[i] < 10) && bearBull[i] == 1)
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