using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICAS.Areas.Admin.Filters;
using ICAS.Areas.Admin.Manager;
using ICASStacks.Repository.Helpers;

namespace ICAS.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {

        [EppSecurity_Authorize]
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }


        [EppSecurity_Authorize]
        public ActionResult Dashboard()
        {

            //Session["ProfileUpdateSucceed"] = "Profile updated successfully";
            //ViewBag.ProfileUpdateSucceed = "Profile updated successfully";
            //Session["ProfileUpdateError"] = "Invalid Profile Session";
            ViewBag.ProfileUpdatedSucceed = Session["ProfileUpdateSucceed"] as string;
            ViewBag.ProfileUpdateError = Session["ProfileUpdateError"] as string;
            Session["ProfileUpdateError"] = null;
            Session["ProfileUpdateSucceed"] = null;
            return View();
        }
	}
}