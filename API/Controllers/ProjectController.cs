﻿/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๗/๐๒/๒๕๖๓>
Modify date : <๒๖/๐๓/๒๕๖๓>
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
    [RoutePrefix("Project")]
    public class ProjectController : ApiController
    {
        [Route("GetList")]
        [HttpGet]
        public HttpResponseMessage GetList()
        {
            DataTable dt = Project.GetList().Tables[0];

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(dt));
        }

        [Route("Get")]
        [HttpGet]
        public HttpResponseMessage Get(string transProjectID = null)
        {
            DataSet ds = Project.Get(transProjectID);
            DataTable dt1 = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];

            List<object> list1 = new List<object>();
            List<object> list2 = new List<object>();

            if (dt1.Rows.Count > 0)
            {
                DataRow dr = dt1.Rows[0];

                list1.Add(new
                {
                    transProjectID = dr["transProjectID"],
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
                    contactNameTH = dr["contactNameTH"],
                    contactNameEN = dr["contactNameEN"],
                    contactEmail = dr["contactEmail"],
                    contactPhone = dr["contactPhone"],
                    registrationStatus = dr["registrationStatus"],
                    location = (dt2.Rows.Count > 0 ? dt2.Rows[0].Table : null)                    
                });
            }

            return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list1.Union(list2).ToList()));
        }
    }
}