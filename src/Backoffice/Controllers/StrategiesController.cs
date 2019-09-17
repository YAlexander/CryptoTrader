using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Infrastructure.BL;
using Microsoft.AspNetCore.Mvc;
using core.Infrastructure.Database.Entities;
using Backoffice.Models.Mappers;

namespace Backoffice.Controllers
{
	public class StrategiesController : Controller
	{
		private StrategyProcessor _strategyProcessor;

		public StrategiesController (StrategyProcessor strategyProcessor)
		{
			_strategyProcessor = strategyProcessor;
		}

		public async Task<IActionResult> Index ()
		{
			IEnumerable<Strategy> strategies = await _strategyProcessor.GetAll();

			return View(strategies.Select(x => x.ToModel()).OrderBy(x => x.Name));
		}

		public async Task<IActionResult> Enable(long id)
		{
			await _strategyProcessor.Enable(id);
			return RedirectToAction("Index");
		}
	}
}