/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๗/๐๕/๒๕๖๓>
Modify date : <๐๘/๐๖/๒๕๖๕>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace API.Models {
	public class TransRegistered {
        public static List<object> GetDataSource(
            string table,
            DataTable dt
        ) {
            List<object> list = new List<object>();

            if (table.Equals("TransRegistered")) {
				foreach (DataRow dr in dt.Rows) {
					DataTable dtCountry = Country.Get(dr["countryID"].ToString());
					Object country = (dtCountry.Rows.Count > 0 ? Country.GetDataSource(dtCountry)[0] : new { });

					DataTable dtProvince = Province.Get(dr["countryID"].ToString(), dr["provinceID"].ToString());
                    Object province = (dtProvince.Rows.Count > 0 ? Province.GetDataSource(dtProvince)[0] : new { });

                    DataTable dtDistrict = District.Get(dr["countryID"].ToString(), dr["provinceID"].ToString(), dr["districtID"].ToString());
                    Object district = (dtDistrict.Rows.Count > 0 ? District.GetDataSource(dtDistrict)[0] : new { });

                    DataTable dtSubdistrict = Subdistrict.Get(dr["countryID"].ToString(), dr["provinceID"].ToString(), dr["districtID"].ToString(), dr["subdistrictID"].ToString());
                    Object subdistrict = (dtSubdistrict.Rows.Count > 0 ? Subdistrict.GetDataSource(dtSubdistrict)[0] : new { });

					list.Add(new {
						ID = (!String.IsNullOrEmpty(dr["transRegisteredID"].ToString()) ? dr["transRegisteredID"] : String.Empty),
						CUID = ((!String.IsNullOrEmpty(dr["transRegisteredID"].ToString()) && !String.IsNullOrEmpty(dr["transProjectID"].ToString())) ? Util.DoGetCUID(new string[] { dr["transRegisteredID"].ToString(), dr["transProjectID"].ToString() }) : String.Empty),
						registeredDate = (!String.IsNullOrEmpty(dr["registeredDates"].ToString()) ? dr["registeredDates"] : String.Empty),
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
							lastPaymentDate = (!String.IsNullOrEmpty(dr["lastPaymentDates"].ToString()) ? dr["lastPaymentDates"] : String.Empty),
							paymentExpire = (!String.IsNullOrEmpty(dr["paymentExpire"].ToString()) ? dr["paymentExpire"] : String.Empty),
							announceDate = (!String.IsNullOrEmpty(dr["announceDates"].ToString()) ? dr["announceDates"] : String.Empty),
							contactPerson = ContactPerson.GetDataSource(dr["contactPerson"].ToString()),
							userTypeSpecific = (!String.IsNullOrEmpty(dr["userTypeSpecific"].ToString()) ? JsonConvert.DeserializeObject<dynamic>(dr["userTypeSpecific"].ToString()) : String.Empty),
							privilege = JsonConvert.DeserializeObject<dynamic>(dr["privilege"].ToString()),
                            sameProject = (!String.IsNullOrEmpty(dr["sameProject"].ToString()) ? dr["sameProject"].ToString() : null)
                        },
						transLocation = new {
							ID = (!String.IsNullOrEmpty(dr["transLocationID"].ToString()) ? dr["transLocationID"] : String.Empty),
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
								}
							}
						},
						transDeliAddress = new {
							ID = (!String.IsNullOrEmpty(dr["transDeliAddressID"].ToString()) ? dr["transDeliAddressID"] : String.Empty),
							address = (!String.IsNullOrEmpty(dr["address"].ToString()) ? dr["address"] : String.Empty),
							country = country,
							province = province,
							district = district,
							subdistrict = subdistrict,
							postalCode = (!String.IsNullOrEmpty(dr["postalCode"].ToString()) ? dr["postalCode"] : String.Empty),
							phoneNumber = (!String.IsNullOrEmpty(dr["phoneNumber"].ToString()) ? dr["phoneNumber"] : String.Empty)
						},
						invoice = new {
							ID = (!String.IsNullOrEmpty(dr["invoiceID"].ToString()) ? dr["invoiceID"] : String.Empty),
							name = new {
								th = (!String.IsNullOrEmpty(dr["invoiceNameTH"].ToString()) ? dr["invoiceNameTH"] : dr["invoiceNameEN"]),
								en = (!String.IsNullOrEmpty(dr["invoiceNameEN"].ToString()) ? dr["invoiceNameEN"] : dr["invoiceNameTH"])
							},
							namePrintReceipt = (!String.IsNullOrEmpty(dr["invoiceNamePrintReceipt"].ToString()) ? dr["invoiceNamePrintReceipt"] : String.Empty),
							billerID = (!String.IsNullOrEmpty(dr["billerID"].ToString()) ? dr["billerID"] : String.Empty),
							merchantName = (!String.IsNullOrEmpty(dr["merchantName"].ToString()) ? dr["merchantName"] : String.Empty),
							qrRef1 = (!String.IsNullOrEmpty(dr["qrRef_1"].ToString()) ? dr["qrRef_1"] : String.Empty),
							qrRef2 = (!String.IsNullOrEmpty(dr["qrRef_2"].ToString()) ? dr["qrRef_2"] : String.Empty),
							qrRef3 = (!String.IsNullOrEmpty(dr["qrRef_3"].ToString()) ? dr["qrRef_3"] : String.Empty),
							qrImage = (!String.IsNullOrEmpty(dr["qrImage"].ToString()) ? dr["qrImage"] : String.Empty),
							qrNewRef1 = (!String.IsNullOrEmpty(dr["qrNewRef_1"].ToString()) ? dr["qrNewRef_1"] : String.Empty),
							bankRequest = (!String.IsNullOrEmpty(dr["bankRequest"].ToString()) ? dr["bankRequest"] : String.Empty),
							bankTransID = (!String.IsNullOrEmpty(dr["bankTransID"].ToString()) ? dr["bankTransID"] : String.Empty),
							payment = new {
								amount = (!String.IsNullOrEmpty(dr["paidAmount"].ToString()) ? float.Parse(dr["paidAmount"].ToString()) : 0),
								confirmDate = (!String.IsNullOrEmpty(dr["paymentConfirmDates"].ToString()) ? dr["paymentConfirmDates"] : String.Empty),
								by = (!String.IsNullOrEmpty(dr["paidBy"].ToString()) ? dr["paidBy"] : String.Empty),
								date = (!String.IsNullOrEmpty(dr["paidDates"].ToString()) ? dr["paidDates"] : String.Empty),
								status = (!String.IsNullOrEmpty(dr["paidStatus"].ToString()) ? dr["paidStatus"] : "N")
                            },
                            privilege = new {
								ID = (!String.IsNullOrEmpty(dr["privilegeID"].ToString()) ? dr["privilegeID"] : String.Empty),
                                promoCode = (!String.IsNullOrEmpty(dr["privilegePromoCode"].ToString()) ? dr["privilegePromoCode"] : String.Empty),
                                name = new {
                                    th = (!String.IsNullOrEmpty(dr["privilegeNameTH"].ToString()) ? dr["privilegeNameTH"] : dr["privilegeNameEN"]),
                                    en = (!String.IsNullOrEmpty(dr["privilegeNameEN"].ToString()) ? dr["privilegeNameEN"] : dr["privilegeNameTH"])
                                },
                                detail = new {
                                    th = (!String.IsNullOrEmpty(dr["privilegeDetailTH"].ToString()) ? dr["privilegeDetailTH"] : dr["privilegeDetailEN"]),
                                    en = (!String.IsNullOrEmpty(dr["privilegeDetailEN"].ToString()) ? dr["privilegeDetailEN"] : dr["privilegeDetailTH"])
                                },
                                discount = (!String.IsNullOrEmpty(dr["discount"].ToString()) ? float.Parse(dr["discount"].ToString()) : 0),
                                expiredDate = (!String.IsNullOrEmpty(dr["privilegeExpiredDates"].ToString()) ? dr["privilegeExpiredDates"] : String.Empty),
                                usagedDate = (!String.IsNullOrEmpty(dr["privilegeUsagedDates"].ToString()) ? dr["privilegeUsagedDates"] : String.Empty),
                                status = (!String.IsNullOrEmpty(dr["privilegeStatus"].ToString()) ? dr["privilegeStatus"] : String.Empty)
                            }
						},
                        totalFeeAmount = (!String.IsNullOrEmpty(dr["totalFeeAmount"].ToString()) ? float.Parse(dr["totalFeeAmount"].ToString()) : 0),
                        seatNO = (!String.IsNullOrEmpty(dr["seatNO"].ToString()) ? dr["seatNO"] : String.Empty),
                        applicantNO = (!String.IsNullOrEmpty(dr["applicantNO"].ToString()) ? dr["applicantNO"] : String.Empty),
                        transScore = new {
                            eventCode = (!String.IsNullOrEmpty(dr["eventCode"].ToString()) ? dr["eventCode"] : String.Empty),
                            subject = (!String.IsNullOrEmpty(dr["subject"].ToString()) ? dr["subject"] : String.Empty),
                            applicantNo = (!String.IsNullOrEmpty(dr["applicantNO"].ToString()) ? dr["applicantNO"] : String.Empty),
                            adfsID = (!String.IsNullOrEmpty(dr["personID"].ToString()) ? dr["personID"] : String.Empty),
                            totalScore = (!String.IsNullOrEmpty(dr["totalScore"].ToString()) ? float.Parse(dr["totalScore"].ToString()).ToString("#.00") : null),
                            examResult = (!String.IsNullOrEmpty(dr["examResult"].ToString()) ? dr["examResult"] : String.Empty)
                        }
					});
				}
            }

			if (table.Equals("TransInvoiceFee")) {
				foreach (DataRow dr in dt.Rows) {
					list.Add(new {
						invoiceID = (!String.IsNullOrEmpty(dr["invoiceID"].ToString()) ? dr["invoiceID"] : String.Empty),
						feeType = new {
							ID = (!String.IsNullOrEmpty(dr["feeTypeID"].ToString()) ? dr["feeTypeID"] : String.Empty),
							name = new {
								th = (!String.IsNullOrEmpty(dr["feeTypeNameTH"].ToString()) ? dr["feeTypeNameTH"] : dr["feeTypeNameEN"]),
								en = (!String.IsNullOrEmpty(dr["feeTypeNameEN"].ToString()) ? dr["feeTypeNameEN"] : dr["feeTypeNameTH"])
							},
							amount = (!String.IsNullOrEmpty(dr["amount"].ToString()) ? float.Parse(dr["amount"].ToString()) : 0),
							toggle = (!String.IsNullOrEmpty(dr["toggle"].ToString()) ? dr["toggle"] : String.Empty)
						}
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
                        isSelected = (!String.IsNullOrEmpty(dr["isSelected"].ToString()) ? (dr["isSelected"].Equals("Y") ? true : false) : false)
                    });
                }
            }

            if (table.Equals("TransRegisteredWithTransProjectIDs")) {
                foreach (DataRow dr in dt.Rows) {
                    list.Add(new {
                        ID = (!String.IsNullOrEmpty(dr["transRegisteredID"].ToString()) ? dr["transRegisteredID"] : String.Empty),
                        registeredDate = (!String.IsNullOrEmpty(dr["registeredDates"].ToString()) ? dr["registeredDates"] : String.Empty),
                        transProject = new {
                            ID = (!String.IsNullOrEmpty(dr["transProjectID"].ToString()) ? dr["transProjectID"] : String.Empty),
                            project = new {
                                ID = (!String.IsNullOrEmpty(dr["projectID"].ToString()) ? dr["projectID"] : String.Empty),
                                category = new {
                                    ID = (!String.IsNullOrEmpty(dr["projectCategoryID"].ToString()) ? dr["projectCategoryID"] : String.Empty),
								}
							}
						}
                    });
                }
            }

            return list;
        }
		public static DataSet GetList(
			string personID,
			string paymentStatus
		) {
			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetListTransRegistered",
				new SqlParameter("@personID", personID),
				new SqlParameter("@paymentStatus", paymentStatus));

			return ds;
		}

        public static DataSet GetListWithTransProjectIDs(
            string personID,
            string transProjectIDs
        ) {
            DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetListTransRegisteredWithTransProjectIDs",
                new SqlParameter("@personID", personID),
                new SqlParameter("@transProjectIDs", transProjectIDs));

            return ds;
        }

        public static DataSet Get(
			string transRegisteredID,
			string personID,
			string transProjectID
		) {
			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetTransRegistered",
				new SqlParameter("@transRegisteredID", transRegisteredID),
				new SqlParameter("@personID", personID),
				new SqlParameter("@transProjectID", transProjectID));

			return ds;
		}

		public static DataSet Set(
			string method,
			string jsonData
		) {
			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscSetTransRegistered",
				new SqlParameter("@method", method),
				new SqlParameter("@jsonData", jsonData));

			return ds;
		}
	}
}