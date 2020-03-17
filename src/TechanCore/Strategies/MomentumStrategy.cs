using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class MomentumStrategy : BaseStrategy<MomentumStrategyOptions>
	{
		public override string Name { get; } = "Momentum Strategy";

		public override int MinNumberOfCandles { get; } = 30;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			MomentumStrategyOptions options = GetOptions;
			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			decimal?[] smaFast = candles.Sma(options.FastSmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] smaSlow = candles.Sma(options.SlowSmaPeriod, CandleVariables.CLOSE).Result;
			decimal?[] mom = candles.Momentum(options.MomentumPeriod, CandleVariables.CLOSE).Result;
			decimal?[] rsi = candles.Rsi(options.RsiPeriod).Result;
			decimal[] closes = candles.Select(x => x.Close).ToArray();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (rsi[i] < 30 && mom[i] > 0 && smaFast[i] > smaSlow[i] && closes[i] > smaSlow[i] && closes[i] > smaFast[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (rsi[i] > 70 && mom[i] < 0 && smaFast[i] < smaSlow[i] && closes[i] < smaSlow[i] && closes[i] < smaFast[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
			}

			return result;
		}

		public MomentumStrategy(MomentumStrategyOptions options) : base(options)
		{
		}
	}
}