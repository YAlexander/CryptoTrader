using TechanCore.Enums;

namespace TechanCore.Strategies.Options
{
	public class MaAdxMacdOptions : IStrategyOption
	{
		public int MacdFastPeriod { get; set; }
		public int MacdSlowPeriod { get; set; }
		public int MacdSignalPeriod { get; set; }

		public MaTypes MaType { get; set; }
		public int FastMaPeriod { get; set; }
		public int SlowMaPeriod { get; set; }
		
		public int AdxPeriod { get; set; }
	}
}