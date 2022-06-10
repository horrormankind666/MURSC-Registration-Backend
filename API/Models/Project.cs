/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๗/๐๒/๒๕๖๓>
Modify date : <๐๒/๐๖/๒๕๖๕>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace API.Models {
	public class TransProject {
        public static List<object> GetDataSource(
            string table,
            DataTable dt
        ) {
            List<object> list = new List<object>();

            if (table.Equals("TransProject")) {
                foreach (DataRow dr in dt.Rows) {
                    list.Add(new {
                        ID = (!String.IsNullOrEmpty(dr["transProjectID"].ToString()) ? dr["transProjectID"] : String.Empty),
                        CUID = (!String.IsNullOrEmpty(dr["transProjectID"].ToString()) ? Util.DoGetCUID(new string[] { dr["transProjectID"].ToString() }) : String.Empty),
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
                            },
                            isExam = (!String.IsNullOrEmpty(dr["isExam"].ToString()) ? dr["isExam"] : String.Empty),
                            examType = (!String.IsNullOrEmpty(dr["examType"].ToString()) ? dr["examType"] : String.Empty),
                            isTeaching = (!String.IsNullOrEmpty(dr["isTeaching"].ToString()) ? dr["isTeaching"] : String.Empty)
                        },
                        description = new {
                            th = (!String.IsNullOrEmpty(dr["descriptionTH"].ToString()) ? dr["descriptionTH"] : dr["descriptionEN"]),
                            en = (!String.IsNullOrEmpty(dr["descriptionEN"].ToString()) ? dr["descriptionEN"] : dr["descriptionTH"])
                        },
                        examDate = new {
                            startDate = (!String.IsNullOrEmpty(dr["examStartDates"].ToString()) ? dr["examStartDates"] : String.Empty),
                            endDate = (!String.IsNullOrEmpty(dr["examEndDates"].ToString()) ? dr["examEndDates"] : String.Empty)
                        },
                        registrationDate = new {
                            startDate = (!String.IsNullOrEmpty(dr["regisStartDates"].ToString()) ? dr["regisStartDates"] : String.Empty),
                            endDate = (!String.IsNullOrEmpty(dr["regisEndDates"].ToString()) ? dr["regisEndDates"] : String.Empty)
                        },
                        lastPaymentDate = (!String.IsNullOrEmpty(dr["lastPaymentDates"].ToString()) ? dr["lastPaymentDates"] : String.Empty),
                        maximumSeat = (!String.IsNullOrEmpty(dr["maximumSeat"].ToString()) ? (int)dr["maximumSeat"] : 0),
                        seatReserved = dr["seatReserved"],
                        minimumFee = (!String.IsNullOrEmpty(dr["minimumFee"].ToString()) ? dr["minimumFee"] : String.Empty),
                        contactPerson = ContactPerson.GetDataSource(dr["contactPerson"].ToString()),
                        registrationStatus = (!String.IsNullOrEmpty(dr["registrationStatus"].ToString()) ? dr["registrationStatus"] : String.Empty),
                        userTypeSpecific = (!String.IsNullOrEmpty(dr["userTypeSpecific"].ToString()) ? JsonConvert.DeserializeObject<dynamic>(dr["userTypeSpecific"].ToString()) : String.Empty),
                        privilege = JsonConvert.DeserializeObject<dynamic>(dr["privilege"].ToString()),
                        sameProject = (!String.IsNullOrEmpty(dr["sameProject"].ToString()) ? dr["sameProject"].ToString() : null)
                    });
                }
            }

            if (table.Equals("TransLocation")) {
                foreach (DataRow dr in dt.Rows) {
                    list.Add(new {
                        ID = (!String.IsNullOrEmpty(dr["transLocationID"].ToString()) ? dr["transLocationID"] : String.Empty),
                        transProjectID = (!String.IsNullOrEmpty(dr["transProjectID"].ToString()) ? dr["transProjectID"] : String.Empty),
                        location = new {
                            ID = (!String.IsNullOrEmpty(dr["locationID"].ToString()) ? dr["locationID"] : String.Empty),
                            name = new {
                                th = (!String.IsNullOrEmpty(dr["locationNameTH"].ToString()) ? dr["locationNameTH"] : dr["locationNameEN"]),
                                en = (!String.IsNullOrEmpty(dr["locationNameEN"].ToString()) ? dr["locationNameEN"] : dr["locationNameTH"])
                            },
                            building = new {
                                ID = (!String.IsNullOrEmpty(dr["buildingID"].ToString()) ? dr["buildingID"] : String.Empty),
                                name = new {
                                    th = (!String.IsNullOrEmpty(dr["buildingNameTH"].ToString()) ? dr["buildingNameTH"] : dr["buildingNameEN"]),
                                    en = (!String.IsNullOrEmpty(dr["buildingNameEN"].ToString()) ? dr["buildingNameEN"] : dr["buildingNameTH"])
                                }
                            },
                        },
                        seatTotal = (!String.IsNullOrEmpty(dr["seatTotal"].ToString()) ? int.Parse(dr["seatTotal"].ToString()) : 0),
                        seatAvailable = (!String.IsNullOrEmpty(dr["seatAvailable"].ToString()) ? int.Parse(dr["seatAvailable"].ToString()) : 0)
                    });
                }
            }

            if (table.Equals("TransFeeType")) {
                foreach (DataRow dr in dt.Rows) {
                    list.Add(new {
                        ID = (!String.IsNullOrEmpty(dr["transFeeTypeID"].ToString()) ? dr["transFeeTypeID"] : String.Empty),
                        transProjectID = (!String.IsNullOrEmpty(dr["transProjectID"].ToString()) ? dr["transProjectID"] : String.Empty),
                        feeType = new {
                            ID = (!String.IsNullOrEmpty(dr["feeTypeID"].ToString()) ? dr["feeTypeID"] : String.Empty),
                            name = new {
                                th = (!String.IsNullOrEmpty(dr["feeTypeNameTH"].ToString()) ? dr["feeTypeNameTH"] : dr["feeTypeNameEN"]),
                                en = (!String.IsNullOrEmpty(dr["feeTypeNameEN"].ToString()) ? dr["feeTypeNameEN"] : dr["feeTypeNameTH"])
                            },
                            amount = (!String.IsNullOrEmpty(dr["amount"].ToString()) ? float.Parse(dr["amount"].ToString()) : 0),
                            toggle = (!String.IsNullOrEmpty(dr["toggle"].ToString()) ? dr["toggle"] : String.Empty),
                        },
                        requiredStatus = (!String.IsNullOrEmpty(dr["requiredStatus"].ToString()) ? dr["requiredStatus"] : String.Empty),
                        isSelected = (!String.IsNullOrEmpty(dr["requiredStatus"].ToString()) ? (dr["requiredStatus"].Equals("Y") ? true : false) : false),
                    });               
                }
            }

            return list;
        }

		public static DataSet GetList(string projectCategory) {
			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetListTransProject",
				new SqlParameter("@projectCategory", projectCategory));

			return ds;
		}

		public static DataSet Get(
			string projectCategory,
			string transProjectID
		) {
			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetTransProject",
                new SqlParameter("@projectCategory", projectCategory),
				new SqlParameter("@transProjectID", transProjectID));

			return ds;
		}
	}
}