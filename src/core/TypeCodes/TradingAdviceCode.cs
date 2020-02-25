using core.Abstractions.TypeCodes;

namespace core.TypeCodes
{
	public class TradingAdviceCode : TypeCodeBase<int, TradingAdviceCode>, ITradingAdviceCode
	{
		public TradingAdviceCode (int code, string description) : base(code, description)
		{
		}

		public static TradingAdviceCode SELL { get; } = new TradingAdviceCode(-1, "SELL");
		public static TradingAdviceCode HOLD { get; } = new TradingAdviceCode(0, "HOLD");
		public static TradingAdviceCode BUY { get; } = new TradingAdviceCode(1, "BUY");
	}
}
