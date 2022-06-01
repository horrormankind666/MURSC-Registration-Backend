/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๑/๐๘/๒๕๖๓>
Modify date : <๒๓/๐๘/๒๕๖๓>
Description : <>
=============================================
*/

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Models;

namespace API.Controllers {
	[RoutePrefix("StudentProfile")]
	public class StudentProfileController: ApiController {
		[Route("Get")]
		[HttpGet]
		public HttpResponseMessage Get(string studentCode = null) {
			List<object> list = new List<object>();

			if (Util.GetIsAuthenticatedByAuthenADFS()) {
                DataSet ds = StudentProfile.Get(studentCode);
				DataTable dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                    list.Add(StudentProfile.GetDataSource(dt)[0]);
			}

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}
	}
}
