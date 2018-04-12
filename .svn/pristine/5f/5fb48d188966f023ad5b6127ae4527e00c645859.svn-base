using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICAS.ClientFilters;
using ICAS.ClientPortal;
using ICAS.Models.ClientPortalModel;
using ICASStacks.DataContract.BioEnroll;
using ICASStacks.StackService;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS.Controllers.ChurchFramework
{
    public class ChurchPortalComController : Controller
    {


        public ActionResult MyChurchPortalInitializer()
        {

            string msg;
            var myAccess = ClientPortalInit.InitPortal(out msg);

            if (!myAccess)
            {
                ViewBag.ConfigMessage = "My Church Portal Initialization Failed! Detail: " +
                                        (msg.Length > 0 ? msg : "Please Check Configuration Settings");
                return View();
            }

            return RedirectToActionPermanent("Login", true);
        }



        public ActionResult Login(string returnUrl)
        {


            //var users = ServiceChurch.GetRemoteUserInfos() ?? new List<RemoteUserInfo>();
            //if (users.Any())
            //{
            //    return RedirectToAction("MyChurch");
            //}
            

            ViewBag.ClientProfileError = Session["InvalidClientProfileSessionError"] as string;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        [EppSecurity_AuthenticateClient]
        public ActionResult Login(string returnUrl, ClientLoginContract model)
        {
            try
            {
                var clientCode = ViewBag.ClientINFOCode as string;
                var firstLogin = ViewBag.FirstLogin;
                var clientData = ViewBag.ClientLoginDataItem as ClientData;
                if (string.IsNullOrEmpty(clientCode))
                {
                    return View(model);
                }
                if (clientData == null || clientData.ClientProfileId < 1)
                {
                    return View(model);
                }

                Session["ClientINFO"] = clientCode;
                Session["ClientDATAINFO"] = clientData.Username;
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

            //var clientData = ViewBag.ClientLoginDataItem as ClientData;
            //if (clientData == null)
            //{
            //    return RedirectToActionPermanent("Login", "ChurchPortalCom");
            //}
            return RedirectToActionPermanent("MyChurch", "Home");
        }



        #region Password

        [EppSecurity_AuthorizeClient]
        public ActionResult ChangeFirstTimePassword()
        {
            var model = new ChangeClientPasswordContract { UserName = User.Identity.Name };
            return View(model);
        }

        [HttpPost]
        [EppSecurity_ChangeClientAccess]
        public ActionResult ChangeFirstTimePassword(ChangeClientPasswordContract model)
        {
            var retVal = ViewBag.IsSuccessful;
            if (!retVal)
            {
                return View(model);
            }

            return RedirectToActionPermanent("MyChurch", "Home");
        }
        #endregion
	}
}