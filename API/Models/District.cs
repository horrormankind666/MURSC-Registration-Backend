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

		public static DataTable Get(
			string country,
			string province,
			string district
		)
		{
			DataTable dt = GetList("", country, province, "", "", "").Tables[0];
			DataRow[] dr = dt.Select("(plcCountryId = '" + country + "') and (plcProvinceId = '" + province + "') and (id = '" + district + "')");

			dt = (dr.Length > 0 ? dr.CopyToDataTable() : dt.Clone());

			return dt;
		}
	}
}