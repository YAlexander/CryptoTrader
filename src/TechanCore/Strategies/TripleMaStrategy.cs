using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public class TripleMaStrategy : BaseStrategy<TripleMaOptions>
	{
		public override string Name { get; } = "Triple MA";

		public override int MinNumberOfCandles { get; } = 50;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			TripleMaOptions options = GetOptions;			
			Validate(candles, options);
			
			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> sma1 = candles.Sma(options.FastSmaPeriod);
			List<decimal?> sma2 = candles.Sma(options.SlowSmaPeriod);
			List<decimal?> ema = candles.Ema(options.EmaPeriod);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (ema[i] > sma2[i] && ema[i - 1] < sma2[i - 1])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY)); // A cross of the EMA and long SMA is a buy signal.
				}
				else if (ema[i] < sma2[i] && ema[i - 1] > sma2[i - 1] || ema[i] < sma1[i] && ema[i - 1] > sma1[i - 1])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL)); // As soon as our EMA crosses below an SMA its a sell signal.
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
			}

			return result;
		}
	}
}
