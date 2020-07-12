using System.Web.Mvc;

namespace AuthorizationServer.Controllers
{
	public class ErrorController : Controller
	{
		public ActionResult Index(string message)
		{
			ViewBag.Message = message;

			return View("Error");
		}
	}
}