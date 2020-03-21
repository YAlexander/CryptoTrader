using System.Collections.Generic;
using System.Linq;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
	public static class StochasticOscillatorExtension
	{
		public static StochasticOscillatorResult StochasticOscillator (this IEnumerable<ICandle> source, int soPeriod, int emaPeriod)
		{
			StochasticOscillatorIndicator so = new StochasticOscillatorIndicator();
			StochasticOscillatorOptions options = new StochasticOscillatorOptions { Period = soPeriod, EmaPeriod = emaPeriod };
			return so.Get(source.ToArray(), options);
		}
	}
}