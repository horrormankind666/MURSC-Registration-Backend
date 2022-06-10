/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๙/๐๖/๒๕๖๕>
Modify date : <๐๙/๐๖/๒๕๖๕>
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

namespace API.Controllers {
    [RoutePrefix("TransPrivilege")]
    public class TransPrivilegeController: ApiController {
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

                    jsonObject.Add("personID", (!String.IsNullOrEmpty(ppid) ? ppid : winaccountName));
                    jsonObject.Add("createdBy", winaccountName);

                    jsonData = JsonConvert.SerializeObject(jsonObject);
                }
                catch {
                    jsonData = String.Empty;
                }
            }

            DataTable dt = TransPrivilege.Set("POST", jsonData).Tables[0];

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(dt));
        }

    }
}
