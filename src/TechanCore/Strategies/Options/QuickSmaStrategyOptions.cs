namespace TechanCore.Strategies.Options
{
	public class QuickSmaStrategyOptions : IStrategyOption
	{
		public int FastSmaPeriod { get; set; }
		public int SlowSmaPeriod { get; set; }
	}
}