namespace TechanCore.Strategies.Options
{
	public class CciScalperStrategyOptions : IStrategyOption
	{
		public int CciPeriod { get; set; }
		public int FastEmaPeriod { get; set; }
		public int NormalEmaPeriod { get; set; }
		public int SlowEmaPeriod { get; set; }
	}
}