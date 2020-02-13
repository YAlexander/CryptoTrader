using System;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
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
			Span<decimal?> values = new Span<decimal?>(source);

			decimal a = 2 / (options.Period + 1);

			int index = 0;
			for (int i = options.Period; i < source.Length; i++)
			{
				if (index == 0)
				{
					result[i] = values.Slice(index, options.Period).ToArray().Average();
				}
				else
				{
					result[i] = a * values[i] + (1 - a) * result[i - 1];
				}

				index++;
			}
			
			return new DefaultIndicatorResult() { Result = result};
		}
	}
}