using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class EmaAdxMacdOptions : IStrategyOption
	{
		public int MacdFastPeriod { get; set; }
		public int MacdSlowPeriod { get; set; }
		public int MacdSignalPeriod { get; set; }

		public int FastEmaPeriod { get; set; }
		public int SlowEmaPeriod { get; set; }
		
		public int AdxEmaPeriod { get; set; }
	}
}