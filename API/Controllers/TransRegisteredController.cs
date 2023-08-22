/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๗/๐๕/๒๕๖๓>
Modify date : <๐๘/๐๖/๒๕๖๕>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using API.Models;

namespace API.Controllers {
	[RoutePrefix("TransRegistered")]
	public class TransRegisteredController: ApiController {
		[Route("GetList")]
		[HttpGet]
		public HttpResponseMessage GetList(string paymentStatus = null) {
			List<object> list = new List<object>();

            if (Util.GetIsAuthenticatedByAuthenADFS()) {
				object obj = Util.GetPPIDByAuthenADFS();
				string ppid = obj.GetType().GetProperty("ppid").GetValue(obj, null).ToString();
				string winaccountName = obj.GetType().GetProperty("winaccountName").GetValue(obj, null).ToString();

				DataSet ds = TransRegistered.GetList((!String.IsNullOrEmpty(ppid) ? ppid : winaccountName), paymentStatus);
				
				list = TransRegistered.GetDataSource("TransRegistered", ds.Tables[0]);
			}

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}
		/*
        [Route("SameProject/GetList")]
        [HttpGet]
        public HttpResponseMessage GetListSameProject(string cuid = null) {
            string transProjectID = String.Empty;
            string[] cuidArray = Util.CUID2Array(cuid);

            if (cuidArray != null) {
                foreach (var data in cuidArray) {
                    transProjectID = data;
                }
            }

            List<object> list = new List<object>();

            if (Util.GetIsAuthenticatedByAuthenADFS()) {
                object obj = Util.GetPPIDByAuthenADFS();
                string ppid = obj.GetType().GetProperty("ppid").GetValue(obj, null).ToString();
                string winaccountName = obj.GetType().GetProperty("winaccountName").GetValue(obj, null).ToString();
				
				DataSet ds = TransRegistered.GetListSameProject((!String.IsNullOrEmpty(ppid) ? ppid : winaccountName), transProjectID);

				list.Add(ds.Tables[0]);

            }

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list[0]));
        }
		*/
        [Route("Get")]
		[HttpGet]
		public HttpResponseMessage Get(string cuid = null) {
			string transRegisteredID = String.Empty;
			string transProjectID = String.Empty;
			string[] cuidArray = Util.CUID2Array(cuid);
			
			if (cuidArray != null) {
				int i = 1;

				foreach (var data in cuidArray) {
					if (i.Equals(1)) transRegisteredID = data;
					if (i.Equals(2)) transProjectID = data;

					i++;
				}
			}

			List<object> list = new List<object>();
			
			if (Util.GetIsAuthenticatedByAuthenADFS()) {
				object obj = Util.GetPPIDByAuthenADFS();
				string ppid = obj.GetType().GetProperty("ppid").GetValue(obj, null).ToString();
				string winaccountName = obj.GetType().GetProperty("winaccountName").GetValue(obj, null).ToString();

				DataSet ds = TransRegistered.Get(transRegisteredID, (!String.IsNullOrEmpty(ppid) ? ppid : winaccountName), transProjectID);
                DataTable dtTransRegistered = ds.Tables[0];
                DataTable dtTransInvoiceFee = ds.Tables[1];
                DataTable dtTransFeeType = ds.Tables[2];

                if (dtTransRegistered.Rows.Count > 0) {
                    List<object> listTransRegisters = TransRegistered.GetDataSource("TransRegistered", dtTransRegistered);
                    List<object> transInvoiceFees = TransRegistered.GetDataSource("TransInvoiceFee", dtTransInvoiceFee);
                    List<object> transFeeTypes = TransRegistered.GetDataSource("TransFeeType", dtTransFeeType);

                    JObject transRegistered = new JObject(JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(listTransRegisters[0])));

                    transRegistered.Add("invoiceFee", JToken.FromObject(transInvoiceFees));
                    transRegistered.Add("transFeeType", JToken.FromObject(transFeeTypes));

                    list.Add(transRegistered);
                }
            }
			
			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}

		[Route("Post")]
		[HttpPost]
		public HttpResponseMessage Post() {
			string jsonData = String.Empty;
                
			if (Util.GetIsAuthenticatedByAuthenADFS())
				jsonData = Request.Content.ReadAsStringAsync().Result;

			if (!String.IsNullOrEmpty(jsonData)) {
				try {
					JObject jsonObject = new JObject(JsonConvert.DeserializeObject<dynamic>(jsonData));
					object obj = Util.GetPPIDByAuthenADFS();
					string ppid = obj.GetType().GetProperty("ppid").GetValue(obj, null).ToString();
					string winaccountName = obj.GetType().GetProperty("winaccountName").GetValue(obj, null).ToString();
					string email = obj.GetType().GetProperty("email").GetValue(obj, null).ToString();

                    jsonObject.Add("personID", (!String.IsNullOrEmpty(ppid) ? ppid : winaccountName));
					jsonObject.Add("createdBy", winaccountName);
					jsonObject.Add("email", email);

					jsonData = JsonConvert.SerializeObject(jsonObject);
				}
				catch {
					jsonData = String.Empty;
				}
			}

			DataTable dt = TransRegistered.Set("POST", jsonData).Tables[0];

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(dt));
		}

		[Route("Put")]
		[HttpPut]
		public HttpResponseMessage Put() {
			string jsonData = String.Empty;

			if (Util.GetIsAuthenticatedByAuthenADFS())
				jsonData = Request.Content.ReadAsStringAsync().Result;

			if (!String.IsNullOrEmpty(jsonData)) {
				try {
					JObject jsonObject = new JObject(JsonConvert.DeserializeObject<dynamic>(jsonData));
					object obj = Util.GetPPIDByAuthenADFS();
					string ppid = obj.GetType().GetProperty("ppid").GetValue(obj, null).ToString();
					string winaccountName = obj.GetType().GetProperty("winaccountName").GetValue(obj, null).ToString();
                    string email = obj.GetType().GetProperty("email").GetValue(obj, null).ToString();

                    jsonObject.Add("personID", (!String.IsNullOrEmpty(ppid) ? ppid : winaccountName));
					jsonObject.Add("createdBy", winaccountName);
                    jsonObject.Add("email", email);

                    jsonData = JsonConvert.SerializeObject(jsonObject);
				}
				catch {
					jsonData = String.Empty;
				}
			}

			DataTable dt = TransRegistered.Set("PUT", jsonData).Tables[0];

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(dt));
		}
	}
}
