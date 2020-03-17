namespace TechanCore.Strategies.Options
{
	public class AwesomeSmaStrategyOptions : IStrategyOption
	{
		public int AwesomeFastPeriod { get; set; }
		public int AwesomeSlowPeriod { get; set; }

		public int SmaFastPeriod { get; set; }
		public int SmaSlowPeriod { get; set; }
	}
}