using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class DoubleVolatilityStrategyOptions : IStrategyOption
	{
		public int FastSmaPeriod { get; set; }
		public int NormalSmaPeriod { get; set; }
		public int SlowSmaPeriod { get; set; }
		
		public int RsiPeriod { get; set; }
	}
}