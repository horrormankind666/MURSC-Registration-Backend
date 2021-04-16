/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๘/๐๖/๒๕๖๓>
Modify date : <๒๙/๐๗/๒๕๖๓>
Description : <>
=============================================
*/

using System.Data;
using System.Data.SqlClient;

namespace API.Models
{
	public class ProjectCategory
	{
		public static DataSet GetList()
		{
			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetListProjectCategory", null);

			return ds;
		}
	
		public static DataSet Get(string projectCategory)
		{
			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetProjectCategory",
				new SqlParameter("@projectCategory", projectCategory));

			return ds;
		}
	}
}