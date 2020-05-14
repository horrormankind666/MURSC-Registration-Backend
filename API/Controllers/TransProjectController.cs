/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๗/๐๒/๒๕๖๓>
Modify date : <๐๕/๐๕/๒๕๖๓>
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

namespace API.Controllers
{
    [RoutePrefix("TransProject")]
    public class TransProjectController : ApiController
    {
        [Route("GetList")]
        [HttpGet]
        public HttpResponseMessage GetList()
        {
            DataTable dt = TransProject.GetList().Tables[0];

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(dt));
        }

        [Route("Get")]
        [HttpGet]
        public HttpResponseMessage Get(string transProjectID = null)
        {
            DataSet ds = TransProject.Get(transProjectID);
            DataTable dt1 = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            DataTable dt3 = ds.Tables[2];

            List<object> list1 = new List<object>();
            List<object> list2 = new List<object>();
            List<object> list3 = new List<object>();

            if (dt1.Rows.Count > 0)
            {
                DataRow dr = dt1.Rows[0];

                list1.Add(new
                {
                    transProjectID = dr["transProjectID"],
                    projectID = dr["projectID"],
                    logo = dr["logo"],
                    projectNameTH = dr["projectNameTH"],
                    projectNameEN = dr["projectNameEN"],
                    detail = dr["detail"],
                    examDate = dr["examDate"],
                    examDates = dr["examDates"],
                    regisStartDate = dr["regisStartDate"],
                    regisStartDates = dr["regisStartDates"],
                    regisEndDate = dr["regisEndDate"],
                    regisEndDates = dr["regisEndDates"],
                    lastPaymentDate = dr["lastPaymentDate"],
                    lastPaymentDates = dr["lastPaymentDates"],
                    maximumSeat = dr["maximumSeat"],
                    minimumFee = dr["minimumFee"],
                    contactNameTH = dr["contactNameTH"],
                    contactNameEN = dr["contactNameEN"],
                    contactEmail = dr["contactEmail"],
                    contactPhone = dr["contactPhone"],
                    registrationStatus = dr["registrationStatus"],
                    location = (dt2.Rows.Count > 0 ? dt2.Rows[0].Table : null),
                    feeType = (dt3.Rows.Count > 0 ? dt3.Rows[0].Table : null)
                });
            }

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list1.Union(list2).Union(list3).ToList()));
        }
    }
}
