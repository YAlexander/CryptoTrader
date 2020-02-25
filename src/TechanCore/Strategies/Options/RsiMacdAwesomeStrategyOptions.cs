using Contracts.Trading;

namespace TechanCore.Strategies.Options
{
	public class RsiMacdAwesomeStrategyOptions : IStrategyOption
	{
		public int MacdFastPeriod { get; set; }
		public int MacdSlowPeriod { get; set; }
		public int MacdSignalPeriod { get; set; }
		
		public int RsiPeriod { get; set; }
		
		public int AwesomeFastPeriod { get; set; }
		public int AwesomeSlowPeriod { get; set; }
	}
}