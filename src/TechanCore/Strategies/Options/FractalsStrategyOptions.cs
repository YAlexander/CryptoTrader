namespace TechanCore.Strategies.Options
{
	public class FractalsStrategyOptions : IStrategyOption
	{
		public int AwesomeFastPeriod { get; set; }
		public int AwesomeSlowPeriod { get; set; }
		
		public int ExitAfterBarsCount { get; set; }
	}
}