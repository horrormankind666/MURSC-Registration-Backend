/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๕/๑๐/๒๕๖๓>
Modify date : <๐๕/๑๐/๒๕๖๓>
Description : <>
=============================================
*/

using System.Data;
using System.Data.SqlClient;

namespace API.Models
{
  public class TransInvoice
  {
    public static DataSet Set(
      string transRegisteredID,
      string fee,
      string createdBy
    )
    {
      DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscSetTransInvoice",
        new SqlParameter("@transRegisteredID", transRegisteredID),
        new SqlParameter("@fee", fee),
        new SqlParameter("@createdBy", createdBy));

      return ds;
    }
  }
}