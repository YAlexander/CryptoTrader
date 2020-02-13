using System;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Indicators;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace core.Trading.TAIndicators
{
	public class SmaIndicator : BaseIndicator<SmaOptions, DefaultIndicatorResult>
	{
		public override string Name { get; } = "Simple Moving Average (SMA) Indicator";
		
		public override DefaultIndicatorResult Get(ICandle[] source, SmaOptions options)
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

		public override DefaultIndicatorResult Get(decimal[] source, SmaOptions options)
		{
			decimal?[] result = new decimal?[source.Length];
			Span<decimal> sma = new Span<decimal>(source);

			int index = 0;
			for (int i = options.Period - 1; i < source.Length; i++)
			{
				result[i] = sma.Slice(index, options.Period).ToArray().Average();
				index++;
			}
			
			return new DefaultIndicatorResult() { Result = result};
		}
	}
}