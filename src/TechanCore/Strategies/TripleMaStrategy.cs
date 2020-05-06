using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class TripleMaStrategy : BaseStrategy<TripleMaStrategyOptions>
	{
		public override string Name { get; } = "Triple MA Strategy";

		public override int MinNumberOfCandles { get; } = 50;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts(ICandle[] candles, IOrdersBook ordersBook = null)
		{
			TripleMaStrategyOptions options = GetOptions;
			Validate(candles, options);
			
			decimal?[] smaFast = candles.Sma(options.FastSmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] smaSlow = candles.Sma(options.SlowSmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] ema = candles.Ema(options.EmaPeriod, CandleVariables.CLOSE).Result;

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					Result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (ema[i] > smaSlow[i] && ema[i - 1] < smaSlow[i - 1])
				{
					Result.Add((candles.ElementAt(i), TradingAdvices.BUY)); // A cross of the EMA and long SMA is a buy signal.
				}
				else if (ema[i] < smaSlow[i] && ema[i - 1] > smaSlow[i - 1] || ema[i] < smaFast[i] && ema[i - 1] > smaFast[i - 1])
				{
					Result.Add((candles.ElementAt(i), TradingAdvices.SELL)); // As soon as our EMA crosses below an SMA its a sell signal.
				}
				else
				{
					Result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
			}

			return Result;
		}

		public TripleMaStrategy(TripleMaStrategyOptions options) : base(options)
		{
		}
	}
}
