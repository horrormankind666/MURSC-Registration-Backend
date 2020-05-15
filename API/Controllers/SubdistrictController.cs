/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๔/๐๕/๒๕๖๓>
Modify date : <๑๔/๐๕/๒๕๖๓>
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
    [RoutePrefix("Subdistrict")]
    public class SubdistrictController : ApiController
    {
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
        )
        {
            DataTable dt = Subdistrict.GetList(keyword, country, province, district, cancelledStatus, sortOrderBy, sortExpression).Tables[0];

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(dt));
        }

        [Route("Get")]
        [HttpGet]
        public HttpResponseMessage Get(string country = "", string province = "", string district = "", string subdistrict = "")
        {
            DataTable dt = Subdistrict.GetList("", country, province, district, "", "", "").Tables[0];
            DataRow[] dr = dt.Select("(plcCountryId = '" + country + "') and (plcProvinceId = '" + province + "') and (plcDistrictId = '" + district + "') and (id = '" + subdistrict + "')");

            dt = (dr.Length > 0 ? dr.CopyToDataTable() : dt.Clone());

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(dt));
        }
    }
}
