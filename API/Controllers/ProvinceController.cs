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
	[RoutePrefix("Province")]
	public class ProvinceController: ApiController {
        [Route("GetList")]
		[HttpGet]
		public HttpResponseMessage GetList(
			string keyword = "",
			string country = "",
			string cancelledStatus = "",
			string sortOrderBy = "",
			string sortExpression = ""
		) {
			DataSet ds = Province.GetList(keyword, country, cancelledStatus, sortOrderBy, sortExpression);
            List<object> list = Province.GetDataSource(ds.Tables[0]);

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}

		[Route("Get")]
		[HttpGet]
		public HttpResponseMessage Get(
			string country = "",
			string province = ""
		) {
			DataTable dt = Province.Get(country, province);
            List<object> list = Province.GetDataSource(dt);

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}
	}
}
