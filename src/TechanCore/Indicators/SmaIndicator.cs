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
	public class SmaIndicator : BaseIndicator<SmaOptions, SeriesIndicatorResult>
	{
		public override string Name { get; } = "Simple Moving Average (SMA) Indicator";
		
		public override SeriesIndicatorResult Get(decimal?[] source, SmaOptions options)
		{
			decimal?[] result = new decimal?[source.Length];
			
			for (int i = 0; i < source.Length; i++)
			{   
				if (i >= options.Period - 1)
				{
					decimal? sum = 0;
					for (int j = i; j >= i - (options.Period - 1); j--)
					{
						sum += source[j];
					}
					
					decimal? avg = sum / options.Period;
					result[i] = avg;
				}
				else
				{
					result[i] = null;
				}
			}

			return new SeriesIndicatorResult { Result = result};
		}
		
		public override SeriesIndicatorResult Get(ICandle[] source, SmaOptions options)
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

		public override SeriesIndicatorResult Get(decimal[] source, SmaOptions options)
		{
			decimal?[] values = source.ToNullable();
			return Get(values, options);
		}
	}
}