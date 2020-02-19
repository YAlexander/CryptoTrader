using System;
using System.Linq;
using Contracts;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
	public class AwesomeOscillatorIndicator : BaseIndicator<AwesomeOscillatorOptions, SeriesIndicatorResult>
	{
		public override string Name { get; } = "Awesome Oscillator (AO) Indicator";
		
		public override SeriesIndicatorResult Get(ICandle[] source, AwesomeOscillatorOptions options)
		{
			decimal[] values = source.Select(x => (x.High + x.Low) / 2).ToArray();

			decimal?[] smaFastData = values.Sma(options.FastSmaPeriod).Result;
			decimal?[] smaSlowData = values.Sma(options.SlowSmaPeriod).Result;

			decimal?[] awesomeData = new decimal?[source.Length];

			for (int i = 0; i < source.Length; i++)
			{
				if (i > 0)
				{
					decimal? aoSecondLast = smaFastData[i - 1] - smaSlowData[i - 1];
					decimal? aoLast = smaFastData[i] - smaSlowData[i];

					if (aoSecondLast <= 0 && aoLast > 0)
					{
						awesomeData[i] = 100;
					}
					else if (aoSecondLast >= 0 && aoLast < 0)
					{
						awesomeData[i] = -100;
					}
					else
					{
						awesomeData[i] = 0;
					}
				}
				else
				{
					awesomeData[i] = 0;
				}
			}

			return new SeriesIndicatorResult { Result = awesomeData };
		}

		public override SeriesIndicatorResult Get(decimal[] source, AwesomeOscillatorOptions options)
		{
			throw new NotImplementedException();
		}

		public override SeriesIndicatorResult Get(decimal?[] source, AwesomeOscillatorOptions options)
		{
			throw new NotImplementedException();
		}
	}
}