/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๗/๐๕/๒๕๖๓>
Modify date : <๐๑/๐๖/๒๕๖๓>
Description : <>
=============================================
*/

using System.Data;
using System.Data.SqlClient;

namespace API.Models
{
    public class TransRegisExam
    {
        public static DataSet Get(
            string transRegisExamID,
	        string personID,
            string transProjectID
        )
        {
            DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetTransRegisExam",
                new SqlParameter("@transRegisExamID",   transRegisExamID),
                new SqlParameter("@personID",           personID),
                new SqlParameter("@transProjectID",     transProjectID));

            return ds;
        }

        public static DataSet Set(
            string method,
            string jsonData
        )
        {
            DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscSetTransRegisExam",
                new SqlParameter("@method",     method),
                new SqlParameter("@jsonData",   jsonData));

            return ds;
        }
    }
}