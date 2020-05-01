using System;
using TechanCore.Analytics.Options;
using TechanCore.Analytics.Result;

namespace TechanCore.Analytics
{
	public class ProfitFactor : IAnalyzer<ProfitFactorOptions, DefaultAnalyticsResult>
	{
		public DefaultAnalyticsResult Get(ProfitFactorOptions options)
		{
			int unprofitableDealsCount = 0;
			int profitableDealsCount = 0;

			if (unprofitableDealsCount == 0)
			{
				throw new Exception("Unable to calculate Profit Factor");
			}

			return new DefaultAnalyticsResult { Result = unprofitableDealsCount / profitableDealsCount };
		}
	}
}
