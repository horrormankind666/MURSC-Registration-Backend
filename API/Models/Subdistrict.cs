/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๔/๐๕/๒๕๖๓>
Modify date : <๐๑/๐๖/๒๕๖๕>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using System.Data;

namespace API.Models {
	public class Subdistrict {
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
                    districtID = (!String.IsNullOrEmpty(dr["plcDistrictId"].ToString()) ? dr["plcDistrictId"] : String.Empty),
                    districtName = new {
                        th = (!String.IsNullOrEmpty(dr["districtNameTH"].ToString()) ? dr["districtNameTH"] : dr["districtNameEN"]),
                        en = (!String.IsNullOrEmpty(dr["districtNameEN"].ToString()) ? dr["districtNameEN"] : dr["districtNameTH"])
                    },
                    zipCode = (!String.IsNullOrEmpty(dr["zipCode"].ToString()) ? dr["zipCode"] : String.Empty),
                    name = new {
                        th = (!String.IsNullOrEmpty(dr["subdistrictNameTH"].ToString()) ? dr["subdistrictNameTH"] : dr["subdistrictNameEN"]),
                        en = (!String.IsNullOrEmpty(dr["subdistrictNameEN"].ToString()) ? dr["subdistrictNameEN"] : dr["subdistrictNameTH"])
                    }
                });
            }

            return list;
        }

        public static DataSet GetList(
			string keyword,
			string country,
			string province,
			string district,
			string cancelledStatus,
			string sortOrderBy,
			string sortExpression
		) {
			UtilService.iUtil iUtilService = new UtilService.iUtil();
			DataSet ds = iUtilService.GetListSubdistrict(Util.infinityConnectionString, keyword, country, province, district, cancelledStatus, sortOrderBy, sortExpression);

			return ds;
		}

		public static DataTable Get(
			string country,
			string province,
			string district,
			string subdistrict
		) {
			DataTable dt = GetList("", country, province, district, "", "", "").Tables[0];
			DataRow[] dr = dt.Select("(plcCountryId = '" + country + "') and (plcProvinceId = '" + province + "') and (plcDistrictId = '" + district + "') and (id = '" + subdistrict + "')");

			dt = (dr.Length > 0 ? dr.CopyToDataTable() : dt.Clone());

			return dt;
		}
	}
}