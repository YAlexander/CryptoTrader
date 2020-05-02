using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TechanCore.Enums
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum Timeframes
	{
		[EnumMember(Value = "MINUTE")]
		MINUTE = 1,

		[EnumMember(Value = "FIVE_MINUTES")]
		FIVE_MINUTES = 5,

		[EnumMember(Value = "QUARTER_HOUR")]
		QUARTER_HOUR = 15,

		[EnumMember(Value = "HALF_HOUR")]
		HALF_HOUR = 30,

		[EnumMember(Value = "HOUR")]
		HOUR = 60,

		[EnumMember(Value = "FOUR_HOUR")]
		FOUR_HOUR = 240,

		[EnumMember(Value = "DAY")]
		DAY = 1440,

		[EnumMember(Value = "WEEK")]
		WEEK = 10080
	}
}