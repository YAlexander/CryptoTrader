using System.Collections.Generic;

namespace TechanCore.Indicators.Results
{
    public class HeikenAshiResult : IResultSet
    {
        public IEnumerable<ICandle> Candles { get; set; }
    }
}