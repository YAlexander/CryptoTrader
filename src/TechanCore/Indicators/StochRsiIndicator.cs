using System;
using System.Linq;
using Contracts;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
	public class StochRsiIndicator : BaseIndicator<StochRsiOptions, SeriesIndicatorResult>
	{
		public override string Name { get; } = "Stochastics RSI (StochRsi) Indicator";
		
		public override SeriesIndicatorResult Get(ICandle[] source, StochRsiOptions options)
		{
			decimal?[] rsi = source.Rsi(options.RsiPeriod).Result;
			decimal?[] stochRsi = new decimal?[source.Length];

			int index = 0;
			for (int i = options.RsiPeriod; i < source.Length; i++)
			{
				decimal?[] valuesForPeriod = rsi.Skip(index).Take(options.RsiPeriod).ToArray();

				stochRsi[i] = (rsi[i] - valuesForPeriod.Min()) / (valuesForPeriod.Max() - valuesForPeriod.Min());

				index++;
			}
			
			return new SeriesIndicatorResult { Result = stochRsi };
		}

		public override SeriesIndicatorResult Get(decimal[] source, StochRsiOptions options)
		{
			throw new NotImplementedException();
		}

		public override SeriesIndicatorResult Get(decimal?[] source, StochRsiOptions options)
		{
			throw new NotImplementedException();
		}
	}
}