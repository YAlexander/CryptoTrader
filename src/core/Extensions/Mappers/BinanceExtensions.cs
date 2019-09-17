using Binance.Net.Objects;
using core.Abstractions.TypeCodes;
using core.TypeCodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace core.Extensions.Mappers
{
	public static class BinanceExtensions
	{
		public static IOrderSideCode ToCode (this OrderSide side)
		{
			switch (side)
			{
				case OrderSide.Buy:
					return OrderSideCode.BUY;

				case OrderSide.Sell:
					return OrderSideCode.SELL;

				default:
					throw new Exception($"Unknown order side {side}");
			}
		}

		public static OrderSide ToSide (this OrderSideCode code)
		{
			if (code == OrderSideCode.BUY)
			{
				return OrderSide.Buy;
			}
			else if (code == OrderSideCode.SELL)
			{
				return OrderSide.Sell;
			}
			else
			{
				throw new Exception($"Unknown code {code.ToString()}");
			}
		}

		public static IOrderTypeCode ToCode (this OrderType type)
		{
			switch (type)
			{
				case OrderType.Limit:
					return OrderTypeCode.LMT;

				case OrderType.Market:
					return OrderTypeCode.MKT;

				default:
					throw new Exception($"Unknown order type {type}");
			}
		}

		public static IFillPoliticsCode ToCode (this TimeInForce politics)
		{
			switch (politics)
			{
				case TimeInForce.FillOrKill:
					return FillPoliticsCode.FOK;

				case TimeInForce.GoodTillCancel:
					return FillPoliticsCode.GTC;

				case TimeInForce.ImmediateOrCancel:
					return FillPoliticsCode.IOC;

				default:
					throw new Exception($"Unknown order fill politics {politics}");
			}
		}

		public static IOrderStatusCode ToStatus(this OrderStatus status)
		{
			switch (status)
			{
				case OrderStatus.New:
					return OrderStatusCode.PENDING;

				case OrderStatus.PartiallyFilled:
					return OrderStatusCode.PARTIALLY_FILLED;

				case OrderStatus.Filled:
					return OrderStatusCode.FILLED;

				case OrderStatus.Canceled:
				case OrderStatus.PendingCancel:
					return OrderStatusCode.CANCELED;

				case OrderStatus.Rejected:
					return OrderStatusCode.ERROR;

				case OrderStatus.Expired:
					return OrderStatusCode.EXPIRED;

				default:
					throw new Exception($"Unknown status {status.ToString()}");
			}
		}
	}
}
