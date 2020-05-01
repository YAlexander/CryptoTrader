using TechanCore.Analytics.Options;
using TechanCore.Analytics.Result;
using System.Linq;

namespace TechanCore.Analytics
{
	public class BetaFactor : IAnalyzer<BetaFactorOptions, DefaultAnalyticsResult>
	{
		public DefaultAnalyticsResult Get(BetaFactorOptions options)
		{
			decimal avgAssetPrice = options.AssetPrices.Average();
			decimal avgSafeAssetPrice = options.SafeAssetPrice.Average();

			decimal sumA = 0;
			decimal sumB = 0;

			for (int i = 0; i < options.AssetPrices.Length; i++)
			{
				sumA += (options.AssetPrices[i] - avgAssetPrice) * (options.SafeAssetPrice[i] - avgSafeAssetPrice);
				sumB += (options.SafeAssetPrice[i] - avgSafeAssetPrice) * (options.SafeAssetPrice[i] - avgSafeAssetPrice);
			}

			return new DefaultAnalyticsResult { Result = (float)(sumA / sumB)};
		}
	}
}
