/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๓๐/๐๖/๒๕๖๓>
Modify date : <๑๒/๐๗/๒๕๖๓>
Description : <>
=============================================
*/

using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using API.Models;

namespace API.Controllers
{
	[RoutePrefix("TransDeliveryAddress")]
	public class TransDeliveryAddressController : ApiController
	{
		[Route("Put")]
		[HttpPut]
		public HttpResponseMessage Put()
		{
			string jsonData = String.Empty;
			string transDeliAddressID = String.Empty;
			string transRegisteredID = String.Empty;
			string address = String.Empty;
			string createdBy = String.Empty;

			if (Util.GetIsAuthenticatedByAuthenADFS())
					jsonData = Request.Content.ReadAsStringAsync().Result;

			if (!String.IsNullOrEmpty(jsonData))
			{
				try
				{
					dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonData);

					transDeliAddressID = jsonObject["transDeliAddressID"];
					transRegisteredID = jsonObject["transRegisteredID"];
					address = JsonConvert.SerializeObject(jsonObject["deliAddress"]);
					createdBy = jsonObject["createdBy"];
				}
				catch
				{
				}
			}

			DataTable dt = TransDeliveryAddress.Set(transDeliAddressID, transRegisteredID, address, createdBy).Tables[0];

			return Request.CreateResponse(HttpStatusCode.OK, Util.APIResponse.GetData(dt));
		}
	}
}
