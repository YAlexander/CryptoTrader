using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers
{
	public class SettingsController : BaseController
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}