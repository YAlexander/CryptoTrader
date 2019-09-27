using Binance.Net.Objects;
using core.Extensions.Mappers;
using core.Infrastructure.Database.Entities;

namespace core.Infrastructure.Models.Mappers
{
	public static class OrderHelper
	{
		public static ExchangeOrder ToOrder (this BinancePlacedOrder externalOrder)
		{
			if (externalOrder == null)
			{
				return null;
			}

			ExchangeOrder order = new ExchangeOrder();
			order.Symbol = externalOrder.Symbol;
			order.Status = externalOrder.Status.ToStatus();
			order.OriginalQuantity = externalOrder.OriginalQuantity;
			order.OrderId = long.Parse(externalOrder.ClientOrderId);
			order.ExchangeOrderId = externalOrder.OrderId.ToString();
			order.OrderSideCode = externalOrder.Side.ToCode();
			order.OrderTypeCode = externalOrder.Type.ToCode();
			order.FillPoliticsCode = externalOrder.TimeInForce.ToCode();
			order.ExecutedQuantity = externalOrder.ExecutedQuantity;
			order.Price = externalOrder.Price;
			order.TransactTime = externalOrder.TransactTime;

			return order;
		}

		public static ExchangeOrder ToOrder (this BinanceCanceledOrder externalOrder)
		{
			if (externalOrder == null)
			{
				return null;
			}

			ExchangeOrder order = new ExchangeOrder();
			order.Symbol = externalOrder.Symbol;
			order.Status = externalOrder.Status.ToStatus();
			order.OriginalQuantity = externalOrder.OriginalQuantity;
			order.OrderId = long.Parse(externalOrder.ClientOrderId);
			order.ExchangeOrderId = externalOrder.OrderId.ToString();
			order.OrderSideCode = externalOrder.Side.ToCode();
			order.OrderTypeCode = externalOrder.Type.ToCode();
			order.FillPoliticsCode = externalOrder.TimeInForce.ToCode();
			order.ExecutedQuantity = externalOrder.ExecutedQuantity;
			order.Price = externalOrder.Price;

			return order;
		}

		public static Order ToOrder(this ExchangeOrder externalOrder)
		{
			Order order = new Order();
			order.Symbol = externalOrder.Symbol;
			order.OrderStatusCode = externalOrder.Status.Code;
			order.Amount = externalOrder.ExecutedQuantity;
			order.Id = externalOrder.OrderId;
			order.ExchangeOrderId = long.Parse(externalOrder.ExchangeOrderId);
			order.OrderSideCode = externalOrder.OrderSideCode.Code;
			order.OrderTypeCode = externalOrder.OrderTypeCode.Code;
			order.FillPoliticsCode = externalOrder.FillPoliticsCode.Code;
			order.Price = externalOrder.Price;

			return order;
		}
	}
}
