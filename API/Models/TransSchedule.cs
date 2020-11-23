/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๑/๑๑/๒๕๖๓>
Modify date : <๑๑/๑๑/๒๕๖๓>
Description : <>
=============================================
*/

using System.Data;
using System.Data.SqlClient;

namespace API.Models
{
  public class TransSchedule
  {
		public static DataSet Get(string projectCategory, string transProjectID)
		{
			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetTransSchedule",
					new SqlParameter("@projectCategory",	projectCategory),
					new SqlParameter("@transProjectID",		transProjectID));

			return ds;
		}
	}
}