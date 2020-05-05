using TechanCore.Enums;

namespace TechanCore.Strategies.Options
{
	public class AdxMasStrategyOptions : IStrategyOption
	{
		public MaTypes MaType { get; set; }

		public int FastMaPeriod { get; set; }
		public int SlowMaPeriod { get; set; }
		
		public int AdxPeriod { get; set; }
	}
}