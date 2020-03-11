using System.Collections.Generic;
using Contracts;
using Contracts.Trading;

namespace TechanCore.Indicators.Results
{
    public class HeikenAshiResult : IResultSet
    {
        public IEnumerable<ICandle> Candles { get; set; }
    }
}