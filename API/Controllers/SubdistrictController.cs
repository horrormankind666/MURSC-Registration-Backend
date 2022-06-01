/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๔/๐๕/๒๕๖๓>
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
	[RoutePrefix("Subdistrict")]
	public class SubdistrictController: ApiController {
        [Route("GetList")]
		[HttpGet]
		public HttpResponseMessage GetList(
			string keyword = "",
			string country = "",
			string province = "",
			string district = "",
			string cancelledStatus = "",
			string sortOrderBy = "",
			string sortExpression = ""
		) {
			DataSet ds = Subdistrict.GetList(keyword, country, province, district, cancelledStatus, sortOrderBy, sortExpression);
            List<object> list = Subdistrict.GetDataSource(ds.Tables[0]);

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}

		[Route("Get")]
		[HttpGet]
		public HttpResponseMessage Get(
			string country = "",
			string province = "",
			string district = "",
			string subdistrict = ""
		) {
			DataTable dt = Subdistrict.Get(country, province, district, subdistrict);
            List<object> list = Subdistrict.GetDataSource(dt);

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}
	}
}
