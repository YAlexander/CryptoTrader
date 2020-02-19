using System;
using Contracts;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
	public class AtrIndicator : BaseIndicator<AtrOptions, SeriesIndicatorResult>
	{
		public override string Name { get; } = "Average True Range (ATR) Indicator";
		
		public override SeriesIndicatorResult Get(ICandle[] source, AtrOptions options)
		{
			decimal[] trueRanges = new decimal[source.Length];
			
			for (int i = 1; i < source.Length; i++)
			{
				// Calculate True Ranges. https://www.earnforex.com/guides/average-true-range/
				trueRanges[i] = Math.Max(source[i].High, source[i - 1].Close) - Math.Min(source[i].Low, source[i - 1].Close);
			}

			return trueRanges.Ema(options.Period);
		}

		public override SeriesIndicatorResult Get(decimal[] source, AtrOptions options)
		{
			throw new NotImplementedException();
		}

		public override SeriesIndicatorResult Get(decimal?[] source, AtrOptions options)
		{
			throw new NotImplementedException();
		}
	}
}