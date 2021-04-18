/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๘/๐๔/๒๕๖๔>
Modify date : <๑๘/๐๔/๒๕๖๔>
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
    public class TransHistory
    {
		public static DataSet Set(
			string personID,
			string transProjectID)
		{
			string contents = HttpContext.Current.Request.Headers["Contents"];
			string deviceInfo = HttpContext.Current.Request.Headers["DeviceInfo"];
			JObject jsonObject = new JObject();

			jsonObject.Add("personID", personID);
			jsonObject.Add("transProjectID", transProjectID);
			jsonObject.Add("contents", (!String.IsNullOrEmpty(contents) ? ("for " + contents) : string.Empty));
			jsonObject.Add("deviceInfo", deviceInfo);

			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscSetTransHistory",
				new SqlParameter("@jsonData", JsonConvert.SerializeObject(jsonObject)));

			return ds;
		}
	}
}