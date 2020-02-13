using System;
using System.Linq;
using Contracts;
using Core.Trading.Extensions;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators
{
	public class RsiIndicator : BaseIndicator<RsiOptions, DefaultIndicatorResult>
	{
		public override string Name { get; } = "Relative Strength Index (RSI) Indicator";
		
		public override DefaultIndicatorResult Get(ICandle[] source, RsiOptions options)
		{
			decimal?[] values = source.Select(x => (decimal?)x.Close).ToArray();
			return Get(values, options);
		}

		public override DefaultIndicatorResult Get(decimal[] source, RsiOptions options)
		{
			decimal?[] values = source.Select(x => (decimal?)x).ToArray();
			return Get(values, options);
		}

		public override DefaultIndicatorResult Get(decimal?[] source, RsiOptions options)
		{
			decimal?[] rsi = new decimal?[source.Length];
			decimal?[] gain = new decimal?[source.Length];
			decimal?[] loss = new decimal?[source.Length];
			
			for (int i = 1; i < source.Length; i++)
			{
				if (source[i] > source[i - 1])
				{
					gain[i] = source[i] - source[i - 1];
				}
				else
				{
					loss[i] = source[i - 1] - source[i];
				}
			}
			
			int index = 0;
			for (int i = options.Period; i < source.Length; i++)
			{
				Range range = new Range(index, index + options.Period);
				
				decimal?[] gainForPeriod = gain[range];
				decimal?[] lossForPeriod = loss[range];
				
				decimal? cu = gainForPeriod.Ema(options.Period).LastOrDefault();
				decimal? cd = lossForPeriod.Ema(options.Period).LastOrDefault();

				rsi[i] = 100 - 100 / (1 + cu / cd);

				index++;
			}
			
			return new DefaultIndicatorResult() { Result = rsi };
		}
	}
}