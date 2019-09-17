using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using core.Abstractions.TypeCodes;
using core.Infrastructure.BL;
using core.Infrastructure.Database.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CandlesWorker.Workers
{
	public abstract class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private ExchangeConfigProcessor _exchangeConfigProcessor;

		public abstract IExchangeCode Exchange { get; }
		protected abstract IPeriodCode DefaultCandleInterval { get; }

		protected Worker (
			ILogger<Worker> logger,
			ExchangeConfigProcessor exchangeConfigProcessor)
		{
			_logger = logger;
			_exchangeConfigProcessor = exchangeConfigProcessor;
		}

		protected override async Task ExecuteAsync (CancellationToken stoppingToken)
		{
			ExchangeConfig config = await _exchangeConfigProcessor.GetExchangeConfig(Exchange.Code);

			if (config == null)
			{
				_logger.LogError($"Config for {Exchange.Description} is not found");
				return;
			}

			List<Task> tasks = new List<Task>();

			if (config.IsEnabled)
			{
				foreach (var pair in config.Pairs)
				{
					if (pair.IsEnabled)
					{
						Task t = Task.Run(() => DoWork(pair, stoppingToken));
						tasks.Add(t);
					}
					else
					{
						_logger.LogInformation($"Pair '{pair.Symbol}' is disabled");
					}
				}

				await Task.WhenAll(tasks.ToArray());
			}
			else
			{
				_logger.LogInformation($"Exchange '{Exchange.Description}' is disabled");
			}
		}

		protected abstract Task DoWork (PairConfig config, CancellationToken stoppingToken);
	}
}
