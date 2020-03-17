namespace TechanCore.Strategies.Options
{
	public class BreakoutMaOptions : IStrategyOption
	{
		public int SmaPeriod { get; set; }
		public int EmaPeriod { get; set; }
		public int AdxPeriod { get; set; }
	}
}