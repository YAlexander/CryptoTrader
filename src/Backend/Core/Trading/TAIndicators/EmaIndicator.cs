using System;
using System.Linq;
using Contracts;
using Contracts.Enums;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators
{
	public class EmaIndicator : BaseIndicator<EmaOptions, DefaultIndicatorResult>
	{
		public override string Name { get; } = "Exponential Moving Average (EMA) Indicator";
		
		public override DefaultIndicatorResult Get(ICandle[] source, EmaOptions options)
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

		public override DefaultIndicatorResult Get(decimal[] source, EmaOptions options)
		{
			decimal?[] values = source.Select(x => (decimal?) x).ToArray();
			return Get(values, options);
		}
		
		public override DefaultIndicatorResult Get(decimal?[] source, EmaOptions options)
		{
			decimal?[] result = new decimal?[source.Length];
			decimal multiplier = 2 / (options.Period + 1);
			
			for (int i = 0; i < source.Length; i++)
			{
				if (i >= options.Period - 1)
				{
					if (result[i - 1].HasValue)
					{
						decimal? emaPrev = result[i - 1].Value;
						result[i] = (source[i] * multiplier) + emaPrev * (1 - multiplier);
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
			
			return new DefaultIndicatorResult() { Result = result};
		}
	}
}