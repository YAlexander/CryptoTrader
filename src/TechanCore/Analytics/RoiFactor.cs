using TechanCore.Analytics.Options;
using TechanCore.Analytics.Result;

namespace TechanCore.Analytics
{
	/// <summary>
	/// Return on Investment (ROI)
	/// </summary>
	public class RoiFactor : IAnalyzer<RoiOptions, DefaultAnalyticsResult>
	{
		public DefaultAnalyticsResult Get(RoiOptions options)
		{
			decimal roi = (options.ValueOfInvestment - options.CostOfInvestment) / options.CostOfInvestment * 100;

			return new DefaultAnalyticsResult { Result = (float)roi };
		}
	}
}
