using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICAS.Areas.Admin.Filters;
using ICAS.Areas.Admin.Manager;
using ICAS.Areas.Admin.Models.PortalModel;
using ICAS.Models.ClientPortalModel;
using ICASStacks.APIObjs;
using ICASStacks.DataContract;
using ICASStacks.StackService;
using WebAdminStacks.APIObjs;
using WebAdminStacks.APIs;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS.Areas.Admin.Controllers.Settings
{
    public class ClientChurchController : Controller
    {


        [EppSecurity_Authorize]
        public ActionResult Index()
        {

            ViewBag.Reply = Session["Reply"] as string;
            ViewBag.Error = Session["Error"] as string;
            Session["Reply"] = "";
            Session["Error"] = "";

            Session["_NewClientChurch"] = null;

            try
            {
                var clientData = MvcApplication.GetClientData(User.Identity.Name) ?? new ClientData();
                //if (clientData.ClientId < 1)
                //{
                //    ViewBag.Error = "Authentication failed! You don't have the administrative right!";
                //    return View(new List<RegisteredChurchServiceAttendanceReportObj>());
                //}


                var items = ServiceChurch.GetAllRegisteredClientChurchObjs() ?? new List<RegisteredClientChurchReportObj>();
                //var items = new List<RegisteredClientChurchReportObj>();
                if (!items.Any())
                {
                    ViewBag.Error = "No Registered Client Church Info Found!";
                    return View(new List<RegisteredClientChurchReportObj>());
                }

                var clientChurchInfos = items;
                Session["_clientChurchInfos"] = clientChurchInfos;
                return View(items);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<RegisteredClientChurchReportObj>());
            }
        }

        #region CRUD

        #region CREATE

        [EppSecurity_Authorize]
        public ActionResult NewClientChurch()
        {

            //var itemTicks = CustomManager.GetUniqueTicks();


            ViewBag.Errors = Session["CreateErrors"] as List<string>;
            ViewBag.Error = Session["CreateError"] as string;
            Session["CreateErrors"] = "";
            Session["CreateError"] = "";

            try
            {

                if (Session["_NewClientChurch"] == null)
                {
                    var clientchurch = new ClientChurchRegistrationObj
                    {
                        ClientChurchProfile = new ClientProfileRegistrationObj
                        {
                            MyRoleIds = new[] { 0 },
                            AllRoles = new List<NameAndValueObject>()
                        },
                        ClientChurchAccount = new ClientChurchAccount(),
                        Action = "NewClientChurch"
                    };
                    var roles = PortalClientRole.GetRolesClient();
                    if (roles == null || !roles.Any())
                    {
                        clientchurch.ClientChurchProfile = new ClientProfileRegistrationObj();
                        ViewBag.Error = "Invalid Client Profile Roles";
                        //return View(clientchurch);
                        return View("NewClientChurch", clientchurch);
                    }

                    foreach (var item in roles)
                    {
                        //if (item.Name == "*" || item.Name == "AgentUser") { continue; }
                        //if (!User.IsInRole("PortalAdmin"))
                        //{
                        //    if (item.Name == "PortalAdmin" || item.Name == "SiteAdmin") { continue; }
                        //}
                        clientchurch.ClientChurchProfile.AllRoles.Add(new NameAndValueObject { Id = item.RoleClientId, Name = item.Name });
                    }

                    Session["_NewClientChurch"] = clientchurch;
                    return View("NewClientChurch", clientchurch);
                }

                
                var model = Session["_NewClientChurch"] as ClientChurchRegistrationObj;
                if (model != null)
                {
                    model.Action = "NewClientChurch";
                }
                return View("NewClientChurch", model);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return View(new ClientChurchRegistrationObj());
            }

        }

        [HttpPost]
        [EppSecurity_Authorize]
        public ActionResult NewClientChurch(ClientChurchRegistrationObj model)
        {
            var errorLists = new List<string>();
            try
            {
                var roles = PortalClientRole.GetRolesClient();
                if (roles == null || !roles.Any())
                {
                    Session["_NewClientChurch"] = model;

                    ViewBag.Error = "Invalid Client Profile Roles";
                    return View("NewClientChurch", new ClientChurchRegistrationObj());
                    //return View(new ClientChurchRegistrationObj());
                }

                model.ClientChurchProfile.AllRoles = new List<NameAndValueObject>();
                foreach (var item in roles)
                {
                    //if (item.Name == "*" || item.Name == "AgentUser") { continue; }
                    //if (!User.IsInRole("PortalAdmin"))
                    //{
                    //    if (item.Name == "PortalAdmin" || item.Name == "SiteAdmin") { continue; }
                    //}
                    model.ClientChurchProfile.AllRoles.Add(new NameAndValueObject { Id = item.RoleClientId, Name = item.Name });
                }

                if (!RegExValidator.IsGSMNumberValid(model.PhoneNumber))
                {
                    model.ClientChurchProfile.Password = "";
                    model.ClientChurchProfile.ConfirmPassword = "";
                    Session["_NewClientChurch"] = model;

                    errorLists.Add("Invalid mobile number");
                    Session["CreateErrors"] = errorLists;
                    //return View("NewClientChurch", model);
                    return Redirect(Url.RouteUrl(new { action = "NewClientChurch" }));
                }

                if (!ModelState.IsValid)
                {
                    model.ClientChurchProfile.Password = "";
                    model.ClientChurchProfile.ConfirmPassword = "";
                    Session["_NewClientChurch"] = model;

                    errorLists = (from value in ViewData.ModelState.Values
                                  where value.Errors.Count > 0
                                  from error in value.Errors
                                  where !string.IsNullOrEmpty(error.ErrorMessage)
                                  select error.ErrorMessage).ToList();

                    Session["CreateErrors"] = errorLists;
                    return Redirect(Url.RouteUrl(new { action = "NewClientChurch" }));
                }

                if (model.MyRoleIds == null || !model.MyRoleIds.Any())
                {
                    model.ClientChurchProfile.Password = "";
                    model.ClientChurchProfile.ConfirmPassword = "";
                    Session["_NewClientChurch"] = model;
                    Session["CreateError"] = "You must select at least one role for this client church";
                    return Redirect(Url.RouteUrl(new { action = "NewClientChurch" }));
                }

                var selRoles = model.ClientChurchProfile.AllRoles.Where(m => model.MyRoleIds.Contains(m.Id)).Select(m => m.Name).ToList();
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                var bankName = ServiceChurch.GetBank(model.BankId).Name;

                var helper = new ClientChurchRegistrationObj
                {
                    ChurchId = model.ChurchId,
                    Name = model.Name,
                    Pastor = model.Pastor,
                    Title = model.Title,
                    Sex = model.Sex,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    StateOfLocationId = model.StateOfLocationId,
                    Address = model.Address,
                    ChurchStructureParishHeadQuarters = model.ChurchStructureParishHeadQuarters,

                    ClientChurchHeadQuarterChurchStructureTypeId = model.ClientChurchHeadQuarterChurchStructureTypeId,
                    RegisteredByUserId = Convert.ToInt32(userData.UserId),
                    //RegisteredByUserId = 1,
                    ClientChurchAccount = new ClientChurchAccount
                    {
                        BankId = model.BankId,
                        AccountTypeId = model.AccountTypeId,
                        AccountName = model.ClientChurchAccount.AccountName,
                        AccountNumber = model.ClientChurchAccount.AccountNumber,
                        BankName = bankName
                    },
                    ClientChurchProfile = new ClientProfileRegistrationObj
                    {
                        Fullname = model.Pastor,
                        MobileNumber = model.PhoneNumber,
                        Sex = model.Sex,
                        Email = model.Email,
                        Username = model.ClientChurchProfile.Username,
                        Password = model.ClientChurchProfile.Password,
                        ConfirmPassword = model.ClientChurchProfile.ConfirmPassword,
                        MyRoles = selRoles.ToArray(),
                        MyRoleIds = model.MyRoleIds,
                        SelectedRoles = string.Join(";", selRoles),
                        //RegisteredByUserId = 1,
                        RegisteredByUserId = Convert.ToInt32(userData.UserId)
                    },
                    MyRoleIds = model.MyRoleIds,
                    
                };


                var retId = ServiceChurch.AddClientChurch(helper);
                if (retId == null)
                {

                    model.ClientChurchProfile.Password = "";
                    model.ClientChurchProfile.ConfirmPassword = "";
                    Session["_NewClientChurch"] = model;

                    Session["CreateError"] = "Unable to set up church. Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "NewClientChurch" }));
                }
                if (!retId.Status.IsSuccessful)
                {

                    model.ClientChurchProfile.Password = "";
                    model.ClientChurchProfile.ConfirmPassword = "";
                    Session["_NewClientChurch"] = model;

                    Session["CreateError"] = string.IsNullOrEmpty(retId.Status.Message.FriendlyMessage) ? "Unable to set up church! Please try again later" : retId.Status.Message.FriendlyMessage;
                    return Redirect(Url.RouteUrl(new { action = "NewClientChurch" }));
                }

                Session["_NewClientChurch"] = null;
                Session["Reply"] = "Church Information was added successfully";
                return Redirect(Url.RouteUrl(new { action = "Index" }));

            }
            catch (Exception ex)
            {
                Session["CreateError"] = ex.Message;
                return Redirect(Url.RouteUrl(new { action = "NewClientChurch" }));
                //return View("NewClientChurch", new ClientChurchRegistrationObj());
                //return View();
            }
        }

        #endregion


        #region EDIT

        public ActionResult ModifyClientChurch(string id)
        {
            
            ViewBag.Errors = Session["EditErrors"] as List<string>;
            ViewBag.Error = Session["EditError"] as string;
            Session["EditErrors"] = null;
            Session["EditError"] = null;

            try
            {

                long clientChurchId;
                Int64.TryParse(id, out clientChurchId);

                if (clientChurchId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                    //return null;
                }
                if (Session["_clientChurchInfos"] == null)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                    //return null;
                }
                var clientChurchs = Session["_clientChurchInfos"] as List<RegisteredClientChurchReportObj>;
                if (clientChurchs == null || !clientChurchs.Any())
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                    //return null;
                }
                var clientChurch = clientChurchs.Find(m => m.ClientChurchId == clientChurchId);
                if (clientChurch == null || clientChurch.ClientChurchId < 1)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                    //return null;
                }


                var helper = new ClientChurchRegistrationObj
                {
                    ClientChurchId = clientChurch.ClientChurchId,
                    ChurchId = clientChurch.ChurchId,
                    Name = clientChurch.Name,
                    Pastor = clientChurch.Pastor,
                    Title = clientChurch.Title,
                    Sex = clientChurch.Sex,
                    PhoneNumber = CustomManager.ReCleanMobile(clientChurch.PhoneNumber),
                    Email = clientChurch.Email,
                    Address = clientChurch.Address,
                    StateOfLocationId = clientChurch.StateOfLocationId,
                    ChurchReferenceNumber = clientChurch.ChurchReferenceNumber,
                    
                    BankId = clientChurch.AccountInfo.BankId,
                    AccountTypeId = clientChurch.AccountInfo.AccountTypeId,
                    ClientChurchHeadQuarterChurchStructureTypeId = clientChurch.ClientChurchHeadQuarterChurchStructureTypeId,

                    ChurchStructureParishHeadQuarters = clientChurch.ChurchStructureParishHeadQuarters,
                    ClientChurchAccount = clientChurch.AccountInfo,
                    Parishes = clientChurch.Parishes,
                    ClientChurchProfile = clientChurch.ClientChurchProfile,

                    Action = "ModifyClientChurch"

                    //HeadQuarterChurchStructureTypeId = client.HeadQuarterChurchStructureTypeId,

                    //RegionId = client.Region.RegionId,
                    //ProvinceId = client.Province.ProvinceId,
                    //ZoneId = client.Zone.ZoneId,
                    //AreaId = client.Area.AreaId,
                    //DioceseId = client.Diocese.DioceseId,
                    //DistrictId = client.District.DistrictId,
                    //StateId = client.State.StateId,
                    //GroupId = client.Group.GroupId,

                    //Username = client.Username,
                };


                var roles = PortalClientRole.GetRolesClient();
                if (roles == null || !roles.Any())
                {
                    ViewBag.Error = "Invalid Client Profile Roles";
                    return View("NewClientChurch", helper);
                }

                helper.ClientChurchProfile.AllRoles = new List<NameAndValueObject>();
                foreach (var item in roles)
                {
                    helper.ClientChurchProfile.AllRoles.Add(new NameAndValueObject { Id = item.RoleClientId, Name = item.Name });
                }

                Session["_selectedClientChurch"] = clientChurch;
                return View("NewClientChurch", helper);

                //return null;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }


        [HttpPost]
        public ActionResult ModifyClientChurch(string id, ClientChurchRegistrationObj model)
        {
            try
            {
                long clientChurchId;
                Int64.TryParse(id, out clientChurchId);

                if (clientChurchId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                    //return null;
                }
                if (model == null)
                {
                    Session["Error"] = "Your session has expired! Please, re-login";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                    //return Redirect(Url.RouteUrl(new { action = "Index" }) + "#edit&" + id);
                }

                if (Session["_selectedClientChurch"] == null)
                {
                    Session["Error"] = "Your session has expired! Please, re-login";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }

                var thisclientChurch = Session["_selectedClientChurch"] as RegisteredClientChurchReportObj;
                if (thisclientChurch == null || thisclientChurch.ClientChurchId < 1)
                {
                    Session["Error"] = "Your session has expired! Please, re-login";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }
               
                var roles = PortalClientRole.GetRolesClient();
                if (roles == null || !roles.Any())
                {
                    Session["Error"] = "Invalid Client Church Profile Roles";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }

                model.ClientChurchProfile.AllRoles = new List<NameAndValueObject>();
                foreach (var item in roles)
                {
                    model.ClientChurchProfile.AllRoles.Add(new NameAndValueObject { Id = item.RoleClientId, Name = item.Name });
                }

                ModelState.Clear();
                if (!ModelState.IsValid)
                {
                    ViewBag.Error = "Please fill all required fields";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }

                var selRoles = model.ClientChurchProfile.AllRoles.Where(m => model.MyRoleIds.Contains(m.Id)).Select(m => m.Name).ToList();
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                var bankName = ServiceChurch.GetBank(model.BankId).Name;

                var helper = new ClientChurchRegistrationObj
                {
                    ClientChurchId = model.ClientChurchId,
                    ChurchId = model.ChurchId,
                    Name = model.Name,
                    Pastor = model.Pastor,
                    Title = model.Title,
                    Sex = model.Sex,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    StateOfLocationId = model.StateOfLocationId,
                    Address = model.Address,
                    ChurchStructureParishHeadQuarters = model.ChurchStructureParishHeadQuarters,

                    ClientChurchHeadQuarterChurchStructureTypeId = model.ClientChurchHeadQuarterChurchStructureTypeId,
                    RegisteredByUserId = 1,
                    ClientChurchAccount = new ClientChurchAccount
                    {
                        BankId = model.BankId,
                        AccountTypeId = model.AccountTypeId,
                        AccountName = model.ClientChurchAccount.AccountName,
                        AccountNumber = model.ClientChurchAccount.AccountNumber,
                        BankName = bankName
                    },
                    ClientChurchProfile = new ClientProfileRegistrationObj
                    {
                        ClientChurchId = model.ClientChurchId,
                        ClientChurchProfileId = model.ClientChurchProfile.ClientChurchProfileId,
                        Fullname = model.Pastor,
                        MobileNumber = model.PhoneNumber,
                        Sex = model.Sex,
                        Email = model.Email,
                        Username = model.ClientChurchProfile.Username,
                        Password = "password.",
                        ConfirmPassword = "password.",
                        //Password = model.ClientChurchProfile.Password,
                        //ConfirmPassword = model.ClientChurchProfile.ConfirmPassword,
                        MyRoles = selRoles.ToArray(),
                        MyRoleIds = model.MyRoleIds,
                        SelectedRoles = string.Join(";", selRoles),
                        //RegisteredByUserId = 1,
                    },
                    MyRoleIds = model.MyRoleIds,
                };

                var retId = ServiceChurch.UpdateClientChurch(helper);
                if (retId == null)
                {
                    Session["Error"] = "Unable to complete your request! Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }

                if (!retId.IsSuccessful)
                {
                    Session["Error"] = string.IsNullOrEmpty(retId.Message.FriendlyMessage) ? "Unable to update this client church" : retId.Message.FriendlyMessage;
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }

                Session["_selectedClientChurch"] = null;
                Session["Reply"] = "Client Church information was updated successfully";
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


        #region MULTIPLE CRUD a Single View

        [Route("ClientChurch/AddClientChurch")]
        //[ActionName("AddClientChurch")]
        public ActionResult ClientChurch()
        {
            ViewBag.Errors = Session["CreateErrors"] as List<string>;
            ViewBag.Error = Session["CreateError"] as string;
            Session["CreateErrors"] = "";
            Session["CreateError"] = "";

            try
            {
                if (Session["_AddClientChurch"] == null)
                {
                    var clientchurch = new ClientChurchRegistrationObj
                    {
                        ClientChurchProfile = new ClientProfileRegistrationObj
                        {
                            MyRoleIds = new[] { 0 },
                            AllRoles = new List<NameAndValueObject>(),
                        },
                        ClientChurchAccount = new ClientChurchAccount(),
                        Action = "ClientChurch"
                    };

                    var roles = PortalClientRole.GetRolesClient();
                    if (roles == null || !roles.Any())
                    {
                        clientchurch.ClientChurchProfile = new ClientProfileRegistrationObj();
                        ViewBag.Error = "Invalid Client Profile Roles";
                        return View(clientchurch.Action, clientchurch);
                        //return View("NewClientChurch", clientchurch);
                    }

                    foreach (var item in roles)
                    {
                        clientchurch.ClientChurchProfile.AllRoles.Add(new NameAndValueObject { Id = item.RoleClientId, Name = item.Name });
                    }

                    Session["_AddClientChurch"] = clientchurch;
                    return View(clientchurch.Action, clientchurch);
                }

                var model = Session["_AddClientChurch"] as ClientChurchRegistrationObj;
                return View("ClientChurch", model);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return View(new ClientChurchRegistrationObj());
            }

        }


        [HttpPost]
        public ActionResult ClientChurch(ClientChurchRegistrationObj model)
        {
            return null;
        }

        #endregion


        #endregion
    }
}