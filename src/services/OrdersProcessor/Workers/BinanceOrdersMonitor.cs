using Binance.Net;
using core.Abstractions.TypeCodes;
using core.Infrastructure.BL;
using core.Infrastructure.BL.OrderProcessors;
using core.Infrastructure.Database.Entities;
using core.TypeCodes;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using OrdersProcessor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrdersProcessor.Workers
{
	// Monitoring orders status on Exchanges
	public class BinanceOrdersMonitor : Worker
	{
		private ILogger<Worker> _logger;
		private ExchangeConfigProcessor _exchangeConfigProcessor;
		private OrderProcessor _orderProcessor;
		private BalanceProcessor _balanceProcessor;

		public BinanceOrdersMonitor (
			ILogger<Worker> logger,
			ExchangeConfigProcessor exchangeConfigProcessor,
			OrderProcessor orderProcessor,
			BalanceProcessor balanceProcessor
			) : base(logger, exchangeConfigProcessor)
		{
			_logger = logger;
			_exchangeConfigProcessor = exchangeConfigProcessor;
			_orderProcessor = orderProcessor;
			_balanceProcessor = balanceProcessor;
		}

		public override IExchangeCode Exchange { get; } = ExchangeCode.BINANCE;

		protected override async Task DoWork (PairConfig config, CancellationToken stoppingToken)
		{
			ExchangeConfig exchangeConfig = await _exchangeConfigProcessor.GetExchangeConfig(Exchange.Code);

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					string listenKey = String.Empty;
					using (BinanceClient client = new BinanceClient())
					{
						listenKey = client.StartUserStream().Data;
					}

					using (BinanceSocketClient socketClient = new BinanceSocketClient())
					{
						CallResult<UpdateSubscription> successAccount = socketClient.SubscribeToUserStream(listenKey,
						accountData => 
						{
							// Process Account info changes there if required
						}, 
						orderData =>
						{
							// TODO: Remove order checking from OrderProcessor
							// Update Deal, update Order
						},
						ocoOrderData =>
						{
							// TODO: Remove order checking from OrderProcessor
							// Update Deal, update Order
						},
						async balancesData =>
						{
							IEnumerable<Balance> balances = balancesData.Select(x => x.ToBalance());
							foreach(Balance balance in balances)
							{
								await _balanceProcessor.UpdateOrCreate(balance);
							}
						});

						while (!stoppingToken.IsCancellationRequested)
						{ 
						}

						await socketClient.UnsubscribeAll();
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex.Message);
				}
			}
		}
	}
}
