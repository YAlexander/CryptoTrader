using Contracts.TypeCodes;

namespace Core.TypeCodes
{
	public class CandleVariableCode : TypeCodeBase<int, CandleVariableCode>, ICandleVariableCode
	{
		private CandleVariableCode (int code, string description) : base(code, description)
		{
		}

		public static CandleVariableCode Unknown { get; } = new CandleVariableCode(0, "UNKNOWN");

		public static CandleVariableCode High { get; } = new CandleVariableCode(1, "HIGH");

		public static CandleVariableCode Low { get; } = new CandleVariableCode(2, "LOW");

		public static CandleVariableCode Close { get; } = new CandleVariableCode(3, "CLOSE");

		public static CandleVariableCode Open { get; } = new CandleVariableCode(4, "OPEN");
	}
}
