﻿using System;
using System.Linq;
using Contracts;
using Contracts.Enums;
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
              				CandleVariables.CLOSE => source.Select(x => x.Close).ToArray(),
              				CandleVariables.HIGH => source.Select(x => x.High).ToArray(),
              				CandleVariables.LOW => source.Select(x => x.Low).ToArray(),
              				CandleVariables.OPEN => source.Select(x => x.Open).ToArray(),
              				_ => throw new Exception("Unknown CandleVariableCode")
              			};

			return Get(values, options);
		}

		public override SeriesIndicatorResult Get(decimal[] source, SmaOptions options)
		{
			decimal?[] values = source.Select(x => (decimal?) x).ToArray();
			return Get(values, options);
		}
	}
}