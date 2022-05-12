/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๗/๐๕/๒๕๖๓>
Modify date : <๑๑/๐๘/๒๕๖๓>
Description : <>
=============================================
*/

using System.Data;
using System.Data.SqlClient;

namespace API.Models {
	public class TransRegistered {
		public static DataSet GetList(
			string personID,
			string paymentStatus
		) {
			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetListTransRegistered",
				new SqlParameter("@personID", personID),
				new SqlParameter("@paymentStatus", paymentStatus));

			return ds;
		}

		public static DataSet Get(
			string transRegisteredID,
			string personID,
			string transProjectID
		) {
			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetTransRegistered",
				new SqlParameter("@transRegisteredID", transRegisteredID),
				new SqlParameter("@personID", personID),
				new SqlParameter("@transProjectID", transProjectID));

			return ds;
		}

		public static DataSet Set(
			string method,
			string jsonData
		) {
			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscSetTransRegistered",
				new SqlParameter("@method", method),
				new SqlParameter("@jsonData", jsonData));

			return ds;
		}
	}
}