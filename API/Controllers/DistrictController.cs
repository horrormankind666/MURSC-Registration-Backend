/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๔/๐๕/๒๕๖๓>
Modify date : <๓๑/๐๕/๒๕๖๕>
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
	[RoutePrefix("District")]
	public class DistrictController: ApiController {
        [Route("GetList")]
		[HttpGet]
		public HttpResponseMessage GetList(
			string keyword = "",
			string country = "",
			string province = "",
			string cancelledStatus = "",
			string sortOrderBy = "",
			string sortExpression = ""
		) {
			DataSet ds = District.GetList(keyword, country, province, cancelledStatus, sortOrderBy, sortExpression);
            List<object> list = District.GetDataSource(ds.Tables[0]);

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}

		[Route("Get")]
		[HttpGet]
		public HttpResponseMessage Get(
			string country = "",
			string province = "",
			string district = ""
		) {
			DataTable dt = District.Get(country, province, district);
            List<object> list = District.GetDataSource(dt);

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}
	}
}
