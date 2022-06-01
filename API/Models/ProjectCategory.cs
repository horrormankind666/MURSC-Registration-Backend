/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๘/๐๖/๒๕๖๓>
Modify date : <๐๑/๐๖/๒๕๖๕>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace API.Models {
	public class ProjectCategory {
        public static List<object> GetDataSource(DataTable dt) {
            List<object> list = new List<object>();

            foreach (DataRow dr in dt.Rows) {
                list.Add(new {
                    ID = (!String.IsNullOrEmpty(dr["ID"].ToString()) ? dr["ID"] : String.Empty),
                    sequence = (!String.IsNullOrEmpty(dr["sequence"].ToString()) ? dr["sequence"] : String.Empty),
                    name = new {
                        th = (!String.IsNullOrEmpty(dr["projectCategoryNameTH"].ToString()) ? dr["projectCategoryNameTH"] : dr["projectCategoryNameEN"]),
                        en = (!String.IsNullOrEmpty(dr["projectCategoryNameEN"].ToString()) ? dr["projectCategoryNameEN"] : dr["projectCategoryNameTH"])
                    },
                    logo = (!String.IsNullOrEmpty(dr["logo"].ToString()) ? dr["logo"] : String.Empty),
                    initial = (!String.IsNullOrEmpty(dr["initial"].ToString()) ? dr["initial"] : String.Empty),
                    projectCount = (!String.IsNullOrEmpty(dr["projectCount"].ToString()) ? dr["projectCount"] : String.Empty),
                });
            }

            return list;
        }

        public static DataSet GetList() {
			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetListProjectCategory", null);

			return ds;
		}
	
		public static DataSet Get(string projectCategory) {
			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetProjectCategory",
				new SqlParameter("@projectCategory", projectCategory));

			return ds;
		}
	}
}