/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๘/๐๖/๒๕๖๓>
Modify date : <๐๘/๐๖/๒๕๖๓>
Description : <>
=============================================
*/

using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Models;

namespace API.Controllers
{
    [RoutePrefix("ProjectCategory")]
    public class ProjectCategoryController : ApiController
    {
        [Route("GetList")]
        [HttpGet]
        public HttpResponseMessage GetList()
        {
            DataTable dt = ProjectCategory.GetList().Tables[0];

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(dt));
        }
    }
}
