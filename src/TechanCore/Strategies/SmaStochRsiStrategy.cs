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

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			SmaStochRsiStrategyOptions options = GetOptions;
			Validate(candles, options);

			StochasticOscillatorResult stoch = candles.StochasticOscillator(options.StochPeriod, options.StochEmaPeriod);
			decimal?[] sma = candles.Close().Sma(options.SmaPeriod).Result;
			decimal?[] rsi = candles.Rsi(options.RsiPeriod).Result;

			for (int i = 0; i < candles.Length; i++)
			{
				if (i < 1)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else
				{
					if (candles[i] .Close> sma[i] && stoch.K[i] > 70 && rsi[i] < 20 && stoch.K[i] > stoch.D[i])
					{
						Result.Add((candles[i], TradingAdvices.BUY));
					}
					else if (candles[i].Close < sma[i] && stoch.K[i] > 70 && rsi[i] > 80 && stoch.K[i] < stoch.D[i])
					{
						Result.Add((candles[i], TradingAdvices.SELL));
					}
					else
					{
						Result.Add((candles[i], TradingAdvices.HOLD));
					}
				}
			}

			return Result;
		}

		public SmaStochRsiStrategy(SmaStochRsiStrategyOptions options) : base(options)
		{
		}
	}
}

