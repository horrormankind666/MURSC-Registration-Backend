/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๑/๐๖/๒๕๖๕>
Modify date : <๑๒/๐๙/๒๕๖๖>
Description : <>
=============================================
*/

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace API.Models {
    public class StudentProfile {
        public static List<object> GetDataSource(DataTable dt) {
            List<object> list = new List<object>();

            if (dt.Rows.Count > 0) {
                DataRow dr = dt.Rows[0];

                list.Add(new {
                    studentCode = dr["studentCode"].ToString(),
                    titleTH = dr["titleTH"].ToString(),
                    titleEN = dr["titleEN"].ToString(),
                    firstNameTH = dr["firstName"].ToString(),
                    middleNameTH = dr["middleName"].ToString(),
                    lastNameTH = dr["lastName"].ToString(),
                    firstNameEN = dr["firstNameEN"].ToString(),
                    middleNameEN = dr["middleNameEN"].ToString(),
                    lastNameEN = dr["lastNameEN"].ToString(),
                    facultyNameTH = dr["facultyNameTH"].ToString(),
                    facultyNameEN = dr["facultyNameEN"].ToString(),
                    programNameTH = dr["programNameTH"].ToString(),
                    programNameEN = dr["programNameEN"].ToString(),
                    admissionYear = dr["admissionYear"].ToString(),
                    idCard = dr["idCard"].ToString(),
                    birthDate = dr["birthDate"].ToString(),
                    address = dr["address"].ToString(),
                    subdistrict = dr["subdistrict"].ToString(),
                    district = dr["district"].ToString(),
                    province = dr["province"].ToString(),
                    country = dr["country"].ToString(),
                    zipCode = dr["zipCode"].ToString(),
                    phoneNumber = dr["phoneNumber"].ToString()
                });
            }

            return list;
        }

        public static DataSet Get(string studentCode) {
            DataSet ds = Util.ExecuteCommandStoredProcedure(Util.infinityConnectionString, "sp_rscGetStudentProfile",
                new SqlParameter("@studentCode", studentCode));

            return ds;
        }
    }
}