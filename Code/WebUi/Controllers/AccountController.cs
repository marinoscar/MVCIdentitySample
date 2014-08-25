using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Security;
using WebUi.App_Start;
using WebUi.Models;

namespace WebUi.Controllers
{


    /// <summary>
    /// http://stackoverflow.com/questions/23742086/getexternallogininfoasync-null-with-owin-in-externallogincallback-unless-already
    /// http://blog.technovert.com/2014/01/google-openid-integration-issues-asp-net-identity/
    /// </summary>
    public class AccountController : Controller
    {

        #region Variable Declaration

        private ApplicationSignInManager _signInManager;

        #endregion

        #region Property Implementation


        public IOwinContext OwinContext
        {
            get { return HttpContext.GetOwinContext(); }
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? (_signInManager = OwinContext.Get<ApplicationSignInManager>()); }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return OwinContext.Authentication;
            }
        }

        #endregion

        #region Login Actions
        
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region External Login Actions
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    //ViewBag.ReturnUrl = returnUrl;
                    //ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    //return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
                    return RedirectToLocal(returnUrl);
            }
        }

        #endregion

        #region Helper Methods
        
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        } 

        #endregion

    }
}
