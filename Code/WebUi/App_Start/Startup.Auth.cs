using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Security;
using WebUi.App_Start;

[assembly: OwinStartup(typeof(Startup))]

namespace WebUi.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the Application Sign In Cookie.
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                AuthenticationMode = AuthenticationMode.Passive,
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/Logout"),
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enable the External Sign In Cookie.
            //app.SetDefaultSignInAsAuthenticationType(DefaultAuthenticationTypes.ExternalCookie);


            // Enable Google authentication.
            app.UseGoogleAuthentication(GetGoogleOptions());
        }

        private GoogleOAuth2AuthenticationOptions GetGoogleOptions()
        {
            var reader = new KeyReader();
            var keys = reader.GetKey("google");
            var options = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = keys.Public,
                ClientSecret = keys.Private,
                //CallbackPath = new PathString("/Account/ExternalCallback")
            };
            return options;
        }
    }
}