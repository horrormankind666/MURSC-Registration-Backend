/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๗/๐๒/๒๕๖๓>
Modify date : <๑๘/๐๔/๒๕๖๔>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using API.Models;

namespace API.Controllers
{
	[RoutePrefix("TransProject")]
	public class TransProjectController : ApiController
	{
		[Route("GetList")]
		[HttpGet]
		public HttpResponseMessage GetList(string projectCategory = null)
		{
			DataSet ds = TransProject.GetList(projectCategory);
			List<object> list = new List<object>();

			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				list.Add(new
				{
					transProjectID = dr["transProjectID"],
					projectCategoryID = dr["projectCategoryID"],
					projectCategoryNameTH = dr["projectCategoryNameTH"],
					projectCategoryNameEN = dr["projectCategoryNameEN"],
					projectCategoryInitial = dr["projectCategoryInitial"],
					projectID = dr["projectID"],					
					logo = dr["logo"],
					projectNameTH = dr["projectNameTH"],
					projectNameEN = dr["projectNameEN"],
					descriptionTH = dr["descriptionTH"],
					descriptionEN = dr["descriptionEN"],
					aboutTH = dr["aboutTH"],
					aboutEN = dr["aboutEN"],
					examStartDate = dr["examStartDate"],
					examStartDates = dr["examStartDates"],
					examEndDate = dr["examEndDate"],
					examEndDates = dr["examEndDates"],
					regisStartDate = dr["regisStartDate"],
					regisStartDates = dr["regisStartDates"],
					regisEndDate = dr["regisEndDate"],
					regisEndDates = dr["regisEndDates"],
					lastPaymentDate = dr["lastPaymentDate"],
					lastPaymentDates = dr["lastPaymentDates"],
					maximumSeat = dr["maximumSeat"],
					seatReserved = dr["seatReserved"],
					minimumFee = dr["minimumFee"],
					contactPerson = JsonConvert.DeserializeObject<dynamic>(dr["contactPerson"].ToString()),
					registrationStatus = dr["registrationStatus"],
					userTypeSpecific = (!String.IsNullOrEmpty(dr["userTypeSpecific"].ToString()) ? dr["userTypeSpecific"].ToString().Split(',') : dr["userTypeSpecific"]),
					isExam = dr["isExam"],
					isTeaching = dr["isTeaching"]
				});
			}

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}

		[Route("Get")]
		[HttpGet]
		public HttpResponseMessage Get(
			string projectCategory = null,
			string cuid = null
		)
		{
			string transProjectID = string.Empty;
			string[] cuidArray = Util.CUID2Array(cuid);
			
			if (cuidArray != null)
			{
				int i = 1;

				foreach (var data in cuidArray)
				{
					if (i.Equals(1)) transProjectID = data;

					i++;
				}
			}

			DataSet ds = TransProject.Get(projectCategory, transProjectID);
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
					projectCategoryID = dr["projectCategoryID"],
					projectCategoryNameTH = dr["projectCategoryNameTH"],
					projectCategoryNameEN = dr["projectCategoryNameEN"],
					projectCategoryInitial = dr["projectCategoryInitial"],
					projectID = dr["projectID"],					
					logo = dr["logo"],
					projectNameTH = dr["projectNameTH"],
					projectNameEN = dr["projectNameEN"],
					descriptionTH = dr["descriptionTH"],
					descriptionEN = dr["descriptionEN"],
					aboutTH = dr["aboutTH"],
					aboutEN = dr["aboutEN"],
					examStartDate = dr["examStartDate"],
					examStartDates = dr["examStartDates"],
					examEndDate = dr["examEndDate"],
					examEndDates = dr["examEndDates"],
					regisStartDate = dr["regisStartDate"],
					regisStartDates = dr["regisStartDates"],
					regisEndDate = dr["regisEndDate"],
					regisEndDates = dr["regisEndDates"],
					lastPaymentDate = dr["lastPaymentDate"],
					lastPaymentDates = dr["lastPaymentDates"],
					maximumSeat = dr["maximumSeat"],
					seatReserved = dr["seatReserved"],
					minimumFee = dr["minimumFee"],
					contactPerson = JsonConvert.DeserializeObject<dynamic>(dr["contactPerson"].ToString()),
					registrationStatus = dr["registrationStatus"],
					userTypeSpecific = (!String.IsNullOrEmpty(dr["userTypeSpecific"].ToString()) ? dr["userTypeSpecific"].ToString().Split(',') : dr["userTypeSpecific"]),
					isExam = dr["isExam"],
					isTeaching = dr["isTeaching"],
					location = (dt2.Rows.Count > 0 ? dt2.Rows[0].Table : null),
					feeType = (dt3.Rows.Count > 0 ? dt3.Rows[0].Table : null)
				});

				string personID = String.Empty;

				if (Util.GetIsAuthenticatedByAuthenADFS())
				{
					object obj = Util.GetPPIDByAuthenADFS();
					string ppid = obj.GetType().GetProperty("ppid").GetValue(obj, null).ToString();
					string winaccountName = obj.GetType().GetProperty("winaccountName").GetValue(obj, null).ToString();

					personID = (!String.IsNullOrEmpty(ppid) ? ppid : winaccountName);
				}

				DataTable dt = TransHistory.Set(personID, transProjectID).Tables[0];
			}
						
			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list1.Union(list2).Union(list3).ToList()));
		}
	}
}
