namespace TechanCore.Strategies.Options
{
	public class RsiMacdStrategyOptions : IStrategyOption
	{
		public int MacdFastSmaPeriod { get; set; }
		public int  MacdSlowSmaPeriod { get; set; }
		public int MacdSignalPeriod { get; set; }
		public int RsiPeriod { get; set; }
	}
}