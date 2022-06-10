/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๙/๐๖/๒๕๖๕>
Modify date : <๐๙/๐๖/๒๕๖๕>
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
    [RoutePrefix("Privilege")]
    public class PrivilegeController: ApiController {
        [Route("Get")]
        [HttpGet]
        public HttpResponseMessage Get(string cuid = null) {
            string promoCode = String.Empty;
            string[] cuidArray = Util.CUID2Array(cuid);

            if (cuidArray != null) {
                int i = 1;

                foreach (var data in cuidArray) {
                    if (i.Equals(1))
                        promoCode = data;

                    i++;
                }
            }

            string personID = String.Empty;

            if (Util.GetIsAuthenticatedByAuthenADFS()) {
                object obj = Util.GetPPIDByAuthenADFS();
                string ppid = obj.GetType().GetProperty("ppid").GetValue(obj, null).ToString();
                string winaccountName = obj.GetType().GetProperty("winaccountName").GetValue(obj, null).ToString();

                personID = (!String.IsNullOrEmpty(ppid) ? ppid : winaccountName);
            }

            DataSet ds = Privilege.Get("", promoCode, personID);
            DataTable dtPrivilege = ds.Tables[0];
            DataTable dtTransPrivilege = ds.Tables[1];

            List<object> list = new List<object>();

            if (dtPrivilege.Rows.Count > 0) {
                List<object> privileges = Privilege.GetDataSource("Privilege", dtPrivilege);
                List<object> transPrivileges = Privilege.GetDataSource("TransPrivilege", dtTransPrivilege);

                JObject privilege = new JObject(JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(privileges[0])));

                privilege.Add("transPrivilege", JToken.FromObject(transPrivileges));

                list.Add(privilege);
            }

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
        }
    }
}