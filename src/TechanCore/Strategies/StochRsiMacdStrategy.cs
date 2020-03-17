using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Results;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class StochRsiMacdStrategy : BaseStrategy<StochRsiMacdStrategyOptions>
	{
		public override string Name { get; } = "STOCH RSI MACD Strategy";

		public override int MinNumberOfCandles { get; } = 99;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			StochRsiMacdStrategyOptions options = GetOptions;
			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			MacdIndicatorResult macd = candles.Macd(options.MacdFastSmaPeriod, options.MacdSlowSmaPeriod, options.MacdSignalPeriod);
			StochasticOscillatorResult stoch = candles.StochasticOscillator(options.StochPeriod, options.StochEmaPeriod);

			for (int i = 0; i < candles.Length; i++)
			{
				if (macd.Macd[i] - macd.Signal[i] < 0 && stoch.K[i] > 70 && stoch.K[i] < stoch.D[i])
				{
					result.Add((candles[i], TradingAdvices.SELL));
				}
				else if (macd.Macd[i] - macd.Signal[i] > 0 && stoch.K[i] < 30 && stoch.K[i] > stoch.D[i])
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

		public StochRsiMacdStrategy(StochRsiMacdStrategyOptions options) : base(options)
		{
		}
	}
}
