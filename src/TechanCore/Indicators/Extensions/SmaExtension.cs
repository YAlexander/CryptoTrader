using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
	public static class SmaExtension
	{
		public static SeriesIndicatorResult Sma (this IEnumerable<ICandle> source, int period, CandleVariables type)
		{
			SmaIndicator sma = new SmaIndicator();
			SmaOptions options = new SmaOptions { Period = period, CandleVariable = type};
			
			return sma.Get(source.ToArray(), options);
		}
		
		public static SeriesIndicatorResult Sma (this IEnumerable<decimal> source, int period)
		{
			SmaIndicator sma = new SmaIndicator();
			SmaOptions options = new SmaOptions { Period = period, CandleVariable = null};
			
			return sma.Get(source.ToArray(), options);
		}
		
		public static SeriesIndicatorResult Sma (this IEnumerable<decimal?> source, int period)
		{
			SmaIndicator sma = new SmaIndicator();
			SmaOptions options = new SmaOptions { Period = period, CandleVariable = null};
			
			return sma.Get(source.ToArray(), options);
		}
	}
}
