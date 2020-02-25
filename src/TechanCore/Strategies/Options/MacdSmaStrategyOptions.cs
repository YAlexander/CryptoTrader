using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class MacdSmaStrategyOptions : IStrategyOption
	{
		public int MacdFastPeriod { get; set; }
		public int MacdSlowPeriod { get; set; }
		public int MacdSignalPeriod { get; set; }
		
		public int FastSmaPeriod { get; set; }
		public int SlowSmaPeriod { get; set; }
		public int SmaPeriod { get; set; }
	}
}