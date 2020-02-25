using core.Abstractions.TypeCodes;

namespace core.TypeCodes
{
	public class ActionCode : TypeCodeBase<int, ActionCode>, IActionCode
	{
		public ActionCode (int code, string description) : base(code, description)
		{
		}

		public static ActionCode CREATED { get; } = new ActionCode(1, "Item Created");

		public static ActionCode UPDATED { get; } = new ActionCode(2, "Item Updated");

		public static ActionCode DELETED { get; } = new ActionCode(3, "Item Deleted");
		
		public static ActionCode FORECAST { get; } = new ActionCode(4, "Forecast");
	}
}
