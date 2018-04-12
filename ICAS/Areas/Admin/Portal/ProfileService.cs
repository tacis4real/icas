using System;
using System.Linq;
using System.Reflection;
using ChurchApp.Areas.Admin.Models.PortalModel;
using ICAS.Areas.Admin.Models.PortalModel;
using WebAdminStacks.APIObjs;
using WebAdminStacks.APIs;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS.Areas.Admin.Portal
{
    public class ProfileService
    {
        public static string[] GetProfileKeysFromReflection()
        {
            try
            {
                var propertyInfos = typeof(ProfileInfo).GetProperties();
                if (!propertyInfos.Any()) { return null; }
                var propertyNames = new string[propertyInfos.Count()];

                for (var i = 0; i < propertyInfos.Count(); i++)
                {
                    propertyNames[i] = propertyInfos[i].Name;
                }

                return propertyNames;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static PropertyInfo[] GetProfileKeyInfo()
        {
            try
            {
                var propertyInfos = typeof(ProfileInfo).GetProperties();
                if (!propertyInfos.Any()) { return null; }
                return propertyInfos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public static bool UpdateUserProfile(ProfileInfo profile, long userId, out string msg)
        {
            try
            {
                if (profile == null)
                {
                    msg = "Empty / Invalid Profile Object";
                    return false;
                }
                if (userId < 1)
                {
                    msg = "Empty / Invalid Profile Object";
                    return false;
                }
                var propertyInfos = typeof(ProfileInfo).GetProperties();
                if (!propertyInfos.Any())
                {
                    msg = "Invalid Profile Object";
                    return false;
                }

                var user = PortalUser.GetRawUser(userId);
                if (user == null || user.UserId < 1)
                {
                    msg = "Unable to retrieve user detail information!";
                    return false;
                }
                
                user.Sex = (int)profile.Sex;
                user.Othernames = profile.FirstName;
                user.Surname = profile.LastName;
                user.MobileNumber = profile.MobileNo;


                var retId = PortalUser.UpdateUser(user, out msg);
                if (!retId)
                {
                    if (string.IsNullOrEmpty(msg))
                    {
                        msg = "Process Failed! Please try again later";
                    }
                    return false;
                }

                msg = "";
                return true;
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        public static UserProfileInfo GetUserProfile(long userId, out string msg)
        {
            try
            {
                var user = PortalUser.GetUser(userId);
                if (user == null || user.UserId < 1)
                {
                    msg = "User information not found!";
                    return null;
                }

                msg = "";
                var myProfileDetail = new UserProfileInfo
                {
                    BBPin = "",
                    DateOfBirth = "",
                    FirstName = user.Othernames,
                    FaceBookId = "",
                    Email = user.Email,
                    GooglePlusId = "",
                    LandPhone = "",
                    LastName = user.Surname,
                    MaritalStatus = MaritalStatus.None,
                    MobileNo = user.MobileNumber,
                    MiddleName = user.MiddleName,
                    ResidentialAddress = "",
                    TwitterId = "",
                    SexId = user.SexId,
                    UserId = userId,
                };
                return myProfileDetail;
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        public static bool RegisterNewUser(PortalUserContract user, out string msg)
        {
            if (!ValidateUser(user, out msg))
            {
                return false;
            }

            var thisUser = new UserRegistrationObj
            {
                Email = user.Email,
                RegisteredByUserId = 0,
                ConfirmPassword = user.ConfirmPassword,
                DeviceName = "",
                DeviceSerialNumber = "",
                MobileNumber = user.MobileNo,
                Othernames = user.FirstName,
                Password = user.Password,
                Sex = user.SexId,
                Surname = user.LastName,
                Username = user.UserName,
            };

            var retId = PortalUser.AddUser(thisUser);
            if (retId == null)
            {
                msg = "Process Failed! Please try again later";
                return false;
            }
            if (!retId.Status.IsSuccessful)
            {
                msg = string.IsNullOrEmpty(retId.Status.Message.FriendlyMessage) ? "Process Failed! Please try again later" : retId.Status.Message.FriendlyMessage;
                return false;
            }
            return true;
        }

        public static bool ValidateUser(PortalUserContract user, out string msg)
        {
            try
            {
                if (user == null)
                {
                    msg = "User object is null or empty";
                    return false;
                }
                if (string.IsNullOrEmpty(user.UserName) || user.UserName.Length < 8)
                {
                    msg = "User name is null or empty / less than 8 character length";
                    return false;
                }
                if (!RegExValidator.IsEmailValid(user.Email))
                {
                    msg = "Invalid email address";
                    return false;
                }
                if (string.IsNullOrEmpty(user.FirstName))
                {
                    msg = "Invalid first name";
                    return false;
                }
                if (string.IsNullOrEmpty(user.LastName))
                {
                    msg = "Invalid last name";
                    return false;
                }

                msg = "";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }








        public static bool IsMultipleLogin(string code, out string msg)
        {
            try
            {
                if (MvcApplication.IsUserAlreadyLoggedIn(code, out msg))
                {
                    return true;
                }
                msg = "";
                return false;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = "Post Login Error Occurred!";
                return true;
            }
        }

        public static void ResetUserData(string username)
        {
            try
            {
                MvcApplication.ResetUserData(username);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
            }
        }

        public static void ResetClientData(string username)
        {
            try
            {
                MvcApplication.ResetClientData(username);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
            }
        }

        public static void ResetLogin(string code)
        {
            try
            {
                MvcApplication.ResetLogin(code);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
            }
        }
    }
}