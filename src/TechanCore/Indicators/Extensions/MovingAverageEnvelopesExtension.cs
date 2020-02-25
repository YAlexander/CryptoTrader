using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Enums;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
    public static class MovingAverageEnvelopesExtension
    {
        public static MovingAverageEnvelopesResult MovingAverageEnvelopes (this IEnumerable<ICandle> source, int period, CandleVariables type, double upwardFactor, double downwardFactor)
        {
            MovingAverageEnvelopesIndicator mae = new MovingAverageEnvelopesIndicator();
            MovingAverageEnvelopesOptions options = new MovingAverageEnvelopesOptions
            {
                Period = period, 
                CandleVariable = type,
                UpwardFactor = upwardFactor,
                DownwardFactor = downwardFactor
            };
			
            return mae.Get(source.ToArray(), options);
        }
		
        public static MovingAverageEnvelopesResult MovingAverageEnvelopes (this IEnumerable<decimal> source, int period, double upwardFactor, double downwardFactor )
        {
            MovingAverageEnvelopesIndicator mae = new MovingAverageEnvelopesIndicator();
            MovingAverageEnvelopesOptions options = new MovingAverageEnvelopesOptions
            {
                Period = period, 
                CandleVariable = null,
                UpwardFactor = upwardFactor,
                DownwardFactor = downwardFactor
            };

            return mae.Get(source.ToArray(), options);
        }
		
        public static MovingAverageEnvelopesResult MovingAverageEnvelopes (this IEnumerable<decimal?> source, int period, double upwardFactor, double downwardFactor)
        {
            MovingAverageEnvelopesIndicator mae = new MovingAverageEnvelopesIndicator();
            MovingAverageEnvelopesOptions options = new MovingAverageEnvelopesOptions
            {
                Period = period, 
                CandleVariable = null,
                UpwardFactor = upwardFactor,
                DownwardFactor = downwardFactor
            };
			
            return mae.Get(source.ToArray(), options);
        }
    }
}