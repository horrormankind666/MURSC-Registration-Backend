/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๑/๑๑/๒๕๖๓>
Modify date : <๐๑/๐๖/๒๕๖๕>
Description : <>
=============================================
*/

using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Models;

namespace API.Controllers {
	[RoutePrefix("TransSchedule")]
	public class TransScheduleController: ApiController {
        [Route("Get")]
		[HttpGet]
		public HttpResponseMessage Get(
			string projectCategory = null,
			string cuid = null
		) {
			string transProjectID = string.Empty;
			string[] cuidArray = Util.CUID2Array(cuid);

			if (cuidArray != null) {
				int i = 1;

				foreach (var data in cuidArray) {
					if (i.Equals(1)) transProjectID = data;

					i++;
				}
			}

			DataSet ds = TransSchedule.Get(projectCategory, transProjectID);
			List<object> list = TransSchedule.GetDataSource(ds.Tables[0]);

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}
	}
}
