/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๗/๐๕/๒๕๖๓>
Modify date : <๐๒/๑๐/๒๕๖๓>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using API.Models;

namespace API.Controllers
{
	[RoutePrefix("TransRegistered")]
	public class TransRegisteredController : ApiController
	{
		[Route("GetList")]
		[HttpGet]
		public HttpResponseMessage GetList(string paymentStatus = null)
		{
			List<object> list = new List<object>();

			if (Util.GetIsAuthenticatedByAuthenADFS())
			{
				object obj = Util.GetPPIDByAuthenADFS();
				string ppid = obj.GetType().GetProperty("ppid").GetValue(obj, null).ToString();
				string winaccountName = obj.GetType().GetProperty("winaccountName").GetValue(obj, null).ToString();

				DataSet ds = TransRegistered.GetList((!String.IsNullOrEmpty(ppid) ? ppid : winaccountName), paymentStatus);

				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					DataTable country = Country.Get(dr["countryID"].ToString());
					DataTable province = Province.Get(dr["countryID"].ToString(), dr["provinceID"].ToString());
					DataTable district = District.Get(dr["countryID"].ToString(), dr["provinceID"].ToString(), dr["districtID"].ToString());
					DataTable subdistrict = Subdistrict.Get(dr["countryID"].ToString(), dr["provinceID"].ToString(), dr["districtID"].ToString(), dr["subdistrictID"].ToString());

					list.Add(new
					{
						transRegisteredID = dr["transRegisteredID"],
						registeredDate = dr["registeredDate"],
						registeredDates = dr["registeredDates"],
						transProjectID = dr["transProjectID"],
						projectCategoryID = dr["projectCategoryID"],
						projectCategoryNameTH = dr["projectCategoryNameTH"],
						projectCategoryNameEN = dr["projectCategoryNameEN"],
						projectCategoryInitial = dr["projectCategoryInitial"],
						projectID = dr["projectID"],
						logo = dr["logo"],
						projectNameTH = dr["projectNameTH"],
						projectNameEN = dr["projectNameEN"],
						subject = dr["subject"],
						descriptionTH = dr["descriptionTH"],
						descriptionEN = dr["descriptionEN"],
						aboutTH = dr["aboutTH"],
						aboutEN = dr["aboutEN"],
						isExam = dr["isExam"],
						isTeaching = dr["isTeaching"],
						examStartDate = dr["examStartDate"],
						examStartDates = dr["examStartDates"],
						examEndDate = dr["examEndDate"],
						examEndDates = dr["examEndDates"],
						lastPaymentDate = dr["lastPaymentDate"],
						lastPaymentDates = dr["lastPaymentDates"],
						paymentExpire = dr["paymentExpire"],
						announceDate = dr["announceDate"],
						announceDates = dr["announceDates"],
						contactPerson = JsonConvert.DeserializeObject<dynamic>(dr["contactPerson"].ToString()),
						userTypeSpecific = (!String.IsNullOrEmpty(dr["userTypeSpecific"].ToString()) ? dr["userTypeSpecific"].ToString().Split(',') : dr["userTypeSpecific"]),
						transLocationID = dr["transLocationID"],
						locationID = dr["locationID"],
						locationNameTH = dr["locationNameTH"],
						locationNameEN = dr["locationNameEN"],
						buildingID = dr["buildingID"],
						buildingNameTH = dr["buildingNameTH"],
						buildingNameEN = dr["buildingNameEN"],
						transDeliAddressID = dr["transDeliAddressID"],
						address = dr["address"],
						country = (country.Rows.Count > 0 ? country.Rows[0].Table : null),
						province = (province.Rows.Count > 0 ? province.Rows[0].Table : null),
						district = (district.Rows.Count > 0 ? district.Rows[0].Table : null),
						subdistrict = (subdistrict.Rows.Count > 0 ? subdistrict.Rows[0].Table : null),
						postalCode = dr["postalCode"],
						phoneNumber = dr["phoneNumber"],
						invoiceID = dr["invoiceID"],
						invoiceNameTH = dr["invoiceNameTH"],
						invoiceNameEN = dr["invoiceNameEN"],
						invoiceNamePrintReceipt = dr["invoiceNamePrintReceipt"],
						billerID = dr["billerID"],
						merchantName = dr["merchantName"],
						qrRef1 = dr["qrRef_1"],
						qrRef2 = dr["qrRef_2"],
						qrRef3 = dr["qrRef_3"],
						qrImage = dr["qrImage"],
						qrNewRef1 = dr["qrNewRef_1"],
						bankRequest = dr["bankRequest"],
						bankTransID = dr["bankTransID"],
						paidAmount = dr["paidAmount"],
						paidBy = dr["paidBy"],
						paidDate = dr["paidDate"],
						paidDates = dr["paidDates"],
						paidStatus = dr["paidStatus"],
						totalFeeAmount = dr["totalFeeAmount"],
						paymentConfirmDate = dr["paymentConfirmDates"],
						seatNO = dr["seatNO"],
						applicantNO = dr["applicantNO"],
						eventCode = dr["eventCode"],
						totalScore = dr["totalScore"],
						examResult = dr["examResult"]
					});
				}
			}

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list));
		}

		[Route("Get")]
		[HttpGet]
		public HttpResponseMessage Get(string cuid = null)
		{
			string transRegisteredID = String.Empty;
			string transProjectID = String.Empty;
			string[] cuidArray = Util.CUID2Array(cuid);

			if (cuidArray != null)
			{
				int i = 1;

				foreach (var data in cuidArray)
				{
					if (i.Equals(1)) transRegisteredID = data;
					if (i.Equals(2)) transProjectID = data;

					i++;
				}
			}

			List<object> list = new List<object>();

			if (Util.GetIsAuthenticatedByAuthenADFS())
			{
				object obj = Util.GetPPIDByAuthenADFS();
				string ppid = obj.GetType().GetProperty("ppid").GetValue(obj, null).ToString();
				string winaccountName = obj.GetType().GetProperty("winaccountName").GetValue(obj, null).ToString();

				DataSet ds = TransRegistered.Get(transRegisteredID, (!String.IsNullOrEmpty(ppid) ? ppid : winaccountName), transProjectID);
				DataTable dt1 = ds.Tables[0];
				DataTable dt2 = ds.Tables[1];
				DataTable dt3 = ds.Tables[2];

				if (dt1.Rows.Count > 0)
				{
					DataRow dr = dt1.Rows[0];
					DataTable country = Country.Get(dr["countryID"].ToString());
					DataTable province = Province.Get(dr["countryID"].ToString(), dr["provinceID"].ToString());
					DataTable district = District.Get(dr["countryID"].ToString(), dr["provinceID"].ToString(), dr["districtID"].ToString());
					DataTable subdistrict = Subdistrict.Get(dr["countryID"].ToString(), dr["provinceID"].ToString(), dr["districtID"].ToString(), dr["subdistrictID"].ToString());

					list.Add(new
					{
						transRegisteredID = dr["transRegisteredID"],
						registeredDate = dr["registeredDate"],
						registeredDates = dr["registeredDates"],
						transProjectID = dr["transProjectID"],
						projectCategoryID = dr["projectCategoryID"],
						projectCategoryNameTH = dr["projectCategoryNameTH"],
						projectCategoryNameEN = dr["projectCategoryNameEN"],
						projectCategoryInitial = dr["projectCategoryInitial"],
						projectID = dr["projectID"],
						logo = dr["logo"],
						projectNameTH = dr["projectNameTH"],
						projectNameEN = dr["projectNameEN"],
						subject = dr["subject"],
						descriptionTH = dr["descriptionTH"],
						descriptionEN = dr["descriptionEN"],
						aboutTH = dr["aboutTH"],
						aboutEN = dr["aboutEN"],
						isExam = dr["isExam"],
						isTeaching = dr["isTeaching"],
						examStartDate = dr["examStartDate"],
						examStartDates = dr["examStartDates"],
						examEndDate = dr["examEndDate"],
						examEndDates = dr["examEndDates"],
						lastPaymentDate = dr["lastPaymentDate"],
						lastPaymentDates = dr["lastPaymentDates"],
						paymentExpire = dr["paymentExpire"],
						announceDate = dr["announceDate"],
						announceDates = dr["announceDates"],
						contactPerson = JsonConvert.DeserializeObject<dynamic>(dr["contactPerson"].ToString()),
						userTypeSpecific = (!String.IsNullOrEmpty(dr["userTypeSpecific"].ToString()) ? dr["userTypeSpecific"].ToString().Split(',') : dr["userTypeSpecific"]),
						transLocationID = dr["transLocationID"],
						locationID = dr["locationID"],
						locationNameTH = dr["locationNameTH"],
						locationNameEN = dr["locationNameEN"],
						buildingID = dr["buildingID"],
						buildingNameTH = dr["buildingNameTH"],
						buildingNameEN = dr["buildingNameEN"],
						transDeliAddressID = dr["transDeliAddressID"],
						address = dr["address"],
						country = (country.Rows.Count > 0 ? country.Rows[0].Table : null),
						province = (province.Rows.Count > 0 ? province.Rows[0].Table : null),
						district = (district.Rows.Count > 0 ? district.Rows[0].Table : null),
						subdistrict = (subdistrict.Rows.Count > 0 ? subdistrict.Rows[0].Table : null),
						postalCode = dr["postalCode"],
						phoneNumber = dr["phoneNumber"],
						invoiceID = dr["invoiceID"],
						invoiceNameTH = dr["invoiceNameTH"],
						invoiceNameEN = dr["invoiceNameEN"],
						invoiceNamePrintReceipt = dr["invoiceNamePrintReceipt"],
						billerID = dr["billerID"],
						merchantName = dr["merchantName"],
						qrRef1 = dr["qrRef_1"],
						qrRef2 = dr["qrRef_2"],
						qrRef3 = dr["qrRef_3"],
						qrImage = dr["qrImage"],
						qrNewRef1 = dr["qrNewRef_1"],
						bankRequest = dr["bankRequest"],
						bankTransID = dr["bankTransID"],
						paidAmount = dr["paidAmount"],
						paidBy = dr["paidBy"],
						paidDate = dr["paidDate"],
						paidDates = dr["paidDates"],
						paidStatus = dr["paidStatus"],
						fee = (dt2.Rows.Count > 0 ? dt2.Rows[0].Table : null),
						totalFeeAmount = dr["totalFeeAmount"],
						feeType = (dt3.Rows.Count > 0 ? dt3.Rows[0].Table : null),
						paymentConfirmDate = dr["paymentConfirmDates"],
						seatNO = dr["seatNO"],
						applicantNO = dr["applicantNO"],
						eventCode = dr["eventCode"],
						totalScore = dr["totalScore"],
						examResult = dr["examResult"]
					});
				}
			}

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(list.ToList()));
		}

		[Route("Post")]
		[HttpPost]
		public HttpResponseMessage Post()
		{
			string jsonData = String.Empty;
                
			if (Util.GetIsAuthenticatedByAuthenADFS())
				jsonData = Request.Content.ReadAsStringAsync().Result;

			if (!String.IsNullOrEmpty(jsonData))
			{
				try
				{
					JObject jsonObject = new JObject(JsonConvert.DeserializeObject<dynamic>(jsonData));
					object obj = Util.GetPPIDByAuthenADFS();
					string ppid = obj.GetType().GetProperty("ppid").GetValue(obj, null).ToString();
					string winaccountName = obj.GetType().GetProperty("winaccountName").GetValue(obj, null).ToString();

					jsonObject.Add("personID", (!String.IsNullOrEmpty(ppid) ? ppid : winaccountName));
					jsonObject.Add("createdBy", winaccountName);

					jsonData = JsonConvert.SerializeObject(jsonObject);
				}
				catch
				{
					jsonData = String.Empty;
				}
			}

			DataTable dt = TransRegistered.Set("POST", jsonData).Tables[0];

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(dt));
		}
	}
}
