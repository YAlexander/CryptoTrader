using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Abstractions.Enums
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum Assets
	{
		/// <summary>
		/// Bitcoin
		/// </summary>
		[EnumMember(Value = "BTC")]
		BTC = 0,

		/// <summary>
		/// Ethereum
		/// </summary>
		[EnumMember(Value = "ETH")]
		ETH = 1,

		/// <summary>
		/// Ripple
		/// </summary>
		[EnumMember(Value = "XRP")]
		XRP = 2,

		/// <summary>
		/// Bitcoin Cash
		/// </summary>
		[EnumMember(Value = "BCH")]
		BCH = 3,

		/// <summary>
		/// EOS
		/// </summary>
		[EnumMember(Value = "EOS")]
		EOS = 4,

		/// <summary>
		/// Litecoin
		/// </summary>
		[EnumMember(Value = "LTC")]
		LTC = 5,

		/// <summary>
		/// Stellar
		/// </summary>
		[EnumMember(Value = "XLM")]
		XLM = 6,

		/// <summary>
		/// Tether
		/// </summary>
		[EnumMember(Value = "USDT")]
		USDT = 7,

		/// <summary>
		/// Monero
		/// </summary>
		[EnumMember(Value = "XMR")]
		XMR = 8,

		/// <summary>
		/// Dash
		/// </summary>
		[EnumMember(Value = "DASH")]
		DASH = 9,

		/// <summary>
		/// Binance Coin
		/// </summary>
		[EnumMember(Value = "BNB")]
		BNB = 10,

		/// <summary>
		/// Ethereum Classic
		/// </summary>
		[EnumMember(Value = "ETC")]
		ETC = 11,

		/// <summary>
		/// TrueUSD
		/// </summary>
		[EnumMember(Value = "TUSD")]
		TUSD = 12,

		/// <summary>
		/// USD Coin
		/// </summary>
		[EnumMember(Value = "USDC")]
		USDC = 13
	}
}