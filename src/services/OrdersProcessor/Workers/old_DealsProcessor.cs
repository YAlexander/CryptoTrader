using core;
using core.Infrastructure.BL;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Notifications;
using core.TypeCodes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyNatsClient;
using MyNatsClient.Encodings.Json;
using MyNatsClient.Events;
using MyNatsClient.Extensions;
using MyNatsClient.Ops;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrdersProcessor.Workers
{
	// Subscribed to new deals from all exchanges. Should process StopLoss and TakeProfit orders
	public class old_DealsProcessor : BackgroundService
	{
		private IOptions<AppSettings> _settings;
		private ILogger<old_DealsProcessor> _logger;
		private AutoTradingProcessor _autoTradingProcessor;
		private ExchangeConfigProcessor _exchangeConfigProcessor;

		public old_DealsProcessor (IOptions<AppSettings> settings, ILogger<old_DealsProcessor> logger, ExchangeConfigProcessor exchangeConfigProcessor, AutoTradingProcessor autoTradingProcessor)
		{
			_logger = logger;
			_settings = settings;
			_autoTradingProcessor = autoTradingProcessor;
			_exchangeConfigProcessor = exchangeConfigProcessor;
		}

		protected override async Task ExecuteAsync (CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					ConnectionInfo cnInfo = new ConnectionInfo(_settings.Value.BusConnectionString);
					using (NatsClient natsClient = new NatsClient(cnInfo))
					{
						if (!natsClient.IsConnected)
						{
							natsClient.Connect();
						}

						await natsClient.SubAsync(_settings.Value.TradesQueueName, stream => stream.Subscribe(msg =>
						{
							try
							{
								Task.Run(async () => await Process(msg, natsClient));
							}
							catch (Exception ex)
							{
								_logger.LogError($"TradingProcessor failed with message: {ex.Message}", ex);
							}
						}));

						natsClient.Events.OfType<ClientDisconnected>().Subscribe(ev =>
						{
							Console.WriteLine($"Client was disconnected due to reason '{ev.Reason}'");

							if (!cnInfo.AutoReconnectOnFailure)
							{
								ev.Client.Connect();
							}
						});

						while (!stoppingToken.IsCancellationRequested)
						{
						}

						natsClient.Disconnect();
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex.Message);
				}
			}
		}

		private async Task Process (MsgOp msg, NatsClient natsClient)
		{
			string payload = msg.GetPayloadAsString();
			Notification<Trade> response = JsonConvert.DeserializeObject<Notification<Trade>>(payload[1..]);

			Trade trade = response.Payload;
			ExchangeConfig config = await _exchangeConfigProcessor.GetExchangeConfig(trade.ExchangeCode);
			PairConfig pairConfig = config.Pairs.Single(x => x.ExchangeCode == trade.ExchangeCode && x.Symbol.Equals(trade.Symbol));

			await _autoTradingProcessor.StopLoss(trade, pairConfig);
			await _autoTradingProcessor.TakeProfit(trade, pairConfig);

			try
			{
				await natsClient.PubAsJsonAsync(_settings.Value.OrdersQueueName, new Notification<object>() { Code = ActionCode.UPDATED.Code, Payload = null });
			}
			catch (Exception ex)
			{
				_logger.LogError("Can't send Nata notification", ex);
			}
		}
	}
}
