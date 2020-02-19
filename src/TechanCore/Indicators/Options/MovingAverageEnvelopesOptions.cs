using System.ComponentModel.DataAnnotations;
using Contracts.Enums;
using Contracts.Trading;

namespace TechanCore.Indicators.Options
{
    public class MovingAverageEnvelopesOptions : IOptionsSet
    {
        public int Period { get; set; }

        /// <summary>
        /// Should be from 0 to 1
        /// </summary>
        public double UpwardFactor { get; set; }

        /// <summary>
        /// Should be from 0 to 1
        /// </summary>
        public double DownwardFactor { get; set; }
        
        public CandleVariables? CandleVariable { get; set; } 
    }
}