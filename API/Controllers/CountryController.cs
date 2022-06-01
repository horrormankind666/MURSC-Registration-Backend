/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๔/๐๕/๒๕๖๓>
Modify date : <๓๑/๐๕/๒๕๖๔>
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
	[RoutePrefix("Country")]
	public class CountryController: ApiController {
        [Route("GetList")]
		[HttpGet]
		public HttpResponseMessage GetList(
			string keyword = "",
			string cancelledStatus = "",
			string sortOrderBy = "",
			string sortExpression = ""
		) {
			DataSet ds = Country.GetList(keyword, cancelledStatus, sortOrderBy, sortExpression);
			List<object> list = Country.GetDataSource(ds.Tables[0]);

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}

		[Route("Get")]
		[HttpGet]
		public HttpResponseMessage Get(string country = "") {
            DataTable dt = Country.Get(country);
            List<object> list = Country.GetDataSource(dt);

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}
	}
}