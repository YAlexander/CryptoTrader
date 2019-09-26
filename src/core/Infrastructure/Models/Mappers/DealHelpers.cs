using core.Abstractions.TypeCodes;
using core.TypeCodes;

namespace core.Infrastructure.Models.Mappers
{
	public static class DealHelpers
	{
		public static IDealStatusCode ToDealCode (this IOrderStatusCode code)
		{
			if (code == (IOrderStatusCode)OrderStatusCode.PENDING || code == (IOrderStatusCode)OrderStatusCode.HOLD || code == (IOrderStatusCode)OrderStatusCode.LISTED)
			{
				return DealStatusCode.OPEN;
			}

			if (code == (IOrderStatusCode)OrderStatusCode.CANCELED || code == (IOrderStatusCode)OrderStatusCode.CLOSED || code == (IOrderStatusCode)OrderStatusCode.FILLED || code == (IOrderStatusCode)OrderStatusCode.PARTIALLY_FILLED)
			{
				return DealStatusCode.CLOSE;
			}

			return DealStatusCode.ERROR;
		}

		public static IDealStatusCode ToDealCode (this OrderStatusCode code)
		{
			return ((IOrderStatusCode)code).ToDealCode();
		}
	}
}
