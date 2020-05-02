using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers
{
	public class SupportController : BaseController
	{
		public IActionResult Index()
		{
			return View();
		}
		
		public IActionResult About()
		{
			return View();
		}
	}
}