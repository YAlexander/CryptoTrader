using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Abstractions.Enums
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum Exchanges
	{
		[EnumMember(Value = "BINANCE")]
		BINANCE,

		[EnumMember(Value = "BITMEX")]
		BITMEX,

		[EnumMember(Value = "QUANTFURY")]
		QUANTFURY
	}
}