using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Infrastructure.BL;
using Microsoft.AspNetCore.Mvc;
using core.Infrastructure.Database.Entities;
using Backoffice.Models.Mappers;
using Backoffice.Models.Filters;
using System;
using core.Abstractions;
using core.Abstractions.TypeCodes;

namespace Backoffice.Controllers
{
	public class StrategiesController : Controller
	{
		private StrategyProcessor _strategyProcessor;
		private CandleProcessor _candleProcessor;

		public StrategiesController (StrategyProcessor strategyProcessor, CandleProcessor candleProcessor)
		{
			_strategyProcessor = strategyProcessor;
			_candleProcessor = candleProcessor;
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

		[HttpGet]
		public async Task<IActionResult> StrategyTesting ()
		{
			IEnumerable<Strategy> strategies = await _strategyProcessor.GetAll();
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> StrategyTesting (StrategyTestingModel model)
		{
			IEnumerable<Candle> candles = await _candleProcessor.GetLast(model.ExchangeCode, model.Symbol, model.IntervalCode, 1500);

			Strategy strategyInfo = await _strategyProcessor.Get(model.StrategyId);

			Type type = Type.GetType(strategyInfo.TypeName, true, true);

			ITradingStrategy strategy = (ITradingStrategy)Activator.CreateInstance(type);

			ITradingAdviceCode res = strategy.Forecast(candles);

			return View();
		}
	}
}