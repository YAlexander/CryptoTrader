using System.Linq;
using TechanCore.Helpers;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
	public class MacdIndicator : BaseIndicator<MacdOptions, MacdIndicatorResult>
	{
		public override string Name { get; } = "Moving Average Convergence Divergence (MACD) Indicator";
		
		public override MacdIndicatorResult Get(ICandle[] source, MacdOptions options)
		{
			decimal[] values = source.Select(x => x.Close).ToArray();
			return Get(values, options);
		}
		
		public override MacdIndicatorResult Get(decimal[] source, MacdOptions options)
		{
			decimal?[] values = source.ToNullable();
			return Get(values, options);
		}

		public override MacdIndicatorResult Get(decimal?[] source, MacdOptions options)
		{
			decimal?[] macd = new decimal?[source.Length];
			decimal?[] hist = new decimal?[source.Length];
			
			decimal?[] fastEma = source.Ema(options.FastPeriod).Result;
			decimal?[] slowEma = source.Ema(options.SlowPeriod).Result;

			for (int i = 0; i < source.Length; i++)
			{
				if (fastEma[i].HasValue && slowEma[i].HasValue)
				{
					macd[i] = fastEma[i] - slowEma[i];
				}
			}

			decimal?[] signal = macd.Ema(options.SignalPeriod).Result;

			for (int i = 0; i < source.Length; i++)
			{
				if (macd[i].HasValue && signal[i].HasValue)
				{
					hist[i] = macd[i] - signal[i];
				}
			}
			
			return new MacdIndicatorResult { Macd = macd, Signal = signal, Hist = hist};
		}
	}
}