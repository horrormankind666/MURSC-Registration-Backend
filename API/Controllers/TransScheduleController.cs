/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๑/๑๑/๒๕๖๓>
Modify date : <๑๑/๑๑/๒๕๖๓>
Description : <>
=============================================
*/

using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using API.Models;

namespace API.Controllers
{
	[RoutePrefix("TransSchedule")]
	public class TransScheduleController : ApiController
	{
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

			DataSet ds = TransSchedule.Get(projectCategory, transProjectID);
			List<object> list = new List<object>();

			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				list.Add(new
				{
					transScheduleID = dr["ID"],				
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
					section = dr["section"],
					schedules = JsonConvert.DeserializeObject<dynamic>(dr["schedules"].ToString())
				});
			}

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}
	}
}
