/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๗/๐๒/๒๕๖๓>
Modify date : <๐๑/๐๖/๒๕๖๕>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using API.Models;

namespace API.Controllers {
	[RoutePrefix("TransProject")]
	public class TransProjectController: ApiController {
        [Route("GetList")]
		[HttpGet]
		public HttpResponseMessage GetList(string projectCategory = null) {
            DataSet ds = TransProject.GetList(projectCategory);
			List<object> list = TransProject.GetDataSource("TransProject", ds.Tables[0]);
           
			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
        }

		[Route("Get")]
		[HttpGet]
		public dynamic Get(
			string projectCategory = null,
			string cuid = null
		) {
			string transProjectID = string.Empty;
			string[] cuidArray = Util.CUID2Array(cuid);
			
			if (cuidArray != null) {
				int i = 1;

				foreach (var data in cuidArray) {
					if (i.Equals(1))
                        transProjectID = data;

					i++;
				}
			}

			DataSet ds = TransProject.Get(projectCategory, transProjectID);
			DataTable dtTransProject = ds.Tables[0];
			DataTable dtTransLocation = ds.Tables[1];
			DataTable dtTransFeeType = ds.Tables[2];

            List<object> list = new List<object>();

			if (dtTransProject.Rows.Count > 0) {
                List<object> transProjects = TransProject.GetDataSource("TransProject", dtTransProject);
                List<object> transLocations = TransProject.GetDataSource("TransLocation", dtTransLocation);
                List<object> transFeeTypes = TransProject.GetDataSource("TransFeeType", dtTransFeeType);

                JObject transProject = new JObject(JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(transProjects[0])));

                transProject.Add("transLocation", JToken.FromObject(transLocations));
                transProject.Add("transFeeType", JToken.FromObject(transFeeTypes));

                list.Add(transProject);

				string personID = String.Empty;

				if (Util.GetIsAuthenticatedByAuthenADFS()) {
					object obj = Util.GetPPIDByAuthenADFS();
					string ppid = obj.GetType().GetProperty("ppid").GetValue(obj, null).ToString();
					string winaccountName = obj.GetType().GetProperty("winaccountName").GetValue(obj, null).ToString();

					personID = (!String.IsNullOrEmpty(ppid) ? ppid : winaccountName);
				}

				JObject parameters = new JObject();

				parameters.Add("projectCategory", projectCategory);
				parameters.Add("transProjectID", transProjectID);

				DataTable dt = SysEvent.Set(Request.RequestUri.ToString(), JsonConvert.SerializeObject(parameters), personID).Tables[0];
			}

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}
	}
}
