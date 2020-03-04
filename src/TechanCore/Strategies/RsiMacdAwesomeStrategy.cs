using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Results;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class RsiMacdAwesomeStrategy : BaseStrategy<RsiMacdAwesomeStrategyOptions>
	{
		public override string Name { get; } = "RSI MACD Awesome Strategy";

		public override int MinNumberOfCandles { get; } = 35;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			RsiMacdAwesomeStrategyOptions options = GetOptions;
			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			MacdIndicatorResult macd = candles.Macd(options.MacdFastPeriod, options.MacdSlowPeriod, options.MacdSignalPeriod);
			decimal?[] rsi = candles.Rsi(options.RsiPeriod).Result;
			decimal?[] ao = candles.AwesomeOscillator(options.AwesomeFastPeriod, options.AwesomeSlowPeriod).Result;

			for (int i = 0; i < candles.Length; i++)
			{
				if (macd.Hist[i] < 0 && ao[i] > 0 && rsi[i] < 45)
				{
					result.Add((candles[i], TradingAdvices.BUY));
				}
				else if (macd.Hist[i] > 0 && ao[i] < 0 && rsi[i] > 45)
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

		public RsiMacdAwesomeStrategy(RsiMacdAwesomeStrategyOptions options) : base(options)
		{
		}
	}
}