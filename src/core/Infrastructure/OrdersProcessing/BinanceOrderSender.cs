using System;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Objects;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Models;
using core.Infrastructure.Models.Mappers;
using core.TypeCodes;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace core.Infrastructure.OrdersProcessing
{
	public class BinanceOrderSender : IExchangeOrdersSender
	{
		private IOptions<AppSettings> _settings;
		private ILogger<IExchangeOrdersSender> _logger;

		public BinanceOrderSender(IOptions<AppSettings> settings, ILogger<IExchangeOrdersSender> logger)
		{
			_settings = settings;
			_logger = logger;
		}

		public IExchangeCode Exchange { get; } = ExchangeCode.BINANCE;

		public async Task<ExchangeOrder> Send (Order order, ExchangeConfig config)
		{
			ExchangeOrder exchangeOrder = null;
			PairConfig pairConfig = config.Pairs.FirstOrDefault(x => x.Symbol.Equals(order.Symbol));

			if (order.UpdateRequired)
			{
				using (BinanceClient client = new BinanceClient(new BinanceClientOptions() { ApiCredentials = new ApiCredentials(config.ApiKey, config.ApiSecret) }))
				{
					if (order.OrderStatusCode == OrderStatusCode.PENDING)
					{
						WebCallResult<BinancePlacedOrder> orderResult = null;
						if (order.OrderTypeCode == OrderTypeCode.MKT.Code)
						{
							if (pairConfig.IsTestMode)
							{
								orderResult = await client.PlaceTestOrderAsync(order.Symbol, (OrderSide)order.OrderSideCode, OrderType.Market, order.Amount.Value, order.Id.ToString(), null, TimeInForce.GoodTillCancel);
							}
							else
							{
								orderResult = await client.PlaceOrderAsync(order.Symbol, (OrderSide)order.OrderSideCode, OrderType.Market, order.Amount.Value, order.Id.ToString(), null, TimeInForce.GoodTillCancel);
							}
						}
						else if (order.OrderTypeCode == OrderTypeCode.LMT.Code)
						{
							if (pairConfig.IsTestMode)
							{
								orderResult = await client.PlaceTestOrderAsync(order.Symbol, (OrderSide)order.OrderSideCode, OrderType.Limit, order.Amount.Value, order.Id.ToString(), order.Price, TimeInForce.GoodTillCancel);
							}
							else
							{
								orderResult = await client.PlaceOrderAsync(order.Symbol, (OrderSide)order.OrderSideCode, OrderType.Limit, order.Amount.Value, order.Id.ToString(), order.Price, TimeInForce.GoodTillCancel);
							}
						}
						else
						{
							throw new Exception($"{order.Id} selected order type isn't supported");
						}

						if (!orderResult.Success)
						{
							throw new Exception(orderResult.Error.Message);
						}

						exchangeOrder = orderResult.Data.ToOrder();
						_logger.LogInformation($"{Exchange.Description} - Order {order.Id} has been processed with status: {orderResult.Data.Status}");
					}
					else if (order.OrderStatusCode == OrderStatusCode.CANCELED)
					{
						WebCallResult<BinanceCanceledOrder> cancelResult = await client.CancelOrderAsync(order.Symbol, order.ExchangeOrderId);

						if (!cancelResult.Success)
						{
							throw new Exception(cancelResult.Error.Message);
						}

						exchangeOrder = cancelResult.Data.ToOrder();
						_logger.LogInformation($"{Exchange.Description} - Order {order.Id} has been processed with status: {cancelResult.Data.Status}");
					}
					else
					{
						throw new Exception($"{Exchange.Description} - Can't update update order {order.Id}: wrong status");
					}
				}
			}

			return exchangeOrder;
		}
	}
}
