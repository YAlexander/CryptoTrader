using System.Collections.Generic;
using System.Threading.Tasks;
using core.Infrastructure.BL;
using core.Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Backoffice.Controllers
{
	public class AssetsController : Controller
	{
		private AssetProcessor _assetProcessor;

		public AssetsController(AssetProcessor assetProcessor)
		{
			_assetProcessor = assetProcessor;
		}

		public async Task<IActionResult> Index ()
		{
			IEnumerable<Asset> assets = await _assetProcessor.GetAll();
			return View(assets);
		}

		[HttpGet]
		public IActionResult Create ()
		{
			return View(new Asset());
		}

		[HttpPost]
		public async Task<IActionResult> Create (Asset asset)
		{
			if (ModelState.IsValid)
			{
				await _assetProcessor.Create(asset);
				return RedirectToAction("Index");
			}

			return View(asset);
		}
	}
}