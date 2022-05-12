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
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers {
	[RoutePrefix("StudentProfile")]
	public class StudentProfileController: ApiController {
		[Route("Get")]
		[HttpGet]
		public HttpResponseMessage Get(string studentCode = null) {
			List<object> list = new List<object>();

			if (Util.GetIsAuthenticatedByAuthenADFS()) {
				DataSet ds = Util.ExecuteCommandStoredProcedure(Util.infinityConnectionString, "sp_rscGetStudentProfile",
					new SqlParameter("@studentCode", studentCode));

				DataTable dt = ds.Tables[0];

				if (dt.Rows.Count > 0) {
					DataRow dr = dt.Rows[0];

					list.Add(new {
						studentCode = dr["studentCode"].ToString(),
						titleTH = dr["titleTH"].ToString(),
						titleEN	= dr["titleEN"].ToString(),
						firstNameTH	= dr["firstName"].ToString(),
						middleNameTH = dr["middleName"].ToString(),
						lastNameTH = dr["lastName"].ToString(),
						firstNameEN	= dr["firstNameEN"].ToString(),
						middleNameEN = dr["middleNameEN"].ToString(),
						lastNameEN = dr["lastNameEN"].ToString(),
						facultyNameTH = dr["facultyNameTH"].ToString(),
						facultyNameEN = dr["facultyNameEN"].ToString(),
						programNameTH = dr["programNameTH"].ToString(),
						programNameEN = dr["programNameEN"].ToString(),
						address = dr["address"].ToString(),
						subdistrict = dr["subdistrict"].ToString(),
						district = dr["district"].ToString(),
						province = dr["province"].ToString(),
						country = dr["country"].ToString(),
						zipCode = dr["zipCode"].ToString(),
						phoneNumber = dr["phoneNumber"].ToString()
					});
				}
			}

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list.ToList()));
		}
	}
}
