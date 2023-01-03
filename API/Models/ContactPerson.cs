/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๐๑/๐๖/๒๕๖๕>
Modify date : <๐๑/๐๖/๒๕๖๕>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace API.Models {
    public class ContactPerson {
        public static List<object> GetDataSource(string datas) {
            List<object> list = new List<object>();

            if (!String.IsNullOrEmpty(datas)) {
                foreach (var data in JsonConvert.DeserializeObject<dynamic>(datas)) {
                    list.Add(new {
                        fullName = new {
                            th = (!String.IsNullOrEmpty(data["FullName"]["TH"].ToString()) ? data["FullName"]["TH"] : data["FullName"]["EN"]),
                            en = (!String.IsNullOrEmpty(data["FullName"]["EN"].ToString()) ? data["FullName"]["EN"] : data["FullName"]["TH"])
                        },
                        email = (!String.IsNullOrEmpty(data["EmailAccount"].ToString()) ? data["EmailAccount"] : String.Empty),
                        phoneNumber = (!String.IsNullOrEmpty(data["TelephoneNO"].ToString()) ? data["TelephoneNO"] : String.Empty)
                    });
                }
            }

            return list;
        }
    }
}