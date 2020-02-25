using core.Infrastructure.Database.Entities;

namespace Backoffice.Models.Mappers
{
	public static class OrderMapper
	{
		public static Order ToOrder (this OrderModel model)
		{
			Order entity = new Order();
			entity.TradingModeCode = model.TradingMode;
			entity.ExchangeCode = model.Exchange;
			entity.OrderSideCode = model.OrderSide;
			entity.OrderTypeCode = model.OrderType;
			entity.OrderStatusCode = model.OrderStatus;
			entity.FillPoliticsCode = model.FillPolitics;

			entity.DealId = model.DealId;
			entity.ExchangeOrderId = model.ExchangeOrderId;
			entity.ExchangeOrderStatusCode = model.ExchangeOrderStatus;
			entity.Symbol = model.Symbol;
			entity.Price = model.Price;
			entity.Amount = model.Amount;
			entity.Limit = model.Limit;
			entity.StopLimit = model.StopPrice;
			entity.StopLoss = model.StopLoss;

			entity.TakeProfit = model.TakeProfit;

			entity.ExpirationDate = model.ExpirationDate;
			entity.LastErrorDate = model.LastErrorDate;
			entity.LastError = model.LastError;
			entity.UpdateRequired = model.UpdateRequired;
			entity.CancelRequired = model.CancelRequired;
			entity.StatusDescription = model.StatusDescription;

			return entity;
		}
	}
}
