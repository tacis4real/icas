using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICAS.Areas.Admin.Filters;
using ICAS.Areas.Admin.Models.PortalModel;
using ICAS.Areas.Admin.Portal;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS.Areas.Admin.Controllers.Framework
{
    public class PortalComController : Controller
    {

        [AllowAnonymous]
        public ActionResult InitializePortal()
        {
            string msg;
            var myAccess = PortalInit.InitPortal(out msg);
            if (!myAccess)
            {
                ViewBag.ConfigMessage = "Portal Initialization Failed! Detail: " +
                                        (msg.Length > 0 ? msg : "Please Check Configuration Settings");
                return View();
            }

            return RedirectToActionPermanent("Login", true);
        }


        public ActionResult Login(string returnUrl)
        {
            ViewBag.UserProfileError = Session["InvalidProfileSessionError"] as string;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [EppSecurity_Authenticate]
        public ActionResult Login(string returnUrl, UserLoginContract model)
        {
            try
            {
                var userCode = ViewBag.UserINFOCode as string;
                var firstLogin = ViewBag.FirstLogin;
                var userData = ViewBag.LoginDataItem as UserData;
                if (string.IsNullOrEmpty(userCode))
                {
                    return View(model);
                }
                if (userData == null || userData.UserId < 1)
                {
                    return View(model);
                }

                Session["UserINFO"] = userCode;
                Session["UserDATAINFO"] = userData.Username;
                if (firstLogin)
                {
                    ViewBag.MyUserName = model.UserName.Trim();
                    return RedirectToAction("ChangeFirstTimePassword");
                }

                return RedirectToLocal(returnUrl);

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }



        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return RedirectPermanent(returnUrl);
            }
            return RedirectToActionPermanent("Dashboard", "Home");
        }



        #region Password

        [EppSecurity_Authorize]
        public ActionResult ChangeFirstTimePassword()
        {
            var model = new ChangePasswordContract { UserName = User.Identity.Name };
            return View(model);
        }


        [HttpPost]
        [EppSecurity_FirstAccess]
        public ActionResult ChangeFirstTimePassword(ChangePasswordContract model)
        {
            var retVal = ViewBag.IsSuccessful;
            if (!retVal)
            {
                return View(model);
            }

            return RedirectToActionPermanent("Dashboard", "Home");
        }
        #endregion
	}
}