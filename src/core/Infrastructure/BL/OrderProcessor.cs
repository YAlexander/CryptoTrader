using core.Abstractions;
using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Database.Managers;
using core.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace core.Infrastructure.BL.OrderProcessors
{
	public class OrderProcessor : BaseProcessor, IOrderProcessor
	{
		private IOrderManager _orderManager;
		private ExchangeConfigManager _exchangeConfigManager;
		private PairConfigManager _pairConfigManager;

		private Dictionary<int, Func<Order, ExchangeConfig, Task<ExchangeOrder>>> orderSenders = new Dictionary<int, Func<Order, ExchangeConfig, Task<ExchangeOrder>>>();

		public OrderProcessor (
			ILogger<OrderProcessor> logger,
			IOptions<AppSettings> settings,
			IEnumerable<IExchangeOrdersSender> senders,
			ExchangeConfigManager exchangeConfigManager,
			PairConfigManager pairConfigManager,
			IOrderManager orderManager) : base(settings, logger)
		{
			_orderManager = orderManager;
			_exchangeConfigManager = exchangeConfigManager;
			_pairConfigManager = pairConfigManager;

			foreach (IExchangeOrdersSender sender in senders)
			{
				orderSenders.Add(sender.Exchange.Code, sender.Send);
			}
		}

		public async Task<Order> Update (Order order)
		{
			return await WithConnection(async (connection, transaction) =>
			{
				return await _orderManager.Update(order, connection, transaction);
			});
		}

		public async Task<Order> Update (ExchangeOrder order)
		{
			return await WithConnection(async (connection, transaction) =>
			{
				return await _orderManager.Update(order, connection, transaction);
			});
		}

		/// <summary>
		/// Send order to Exchange
		/// </summary>
		/// <param name="orderId"></param>
		public async Task<ExchangeOrder> List (long orderId)
		{
			Order order = null;

			ExchangeConfig config = await WithConnection(async (connection, transaction) =>
			{
				order = await _orderManager.Get(orderId, connection);
				ExchangeConfig config = await _exchangeConfigManager.Get(order.ExchangeCode, connection, transaction);
				config.Pairs = (List<PairConfig>)await _pairConfigManager.GetAssignedPairs(config.ExchangeCode, connection, transaction);
				return config;
			});

			Func<Order, ExchangeConfig, Task<ExchangeOrder>> sender = orderSenders[config.ExchangeCode];
			return await sender?.Invoke(order, config);
		}

		public async Task<long?> Create (Order order)
		{
			return await WithConnection(async (connection, transaction) =>
			{
				return await _orderManager.Create(order, connection, transaction);
			});
		}
	}
}
