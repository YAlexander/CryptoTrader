using System.Collections.Generic;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Results;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class MaStochRsiStrategy : BaseStrategy<MaStochRsiStrategyOptions>
	{
		public override string Name { get; } = "SMA Stoch RSI Strategy";

		public override int MinNumberOfCandles { get; } = 150;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			MaStochRsiStrategyOptions options = GetOptions;
			Validate(candles, options);

			StochasticOscillatorResult stoch = candles.StochasticOscillator(options.StochPeriod, options.StochEmaPeriod);
			decimal?[] rsi = candles.Rsi(options.RsiPeriod).Result;
			decimal?[] ma;

			if (options.MaType == MaTypes.EMA)
			{
				ma = candles.Ema(options.MaPeriod, CandleVariables.CLOSE).Result;
			}
			else if (options.MaType == MaTypes.WMA)
			{
				ma = candles.Wma(options.MaPeriod, CandleVariables.CLOSE).Result;
			}
			else
			{
				ma = candles.Sma(options.MaPeriod, CandleVariables.CLOSE).Result;
			}

			for (int i = 0; i < candles.Length; i++)
			{
				if (i < 1)
				{
					Result.Add((candles[i], TradingAdvices.HOLD));
				}
				else
				{
					if (candles[i] .Close> ma[i] && stoch.K[i] > 70 && rsi[i] < 20 && stoch.K[i] > stoch.D[i])
					{
						Result.Add((candles[i], TradingAdvices.BUY));
					}
					else if (candles[i].Close < ma[i] && stoch.K[i] > 70 && rsi[i] > 80 && stoch.K[i] < stoch.D[i])
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

		public MaStochRsiStrategy(MaStochRsiStrategyOptions options) : base(options)
		{
		}
	}
}

