using Contracts.Enums;
using Contracts.Trading;

namespace core.Trading.TAIndicators.Options
{
    public class MovingAverageEnvelopesOptions : IOptionsSet
    {
        public int Period { get; set; }

        /// <summary>
        /// Should be less then 1
        /// </summary>
        public double UpwardFactor { get; set; }

        /// <summary>
        /// Should be less then 1
        /// </summary>
        public double DownwardFactor { get; set; }
        
        public CandleVariables? CandleVariable { get; set; } 
    }
}