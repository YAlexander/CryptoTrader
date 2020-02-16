using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
    public static class MovingAverageEnvelopesExtension
    {
        public static MovingAverageEnvelopesResult MovingAverageEnvelopes (this IEnumerable<ICandle> source, int period, CandleVariables type, double upwardFactor, double downwardFactor)
        {
            MovingAverageEnvelopesIndicator sma = new MovingAverageEnvelopesIndicator();
            MovingAverageEnvelopesOptions options = new MovingAverageEnvelopesOptions
            {
                Period = period, 
                CandleVariable = type,
                UpwardFactor = upwardFactor,
                DownwardFactor = downwardFactor
            };
			
            return sma.Get(source.ToArray(), options);
        }
		
        public static MovingAverageEnvelopesResult Sma (this IEnumerable<decimal> source, int period, double upwardFactor, double downwardFactor )
        {
            MovingAverageEnvelopesIndicator sma = new MovingAverageEnvelopesIndicator();
            MovingAverageEnvelopesOptions options = new MovingAverageEnvelopesOptions
            {
                Period = period, 
                CandleVariable = null,
                UpwardFactor = upwardFactor,
                DownwardFactor = downwardFactor
            };

            return sma.Get(source.ToArray(), options);
        }
		
        public static MovingAverageEnvelopesResult Sma (this IEnumerable<decimal?> source, int period, double upwardFactor, double downwardFactor)
        {
            MovingAverageEnvelopesIndicator sma = new MovingAverageEnvelopesIndicator();
            MovingAverageEnvelopesOptions options = new MovingAverageEnvelopesOptions
            {
                Period = period, 
                CandleVariable = null,
                UpwardFactor = upwardFactor,
                DownwardFactor = downwardFactor
            };
			
            return sma.Get(source.ToArray(), options);
        }
    }
}