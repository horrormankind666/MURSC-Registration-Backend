/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๗/๐๒/๒๕๖๓>
Modify date : <๒๗/๐๒/๒๕๖๓>
Description : <>
=============================================
*/

using System.Data;
using System.Data.SqlClient;

namespace API.Models
{
    public class Project
    {
        public static DataSet GetList()
        {
            DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetListProject", null);

            return ds;
        }

        public static DataSet Get(string transProjectID)
        {
            DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetProject",
                new SqlParameter("@transProjectID", transProjectID));

            return ds;
        }
    }
}