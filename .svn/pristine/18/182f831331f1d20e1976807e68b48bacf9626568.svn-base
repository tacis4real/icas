using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ICAS.ClientFilters;
using ICAS.Models.ClientPortalModel;
using ICASStacks.APIObjs;
using ICASStacks.Repository.Helpers;
using ICASStacks.StackService;
using WebAdminStacks.APIObjs;
using WebAdminStacks.APIs;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS.Controllers.Administrative
{

    [EppSecurity_AuthorizeClient(Roles = "Pastor,ChurchAdmin")]
    public class ChurchServiceAttendanceController : Controller
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
                var clientData = MvcApplication.GetClientData(User.Identity.Name) ?? new ClientData();
                if (clientData.ClientId < 1)
                {
                    ViewBag.Error = "Authentication failed! You don't have the administrative right!";
                    return View(new List<RegisteredChurchServiceAttendanceReportObj>());
                }

                var items = ServiceChurch.GetAllRegisteredChurchServiceAttendanceObjs(clientData.ClientId) ?? new List<RegisteredChurchServiceAttendanceReportObj>();
                //var items = ServiceChurch.GetAllRegisteredChurchServiceAttendanceObjs(2) ?? new List<RegisteredChurchServiceAttendanceReportObj>();
                if (!items.Any())
                {
                    ViewBag.Error = "No Registered Church Service Attendance Info Found!";
                    return View(new List<RegisteredChurchServiceAttendanceReportObj>());
                }

                var churchServiceAttendances = items;
                Session["_churchServiceAttendanceInfos"] = churchServiceAttendances;
                return View(churchServiceAttendances);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<RegisteredChurchServiceAttendanceReportObj>());
            }
        }
        

        #region CRUD

        #region Take Attendance

        //[Route("Attendance/Fill")]
        [EppSecurity_AuthorizeClient(Roles = "Pastor,ChurchAdmin")]
        public ActionResult TakeAttendance()
        {
            var clientData = MvcApplication.GetClientData(User.Identity.Name) ?? new ClientData();
            ViewBag.Errors = Session["CreateErrors"] as List<string>;
            ViewBag.Error = Session["CreateError"] as string;
            Session["CreateErrors"] = "";
            Session["CreateError"] = "";

            var clientChurchData = MvcApplication.GetClientChurchData(User.Identity.Name) ?? new ClientChurchData();
            var thisClientChurchId = clientChurchData.ChurchId;


            //var churchServiceAttendanceCollectionTypes = ServiceChurch.GetChurchCollectionTypesByChurchId(3);
            var churchServiceAttendanceCollectionTypes = ServiceChurch.GetChurchCollectionTypesByChurchId(thisClientChurchId);
            var clientchurchCollectionTypes = ServiceChurch.GetClientChurchCollectionTypesByClientChurchId(clientData.ClientId);

            if (Session["_NewTakeChurchServiceAttendance"] == null)
            {
                var takeChurchServiceAttendance = new ChurchServiceAttendanceRegObj
                {
                    ClientChurchId = clientData.ClientId,
                    ChurchServiceAttendanceDetail = new ClientChurchServiceAttendanceDetailObj
                    {
                        ClientChurchServiceAttendanceCollections = new List<ClientChurchServiceAttendanceCollectionObj>(clientchurchCollectionTypes ?? new List<ClientChurchServiceAttendanceCollectionObj>())
                    }
                };
                return View(takeChurchServiceAttendance);
            }

            var model = Session["_NewTakeChurchServiceAttendance"] as ChurchServiceAttendanceRegObj;
            return View(model);

        }

        [HttpPost]
        //[Route("Attendance/Fill")]
        [EppSecurity_AuthorizeClient(Roles = "Pastor,ChurchAdmin")]
        public ActionResult TakeAttendance(ChurchServiceAttendanceRegObj model)
        {
            var errorLists = new List<string>();
            try
            {
                Session["_NewTakeChurchServiceAttendance"] = model;
                var clientData = MvcApplication.GetClientData(User.Identity.Name) ?? new ClientData();
                if (clientData.ClientId < 1)
                {
                    ViewBag.Error = "Authentication failed! You don't have the administrative right!";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }

                if (!ModelState.IsValid)
                {
                    Session["_NewTakeChurchServiceAttendance"] = model;

                    errorLists = (from value in ViewData.ModelState.Values
                                  where value.Errors.Count > 0
                                  from error in value.Errors
                                  where !string.IsNullOrEmpty(error.ErrorMessage)
                                  select error.ErrorMessage).ToList();

                    Session["CreateErrors"] = errorLists;
                    return Redirect(Url.RouteUrl(new { action = "TakeAttendance" }));
                }

                var helper = new ChurchServiceAttendanceRegObj
                {
                    ChurchServiceTypeRefId = model.ChurchServiceTypeRefId,
                    ClientChurchId = model.ClientChurchId,
                    Preacher = model.Preacher,
                    ServiceTheme = model.ServiceTheme,
                    BibleReadingText = model.BibleReadingText,

                    ChurchServiceAttendanceDetail = new ClientChurchServiceAttendanceDetailObj
                    {
                        NumberOfMen  = model.ChurchServiceAttendanceDetail.NumberOfMen,
                        NumberOfWomen = model.ChurchServiceAttendanceDetail.NumberOfWomen,
                        NumberOfChildren = model.ChurchServiceAttendanceDetail.NumberOfChildren,
                        FirstTimer = model.ChurchServiceAttendanceDetail.FirstTimer,
                        NewConvert = model.ChurchServiceAttendanceDetail.NewConvert,

                        ClientChurchServiceAttendanceCollections = model.ChurchServiceAttendanceDetail.ClientChurchServiceAttendanceCollections

                        //ChurchServiceAttendanceCollections = model.ChurchServiceAttendanceDetail.ChurchServiceAttendanceCollections
                    },
                    
                    TakenByUserId = Convert.ToInt32(clientData.ClientId),
                    DateServiceHeld = model.DateServiceHeld
                };

                var retId = ServiceChurch.AddChurchServiceAttendance(helper);
                if (retId == null)
                {
                    Session["CreateError"] = "Unable to add church service attendance info. Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "TakeAttendance" }));
                }
                if (!retId.Status.IsSuccessful)
                {
                    Session["CreateError"] = string.IsNullOrEmpty(retId.Status.Message.FriendlyMessage) ? "Unable to add church service attendance info! Please try again later" : retId.Status.Message.FriendlyMessage;
                    return Redirect(Url.RouteUrl(new { action = "TakeAttendance" }));
                }

                Session["_NewTakeChurchServiceAttendance"] = null;
                Session["Reply"] = "Church Service Attendance Information was added successfully";
                return Redirect(Url.RouteUrl(new { action = "Index" }));


            }
            catch (Exception ex)
            {
                Session["CreateError"] = ex.Message;
                return Redirect(Url.RouteUrl(new { action = "TakeAttendance" }));
            }
        }
        #endregion

        #region Modify Attendance

        [EppSecurity_AuthorizeClient(Roles = "Pastor, ChurchAdmin")]
        public ActionResult ModifyAttendance(string id)
        {
            try
            {

                long attendanceId;
                Int64.TryParse(id, out attendanceId);

                ViewBag.Error = Session["EditError"] as string;
                Session["EditError"] = "";

                if (attendanceId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return RedirectToAction("Index");
                }
                if (Session["_churchServiceAttendanceInfos"] == null)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return RedirectToAction("Index");
                }
                var churchServiceAttendances = Session["_churchServiceAttendanceInfos"] as List<RegisteredChurchServiceAttendanceReportObj>;
                if (churchServiceAttendances == null || !churchServiceAttendances.Any())
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return RedirectToAction("Index");
                }

                var churchServiceAttendance = churchServiceAttendances.Find(m => m.ChurchServiceAttendanceId == attendanceId);
                if (churchServiceAttendance == null || churchServiceAttendance.ChurchServiceAttendanceId < 1)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return RedirectToAction("Index");
                }

                var helper = new ChurchServiceAttendanceRegObj
                {
                    ChurchServiceAttendanceId = churchServiceAttendance.ChurchServiceAttendanceId,
                    ChurchServiceTypeRefId = churchServiceAttendance.ChurchServiceTypeRefId,
                    ClientChurchId = churchServiceAttendance.ClientChurchId,
                    Preacher = churchServiceAttendance.Preacher,
                    ServiceTheme = churchServiceAttendance.ServiceTheme,
                    BibleReadingText = churchServiceAttendance.BibleReadingText,
                    ChurchServiceAttendanceDetail = churchServiceAttendance.ServiceAttendanceDetail,
                    
                    #region Oldiessss

                    //ChurchServiceAttendanceDetail = new ChurchServiceAttendanceDetailObj
                    //{
                    //    NumberOfMen = churchServiceAttendance.ServiceAttendanceDetail.NumberOfMen,
                    //    NumberOfWomen = churchServiceAttendance.ServiceAttendanceDetail.NumberOfWomen,
                    //    NumberOfChildren = churchServiceAttendance.ServiceAttendanceDetail.NumberOfChildren,
                    //    FirstTimer = churchServiceAttendance.ServiceAttendanceDetail.FirstTimer,
                    //    NewConvert = churchServiceAttendance.ServiceAttendanceDetail.NewConvert,
                    //    ChurchServiceAttendanceCollections = new List<ChurchServiceAttendanceCollectionObj>
                    //    {
                    //        new ChurchServiceAttendanceCollectionObj
                    //        {
                    //            CollectionTypeId = churchServiceAttendance.ServiceAttendanceDetail.ChurchServiceAttendanceCollections
                    //        }
                    //    }
                    //},


                    //NumberOfMen = churchServiceAttendance.ChurchServiceAttendanceAttendee.NumberOfMen,
                    //NumberOfWomen = churchServiceAttendance.ChurchServiceAttendanceAttendee.NumberOfWomen,
                    //NumberOfChildren = churchServiceAttendance.ChurchServiceAttendanceAttendee.NumberOfChildren,
                    //FirstTimer = churchServiceAttendance.ChurchServiceAttendanceAttendee.FirstTimer,
                    //NewConvert = churchServiceAttendance.ChurchServiceAttendanceAttendee.NewConvert,

                    //Offerring = churchServiceAttendance.ClientChurchCollection.Offerring,
                    //Tithe = churchServiceAttendance.ClientChurchCollection.Tithe,
                    //ThanksGiving = churchServiceAttendance.ClientChurchCollection.ThanksGiving,
                    //BuildingProjectFund = churchServiceAttendance.ClientChurchCollection.BuildingProjectFund,
                    //SpecialThanksGiving = churchServiceAttendance.ClientChurchCollection.SpecialThanksGiving,
                    //Donation = churchServiceAttendance.ClientChurchCollection.Donation,
                    //FirstFruit = churchServiceAttendance.ClientChurchCollection.FirstFruit,
                    //WelfareCharity = churchServiceAttendance.ClientChurchCollection.WelfareCharity,
                    //Others = churchServiceAttendance.ClientChurchCollection.Others,
                    #endregion

                    DateServiceHeld = churchServiceAttendance.DateServiceHeld
                };

                Session["_selectedChurchServiceAttendance"] = churchServiceAttendance;
                return View(helper);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            } 
        }

        [HttpPost]
        [EppSecurity_AuthorizeClient(Roles = "Pastor,ChurchAdmin")]
        public ActionResult ModifyAttendance(string id, ChurchServiceAttendanceRegObj model)
        {
            try
            {
                long attendanceId;
                Int64.TryParse(id, out attendanceId);

                if (attendanceId < 1)
                {
                    Session["EditError"] = "Invalid selection";
                    return Redirect(Url.RouteUrl(new { action = "ModifyAttendance" }));
                }
                if (model == null)
                {
                    Session["EditError"] = "Your session has expired! Please, re-login";
                    return Redirect(Url.RouteUrl(new { action = "ModifyAttendance" }));
                }

                var thisChurchServiceAttendance = Session["_selectedChurchServiceAttendance"] as RegisteredChurchServiceAttendanceReportObj;
                if (thisChurchServiceAttendance == null || thisChurchServiceAttendance.ChurchServiceAttendanceId < 1)
                {
                    Session["EditError"] = "Your session has expired! Please, re-login";
                    return Redirect(Url.RouteUrl(new { action = "ModifyAttendance" }));
                }

                ModelState.Clear();
                if (!ModelState.IsValid)
                {
                    var errorLists = (from value in ViewData.ModelState.Values
                                      where value.Errors.Count > 0
                                      from error in value.Errors
                                      where !string.IsNullOrEmpty(error.ErrorMessage)
                                      select error.ErrorMessage).ToList();

                    Session["EditErrors"] = errorLists;
                    return Redirect(Url.RouteUrl(new { action = "Index" }) + "#edit&" + id);
                }

                var clientData = MvcApplication.GetClientData(User.Identity.Name) ?? new ClientData();
                model.TakenByUserId = Convert.ToInt32(clientData.ClientProfileId);

                var helper = new ChurchServiceAttendanceRegObj
                {
                    ChurchServiceAttendanceId = thisChurchServiceAttendance.ChurchServiceAttendanceId,
                    ChurchServiceTypeRefId = model.ChurchServiceTypeRefId,
                    ClientChurchId = model.ClientChurchId,
                    Preacher = model.Preacher,
                    ServiceTheme = model.ServiceTheme,
                    BibleReadingText = model.BibleReadingText,

                    ChurchServiceAttendanceDetail = model.ChurchServiceAttendanceDetail,
                    DateServiceHeld = model.DateServiceHeld,
                };

                // Checks whether the date has been changed
                var check = DateScrutnizer.ReverseToServerDate(model.DateServiceHeld);
                if (!check.IsNullOrEmpty())
                {
                    helper.DateServiceHeld = check;
                }

                var retId = ServiceChurch.UpdateChurchServiceAttendance(helper);
                if (retId == null)
                {
                    Session["EditError"] = "Unable to complete your request! Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }
                if (!retId.IsSuccessful)
                {
                    Session["EditError"] = string.IsNullOrEmpty(retId.Message.FriendlyMessage) ? "Unable to update this service attendance" : retId.Message.FriendlyMessage;
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }

                Session["_selectedChurchServiceAttendance"] = null;
                Session["Reply"] = "Church service attendance information was updated successfully";
                return Redirect(Url.RouteUrl(new { action = "Index" }));


            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
        #endregion
        
        #region Attendance Detail Info

        [EppSecurity_AuthorizeClient(Roles = "Pastor,ChurchAdmin")]
        public ActionResult AttendanceInfo(string churchServiceAttendanceId)
        {
            try
            {

                long attendanceId;
                Int64.TryParse(churchServiceAttendanceId, out attendanceId);

                ViewBag.Reply = Session["Reply"] as string;
                Session["Reply"] = "";

                if (attendanceId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return RedirectToAction("Index");
                }
                if (Session["_churchServiceAttendanceInfos"] == null)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return RedirectToAction("Index");
                }

                var churchServiceAttendances = Session["_churchServiceAttendanceInfos"] as List<RegisteredChurchServiceAttendanceReportObj>;
                if (churchServiceAttendances == null || !churchServiceAttendances.Any())
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return RedirectToAction("Index");
                }

                var churchServiceAttendance = churchServiceAttendances.Find(m => m.ChurchServiceAttendanceId == attendanceId);
                if (churchServiceAttendance == null || churchServiceAttendance.ChurchServiceAttendanceId < 1)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return RedirectToAction("Index");
                }

                return PartialView(churchServiceAttendance);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                Session["Error"] = ex.Message;
                return null;
            }
        }

        #endregion


       

        #endregion
    }
}