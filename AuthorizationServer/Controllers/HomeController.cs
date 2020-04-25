/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๒/๑๒/๒๕๖๒>
Modify date : <๑๗/๑๒/๒๕๖๒>
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
            string token = String.Empty;

            if (!Request.IsAuthenticated)
                Response.Redirect(Url.Action("SignIn", "Authen"));
            else
            {           
            
                Claims c = new Claims();
                dynamic u = c.UserInfo();

                token = u.openID.id_token;

                char[] tokenArray = token.ToCharArray();
                Array.Reverse(tokenArray);
                token = new string(tokenArray);

                byte[] encDataByte = new byte[token.Length];
                encDataByte = Encoding.UTF8.GetBytes(token);
                token = Convert.ToBase64String(encDataByte);            
            }

            ViewBag.Title = "Mahidol University Authorization Server";
            ViewBag.Token = token;

            return View();
        }
    }
}