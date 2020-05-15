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
    public class District
    {
        public static DataSet GetList(
            string keyword,
            string country,
            string province,
            string cancelledStatus,
            string sortOrderBy,
            string sortExpression
        )
        {
            UtilService.iUtil iUtilService = new UtilService.iUtil();
            DataSet ds = iUtilService.GetListDistrict(Util.infinityConnectionString, keyword, country, province, cancelledStatus, sortOrderBy, sortExpression);

            return ds;
        }
    }
}