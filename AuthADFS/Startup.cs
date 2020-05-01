/*
=============================================
Author      : <ยุทธภูมิ ตวันนา>
Create date : <๑๒/๑๒/๒๕๖๒>
Modify date : <๐๑/๐๕/๒๕๖๓>
Description : <>
=============================================
*/

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.WsFederation;
using Owin;
using System.IdentityModel;
using AuthorizationServer.Controllers;

[assembly: OwinStartup(typeof(AuthorizationServer.Startup))]

namespace AuthorizationServer
{
    public partial class Startup
    {
        /*
        private static string clientId = "e43a62d7-381a-453d-841c-2ec769f9cc8e";
        private static string clientSecret = "FT0bKrw90-B2dVYIzgmCuOR0vOFSdj1tJMI4I1Ri";
        private static string resource = "e43a62d7-381a-453d-841c-2ec769f9cc8e";
        private static string metadataAddress = "https://devadfs.mahidol.ac.th/adfs/.well-known/openid-configuration";
        private static string postLogoutRedirectUri = "http://localhost:4279";        
        
        private static string clientId = "ea4f5ba7-b59b-4673-84e5-429670b09081";
        private static string clientSecret = "kHs3H51qio83jF-Fdm1y-2PTiBSOzD771i2UPaml";
        private static string resource = "5ba08708-f353-44f1-89ab-5061cb9a285d";
        private static string metadataAddress = "https://idp.mahidol.ac.th/adfs/.well-known/openid-configuration";
        private static string postLogoutRedirectUri = "https://mursc.mahidol.ac.th/AuthorizationServer";       
        */
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            /*                
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                CookieManager = new SystemWebCookieManager()
            });
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
            */
        }
    }
}
