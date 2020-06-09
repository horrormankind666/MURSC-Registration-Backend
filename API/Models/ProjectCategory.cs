/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๘/๐๖/๒๕๖๓>
Modify date : <๐๘/๐๖/๒๕๖๓>
Description : <>
=============================================
*/

using System.Data;

namespace API.Models
{
    public class ProjectCategory
    {
        public static DataSet GetList()
        {
            DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetListProjectCategory", null);

            return ds;
        }

    }
}