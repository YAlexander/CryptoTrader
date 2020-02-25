using System.Collections.Generic;
using Contracts;
using Contracts.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class RsiAwesomeMfiStrategy : BaseStrategy<RsiAwesomeMfiStrategyOptions>
	{
		public override string Name { get; } = "RSI Awesome MFI Strategy";
		
		public override int MinNumberOfCandles { get; } = 35;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			RsiAwesomeMfiStrategyOptions options = GetOptions;
			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			decimal?[] rsi = candles.Rsi(options.RsiPeriod).Result;
			decimal?[] mfi = candles.Mfi(options.MfiPeriod).Result;
			decimal?[] ao = candles.AwesomeOscillator(options.AwesomeFastPeriod, options.AwesomeSlowPeriod).Result;

			for (int i = 0; i < candles.Length; i++)
			{
				if (mfi[i] < 30 && rsi[i] < 45 && ao[i] > 0)
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (mfi[i] > 30 && rsi[i] > 45 && ao[i] < 0)
				{
					result.Add((candles[i], TradingAdvices.SELL));
				}
				else
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
			}

			return result;
		}

		public RsiAwesomeMfiStrategy(RsiAwesomeMfiStrategyOptions options) : base(options)
		{
		}
	}
}