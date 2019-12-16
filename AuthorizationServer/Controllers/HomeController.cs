/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๒/๑๒/๒๕๖๒>
Modify date : <๑๖/๑๒/๒๕๖๒>
Description : <>
=============================================
*/

using System;
using System.Text;
using System.Web.Mvc;

namespace AuthorizationServer.Controllers
{
    public class HomeController : Controller
    {       
        public ActionResult Index()
        {
            string idToken = String.Empty;

            if (!Request.IsAuthenticated)
                Response.Redirect(Url.Action("SignIn", "Authen"));
            else
            {
                Claims c = new Claims();
                dynamic u = c.UserInfo();

                idToken = u.openID.id_token;

                char[] idTokenArray = idToken.ToCharArray();
                Array.Reverse(idTokenArray);
                idToken = new string(idTokenArray);

                byte[] encDataByte = new byte[idToken.Length];
                encDataByte = Encoding.UTF8.GetBytes(idToken);
                idToken = Convert.ToBase64String(encDataByte);
            }

            ViewBag.Title = "Mahidol University Authorization Server";
            ViewBag.IdToken = idToken;

            return View();
        }
    }
}