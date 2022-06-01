/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๘/๐๖/๒๕๖๓>
Modify date : <๒๘/๐๕/๒๕๖๕>
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
	[RoutePrefix("ProjectCategory")]
	public class ProjectCategoryController: ApiController {
        [Route("GetList")]
		[HttpGet]
		public HttpResponseMessage GetList() {
            DataSet ds = ProjectCategory.GetList();
            List<object> list = ProjectCategory.GetDataSource(ds.Tables[0]);

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
        }

		[Route("Get")]
		[HttpGet]
		public HttpResponseMessage Get(string projectCategory = "") {
            DataSet ds = ProjectCategory.Get(projectCategory);
            List<object> list = ProjectCategory.GetDataSource(ds.Tables[0]);

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}
	}
}
