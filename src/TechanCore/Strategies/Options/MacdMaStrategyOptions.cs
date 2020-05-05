using TechanCore.Enums;

namespace TechanCore.Strategies.Options
{
	public class MacdMaStrategyOptions : IStrategyOption
	{
		public int MacdFastPeriod { get; set; }
		public int MacdSlowPeriod { get; set; }
		public int MacdSignalPeriod { get; set; }
		
		public MaTypes MaType { get; set; }
		public int FastMaPeriod { get; set; }
		public int SlowMaPeriod { get; set; }
		public int NormalMaPeriod { get; set; }
	}
}