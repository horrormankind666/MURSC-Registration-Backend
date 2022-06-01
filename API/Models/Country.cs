/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๔/๐๕/๒๕๖๓>
Modify date : <๓๑/๐๕/๒๕๖๕>
Description : <>
=============================================
*/

using System;
using System.Data;
using System.Collections.Generic;

namespace API.Models {
    public class Country {
        public static List<object> GetDataSource(DataTable dt) {
            List<object> list = new List<object>();

            foreach (DataRow dr in dt.Rows) {
                list.Add(new {
                    ID = (!String.IsNullOrEmpty(dr["id"].ToString()) ? dr["id"] : String.Empty),
                    name = new {
                        th = (!String.IsNullOrEmpty(dr["countryNameTH"].ToString()) ? dr["countryNameTH"] : dr["countryNameEN"]),
                        en = (!String.IsNullOrEmpty(dr["countryNameEN"].ToString()) ? dr["countryNameEN"] : dr["countryNameTH"])
                    },
                    isoCountryCodes2Letter = (!String.IsNullOrEmpty(dr["isoCountryCodes2Letter"].ToString()) ? dr["isoCountryCodes2Letter"] : String.Empty),
                    isoCountryCodes3Letter = (!String.IsNullOrEmpty(dr["isoCountryCodes3Letter"].ToString()) ? dr["isoCountryCodes3Letter"] : String.Empty)
                });
            }

            return list;
        }

        public static DataSet GetList(
			string keyword,
			string cancelledStatus,
			string sortOrderBy,
			string sortExpression
		) {
			UtilService.iUtil iUtilService = new UtilService.iUtil();
			DataSet ds = iUtilService.GetListCountry(Util.infinityConnectionString, keyword, cancelledStatus, sortOrderBy, sortExpression);

			return ds;
		}

		public static DataTable Get(string country) {
			DataTable dt = GetList("", "", "", "").Tables[0];
			DataRow[] dr = dt.Select("id = '" + country + "'");

			dt = (dr.Length > 0 ? dr.CopyToDataTable() : dt.Clone());

			return dt;
		}
	}
}