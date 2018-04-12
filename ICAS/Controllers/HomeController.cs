using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICAS.Areas.Admin.Manager;
using ICAS.ClientFilters;
using ICAS.Models.ClientPortalModel;
using ICASStacks.APIObjs;
using ICASStacks.DataContract.BioEnroll;
using ICASStacks.StackService;

namespace ICAS.Controllers
{
    public class HomeController : Controller
    {

        [EppSecurity_AuthorizeClient]
        public ActionResult Index()
        {
            
            return RedirectToAction("MyChurch");
        }


        [EppSecurity_AuthorizeClient]
        public ActionResult MyChurch()
        {

            //ViewBag.ProfileUpdatedSucceed = Session["ProfileUpdateSucceed"] as string;
            //ViewBag.ProfileUpdateError = Session["ProfileUpdateError"] as string;
            //Session["ProfileUpdateError"] = null;
            //Session["ProfileUpdateSucceed"] = null;

            ViewBag.ClientProfileUpdatedSucceed = Session["ClientProfileUpdatedSucceed"] as string;
            ViewBag.ClientProfileUpdatedError = Session["ClientProfileUpdatedError"] as string;
            Session["ClientProfileUpdatedSucceed"] = null;
            Session["ClientProfileUpdatedError"] = null;

            var clientData = MvcApplication.GetClientData(User.Identity.Name) ?? new ClientData();
            if (clientData.ClientId < 1)
            {
                return View(new ClientDashboardObj());
            }

            var dashboard = CustomManager.GetClientDashboardObjs(clientData.ClientId);
            return View(dashboard ?? new ClientDashboardObj());

        }




        public ActionResult MakePay()
        {

            if (Session["_NewMakePay"] == null)
            {

                var reqRef = CustomManager.GetUniqueId();
                var makePay = new ChurchPayeePayObj { RquestReference = reqRef.ToString(CultureInfo.InvariantCulture) };

                return View(makePay);
            }

            var model = Session["_NewMakePay"] as ChurchPayeePayObj;
            return View(model);
        }


        [HttpPost]
        public ActionResult MakePay(ChurchPayeePayObj model)
        {

            model.Amount = (model.Amount*100);
            Session["ProcessPaymentDetails"] = model;
            return RedirectToAction("ProcessPayment");

        }


        public ActionResult ProcessPayment()
        {

            try
            {
                var processPayDetail = Session["ProcessPaymentDetails"] as ChurchPayeePayObj;
                if (processPayDetail != null && processPayDetail.ChurchId > 0)
                {
                    return View(processPayDetail);
                }

                return null;
            }
            catch (Exception ex)
            {
                Session["CreateError"] = ex.Message;
                return Redirect(Url.RouteUrl(new { action = "TakeAttendance" }));
            }

        }

        
    }
}