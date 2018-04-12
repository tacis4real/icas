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

        //[EppSecurity_Authorize]
        public ActionResult MyProfile()
        {


            try
            {
                string msg;
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    return PartialView(new UserProfileInfo());
                }
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
                return PartialView(myProfile);
            }
            catch (Exception)
            {
                return PartialView(new UserProfileInfo());
            }
        }


        [HttpPost]
        //[EppSecurity_Authorize]
        public ActionResult MyProfile(UserProfileInfo model)
        {

            try
            {

                ModelState.Clear();
                ViewBag.UpdateSucceed = "0";

                string msg;
                var roles = PortalRole.GetRolesForUser(User.Identity.Name, out msg) ?? new[] { "*" };
                model.Roles = string.Join(";", roles);

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Kindly supply all necessary information");
                    return View(model);
                }

                var userData = MvcApplication.GetUserData(User.Identity.Name);

                userData.UserId = 0;
                //if (userData == null || userData.UserId < 1)
                if (userData.UserId < 1)
                {
                    ModelState.AddModelError("", "Invalid Profile Session");
                    return View(model);
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
                    ViewBag.UpdateSucceed = "1";
                    return View(model);
                }

                ModelState.AddModelError("", string.IsNullOrEmpty(msg) ? "Unable to update profile at this time!" : msg);
                return View(model);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return View(model);
            }
        }


        public ActionResult ChangeMyPassword()
        {
            ViewBag.IsSuccessful = false;
            var model = new ChangePasswordContract { UserName = User.Identity.Name };
            return PartialView(model);
        }

        #endregion

    }
}