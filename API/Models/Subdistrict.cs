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
    public class Subdistrict
    {
        //public DataSet GetListSubdistrict(string connString, string keyword, string country, string province, string district, string cancelledStatus, string sortOrderBy, string sortExpression)
        public static DataSet GetList(
            string keyword,
            string country,
            string province,
            string district,
            string cancelledStatus,
            string sortOrderBy,
            string sortExpression
        )
        {
            UtilService.iUtil iUtilService = new UtilService.iUtil();
            DataSet ds = iUtilService.GetListSubdistrict(Util.infinityConnectionString, keyword, country, province, district, cancelledStatus, sortOrderBy, sortExpression);

            return ds;
        }

        public static DataTable Get(
            string country,
            string province,
            string district,
            string subdistrict
        )
        {
            DataTable dt = GetList("", country, province, district, "", "", "").Tables[0];
            DataRow[] dr = dt.Select("(plcCountryId = '" + country + "') and (plcProvinceId = '" + province + "') and (plcDistrictId = '" + district + "') and (id = '" + subdistrict + "')");

            dt = (dr.Length > 0 ? dr.CopyToDataTable() : dt.Clone());

            return dt;
        }
    }
}