namespace TechanCore.Strategies.Options
{
	public class BigTreeStrategyOptions : IStrategyOption
	{
		public int VeryFastSmaPeriod { get; set; }
		public int FastSmaPeriod { get; set; }
		public int SlowSmaPeriod { get; set; }
	}
}