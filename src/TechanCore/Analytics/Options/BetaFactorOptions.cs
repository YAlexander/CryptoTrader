namespace TechanCore.Analytics.Options
{
	public class BetaFactorOptions : IOptionsSet
	{
		public decimal[] AssetPrices { get; set; }
		public decimal[] SafeAssetPrice { get; set; }
	}
}
