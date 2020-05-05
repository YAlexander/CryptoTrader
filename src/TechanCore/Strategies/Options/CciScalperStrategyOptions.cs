using TechanCore.Enums;

namespace TechanCore.Strategies.Options
{
	public class CciScalperStrategyOptions : IStrategyOption
	{
		public int CciPeriod { get; set; }
		public MaTypes MaType { get; set; }
		public int FastMaPeriod { get; set; }
		public int NormalMaPeriod { get; set; }
		public int SlowMaPeriod { get; set; }
	}
}