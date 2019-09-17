using core.Abstractions.TypeCodes;

namespace core.TypeCodes
{
	public class PeriodCode : TypeCodeBase<int, PeriodCode>, IPeriodCode
	{
		public PeriodCode (int code, string description) : base(code, description)
		{
		}

		public static PeriodCode UNKNOWN { get; } = new PeriodCode(0, "UNKNOWN");
		public static PeriodCode MINUTE { get; } = new PeriodCode(1, "MINUTE");
		public static PeriodCode FIVE_MINUTES { get; } = new PeriodCode(5, "FIVE_MINUTES");
		public static PeriodCode QUARTER_HOUR { get; } = new PeriodCode(15, "QUARTER_HOUR");
		public static PeriodCode HALF_HOUR { get; } = new PeriodCode(30, "HALF_HOUR");
		public static PeriodCode HOUR { get; } = new PeriodCode(60, "HOUR");
		public static PeriodCode FOUR_HOUR { get; } = new PeriodCode(240, "FOUR_HOUR");
		public static PeriodCode DAY { get; } = new PeriodCode(1440, "DAY");
	}
}
