using System;
using System.Linq;
using Contracts;
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
			
			for (int i = options.Period; i < source.Length; i++)
			{
				result[i] = source.Skip(i - options.Period).Take(options.Period).Average();
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