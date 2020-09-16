/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๘/๑๒/๒๕๖๒>
Modify date : <๑๑/๐๙/๒๕๖๓>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ResourceServer.Controllers
{
	[Route("API/AuthenResource")]
	[ApiController]
	public class ResourceController : ControllerBase
	{
		[Route("IsAuthenticated")]
		[HttpGet]
		public ActionResult<dynamic> IsAuthenticated()
		{
			string authorization = Request.Headers["Authorization"];
			bool isAuthenticated = false;
			List<object> userInfoList = new List<object>();

			if (String.IsNullOrEmpty(authorization))
				isAuthenticated = false;
			else
				isAuthenticated = User.Identity.IsAuthenticated;

			return isAuthenticated;
		}

		[Route("UserInfo")]
		[HttpGet]
		public ActionResult<dynamic> UserInfo()
		{
			string authorization = Request.Headers["Authorization"];
			string token = String.Empty;
			string winaccountname = String.Empty;
			string email = String.Empty;
			string ppid = String.Empty;
			StringBuilder jwtHeader = new StringBuilder();
			StringBuilder jwtPayload = new StringBuilder();
			List<object> userInfoList = new List<object>();
            
			if (String.IsNullOrEmpty(authorization))
			{
				userInfoList = null;
			}
			else
			{
				userInfoList.Add(new
				{
					isAuthenticated = User.Identity.IsAuthenticated
				});

				if (User.Identity.IsAuthenticated)
				{ 
					//try
					//{
						if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
						{
							token = authorization.Substring("Bearer ".Length).Trim();
						}

						//byte[] decDataByte = Convert.FromBase64String(token);
						//token = Encoding.UTF8.GetString(decDataByte);

						char[] tokenArray = (token.Substring(28)).ToCharArray();
						Array.Reverse(tokenArray);
						token = new string(tokenArray);
                
						var handler = new JwtSecurityTokenHandler();
						var tokenS = handler.ReadToken(token) as JwtSecurityToken;                        

						jwtHeader.Append("{");
						foreach (var h in tokenS.Header)
						{
							jwtHeader.AppendFormat("'{0}': '{1}',", h.Key, h.Value);
						}
						jwtHeader.Append("}");

						jwtPayload.Append("{");
						foreach (Claim c in tokenS.Claims)
						{
							jwtPayload.AppendFormat("'{0}': '{1}',", c.Type, EncodeURI(c.Value.ToString()));

							if (c.Type.Equals("winaccountname"))
								winaccountname = c.Value;

							if (c.Type.Equals("email"))
								email = c.Value;

							if (c.Type.Equals("ppid"))
								ppid = c.Value;
						}
						jwtPayload.Append("}");

						userInfoList.Add(new {
							//header = JsonConvert.DeserializeObject<dynamic>(jwtHeader.ToString()),
							payload = JsonConvert.DeserializeObject<dynamic>(jwtPayload.ToString()),
							personal = GetPersonal(winaccountname, email, ppid)
						});
					//}
					//catch
					//{
					//}
				}
			}
            
			object userInfoResult = new { data = userInfoList };

			return userInfoResult;
		}

		private static string StringReverse(string plainText)
		{
			string result = null;

			try
			{
				char[] charArray = plainText.ToCharArray();
				Array.Reverse(charArray);
				result = new string(charArray);
			}
			catch
			{
			}

			return result;
		}


		private static string Base64Encode(string plainText)
		{
			string result = null;

			try
			{ 
				var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

				result = Convert.ToBase64String(plainTextBytes);
			}
			catch
			{

			}

			return result;
		}

		private static string EncodeURI(string plainText)
		{
			string result = StringReverse(Base64Encode(StringReverse(plainText)));

			return (!String.IsNullOrEmpty(result) ? result : null);
		}

		private string GetTokenAccess()
		{
			//var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://devadfs.mahidol.ac.th/adfs/oauth2/token/");
			//var postData = "grant_type=client_credentials&client_id=e43a62d7-381a-453d-841c-2ec769f9cc8e&client_secret=FT0bKrw90-B2dVYIzgmCuOR0vOFSdj1tJMI4I1Ri";
			var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://idp.mahidol.ac.th/adfs/oauth2/token/");
			var postData = "grant_type=client_credentials&client_id=ea4f5ba7-b59b-4673-84e5-429670b09081&client_secret=kHs3H51qio83jF-Fdm1y-2PTiBSOzD771i2UPaml";           
			var data = Encoding.ASCII.GetBytes(postData);

			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentLength = data.Length;

			using (var stream = httpWebRequest.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
			}

			var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			using (var sr = new StreamReader(httpWebResponse.GetResponseStream()))
			{
				var result = sr.ReadToEnd();

				dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(result);

				return jsonObject["access_token"];
			};
		}

		private object GetPersonal(string winaccountname, string email, string ppid)
		{
			string host = email.Split('@')[1];
			string personType = host.Split('.')[0];
			object personalInfoResult = null;

			if (personType.Equals("student"))
			{
				//ppid = "6301001";
				string type = (winaccountname.Substring(0, 1)).ToUpper();
				
				personalInfoResult = GetStudent((Regex.IsMatch(type, "^[a-zA-Z]*$") ? type : String.Empty), ppid);
			}
			else
				personalInfoResult = GetHRi(ppid);

			return personalInfoResult;
		}

		private dynamic GetStudent(string type, string studentCode)
		{
			object personalInfoResult = null;
			JObject profileObj = new JObject();

			string authorization = Request.Headers["Authorization"];
			string token = String.Empty;

			if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
			{
				token = authorization.Substring("Bearer ".Length).Trim();
			}

			var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://mursc.mahidol.ac.th/API/StudentProfile/Get?studentCode=" + studentCode);

			httpWebRequest.Headers.Add("Authorization", "Bearer " + token);

			var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			
			using (var sr = new StreamReader(httpWebResponse.GetResponseStream()))
			{
				var result = sr.ReadToEnd();

				dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(result);
				JArray data = jsonObject["data"];

				if (data.Count > 0)
				{
					JObject dr = jsonObject["data"][0];

					profileObj.Add("type",					("student" + type));
					profileObj.Add("personalID",		EncodeURI(dr["studentCode"].ToString()));
					profileObj.Add("titleTH",				EncodeURI(dr["titleTH"].ToString()));
					profileObj.Add("titleEN",				EncodeURI(dr["titleEN"].ToString()));
					profileObj.Add("firstNameTH",		EncodeURI(dr["firstNameTH"].ToString()));
					profileObj.Add("middleNameTH",	EncodeURI(dr["middleNameTH"].ToString()));
					profileObj.Add("lastNameTH",		EncodeURI(dr["lastNameTH"].ToString()));
					profileObj.Add("firstNameEN",		EncodeURI(dr["firstNameEN"].ToString()));
					profileObj.Add("middleNameEN",	EncodeURI(dr["middleNameEN"].ToString()));
					profileObj.Add("lastNameEN",		EncodeURI(dr["lastNameEN"].ToString()));
					profileObj.Add("fullNameTH",		EncodeURI((dr["firstNameTH"].ToString() + (!String.IsNullOrEmpty(dr["middleNameTH"].ToString()) ? (" " + dr["middleNameTH"].ToString()) : "") + " " + dr["lastNameTH"].ToString()).ToString()));
					profileObj.Add("fullNameEN",		EncodeURI((dr["firstNameEN"].ToString() + (!String.IsNullOrEmpty(dr["middleNameEN"].ToString()) ? (" " + dr["middleNameEN"].ToString()) : "") + " " + dr["lastNameEN"].ToString()).ToString()));
					profileObj.Add("facultyNameTH", EncodeURI(dr["facultyNameTH"].ToString()));
					profileObj.Add("facultyNameEN", EncodeURI(dr["facultyNameEN"].ToString()));
					profileObj.Add("programNameTH", EncodeURI(dr["programNameTH"].ToString()));
					profileObj.Add("programNameEN", EncodeURI(dr["programNameEN"].ToString()));
					profileObj.Add("address",				EncodeURI(dr["address"].ToString()));
					profileObj.Add("subdistrict",		EncodeURI(dr["subdistrict"].ToString()));
					profileObj.Add("district",			EncodeURI(dr["district"].ToString()));
					profileObj.Add("province",			EncodeURI(dr["province"].ToString()));
					profileObj.Add("country",				EncodeURI(dr["country"].ToString()));
					profileObj.Add("zipCode",				EncodeURI(dr["zipCode"].ToString()));
					profileObj.Add("phoneNumber",		EncodeURI(dr["phoneNumber"].ToString()));

					personalInfoResult = profileObj;
				}
			};
			
			return personalInfoResult;
		}

		private object GetHRi(string personalId)
		{
			object personalInfoResult = null;
			dynamic personalInfo = null;
			JObject profileObj = new JObject();

			try
			{
				var httpWebRequest = ((HttpWebRequest)WebRequest.Create("https://smartedu.mahidol.ac.th/Infinity/AUNQA/API/HRi/GetData"));

				httpWebRequest.Method = "POST";

				using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
				{
					string postData = ("{\"personalId\": \"" + personalId + "\"}");

					streamWriter.Write(postData);
				}

				var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

				using (var sr = new StreamReader(httpWebResponse.GetResponseStream()))
				{
					var result = sr.ReadToEnd();

					dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(result);

					personalInfo = (jsonObject.SelectToken("data.content") != null ? jsonObject.SelectToken("data.content.personal") : null);
				};		
                
				if (personalInfo != null)
				{
					dynamic positionInfo = null;

					if (personalInfo["positions"] != null)
						positionInfo = personalInfo["positions"][0];

					string phoneNumber = String.Empty;

					if (personalInfo["addresses"] != null)
					{
						foreach (dynamic dr in personalInfo["addresses"])
						{
							if (String.IsNullOrEmpty(phoneNumber))
							{
								phoneNumber = ((String.IsNullOrEmpty(phoneNumber) && dr["telephoneNumber"] != null) ? dr["telephoneNumber"] : phoneNumber);
								phoneNumber = ((String.IsNullOrEmpty(phoneNumber) && dr["detail1"] != null) ? dr["detail1"] : phoneNumber);
								phoneNumber = ((String.IsNullOrEmpty(phoneNumber) && dr["detail2"] != null) ? dr["detail2"] : phoneNumber);
								phoneNumber = ((String.IsNullOrEmpty(phoneNumber) && dr["detail3"] != null) ? dr["detail3"] : phoneNumber);
							}
						}
					}

					profileObj.Add("type",          "personnel");
					profileObj.Add("personalID",		EncodeURI(personalInfo["personalId"].ToString()));
					profileObj.Add("titleTH",				EncodeURI(personalInfo["title"].ToString()));
					profileObj.Add("titleEN",				EncodeURI(personalInfo["titleEn"].ToString()));
					profileObj.Add("firstNameTH",		EncodeURI(personalInfo["firstName"].ToString()));
					profileObj.Add("middleNameTH",	EncodeURI(personalInfo["middleName"].ToString()));
					profileObj.Add("lastNameTH",		EncodeURI(personalInfo["lastName"].ToString()));
					profileObj.Add("firstNameEN",		EncodeURI(personalInfo["firstNameEn"].ToString()));
					profileObj.Add("middleNameEN",	EncodeURI(personalInfo["middleNameEn"].ToString()));
					profileObj.Add("lastNameEN",		EncodeURI(personalInfo["lastNameEn"].ToString()));
					profileObj.Add("fullNameTH",		EncodeURI((personalInfo["firstName"] + (!String.IsNullOrEmpty(personalInfo["middleName"].ToString()) ? (" " + personalInfo["middleName"]) : "") + " " + personalInfo["lastName"]).ToString()));
					profileObj.Add("fullNameEN",		EncodeURI((personalInfo["firstNameEn"] + (!String.IsNullOrEmpty(personalInfo["middleNameEn"].ToString()) ? (" " + personalInfo["middleNameEn"]) : "") + " " + personalInfo["lastNameEn"]).ToString()));
					profileObj.Add("facultyNameTH", EncodeURI((positionInfo != null ? positionInfo["organization"]["faculty"]["name"] : null).ToString()));
					profileObj.Add("facultyNameEN", EncodeURI((positionInfo != null ? positionInfo["organization"]["faculty"]["fullname"] : null).ToString()));
					profileObj.Add("programNameTH", EncodeURI((positionInfo != null ? positionInfo["organization"]["name"] : null).ToString()));
					profileObj.Add("programNameEN", EncodeURI((positionInfo != null ? positionInfo["organization"]["fullname"] : null).ToString()));
					profileObj.Add("address",				null);
					profileObj.Add("subdistrict",		null);
					profileObj.Add("district",			null);
					profileObj.Add("province",			null);
					profileObj.Add("country",				null);
					profileObj.Add("zipCode",				null);
					profileObj.Add("phoneNumber",		EncodeURI((!String.IsNullOrEmpty(phoneNumber) ? phoneNumber : null).ToString()));

					personalInfoResult = profileObj;
				}
			}
			catch
			{
			}

			return personalInfoResult;
		}
	}
}