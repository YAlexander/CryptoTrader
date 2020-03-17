namespace TechanCore.Strategies.Options
{
	public class ThreeMAgosStrategyOptions : IStrategyOption
	{
		public int SmaPeriod { get; set; }
		public int EmaPeriod { get; set; }
		public int WmaPeriod { get; set; }
	}
}