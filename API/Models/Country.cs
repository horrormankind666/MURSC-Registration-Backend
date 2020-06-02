/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๔/๐๕/๒๕๖๓>
Modify date : <๐๑/๐๖/๒๕๖๓>
Description : <>
=============================================
*/

using System.Data;

namespace API.Models
{
    public class Country
    {
        public static DataSet GetList(
            string keyword,
            string cancelledStatus,
            string sortOrderBy,
            string sortExpression
        )
        {
            UtilService.iUtil iUtilService = new UtilService.iUtil();
            DataSet ds = iUtilService.GetListCountry(Util.infinityConnectionString, keyword, cancelledStatus, sortOrderBy, sortExpression);

            return ds;
        }

        public static DataTable Get(string country)
        {
            DataTable dt = GetList("", "", "", "").Tables[0];
            DataRow[] dr = dt.Select("id = '" + country + "'");

            dt = (dr.Length > 0 ? dr.CopyToDataTable() : dt.Clone());

            return dt;
        }
    }
}