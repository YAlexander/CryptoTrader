using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Results;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class SmaStochRsiStrategy : BaseStrategy<SmaStochRsiStrategyOptions>
	{
		public override string Name { get; } = "SMA Stoch RSI Strategy";

		public override int MinNumberOfCandles { get; } = 150;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] source)
		{
			SmaStochRsiStrategyOptions options = GetOptions;
			Validate(source, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			StochasticOscillatorResult stoch = source.StochasticOscillator(options.StochPeriod, options.StochEmaPeriod);
			decimal?[] sma = source.Close().Sma(options.SmaPeriod).Result;
			decimal?[] rsi = source.Rsi(options.RsiPeriod).Result;

			for (int i = 0; i < source.Length; i++)
			{
				if (i < 1)
				{
					result.Add((source[i], TradingAdvices.HOLD));
				}
				else
				{
					if (source[i] .Close> sma[i] && stoch.K[i] > 70 && rsi[i] < 20 && stoch.K[i] > stoch.D[i])
					{
						result.Add((source[i], TradingAdvices.BUY));
					}
					else if (source[i].Close < sma[i] && stoch.K[i] > 70 && rsi[i] > 80 && stoch.K[i] < stoch.D[i])
					{
						result.Add((source[i], TradingAdvices.SELL));
					}
					else
					{
						result.Add((source[i], TradingAdvices.HOLD));
					}
				}
			}

			return result;
		}

		public SmaStochRsiStrategy(SmaStochRsiStrategyOptions options) : base(options)
		{
		}
	}
}

