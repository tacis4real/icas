using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICAS.Areas.Admin.Filters;
using ICAS.Areas.Admin.Manager;
using ICAS.Areas.Admin.Models.PortalModel;
using ICAS.Areas.Admin.Portal;
using WebAdminStacks.APIs;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS.Areas.Admin.Controllers.Framework
{
    public class AppUserProfileController : Controller
    {


        #region Core Admin

        [EppSecurity_Authorize]
        public ActionResult MyProfile()
        {

            ViewBag.Errors = Session["ProfileErrors"] as List<string>;
            ViewBag.Error = Session["ProfileError"] as string;

            Session["ProfileErrors"] = null;
            Session["ProfileError"] = null;


            try
            {


                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return View(new UserProfileInfo());
                }
                string msg;
                var myProfile = ProfileService.GetUserProfile(userData.UserId, out msg);
                if (myProfile == null) return View(new UserProfileInfo());
                var roles = PortalRole.GetRolesForUser(User.Identity.Name, out msg);
                if (roles != null && roles.Length > 0)
                {
                    myProfile.Roles = string.Join(";", roles);
                    myProfile.MyRoles = roles;
                }
                myProfile.MobileNo = CustomManager.ReCleanMobile(myProfile.MobileNo);
                myProfile.UserName = userData.Username;
                myProfile.Email = userData.Email;
                return View(myProfile);



                //if (Session["myProfile"] == null)
                //{
                //    var userData = MvcApplication.GetUserData(User.Identity.Name);
                //    if (userData == null || userData.UserId < 1)
                //    {
                //        return View(new UserProfileInfo());
                //    }
                //    string msg;
                //    var myProfile = ProfileService.GetUserProfile(userData.UserId, out msg);
                //    if (myProfile == null) return View(new UserProfileInfo());
                //    var roles = PortalRole.GetRolesForUser(User.Identity.Name, out msg);
                //    if (roles != null && roles.Length > 0)
                //    {
                //        myProfile.Roles = string.Join(";", roles);
                //        myProfile.MyRoles = roles;
                //    }
                //    myProfile.MobileNo = CustomManager.ReCleanMobile(myProfile.MobileNo);
                //    myProfile.UserName = userData.Username;
                //    myProfile.Email = userData.Email;
                //    return View(myProfile);
                //}

                //var model = Session["myProfile"] as UserProfileInfo;
                //return View(model);
                
                
            }
            catch (Exception)
            {
                return View(new UserProfileInfo());
            }
        }


        [HttpPost]
        [EppSecurity_Authorize]
        public ActionResult MyProfile(UserProfileInfo model)
        {

            var errorLists = new List<string>();
            try
            {

                ModelState.Clear();
                ViewBag.UpdateSucceed = "0";

                string msg;
                var roles = PortalRole.GetRolesForUser(User.Identity.Name, out msg) ?? new[] { "*" };
                model.Roles = string.Join(";", roles);

                if (!RegExValidator.IsGSMNumberValid(model.MobileNo))
                {
                    ModelState.AddModelError("", "Invalid Mobile Number");
                    return View(model);
                }

                if (!ModelState.IsValid)
                {


                    ModelState.AddModelError("", "Kindly supply all necessary information");
                    return View(model);


                    //Session["myProfile"] = model;
                    //errorLists = (from value in ViewData.ModelState.Values
                    //              where value.Errors.Count > 0
                    //              from error in value.Errors
                    //              where !string.IsNullOrEmpty(error.ErrorMessage)
                    //              select error.ErrorMessage).ToList();
                    //Session["ProfileErrors"] = errorLists;
                    //return Redirect(Url.RouteUrl(new { action = "MyProfile" }));
                    
                }

                var userData = MvcApplication.GetUserData(User.Identity.Name);

                //userData.UserId = 0;
                //if (userData == null || userData.UserId < 1)
                //if (userData.UserId < 1)
                if (userData == null || userData.UserId < 1)
                {
                    //ModelState.AddModelError("", "Invalid Profile Session");
                    //return View(model);

                    //Session["myProfile"] = model;
                    
                    //return Redirect(Url.RouteUrl(new { action = "MyProfile" }));
                    //return Redirect(Url.RouteUrl(new { action = "Index", controller = "Home" }));
                    //Session["myProfile"] = null;
                    Session["InvalidProfileSessionError"] = "Invalid Profile Session";
                    return Redirect(Url.RouteUrl(new { action = "Index", controller = "SignOut" }));
                }
                
                var myProfile = new ProfileInfo
                {
                    LastName = model.LastName,
                    BBPin = model.BBPin,
                    DateOfBirth = model.DateOfBirth,
                    FaceBookId = model.FaceBookId,
                    FirstName = model.FirstName,
                    GooglePlusId = model.GooglePlusId,
                    LandPhone = model.LandPhone,
                    MaritalStatus = model.MaritalStatus,
                    MiddleName = model.MiddleName,
                    MobileNo = model.MobileNo,
                    ResidentialAddress = model.ResidentialAddress,
                    Sex = (Sex)Enum.Parse(typeof(Sex), model.SexId.ToString(CultureInfo.InvariantCulture)),
                    TwitterId = model.TwitterId,
                    UserName = model.UserName,
                };

                if (ProfileService.UpdateUserProfile(myProfile, model.UserId, out msg))
                {
                    
                    //ViewBag.UpdateSucceed = "1";
                    //return View(model);
                    //ViewBag.ProfileUpdateSucceed = "Profile updated successfully";

                    //Session["myProfile"] = null;
                    Session["ProfileUpdateSucceed"] = "Profile updated successfully";
                    return Redirect(Url.RouteUrl(new { action = "Dashboard", controller = "Home" }));
                    
                }


                
                //ModelState.AddModelError("", string.IsNullOrEmpty(msg) ? "Unable to update profile at this time!" : msg);
                //return View(model);
                //return Redirect(Url.RouteUrl(new { action = "MyProfile" }));
                //Session["myProfile"] = model;

                //Session["myProfile"] = null;
                Session["ProfileUpdateError"] = string.IsNullOrEmpty(msg) ? "Unable to update profile at this time!" : msg;
                return Redirect(Url.RouteUrl(new { action = "Dashboard", controller = "Home" }));
                
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return View(model);
            }
        }


        [EppSecurity_Authorize]
        public ActionResult ChangeMyPassword()
        {
            ViewBag.IsSuccessful = false;
            var model = new ChangePasswordContract { UserName = User.Identity.Name };
            return View(model);
        }


        [HttpPost]
        [EppSecurity_Authorize]
        [EppSecurity_ChangeAccess]
        public ActionResult ChangeMyPassword(ChangePasswordContract model)
        {

            var error = ViewBag.Error as string;
            if (!string.IsNullOrEmpty(error) && error.Length > 1)
            {
                return View(new ChangePasswordContract());
            }
            var retVal = ViewBag.IsSuccessful != null && (bool)ViewBag.IsSuccessful;
            return View(!retVal ? model : new ChangePasswordContract());
        }

        #endregion

    }
}