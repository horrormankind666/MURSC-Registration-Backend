/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๗/๐๒/๒๕๖๓>
Modify date : <๑๔/๐๕/๒๕๖๓>
Description : <>
=============================================
*/

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace API
{
    public class Util
    {
        public class APIResponse
        {
            public bool status { get; set; }

            public object data { get; set; }

            public string message { get; set; }

            public APIResponse(bool status = true, string message = null)
            {
                this.status = status;
                this.message = (!status ? message : null);
            }

            public static APIResponse GetData(dynamic ob, bool isAuthen = true, string message = null)
            {
                APIResponse obj = null;

                try
                {
                    obj = new APIResponse
                    {
                        data = ob
                    };

                    if (!isAuthen)
                        obj = new APIResponse(false, (String.IsNullOrEmpty(message) ? "permissionNotFound" : message));
                }
                catch (Exception ex)
                {
                    obj = new APIResponse(false, ex.Message);
                }

                return obj;
            }
        }

        public static string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public static string infinityConnectionString = ConfigurationManager.ConnectionStrings["infinityConnectionString"].ConnectionString;

        public static SqlConnection ConnectDB(string connString)
        {
            SqlConnection conn = new SqlConnection(connString);

            return conn;
        }

        public static DataSet ExecuteCommandStoredProcedure(string connString, string spName, params SqlParameter[] values)
        {
            SqlConnection conn = ConnectDB(connString);
            SqlCommand cmd = new SqlCommand(spName, conn);
            DataSet ds = new DataSet();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 1000;

            if (values != null && values.Length > 0)
                cmd.Parameters.AddRange(values);

            try
            {
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                ds = new DataSet();
                da.Fill(ds);
            }
            finally
            {
                cmd.Dispose();

                conn.Close();
                conn.Dispose();
            }

            return ds;
        }

        public static HttpCookie GetCookie(string cookieName)
        {
            HttpCookie cookieObj = new HttpCookie(cookieName);
            cookieObj = HttpContext.Current.Request.Cookies[cookieName];

            return cookieObj;
        }

        public static bool CookieExist(string cookieName)
        {
            HttpCookie cookieObj = GetCookie(cookieName);

            return (cookieObj == null ? false : true);
        }
    }
}