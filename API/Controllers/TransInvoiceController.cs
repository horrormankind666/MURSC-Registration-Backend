/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๕/๑๐/๒๕๖๓>
Modify date : <๒๓/๑๒/๒๕๖๕>
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
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace API.Controllers {
	[RoutePrefix("TransInvoice")]
	public class TransInvoiceController: ApiController {
        [Route("Get")]
        [HttpGet]
        public HttpResponseMessage Get(string cuid = null) {
            string transInvoiceID = String.Empty;
            string transRegisteredID = String.Empty;
            string[] cuidArray = Util.CUID2Array(cuid);

            if (cuidArray != null) {
                int i = 1;

                foreach (var data in cuidArray) {
                    if (i.Equals(1)) transInvoiceID = data;
                    if (i.Equals(2)) transRegisteredID = data;

                    i++;
                }
            }

            List<object> list = new List<object>();

            if (Util.GetIsAuthenticatedByAuthenADFS()) {
                DataSet ds = TransInvoice.Get(transInvoiceID, transRegisteredID);

                list = TransInvoice.GetDataSource(ds.Tables[0]);
            }

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
        }

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
