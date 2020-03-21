namespace TechanCore.Strategies.Options
{
	public class RsiSmaCrossoverStrategyOptions : IStrategyOption
	{
		public int SmaFastPeriod { get; set; }
		public int SmaSlowPeriod { get; set; }
		public int RsiPeriod { get; set; }
	}
}