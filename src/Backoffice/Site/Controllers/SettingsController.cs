using Microsoft.AspNetCore.Mvc;
using Site.Models;

namespace Site.Controllers
{
	public class SettingsController : BaseController
	{
		public IActionResult ExchangeSettings()
		{
			return View(new ExchangeSettingsModel());
		}
	}
}