/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๒/๑๒/๒๕๖๒>
Modify date : <๑๓/๐๔/๒๕๖๓>
Description : <>
=============================================
*/

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AuthorizationServer.Startup))]

namespace AuthorizationServer
{
    public class Startup
    {
        private static string clientId = "e43a62d7-381a-453d-841c-2ec769f9cc8e";
        private static string clientSecret = "FT0bKrw90-B2dVYIzgmCuOR0vOFSdj1tJMI4I1Ri";
        private static string resource = "e43a62d7-381a-453d-841c-2ec769f9cc8e";
        private static string metadataAddress = "https://devadfs.mahidol.ac.th/adfs/.well-known/openid-configuration";
        private static string postLogoutRedirectUri = "http://localhost:4279";
        /*
        private static string clientId = "fe98b411-e6a5-4f45-bf31-27e4d94bb16d";
        private static string clientSecret = "FT0bKrw90-B2dVYIzgmCuOR0vOFSdj1tJMI4I1Ri";
        private static string resource = "f5c0346b-3ef6-4d34-9dad-eb216b9a2a48";
        private static string metadataAddress = "https://idp.mahidol.ac.th/adfs/.well-known/openid-configuration";
        private static string postLogoutRedirectUri = "https://extrabenefits.mahidol.ac.th/";
        */
        public void Configuration(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Resource = resource,
                Scope = "openid allatclaims",
                ResponseType = "code id_token",
                MetadataAddress = metadataAddress,
                PostLogoutRedirectUri = postLogoutRedirectUri,
                RedirectUri = postLogoutRedirectUri,
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = async notification =>
                    {
                        notification.AuthenticationTicket.Identity.AddClaim(new Claim("id_token", notification.ProtocolMessage.IdToken));
                    },
                    RedirectToIdentityProvider = async n =>
                    {
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token").Value;
                            n.ProtocolMessage.IdTokenHint = idTokenHint;
                        }
                    },
                    AuthenticationFailed = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/Error?message=" + context.Exception.Message);

                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}
