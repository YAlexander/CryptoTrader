using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers
{
	public class AccountsController : BaseController
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}