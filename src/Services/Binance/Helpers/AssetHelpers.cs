using System;
using System.Collections.Generic;
using Contracts.Enums;

namespace Binance.Helpers
{
	public static class AssetHelpers
	{
		public static string GetAssetCode(Assets asset)
		{
			if (ExchangeAssetCodes.ContainsKey(asset))
			{
				return ExchangeAssetCodes[asset];
			}
			
			throw new Exception($"Unsupported asset {asset}");
		}
		
		private static readonly Dictionary<Assets, string> ExchangeAssetCodes = new Dictionary<Assets, string>()
		{
			[Assets.BCH] = "BCH",
			[Assets.BNB] = "BNB",
			[Assets.BTC] = "BTC",
			[Assets.EOS] = "EOC",
			[Assets.ETC] = "ETC",
			[Assets.ETH] = "ETH",
			[Assets.LTC] = "LTC",
			[Assets.XLM] = "XLM",
			[Assets.XMR] = "XMR",
			[Assets.XRP] = "XRP",
			[Assets.DASH] = "DASH",
			[Assets.TUSD] = "TUSD",
			[Assets.USDC] = "USDC",
			[Assets.USDT] = "USDT"
		};
	}
}