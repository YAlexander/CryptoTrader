using Backoffice.Models;
using Backoffice.Models.Mappers;
using core.Infrastructure.BL;
using core.Infrastructure.Database.Entities;
using core.TypeCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Controllers
{
	public class ExchangeController : Controller
	{
		private ILogger<ExchangeController> _logger;
		private ExchangeConfigProcessor _exchangeConfigProcessor;

		public ExchangeController (
			ILogger<ExchangeController> logger,
			ExchangeConfigProcessor exchangeConfigProcessor
			)
		{
			_logger = logger;
			_exchangeConfigProcessor = exchangeConfigProcessor;
		}

		[HttpGet]
		public async Task<IActionResult> Index ()
		{
			IEnumerable<ExchangeConfig> configs = await _exchangeConfigProcessor.GetExchangeConfigs();
			return View(configs.Select(item => item.ToModel()).OrderBy(x => x.ExchangeName).ToList());
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			ExchangeConfig config = await _exchangeConfigProcessor.GetExchangeConfig(id);
			return View(config.ToModel());
		}
	}
}