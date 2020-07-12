/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๔/๐๗/๒๕๖๓>
Modify date : <๑๒/๐๗/๒๕๖๓>
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
using RestSharp;
using System.Text;
using Newtonsoft.Json;
using API.Models;

namespace API.Controllers
{
	[RoutePrefix("QRCodePayment")]
	public class QRCodePaymentController : ApiController
	{
		[Route("{projectCategory}/Get")]
		[HttpPost]
		public HttpResponseMessage Get(string projectCategory)
		{
			List<object> list = new List<object>();

			if (Util.GetIsAuthenticatedByAuthenADFS())
			{
				string jsonData = Request.Content.ReadAsStringAsync().Result;
				string transRegisteredID = String.Empty;
				string personID = String.Empty;
				string transProjectID = String.Empty;
				string taxNo = String.Empty;
				string suffix = String.Empty;
				string billerID = String.Empty;
				string campus = String.Empty;
				string profitCenter = String.Empty;
				string branchNo = String.Empty;
				string subBranch = String.Empty;
				string merchantName = String.Empty;
				bool isError = false;
				object scbReq = null;

				if (!String.IsNullOrEmpty(jsonData))
				{
					try
					{
						dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonData);

						transRegisteredID = jsonObject["transRegisteredID"];
						personID = jsonObject["personID"];
						transProjectID = jsonObject["transProjectID"];
					}
					catch
					{
					}
				}

				if (projectCategory.Equals("CBE"))
				{
					if (!String.IsNullOrEmpty(transRegisteredID) && !String.IsNullOrEmpty(personID) && !String.IsNullOrEmpty(transProjectID))
					{
						string invoiceID = String.Empty;
						string totalFeeAmount = String.Empty;
						string lastPaymentDate = String.Empty;

						DataSet ds1 = Util.ExecuteCommandStoredProcedure(Util.infinityConnectionString, "sp_rscGetFinPayBankQRCode",
							new SqlParameter("@systemRef", "MU_MURSC-CBX"),
							new SqlParameter("@profitCenter", "003"),
							new SqlParameter("@branchNo", "001"),
							new SqlParameter("@subBranch", "001"));

						DataTable dt1 = ds1.Tables[0];

						if (dt1.Rows.Count > 0)
						{
							DataRow dr1 = dt1.Rows[0];

							taxNo = dr1["taxNo"].ToString();
							suffix = dr1["suffix"].ToString();
							billerID = (taxNo + suffix);
							campus = dr1["campus"].ToString();
							profitCenter = dr1["profitCenter"].ToString();
							branchNo = dr1["branchNo"].ToString();
							subBranch = dr1["subBranch"].ToString();
							merchantName = dr1["systemRef"].ToString();

							DataSet ds2 = TransRegistered.Get(transRegisteredID, personID, transProjectID);
							DataTable dt2 = ds2.Tables[0];
							DataRow[] dr2 = dt2.Select("(transRegisteredID = '" + transRegisteredID + "') and (personID = '" + personID + "') and (transProjectID = '" + transProjectID + "')");

							personID = dr2.Length.ToString();

							if (dr2.Length > 0)
							{
								personID = dr2[0]["personID"].ToString();
								invoiceID = dr2[0]["invoiceID"].ToString();
								totalFeeAmount = String.Format("{0:0.00}", dr2[0]["totalFeeAmount"]);
								lastPaymentDate = dr2[0]["lastPaymentDateForQRCode"].ToString();
							}
							else
								isError = true;
						}
						else
							isError = true;

						if (isError.Equals(false))
						{
							scbReq = new
							{
								biller_id = billerID,
								merchant_name = merchantName,
								amount = totalFeeAmount,
								ref_1 = (campus + profitCenter + branchNo + subBranch + invoiceID.Substring(3)),
								ref_2 = (personID.Substring(0, 14) + lastPaymentDate),
								ref_3 = personID
							};
						}
					}
				}

				string username = "mursc";
				string password = "MURSC#2020#";
				string tokenBasicEncoded = String.Format("{0}{1}", "Basic ", Convert.ToBase64String(Encoding.GetEncoding("UTF-8").GetBytes(username + ":" + password)));

				var client = new RestClient("https://smartedu.mahidol.ac.th/scbapi/muBarcode/muQrCodeGen");
				var request = new RestRequest(Method.POST);

				request.AddHeader("Authorization", tokenBasicEncoded);
				request.AddHeader("cache-control", "no-cache");
				request.AddHeader("content-type", "application/json");
				request.AddParameter("application/json", JsonConvert.SerializeObject(scbReq), ParameterType.RequestBody);

				IRestResponse response = client.Execute(request);				
				dynamic qrCodeObj = JsonConvert.DeserializeObject(response.Content);

				list.Add(new
				{
					qrCode = qrCodeObj.qr_code,
					qrMessage = qrCodeObj.qr_message,
					qrFormat = qrCodeObj.qr_format,
					qrImage64 = qrCodeObj.qr_image64,
					qrResponse = qrCodeObj.qr_response,
					qrNewRef1 = qrCodeObj.qr_new_ref_1
				});
			}

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list.ToList()));
		}
	}
}
