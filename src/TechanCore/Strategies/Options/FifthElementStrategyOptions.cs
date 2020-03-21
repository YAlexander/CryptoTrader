namespace TechanCore.Strategies.Options
{
	public class FifthElementStrategyOptions : IStrategyOption
	{
		public int FastPeriod { get; set; }
		public int SlowPeriod { get; set; }
		public int SignalPeriod { get; set; }
	}
}