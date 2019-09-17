using core.Abstractions.TypeCodes;

namespace core.TypeCodes
{
	public class CandleVariableCode : TypeCodeBase<int, CandleVariableCode>, ICandleVariableCode
	{
		public CandleVariableCode (int code, string description) : base(code, description)
		{
		}

		public static CandleVariableCode UNKNOWN { get; } = new CandleVariableCode(0, "UNKNOWN");

		public static CandleVariableCode HIGH { get; } = new CandleVariableCode(1, "HIGH");

		public static CandleVariableCode LOW { get; } = new CandleVariableCode(2, "LOW");

		public static CandleVariableCode CLOSE { get; } = new CandleVariableCode(3, "CLOSE");

		public static CandleVariableCode OPEN { get; } = new CandleVariableCode(4, "OPEN");
	}
}
