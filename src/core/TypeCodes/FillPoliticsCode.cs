using core.Abstractions.TypeCodes;

namespace core.TypeCodes
{
	public class FillPoliticsCode : TypeCodeBase<int, FillPoliticsCode>, IFillPoliticsCode
	{
		public FillPoliticsCode (int code, string description) : base(code, description)
		{
		}

		public static FillPoliticsCode GTC { get; } = new FillPoliticsCode(0, "GOOD TILL CANCELLED");

		// Must be fully filled, else cancelled
		public static FillPoliticsCode FOK { get; } = new FillPoliticsCode(1, "FILL OR KILL");

		// Fill max available, rest cancelled
		public static FillPoliticsCode IOC { get; } = new FillPoliticsCode(2, "IMMEDIATE OR CANCEL");
	}
}
