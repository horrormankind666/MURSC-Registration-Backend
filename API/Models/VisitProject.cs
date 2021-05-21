/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๘/๐๔/๒๕๖๔>
Modify date : <๒๑/๐๕/๒๕๖๔>
Description : <>
=============================================
*/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Models
{
    public class VisitProject
    {
		public static DataSet Set(
			string personID,
			string transProjectID)
		{
			string forContent = HttpContext.Current.Request.Headers["ForContent"];
			string deviceInfo = HttpContext.Current.Request.Headers["DeviceInfo"];
			JObject jsonObject = new JObject();

			jsonObject.Add("personID", personID);
			jsonObject.Add("transProjectID", transProjectID);
			jsonObject.Add("forContent", (!String.IsNullOrEmpty(forContent) ? ("for " + forContent) : string.Empty));
			jsonObject.Add("deviceInfo", deviceInfo);
			jsonObject.Add("actionIP", Util.GetIP());

			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscSetVisitProject",
				new SqlParameter("@jsonData", JsonConvert.SerializeObject(jsonObject)));

			return ds;
		}
	}
}