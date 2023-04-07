/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๔/๐๗/๒๕๖๓>
Modify date : <๒๐/๑๒/๒๕๖๕>
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
using Newtonsoft.Json.Linq;
using API.Models;

namespace API.Controllers {
	[RoutePrefix("QRCodePayment")]
	public class QRCodePaymentController: ApiController {
		[Route("{projectCategory}/Put")]
		[HttpPut]
		public HttpResponseMessage Get(string projectCategory) {
			string jsonData = String.Empty;
		
			if (Util.GetIsAuthenticatedByAuthenADFS())
				jsonData = Request.Content.ReadAsStringAsync().Result;

			List<object> list = new List<object>();

			if (!String.IsNullOrEmpty(jsonData)) {
				int errorCode = 0;
                string invoiceID = String.Empty;
				string invoiceTagID = String.Empty;
                string referenceKey = String.Empty;
                string totalFeeAmount = String.Empty;
				string paymentConfirmDate = String.Empty;
                string lastPaymentDate = String.Empty;
				string taxNo = String.Empty;
				string suffix = String.Empty;
				string billerID = String.Empty;
				string ref1 = String.Empty;
				string ref2 = String.Empty;
				string ref3 = String.Empty;
				string qrNewRef1 = String.Empty;
				string campus = String.Empty;
				string profitCenter = String.Empty;
				string branchNo = String.Empty;
				string subBranch = String.Empty;
				string merchantName = String.Empty;
				string paidAmount = String.Empty;
				string paidStatus = String.Empty;
				string actionDate = String.Empty;
				dynamic qrCodeObj = null;

                try {
					JObject jsonObject = new JObject(JsonConvert.DeserializeObject<dynamic>(jsonData));
					string transRegisteredID = jsonObject["transRegisteredID"].ToString();
					string transProjectID = jsonObject["transProjectID"].ToString();

                    object obj = Util.GetPPIDByAuthenADFS();
					string ppid = obj.GetType().GetProperty("ppid").GetValue(obj, null).ToString();
					string winaccountName = obj.GetType().GetProperty("winaccountName").GetValue(obj, null).ToString();

					string personID = (!String.IsNullOrEmpty(ppid) ? ppid : winaccountName);
                    object scbReq = null;
				
					DataSet ds1 = TransRegistered.Get(transRegisteredID, personID, transProjectID);
					DataTable dt1 = ds1.Tables[0];
							
					if (dt1.Rows.Count > 0) {
						DataRow dr1 = dt1.Rows[0];
						personID = dr1["personID"].ToString();
						referenceKey = dr1["referenceKey"].ToString();
                        invoiceID = dr1["invoiceID"].ToString();
						invoiceTagID = dr1["invoiceTagID"].ToString();
						totalFeeAmount = String.Format("{0:0.00}", dr1["totalFeeAmount"]);
                        paymentConfirmDate = dr1["paymentConfirmDateForQRCode"].ToString();
                        lastPaymentDate = dr1["lastPaymentDateForQRCode"].ToString();                        

                        DataSet ds2 = ProjectCategory.Get(projectCategory);
						DataTable dt2 = ds2.Tables[0];

						if (dt2.Rows.Count > 0) {
							DataRow dr2 = dt2.Rows[0];
							string systemRef = dr2["systemRef"].ToString();

							DataSet ds3 = Util.ExecuteCommandStoredProcedure(Util.infinityConnectionString, "sp_rscGetFinPayBankQRCode",
								new SqlParameter("@systemRef", systemRef));

							DataTable dt3 = ds3.Tables[0];

							if (dt3.Rows.Count > 0) {
								DataRow dr3 = dt3.Rows[0];

								taxNo = dr3["taxNo"].ToString();
								suffix = dr3["suffix"].ToString();
								billerID = (taxNo + suffix);
								campus = dr3["campus"].ToString();
								profitCenter = dr3["profitCenter"].ToString();
								branchNo = dr3["branchNo"].ToString();
								subBranch = dr3["subBranch"].ToString();
								merchantName = dr3["systemRef"].ToString();
							}
						}
					}
					else
						errorCode = 2;

					if (errorCode.Equals(0)) {
                        /*
						ref1 = (campus + profitCenter + branchNo + subBranch + invoiceID.Substring(6));
						ref2 = (invoiceID.Substring(3) + lastPaymentDate);
						*/
                        ref1 = (campus + profitCenter + branchNo + subBranch + referenceKey);
                        ref2 = (invoiceTagID.PadLeft(14, '0') + lastPaymentDate);
                        ref3 = personID;
						
						if (ref2.Length.Equals(20)) {
							scbReq = new {
								biller_id = billerID,
								merchant_name = merchantName,
								amount = totalFeeAmount,
								ref_1 = ref1,
								ref_2 = ref2,
								ref_3 = ref3
							};

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
							qrCodeObj = JsonConvert.DeserializeObject(response.Content);

							errorCode = (qrCodeObj.qr_code == "00" ? 0 : 1);

							if (errorCode.Equals(0)) {
								if (qrCodeObj != null) {
									qrNewRef1 = qrCodeObj.qr_new_ref_1;

									if (qrNewRef1.Length.Equals(20)) {
										jsonObject.Add("personID", (!String.IsNullOrEmpty(ppid) ? ppid : winaccountName));
										jsonObject.Add("transInvoiceID", invoiceID);
										jsonObject.Add("billerID", billerID);
										jsonObject.Add("merchantName", merchantName);
										jsonObject.Add("qrRef_1", ref1);
										jsonObject.Add("qrRef_2", ref2);
										jsonObject.Add("qrRef_3", ref3);
										jsonObject.Add("qrImage", (qrCodeObj != null ? qrCodeObj.qr_image64 : null));
										jsonObject.Add("qrNewRef_1", qrNewRef1);
										jsonObject.Add("paidAmount", totalFeeAmount);
										jsonObject.Add("createdBy", winaccountName);

										jsonData = JsonConvert.SerializeObject(jsonObject);

										DataSet ds3 = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscSetTransInvoiceRef",
											new SqlParameter("@jsonData", jsonData));

										DataTable dt3 = ds3.Tables[0];

										if (dt3.Rows.Count > 0) {
											DataRow dr3 = dt3.Rows[0];

											merchantName = dr3["merchantName"].ToString();
											paidAmount = dr3["paidAmount"].ToString();
											paidStatus = dr3["paidStatus"].ToString();
											errorCode = int.Parse(dr3["errorCode"].ToString());
											actionDate = dr3["actionDate"].ToString();
										}
										else {
											errorCode = 1;
											qrCodeObj = null;
										}
									}
									else {
										errorCode = 3;
										qrCodeObj = null;
									}
								}
								else {
									errorCode = 3;
									qrCodeObj = null;
								}
							}
							else {
								errorCode = 3;
								qrCodeObj = null;
							}
						}
                        else {
                            errorCode = 3;
                            qrCodeObj = null;
                        }
                    }
					else
						errorCode = 2;
				}
				catch {
					errorCode = 1;
				}

				list.Add(new {
                    errorCode = errorCode,
					qrCode = (qrCodeObj != null ? qrCodeObj.qr_code : null),
					qrMessage = (qrCodeObj != null ? qrCodeObj.qr_message : null),
					qrFormat = (qrCodeObj != null ? qrCodeObj.qr_format : null),
					qrImage64 = (qrCodeObj != null ? qrCodeObj.qr_image64 : null),
					qrResponse = (qrCodeObj != null ? qrCodeObj.qr_response : null),
					qrRef1 = (!String.IsNullOrEmpty(ref1) ? ref1 : null),
					qrRef2 = (!String.IsNullOrEmpty(ref2) ? ref2 : null),
					qrRef3 = (!String.IsNullOrEmpty(ref3) ? ref3 : null),
					qrNewRef1 = (qrCodeObj != null ? qrNewRef1 : null),
					merchantName = (!String.IsNullOrEmpty(merchantName) ? merchantName : null),
					paidAmount = (!String.IsNullOrEmpty(paidAmount) ? paidAmount : null),
					paidStatus = (!String.IsNullOrEmpty(paidStatus) ? paidStatus : null),
					actionDate = (!String.IsNullOrEmpty(actionDate) ? actionDate : null)
				});
			}

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list.ToList()));			
		}
	}
}
