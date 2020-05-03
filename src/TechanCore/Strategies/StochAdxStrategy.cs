using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Results;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{

	public class StochAdxStrategy : BaseStrategy<StochAdxStrategyOptions>
	{
		public override string Name { get; } = "Stoch ADX";
		
		public override int MinNumberOfCandles { get; } = 15;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			StochAdxStrategyOptions options = GetOptions;
			Validate(candles, options);

			StochasticOscillatorResult stoch = candles.StochasticOscillator(options.StochPeriod, options.StochEmaPeriod);
			AdxResult adx = candles.Adx(options.AdxPeriod);
			decimal?[] bearBull = candles.BearAndBull().Result;

			for (int i = 0; i < candles.Count(); i++)
			{
				if (adx.Adx[i] > 50 && (stoch.K[i] > 90 || stoch.D[i] > 90) && bearBull[i] == -1)
				{
					Result.Add((candles[i], TradingAdvices.SELL));
				}
				else if (adx.Adx[i] < 20 && (stoch.K[i] < 10 || stoch.D[i] < 10) && bearBull[i] == 1)
				{
					Result.Add((candles[i], TradingAdvices.BUY));
				}
				else
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
			}

			return Result;
		}

		public StochAdxStrategy(StochAdxStrategyOptions options) : base(options)
		{
		}
	}
}