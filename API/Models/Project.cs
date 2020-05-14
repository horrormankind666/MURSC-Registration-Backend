/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๗/๐๒/๒๕๖๓>
Modify date : <๑๒/๐๕/๒๕๖๓>
Description : <>
=============================================
*/

using System.Data;
using System.Data.SqlClient;

namespace API.Models
{
    public class TransProject
    {
        public static DataSet GetList()
        {
            DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetListTransProject", null);

            return ds;
        }

        public static DataSet Get(string transProjectID)
        {
            DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetTransProject",
                new SqlParameter("@transProjectID", transProjectID));

            return ds;
        }
    }
}