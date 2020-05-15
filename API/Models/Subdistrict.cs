/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๔/๐๕/๒๕๖๓>
Modify date : <๑๔/๐๕/๒๕๖๓>
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
    }
}