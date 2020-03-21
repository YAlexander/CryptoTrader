namespace TechanCore.Strategies.Options
{
	public class AwesomeMacdStrategyOptions : IStrategyOption
	{
		public int MacdFastPeriod { get; set; }
		public int MacdSlowPeriod { get; set; }
		public int MacdSignalPeriod { get; set; }
		
		public int AwesomeFastPeriod { get; set; }
		public int AwesomeSlowPeriod { get; set; }
	}
}