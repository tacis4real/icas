using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICAS.Areas.Admin.Manager;
using ICAS.Areas.Admin.Models.PortalModel;
using WebAdminStacks.APIObjs;
using WebAdminStacks.APIs;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS.Areas.Admin.Controllers.Framework
{
    public class AppUserAdminController : Controller
    {
        

        public ActionResult Index()
        {
            ViewBag.Reply = Session["Reply"] as string;
            ViewBag.Error = Session["Error"] as string;
            Session["Reply"] = "";
            Session["Error"] = "";

            try
            {

                //var userList = GetUsers();
                var items = PortalUser.GetUserList() ?? new List<RegisteredUserReportObj>();
                if (!items.Any())
                {
                    ViewBag.Error = "No Registered User Info Found!";
                    return View(new List<RegisteredUserReportObj>());
                }
                if (!User.IsInRole("PortalAdmin"))
                {
                    items = items.FindAll(m => m.UserId != 1);
                    if (!items.Any())
                    {
                        ViewBag.Error = "No User Info Found!";
                        return View(new List<RegisteredUserReportObj>());
                    }
                }
                var users = items.Where(item => !item.MyRoles.Contains("AgentUser")).ToList();

                Session["_portalUsersInfos"] = users;
                return View(users);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<RegisteredUserReportObj>());
            }
        }


        #region User CRUD

        #region CREATE


        public ActionResult AddPortalUser()
        {
            ViewBag.Errors = Session["CreateErrors"] as List<string>;
            ViewBag.Error = Session["CreateError"] as string;
            Session["CreateErrors"] = "";
            Session["CreateError"] = "";

            if (Session["_NewPortalUser"] == null)
            {
                string msg;
                var user = new AuthPortalUser
                {
                    MyRoleIds = new[] { 0 }, AllRoles = new List<NameAndValueObject>(),
                    Action = "AddPortalUser"
                };
                var roles = PortalRole.GetRoles();
                if (roles == null || !roles.Any())
                {
                    ViewBag.Error = "Invalid User Roles";
                    return View("AddPortalUser", new AuthPortalUser { Action = "AddPortalUser" });
                    //return View(new AuthPortalUser());
                }

                foreach (var item in roles)
                {
                    if (item.Name == "*" || item.Name == "AgentUser") { continue; }
                    //if (!User.IsInRole("PortalAdmin"))
                    //{
                    //    if (item.Name == "PortalAdmin" || item.Name == "SiteAdmin") { continue; }
                    //}
                    user.AllRoles.Add(new NameAndValueObject { Id = item.RoleId, Name = item.Name });
                }
                user.SexId = 1;
                Session["_NewPortalUser"] = user;
                return View("AddPortalUser", user);
                //return View(user);
            }

            var model = Session["_NewPortalUser"] as AuthPortalUser;
            if (model != null)
            {
                model.Action = "AddPortalUser";
            }
            return View("AddPortalUser", model);
            //return View(model);
        }

        [HttpPost]
        public ActionResult AddPortalUser(AuthPortalUser model)
        {
            var errorLists = new List<string>();
            try
            {
                Session["_NewPortalUser"] = model;
                model.SexId = 1;

                var roles = PortalRole.GetRoles();
                if (roles == null || !roles.Any())
                {
                    ViewBag.Error = "Invalid User Roles";
                    return View("AddPortalUser", new AuthPortalUser());
                    //return View(new AuthPortalUser());
                }
                model.AllRoles = new List<NameAndValueObject>();
                foreach (var item in roles)
                {
                    if (item.Name == "*" || item.Name == "AgentUser") { continue; }
                    //if (!User.IsInRole("PortalAdmin"))
                    //{
                    //    if (item.Name == "PortalAdmin" || item.Name == "SiteAdmin") { continue; }
                    //}
                    model.AllRoles.Add(new NameAndValueObject { Id = item.RoleId, Name = item.Name });
                }

                if (!RegExValidator.IsGSMNumberValid(model.MobileNo))
                {
                    errorLists.Add("Invalid mobile number");
                    Session["CreateErrors"] = errorLists;
                    return Redirect(Url.RouteUrl(new { action = "AddPortalUser" }));
                }

                if (!ModelState.IsValid)
                {
                    model.Password = "";
                    model.ConfirmPassword = "";
                    Session["_NewPortalUser"] = model;

                    errorLists = (from value in ViewData.ModelState.Values
                                  where value.Errors.Count > 0
                                  from error in value.Errors
                                  where !string.IsNullOrEmpty(error.ErrorMessage)
                                  select error.ErrorMessage).ToList();

                    Session["CreateErrors"] = errorLists;
                    //return View();
                    return Redirect(Url.RouteUrl(new { action = "AddPortalUser" }));
                }

                if (model.MyRoleIds == null || !model.MyRoleIds.Any())
                {
                    model.Password = "";
                    model.ConfirmPassword = "";
                    Session["_NewPortalUser"] = model;
                    Session["CreateError"] = "You must select at least one role for this user";
                    return Redirect(Url.RouteUrl(new { action = "AddPortalUser" }));
                    //return View();
                }

                var selRoles = model.AllRoles.Where(m => model.MyRoleIds.Contains(m.Id)).Select(m => m.Name).ToList();
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                var helper = new UserRegistrationObj
                {
                    ConfirmPassword = model.ConfirmPassword,
                    Email = model.Email,
                    Othernames = model.FirstName,
                    Surname = model.LastName,
                    MobileNumber = model.MobileNo,
                    MyRoleIds = model.MyRoleIds,
                    MyRoles = selRoles.ToArray(),
                    Username = model.UserName,
                    Password = model.Password,
                    SelectedRoles = string.Join(";", selRoles),
                    Sex = model.SexId,
                    RegisteredByUserId = 1,
                    //RegisteredByUserId = Convert.ToInt32(userData.UserId),
                };

                //var structure = ServiceChurch.AddClientChurchStructure<Region>(new ClientChurchStructureRegObj());

                var retId = PortalUser.AddUser(helper);
                if (retId == null)
                {
                    Session["CreateError"] = "Unable to add user. Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "AddPortalUser" }));
                    //return View();
                }
                if (!retId.Status.IsSuccessful)
                {
                    Session["CreateError"] = string.IsNullOrEmpty(retId.Status.Message.FriendlyMessage) ? "Unable to add user! Please try again later" : retId.Status.Message.FriendlyMessage;
                    return Redirect(Url.RouteUrl(new { action = "AddPortalUser" }));
                    //return View();
                }

                Session["_NewPortalUser"] = null;
                Session["Reply"] = "User Information was added successfully";
                return Redirect(Url.RouteUrl(new { action = "Index" }));
                //return View();

            }
            catch (Exception ex)
            {
                Session["CreateError"] = ex.Message;
                return Redirect(Url.RouteUrl(new { action = "AddPortalUser" }));
                //return View();
            }
        }
        #endregion


        #region EDIT

        public ActionResult ModifyPortalUser(string id)
        {
            ViewBag.Errors = Session["EditErrors"] as List<string>;
            ViewBag.Error = Session["EditError"] as string;
            Session["EditErrors"] = null;
            Session["EditError"] = null;

            try
            {

                long userId;
                Int64.TryParse(id, out userId);
                if (userId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }
                if (Session["_portalUsersInfos"] == null)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }
                var portalUsers = Session["_portalUsersInfos"] as List<RegisteredUserReportObj>;
                if (portalUsers == null || !portalUsers.Any())
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }
                var portalUser = portalUsers.Find(m => m.UserId == userId);
                if (portalUser == null || portalUser.UserId < 1)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }

                var helper = new AuthPortalUser
                {
                    ConfirmPassword = "",
                    Email = portalUser.Email,
                    FailedPasswordCount = portalUser.FailedPasswordCount,
                    FirstName = portalUser.Othernames,
                    IsApproved = portalUser.IsApproved,
                    IsLockedOut = portalUser.IsLockedOut,
                    LastName = portalUser.Surname,
                    LastLockedOutTimeStamp = portalUser.LastLockedOutTimeStamp,
                    LastLoginTimeStamp = portalUser.LastLoginTimeStamp,
                    LastPasswordChangeTimeStamp = portalUser.PasswordChangeTimeStamp,
                    MiddleName = portalUser.MiddleName,
                    MobileNo = CustomManager.ReCleanMobile(portalUser.MobileNumber),
                    MyRoleIds = portalUser.MyRoleIds,
                    MyRoles = portalUser.MyRoles,
                    UserName = portalUser.Username,
                    Password = portalUser.Password,
                    SelectedRoles = portalUser.SelectedRoles,
                    SexId = portalUser.SexId,
                    TimeStampRegistered = portalUser.TimeStampRegistered,
                    UserId = portalUser.UserId,
                    Roles = portalUser.MyRoleIds.ToList(),

                    Action = "ModifyPortalUser"
                };

                var roles = PortalRole.GetRoles();
                if (roles == null || !roles.Any())
                {
                    ViewBag.Error = "Invalid User Roles";
                    return View("AddPortalUser", helper);
                }
                helper.AllRoles = new List<NameAndValueObject>();

                foreach (var item in roles)
                {
                    if (item.Name == "*" || item.Name == "AgentUser") { continue; }
                    //if (!User.IsInRole("PortalAdmin"))
                    //{
                    //    if (item.Name == "PortalAdmin" || item.Name == "SiteAdmin") { continue; }
                    //}
                    helper.AllRoles.Add(new NameAndValueObject { Id = item.RoleId, Name = item.Name });
                }

                Session["_selectedPortalUser"] = portalUser;
                return View("AddPortalUser", helper);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        [HttpPost]
        public ActionResult ModifyPortalUser(string id, AuthPortalUser model)
        {
            
            var errorLists = new List<string>();
            try
            {
                long userId;
                Int64.TryParse(id, out userId);


                if (userId < 1)
                {
                    Session["Error"] = "Invalid selection";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }

                if (model == null)
                {
                    Session["Error"] = "Your session has expired! Please, re-login";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }

                if (Session["_selectedPortalUser"] == null)
                {
                    Session["Error"] = "Your session has expired! Please, re-login";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }

                var thisPortalUser = Session["_selectedPortalUser"] as RegisteredUserReportObj;
                if (thisPortalUser == null || thisPortalUser.UserId < 1)
                {
                    Session["Error"] = "Your session has expired! Please, re-login";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }

                var roles = PortalRole.GetRoles();
                if (roles == null || !roles.Any())
                {
                    Session["Error"] = "Invalid User Roles";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }
                model.AllRoles = new List<NameAndValueObject>();
                foreach (var item in roles)
                {
                    //if (item.Name == "*" || item.Name == "AgentUser") { continue; }
                    //if (!User.IsInRole("PortalAdmin"))
                    //{
                    //    if (item.Name == "PortalAdmin" || item.Name == "SiteAdmin") { continue; }
                    //}
                    model.AllRoles.Add(new NameAndValueObject { Id = item.RoleId, Name = item.Name });
                }

                //if (model.MobileNo.Substring(0, 3) != "234")
                //{
                //    model.MobileNo = model.MobileNo.TrimStart('0');
                //    model.MobileNo = "234" + model.MobileNo;
                //}
                if (!RegExValidator.IsGSMNumberValid(model.MobileNo))
                {
                    Session["EditError"] = "Invalid mobile number";
                    return Redirect(Url.RouteUrl(new { action = "ModifyPortalUser" }));
                }

                model.SexId = 1;
                ModelState.Clear();
                if (!ModelState.IsValid)
                {
                    Session["_NewPortalUser"] = model;
                    Session["EditError"] = "Please fill all required fields";
                    return Redirect(Url.RouteUrl(new { action = "ModifyPortalUser" }));
                }

                var selRoles = model.AllRoles.Where(m => model.MyRoleIds.Contains(m.Id)).Select(m => m.Name).ToList();
                var helper = new UserRegistrationObj
                {
                    Password = "password.",
                    ConfirmPassword = "password.",
                    Email = model.Email,
                    Othernames = model.FirstName,
                    Surname = model.LastName,
                    MobileNumber = model.MobileNo,
                    MyRoleIds = model.MyRoleIds,
                    MyRoles = selRoles.ToArray(),
                    Username = model.UserName,
                    SelectedRoles = string.Join(";", selRoles),
                    Sex = model.SexId,
                    UserId = model.UserId,
                    //IsActive = model.IsApproved
                };

                var retId = PortalUser.UpdateUser(helper);
                if (retId == null)
                {
                    Session["Error"] = "Unable to complete your request! Please try again later";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }

                if (!retId.IsSuccessful)
                {
                    Session["Error"] = string.IsNullOrEmpty(retId.Message.FriendlyMessage) ? "Unable to update this user" : retId.Message.FriendlyMessage;
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }

                Session["_selectedPortalUser"] = null;
                Session["Reply"] = "Information was updated successfully";
                return Redirect(Url.RouteUrl(new { action = "Index" }));


            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }


        public ActionResult ChangeUserStatus(string userId, string callerId)
        {
            try
            {

                long portalUserId;
                int callerTypeId;
                Int64.TryParse(userId, out portalUserId);
                Int32.TryParse(callerId, out callerTypeId);

                if (portalUserId < 1 || callerTypeId < 1 || callerTypeId > 3)
                {
                    ViewBag.Error = "Invalid selection";
                    return null;
                }
                if (Session["_portalUsersInfos"] == null)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return null;
                }
                var users = Session["_portalUsersInfos"] as List<RegisteredUserReportObj>;
                if (users == null || !users.Any())
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return null;
                }
                var user = users.Find(m => m.UserId == portalUserId);
                if (user == null || user.UserId < 1)
                {
                    ViewBag.Error = "Your session has expired! Please try again later";
                    return null;
                }
                if (user.UserId == 1)
                {
                    ViewBag.Error = "Sorry, You cannot edit this user information";
                    return null;
                }

                var approveAgent = new ResetPasswordContract
                {
                    UserId = user.UserId,
                    UserName = user.Username,
                    FullName = user.Surname + " " + user.Othernames,
                    CallerType = callerTypeId,
                    ProcessType = callerTypeId == 1 ? "Reset User Password" : callerTypeId == 2 ? "Lock User Account" : callerTypeId == 3 ? "Unlock User Account" : "Unknown"
                };
                return PartialView(approveAgent);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                Session["Error"] = ex.Message;
                return null;
            }
        }



        [HttpPost]
        public ActionResult ChangeUserStatus(ResetPasswordContract model)
        {
            try
            {

                if (model == null || model.CallerType < 1 || model.CallerType > 3)
                {
                    Session["Error"] = "Invalid Request";
                    return Redirect(Url.RouteUrl(new { action = "Index" }));
                }

                switch (model.CallerType)
                {
                    case 1:
                        var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                        if (userData.UserId < 1)
                        {
                            Session["EditError"] = "Invalid user information! Please try again later";
                            return Redirect(Url.RouteUrl(new { action = "Index", controller = "SignOut" }));
                        }
                        var newPasswordInfo = PortalUser.ResetPassword(model.UserName);
                        if (newPasswordInfo == null)
                        {
                            Session["Error"] = "Process Failed! Unable to reset password";
                            return Redirect(Url.RouteUrl(new { action = "Index" }));
                        }
                        if (!newPasswordInfo.Status.IsSuccessful)
                        {
                            Session["Error"] = string.IsNullOrEmpty(newPasswordInfo.Status.Message.FriendlyMessage) ? "Process Failed! Unable to update user's account" : newPasswordInfo.Status.Message.FriendlyMessage;
                            return Redirect(Url.RouteUrl(new { action = "Index" }));
                        }
                        Session["Reply"] = "Password was reset to: " + newPasswordInfo.NewPassword;
                        return Redirect(Url.RouteUrl(new { action = "Index" }));
                    case 2:
                        var lockUser = PortalUser.LockUser(model.UserName);
                        if (lockUser == null)
                        {
                            Session["Error"] = "Process Failed! Unable to lock account";
                            return Redirect(Url.RouteUrl(new { action = "Index" }));
                        }
                        if (!lockUser.Status.IsSuccessful)
                        {
                            Session["Error"] = string.IsNullOrEmpty(lockUser.Status.Message.FriendlyMessage) ? "Process Failed! Unable to update user's account" : lockUser.Status.Message.FriendlyMessage;
                            return Redirect(Url.RouteUrl(new { action = "Index" }));
                        }
                        Session["Reply"] = "User account was updated successfully";
                        return Redirect(Url.RouteUrl(new { action = "Index" }));
                    case 3:
                        var unlockUser = PortalUser.UnlockUser(model.UserName);
                        if (unlockUser == null)
                        {
                            Session["Error"] = "Process Failed! Unable to unlock account";
                            return Redirect(Url.RouteUrl(new { action = "Index" }));
                        }
                        if (!unlockUser.Status.IsSuccessful)
                        {
                            Session["Error"] = string.IsNullOrEmpty(unlockUser.Status.Message.FriendlyMessage) ? "Process Failed! Unable to update user's account" : unlockUser.Status.Message.FriendlyMessage;
                            return Redirect(Url.RouteUrl(new { action = "Index" }));
                        }
                        Session["Reply"] = "User account was updated successfully";
                        return Redirect(Url.RouteUrl(new { action = "Index" }));
                }

                Session["Error"] = "Unable to process your request! Please try again later";
                return Redirect(Url.RouteUrl(new { action = "Index" }));
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                Session["Error"] = ex.Message;
                return Redirect(Url.RouteUrl(new { action = "Index" }));
            }
        }

        #endregion
        
        #endregion


        #region API-Controller


        //[Route("api/StationUser/Users")]
        [HttpGet]
        public IEnumerable<RegisteredUserReportObj> GetUsers()
        {

            try
            {
                return PortalUser.GetUserList() ?? new List<RegisteredUserReportObj>();
            }
            catch (Exception)
            {
                return null;
            }

        }

        [HttpGet]
        [Route("api/StationUser/Users")]
        public JsonResult GetUserData()
        {

            try
            {
                var users = PortalUser.GetUserList() ?? new List<RegisteredUserReportObj>();
                return Json(users, JsonRequestBehavior.AllowGet);  
            }
            catch (Exception)
            {
                return null;
            }

        }


        [HttpGet]
        [Route("api/StationUser/UserDetail")]
        public ActionResult GetUserDetail()
        {

            try
            {
                var users = PortalUser.GetUserList() ?? new List<RegisteredUserReportObj>();
                return Json(users, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }

        }

        [HttpPost]
        [Route("api/StationUser/AddUserDetail")]
        public ActionResult AddUserDetail(UserRegistrationObj userRegistrationObj)
        {

            try
            {
                var user = PortalUser.AddUser(userRegistrationObj) ?? new UserRegResponse();
                return Json(user, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }

        }

        #endregion


    }
}