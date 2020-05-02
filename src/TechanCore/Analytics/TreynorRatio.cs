using TechanCore.Analytics.Options;
using TechanCore.Analytics.Result;

namespace TechanCore.Analytics
{
	public class TreynorRatio : IAnalyzer<TreynorRatioOptions, DefaultAnalyticsResult>
	{
		public DefaultAnalyticsResult Get(TreynorRatioOptions options)
		{
			float ratio = (options.PortfolioReturn - options.RiskFreeRate) / options.Beta;

			return new DefaultAnalyticsResult { Result = ratio };
		}
	}
}
