/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๔/๐๕/๒๕๖๓>
Modify date : <๓๑/๐๕/๒๕๖๕>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using System.Data;

namespace API.Models {
	public class Province {
        public static List<object> GetDataSource(DataTable dt) {
            List<object> list = new List<object>();

            foreach (DataRow dr in dt.Rows) {
                list.Add(new {
                    ID = (!String.IsNullOrEmpty(dr["id"].ToString()) ? dr["id"] : String.Empty),
                    countryID = (!String.IsNullOrEmpty(dr["plcCountryId"].ToString()) ? dr["plcCountryId"] : String.Empty),
                    isoCountryCodes3Letter = (!String.IsNullOrEmpty(dr["isoCountryCodes3Letter"].ToString()) ? dr["isoCountryCodes3Letter"] : String.Empty),
                    name = new {
                        th = (!String.IsNullOrEmpty(dr["provinceNameTH"].ToString()) ? dr["provinceNameTH"] : dr["provinceNameEN"]),
                        en = (!String.IsNullOrEmpty(dr["provinceNameEN"].ToString()) ? dr["provinceNameEN"] : dr["provinceNameTH"])
                    },
                    regional = (!String.IsNullOrEmpty(dr["regionalName"].ToString()) ? dr["regionalName"] : String.Empty)
                });
            }

            return list;
        }
        public static DataSet GetList(
			string keyword,
			string country,
			string cancelledStatus,
			string sortOrderBy,
			string sortExpression
		) {
			UtilService.iUtil iUtilService = new UtilService.iUtil();
			DataSet ds = iUtilService.GetListProvince(Util.infinityConnectionString, keyword, country, cancelledStatus, sortOrderBy, sortExpression);

			return ds;
		}

		public static DataTable Get(
			string country,
			string province
		) {
			DataTable dt = GetList("", country, "", "", "").Tables[0];
			DataRow[] dr = dt.Select("(plcCountryId = '" + country + "') and (id = '" + province + "')");

			dt = (dr.Length > 0 ? dr.CopyToDataTable() : dt.Clone());

			return dt;
		}
	}
}