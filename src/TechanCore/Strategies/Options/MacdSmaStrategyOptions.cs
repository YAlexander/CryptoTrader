using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class MacdSmaStrategyOptions : IStrategyOption
	{
		public int FastPeriod { get; set; }
		public int SlowPeriod { get; set; }
		public int SignalPeriod { get; set; }
		
		public int FastSmaPeriod { get; set; }
		public int SlowSmaPeriod { get; set; }
		public int SmaPeriod { get; set; }
	}
}