using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICAS.ClientFilters;
using ICAS.Models.ClientPortalModel;
using ICASStacks.APIObjs;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.DataContract.Enum;
using ICASStacks.StackService;
using WebCribs.TechCracker.WebCribs.TechCracker;
using WebCribs.TechCracker.WebCribs.TechCracker.EnumInfo;

namespace ICAS.Controllers.Administrative
{
    public class AdministrativeSettingController : Controller
    {


        #region Collection Types

        [EppSecurity_AuthorizeClient(Roles = "Pastor, ChurchAdmin")]
        public ActionResult MyChurchCollectionTypes()
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
                    return View(new List<CollectionTypeObj>());
                }

                var item = ServiceChurch.GetClientChurchCollectionTypeReportObj(clientData.ClientId) ?? new ClientChurchCollectionType();
                if (item.ClientChurchCollectionTypeId < 1)
                {
                    ViewBag.Error = "No Registered Collection Type Info Found!";
                    return View(new List<CollectionTypeObj>());
                }

                var collectionTypes = item.CollectionTypes;
                if (!collectionTypes.Any())
                {
                    ViewBag.Error = "No Registered Collection Type Info Found!";
                    return View(new List<CollectionTypeObj>());
                }

                Session["_clientChurchCollectionTypeInfo"] = item;
                return View(collectionTypes);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<CollectionTypeObj>());
            }

        }


        #region CRUD


        #region Modify Collection Type

        [EppSecurity_AuthorizeClient(Roles = "Pastor, ChurchAdmin")]
        public ActionResult ModifyCollectionType(string id)
        {
            try
            {
                ViewBag.Error = Session["EditError"] as string;
                Session["EditError"] = "";

                if (id.IsNullOrEmpty() || id.Length == 0)
                {
                    ViewBag.Error = "Invalid selection";
                    return RedirectToAction("MyChurchCollectionTypes");
                }

                if (Session["_clientChurchCollectionTypeInfo"] == null)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return RedirectToAction("MyChurchCollectionTypes");
                }

                var collectionTypeItem = Session["_clientChurchCollectionTypeInfo"] as ClientChurchCollectionType;
                if (collectionTypeItem == null || collectionTypeItem.ClientChurchCollectionTypeId < 1)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return RedirectToAction("MyChurchCollectionTypes");
                }

                var collectionTypes = collectionTypeItem.CollectionTypes;
                var collectionType = collectionTypes.Find(m => m.CollectionRefId == id);
                if (collectionType == null || collectionType.CollectionRefId.Length == 0)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return RedirectToAction("MyChurchCollectionTypes");
                }

                var helper = new CollectionTypeObj
                {
                    CollectionRefId = collectionType.CollectionRefId,
                    Name = collectionType.Name,
                    ChurchStructureTypeObjs = collectionType.ChurchStructureTypeObjs
                };

                Session["_selectedClientChurchCollectionType"] = collectionType;
                return PartialView(helper);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return RedirectToAction("MyChurchCollectionTypes"); ;
            }
        }
        
        [HttpPost]
        [EppSecurity_AuthorizeClient(Roles = "Pastor, ChurchAdmin")]
        public ActionResult ModifyCollectionType(CollectionTypeObj model)
        {
            try
            {
                if (model == null)
                {
                    Session["EditError"] = "Your session has expired! Please, re-login";
                    return Redirect(Url.RouteUrl(new { action = "ModifyCollectionType" }));
                }

                var selectedCollectionType =
                    Session["_selectedClientChurchCollectionType"] as CollectionTypeObj;

                if (selectedCollectionType == null || selectedCollectionType.CollectionRefId.Length == 0)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return RedirectToAction("MyChurchCollectionTypes");
                }

                var thisClientChurchCollectionType =
                    Session["_clientChurchCollectionTypeInfo"] as ClientChurchCollectionType;
                if (thisClientChurchCollectionType == null || thisClientChurchCollectionType.ClientChurchCollectionTypeId < 1)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "MyChurchCollectionTypes" }));
                }

                ModelState.Clear();
                if (!ModelState.IsValid)
                {
                    var errorLists = (from value in ViewData.ModelState.Values
                                      where value.Errors.Count > 0
                                      from error in value.Errors
                                      where !string.IsNullOrEmpty(error.ErrorMessage)
                                      select error.ErrorMessage).ToList();

                    Session["EditError"] = errorLists;
                    return Redirect(Url.RouteUrl(new { action = "ModifyCollectionType" }));
                }

                #region Updating Selected Client Church Collection with the model Data

                var collectionTypes = thisClientChurchCollectionType.CollectionTypes;
                collectionTypes.Where(x => x.CollectionRefId == model.CollectionRefId).ToList().ForEachx(c => c.Name = model.Name);

                #endregion


                var clientData = MvcApplication.GetClientData(User.Identity.Name) ?? new ClientData();
                var helper = new ClientChurchCollectionType
                {
                    ClientChurchCollectionTypeId = thisClientChurchCollectionType.ClientChurchCollectionTypeId,
                    ClientChurchId = thisClientChurchCollectionType.ClientChurchId,
                    CollectionTypes = collectionTypes,
                    TimeStampAdded = thisClientChurchCollectionType.TimeStampAdded,
                    AddedByUserId = thisClientChurchCollectionType.AddedByUserId
                };

                var retId = ServiceChurch.UpdateClientChurchCollectionType(helper);
                if (retId == null)
                {
                    Session["Error"] = "Unable to complete your request! Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "MyChurchCollectionTypes" }));
                }
                if (!retId.IsSuccessful)
                {
                    Session["Error"] = string.IsNullOrEmpty(retId.Message.FriendlyMessage) ? "Unable to update this service attendance" : retId.Message.FriendlyMessage;
                    return Redirect(Url.RouteUrl(new { action = "MyChurchCollectionTypes" }));
                }

                Session["_selectedClientChurchCollectionType"] = null;
                Session["Reply"] = "Collection type information was updated successfully";
                return Redirect(Url.RouteUrl(new { action = "MyChurchCollectionTypes" }));

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return RedirectToAction("MyChurchCollectionTypes"); ;
            }
        }


        #endregion


        #endregion

        #endregion




        #region Church Service

        [EppSecurity_AuthorizeClient(Roles = "Pastor, ChurchAdmin")]
        public ActionResult MyChurchServices()
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
                    return View(new List<ChurchServiceDetailObj>());
                }

                var item = ServiceChurch.GetClientChurchServiceTypeReportObj(clientData.ClientId) ?? new ClientChurchService();
                if (item.ClientChurchServiceId < 1)
                {
                    ViewBag.Error = "No Registered Church Service Type Info Found!";
                    return View(new List<ChurchServiceDetailObj>());
                }

                var serviceTypes = item.ServiceTypeDetail;
                if (!serviceTypes.Any())
                {
                    ViewBag.Error = "No Registered Church Service Type Info Found!";
                    return View(new List<ChurchServiceDetailObj>());
                }

                Session["_clientChurchServiceTypeInfo"] = item;
                return View(serviceTypes);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<ChurchServiceDetailObj>());
            }


        }


        #region CRUD



        [EppSecurity_AuthorizeClient(Roles = "Pastor, ChurchAdmin")]
        public ActionResult ModifyServiceType(string id)
        {
            try
            {
                ViewBag.Error = Session["EditError"] as string;
                Session["EditError"] = "";

                if (id.IsNullOrEmpty() || id.Length == 0)
                {
                    ViewBag.Error = "Invalid selection";
                    return RedirectToAction("MyChurchServices");
                }

                if (Session["_clientChurchServiceTypeInfo"] == null)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return RedirectToAction("MyChurchServices");
                }

                var serviceTypeItem = Session["_clientChurchServiceTypeInfo"] as ClientChurchService;
                if (serviceTypeItem == null || serviceTypeItem.ClientChurchServiceId < 1)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return RedirectToAction("MyChurchServices");
                }

                var serviceTypes = serviceTypeItem.ServiceTypeDetail;
                var serviceType = serviceTypes.Find(m => m.ChurchServiceTypeRefId == id);
                if (serviceType == null || serviceType.ChurchServiceTypeRefId.Length == 0)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return RedirectToAction("MyChurchServices");
                }

                #region Generating WeekDays Array
                var myDayOfWeekItemLists = EnumHelper.GetEnumList(typeof(WeekDays));
                var dayOfWeekObjs = new List<NameAndValueObject>();
                if (myDayOfWeekItemLists != null)
                {
                    dayOfWeekObjs = myDayOfWeekItemLists.Select(m => new NameAndValueObject
                    {
                        Name = m.Name,
                        Id = m.Id
                    }).ToList();
                }
                #endregion

                var helper = new ClientChurchServiceDetailObj
                {
                    ChurchServiceTypeRefId = serviceType.ChurchServiceTypeRefId,
                    Name = serviceType.Name,
                    DayOfWeekId = serviceType.DayOfWeekId,
                    DayOfWeek = serviceType.DayOfWeek,
                    WeekDays = dayOfWeekObjs,
                    MyWeekDayIds = new[] { serviceType.DayOfWeekId }
                };

                //var helper = new ChurchServiceDetailObj
                //{
                //    ChurchServiceTypeRefId = serviceType.ChurchServiceTypeRefId,
                //    Name = serviceType.Name
                //};

                Session["_selectedClientChurchServiceType"] = serviceType;
                return PartialView(helper);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return RedirectToAction("MyChurchServices"); ;
            }
        }


        [HttpPost]
        [EppSecurity_AuthorizeClient(Roles = "Pastor, ChurchAdmin")]
        public ActionResult ModifyServiceType(ClientChurchServiceDetailObj model)
        {

            try
            {
                if (model == null)
                {
                    Session["EditError"] = "Your session has expired! Please, re-login";
                    return Redirect(Url.RouteUrl(new { action = "ModifyServiceType" }));
                }

                var selectedServiceType =
                    Session["_selectedClientChurchServiceType"] as ChurchServiceDetailObj;

                if (selectedServiceType == null || selectedServiceType.ChurchServiceTypeRefId.Length == 0)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return RedirectToAction("MyChurchServices");
                }

                var thisClientChurchServiceType =
                    Session["_clientChurchServiceTypeInfo"] as ClientChurchService;
                if (thisClientChurchServiceType == null || thisClientChurchServiceType.ClientChurchServiceId < 1)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "MyChurchServices" }));
                }

                ModelState.Clear();
                if (!ModelState.IsValid)
                {
                    var errorLists = (from value in ViewData.ModelState.Values
                                      where value.Errors.Count > 0
                                      from error in value.Errors
                                      where !string.IsNullOrEmpty(error.ErrorMessage)
                                      select error.ErrorMessage).ToList();

                    Session["EditError"] = errorLists;
                    return Redirect(Url.RouteUrl(new { action = "ModifyServiceType" }));
                }

                #region Updating Selected Client Church Service with the model Data

                //users.ToList().ForEach(u =>
                //{
                //    u.property1 = value1;
                //    u.property2 = value2;
                //}); var selRoles = model.AllRoles.Where(m => model.MyRoleIds.Contains(m.Id)).Select(m => m.Name).ToList();

                #region Generating WeekDays Array

                model.WeekDays = new List<NameAndValueObject>();
                var myDayOfWeekItemLists = EnumHelper.GetEnumList(typeof(WeekDays));
                //var dayOfWeekObjs = new List<NameAndValueObject>();
                if (myDayOfWeekItemLists != null)
                {
                    model.WeekDays = myDayOfWeekItemLists.Select(m => new NameAndValueObject
                    {
                        Name = m.Name,
                        Id = m.Id
                    }).ToList();
                }
                #endregion

                var dayOfWeekId = model.MyWeekDayIds[0];
                var dayOfWeek = model.WeekDays.Where(w => model.MyWeekDayIds.Contains(w.Id)).Select(n => n.Name).ToList();
                var serviceTypes = thisClientChurchServiceType.ServiceTypeDetail;
                serviceTypes.Where(x => x.ChurchServiceTypeRefId == model.ChurchServiceTypeRefId).ToList().ForEachx(c =>
                {
                    c.Name = model.Name;
                    c.DayOfWeekId = dayOfWeekId;
                    c.DayOfWeek = dayOfWeek[0];
                });

                #endregion

                var helper = new ClientChurchService
                {
                    ClientChurchServiceId = thisClientChurchServiceType.ClientChurchServiceId,
                    ClientChurchId = thisClientChurchServiceType.ClientChurchId,
                    ServiceTypeDetail = serviceTypes,
                    TimeStampAdded = thisClientChurchServiceType.TimeStampAdded,
                    AddedByUserId = thisClientChurchServiceType.AddedByUserId
                };

                var retId = ServiceChurch.UpdateClientChurchServiceType(helper);
                if (retId == null)
                {
                    Session["Error"] = "Unable to complete your request! Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "MyChurchServices" }));
                }
                if (!retId.IsSuccessful)
                {
                    Session["Error"] = string.IsNullOrEmpty(retId.Message.FriendlyMessage) ? "Unable to update this service type" : retId.Message.FriendlyMessage;
                    return Redirect(Url.RouteUrl(new { action = "MyChurchServices" }));
                }

                Session["_selectedClientChurchServiceType"] = null;
                Session["Reply"] = "Church Service type information was updated successfully";
                return Redirect(Url.RouteUrl(new { action = "MyChurchServices" }));

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return RedirectToAction("MyChurchServices"); ;
            }

        }

        #endregion

        #endregion


    }
}