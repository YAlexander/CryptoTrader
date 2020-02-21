using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Enums;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
    public static class TemaExtension
    {
        public static SeriesIndicatorResult Tema (this IEnumerable<ICandle> source, int period, CandleVariables type)
        {
            TripleExponentialMovingAverageIndicator tema = new TripleExponentialMovingAverageIndicator();
            TemaOptions options = new TemaOptions { Period = period, CandleVariable = type};
			
            return tema.Get(source.ToArray(), options);
        }
		
        public static SeriesIndicatorResult Tema (this IEnumerable<decimal> source, int period)
        {
            TripleExponentialMovingAverageIndicator tema = new TripleExponentialMovingAverageIndicator();
            TemaOptions options = new TemaOptions { Period = period, CandleVariable = null};
			
            return tema.Get(source.ToArray(), options);
        }
		
        public static SeriesIndicatorResult Tema (this IEnumerable<decimal?> source, int period)
        {
            TripleExponentialMovingAverageIndicator tema = new TripleExponentialMovingAverageIndicator();
            TemaOptions options = new TemaOptions { Period = period, CandleVariable = null};
			
            return tema.Get(source.ToArray(), options);
        }
    }
}