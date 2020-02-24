using System;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
	public class EmaIndicator : BaseIndicator<EmaOptions, SeriesIndicatorResult>
	{
		public override string Name { get; } = "Exponential Moving Average (EMA) Indicator";
		
		public override SeriesIndicatorResult Get(ICandle[] source, EmaOptions options)
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

		public override SeriesIndicatorResult Get(decimal[] source, EmaOptions options)
		{
			decimal?[] values = source.ToNullable();
			return Get(values, options);
		}
		
		public override SeriesIndicatorResult Get(decimal?[] source, EmaOptions options)
		{
			decimal?[] result = new decimal?[source.Length];
			decimal multiplier = (decimal)2 / (options.Period + 1);
			
			for (int i = 0; i < source.Length; i++)
			{
				if (i >= options.Period - 1)
				{
					if (result[i - 1].HasValue)
					{
						decimal? emaPrev = result[i - 1];
						result[i] = source[i] * multiplier + emaPrev * (1 - multiplier);
					}
					else
					{
						decimal? sum = 0;
						
						for (int j = i; j >= i - (options.Period - 1); j--)
						{
							sum += source[i];
						}
						
						result[i] = sum / options.Period;
					}
				}
				else
				{
					result[i] = null;
				}
			}
			
			return new SeriesIndicatorResult { Result = result};
		}
	}
}