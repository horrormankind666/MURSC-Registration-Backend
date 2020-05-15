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
    public class Province
    {
        public static DataSet GetList(
            string keyword,
            string country,
            string cancelledStatus,
            string sortOrderBy,
            string sortExpression
        )
        {
            UtilService.iUtil iUtilService = new UtilService.iUtil();
            DataSet ds = iUtilService.GetListProvince(Util.infinityConnectionString, keyword, country, cancelledStatus, sortOrderBy, sortExpression);

            return ds;
        }
    }
}