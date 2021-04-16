/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๕/๐๘/๒๕๖๓>
Modify date : <๒๕/๐๘/๒๕๖๓>
Description : <>
=============================================
*/

using System;
using System.Data.SqlClient;
using System.Web.Http;
using Newtonsoft.Json;

namespace API.Controllers
{
	[RoutePrefix("BankQRCodeNoti")]
	public class BankQRCodeNotiController : ApiController
	{
		[Route("Post")]
		[HttpPost]
		public dynamic Post()
		{
			string jsonData =  Request.Content.ReadAsStringAsync().Result;
			dynamic jsonObject = null;
			object result = null;

			if (!String.IsNullOrEmpty(jsonData))
			{
				try
				{
					jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonData);
					
					Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscSetBankQRCodeNoti",
						new SqlParameter("@jsonData", jsonData));				

					result = new
					{
						res_code = "00",
						res_desc = "success",
						transactionId = jsonObject["transactionId"],
						confirmId = jsonObject["confirmId"]
					};
				}
				catch (Exception ex)
				{
					result = new
					{
						res_code = "88",
						res_desc = ("error BankQRCodeNoti : " + jsonData)
					};

					try
					{
						Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscSetSysErrorLog",
							new SqlParameter("@systemName", "BankQRCodeNoti"),
							new SqlParameter("@errorNumber", "88"),
							new SqlParameter("@errorMessage", ex.Message),
							new SqlParameter("@hint", jsonData),
							new SqlParameter("@url", ""));
					}
					catch (Exception)
					{
					}
				}
			}

			return result;
		}
	}
}