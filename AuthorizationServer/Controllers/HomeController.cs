/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๒/๑๒/๒๕๖๒>
Modify date : <๑๒/๑๒/๒๕๖๒>
Description : <>
=============================================
*/

using System.Web.Mvc;

namespace AuthorizationServer.Controllers
{
    public class HomeController : Controller
    {       
        public ActionResult Index()
        {           
            ViewBag.Title = "Mahidol University Authorization Server";

            return View();
        }
    }
}