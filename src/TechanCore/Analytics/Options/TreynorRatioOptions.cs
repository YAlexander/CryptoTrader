namespace TechanCore.Analytics.Options
{
	public class TreynorRatioOptions : IOptionsSet
	{
		public float PortfolioReturn { get; set; }
		public float RiskFreeRate { get; set; }
		public float Beta { get; set; }
	}
}
