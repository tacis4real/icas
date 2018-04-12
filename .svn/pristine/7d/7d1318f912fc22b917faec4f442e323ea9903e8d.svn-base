using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ICAS.ClientPortal;
using ICAS.Models.ClientPortalModel;
using ICASStacks.DataContract;
using ICASStacks.StackService;
using WebAdminStacks.APIObjs;
using WebAdminStacks.APIs;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS.ClientFilters
{
    // ReSharper disable  InconsistentNaming
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class EppSecurity_ChangeClientAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.IsSuccessful = false;
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                //filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid Password Information");
                //filterContext.Controller.ViewBag.Error = "Invalid Password Information";
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid Password Information: New and Confirm Password not match");
                filterContext.Controller.ViewBag.Error = "Invalid Password Information: New and Confirm Password not match";
            }

            var modelList = filterContext.ActionParameters.Where(ap => ap.Key == "model").ToList();
            if (modelList.IsNullOrEmpty())
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid Password Information");
                filterContext.Controller.ViewBag.Error = "Invalid Password Information";
                return;
            }
            if (!modelList.Any() || modelList.Count != 1)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid Password Information");
                filterContext.Controller.ViewBag.Error = "Invalid Password Information";
                return;
            }

            var model = modelList[0].Value as ChangeClientPasswordContract;

            if (model == null)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid Password Information");
                filterContext.Controller.ViewBag.Error = "Invalid Password Information";
                return;
            }
            if (
              string.Compare(model.OldPassword.Trim(), model.NewPassword.Trim(),
                  StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                model.ConfirmPassword = "";
                model.NewPassword = "";
                model.OldPassword = "";
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Old Password and New Password cannot be same");
                filterContext.Controller.ViewBag.Error = "Current Password and New Password cannot be same";
                return;
            }

            if (
                string.Compare(model.ConfirmPassword.Trim(), model.NewPassword.Trim(),
                    StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                model.ConfirmPassword = "";
                model.NewPassword = "";
                model.OldPassword = "";
                filterContext.Controller.ViewData.ModelState.AddModelError("", "New Password and Confirm New Password must match");
                filterContext.Controller.ViewBag.Error = "New Password and Confirm New Password must match";
                return;
            }


            var changePassword = PortalClientUser.ChangeClientChurchPassword(model.UserName, model.OldPassword, model.NewPassword);
            if (changePassword == null)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Process Failed! Unable to change password");
                filterContext.Controller.ViewBag.Error = "Process Failed! Unable to change password";
                return;
            }
            if (!changePassword.Status.IsSuccessful)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("", string.IsNullOrEmpty(changePassword.Status.Message.FriendlyMessage) ? "Process Failed! Unable to change your password" : changePassword.Status.Message.FriendlyMessage);
                filterContext.Controller.ViewBag.Error = string.IsNullOrEmpty(changePassword.Status.Message.FriendlyMessage) ? "Process Failed! Unable to change your password" : changePassword.Status.Message.FriendlyMessage;
                return;
            }


            filterContext.Controller.ViewBag.IsSuccessful = true;
            base.OnActionExecuting(filterContext);
        }
    }
    

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class EppSecurity_AuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                if (httpContext == null)
                {
                    throw new ArgumentNullException("httpContext");
                }

                var user = httpContext.User;
                if (!user.Identity.IsAuthenticated)
                {
                    return false;
                }

                var _usersSplit = Users.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                var _rolesSplit = Roles.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (_usersSplit.Length > 0 && !_usersSplit.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase))
                {
                    return false;
                }

                if (_rolesSplit.Length > 0 && !_rolesSplit.Any(user.IsInRole))
                {
                    return false;
                }
                if (MvcApplication.GetUserData(user.Identity.Name) == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
            
        }
        
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.HttpContext.User.Identity.IsAuthenticated && filterContext.Result is HttpUnauthorizedResult)
            {
                //var values = new RouteValueDictionary(new
                //{
                //    action = "Login",
                //    controller = "PortalCom"
                //});

                var values = new RouteValueDictionary(new
                {
                    action = "Login",
                    controller = "PortalCom",
                    Area = "Admin"
                });
                filterContext.Result = new RedirectToRouteResult(values);
            }
        }

    }






    // ReSharper disable once InconsistentNaming
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class EppSecurity_AuthenticateClientAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            string msg;

            filterContext.Controller.ViewBag.ClientINFOCode = null;
            filterContext.Controller.ViewBag.FirstLogin = null;

            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Provide login information");
                return;
            }

            var modelList = filterContext.ActionParameters.Where(ap => ap.Key == "model").ToList();
            if (modelList.IsNullOrEmpty() || !modelList.Any() || modelList.Count != 1)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid Login Information");
                return;
            }


            // I Comment Start here....

            var model = modelList[0].Value as ClientLoginContract;
            if (model == null)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid Login Information");
                return;
            }

            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password) || model.Password.Length < 8)
            {
                model.Password = "";
                filterContext.ActionParameters["model"] = model;
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Empty / Invalid username or password or password length");
                return;
            }

            //Validate Client

            #region Latest for Client Church Validation

            ClientChurchLoginResponseObj clientChurch;
            ChurchThemeSetting clientChurchTheme;
            string[] clientRoles;
            try
            {

                #region Validate Client From Database and return it

                clientChurch = PortalClientUser.LoginClientChurchUser(model.UserName, model.Password, 2, "");
                if (clientChurch == null)
                {
                    model.Password = "";
                    filterContext.ActionParameters["model"] = model;
                    filterContext.Controller.ViewData.ModelState.AddModelError("", "Login Failed! Please try again later");
                    return;
                }

                if (clientChurch.ClientChurchProfileId < 1)
                {
                    model.Password = "";
                    filterContext.ActionParameters["model"] = model;
                    filterContext.Controller.ViewData.ModelState.AddModelError("", string.IsNullOrEmpty(clientChurch.Status.Message.FriendlyMessage) ? "Login Failed!" : clientChurch.Status.Message.FriendlyMessage);
                    return;
                }

                #endregion


                #region Get Roles for Validated Client from Database

                clientRoles = PortalClientRole.GetRolesForClientChurchProfile(model.UserName, out msg);
                if (clientRoles == null || clientRoles.Length < 1)
                {
                    model.Password = "";
                    filterContext.ActionParameters["model"] = model;
                    filterContext.Controller.ViewData.ModelState.AddModelError("", msg.Length > 0 ? msg : "You have not been assigned to any role!");
                    return;
                }

                #endregion


                #region Get Client Parent Theme Details from Database

                // Get Client Parent Church Info Here
                clientChurchTheme = ServiceChurch.GetClientChurchThemeDetail(clientChurch.ClientChurchId);
                if (clientChurchTheme == null)
                {
                    model.Password = "";
                    filterContext.ActionParameters["model"] = model;
                    filterContext.Controller.ViewData.ModelState.AddModelError("", "Login Failed! No Client Church Theme Info Found. Please try again later");
                    return;
                }

                #endregion


            }
            catch (Exception ex)
            {
                model.Password = "";
                filterContext.ActionParameters["model"] = model;
                filterContext.Controller.ViewData.ModelState.AddModelError("", ex.Message);
                return;
            }


            #region Manipulating Validated Client Detail for Login Sessioning
            //Check Multiple Login
            //Log user Out of previous login
            //Create new login
            var code = model.UserName.Trim() + model.Password.Trim();
            if (ClientProfileService.IsMultipleLogin(code, out msg))
            {
                if (filterContext.HttpContext.Session != null)
                {
                    ClientProfileService.ResetLogin(code);
                    ClientProfileService.ResetClientData(model.UserName.Trim());
                    filterContext.HttpContext.Session["ClientINFO"] = null;
                }
                new FormsAuthenticationService().SignOut();
            }

            var clientChurchProfileId = clientChurch.ClientChurchProfileId;
            var clientChurchId = clientChurch.ClientChurchId;
            var clientChurchData = new ClientData
            {
                ClientProfileId = clientChurchProfileId,
                ClientId = clientChurchId,
                Username = clientChurch.Username,
                Email = clientChurch.Email,
                FullName = clientChurch.Fullname,
                MobileNumber = clientChurch.MobileNumber,
                Roles = clientRoles,
                AuthToken = clientChurch.AuthToken
            };

            if (!MvcApplication.SetClientData(clientChurchData))
            {
                model.Password = "";
                filterContext.ActionParameters["model"] = model;
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid authentication!");
                return;
            }

            clientChurchProfileId = clientChurch.ClientChurchProfileId;
            clientChurchId = clientChurch.ClientChurchId;
            var clientChurchParentData = new ClientChurchData
            {
                ClientId = clientChurchId,
                ChurchId = clientChurchTheme.ChurchId,
                Username = clientChurch.Username,
                Logo = clientChurchTheme.ThemeLogo,
                LogoPath = clientChurchTheme.ThemeLogoPath,
                Theme = clientChurchTheme.ThemeColor
            };

            if (!MvcApplication.SetClientChurchData(clientChurchParentData))
            {
                model.Password = "";
                filterContext.ActionParameters["model"] = model;
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid authentication!");
                return;
            }
            #endregion


            #region Constructing Client Logging Session Detail & Creating Client Session Ticket

            var ticketData = clientChurchProfileId + "|" + clientChurchId + "|" + clientChurch.Username + "|" + string.Join(";", clientRoles);
            var encTicket = new FormsAuthenticationService().SignIn(model.UserName, false, ticketData);
            if (String.IsNullOrEmpty(encTicket))
            {
                model.Password = "";
                filterContext.ActionParameters["model"] = model;
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid authentication!");
                return;
            }

            filterContext.HttpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
            filterContext.Controller.ViewBag.ClientINFOCode = code.GetHashCode().ToString(CultureInfo.InvariantCulture);
            filterContext.Controller.ViewBag.FirstLogin = clientChurch.IsFirstTimeAccess;
            filterContext.Controller.ViewBag.ClientLoginDataItem = clientChurchData;
            base.OnActionExecuting(filterContext);

            #endregion


            #endregion

            #region Old Validation

            //ClientLoginResponseObj client;
            //ChurchThemeSetting clientChurchTheme;
            //string[] clientRoles;
            //try
            //{

            //    #region Validate Client From Database and return it

            //    client = PortalClientUser.LoginClientUser(model.UserName, model.Password, 2, "");
            //    if (client == null)
            //    {
            //        model.Password = "";
            //        filterContext.ActionParameters["model"] = model;
            //        filterContext.Controller.ViewData.ModelState.AddModelError("", "Login Failed! Please try again later");
            //        return;
            //    }

            //    if (client.ClientProfileId < 1)
            //    {
            //        model.Password = "";
            //        filterContext.ActionParameters["model"] = model;
            //        filterContext.Controller.ViewData.ModelState.AddModelError("", string.IsNullOrEmpty(client.Status.Message.FriendlyMessage) ? "Login Failed!" : client.Status.Message.FriendlyMessage);
            //        return;
            //    }

            //    #endregion


            //    #region Get Roles for Validated Client from Database

            //    clientRoles = PortalClientRole.GetRolesForClient(model.UserName, out msg);
            //    if (clientRoles == null || clientRoles.Length < 1)
            //    {
            //        model.Password = "";
            //        filterContext.ActionParameters["model"] = model;
            //        filterContext.Controller.ViewData.ModelState.AddModelError("", msg.Length > 0 ? msg : "You have not been assigned to any role!");
            //        return;
            //    }

            //    #endregion


            //    #region Get Client Parent Theme Details from Database

            //    // Get Client Parent Church Info Here
            //    clientChurchTheme = ServiceChurch.GetClientChurchThemeInfo(client.ClientId);
            //    if (clientChurchTheme == null)
            //    {
            //        model.Password = "";
            //        filterContext.ActionParameters["model"] = model;
            //        filterContext.Controller.ViewData.ModelState.AddModelError("", "Login Failed! No Client Church Theme Info Found. Please try again later");
            //        return;
            //    }

            //    #endregion


            //}
            //catch (Exception ex)
            //{
            //    model.Password = "";
            //    filterContext.ActionParameters["model"] = model;
            //    filterContext.Controller.ViewData.ModelState.AddModelError("", ex.Message);
            //    return;
            //}


            //#region Manipulating Validated Client Detail for Login Sessioning
            ////Check Multiple Login
            ////Log user Out of previous login
            ////Create new login
            //var code = model.UserName.Trim() + model.Password.Trim();
            //if (ClientProfileService.IsMultipleLogin(code, out msg))
            //{
            //    if (filterContext.HttpContext.Session != null)
            //    {
            //        ClientProfileService.ResetLogin(code);
            //        ClientProfileService.ResetClientData(model.UserName.Trim());
            //        filterContext.HttpContext.Session["ClientINFO"] = null;
            //    }
            //    new FormsAuthenticationService().SignOut();
            //}

            //var clientProfileId = client.ClientProfileId;
            //var clientId = client.ClientId;
            //var clientData = new ClientData
            //{
            //    ClientProfileId = clientProfileId,
            //    ClientId = clientId,
            //    Username = client.Username,
            //    Email = client.Email,
            //    FullName = client.Fullname,
            //    MobileNumber = client.MobileNumber,
            //    Roles = clientRoles,
            //    AuthToken = client.AuthToken
            //};

            //if (!MvcApplication.SetClientData(clientData))
            //{
            //    model.Password = "";
            //    filterContext.ActionParameters["model"] = model;
            //    filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid authentication!");
            //    return;
            //}

            //clientProfileId = client.ClientProfileId;
            //clientId = client.ClientId;
            //var clientChurchData = new ClientChurchData
            //{
            //    ClientId = clientId,
            //    ChurchId = clientChurchTheme.ChurchId,
            //    Username = client.Username,
            //    Logo = clientChurchTheme.ThemeLogo,
            //    LogoPath = clientChurchTheme.ThemeLogoPath,
            //    Theme = clientChurchTheme.ThemeColor
            //};

            //if (!MvcApplication.SetClientChurchData(clientChurchData))
            //{
            //    model.Password = "";
            //    filterContext.ActionParameters["model"] = model;
            //    filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid authentication!");
            //    return;
            //}
            //#endregion


            //#region Constructing Client Logging Session Detail & Creating Client Session Ticket

            //var ticketData = clientProfileId + "|" + clientId + "|" + client.Username + "|" + string.Join(";", clientRoles);
            //var encTicket = new FormsAuthenticationService().SignIn(model.UserName, false, ticketData);
            //if (String.IsNullOrEmpty(encTicket))
            //{
            //    model.Password = "";
            //    filterContext.ActionParameters["model"] = model;
            //    filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid authentication!");
            //    return;
            //}

            //filterContext.HttpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
            //filterContext.Controller.ViewBag.ClientINFOCode = code.GetHashCode().ToString(CultureInfo.InvariantCulture);
            //filterContext.Controller.ViewBag.FirstLogin = client.IsFirstTimeAccess;
            //filterContext.Controller.ViewBag.ClientLoginDataItem = clientData;
            //base.OnActionExecuting(filterContext);

            //#endregion


            #endregion
            
        }

    }

    // ReSharper disable once InconsistentNaming
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class EppSecurity_AuthorizeClientAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.HttpContext.User.Identity.IsAuthenticated && filterContext.Result is HttpUnauthorizedResult)
            {

                var values = new RouteValueDictionary(new
                {
                    action = "Login",
                    controller = "ChurchPortalCom"
                });
                filterContext.Result = new RedirectToRouteResult(values);
            }
            else
            {
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    var values = new RouteValueDictionary(new
                    {
                        action = "Login",
                        controller = "ChurchPortalCom"
                    });
                    filterContext.Result = new RedirectToRouteResult(values);
                }

            }

        }


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                if (httpContext == null)
                {
                    throw new ArgumentNullException("httpContext");
                }

                var client = httpContext.User;
                if (!client.Identity.IsAuthenticated)
                {
                    return false;
                }

                var _clientsSplit = Users.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                var _rolesSplit = Roles.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (_clientsSplit.Length > 0 && !_clientsSplit.Contains(client.Identity.Name, StringComparer.OrdinalIgnoreCase))
                {
                    return false;
                }

                if (_rolesSplit.Length > 0 && !_rolesSplit.Any(client.IsInRole))
                {
                    return false;
                }
                if (MvcApplication.GetClientData(client.Identity.Name) == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }

        }

    }













    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class EppParamActionAttribute : ActionNameSelectorAttribute
    {
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                return true;

            var request = controllerContext.RequestContext.HttpContext.Request;
            var myItem = request[methodInfo.Name];
            return myItem != null;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class EppUrlManagerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var thisUrl = filterContext.RequestContext.HttpContext.Request.Url.ToString();       //.RedirectToRoute(filterContext.RequestContext.HttpContext.Request.UrlReferrer.ToString(), filterContext.RouteData);
            base.OnActionExecuted(filterContext);
        }
    }

    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class EppSecurity_ResetClientAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.IsSuccessful = false;
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid update information");
                filterContext.Controller.ViewBag.Error = "Invalid update information";
                return;
            }

            var modelList = filterContext.ActionParameters.Where(ap => ap.Key == "model").ToList();
            if (modelList.IsNullOrEmpty())
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid update information");
                filterContext.Controller.ViewBag.Error = "Invalid update information";
                return;
            }
            if (!modelList.Any() || modelList.Count != 1)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid update information");
                filterContext.Controller.ViewBag.Error = "Invalid update information";
                return;
            }

            var model = modelList[0].Value as ResetChurchAdminPasswordContract;

            if (model == null)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid update information");
                filterContext.Controller.ViewBag.Error = "Invalid update information";
                return;
            }

            var passReset = PortalClientUser.ResetChurchAdminUserPassword(model.UserName);
            if (passReset == null)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("", "Process Failed! Unable to reset password");
                filterContext.Controller.ViewBag.Error = "Process Failed! Unable to reset password";
                return;
            }
            if (!passReset.Status.IsSuccessful)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("", string.IsNullOrEmpty(passReset.Status.Message.FriendlyMessage) ? "Process Failed! Unable to update password" : passReset.Status.Message.FriendlyMessage);
                filterContext.Controller.ViewBag.Error = string.IsNullOrEmpty(passReset.Status.Message.FriendlyMessage) ? "Process Failed! Unable to update password" : passReset.Status.Message.FriendlyMessage;
                return;
            }

            filterContext.Controller.ViewBag.IsSuccessful = true;
            filterContext.Controller.ViewBag.ThisNewPassword = passReset.NewPassword;
            base.OnActionExecuting(filterContext);
        }
    }


    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    //public sealed class EppSecurity_LockAccessAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        filterContext.Controller.ViewBag.IsSuccessful = false;
    //        if (!filterContext.Controller.ViewData.ModelState.IsValid)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid user information");
    //            filterContext.Controller.ViewBag.Error = "Invalid update information";
    //            return;
    //        }

    //        var modelList = filterContext.ActionParameters.Where(ap => ap.Key == "model").ToList();
    //        if (modelList.IsNullOrEmpty())
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid user information");
    //            filterContext.Controller.ViewBag.Error = "Invalid update information";
    //            return;
    //        }
    //        if (!modelList.Any() || modelList.Count != 1)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid user information");
    //            filterContext.Controller.ViewBag.Error = "Invalid update information";
    //            return;
    //        }

    //        var model = modelList[0].Value as ResetPasswordContract;

    //        if (model == null)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid user information");
    //            filterContext.Controller.ViewBag.Error = "Invalid update information";
    //            return;
    //        }
    //        var lockUser = PortalUser.LockUser(model.UserName);
    //        if (lockUser == null)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Process Failed! Unable to unlock account");
    //            filterContext.Controller.ViewBag.Error = "Process Failed! Unable to unlock account";
    //            return;
    //        }
    //        if (!lockUser.Status.IsSuccessful)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", string.IsNullOrEmpty(lockUser.Status.Message.FriendlyMessage) ? "Process Failed! Unable to update user's account" : lockUser.Status.Message.FriendlyMessage);
    //            filterContext.Controller.ViewBag.Error = string.IsNullOrEmpty(lockUser.Status.Message.FriendlyMessage) ? "Process Failed! Unable to update user's account" : lockUser.Status.Message.FriendlyMessage;
    //            return;
    //        }

    //        filterContext.Controller.ViewBag.IsSuccessful = true;
    //        base.OnActionExecuting(filterContext);
    //    }
    //}


    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    //public sealed class EppSecurity_UnlockAccessAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        filterContext.Controller.ViewBag.IsSuccessful = false;
    //        if (!filterContext.Controller.ViewData.ModelState.IsValid)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid user information");
    //            filterContext.Controller.ViewBag.Error = "Invalid update information";
    //            return;
    //        }

    //        var modelList = filterContext.ActionParameters.Where(ap => ap.Key == "model").ToList();
    //        if (modelList.IsNullOrEmpty())
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid user information");
    //            filterContext.Controller.ViewBag.Error = "Invalid update information";
    //            return;
    //        }
    //        if (!modelList.Any() || modelList.Count != 1)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid user information");
    //            filterContext.Controller.ViewBag.Error = "Invalid update information";
    //            return;
    //        }

    //        var model = modelList[0].Value as ResetPasswordContract;

    //        if (model == null)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid user information");
    //            filterContext.Controller.ViewBag.Error = "Invalid update information";
    //            return;
    //        }
    //        var unlock = PortalUser.UnlockUser(model.UserName);
    //        if (unlock == null)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Process Failed! Unable to unlock user");
    //            filterContext.Controller.ViewBag.Error = "Process Failed! Unable to unlock user";
    //            return;
    //        }
    //        if (!unlock.Status.IsSuccessful)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", string.IsNullOrEmpty(unlock.Status.Message.FriendlyMessage) ? "Process Failed! Unable to update user's account" : unlock.Status.Message.FriendlyMessage);
    //            filterContext.Controller.ViewBag.Error = string.IsNullOrEmpty(unlock.Status.Message.FriendlyMessage) ? "Process Failed! Unable to update user's account" : unlock.Status.Message.FriendlyMessage;
    //            return;
    //        }

    //        filterContext.Controller.ViewBag.IsSuccessful = true; ;
    //        base.OnActionExecuting(filterContext);
    //    }
    //}


    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    //public sealed class EppSecurity_AccessAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {

    //        if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
    //        {
    //            filterContext.Controller.ViewBag.UserAuthInfo = null;
    //            base.OnActionExecuting(filterContext);
    //            return;
    //        }

    //        var frmId = (FormsIdentity)filterContext.HttpContext.User.Identity;
    //        var usData = frmId.Ticket.UserData;
    //        if (string.IsNullOrEmpty(usData))
    //        {
    //            filterContext.Controller.ViewBag.UserAuthInfo = null;
    //            base.OnActionExecuting(filterContext);
    //            return;
    //        }

    //        var userDataSplit = usData.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
    //        if (!userDataSplit.Any() || userDataSplit.Length != 3)
    //        {
    //            filterContext.Controller.ViewBag.UserAuthInfo = null;
    //            base.OnActionExecuting(filterContext);
    //            return;
    //        }

    //        if (!DataCheck.IsNumeric(userDataSplit[0].Trim()))
    //        {
    //            filterContext.Controller.ViewBag.UserAuthInfo = null;
    //            base.OnActionExecuting(filterContext);
    //            return;
    //        }

    //        var roles = userDataSplit[2].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

    //        var identity = new FormsIdentity(frmId.Ticket);
    //        var principal = new StcPrincipal(identity, roles);

    //        var userData = new UserData
    //        {
    //            UserId = long.Parse(userDataSplit[0].Trim()),
    //            Username = frmId.Name,
    //            Email = userDataSplit[1].Trim(),
    //            Roles = roles,
    //        };

    //        if (!MvcApplication.SetUserData(userData))
    //        {
    //            filterContext.Controller.ViewBag.UserAuthInfo = null;
    //            base.OnActionExecuting(filterContext);
    //            return;
    //        }


    //        filterContext.Controller.ViewBag.UserAuthInfo = userData;
    //        filterContext.HttpContext.User = principal;
    //        base.OnActionExecuting(filterContext);
    //    }

    //}

    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    //public sealed class EppSecurity_AuthenticateAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        string msg;

    //        filterContext.Controller.ViewBag.UserINFOCode = null;
    //        filterContext.Controller.ViewBag.FirstLogin = null;

    //        if (!filterContext.Controller.ViewData.ModelState.IsValid)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Provide login information");
    //            return;
    //        }


    //        var modelList = filterContext.ActionParameters.Where(ap => ap.Key == "model").ToList();
    //        if (modelList.IsNullOrEmpty() || !modelList.Any() || modelList.Count != 1)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid Login Information");
    //            return;
    //        }

    //        var model = modelList[0].Value as UserLoginContract;

    //        if (model == null)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid Login Information");
    //            return;
    //        }

    //        if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password) || model.Password.Length < 8)
    //        {
    //            model.Password = "";
    //            filterContext.ActionParameters["model"] = model;
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Empty / Invalid username or password or password length");
    //            return;
    //        }

    //        //Validate User
    //        UserLoginResponseObj user;
    //        string[] userRoles;
    //        try
    //        {

    //            user = PortalUser.LoginUser(model.UserName, model.Password, 2, "");
    //            if (user == null)
    //            {
    //                model.Password = "";
    //                filterContext.ActionParameters["model"] = model;
    //                filterContext.Controller.ViewData.ModelState.AddModelError("", "Login Failed! Please try again later");
    //                return;
    //            }

    //            if (user.UserId < 1)
    //            {
    //                model.Password = "";
    //                filterContext.ActionParameters["model"] = model;
    //                filterContext.Controller.ViewData.ModelState.AddModelError("", string.IsNullOrEmpty(user.Status.Message.FriendlyMessage) ? "Login Failed!" : user.Status.Message.FriendlyMessage);
    //                return;
    //            }

    //            userRoles = PortalRole.GetRolesForUser(model.UserName, out msg);
    //            if (userRoles == null || userRoles.Length < 1)
    //            {
    //                model.Password = "";
    //                filterContext.ActionParameters["model"] = model;
    //                filterContext.Controller.ViewData.ModelState.AddModelError("", msg.Length > 0 ? msg : "You have not been assigned to any role!");
    //                return;
    //            }
    //            if (userRoles.Contains("AgentUser"))
    //            {
    //                model.Password = "";
    //                filterContext.ActionParameters["model"] = model;
    //                filterContext.Controller.ViewData.ModelState.AddModelError("", msg.Length > 0 ? msg : "Access Denied");
    //                return;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            model.Password = "";
    //            filterContext.ActionParameters["model"] = model;
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", ex.Message);
    //            return;
    //        }

    //        //Check Multiple Login
    //        //Log user Out of previous login
    //        //Create new login
    //        var code = model.UserName.Trim() + model.Password.Trim();
    //        if (ClientProfileService.IsMultipleLogin(code, out msg))
    //        {
    //            if (filterContext.HttpContext.Session != null)
    //            {
    //                ClientProfileService.ResetLogin(code);
    //                ClientProfileService.ResetUserData(model.UserName.Trim());
    //                filterContext.HttpContext.Session["UserINFO"] = null;
    //            }
    //            new FormsAuthenticationService().SignOut();
    //        }


    //        var userId = user.UserId;
    //        var userData = new UserData
    //        {
    //            UserId = userId,
    //            Username = user.Username,
    //            Email = user.Email,
    //            Roles = userRoles,
    //            AuthToken = user.AuthToken
    //        };

    //        if (!MvcApplication.SetUserData(userData))
    //        {
    //            model.Password = "";
    //            filterContext.ActionParameters["model"] = model;
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid authentication!");
    //            return;
    //        }

    //        var ticketData = userId + "|" + user.Email + "|" + string.Join(";", userRoles);
    //        var encTicket = new FormsAuthenticationService().SignIn(model.UserName, false, ticketData);
    //        if (String.IsNullOrEmpty(encTicket))
    //        {
    //            model.Password = "";
    //            filterContext.ActionParameters["model"] = model;
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid authentication!");
    //            return;
    //        }

    //        filterContext.HttpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
    //        filterContext.Controller.ViewBag.UserINFOCode = code.GetHashCode().ToString(CultureInfo.InvariantCulture);
    //        filterContext.Controller.ViewBag.FirstLogin = user.IsFirstTimeAccess;
    //        filterContext.Controller.ViewBag.LoginDataItem = userData;
    //        base.OnActionExecuting(filterContext);
    //    }

    //}

    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    //public sealed class EppSecurity_RegisterAccessAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        if (!filterContext.HttpContext.Request.IsAjaxRequest())
    //        { return; }
    //        filterContext.HttpContext.Response.StatusCode = 600;
    //        filterContext.Controller.ViewBag.ValidAuthourized = "0";

    //        var error = new StringBuilder();
    //        var modelState = filterContext.Controller.ViewData.ModelState;
    //        if (!modelState.IsValid)
    //        {
    //            var errorModel =
    //                    from x in modelState.Keys
    //                    where modelState[x].Errors.Count > 0
    //                    select new
    //                    {

    //                        key = x,
    //                        errors = modelState[x].Errors.
    //                                               Select(y => y.ErrorMessage).
    //                                               ToArray()
    //                    };

    //            foreach (var item in errorModel)
    //            {
    //                error.AppendLine(string.Format("Error Key: {0} Error Message: {1}", item.key, string.Join(",", item.errors)));

    //            }

    //            filterContext.HttpContext.Response.AppendHeader("message", error.ToString());
    //            return;
    //        }


    //        var modelList = filterContext.ActionParameters.Where(ap => ap.Key == "model").ToList();
    //        if (modelList.IsNullOrEmpty())
    //        {
    //            filterContext.HttpContext.Response.AppendHeader("message", "Invalid Registration Information");
    //            return;
    //        }
    //        if (!modelList.Any() || modelList.Count != 1)
    //        {
    //            filterContext.HttpContext.Response.AppendHeader("message", "Invalid Registration Information");
    //            return;
    //        }

    //        var model = modelList[0].Value as PortalUserContract;

    //        if (model == null)
    //        {
    //            filterContext.HttpContext.Response.AppendHeader("message", "Invalid Registration Information");
    //            return;
    //        }

    //        if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName))
    //        {
    //            filterContext.HttpContext.Response.AppendHeader("message", "Invalid Registration Information");
    //            return;
    //        }


    //        string msg;
    //        var retVal = ProfileService.RegisterNewUser(model, out msg);
    //        if (!retVal)
    //        {
    //            filterContext.HttpContext.Response.AppendHeader("message", msg.Length > 0 ? msg : "Invalid Registration Information");
    //            return;
    //        }
    //        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
    //        filterContext.HttpContext.Response.AppendHeader("", "");
    //        filterContext.Controller.ViewBag.ValidAuthourized = "1";
    //        base.OnActionExecuting(filterContext);
    //    }
    //}
    

    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    //public sealed class EppSecurity_FirstAccessAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        filterContext.Controller.ViewBag.IsSuccessful = false;
    //        if (!filterContext.Controller.ViewData.ModelState.IsValid)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid update information");
    //            return;
    //        }

    //        var modelList = filterContext.ActionParameters.Where(ap => ap.Key == "model").ToList();
    //        if (modelList.IsNullOrEmpty())
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid update information");
    //            return;
    //        }
    //        if (!modelList.Any() || modelList.Count != 1)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid update information");
    //            return;
    //        }

    //        var model = modelList[0].Value as ChangePasswordContract;
    //        if (model == null)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Invalid update information");
    //            return;
    //        }
    //        if (
    //          string.Compare(model.OldPassword.Trim(), model.NewPassword.Trim(),
    //              StringComparison.InvariantCultureIgnoreCase) == 0)
    //        {
    //            model.ConfirmPassword = "";
    //            model.NewPassword = "";
    //            model.OldPassword = "";
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Old Password and New Password" +
    //                                                                           " must be different");
    //            return;
    //        }

    //        if (
    //            string.Compare(model.ConfirmPassword.Trim(), model.NewPassword.Trim(),
    //                StringComparison.InvariantCultureIgnoreCase) != 0)
    //        {
    //            model.ConfirmPassword = "";
    //            model.NewPassword = "";
    //            model.OldPassword = "";
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "New Password and Confirm New Password must match");
    //            return;
    //        }

    //        var changePassword = PortalUser.ChangeFirstTimePassword(model.UserName, model.OldPassword, model.NewPassword);
    //        if (changePassword == null)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", "Process Failed! Unable to change password");
    //            return;
    //        }
    //        if (!changePassword.Status.IsSuccessful)
    //        {
    //            filterContext.Controller.ViewData.ModelState.AddModelError("", string.IsNullOrEmpty(changePassword.Status.Message.FriendlyMessage) ? "Process Failed! Unable to update password" : changePassword.Status.Message.FriendlyMessage);
    //            return;
    //        }


    //        filterContext.Controller.ViewBag.IsSuccessful = true;
    //        base.OnActionExecuting(filterContext);
    //    }

    //}
}