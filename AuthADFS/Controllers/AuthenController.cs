/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๒/๑๒/๒๕๖๒>
Modify date : <๐๑/๐๕/๒๕๖๒>
Description : <>
=============================================
*/

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;

namespace AuthorizationServer.Controllers
{
	public class ClaimTypes
	{
		public const string Actor = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor";
		public const string Anonymous = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/anonymous";
		public const string Authentication = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication";
		public const string AuthenticationInstant = "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant";
		public const string AuthenticationMethod = "http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod";
		public const string AuthorizationDecision = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authorizationdecision";
		public const string ClaimType2005Namespace = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims";
		public const string ClaimType2009Namespace = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims";
		public const string ClaimTypeNamespace = "http://schemas.microsoft.com/ws/2008/06/identity/claims";
		public const string CookiePath = "http://schemas.microsoft.com/ws/2008/06/identity/claims/cookiepath";
		public const string Country = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country";
		public const string DateOfBirth = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth";
		public const string DenyOnlyPrimaryGroupSid = "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarygroupsid";
		public const string DenyOnlyPrimarySid = "http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarysid";
		public const string DenyOnlySid = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/denyonlysid";
		public const string Dns = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dns";
		public const string Dsa = "http://schemas.microsoft.com/ws/2008/06/identity/claims/dsa";
		public const string Email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
		public const string Expiration = "http://schemas.microsoft.com/ws/2008/06/identity/claims/expiration";
		public const string Expired = "http://schemas.microsoft.com/ws/2008/06/identity/claims/expired";
		public const string Gender = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender";
		public const string GivenName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
		public const string GroupSid = "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid";
		public const string Hash = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/hash";
		public const string HomePhone = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/homephone";
		public const string IsPersistent = "http://schemas.microsoft.com/ws/2008/06/identity/claims/ispersistent";
		public const string Locality = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/locality";
		public const string MobilePhone = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone";
		public const string Name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
		public const string NameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
		public const string OtherPhone = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/otherphone";
		public const string PostalCode = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/postalcode";
		public const string PPID = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/privatepersonalidentifier";
		public const string PrimaryGroupSid = "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid";
		public const string PrimarySid = "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid";
		public const string Role = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
		public const string Rsa = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/rsa";
		public const string SerialNumber = "http://schemas.microsoft.com/ws/2008/06/identity/claims/serialnumber";
		public const string Sid = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid";
		public const string Spn = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/spn";
		public const string StateOrProvince = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/stateorprovince";
		public const string StreetAddress = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/streetaddress";
		public const string Surname = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
		public const string System = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/system";
		public const string Thumbprint = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/thumbprint";
		public const string Upn = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn";
		public const string Uri = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri";
		public const string UserData = "http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata";
		public const string Version = "http://schemas.microsoft.com/ws/2008/06/identity/claims/version";
		public const string Webpage = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/webpage";
		public const string WindowsAccountName = "http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsaccountname";
		public const string X500DistinguishedName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/x500distinguishedname";
	}

	public class Claims
	{
		public object Request { get; private set; }

		public class OpenID
		{
			public string id_token { get; set; }
			public string appid { get; set; }
			public string apptype { get; set; }
			public string aud { get; set; }
			public string auth_time { get; set; }
			public string c_hash { get; set; }
			public string exp { get; set; }
			public string iat { get; set; }
			public string iss { get; set; }
			public string nonce { get; set; }
			public string sid { get; set; }
			public string ver { get; set; }
		}

		public class Profile
		{
			public string actor { get; set; }
			public string anonymous { get; set; }
			public string authentication { get; set; }
			public string authenticationInstant { get; set; }
			public string authenticationMethod { get; set; }
			public string authorizationDecision { get; set; }
			public string claimType2005Namespace { get; set; }
			public string claimType2009Namespace { get; set; }
			public string claimTypeNamespace { get; set; }
			public string cookiePath { get; set; }
			public string country { get; set; }
			public string dateOfBirth { get; set; }
			public string denyOnlyPrimaryGroupSid { get; set; }
			public string denyOnlyPrimarySid { get; set; }
			public string denyOnlySid { get; set; }
			public string dns { get; set; }
			public string dsa { get; set; }
			public string email { get; set; }
			public string expiration { get; set; }
			public string expired { get; set; }
			public string gender { get; set; }
			public string givenName { get; set; }
			public string groupSid { get; set; }
			public string hash { get; set; }
			public string homePhone { get; set; }
			public string isPersistent { get; set; }
			public string locality { get; set; }
			public string mobilePhone { get; set; }
			public string name { get; set; }
			public string nameIdentifier { get; set; }
			public string otherPhone { get; set; }
			public string postalCode { get; set; }
			public string ppid { get; set; }
			public string primaryGroupSid { get; set; }
			public string primarySid { get; set; }
			public string role { get; set; }
			public string rsa { get; set; }
			public string serialNumber { get; set; }
			public string sid { get; set; }
			public string spn { get; set; }
			public string stateOrProvince { get; set; }
			public string streetAddress { get; set; }
			public string surname { get; set; }
			public string system { get; set; }
			public string thumbprint { get; set; }
			public string upn { get; set; }
			public string uri { get; set; }
			public string userData { get; set; }
			public string version { get; set; }
			public string webpage { get; set; }
			public string windowsAccountName { get; set; }
			public string x500DistinguishedName { get; set; }
		}

