using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICAS.ClientPortal;

namespace ICAS.Controllers.ChurchFramework
{
    public class SignOutClientController : Controller
    {
        //
        // GET: /SignOut/
        public ActionResult Index(string retUrl)
        {
            //empty all persistent variables & sessions
            Session["ClientINFO"] = null;
            //new FormsAuthenticationService().SignOut();
            new FormsAuthenticationService().SignOut();
            return RedirectToAction("Login", "ChurchPortalCom", new { returnUrl = retUrl });
        }
	}
}