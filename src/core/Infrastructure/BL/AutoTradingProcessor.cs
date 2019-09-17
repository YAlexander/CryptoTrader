using core.Abstractions.Database;
using core.Abstractions.TypeCodes;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Database.Managers;
using core.Infrastructure.Models;
using core.Trading.Models;
using core.TypeCodes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core.Infrastructure.BL
{
	public class AutoTradingProcessor : BaseProcessor
	{
		private IExchangeCode DefaultCode = ExchangeCode.UNKNOWN;

		private IOrderManager _orderManager;
		private IOrderProcessor _orderProcessor;
		private IBalanceManager _balanceManager;
		private IDealManager _dealManager;
		private ITradesManager _tradesManager;
		private ILogger<AutoTradingProcessor> _logger;

		public AutoTradingProcessor (
			IOptions<AppSettings> settings,
			ILogger<AutoTradingProcessor> logger,
			IOrderProcessor orderProcessor,
			IBalanceManager balanceManager,
			ITradesManager tradesManager,
			IOrderManager orderManager,
			IDealManager dealManager) : base(settings, logger)
		{
			_logger = logger;
			_orderProcessor = orderProcessor;
			_balanceManager = balanceManager;
			_dealManager = dealManager;
			_tradesManager = tradesManager;
			_orderManager = orderManager;
		}

		public async Task Buy (TradingForecast forecast, PairConfig config)
		{
			// TODO: Add check for blocked trading
			IExchangeCode exchangeCode = ExchangeCode.Create(forecast.ExchangeCode);
			Deal deal = new Deal();
			deal.Exchange = forecast.ExchangeCode;
			deal.Symbol = forecast.Symbol;
			deal.StatusCode = DealStatusCode.OPEN;

			await WithConnection(async (connection, transaction) =>
			{
				deal.Id = await _dealManager.Create(deal, connection, transaction);

				Order order = new Order();
				order.ExchangeCode = forecast.ExchangeCode;
				order.Symbol = forecast.Symbol;
				order.TradingModeCode = TradingModeCode.AUTO;
				order.FillPoliticsCode = FillPoliticsCode.GTC;
				order.OrderStatusCode = OrderStatusCode.PENDING;
				order.DealId = deal.Id;
				order.OrderTypeCode = OrderTypeCode.MKT;
				order.OrderSideCode = OrderSideCode.BUY;

				// TODO: Add Asset fields to config
				Balance balance = await _balanceManager.Get(deal.Symbol[4..6], exchangeCode.Code, connection, transaction);

				Trade trade = await _tradesManager.GetLast(exchangeCode.Code, deal.Symbol, connection, transaction);

				decimal amountToBuy = Math.Round(config.isMaxAmountPercent
									? (balance.Available * config.MaxOrderAmount.Value / 100) / trade.Price
									: (balance.Available > config.MaxOrderAmount.Value ? config.MaxOrderAmount.Value : balance.Available) / trade.Price, 3);

				if (amountToBuy * trade.Price < 15)
				{
					throw new Exception($"Not enough funds ({exchangeCode.Description} - {forecast.Symbol})");
				}

				order.Amount = amountToBuy;

				long? orderId = await _orderManager.Create(order, connection, transaction);

				ExchangeOrder processedOrder = null;
				try
				{
					processedOrder = await _orderProcessor.List(orderId.Value);
				}
				catch (Exception ex)
				{
					_logger.LogError($"{exchangeCode.Description} - Order listing is failed with message: {ex.Message}", ex);
					throw;
				}

				_logger.LogInformation($"Order {orderId} has been listed");

				deal.AvgOpenPrice = processedOrder.Price;
				deal.Amount = processedOrder.ExecutedQuantity;
				deal.StatusCode = GetDealCode(processedOrder.Status).Code;
				deal.EstimatedFee = processedOrder.ExecutedQuantity * processedOrder.Price * (decimal)(config.ExchangeFeeBuy ?? 0);
				deal.StopLoss = config.DefaultStopLossPercent.HasValue ? processedOrder.Price - processedOrder.Price * (decimal)config.DefaultStopLossPercent.Value / 100 : 0;
				deal.TakeProfit = config.DefaultTakeProfitPercent.HasValue ? processedOrder.Price + processedOrder.Price * (decimal)config.DefaultTakeProfitPercent.Value / 100 : 0;

				await _dealManager.Update(deal, connection, transaction);
				_logger.LogInformation($"Deal {deal.Id} has been updated");

				await _orderManager.Update(processedOrder, connection, transaction);
				_logger.LogInformation($"Order {orderId} has been updated");

				return Task.CompletedTask;
			});
		}

		public async Task Sell (TradingForecast forecast, PairConfig config)
		{
			IExchangeCode exchangeCode = ExchangeCode.Create(forecast.ExchangeCode);

			await WithConnection(async (connection, transaction) =>
			{
				IEnumerable<Deal> deals = await _dealManager.GetOpenDeals(forecast.ExchangeCode, forecast.Symbol, connection, transaction);

				Trade lastTrade = await _tradesManager.GetLast(forecast.ExchangeCode, forecast.Symbol, connection, transaction);

				// TODO: Revise sell logic
				foreach (Deal deal in deals.ToList())
				{
					// TODO: Add minimal profit and replace condition
					if(deal.TakeProfit.HasValue && deal.TakeProfit.Value < lastTrade.Price || deal.AvgOpenPrice + deal.EstimatedFee <= lastTrade.Price)
					{
						continue;
					}

					IEnumerable<Order> orders = await _dealManager.GetAssociatedOrders(deal.Id, connection, transaction);
					if (orders.Count() == 1)
					{
						decimal? amount = orders.First().Amount;

						Order order = new Order();
						order.ExchangeCode = forecast.ExchangeCode;
						order.Symbol = forecast.Symbol;
						order.TradingModeCode = TradingModeCode.AUTO;
						order.FillPoliticsCode = FillPoliticsCode.GTC;
						order.OrderStatusCode = OrderStatusCode.PENDING;
						order.DealId = deal.Id;
						order.OrderTypeCode = OrderTypeCode.MKT;
						order.OrderSideCode = OrderSideCode.SELL;
						order.Amount = amount;

						long? orderId = await _orderManager.Create(order, connection, transaction);

						ExchangeOrder processedOrder = null;
						try
						{
							processedOrder = await _orderProcessor.List(orderId.Value);
						}
						catch (Exception ex)
						{
							_logger.LogError($"{exchangeCode.Description} - Order listing is failed with message: {ex.Message}", ex);
							throw;
						}

						deal.AvgClosePrice = processedOrder.Price;
						deal.Amount = processedOrder.ExecutedQuantity;
						deal.StatusCode = GetDealCode(processedOrder.Status).Code;
						// TODO: Check with USDT
						deal.EstimatedFee += processedOrder.ExecutedQuantity * processedOrder.Price * (decimal)(config.ExchangeFeeSell ?? 0);

						await _dealManager.Update(deal, connection, transaction);
						_logger.LogInformation($"Deal {deal.Id} has been updated");

						await _orderManager.Update(processedOrder, connection, transaction);
						_logger.LogInformation($"Order {orderId} has been updated");
					}
					else
					{
						_logger.LogError($"Please check orders for deal {deal.Id}");
					}
				}

				return Task.CompletedTask;
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
					if(deal.StopLoss.HasValue && trade.Price < deal.StopLoss.Value)
					{
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

						long? orderId = await _orderManager.Create(order, connection, transaction);
						
						ExchangeOrder processedOrder = null;
						try
						{
							processedOrder = await _orderProcessor.List(orderId.Value);
						}
						catch (Exception ex)
						{
							_logger.LogError($"{exchangeCode.Description} - Order listing is failed with message: {ex.Message}", ex);
							throw;
						}

						deal.AvgClosePrice = processedOrder.Price;
						deal.Amount = processedOrder.ExecutedQuantity;
						deal.StatusCode = GetDealCode(processedOrder.Status).Code;

						await _dealManager.Update(deal, connection, transaction);
						_logger.LogInformation($"Deal {deal.Id} has been updated");

						await _orderManager.Update(processedOrder, connection, transaction);
						_logger.LogInformation($"Order {orderId} has been updated");
					}
				}

				return Task.CompletedTask;
			});
		}

		public async Task TakeProfit (Trade trade, PairConfig config)
		{
			await WithConnection(async (connection, transaction) =>
			{
				IExchangeCode exchangeCode = ExchangeCode.Create(trade.ExchangeCode);

				IEnumerable<Deal> deals = await _dealManager.GetOpenDeals(trade.ExchangeCode, trade.Symbol, connection, transaction);
				foreach (Deal deal in deals)
				{
					if (deal.TakeProfit.HasValue && trade.Price > deal.TakeProfit.Value)
					{
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

						long? orderId = await _orderManager.Create(order, connection, transaction);

						ExchangeOrder processedOrder = null;
						try
						{
							processedOrder = await _orderProcessor.List(orderId.Value);
						}
						catch (Exception ex)
						{
							_logger.LogError($"{exchangeCode.Description} - Order listing is failed with message: {ex.Message}", ex);
							throw;
						}

						deal.AvgClosePrice = processedOrder.Price;
						deal.Amount = processedOrder.ExecutedQuantity;
						deal.StatusCode = GetDealCode(processedOrder.Status).Code;

						await _dealManager.Update(deal, connection, transaction);
						_logger.LogInformation($"Deal {deal.Id} has been updated");

						await _orderManager.Update(processedOrder, connection, transaction);
						_logger.LogInformation($"Order {orderId} has been updated");
					}
				}

				return Task.CompletedTask;
			});
		}

		private IDealStatusCode GetDealCode (IOrderStatusCode code)
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
	}
}
