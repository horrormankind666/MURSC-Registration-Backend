/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๓๐/๐๖/๒๕๖๓>
Modify date : <๓๐/๐๖/๒๕๖๓>
Description : <>
=============================================
*/

using System.Data;
using System.Data.SqlClient;

namespace API.Models
{
  public class TransDeliveryAddress
  {
    public static DataSet Set(
			string transDeliAddressID,
			string transRegisteredID,
      string address,
	    string createdBy
    )
    {
      DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscSetTransDeliveryAddress",
				new SqlParameter("@transDeliAddressID", transDeliAddressID),
				new SqlParameter("@transRegisteredID", transRegisteredID),
				new SqlParameter("@address", address),
				new SqlParameter("@createdBy", createdBy));

      return ds;
    }
  }
}