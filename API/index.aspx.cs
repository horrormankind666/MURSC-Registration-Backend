using API.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web.Http.Results;
using System.Web;

namespace API {
    public partial class index: Page {
        protected void Page_Load(
            object sender,
            EventArgs e
        ) {
            /*
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

            string jsonData = "{\"transRegisteredID\":\"E36CE97B-4CBB-404C-8600-7B5C0A20CE80\",\"transProjectID\":\"c2650ec6-6a2b-462d-917f-47834da8231f\",\"issueReceipt\":\"วริศา พรโสภณ\"}";
            JObject jsonObject = new JObject(JsonConvert.DeserializeObject<dynamic>(jsonData));
            string projectCategory = "CBE";
            string transRegisteredID = jsonObject["transRegisteredID"].ToString();
            string transProjectID = jsonObject["transProjectID"].ToString();
            string personID = "el7n9ru4wt5il958si";
            object scbReq = null;

            string token = "MjQvMDcvMjU2NyAxMDozMjoyNA==0mDOiwS5Ihc4PZJcs20rv6aHrlPOgQi0TdNgSLsT0Y-wOXVxPnndBK-9mFMSQgJwJOCZzJqYrBSEME47d22vq-pqx9ETSs-dmSD4hhuKThxn5uuARJGt9wGEtSddOtUCPxx7fbw3_mtloyH6cmNLsWwjX_OYKRHffbhQNX46bZAm1IUvPxrunT9lB8wbRYiY1uKqKCrqbCqhMLB94PdSgipXSWNaHkyQFypTSTXILZeySOuXTM8sHf12S861J2U-SK3-ZAgPCTNQF6qTA87xd746xWzwGyDbc7Q5pg6vUjjDMK8eoGAmJ8drPCYMRfllQHvISxURp6Bh7GFgzZubhUgWJ--7CUWZSBmJl6m58G7zarBpy-Uhj4tF_wlSzV_OhiPnxXaCgSPqtwYEllPWSzzYhCq3nicHKws8PW-itKl77RhyZHhICEil0-eKc4xQUaS4mhBDTBKJQ1JG-C1bb3Z1TN-o0ruAKBN_Q1eFKaa72lPlujwz0ER9yzeMTvzsY_k-68UZBA10zuWiOSGEmhLpJF2FJF6jRiy5zfcz6Bcu8ALeNh7OPrmCGi30TSZ83OiPjYXYzuCbLM84xQ2qveTk0c3KDWZMEps6K4y_WU1IqzkEfQ34tp0hEiNWHtHW6_JK5TKBPwPgUOQxGEp2APxE9DNEEevPB0fzNutWx7i.0nI3NkWOdnbKdjQOZTblV0RjdldUFXMwIiOig2chh2XjJCLiQWauVGcvByctlWYsNGdhxGbhJiOiA3YzJCLiAjLxIiOiIXZ2JCLiQncvB3cuFmcURWZ0NWZ09mcQRmcvd3czFGU6MXZzNXYsNmOjFmOw4iM6wUTBNlOjRnOzVWbh5mOzl2ch9mOuJXdiojIk9Ga0VWboRXdhJCLiYWOwUjM3UWN0UzYz0COwkTYtczY0QTLwMmM20iMwYjYhJzM4IiOiQWawBXYiwiIsFWa05WZklmZu92QiojIlBXe0BHchJCLik2c4UTOslWN0dHN1JXOudDblJiOiQWawBnIsIibvB3bz5mcvBlI6ISZtFmbflHbp1WYmJCLiE2cpJXYXJiOiUWbh52XuVmdpdmIsIibvBnLhNXayF2diojIl1WYuRnb192YjFmbpdnIsICa05yYh5CbvRWaoFWbA52bw5SYzlmchdnI6ICbpFWblJCLi42bw5SYzlmchdnI6ISZtFmbfVWdxlmb1JCLigGduMWYuw2bklGah1GQu9GcuE2cpJXY3JiOi4Gc1JCLiczMzYzMtMDN3MzNzEzM1ITLwYDN5QTO2QjMz0iMxQDMzEDO2MjMtEjMtUTLx0yUiojIkl2ciwiI9MHeycXa3hlMLJzLzEmVDRDN1MGeMhkZx5EaHlXZ6l2L2IWQ3kjUYt0a1AlI6IiY1NnIsICeNdkWp5kMONTVUlFaWpXT0llMZh3aUxkMJJTWwAzUatmUqpFdZRVWzUkaNpGZUl1dZRUT1UlMOJTUU1kaKpWW0V0Rah3aUxENV1mTwAzUZVTTtpFdBRVTyUEVNpmRX9kLxUTOzkzM5cjM3gDOzcTN4MjNiojIlNmbv5mIsQDN5ETO3EjM3EjOiUWbpR3XoRXdhJCL0QTN1kzNxIzNxojIwhXZiwCN0kTM5cTMycTM6IiZi5mIsQDN5ETO3EjM3EjOiQXYpJCLiMnZkF2LoRnLjFmLs9GZphWYt5Cckl2LvozcwRHdoJiOiM3cpJCLiYWOwUjM3UWN0UzYz0COwkTYtczY0QTLwMmM20iMwYjYhJzM4IiOiQWdhJye.9JSW6Z3az8mVnJHaSh2Tjt0QiVTLpRmaDhUbXNjI6ICZptmIsISW6Z3az8mVnJHaSh2Tjt0QiVTLpRmaDhUbXNjI6ICd1gnIsIiN1IzUSJiOicGbhJCLiQ1VKJiOiAXe0Jye";
            char[] tokenArray = (token.Substring(28)).ToCharArray();
            Array.Reverse(tokenArray);
            token = new string(tokenArray);

            string ppid = string.Empty;
            string winaccountName = string.Empty;
            string email = string.Empty;

            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            foreach (Claim c in tokenS.Claims)
            {
                if (c.Type.Equals("ppid"))
                    ppid = c.Value;

                if (c.Type.Equals("winaccountname"))
                    winaccountName = c.Value;

                if (c.Type.Equals("email"))
                    email = c.Value;
            }

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
                lastPaymentDate = "240724";

                DataSet ds2 = ProjectCategory.Get(projectCategory);
                DataTable dt2 = ds2.Tables[0];

                if (dt2.Rows.Count > 0) {
                    DataRow dr2 = dt2.Rows[0];
                    string systemRef = dr2["systemRef"].ToString();

                    DataSet ds3 = Util.ExecuteCommandStoredProcedure(Util.infinityConnectionString, "sp_rscGetFinPayBankQRCode",
                        new SqlParameter("@systemRef", systemRef));

                    DataTable dt3 = ds3.Tables[0];

                    if (dt3.Rows.Count > 0)
                    {
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
                                    
                                    DataSet ds4 = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscSetTransInvoiceRef",
                                        new SqlParameter("@jsonData", jsonData));

                                    DataTable dt4 = ds4.Tables[0];

                                    if (dt4.Rows.Count > 0)
                                    {
                                        DataRow dr4 = dt4.Rows[0];

                                        merchantName = dr4["merchantName"].ToString();
                                        paidAmount = dr4["paidAmount"].ToString();
                                        paidStatus = dr4["paidStatus"].ToString();
                                        errorCode = int.Parse(dr4["errorCode"].ToString());
                                        actionDate = dr4["actionDate"].ToString();
                                    }
                                    else
                                    {
                                        errorCode = 1;
                                        qrCodeObj = null;
                                    }

                                    Response.Write("<img src=\"data:image/png;base64," + qrCodeObj.qr_image64 + " alt=\"\" />");
                                }
                                else
                                {
                                    errorCode = 3;
                                    qrCodeObj = null;
                                }
                            }
                            else
                            {
                                errorCode = 3;
                                qrCodeObj = null;
                            }
                        }
                        else
                        {
                            errorCode = 3;
                            qrCodeObj = null;
                        }
                    }
                    else {
                        errorCode = 3;
                        qrCodeObj = null;
                    }
                }
            }
            */
        }
    }
}