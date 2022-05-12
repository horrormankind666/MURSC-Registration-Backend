
/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๒/๑๒/๒๕๖๒>
Modify date : <๑๒/๑๒/๒๕๖๒>
Description : <>
=============================================
*/

using System.Web.Mvc;

namespace AuthorizationServer {
	public class FilterConfig {
		public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
			filters.Add(new HandleErrorAttribute());
		}
	}
}
