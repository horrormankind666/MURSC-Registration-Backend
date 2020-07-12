/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๒๗/๐๕/๒๕๖๓>
Modify date : <๑๒/๐๗/๒๕๖๓>
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
using API.Models;

namespace API.Controllers
{
	[RoutePrefix("TransRegistered")]
	public class TransRegisteredController : ApiController
	{
		[Route("Get")]
		[HttpGet]
		public HttpResponseMessage Get(
			string transRegisteredID = null,
			string personID = null,
			string transProjectID = null)
		{
			List<object> list = new List<object>();

			if (Util.GetIsAuthenticatedByAuthenADFS())
			{
				DataSet ds = TransRegistered.Get(transRegisteredID, personID, transProjectID);
				DataTable dt1 = ds.Tables[0];
				DataTable dt2 = ds.Tables[1];               

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
						about = dr["about"],
						examStartDate = dr["examStartDate"],
						examStartDates = dr["examStartDates"],
						examEndDate = dr["examEndDate"],
						examEndDates = dr["examEndDates"],
						lastPaymentDate = dr["lastPaymentDate"],
						lastPaymentDates = dr["lastPaymentDates"],
						contactNameTH = dr["contactNameTH"],
						contactNameEN = dr["contactNameEN"],
						contactEmail = dr["contactEmail"],
						contactPhone = dr["contactPhone"],
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
						qrRef_1 = dr["qrRef_1"],
						qrRef_2 = dr["qrRef_2"],
						qrRef_3 = dr["qrRef_3"],
						bankRequest = dr["bankRequest"],
						bankTransID = dr["bankTransID"],
						paidAmount = dr["paidAmount"],
						paidBy = dr["paidBy"],
						paidDate = dr["paidDate"],
						paidDates = dr["paidDates"],
						paidStatus = dr["paidStatus"],
						fee = (dt2.Rows.Count > 0 ? dt2.Rows[0].Table : null),
						totalFeeAmount = dr["totalFeeAmount"]
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

			DataTable dt = TransRegistered.Set("POST", jsonData).Tables[0];

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(dt));
		}
	}
}
