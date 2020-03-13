using Contracts.Trading;
using TechanCore.Enums;

namespace TechanCore.Indicators.Options
{
    public class HeikenAshiOptions : IOptionsSet
    {
        public bool IsSmoothed { get; set; }
        public MaTypes MaType { get; set; }
        public int MaPeriod { get; set; }
    }
}