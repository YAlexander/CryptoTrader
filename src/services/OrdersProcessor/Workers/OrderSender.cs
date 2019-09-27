using core;
using core.Abstractions;
using core.Infrastructure.BL;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Models;
using core.Infrastructure.Models.Mappers;
using core.Infrastructure.Notifications;
using core.TypeCodes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyNatsClient;
using MyNatsClient.Events;
using MyNatsClient.Extensions;
using MyNatsClient.Ops;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrdersProcessor.Workers
{
	public class OrderSender : BackgroundService
	{
		private ILogger<OrderSender> _logger;
		private IOptions<AppSettings> _settings;
		private ExchangeConfigProcessor _exchangeConfigProcessor;
		private DealProcessor _dealProcessor;
		private IEnumerable<IExchangeOrdersSender> _senders;

		public OrderSender(ILogger<OrderSender> logger, ExchangeConfigProcessor exchangeConfigProcessor, DealProcessor dealProcessor, IOptions<AppSettings> settings, IEnumerable<IExchangeOrdersSender> senders)
		{
			_logger = logger;
			_settings = settings;
			_senders = senders;
			_dealProcessor = dealProcessor;
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

						await natsClient.SubAsync(_settings.Value.OrdersQueueName, stream => stream.Subscribe(msg =>
						{
							try
							{
								Task.Run(async () => await Process(msg));
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

		private async Task Process (MsgOp msg)
		{
			string payload = msg.GetPayloadAsString();
			Notification<Order> response = JsonConvert.DeserializeObject<Notification<Order>>(payload[1..]);
			Order order = response.Payload;

			if (order.OrderStatusCode == OrderStatusCode.PENDING.Code || order.UpdateRequired)
			{
				IExchangeOrdersSender sender = _senders.SingleOrDefault(x => x.Exchange.Code == order.ExchangeCode);
				ExchangeConfig config = await _exchangeConfigProcessor.GetExchangeConfig(order.ExchangeCode);

				if (sender != null)
				{
					ExchangeOrder res = await sender.Send(order, config);
					Deal deal = await _dealProcessor.UpdateForOrder(res.ToOrder(), config.Pairs.SingleOrDefault(x => x.Symbol.Equals(res.Symbol)));
				}
			}
		}
	}
}
