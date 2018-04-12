using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using ICAS.ClientFilters;
using ICAS.Models.ClientPortalModel;
using ICASStacks.APIObjs;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.StackService;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS.Controllers.Administrative
{

    [EppSecurity_AuthorizeClient(Roles = "Pastor,ChurchAdmin")]
    public class RemittanceController : Controller
    {


        [EppSecurity_AuthorizeClient(Roles = "Pastor,ChurchAdmin")]
        public ActionResult Index()
        {

            ViewBag.Reply = Session["Reply"] as string;
            ViewBag.Error = Session["Error"] as string;
            Session["Reply"] = "";
            Session["Error"] = "";

            try
            {
                //var items = ServiceChurch.GetChurchServiceAttendanceRemittances();
                var items = new List<ChurchServiceAttendanceRemittanceReportObj>();
                if (!items.Any())
                {
                    ViewBag.Error = "No Registered Church Service Attendance Remittance Info Found!";
                    return View(new List<ChurchServiceAttendanceRemittanceReportObj>());
                }

                Session["_churchServiceAttendanceRemittanceInfos"] = items;
                return View(items);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<ChurchServiceAttendanceRemittanceReportObj>());
            }
        }


        [EppSecurity_AuthorizeClient(Roles = "Pastor,ChurchAdmin")]
        public ActionResult RemittanceDetail(int remittanceType, string start, string end)
        {
            try
            {
                ViewBag.Reply = Session["Reply"] as string;
                Session["Reply"] = "";

                start = DateScrutnizer.ReverseToServerDate(start.Replace('-', '/'));
                end = DateScrutnizer.ReverseToServerDate(end.Replace('-', '/'));


                var clientData = MvcApplication.GetClientData(User.Identity.Name) ?? new ClientData();
                var clientChurchData = MvcApplication.GetClientChurchData(User.Identity.Name) ?? new ClientChurchData();


                //var check = ServiceChurch.GetClientChurchServiceAttendanceRemittance(clientChurchData.ChurchId, 2, start, end);
                //var check = ServiceChurch.GetClientChurchServiceAttendanceRemittance(2, 2, start, end);

                var check = ServiceChurch.GetClientChurchServiceAttendanceRemittance(clientChurchData.ChurchId, clientData.ClientId, start, end);
                if (check == null || !check.RemittanceDetailReport.ChurchServiceAttendanceRemittanceCollections.Any() ||
                    !check.RemittanceDetailReport.RemittanceChurchServiceDetails.Any() ||
                    !check.RemittanceDetailReport.CollectionRemittanceDetails.Any())
                {
                    return null;

                    // What am trying to achiecve here is return a differnt Partial View, If the Check (Remittance Objects)
                    // is Empty. Moreso, I think all the latest Attendance can be Calculated for Remittance in Index of
                    // this Controller and store in a Session. Then anytime you're selecting different range for Remittance
                    // Check against the one in Session but that's not possible because series of Date Range can be selected.

                    // But alternatives to this is to load all the Attendance, in the Index, then when calculating for Remittance
                    // Filter the Attendance list in the session with the selected Date range, but the issue is the list of 
                    // Attendance that falls within the selected Date range have to be pass along to the Backend for Calculation b4 
                    // Now return back to Controller again.

                }

                check.RemittanceType = remittanceType;
                return PartialView(check);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                Session["Error"] = ex.Message;
                return null;
            }
        }




        
        




        public ActionResult NationalRemittance()
        {
            var remittanceReport = Session["_remittanceReport"] as ChurchServiceAttendanceRemittanceReportObj;
            if (remittanceReport == null || !remittanceReport.RemittanceDetailReport.ChurchServiceAttendanceRemittanceCollections.Any() ||
                    !remittanceReport.RemittanceDetailReport.RemittanceChurchServiceDetails.Any() ||
                    !remittanceReport.RemittanceDetailReport.CollectionRemittanceDetails.Any())
            {
                // Redirect to a Partial View : That says No remittance Record for the seleted Date Range
            }

            return PartialView(remittanceReport);
        }


        [EppSecurity_AuthorizeClient(Roles = "Pastor,ChurchAdmin")]
        public ActionResult SetRemittanceCollectionPercent()
        {
            var clientData = MvcApplication.GetClientData(User.Identity.Name) ?? new ClientData();
            ViewBag.Errors = Session["SettingErrors"] as List<string>;
            ViewBag.Error = Session["SettingError"] as string;
            Session["SettingErrors"] = null;
            Session["SettingError"] = null;

            var clientChurchData = MvcApplication.GetClientChurchData(User.Identity.Name) ?? new ClientChurchData();
            var thisClientChurchId = clientChurchData.ChurchId;

            //var clientchurchCollectionTypes = ServiceChurch.GetClientChurchCollectionTypeForSetting(2, 3);
            var setClientChurchCollectionRemittancePercent = new ClientChurchCollectionTypeSettingObjs();

            //var churchCollectionTypes = ServiceChurch.GetChurchCollectionTypeForSetting(3);
            //var setChurchCollectionRemittancePercent = new ChurchCollectionTypeSettingObjs();

            var clientchurchCollectionTypes = ServiceChurch.GetClientChurchCollectionTypeForSettings(clientData.ClientId);

            if (Session["_NewSetChurchCollectionRemittancePercent"] == null)
            {
                //foreach (var clientchurchCollectionType in clientchurchCollectionTypes.ClientChurchCollectionStructureTypes)
                //{
                //    var clientChurchCollectionStructureType = new ClientChurchCollectionTypeObjs
                //    {
                //        CollectionTypeId = clientchurchCollectionType.CollectionTypeId,
                //        Name = clientchurchCollectionType.Name,
                //        ChurchStructureTypeObjs = clientchurchCollectionType.ChurchStructureTypeObjs
                //    };
                //    setClientChurchCollectionRemittancePercent.ClientChurchCollectionStructureTypes.Add(clientChurchCollectionStructureType);
                //}

                //clientchurchCollectionTypes.ClientId = 2;
                return View(clientchurchCollectionTypes);

                //churchCollectionTypes.ChurchId = 3;
                //setChurchCollectionRemittancePercent.ChurchId = 3;
                //setChurchCollectionRemittancePercent.ClientId = 2;
                
            }

            var model = Session["_NewSetChurchCollectionRemittancePercent"] as ClientChurchCollectionTypeSettingObjs;
            return View(model);
        }

        [HttpPost]
        [EppSecurity_AuthorizeClient(Roles = "Pastor,ChurchAdmin")]
        public ActionResult SetRemittanceCollectionPercent(ClientChurchCollectionTypeSettingObjs model)
        {
            try
            {
                Session["_NewSetChurchCollectionRemittancePercent"] = model;
                if (model == null)
                {
                    Session["SettingError"] = "Your session has expired! Please, re-login";
                    return Redirect(Url.RouteUrl(new { action = "SetRemittanceCollectionPercent" }));
                }

                if (!ModelState.IsValid)
                {
                    Session["_NewSetChurchCollectionRemittancePercent"] = model;
                    var errorLists = (from value in ViewData.ModelState.Values
                        where value.Errors.Count > 0
                        from error in value.Errors
                        where !string.IsNullOrEmpty(error.ErrorMessage)
                        select error.ErrorMessage).ToList();

                    Session["SettingErrors"] = errorLists;
                    return Redirect(Url.RouteUrl(new { action = "SetRemittanceCollectionPercent" }));
                }


                var retId = ServiceChurch.SetClientChurchCollectionTypeForRemittance(model);
                if (retId == null)
                {
                    Session["SettingError"] = "Unable to complete your request! Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "SetRemittanceCollectionPercent" }));
                }
                if (!retId.IsSuccessful)
                {
                    Session["SettingError"] = string.IsNullOrEmpty(retId.Message.FriendlyMessage) ? "Unable to update remittance collection types setting" : retId.Message.FriendlyMessage;
                    return Redirect(Url.RouteUrl(new { action = "SetRemittanceCollectionPercent" }));
                }

                Session["_NewSetChurchCollectionRemittancePercent"] = null;
                Session["Reply"] = "Remittance Collection Types Setting updated successfully";
                return Redirect(Url.RouteUrl(new { action = "Index" }));

            }
            catch (Exception ex)
            {
                Session["SettingError"] = ex.Message;
                return Redirect(Url.RouteUrl(new { action = "SetRemittanceCollectionPercent" }));
            }
        }


        public ActionResult CompareRemittance()
        {
            ViewBag.Reply = Session["Reply"] as string;
            ViewBag.Error = Session["Error"] as string;
            Session["Reply"] = "";
            Session["Error"] = "";

            return View();
        }
    }

}