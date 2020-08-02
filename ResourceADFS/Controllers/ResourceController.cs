/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๘/๑๒/๒๕๖๒>
Modify date : <๐๑/๐๘/๒๕๖๓>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
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
							jwtPayload.AppendFormat("'{0}': '{1}',", c.Type, c.Value);

							if (c.Type.Equals("email"))
								email = c.Value;

							if (c.Type.Equals("ppid"))
								ppid = c.Value;
						}
						jwtPayload.Append("}");

						userInfoList.Add(new {
							header = JsonConvert.DeserializeObject<dynamic>(jwtHeader.ToString()),
							payload = JsonConvert.DeserializeObject<dynamic>(jwtPayload.ToString()),
							personal = GetPersonal(email, ppid)
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

		private object GetPersonal(string email, string ppid)
		{
			string host = email.Split('@')[1];
			string personType = host.Split('.')[0];
			object personalInfoResult = null;

			if (personType.Equals("student"))
			{
				//ppid = "6301001";
				personalInfoResult = GetStudent(ppid);
			}
			else
				personalInfoResult = GetHRi(ppid);

			return personalInfoResult;
		}

		private dynamic GetStudent(string studentCode)
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

					profileObj.Add("type",					"student");
					profileObj.Add("personalID",		dr["studentCode"].ToString());
					profileObj.Add("title",					dr["title"].ToString());
					profileObj.Add("firstNameTH",		dr["firstNameTH"].ToString());
					profileObj.Add("middleNameTH",	dr["middleNameTH"].ToString());
					profileObj.Add("lastNameTH",		dr["lastNameTH"].ToString());
					profileObj.Add("firstNameEN",		dr["firstNameEN"].ToString());
					profileObj.Add("middleNameEN",	dr["middleNameEN"].ToString());
					profileObj.Add("lastNameEN",		dr["lastNameEN"].ToString());
					profileObj.Add("address",				dr["address"].ToString());
					profileObj.Add("subdistrict",		dr["subdistrict"].ToString());
					profileObj.Add("district",			dr["district"].ToString());
					profileObj.Add("province",			dr["province"].ToString());
					profileObj.Add("country",				dr["country"].ToString());
					profileObj.Add("zipCode",				dr["zipCode"].ToString());
					profileObj.Add("phoneNumber",		dr["phoneNumber"].ToString());

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
					profileObj.Add("type",          "personnel");
					profileObj.Add("personalID",    personalInfo["personalId"]);
					profileObj.Add("title",         personalInfo["title"]);
					profileObj.Add("firstNameTH",   personalInfo["firstName"]);
					profileObj.Add("middleNameTH",  personalInfo["middleName"]);
					profileObj.Add("lastNameTH",    personalInfo["lastName"]);
					profileObj.Add("firstNameEN",   personalInfo["firstNameEn"]);
					profileObj.Add("middleNameEN",  personalInfo["middleNameEn"]);
					profileObj.Add("lastNameEN",    personalInfo["lastNameEn"]);
					profileObj.Add("address",				null);
					profileObj.Add("subdistrict",		null);
					profileObj.Add("district",			null);
					profileObj.Add("province",			null);
					profileObj.Add("country",				null);
					profileObj.Add("zipCode",				null);
					profileObj.Add("phoneNumber",		null);

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