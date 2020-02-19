using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class MomentumStrategyOptions : IStrategyOption
	{
		public int FastSmaPeriod { get; set; }
		public int SlowSmaPeriod { get; set; }
		public int MomentumPeriod { get; set; }
		public int RsiPeriod { get; set; }
	}
}