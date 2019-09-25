using Backoffice.Models;
using Backoffice.Models.Mappers;
using core.Abstractions.Database;
using core.Infrastructure.BL.OrderProcessors;
using core.Infrastructure.Database.Entities;
using core.TypeCodes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backoffice.Controllers
{
	public class OrdersController : Controller
	{
		private OrderProcessor _orderProcessor;
		private IPairConfigManager _pairConfigManager;

		public OrdersController(OrderProcessor orderProcessor, IPairConfigManager pairConfigManager)
		{
			_orderProcessor = orderProcessor;
			_pairConfigManager = pairConfigManager;
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
				await _orderProcessor.Create(entity);

				return RedirectToAction("CreateOrder");
			}

			return View(model);
		}

	}
}
