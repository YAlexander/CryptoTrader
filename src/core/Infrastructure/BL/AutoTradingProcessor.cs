using core.Abstractions.Database;
using core.Abstractions.TypeCodes;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Database.Managers;
using core.Infrastructure.Models;
using core.Infrastructure.Models.Mappers;
using core.Infrastructure.Notifications;
using core.Trading.Models;
using core.TypeCodes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyNatsClient;
using MyNatsClient.Encodings.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core.Infrastructure.BL
{
	// TODO: Rewrite logic
	public class AutoTradingProcessor : BaseProcessor
	{
		private IOptions<AppSettings> _settings;
		private IOrderManager _orderManager;
		private IOrderProcessor _orderProcessor;
		private IBalanceManager _balanceManager;
		private IDealManager _dealManager;
		private ITradesManager _tradesManager;
		private IBookManager _bookManager;
		private DealProcessor _dealProcessor;
		private ILogger<AutoTradingProcessor> _logger;
		private NatsConnector _connector;

		public AutoTradingProcessor (
			IOptions<AppSettings> settings,
			ILogger<AutoTradingProcessor> logger,
			IOrderProcessor orderProcessor,
			IBalanceManager balanceManager,
			ITradesManager tradesManager,
			IOrderManager orderManager,
			IBookManager bookManager,
			IDealManager dealManager,
			DealProcessor dealProcessor,
			NatsConnector connector) : base(settings, logger)
		{
			_logger = logger;
			_orderProcessor = orderProcessor;
			_balanceManager = balanceManager;
			_dealManager = dealManager;
			_tradesManager = tradesManager;
			_orderManager = orderManager;
			_dealProcessor = dealProcessor;
			_bookManager = bookManager;
			_connector = connector;
			_settings = settings;
		}

		public Task<Deal> Buy (TradingForecast forecast, PairConfig config)
		{
			return WithConnection(async (connection, transaction) =>
			{
				Book book = await _bookManager.GetLast(forecast.ExchangeCode, forecast.Symbol, connection, transaction);
				Balance balance = await _balanceManager.Get(config.AssetTwo, forecast.ExchangeCode, connection, transaction);

				Order order = new Order();
				order.ExchangeCode = forecast.ExchangeCode;
				order.Symbol = forecast.Symbol;
				order.TradingModeCode = TradingModeCode.AUTO;
				order.FillPoliticsCode = FillPoliticsCode.GTC;
				order.OrderStatusCode = OrderStatusCode.PENDING;
				order.OrderTypeCode = OrderTypeCode.LMT;
				order.OrderSideCode = OrderSideCode.BUY;

				decimal availableAmount = balance.Available <= config.MaxOrderAmount.Value ? balance.Available : balance.Available;

				if (availableAmount < config.MinOrderAmount.Value)
				{
					throw new Exception($"Not enough funds. Exchange code: {forecast.ExchangeCode}, Asset: {config.AssetTwo}");
				}

				decimal quantity = Math.Round(availableAmount / book.BestAskPrice, 3);

				order.Price = book.BestAskPrice;
				order.Amount = quantity;

				Deal deal = await _dealProcessor.CreateForOrder(order);
				return deal;
			});
		}

		public Task<Deal> Sell (TradingForecast forecast, PairConfig config)
		{
			return WithConnection(async (connection, transaction) =>
			{
				Book book = await _bookManager.GetLast(forecast.ExchangeCode, forecast.Symbol, connection, transaction);
				Balance balance = await _balanceManager.Get(config.AssetOne, forecast.ExchangeCode, connection, transaction);

				Order order = new Order();
				order.ExchangeCode = forecast.ExchangeCode;
				order.Symbol = forecast.Symbol;
				order.TradingModeCode = TradingModeCode.AUTO;
				order.FillPoliticsCode = FillPoliticsCode.GTC;
				order.OrderStatusCode = OrderStatusCode.PENDING;
				order.OrderTypeCode = OrderTypeCode.LMT;
				order.OrderSideCode = OrderSideCode.SELL;

				decimal availableQuantity = (balance.Available <= config.MaxOrderAmount.Value / book.BestBidPrice ? balance.Available : balance.Available / book.BestBidPrice);

				if (availableQuantity < config.MinOrderAmount.Value / book.BestBidPrice)
				{
					throw new Exception($"Not enough funds. Exchange code: {forecast.ExchangeCode}, Asset: {config.AssetTwo}");
				}

				decimal quantity = Math.Round(availableQuantity, 3);

				order.Price = book.BestBidPrice;
				order.Amount = quantity;

				Deal deal = await _dealProcessor.CreateForOrder(order);
				return deal;
			});
		}

		public async Task StopLoss(Trade trade, PairConfig config)
		{
			await WithConnection(async (connection, transaction) =>
			{
				IExchangeCode exchangeCode = ExchangeCode.Create(trade.ExchangeCode);

				IEnumerable<Deal> deals = await _dealManager.GetOpenDeals(trade.ExchangeCode, trade.Symbol, connection, transaction);
				foreach(Deal deal in deals)
				{
					deal.Orders = await _orderManager.GetOrdersByDealId(deal.Id, connection, transaction) as List<Order>;
					if (deal.StopLoss.HasValue && trade.Price < deal.StopLoss.Value)
					{
						foreach(Order dealOrder in deal.Orders)
						{
							if(dealOrder.OrderStatusCode == OrderStatusCode.PENDING.Code || dealOrder.OrderStatusCode == OrderStatusCode.EXPIRED.Code || dealOrder.OrderStatusCode == OrderStatusCode.HOLD.Code)
							{
								await _orderManager.Delete(dealOrder.Id, connection, transaction);
							}
							else if(dealOrder.OrderStatusCode == OrderStatusCode.LISTED.Code)
							{
								dealOrder.OrderStatusCode = OrderStatusCode.CANCELED.Code;
								dealOrder.Updated = DateTime.UtcNow;
								dealOrder.UpdateRequired = true;

								await _orderManager.Update(dealOrder, connection, transaction);

								NatsClient client = _connector.Client;
								await client.PubAsJsonAsync(_settings.Value.OrdersQueueName, new Notification<Order>() { Code = ActionCode.UPDATED.Code, Payload = dealOrder });
							}
						}

						Order order = new Order();
						order.ExchangeCode = trade.ExchangeCode;
						order.Symbol = trade.Symbol;
						order.TradingModeCode = TradingModeCode.AUTO;
						order.FillPoliticsCode = FillPoliticsCode.GTC;
						order.OrderStatusCode = OrderStatusCode.PENDING;
						order.DealId = deal.Id;
						order.OrderTypeCode = OrderTypeCode.MKT;
						order.OrderSideCode = OrderSideCode.SELL;
						order.Amount = deal.Amount;

						order.Id = await _orderManager.Create(order, connection, transaction);
					}
				}

				return Task.CompletedTask;
			});
		}

		public Task TakeProfit (Trade trade, PairConfig config)
		{
			throw new NotImplementedException();
		}
	}
}
