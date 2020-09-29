using System;
using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
	public class LinearlyWeightedMovingAverageIndicator : BaseIndicator<LinearlyWeightedMovingAverageOptions, SeriesIndicatorResult>
	{
		public override string Name => "Linearly Weighted Moving Average (LWMA) Indicator";

		public override SeriesIndicatorResult Get(ICandle[] source, LinearlyWeightedMovingAverageOptions options)
		{
			decimal[] values = options.CandleVariable switch
			{
				CandleVariables.CLOSE => source.Close(),
				CandleVariables.HIGH => source.High(),
				CandleVariables.LOW => source.Low(),
				CandleVariables.OPEN => source.Open(),
				_ => throw new Exception("Unknown CandleVariableCode")
			};

			return Get(values, options);
		}

		public override SeriesIndicatorResult Get(decimal[] source, LinearlyWeightedMovingAverageOptions options)
		{
			decimal?[] values = source.ToNullable();
			return Get(values, options);
		}

		public override SeriesIndicatorResult Get(decimal?[] source, LinearlyWeightedMovingAverageOptions options)
		{
			decimal?[] result = new decimal?[source.Length];

			int weightsSum = Enumerable.Range(1, options.Period).Sum();

			for (int i = 0; i < source.Length; i++)
			{
				if (i > options.Period)
				{
					decimal? ma = 0m;

					for (int index = 1; index <= options.Period; index++)
					{
						ma += index * source[i - options.Period + index - 1];
					}

					result[i] = ma / weightsSum;
				}
			}

			return new SeriesIndicatorResult { Result = result };
		}
	}
}
