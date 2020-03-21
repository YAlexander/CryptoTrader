using System.Collections.Generic;

namespace TechanCore.Indicators.Results
{
    public class RenkoResult : IResultSet
    {
        public List<RenkoBrick> Bricks { get; set; }
    }
}