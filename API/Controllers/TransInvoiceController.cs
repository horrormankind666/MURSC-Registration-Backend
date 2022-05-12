/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๕/๑๐/๒๕๖๓>
Modify date : <๐๕/๑๐/๒๕๖๓>
Description : <>
=============================================
*/

using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using API.Models;

namespace API.Controllers {
	[RoutePrefix("TransInvoice")]
	public class TransInvoiceController: ApiController {
		[Route("Put")]
		[HttpPut]
		public HttpResponseMessage Put() {
			string jsonData = String.Empty;
			string transRegisteredID = String.Empty;
			string fee = String.Empty;
			string createdBy = String.Empty;


			if (Util.GetIsAuthenticatedByAuthenADFS())
				jsonData = Request.Content.ReadAsStringAsync().Result;

			if (!String.IsNullOrEmpty(jsonData)) {
				try {
					dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonData);
					object obj = Util.GetPPIDByAuthenADFS();
					string ppid = obj.GetType().GetProperty("ppid").GetValue(obj, null).ToString();
					string winaccountName = obj.GetType().GetProperty("winaccountName").GetValue(obj, null).ToString();

					transRegisteredID = jsonObject["transRegisteredID"];
					fee = (jsonObject["fee"] != null ? JsonConvert.SerializeObject(jsonObject["fee"]) : jsonObject["fee"]);
					createdBy = winaccountName;
				}
				catch {
				}
			}

			DataTable dt = TransInvoice.Set(transRegisteredID, fee, createdBy).Tables[0];

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(dt));
		}
	}
}
