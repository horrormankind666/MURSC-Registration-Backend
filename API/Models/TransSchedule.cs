/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๑/๑๑/๒๕๖๓>
Modify date : <๐๑/๐๖/๒๕๖๕>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Newtonsoft.Json;

namespace API.Models {
	public class TransSchedule {
        public static List<object> GetDataSource(DataTable dt) {
            List<object> list = new List<object>();

            foreach (DataRow dr in dt.Rows) {
                List<object> schedules = new List<object>();

                if (!String.IsNullOrEmpty(dr["schedules"].ToString())) {
                    string startDateTime = String.Empty;
                    string endDateTime = String.Empty;
                    
                    foreach (var data in JsonConvert.DeserializeObject<dynamic>(dr["schedules"].ToString())) {
                        startDateTime = (!String.IsNullOrEmpty(data["startTime"].ToString()) ? data["startTime"].ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("en-EN")) : String.Empty);
                        endDateTime = (!String.IsNullOrEmpty(data["endTime"].ToString()) ? data["endTime"].ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("en-EN")) : String.Empty);
                        
                        schedules.Add(new {
                            ID = (!String.IsNullOrEmpty(data["id"].ToString()) ? data["id"] : String.Empty),
                            lessonName = new {
                                th = (!String.IsNullOrEmpty(data["lessonNameTH"].ToString()) ? data["lessonNameTH"] : data["lessonNameEN"]),
                                en = (!String.IsNullOrEmpty(data["lessonNameEN"].ToString()) ? data["lessonNameEN"] : data["lessonNameTH"])
                            },
                            dateTime = new {
                                start = new {
                                    date = startDateTime.Substring(0, 10),
                                    time = startDateTime.Substring(11)
                                },
                                end = new {
                                    date = endDateTime.Substring(0, 10),
                                    time = endDateTime.Substring(11)
                                }
                            },
                            instructor = (!String.IsNullOrEmpty(data["instructor"].ToString()) ? data["instructor"] : String.Empty),
                            typeSchedule = (!String.IsNullOrEmpty(data["typeSchedule"].ToString()) ? data["typeSchedule"] : String.Empty)
                        });
                    }
                }

                list.Add(new {
                    ID = (!String.IsNullOrEmpty(dr["ID"].ToString()) ? dr["ID"] : String.Empty),
                    transProject = new {
                        ID = (!String.IsNullOrEmpty(dr["transProjectID"].ToString()) ? dr["transProjectID"] : String.Empty),
                        project = new {
                            ID = (!String.IsNullOrEmpty(dr["projectID"].ToString()) ? dr["projectID"] : String.Empty),
                            category = new {
                                ID = (!String.IsNullOrEmpty(dr["projectCategoryID"].ToString()) ? dr["projectCategoryID"] : String.Empty),
                                name = new {
                                    th = (!String.IsNullOrEmpty(dr["projectCategoryNameTH"].ToString()) ? dr["projectCategoryNameTH"] : dr["projectCategoryNameEN"]),
                                    en = (!String.IsNullOrEmpty(dr["projectCategoryNameEN"].ToString()) ? dr["projectCategoryNameEN"] : dr["projectCategoryNameTH"])
                                },
                                initial = (!String.IsNullOrEmpty(dr["projectCategoryInitial"].ToString()) ? dr["projectCategoryInitial"] : String.Empty)
                            },
                            logo = (!String.IsNullOrEmpty(dr["logo"].ToString()) ? dr["logo"] : String.Empty),
                            name = new {
                                th = (!String.IsNullOrEmpty(dr["projectNameTH"].ToString()) ? dr["projectNameTH"] : dr["projectNameEN"]),
                                en = (!String.IsNullOrEmpty(dr["projectNameEN"].ToString()) ? dr["projectNameEN"] : dr["projectNameTH"])
                            },
                            about = new {
                                th = (!String.IsNullOrEmpty(dr["aboutTH"].ToString()) ? dr["aboutTH"] : dr["aboutEN"]),
                                en = (!String.IsNullOrEmpty(dr["aboutEN"].ToString()) ? dr["aboutEN"] : dr["aboutTH"])
                            }
                        },
                        description = new {
                            th = (!String.IsNullOrEmpty(dr["descriptionTH"].ToString()) ? dr["descriptionTH"] : dr["descriptionEN"]),
                            en = (!String.IsNullOrEmpty(dr["descriptionEN"].ToString()) ? dr["descriptionEN"] : dr["descriptionTH"])
                        }
                    },
                    section = (!String.IsNullOrEmpty(dr["section"].ToString()) ? dr["section"] : String.Empty),
                    schedules = schedules
                });
            }

            return list;
        }

		public static DataSet Get(
			string projectCategory,
			string transProjectID
		) {
			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetTransSchedule",
				new SqlParameter("@projectCategory", projectCategory),
				new SqlParameter("@transProjectID", transProjectID));

			return ds;
		}
	}
}