/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๙/๐๖/๒๕๖๕>
Modify date : <๐๙/๐๖/๒๕๖๕>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace API.Models {
    public class Privilege {
		public static List<object> GetDataSource(
            string table,
            DataTable dt
        ) {
            List<object> list = new List<object>();

            if (table.Equals("Privilege")) {
				foreach (DataRow dr in dt.Rows) {
                    list.Add(new {
                        ID = (!String.IsNullOrEmpty(dr["ID"].ToString()) ? dr["ID"] : String.Empty),
                        promoCode = (!String.IsNullOrEmpty(dr["promoCode"].ToString()) ? dr["promoCode"] : String.Empty),
                        name = new {
                            th = (!String.IsNullOrEmpty(dr["nameTH"].ToString()) ? dr["nameTH"] : dr["nameEN"]),
                            en = (!String.IsNullOrEmpty(dr["nameEN"].ToString()) ? dr["nameEN"] : dr["nameTH"])
                        },
                        nameTH = dr["nameTH"],
                        nameEN = dr["nameEN"],
                        detail = new {
                            th = (!String.IsNullOrEmpty(dr["detailTH"].ToString()) ? dr["detailTH"] : dr["detailEN"]),
                            en = (!String.IsNullOrEmpty(dr["detailEN"].ToString()) ? dr["detailEN"] : dr["detailTH"])
                        },
                        detailTH = dr["detailTH"],
                        detailEN = dr["detailEN"],
                        discount = (!String.IsNullOrEmpty(dr["discount"].ToString()) ? float.Parse(dr["discount"].ToString()) : 0),
                        expired = new {
                            date = (!String.IsNullOrEmpty(dr["expiredDates"].ToString()) ? dr["expiredDates"] : String.Empty),
                            status = (!String.IsNullOrEmpty(dr["expiredStatus"].ToString()) ? dr["expiredStatus"] : String.Empty)
                        },
                        expiredDate = dr["expiredDate"]
                    });
				}
            }

			if (table.Equals("TransPrivilege")) {
				foreach (DataRow dr in dt.Rows) {
					list.Add(new {
                        ID = (!String.IsNullOrEmpty(dr["privilegeID"].ToString()) ? dr["privilegeID"] : String.Empty),
                        promoCode = (!String.IsNullOrEmpty(dr["promoCode"].ToString()) ? dr["promoCode"] : String.Empty),
                        name = new {
                            th = (!String.IsNullOrEmpty(dr["nameTH"].ToString()) ? dr["nameTH"] : dr["nameEN"]),
                            en = (!String.IsNullOrEmpty(dr["nameEN"].ToString()) ? dr["nameEN"] : dr["nameTH"])
                        },
                        detail = new {
                            th = (!String.IsNullOrEmpty(dr["detailTH"].ToString()) ? dr["detailTH"] : dr["detailEN"]),
                            en = (!String.IsNullOrEmpty(dr["detailEN"].ToString()) ? dr["detailEN"] : dr["detailTH"])
                        },
                        discount = (!String.IsNullOrEmpty(dr["discount"].ToString()) ? float.Parse(dr["discount"].ToString()) : 0),
                        expiredDate = (!String.IsNullOrEmpty(dr["expiredDates"].ToString()) ? dr["expiredDates"] : String.Empty),
                        usagedDate = (!String.IsNullOrEmpty(dr["usagedDates"].ToString()) ? dr["usagedDates"] : String.Empty)
                    });
				}
			}

            return list;
        }

        public static DataSet Get(
            string privilegeID,
            string promoCode,
            string personID
        ) {
            DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetPrivilege",
                new SqlParameter("@privilegeID", privilegeID),
                new SqlParameter("@promoCode", promoCode),
                new SqlParameter("@personID", personID));

            return ds;
        }
    }
}