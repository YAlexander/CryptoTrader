using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
	public static class EmaExtension
	{
		public static SeriesIndicatorResult Ema (this IEnumerable<ICandle> source, int period, CandleVariables type)
		{
			EmaIndicator ema = new EmaIndicator();
			EmaOptions options = new EmaOptions { Period = period, CandleVariable = type};
			
			return ema.Get(source.ToArray(), options);
		}
		
		public static SeriesIndicatorResult Ema (this IEnumerable<decimal> source, int period)
		{
			EmaIndicator ema = new EmaIndicator();
			EmaOptions options = new EmaOptions { Period = period, CandleVariable = null};
			
			return ema.Get(source.ToArray(), options);
		}
		
		public static SeriesIndicatorResult Ema (this IEnumerable<decimal?> source, int period)
		{
			EmaIndicator ema = new EmaIndicator();
			EmaOptions options = new EmaOptions { Period = period, CandleVariable = null};
			
			return ema.Get(source.ToArray(), options);
		}
	}
}