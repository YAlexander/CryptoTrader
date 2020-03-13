using System;
using TechanCore.Enums;

namespace TechanCore.Indicators.Results
{
    public class RenkoBrick
    {
        public RenkoBrickType Type { get; set; }
        public DateTime Time { get; set; }
        public decimal Price { get; set; }
        
        public decimal High { get; set; }
        public decimal Low { get; set; }
    }
}