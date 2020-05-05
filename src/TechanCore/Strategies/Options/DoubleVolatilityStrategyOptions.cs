using TechanCore.Enums;

namespace TechanCore.Strategies.Options
{
	public class DoubleVolatilityStrategyOptions : IStrategyOption
	{
		public MaTypes MaType { get; set; }
		public int FastMaPeriod { get; set; }
		public int NormalMaPeriod { get; set; }
		public int SlowMaPeriod { get; set; }
		
		public int RsiPeriod { get; set; }
	}
}