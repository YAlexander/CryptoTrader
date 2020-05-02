using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers
{
	public class OrdersController : BaseController
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}