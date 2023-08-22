/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๗/๐๒/๒๕๖๓>
Modify date : <๓๑/๐๕/๒๕๖๕>
Description : <>
=============================================
*/

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace API {
	public class Util {
		public class APIResponse {
			public bool status {
				get;
				set;
			}

			public object data {
				get;
				set;
			}

			public string message {
				get;
				set;
			}

			public APIResponse(
				bool status = true,
				string message = null
			) {
				this.status = status;
				this.message = (!status ? message : null);
			}

			public static APIResponse GetData(
				dynamic ob,
				bool isAuthen = true,
				string message = null
			) {
				APIResponse obj = null;

				try {
					obj = new APIResponse {
						data = ob
					};

					if (!isAuthen)
						obj = new APIResponse(false, (String.IsNullOrEmpty(message) ? "permissionNotFound" : message));
				}
				catch (Exception ex) {
					obj = new APIResponse(false, ex.Message);
				}

				return obj;
			}
		}

		public static string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
		public static string infinityConnectionString = ConfigurationManager.ConnectionStrings["infinityConnectionString"].ConnectionString;
		public static string cookieName = "MURSC.Cookies";

		public static SqlConnection ConnectDB(string connString) {
			SqlConnection conn = new SqlConnection(connString);

			return conn;
		}

		public static DataSet ExecuteCommandStoredProcedure(
			string connString,
			string spName,
			params SqlParameter[] values
		) {
			SqlConnection conn = ConnectDB(connString);
			SqlCommand cmd = new SqlCommand(spName, conn);
			DataSet ds = new DataSet();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 1000;

			if (values != null && values.Length > 0)
				cmd.Parameters.AddRange(values);

			try {
				conn.Open();

				SqlDataAdapter da = new SqlDataAdapter(cmd);

				ds = new DataSet();
				da.Fill(ds);
			}
			finally {
				cmd.Dispose();

				conn.Close();
				conn.Dispose();
			}

			return ds;
		}

		public static HttpCookie GetCookie(string cookieName) {
			HttpCookie cookieObj = new HttpCookie(cookieName);
			cookieObj = HttpContext.Current.Request.Cookies[cookieName];

			return cookieObj;
		}

		public static string GetIP() {
			string _ip = String.Empty;

			if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
				_ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
			else
				if (!String.IsNullOrWhiteSpace(HttpContext.Current.Request.UserHostAddress))
					_ip = HttpContext.Current.Request.UserHostAddress;

			if (_ip == "::1")
				_ip = "127.0.0.1";

			return _ip;
		}

		public static bool CookieExist(string cookieName) {
			HttpCookie cookieObj = GetCookie(cookieName);

			return (cookieObj == null ? false : true);
		}

		private static object GetUserInfoByAuthenADFS() {
			object userInfoResult = null;

			try {
				string authorization = HttpContext.Current.Request.Headers["Authorization"];
				string token = String.Empty;

				if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) {
					token = authorization.Substring("Bearer ".Length).Trim();
				}

				var httpWebRequest = ((HttpWebRequest)WebRequest.Create("https://mursc.mahidol.ac.th/ResourceADFS/API/AuthenResource/UserInfo"));

				httpWebRequest.Headers.Add("Authorization", "Bearer " + token);

				var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

				using (var sr = new StreamReader(httpWebResponse.GetResponseStream())) {
					var result = sr.ReadToEnd();

					dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(result);

					userInfoResult = jsonObject["data"];
				};
			}
			catch {
			}

			return userInfoResult;
		}

		public static bool GetIsAuthenticatedByAuthenADFS() {
			bool isAuthenticated = false;

			try {
				string authorization = HttpContext.Current.Request.Headers["Authorization"];
				string token = String.Empty;

				if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) {
					token = authorization.Substring("Bearer ".Length).Trim();
				}

				var httpWebRequest = ((HttpWebRequest)WebRequest.Create("https://mursc.mahidol.ac.th/ResourceADFS/API/AuthenResource/IsAuthenticated"));

				httpWebRequest.Headers.Add("Authorization", "Bearer " + token);

				var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

				using (var sr = new StreamReader(httpWebResponse.GetResponseStream())) {
					var result = sr.ReadToEnd();

					isAuthenticated = (result.Equals("true") ? true : false);
				};
			}
			catch {
			}

			return isAuthenticated;
		}

		public static dynamic GetPPIDByAuthenADFS() {
			object result = null;
			string ppid = String.Empty;
			string winaccountName = String.Empty;
			string email = String.Empty;

			try {
				string authorization = HttpContext.Current.Request.Headers["Authorization"];
				string token = String.Empty;

				if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) {
					token = authorization.Substring("Bearer ".Length).Trim();
				}

				char[] tokenArray = (token.Substring(28)).ToCharArray();
				Array.Reverse(tokenArray);
				token = new string (tokenArray);

				var handler = new JwtSecurityTokenHandler();
				var tokenS = handler.ReadToken(token) as JwtSecurityToken;

				foreach (Claim c in tokenS.Claims) {
					if (c.Type.Equals("ppid"))
						ppid = c.Value;

					if (c.Type.Equals("winaccountname"))
						winaccountName = c.Value;

					if (c.Type.Equals("email"))
						email = c.Value;
				}
			}
			catch {
			}

			result = new {
				ppid = ppid,
				winaccountName = winaccountName,
				email = email
			};

			return result;
		}

		public static dynamic CUID2Array(string cuid) {
			string[] result = null;

			if (!string.IsNullOrEmpty(cuid)) {
				try {
					byte[] decDataByte = Convert.FromBase64String(cuid);
					cuid = Encoding.UTF8.GetString(decDataByte);
					
					string[] cuidArray = null;
					string uid = String.Empty;
					string uidChk = String.Empty;
					string data = String.Empty;

					if (!string.IsNullOrEmpty(cuid))
						cuidArray = cuid.Split('.');

					if (cuidArray.Length.Equals(3)) {
						uid = cuidArray[0];
						uidChk = cuidArray[1];
						data = cuidArray[2];

						uid = StringReverseConvertBase64(uid);
						uidChk = StringReverse(uidChk);
						data = StringReverseConvertBase64(data);

						/*
						if (uid.Equals(uidChk))
						*/
						result = data.Split('.');
					}
				}
				catch {
				}
			}

			return result;
		}

		public static string StringReverse(string str) {
			string result = String.Empty;

			try {
				char[] charArray = str.ToCharArray();
				Array.Reverse(charArray);
				result = new string(charArray);
			}
			catch {
			}

			return result;
		}

		public static string StringReverseConvertBase64(string str) {
			string result = String.Empty;

			try {
				char[] charArray = str.ToCharArray();
				Array.Reverse(charArray);
				str = new string(charArray);

				result = Encoding.UTF8.GetString(Convert.FromBase64String(str));
			}
			catch {
			}

			return result;
		}

		public static string Base64Encode(string plainText) {
			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

			return Convert.ToBase64String(plainTextBytes);
		}

        public static string DoGeneratePasscode(
            int _lenChars,
            int _lenNonAlphaNumericChars
        ) {
            string _allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string _allowedNonAlphaNum = "!@#$%^&()_-+=[{]};";

            Random _rd = new Random();

            if (_lenNonAlphaNumericChars > _lenChars || _lenChars <= 0 || _lenNonAlphaNumericChars < 0)
                throw new ArgumentOutOfRangeException();

            char[] _pass = new char[_lenChars];
            int[] _pos = new int[_lenChars];
            int _i = 0, _j = 0, _temp = 0;
            bool _flag = false;

            while (_i < (_lenChars - 1)) {
                _j = 0;
                _flag = false;
                _temp = _rd.Next(0, _lenChars);

                for (_j = 0; _j < _lenChars; _j++) {
                    if (_temp == _pos[_j]) {
                        _flag = true;
                        _j = _lenChars;
                    }
                }

                if (!_flag) {
                    _pos[_i] = _temp;
                    _i++;
                }
            }

            for (_i = 0; _i < (_lenChars - _lenNonAlphaNumericChars); _i++)
                _pass[_i] = _allowedChars[_rd.Next(0, _allowedChars.Length)];

            for (_i = (_lenChars - _lenNonAlphaNumericChars); _i < _lenChars; _i++)
                _pass[_i] = _allowedNonAlphaNum[_rd.Next(0, _allowedNonAlphaNum.Length)];

            char[] _sorted = new char[_lenChars];

            for (_i = 0; _i < _lenChars; _i++)
                _sorted[_i] = _pass[_pos[_i]];

            string _resultPass = new String(_sorted);

            return _resultPass;
        }

		public static string DoGetCUID(string[] data) {
			string randAlphaNumStr = DoGeneratePasscode(20, 0);

            return (
				Base64Encode(
					StringReverse(Base64Encode(randAlphaNumStr)) + "." + 
					StringReverse(randAlphaNumStr) + "." +
					StringReverse(Base64Encode(String.Join(".", data)))
				)
			);            
        }
    }
}