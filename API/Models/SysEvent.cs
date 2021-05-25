/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๔/๐๕/๒๕๖๔>
Modify date : <๒๔/๐๕/๒๕๖๔>
Description : <>
=============================================
*/

using System.Data;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Models
{
    public class SysEvent
    {
		public static DataSet Set(
			string url,
			string parameters,
			string personID)
		{
			string authorization = HttpContext.Current.Request.Headers["Authorization"];
			string cookie = HttpContext.Current.Request.Headers["Cookie"];
			string deviceInfo = HttpContext.Current.Request.Headers["DeviceInfo"];
			string remark = HttpContext.Current.Request.Headers["Remark"];
			JObject jsonObject = new JObject();
			JObject header = new JObject();

			header.Add("Authorization", authorization);

			jsonObject.Add("url", url);
			jsonObject.Add("parameters", parameters);
			jsonObject.Add("headers", JsonConvert.SerializeObject(header));
			jsonObject.Add("cookie", (Util.CookieExist(Util.cookieName) ? Util.GetCookie(Util.cookieName).Value : string.Empty));
			jsonObject.Add("deviceInfo", deviceInfo);
			jsonObject.Add("remark", remark);
			jsonObject.Add("actionBy", personID);
			jsonObject.Add("actionIP", Util.GetIP());

			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscSetSysEvent",
				new SqlParameter("@jsonData", JsonConvert.SerializeObject(jsonObject)));

			return ds;
		}
	}
}