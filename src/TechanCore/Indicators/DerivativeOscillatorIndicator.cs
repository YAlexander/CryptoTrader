using System;
using System.Linq;
using Contracts;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
	public class DerivativeOscillatorIndicator : BaseIndicator<DerivativeOscillatorOptions, SeriesIndicatorResult>
	{
		public override string Name { get; } = "Derivative Oscillator (DO) Indicator";
		
		public override SeriesIndicatorResult Get(ICandle[] source, DerivativeOscillatorOptions options)
		{
			decimal?[] rsi = source.Rsi(options.RsiPeriod).Result;

			decimal?[] ema1Source = rsi.Select(x => x).ToArray();
			decimal?[] ema1 = ema1Source.Ema(options.EmaSlowPeriod).Result;
			decimal?[] ema2 = ema1.Ema(options.EmaFastPeriod).Result;
			decimal?[] sma = ema2.Sma(options.SmaPeriod).Result;

			decimal?[] result = new decimal?[sma.Length];

			for (int i = 0; i < sma.Length; i++)
			{
				if (sma[i] != null && ema2[i] != null)
				{
					result[i] = ema2[i] - sma[i];
				}
			}

			return new SeriesIndicatorResult() { Result = result};

		}

		public override SeriesIndicatorResult Get(decimal[] source, DerivativeOscillatorOptions options)
		{
			throw new NotImplementedException();
		}

		public override SeriesIndicatorResult Get(decimal?[] source, DerivativeOscillatorOptions options)
		{
			throw new NotImplementedException();
		}
	}
}



