using Binance.Net;
using core;
using core.Abstractions.TypeCodes;
using core.Infrastructure.BL;
using core.Infrastructure.Database.Entities;
using core.TypeCodes;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TradesWorker.Workers
{
	public class BinanceBookWorker : Worker
	{
		private IOptions<AppSettings> _settings;
		private ILogger<Worker> _logger;
		private BookProcessor _bookProcessor;

		public BinanceBookWorker (
			IOptions<AppSettings> settings,
			ILogger<Worker> logger,
			ExchangeConfigProcessor exchangeConfigProcessor,
			BookProcessor bookProcessor) : base(logger, exchangeConfigProcessor)
		{
			_logger = logger;
			_settings = settings;
			_bookProcessor = bookProcessor;
		}

		public override IExchangeCode Exchange { get; set; } = ExchangeCode.BINANCE;

		protected override async Task DoWork (PairConfig pair, CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					using (BinanceSocketClient client = new BinanceSocketClient())
					{
						CallResult<UpdateSubscription> res = client.SubscribeToBookTickerStream(pair.Symbol, async (data) =>
						{
							Book book = new Book();
							book.Symbol = data.Symbol;
							book.BestAskPrice = data.BestAskPrice;
							book.BestAskQuantity = data.BestAskQuantity;
							book.BestBidPrice = data.BestBidPrice;
							book.BestBidQuantity = data.BestBidQuantity;

							await _bookProcessor.Create(book);
						});

						res.Data.ConnectionLost += () => { _logger.LogError($"Connection to {Exchange} is lost"); };
						res.Data.ConnectionRestored += (data) => { _logger.LogError($"Connection to {Exchange} is Restored"); };

						while (!stoppingToken.IsCancellationRequested)
						{
						}
					}
				}
				catch (Exception ex)
				{
					_logger.LogError($"{Exchange.Description} Trades service failed with message {ex.Message}", ex);
				}
			}
		}
	}
}
