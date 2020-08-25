/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๕/๐๘/๒๕๖๓>
Modify date : <๒๕/๐๘/๒๕๖๓>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
	[RoutePrefix("BankQRCodeNoti")]
	public class BankQRCodeNotiController : ApiController
	{
		[Route("Post")]
		[HttpPost]
		public HttpResponseMessage Post()
		{
			string jsonData = Request.Content.ReadAsStringAsync().Result;
			string xmlData = String.Empty;
			List<object> list = new List<object>();

			if (!String.IsNullOrEmpty(jsonData))
			{
				try
				{
					UtilService.iUtil iUtilService = new UtilService.iUtil();
					JObject jsonObject = new JObject(JsonConvert.DeserializeObject<dynamic>(jsonData));

					xmlData = String.Format(
						"<row>" +
						"<bankRequest>{0}</bankRequest>" +
						"<sendingBankCode>{1}</sendingBankCode>" +
						"<payeeProxyId>{2}</payeeProxyId>" +
						"<amount>{3}</amount>" +
						"<transactionId>{4}</transactionId>" +
						"<transactionDateandTime>{5}</transactionDateandTime>" +
						"<billPaymentRef1>{6}</billPaymentRef1>" +
						"<billPaymentRef2>{7}</billPaymentRef2>" +
						"<billPaymentRef3>{8}</billPaymentRef3>" +
						"<confirmId>{9}</confirmId>" +
						"<ip>{10}</ip>" +
						"</row>",
						jsonData,
						jsonObject["sendingBankCode"],
						jsonObject["payeeProxyId"],
						jsonObject["amount"],
						jsonObject["transactionId"],
						jsonObject["transactionDateandTime"],
						jsonObject["billPaymentRef1"],
						jsonObject["billPaymentRef2"],
						jsonObject["billPaymentRef3"],
						jsonObject["confirmId"],
						iUtilService.GetIP()
					);

					Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscSetBankQRCodeNoti",
						new SqlParameter("@xmlData", xmlData));

					list.Add(new
					{
						errorCode = "0",
						message = "Success",
						transactionId = jsonObject["transactionId"],
						confirmId = jsonObject["confirmId"]
					});
				}
				catch (Exception ex)
				{
					list.Add(new
					{
						errorCode = "1",
						message = ex.Message
					});
				}
			}

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}
	}
}