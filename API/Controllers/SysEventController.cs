/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๔/๐๕/๒๕๖๔>
Modify date : <๒๔/๐๕/๒๕๖๔>
Description : <>
=============================================
*/

using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using API.Models;

namespace API.Controllers
{
	[RoutePrefix("SysEvent")]
	public class SysEventController : ApiController
	{
		[Route("Post")]
		[HttpPost]
		public HttpResponseMessage Post()
		{
			string jsonData = Request.Content.ReadAsStringAsync().Result;
			string url = String.Empty;

			if (!String.IsNullOrEmpty(jsonData))
			{
				try
				{
					JObject jsonObject = new JObject(JsonConvert.DeserializeObject<dynamic>(jsonData));
					url = jsonObject["url"].ToString();
				}
				catch
				{
					url = String.Empty;
				}
			}

			string personID = String.Empty;

			if (Util.GetIsAuthenticatedByAuthenADFS())
			{
				object obj = Util.GetPPIDByAuthenADFS();
				string ppid = obj.GetType().GetProperty("ppid").GetValue(obj, null).ToString();
				string winaccountName = obj.GetType().GetProperty("winaccountName").GetValue(obj, null).ToString();

				personID = (!String.IsNullOrEmpty(ppid) ? ppid : winaccountName);
			}

			DataTable dt = SysEvent.Set(url, String.Empty, personID).Tables[0];

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(dt));
		}
	}
}
