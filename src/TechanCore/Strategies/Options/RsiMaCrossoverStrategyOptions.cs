using TechanCore.Enums;

namespace TechanCore.Strategies.Options
{
	public class RsiMaCrossoverStrategyOptions : IStrategyOption
	{
		public MaTypes MaType { get; set; }
		public int MaFastPeriod { get; set; }
		public int MaSlowPeriod { get; set; }
		public int RsiPeriod { get; set; }
	}
}