using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICAS.Areas.Admin.Manager;
using ICAS.Areas.Admin.Models.PortalModel;
using ICAS.ClientFilters;
using ICAS.ClientPortal;
using ICAS.Models.ClientPortalModel;
using WebAdminStacks.APIs;
using WebCribs.TechCracker.WebCribs.TechCracker;
using Sex = ICAS.Models.ClientPortalModel.Sex;

namespace ICAS.Controllers.ChurchFramework
{
    public class MyChurchAdminController : Controller
    {



        #region My Church Core Admin

        [EppSecurity_AuthorizeClient]
        public ActionResult MyProfile()
        {


            ViewBag.Errors = Session["ClientProfileErrors"] as List<string>;
            ViewBag.Error = Session["ClientProfileError"] as string;
            Session["ClientProfileErrors"] = null;
            Session["ClientProfileError"] = null;
            

            try
            {

                string msg;
                var clientData = MvcApplication.GetClientData(User.Identity.Name);
                if (clientData == null || clientData.ClientProfileId < 1)
                {
                    return View(new ClientChurchUserProfileInfo());
                }

                var myProfile = ClientProfileService.GetClientChurchUserProfile(clientData.ClientProfileId, out msg);
                if (myProfile == null) return View(new ClientChurchUserProfileInfo());
                var roles = PortalClientRole.GetRolesForClientChurchProfile(User.Identity.Name, out msg);
                if (roles != null && roles.Length > 0)
                {
                    myProfile.Roles = string.Join(";", roles);
                    myProfile.MyRoles = roles;
                }

                myProfile.MobileNo = CustomManager.ReCleanMobile(myProfile.MobileNo);
                myProfile.UserName = clientData.Username;
                myProfile.Email = clientData.Email;
                return View(myProfile);


                //if (Session["myClientProfile"] == null)
                //{
                //    string msg;
                //    var clientData = MvcApplication.GetClientData(User.Identity.Name);
                //    if (clientData == null || clientData.ClientProfileId < 1)
                //    {
                //        return View(new ClientChurchUserProfileInfo());
                //    }

                //    var myProfile = ClientProfileService.GetClientChurchUserProfile(clientData.ClientProfileId, out msg);
                //    if (myProfile == null) return View(new ClientChurchUserProfileInfo());
                //    var roles = PortalClientRole.GetRolesForClientChurchProfile(User.Identity.Name, out msg);
                //    if (roles != null && roles.Length > 0)
                //    {
                //        myProfile.Roles = string.Join(";", roles);
                //        myProfile.MyRoles = roles;
                //    }

                //    myProfile.MobileNo = CustomManager.ReCleanMobile(myProfile.MobileNo);
                //    myProfile.UserName = clientData.Username;
                //    myProfile.Email = clientData.Email;
                //    return View(myProfile);
                //}

                //var model = Session["myClientProfile"] as ClientChurchUserProfileInfo;
                //return View(model);


            }
            catch (Exception)
            {
                return View(new ClientChurchUserProfileInfo());
            }
        }


        [HttpPost]
        [EppSecurity_AuthorizeClient]
        public ActionResult MyProfile(ClientChurchUserProfileInfo model)
        {

            var errorLists = new List<string>();
            try
            {
                //ModelState.Clear();
                ViewBag.UpdateSucceed = "0";

                string msg;
                var roles = PortalClientRole.GetRolesForClientChurchProfile(User.Identity.Name, out msg) ?? new[] { "*" };
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


                    //Session["myClientProfile"] = model;
                    //errorLists = (from value in ViewData.ModelState.Values
                    //              where value.Errors.Count > 0
                    //              from error in value.Errors
                    //              where !string.IsNullOrEmpty(error.ErrorMessage)
                    //              select error.ErrorMessage).ToList();
                    //Session["ClientProfileErrors"] = errorLists;
                    //return Redirect(Url.RouteUrl(new { action = "MyProfile" }));

                   
                }


                var clientData = MvcApplication.GetClientData(User.Identity.Name);
                if (clientData == null || clientData.ClientProfileId < 1)
                {
                    //ModelState.AddModelError("", "Invalid Profile Session");
                    //return View(model);


                    Session["InvalidClientProfileSessionError"] = "Invalid Profile Session";
                    return Redirect(Url.RouteUrl(new { action = "Index", controller = "SignOutClient" }));
                    //return View(model);  

                    //Session["ClientProfileError"] = "Invalid Profile Session";
                    //return Redirect(Url.RouteUrl(new { action = "MyProfile" }));
                }

                //if (clientData == null || clientData.ClientProfileId < 1)
                //{
                //    ModelState.AddModelError("", "Invalid Profile Session");
                //    return View(model);
                //}

                var myProfile = new ClientChurchProfileInfo
                {
                    FullName = model.FullName,
                    BBPin = model.BBPin,
                    DateOfBirth = model.DateOfBirth,
                    FaceBookId = model.FaceBookId,
                    GooglePlusId = model.GooglePlusId,
                    LandPhone = model.LandPhone,
                    MaritalStatus = model.MaritalStatus,
                    MobileNo = model.MobileNo,
                    ResidentialAddress = model.ResidentialAddress,
                    Sex = (Sex)Enum.Parse(typeof(Sex), model.SexId.ToString(CultureInfo.InvariantCulture)),
                    TwitterId = model.TwitterId,
                    UserName = model.UserName,
                };


                if (ClientProfileService.UpdateClientChurchUserProfile(myProfile, model.ClientChurchProfileId, out msg))
                {

                    Session["ClientProfileUpdatedSucceed"] = "Profile updated successfully";
                    return Redirect(Url.RouteUrl(new { action = "MyChurch", controller = "Home" }));
                    //ViewBag.UpdateSucceed = "1";
                    //return View(model);
                }


                Session["ClientProfileUpdatedError"] = string.IsNullOrEmpty(msg) ? "Unable to update profile at this time!" : msg;
                return Redirect(Url.RouteUrl(new { action = "MyChurch", controller = "Home" }));

                //ModelState.AddModelError("", string.IsNullOrEmpty(msg) ? "Unable to update profile at this time!" : msg);
                //return View(model);

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return View(model);
            }
        }


        [EppSecurity_AuthorizeClient]
        public ActionResult ChangeMyPassword()
        {
            ViewBag.IsSuccessful = false;
            var model = new ChangeClientPasswordContract { UserName = User.Identity.Name };
            return View(model);

        }


        [HttpPost]
        [EppSecurity_AuthorizeClient]
        [EppSecurity_ChangeClientAccess]
        public ActionResult ChangeMyPassword(ChangeClientPasswordContract model)
        {
            var error = ViewBag.Error as string;
            if (!string.IsNullOrEmpty(error) && error.Length > 1)
            {
                return View(new ChangeClientPasswordContract());
            }
            var retVal = ViewBag.IsSuccessful != null && (bool)ViewBag.IsSuccessful;
            return View(!retVal ? model : new ChangeClientPasswordContract());
        }

        #endregion


       
	}
}