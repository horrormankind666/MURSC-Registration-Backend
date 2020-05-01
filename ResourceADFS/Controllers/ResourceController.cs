/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๘/๑๒/๒๕๖๒>
Modify date : <๐๑/๐๕/๒๕๖๓>
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ResourceServer.Controllers
{
    [Route("API/AuthenResource")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        [Route("UserInfo")]
        [HttpGet]
        public ActionResult<dynamic> UserInfo()
        {
            string authorization = Request.Headers["Authorization"];
            string token = String.Empty;
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
                    try
                    {
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
                        }
                        jwtPayload.Append("}");

                        userInfoList.Add(new {
                            header = JsonConvert.DeserializeObject<dynamic>(jwtHeader.ToString()),
                            payload = JsonConvert.DeserializeObject<dynamic>(jwtPayload.ToString())
                        });
                    }
                    catch
                    {
                    }
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
    }
}