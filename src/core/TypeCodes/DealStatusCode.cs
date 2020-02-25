using core.Abstractions.TypeCodes;

namespace core.TypeCodes
{
	public class DealStatusCode : TypeCodeBase<int, DealStatusCode>, IDealStatusCode
	{
		public DealStatusCode (int code, string description) : base(code, description)
		{
		}

		public static DealStatusCode OPEN { get; } = new DealStatusCode(0, "OPEN");
		public static DealStatusCode CLOSE { get; } = new DealStatusCode(1, "CLOSE");
		public static DealStatusCode ERROR { get; } = new DealStatusCode(2, "ERROR");
	}
}
