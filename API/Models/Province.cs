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

		public static DataTable Get(
			string country,
			string province
		)
		{
			DataTable dt = GetList("", country, "", "", "").Tables[0];
			DataRow[] dr = dt.Select("(plcCountryId = '" + country + "') and (id = '" + province + "')");

			dt = (dr.Length > 0 ? dr.CopyToDataTable() : dt.Clone());

			return dt;
		}
	}
}