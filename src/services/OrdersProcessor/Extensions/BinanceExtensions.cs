using Binance.Net.Objects;
using core.Extensions.Mappers;
using core.Infrastructure.Database.Entities;
using System;

namespace OrdersProcessor.Extensions
{
	public static class BinanceExtensions
	{
		public static Order ToOrder(this BinanceStreamOrderUpdate binanceOrder)
		{
			Order order = new Order();
			order.Id = long.Parse(binanceOrder.ClientOrderId);
			order.ExchangeOrderId = binanceOrder.OrderId;
			order.Symbol = binanceOrder.Symbol;
			order.Updated = DateTime.Now;
			order.Price = binanceOrder.Price;
			order.Amount = binanceOrder.Quantity;
			order.OrderStatusCode = binanceOrder.Status.ToStatus().Code;
			order.ExchangeOrderStatusCode = (long)binanceOrder.Status;
			order.Fee = binanceOrder.Commission;
			order.Asset = binanceOrder.CommissionAsset;

			return order;
		}

		public static Balance ToBalance(this BinanceStreamBalance binanceBalance)
		{
			Balance balance = new Balance();
			balance.Asset = binanceBalance.Asset;
			balance.Available = binanceBalance.Free;
			balance.Total = binanceBalance.Total;

			return balance;
		}
	}
}
