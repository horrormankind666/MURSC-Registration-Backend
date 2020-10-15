/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๒/๑๒/๒๕๖๒>
Modify date : <๒๒/๐๗/๒๕๖๓>
Description : <>
=============================================
*/

using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Web.Mvc;

namespace AuthorizationServer.Controllers
{
	public class HomeController : Controller
	{       
		public ActionResult Index()
		{
			string token = String.Empty;
			string winaccountname = String.Empty;
			string email = String.Empty;

			//if (!Request.IsAuthenticated)
			//	Response.Redirect(Url.Action("SignIn", "Authen"));
			//else
			if (Request.IsAuthenticated)
			{           
				Claims c = new Claims();
				dynamic u = c.UserInfo();

				token = u.openID.id_token;

				var handler = new JwtSecurityTokenHandler();
				var tokenS = handler.ReadToken(token) as JwtSecurityToken;

				char[] tokenArray = token.ToCharArray();
				Array.Reverse(tokenArray);
				token = new string(tokenArray);

				string dt = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss", new CultureInfo("th-TH"));
				byte[] encDataByte = new byte[dt.Length];
				encDataByte = Encoding.UTF8.GetBytes(dt);
				token = (Convert.ToBase64String(encDataByte) + token);

				/*
				byte[] encDataByte = new byte[token.Length];
				encDataByte = Encoding.UTF8.GetBytes(token);
				token = Convert.ToBase64String(encDataByte);
				*/

				foreach (Claim claim in tokenS.Claims)
				{
					if (claim.Type.Equals("winaccountname"))
						winaccountname = claim.Value;

					if (claim.Type.Equals("email"))
						email = claim.Value;
				}
			}
            
			ViewBag.Title = "Mahidol University Authorization Server";
			ViewBag.Token = token;
			ViewBag.Email = (!String.IsNullOrEmpty(email) ? email.Split('@')[0] : winaccountname);

			return View();
		}
	}
}