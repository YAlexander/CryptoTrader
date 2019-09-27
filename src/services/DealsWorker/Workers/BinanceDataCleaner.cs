using core.Abstractions.TypeCodes;
using core.Infrastructure.BL;
using core.Infrastructure.Database.Entities;
using core.TypeCodes;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TradesWorker.Workers
{
	public class BinanceDataBookWorker : Worker
	{
		private ILogger<Worker> _logger;
		private BookProcessor _bookProcessor;
		private TradesProcessor _tradesProcessor;

		public BinanceDataBookWorker (
			ILogger<Worker> logger,
			ExchangeConfigProcessor exchangeConfigProcessor,
			TradesProcessor tradesProcessor,
			BookProcessor bookProcessor) : base(logger, exchangeConfigProcessor)
		{
			_logger = logger;
			_bookProcessor = bookProcessor;
			_tradesProcessor = tradesProcessor;
		}

		public override IExchangeCode Exchange { get; set; } = ExchangeCode.BINANCE;

		protected override async Task DoWork (PairConfig pair, CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					await _tradesProcessor.ClearData(Exchange.Code, pair.Symbol);
					await _bookProcessor.ClearData(Exchange.Code, pair.Symbol);
					await Task.Delay(TimeSpan.FromHours(1));
				}
				catch (Exception ex)
				{
					_logger.LogError($"{Exchange.Description} Trades service failed with message {ex.Message}", ex);
				}
			}
		}
	}
}
