using core.Abstractions.Database;
using core.Abstractions.TypeCodes;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Database.Managers;
using core.Infrastructure.Models.Mappers;
using core.TypeCodes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace core.Infrastructure.BL
{
	public class DealProcessor : BaseProcessor
	{
		private IDealManager _dealManager;
		private IOrderManager _orderManager;

		public DealProcessor (
			IOptions<AppSettings> settings,
			ILogger logger,
			IOrderManager orderManager,
			IDealManager dealManager) : base(settings, logger)
		{
			_dealManager = dealManager;
			_orderManager = orderManager;
		}

		public Task<Deal> Update(Deal deal)
		{
			return WithConnection((connection, transaction) =>
			{
				return _dealManager.Update(deal, connection, transaction);
			});
		}

		public Task<long> Create (Deal deal)
		{
			return WithConnection((connection, transaction) =>
			{
				return _dealManager.Create(deal, connection, transaction);
			});
		}

		public async Task<Deal> CreateForOrder (Order order)
		{
			// TODO: Check available balance

			return await WithConnection(async (connection, transaction) =>
			{
				Deal deal = new Deal();
				deal.StatusCode = DealStatusCode.OPEN.Code;
				deal.Symbol = order.Symbol;
				deal.Exchange = order.ExchangeCode;
				deal.StopLoss = order.StopLoss;
				deal.TakeProfit = order.TakeProfit;
				deal.Id = await _dealManager.Create(deal, connection, transaction);

				order.DealId = deal.Id;
				order.Id = await _orderManager.Create(order, connection, transaction);

				deal.Orders.Add(order);

				return deal;
			});
		}

		public async Task<Deal> UpdateForOrder (Order order)
		{
			if (!order.DealId.HasValue)
			{
				return null;
			}

			return await WithConnection(async (connection, transaction) =>
			{
				Deal deal = await _dealManager.Get(order.DealId.Value, connection, transaction);
				if(deal == null)
				{
					throw new Exception($"Deal {order.DealId} not found");
				}

				Order storedOrder = await _orderManager.Update(order, connection, transaction);

				if(order.OrderSideCode == OrderSideCode.BUY.Code)
				{
					deal.AvgOpenPrice = order.Price;
					deal.Amount = order.Amount;
					deal.EstimatedFee = order.Fee * order.Price;

					if(order.TradingModeCode == TradingModeCode.AUTO.Code)
					{
						Order sellOrder = new Order();
						sellOrder.ExchangeCode = deal.Exchange;
						sellOrder.Symbol = deal.Symbol;
						sellOrder.TradingModeCode = TradingModeCode.AUTO.Code;
						sellOrder.FillPoliticsCode = FillPoliticsCode.GTC.Code;
						sellOrder.OrderStatusCode = OrderStatusCode.PENDING.Code;
						sellOrder.DealId = deal.Id;
						sellOrder.OrderTypeCode = OrderTypeCode.LMT.Code;
						sellOrder.OrderSideCode = OrderSideCode.SELL.Code;
						sellOrder.Amount = deal.Amount;
						sellOrder.Price = deal.TakeProfit;

						await _orderManager.Create(order, connection, transaction);
					}
				}
				else
				{
					deal.AvgClosePrice = order.Price;
					deal.EstimatedFee += order.Fee;
					deal.StatusCode = OrderStatusCode.Create(order.OrderStatusCode).ToDealCode().Code;
				}

				deal = await _dealManager.Update(deal, connection, transaction);
				deal.Orders.Add(storedOrder);
				return deal;
			});
		}

		public Task<Deal> Get(long id)
		{
			return WithConnection((connection, transaction) =>
			{
				return _dealManager.Get(id, connection, transaction);
			});
		}		
	}
}
