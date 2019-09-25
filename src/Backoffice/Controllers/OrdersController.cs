using Backoffice.Models;
using Backoffice.Models.Mappers;
using core;
using core.Abstractions.Database;
using core.Infrastructure;
using core.Infrastructure.BL.OrderProcessors;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Notifications;
using core.TypeCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyNatsClient;
using MyNatsClient.Encodings.Json;
using System;
using System.Threading.Tasks;

namespace Backoffice.Controllers
{
	public class OrdersController : Controller
	{
		private ILogger<OrdersController> _logger;
		private OrderProcessor _orderProcessor;
		private IPairConfigManager _pairConfigManager;
		private NatsConnector _connector;
		private IOptions<AppSettings> _settings;

		public OrdersController (ILogger<OrdersController> logger, IOptions<AppSettings> settings, OrderProcessor orderProcessor, IPairConfigManager pairConfigManager, NatsConnector connector)
		{
			_logger = logger;
			_settings = settings;
			_orderProcessor = orderProcessor;
			_pairConfigManager = pairConfigManager;
			_connector = connector;
		}

		[HttpGet]
		public IActionResult CreateOrder ()
		{
			OrderModel model = new OrderModel();
			model.TradingMode = TradingModeCode.MANUAL;

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrder (OrderModel model)
		{
			if (ModelState.IsValid)
			{
				Order entity = model.ToOrder();
				entity.Id = await _orderProcessor.Create(entity);

				try
				{
					NatsClient client = _connector.Client;
					await client.PubAsJsonAsync(_settings.Value.OrdersQueueName, new Notification<Order>() { Code = ActionCode.CREATED.Code, Payload = entity });
				}
				catch(Exception ex)
				{
					_logger.LogError("Can't send Nata notification", ex);
				}

				return RedirectToAction("CreateOrder");
			}

			return View(model);
		}

	}
}
