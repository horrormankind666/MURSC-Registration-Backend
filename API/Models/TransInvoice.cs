/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๕/๑๐/๒๕๖๓>
Modify date : <๒๓/๑๒/๒๕๖๕>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace API.Models {
    public class TransInvoice {
		public static List<object> GetDataSource(DataTable dt) {
			List<object> list = new List<object>();

			foreach (DataRow dr in dt.Rows) {
				list.Add(new {
					transProject = new {
                        lastPaymentDate = (!String.IsNullOrEmpty(dr["lastPaymentDates"].ToString()) ? dr["lastPaymentDates"] : String.Empty),
                        paymentExpire = (!String.IsNullOrEmpty(dr["paymentExpire"].ToString()) ? dr["paymentExpire"] : String.Empty)
                    },
					ID = (!String.IsNullOrEmpty(dr["ID"].ToString()) ? dr["ID"] : String.Empty),
                    transRegisteredID = (!String.IsNullOrEmpty(dr["transRegisteredID"].ToString()) ? dr["transRegisteredID"] : String.Empty),
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
					}
				});
            }

            return list;
        }

        public static DataSet Get(
            string transInvoiceID,
            string transRegisteredID
        ) {
			DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscGetTransInvoice",
                new SqlParameter("@transInvoiceID", transInvoiceID),
                new SqlParameter("@transRegisteredID", transRegisteredID));

			return ds;
		}

        public static DataSet Set(
            string transRegisteredID,
            string fee,
            string createdBy
        ) {
            DataSet ds = Util.ExecuteCommandStoredProcedure(Util.connectionString, "sp_rscSetTransInvoice",
                new SqlParameter("@transRegisteredID", transRegisteredID),
                new SqlParameter("@fee", fee),
                new SqlParameter("@createdBy", createdBy));

            return ds;
        }
    }
}