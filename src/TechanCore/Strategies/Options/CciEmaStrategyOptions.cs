namespace TechanCore.Strategies.Options
{
	public class CciEmaStrategyOptions : IStrategyOption
	{
		public int CciPeriod { get; set; }
		public int EmaFast { get; set; }
		public int EmaSlow { get; set; }
	}
}