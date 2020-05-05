using TechanCore.Enums;

namespace TechanCore.Strategies.Options
{
	public class MaStochRsiStrategyOptions : IStrategyOption
	{
		public int StochPeriod { get; set; }
		
		public int StochEmaPeriod { get; set; }
		
		public MaTypes MaType { get; set; }
		public int MaPeriod { get; set; }
		
		public int RsiPeriod { get; set; }
	}
}