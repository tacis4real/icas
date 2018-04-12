using System;
using System.Linq;
using System.Reflection;
using ICAS.Models.ClientPortalModel;
using WebAdminStacks.APIs;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS.ClientPortal
{
    public class ClientProfileService
    {
        public static string[] GetProfileKeysFromReflection()
        {
            try
            {
                var propertyInfos = typeof(ClientProfileInfo).GetProperties();
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
                var propertyInfos = typeof(ClientProfileInfo).GetProperties();
                if (!propertyInfos.Any()) { return null; }
                return propertyInfos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #region Client Church

        public static ClientChurchUserProfileInfo GetClientChurchUserProfile(long clientChurchProfileId, out string msg)
        {
            try
            {
                var clientAdminUser = PortalClientUser.GetClientChurchAdminUser(clientChurchProfileId);
                if (clientAdminUser == null || clientAdminUser.ClientChurchProfileId < 1)
                {
                    msg = "Admin User information not found!";
                    return null;
                }

                msg = "";
                var myProfileDetail = new ClientChurchUserProfileInfo
                {
                    BBPin = "",
                    DateOfBirth = "",
                    FullName = clientAdminUser.Fullname,
                    FaceBookId = "",
                    Email = clientAdminUser.Email,
                    GooglePlusId = "",
                    LandPhone = "",
                    MaritalStatus = MaritalStatus.None,
                    MobileNo = clientAdminUser.MobileNumber,
                    ResidentialAddress = "",
                    TwitterId = "",
                    SexId = clientAdminUser.SexId,
                    ClientChurchProfileId = clientChurchProfileId,
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

        public static bool UpdateClientChurchUserProfile(ClientChurchProfileInfo profile, long clientProfileId, out string msg)
        {
            try
            {
                if (profile == null)
                {
                    msg = "Empty / Invalid Profile Object";
                    return false;
                }
                if (clientProfileId < 1)
                {
                    msg = "Empty / Invalid Profile Object";
                    return false;
                }
                var propertyInfos = typeof(ClientChurchProfileInfo).GetProperties();
                if (!propertyInfos.Any())
                {
                    msg = "Invalid Profile Object";
                    return false;
                }

                var clientChurchUser = PortalClientUser.GetRawClientChurchUser(clientProfileId);
                if (clientChurchUser == null || clientChurchUser.ClientChurchProfileId < 1)
                {
                    msg = "Unable to retrieve user detail information!";
                    return false;
                }

                clientChurchUser.Sex = (int)profile.Sex;
                clientChurchUser.Fullname = profile.FullName;
                clientChurchUser.MobileNumber = profile.MobileNo;


                var retId = PortalClientUser.UpdateClientChurchUser(clientChurchUser, out msg);
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

        #endregion




        public static bool UpdateClientUserProfile(ClientProfileInfo profile, long clientProfileId, out string msg)
        {
            try
            {
                if (profile == null)
                {
                    msg = "Empty / Invalid Profile Object";
                    return false;
                }
                if (clientProfileId < 1)
                {
                    msg = "Empty / Invalid Profile Object";
                    return false;
                }
                var propertyInfos = typeof(ClientProfileInfo).GetProperties();
                if (!propertyInfos.Any())
                {
                    msg = "Invalid Profile Object";
                    return false;
                }

                var clientUser = PortalClientUser.GetRawClientUser(clientProfileId);
                if (clientUser == null || clientUser.ClientProfileId < 1)
                {
                    msg = "Unable to retrieve user detail information!";
                    return false;
                }

                clientUser.Sex = (int)profile.Sex;
                clientUser.Fullname = profile.FullName;
                clientUser.MobileNumber = profile.MobileNo;


                var retId = PortalClientUser.UpdateClientUser(clientUser, out msg);
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

        public static ClientUserProfileInfo GetClientUserProfile(long clientProfileId, out string msg)
        {
            try
            {
                var clientAdminUser = PortalClientUser.GetClientAdminUser(clientProfileId);
                if (clientAdminUser == null || clientAdminUser.ClientProfileId < 1)
                {
                    msg = "Admin User information not found!";
                    return null;
                }

                msg = "";
                var myProfileDetail = new ClientUserProfileInfo
                {
                    BBPin = "",
                    DateOfBirth = "",
                    FullName = clientAdminUser.Fullname,
                    FaceBookId = "",
                    Email = clientAdminUser.Email,
                    GooglePlusId = "",
                    LandPhone = "",
                    MaritalStatus = MaritalStatus.None,
                    MobileNo = clientAdminUser.MobileNumber,
                    ResidentialAddress = "",
                    TwitterId = "",
                    SexId = clientAdminUser.SexId,
                    ClientProfileId = clientProfileId,
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

        public static bool RegisterNewUser(ClientPortalUserContract user, out string msg)
        {
            //if (!ValidateUser(user, out msg))
            //{
            //    return false;
            //}

            //var thisUser = new UserRegistrationObj
            //{
            //    Email = user.Email,
            //    RegisteredByUserId = 0,
            //    ConfirmPassword = user.ConfirmPassword,
            //    DeviceName = "",
            //    DeviceSerialNumber = "",
            //    MobileNumber = user.MobileNo,
            //    Othernames = user.FirstName,
            //    Password = user.Password,
            //    Sex = user.SexId,
            //    Surname = user.LastName,
            //    Username = user.UserName,
            //};

            //var retId = PortalUser.AddUser(thisUser);
            //if (retId == null)
            //{
            //    msg = "Process Failed! Please try again later";
            //    return false;
            //}
            //if (!retId.Status.IsSuccessful)
            //{
            //    msg = string.IsNullOrEmpty(retId.Status.Message.FriendlyMessage) ? "Process Failed! Please try again later" : retId.Status.Message.FriendlyMessage;
            //    return false;
            //}
            msg = "";
            return true;
        }

        public static bool ValidateUser(ClientPortalUserContract user, out string msg)
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