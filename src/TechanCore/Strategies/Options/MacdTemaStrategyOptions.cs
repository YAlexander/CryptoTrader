namespace TechanCore.Strategies.Options
{
	public class MacdTemaStrategyOptions : IStrategyOption
	{
		public int MacdFastPeriod { get; set; }
		public int MacdSlowPeriod { get; set; }
		public int MacdSignalPeriod { get; set; }
		
		public int TemaPeriod { get; set; }
	}
}