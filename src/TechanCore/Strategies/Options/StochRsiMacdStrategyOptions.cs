using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class StochRsiMacdStrategyOptions : IStrategyOption
	{
		public int StochPeriod { get; set; }
		public int StochEmaPeriod { get; set; }
		
		public int MacdFastSmaPeriod { get; set; }
		public int  MacdSlowSmaPeriod { get; set; }
		public int MacdSignalPeriod { get; set; }
	}
}