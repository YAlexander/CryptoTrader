using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using core.Trading.TAIndicators;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
	public static class SmaExtension
	{
		public static DefaultIndicatorResult Sma (this IEnumerable<ICandle> source, int period, CandleVariables type)
		{
			SmaIndicator sma = new SmaIndicator();
			SmaOptions options = new SmaOptions() { Period = period, CandleVariable = type};
			
			return sma.Get(source.ToArray(), options);
		}
		
		public static DefaultIndicatorResult Sma (this IEnumerable<decimal> source, int period)
		{
			SmaIndicator sma = new SmaIndicator();
			SmaOptions options = new SmaOptions() { Period = period, CandleVariable = null};
			
			return sma.Get(source.ToArray(), options);
		}
		
		public static DefaultIndicatorResult Sma (this IEnumerable<decimal?> source, int period)
		{
			SmaIndicator sma = new SmaIndicator();
			SmaOptions options = new SmaOptions() { Period = period, CandleVariable = null};
			
			return sma.Get(source.ToArray(), options);
		}
	}
}
