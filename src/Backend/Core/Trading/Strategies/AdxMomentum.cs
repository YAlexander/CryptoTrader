using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Contracts.Trading;
using Core.Trading.Extensions;
using Core.Trading.Strategies.StrategyOptions;
using Persistence.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class AdxMomentum : BaseStrategy
	{
		public override string Name { get; } = "ADX Momentum";
		
		public override int MinNumberOfCandles { get; } = 25;
		
		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			AdxMomentumOptions options = Options.GetOptions<AdxMomentumOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> adx = candles.Adx(options?.Adx ?? 14);
			List<decimal?> diPlus = candles.PlusDi(options?.PlusDi ?? 25);
			List<decimal?> diMinus = candles.MinusDi(options?.MinusDi ?? 25);
			//List<decimal?> sar = candles.Sar(options?.AccelerationFactor ?? 0.02, options?.MaximumAccelerationFactor ?? 0.2);
			List<decimal?> mom = candles.Mom(options?.Mom ?? 14);

			for (int i = 0; i < candles.Count(); i++)
			{

				if (adx[i] > 25 && mom[i] < 0 && diMinus[i] > 25 && diPlus[i] < diMinus[i])
				{
					result.Add((candles[i], TradingAdvices.SELL));
				}
				else if (adx[i] > 25 && mom[i] > 0 && diPlus[i] > 25 && diPlus[i] > diMinus[i])
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
			}

			return result;
		}
	}
}