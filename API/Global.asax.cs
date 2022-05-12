using System;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace API {
	public class WebApiApplication: HttpApplication {
		protected void Application_Start() {
			GlobalConfiguration.Configure(WebApiConfig.Register);

			JavaScriptSerializer json = new JavaScriptSerializer();
			json.MaxJsonLength = Int32.MaxValue;
		}
	}
}
