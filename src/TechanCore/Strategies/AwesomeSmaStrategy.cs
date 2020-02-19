using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class AwesomeSmaStrategy : BaseStrategy<AwesomeSmaStrategyOptions>
	{
		public override string Name { get; } = "Awesome SMA";

		public override int MinNumberOfCandles { get; } = 40;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			AwesomeSmaStrategyOptions options = GetOptions;
			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			decimal?[] ao = candles.AwesomeOscillator(options.AwesomeFastPeriod, options.AwesomeSlowPeriod).Result;
			decimal?[] smaShort = candles.Sma(options.SmaFastPeriod, CandleVariables.CLOSE).Result;
			decimal?[] smaLong = candles.Sma(options.SmaSlowPeriod, CandleVariables.CLOSE).Result;

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles[i], TradingAdvices.HOLD));
				}
				else if (ao[i] > 0 && ao[i - 1] < 0 && smaShort[i] > smaLong[i] || ao[i] > 0 && smaShort[i] > smaLong[i] && smaShort[i - 1] < smaLong[i - 1])
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (smaShort[i] < smaLong[i] && smaShort[i - 1] > smaLong[i - 1])
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

		public AwesomeSmaStrategy(AwesomeSmaStrategyOptions options) : base(options)
		{
		}
	}
}