		public object UserInfo()
		{
			/*
			string aaa = String.Empty;

			foreach (var claim in ((ClaimsPrincipal)User).Claims)
			{
				aaa += (claim.Type + " " + claim.Value);
			}

			return aaa;
			*/

			var openIDObj = new Claims.OpenID();
			var profileObj = new Claims.Profile();

			try
			{
				openIDObj.id_token = ClaimsPrincipal.Current.FindFirst("id_token")?.Value ?? String.Empty;
				openIDObj.appid = ClaimsPrincipal.Current.FindFirst("appid")?.Value ?? String.Empty;
				openIDObj.apptype = ClaimsPrincipal.Current.FindFirst("apptype")?.Value ?? String.Empty;
				openIDObj.aud = ClaimsPrincipal.Current.FindFirst("aud")?.Value ?? String.Empty;
				openIDObj.auth_time = ClaimsPrincipal.Current.FindFirst("auth_time")?.Value ?? String.Empty;
				openIDObj.c_hash = ClaimsPrincipal.Current.FindFirst("c_hash")?.Value ?? String.Empty;
				openIDObj.exp = ClaimsPrincipal.Current.FindFirst("exp")?.Value ?? String.Empty;
				openIDObj.iat = ClaimsPrincipal.Current.FindFirst("iat")?.Value ?? String.Empty;
				openIDObj.iss = ClaimsPrincipal.Current.FindFirst("iss")?.Value ?? String.Empty;
				openIDObj.nonce = ClaimsPrincipal.Current.FindFirst("nonce")?.Value ?? String.Empty;
				openIDObj.sid = ClaimsPrincipal.Current.FindFirst("sid")?.Value ?? String.Empty;
				openIDObj.ver = ClaimsPrincipal.Current.FindFirst("ver")?.Value ?? String.Empty;

				profileObj.actor = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Actor)?.Value ?? String.Empty;
				profileObj.anonymous = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Anonymous)?.Value ?? String.Empty;
				profileObj.authentication = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Authentication)?.Value ?? String.Empty;
				profileObj.authenticationInstant = ClaimsPrincipal.Current.FindFirst(ClaimTypes.AuthenticationInstant)?.Value ?? String.Empty;
				profileObj.authenticationMethod = ClaimsPrincipal.Current.FindFirst(ClaimTypes.AuthenticationMethod)?.Value ?? String.Empty;
				profileObj.authorizationDecision = ClaimsPrincipal.Current.FindFirst(ClaimTypes.AuthorizationDecision)?.Value ?? String.Empty;
				profileObj.claimType2005Namespace = ClaimsPrincipal.Current.FindFirst(ClaimTypes.ClaimType2005Namespace)?.Value ?? String.Empty;
				profileObj.claimType2009Namespace = ClaimsPrincipal.Current.FindFirst(ClaimTypes.ClaimType2009Namespace)?.Value ?? String.Empty;
				profileObj.claimTypeNamespace = ClaimsPrincipal.Current.FindFirst(ClaimTypes.ClaimTypeNamespace)?.Value ?? String.Empty;
				profileObj.cookiePath = ClaimsPrincipal.Current.FindFirst(ClaimTypes.CookiePath)?.Value ?? String.Empty;
				profileObj.country = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Country)?.Value ?? String.Empty;
				profileObj.dateOfBirth = ClaimsPrincipal.Current.FindFirst(ClaimTypes.DateOfBirth)?.Value ?? String.Empty;
				profileObj.denyOnlyPrimaryGroupSid = ClaimsPrincipal.Current.FindFirst(ClaimTypes.DenyOnlyPrimaryGroupSid)?.Value ?? String.Empty;
				profileObj.denyOnlyPrimarySid = ClaimsPrincipal.Current.FindFirst(ClaimTypes.DenyOnlyPrimarySid)?.Value ?? String.Empty;
				profileObj.denyOnlySid = ClaimsPrincipal.Current.FindFirst(ClaimTypes.DenyOnlySid)?.Value ?? String.Empty;
				profileObj.dns = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Dns)?.Value ?? String.Empty;
				profileObj.dsa = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Dsa)?.Value ?? String.Empty;
				profileObj.email = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email)?.Value ?? String.Empty;
				profileObj.expiration = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Expiration)?.Value ?? String.Empty;
				profileObj.expired = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Expired)?.Value ?? String.Empty;
				profileObj.gender = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Gender)?.Value ?? String.Empty;
				profileObj.givenName = ClaimsPrincipal.Current.FindFirst(ClaimTypes.GivenName)?.Value ?? String.Empty;
				profileObj.groupSid = ClaimsPrincipal.Current.FindFirst(ClaimTypes.GroupSid)?.Value ?? String.Empty;
				profileObj.hash = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Hash)?.Value ?? String.Empty;
				profileObj.homePhone = ClaimsPrincipal.Current.FindFirst(ClaimTypes.HomePhone)?.Value ?? String.Empty;
				profileObj.isPersistent = ClaimsPrincipal.Current.FindFirst(ClaimTypes.IsPersistent)?.Value ?? String.Empty;
				profileObj.locality = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Locality)?.Value ?? String.Empty;
				profileObj.mobilePhone = ClaimsPrincipal.Current.FindFirst(ClaimTypes.MobilePhone)?.Value ?? String.Empty;
				profileObj.name = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Name)?.Value ?? String.Empty;
				profileObj.nameIdentifier = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? String.Empty;
				profileObj.otherPhone = ClaimsPrincipal.Current.FindFirst(ClaimTypes.OtherPhone)?.Value ?? String.Empty;
				profileObj.postalCode = ClaimsPrincipal.Current.FindFirst(ClaimTypes.PostalCode)?.Value ?? String.Empty;
				profileObj.ppid = ClaimsPrincipal.Current.FindFirst(ClaimTypes.PPID)?.Value ?? String.Empty;
				profileObj.primaryGroupSid = ClaimsPrincipal.Current.FindFirst(ClaimTypes.PrimaryGroupSid)?.Value ?? String.Empty;
				profileObj.primarySid = ClaimsPrincipal.Current.FindFirst(ClaimTypes.PrimarySid)?.Value ?? String.Empty;
				profileObj.role = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Role)?.Value ?? String.Empty;
				profileObj.rsa = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Rsa)?.Value ?? String.Empty;
				profileObj.serialNumber = ClaimsPrincipal.Current.FindFirst(ClaimTypes.SerialNumber)?.Value ?? String.Empty;
				profileObj.sid = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Sid)?.Value ?? String.Empty;
				profileObj.spn = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Spn)?.Value ?? String.Empty;
				profileObj.stateOrProvince = ClaimsPrincipal.Current.FindFirst(ClaimTypes.StateOrProvince)?.Value ?? String.Empty;
				profileObj.streetAddress = ClaimsPrincipal.Current.FindFirst(ClaimTypes.StreetAddress)?.Value ?? String.Empty;
				profileObj.surname = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Surname)?.Value ?? String.Empty;
				profileObj.system = ClaimsPrincipal.Current.FindFirst(ClaimTypes.System)?.Value ?? String.Empty;
				profileObj.thumbprint = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Thumbprint)?.Value ?? String.Empty;
				profileObj.upn = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Upn)?.Value ?? String.Empty;
				profileObj.uri = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Uri)?.Value ?? String.Empty;
				profileObj.userData = ClaimsPrincipal.Current.FindFirst(ClaimTypes.UserData)?.Value ?? String.Empty;
				profileObj.version = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Version)?.Value ?? String.Empty;
				profileObj.webpage = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Webpage)?.Value ?? String.Empty;
				profileObj.windowsAccountName = ClaimsPrincipal.Current.FindFirst(ClaimTypes.WindowsAccountName)?.Value ?? String.Empty;
				profileObj.x500DistinguishedName = ClaimsPrincipal.Current.FindFirst(ClaimTypes.X500DistinguishedName)?.Value ?? String.Empty;
			}
			catch
			{
			}

			object userInfoResult = new { openID = openIDObj, profile = profileObj };
                        
			return userInfoResult;
		}
	}
    
	public class AuthenController : Controller
	{
		[Route("UserInfo")]
		[HttpGet]
		public ActionResult UserInfo()
		{
			Claims c = new Claims();
			List<object> userInfoList = new List<object>();
			userInfoList.Add(c.UserInfo());

			object userInfoResult = new { data = userInfoList };

			return Json(userInfoResult, JsonRequestBehavior.AllowGet);
		}

		public void SignIn()
		{
			HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/AuthADFS" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
		}

		public void SignOut()
		{
			this.EndSession();
			HttpContext.GetOwinContext().Authentication.SignOut(OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
		}

		public void EndSession()
		{
			HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
		}
	}
}