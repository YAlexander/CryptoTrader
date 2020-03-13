using System.Collections.Generic;
using Contracts.Trading;

namespace TechanCore.Indicators.Results
{
    public class RenkoResult : IResultSet
    {
        public List<RenkoBrick> Bricks { get; set; }
    }
}