using Backoffice.Models;
using core.Infrastructure.BL;
using core.Infrastructure.Database.Entities;
using core.TypeCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Controllers
{
	public class PairsController : Controller
	{
		private PairConfigProcessor _pairConfigProcessor;
		private AssetProcessor _assetProcessor;

		public PairsController (PairConfigProcessor pairConfigProcessor, AssetProcessor assetProcessor)
		{
			_pairConfigProcessor = pairConfigProcessor;
			_assetProcessor = assetProcessor;
		}

		public async Task<IActionResult> AssignedPairs (int id)
		{
			IEnumerable<Asset> assets = await _assetProcessor.GetAll();
			IEnumerable<PairConfig> assignedPairs = await _pairConfigProcessor.GetAssigned(id);

			PairConfigsModel model = new PairConfigsModel();
			model.ExchangeCode = id;
			model.ExchangeName = ExchangeCode.Create(id).Description;
			model.Configs = assignedPairs.ToList();
			model.AllAssets = assets.Select(x => new SelectListItem(x.Name, x.Code)).ToList();

			return View(model);
		}
	}
}