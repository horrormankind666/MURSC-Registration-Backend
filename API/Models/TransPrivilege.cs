/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๙/๐๖/๒๕๖๕>
Modify date : <๐๙/๐๖/๒๕๖๕>
Description : <>
=============================================
*/

using System.Data;
using System.Data.SqlClient;

namespace API.Models {
    public class TransPrivilege {
        public static DataSet Set(
            string method,
            string jsonData
        ) {
            DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscSetTransPrivilege",
                new SqlParameter("@method", method),
                new SqlParameter("@jsonData", jsonData));

            return ds;
        }
    }
}