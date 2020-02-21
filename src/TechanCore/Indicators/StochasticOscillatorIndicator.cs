using System;
using System.Linq;
using Contracts;
using TechanCore.Helpers;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
	public class StochasticOscillatorIndicator : BaseIndicator<StochasticOscillatorOptions, StochasticOscillatorResult>
	{
		public override string Name { get; } = "Stochastic Oscillator (SO) Indicator";
		
		public override StochasticOscillatorResult Get(ICandle[] source, StochasticOscillatorOptions options)
		{
			decimal[] closeValues = source.Close();
			Span<decimal> values = new Span<decimal>(closeValues);
			decimal?[] k = new decimal?[source.Length];

			int index = 0; 
			for (int i = 0; options.Period < closeValues.Length; i++)
			{
				decimal[] valuesForPeriod = values.Slice(index, options.Period).ToArray();
				decimal minValue = valuesForPeriod.Min();
				decimal maxValue = valuesForPeriod.Max();

				k[i] = 100 * (closeValues[i] - minValue) / (maxValue - minValue);  

				index++;
			}

			decimal?[] d = k.Ema(options.EmaPeriod).Result;

			return new StochasticOscillatorResult { K = k, D = d };
		}

		public override StochasticOscillatorResult Get(decimal[] source, StochasticOscillatorOptions options)
		{
			throw new NotImplementedException();
		}

		public override StochasticOscillatorResult Get(decimal?[] source, StochasticOscillatorOptions options)
		{
			throw new NotImplementedException();
		}
	}
}