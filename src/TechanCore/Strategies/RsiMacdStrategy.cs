using System.Collections.Generic;
using Contracts;
using Contracts.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Results;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class RsiMacd : BaseStrategy <RsiMacdStrategyOptions>
	{
		public override string Name { get; } = "RSI MACD";
		
		public override int MinNumberOfCandles { get; } = 52;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			RsiMacdStrategyOptions options = GetOptions;
			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			MacdIndicatorResult macd = candles.Macd(options.MacdFastSmaPeriod, options.MacdSlowSmaPeriod, options.MacdSignalPeriod);
			decimal?[] rsi = candles.Rsi(options.RsiPeriod).Result;

			for (int i = 0; i < candles.Length; i++)
			{
				if (rsi[i] > 70 && macd.Macd[i] - macd.Signal[i] < 0)
				{
					result.Add((candles[i], TradingAdvices.SELL));
				}
				else if (rsi[i] < 30 && macd.Macd[i] - macd.Signal[i] > 0)
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

		public RsiMacd(RsiMacdStrategyOptions options) : base(options)
		{
		}
	}
}
