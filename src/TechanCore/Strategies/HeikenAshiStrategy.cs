using System;
using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Results;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class HeikenAshiStrategy : BaseStrategy<HeikenAshiStrategyOptions>
	{
		public HeikenAshiStrategy(HeikenAshiStrategyOptions options) : base(options)
		{
		}

		public override string Name { get; } = "Heiken Ashi (HA) Strategy";

		public override int MinNumberOfCandles { get; } = 25; 

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles)
		{
			HeikenAshiStrategyOptions options = GetOptions;
			Validate(candles, options);

			List<ICandle> heikenSmoothed = candles.HeikenAshi(options.MaType, options.MaPeriod, true).Candles.ToList();

			for (int i = 1; i < heikenSmoothed.Count(); i++)
			{
				if (heikenSmoothed[i] == null || heikenSmoothed[i - 1] == null)
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
				else
				{
					// TODO: Implement logic
				}
			}

			return result;
		}
	}
}
