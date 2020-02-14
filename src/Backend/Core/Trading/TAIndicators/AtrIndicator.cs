using System;
using Contracts;
using Core.Trading.TAIndicators;
using Core.Trading.TAIndicators.Extensions;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace core.Trading.TAIndicators
{
	public class AtrIndicator : BaseIndicator<AtrOptions, DefaultIndicatorResult>
	{
		public override string Name { get; } = "ATR Indicator";
		
		public override DefaultIndicatorResult Get(ICandle[] source, AtrOptions options)
		{
			decimal[] trueRanges = new decimal[source.Length];
			
			for (int i = 1; i < source.Length; i++)
			{
				// Calculate True Ranges. https://www.earnforex.com/guides/average-true-range/
				trueRanges[i] = Math.Max(source[i].High, source[i - 1].Close) - Math.Min(source[i].Low, source[i - 1].Close);
			}

			return trueRanges.Ema(options.Period);
		}

		public override DefaultIndicatorResult Get(decimal[] source, AtrOptions options)
		{
			throw new NotImplementedException();
		}

		public override DefaultIndicatorResult Get(decimal?[] source, AtrOptions options)
		{
			throw new NotImplementedException();
		}
	}
}