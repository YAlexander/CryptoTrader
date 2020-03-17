using System.Linq;
using TechanCore.Helpers;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
	public class RsiIndicator : BaseIndicator<RsiOptions, SeriesIndicatorResult>
	{
		public override string Name { get; } = "Relative Strength Index (RSI) Indicator";
		
		public override SeriesIndicatorResult Get(decimal?[] source, RsiOptions options)
		{
			decimal?[] rsi = new decimal?[source.Length];
			decimal?[] change = new decimal?[source.Length];

			for (int i = options.Period; i < source.Length; i++)
			{
				decimal? averageGain = change.Skip(i - options.Period).Take(options.Period).Where(x => x > 0).Sum() / change.Length; 
				decimal? averageLoss = -1 * change.Skip(i - options.Period).Take(options.Period).Where(x => x < 0).Sum() / change.Length;
				decimal? rs = averageGain / averageLoss; 
				rsi[i] = 100 - 100 / (1 + rs);
				
				change[i] = source[i] - source[i - 1];
			}

			return new SeriesIndicatorResult { Result = rsi };
		}
		
		public override SeriesIndicatorResult Get(ICandle[] source, RsiOptions options)
		{
			decimal[] values = source.Close();
			return Get(values, options);
		}

		public override SeriesIndicatorResult Get(decimal[] source, RsiOptions options)
		{
			decimal?[] values = source.ToNullable();
			return Get(values, options);
		}
	}
}