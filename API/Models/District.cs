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
	public class District {
        public static List<object> GetDataSource(DataTable dt) {
            List<object> list = new List<object>();

            foreach (DataRow dr in dt.Rows) {
                list.Add(new {
                    ID = (!String.IsNullOrEmpty(dr["id"].ToString()) ? dr["id"] : String.Empty),
                    countryID = (!String.IsNullOrEmpty(dr["plcCountryId"].ToString()) ? dr["plcCountryId"] : String.Empty),
                    isoCountryCodes3Letter = (!String.IsNullOrEmpty(dr["isoCountryCodes3Letter"].ToString()) ? dr["isoCountryCodes3Letter"] : String.Empty),
                    provinceID = (!String.IsNullOrEmpty(dr["plcProvinceId"].ToString()) ? dr["plcProvinceId"] : String.Empty),
                    provinceName = new {
                        th = (!String.IsNullOrEmpty(dr["provinceNameTH"].ToString()) ? dr["provinceNameTH"] : dr["provinceNameEN"]),
                        en = (!String.IsNullOrEmpty(dr["provinceNameEN"].ToString()) ? dr["provinceNameEN"] : dr["provinceNameTH"])
                    },
                    name = new {
                        th = (!String.IsNullOrEmpty(dr["districtNameTH"].ToString()) ? dr["districtNameTH"] : dr["districtNameEN"]),
                        en = (!String.IsNullOrEmpty(dr["districtNameEN"].ToString()) ? dr["districtNameEN"] : dr["districtNameTH"])
                    },
                    zipCode = (!String.IsNullOrEmpty(dr["zipCode"].ToString()) ? dr["zipCode"] : String.Empty)
                });
            }

            return list;
        }

        public static DataSet GetList(
			string keyword,
			string country,
			string province,
			string cancelledStatus,
			string sortOrderBy,
			string sortExpression
		) {
			UtilService.iUtil iUtilService = new UtilService.iUtil();
			DataSet ds = iUtilService.GetListDistrict(Util.infinityConnectionString, keyword, country, province, cancelledStatus, sortOrderBy, sortExpression);

			return ds;
		}

		public static DataTable Get(
			string country,
			string province,
			string district
		) {
			DataTable dt = GetList("", country, province, "", "", "").Tables[0];
			DataRow[] dr = dt.Select("(plcCountryId = '" + country + "') and (plcProvinceId = '" + province + "') and (id = '" + district + "')");

			dt = (dr.Length > 0 ? dr.CopyToDataTable() : dt.Clone());

			return dt;
		}
	}
}