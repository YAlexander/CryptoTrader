using core.Abstractions.TypeCodes;

namespace core.TypeCodes
{
	public class TradingModeCode : TypeCodeBase<int, TradingModeCode>, ITradingModeCode
	{
		public TradingModeCode (int code, string description) : base(code, description)
		{
		}

		public static TradingModeCode AUTO { get; } = new TradingModeCode(0, "AUTOMATIC");
		public static TradingModeCode MANUAL { get; } = new TradingModeCode(1, "MANUAL");
	}
}
