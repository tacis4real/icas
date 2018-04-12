using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Helpers;
using WebAdminStacks.APIObjs;
using WebAdminStacks.Common;
using WebAdminStacks.DataContract;
using WebAdminStacks.Infrastructure;
using WebAdminStacks.Infrastructure.Contract;
using WebAdminStacks.Repository.Helper;
using WebCribs.TechCracker.WebCribs.TechCracker;
using WebCribs.TechCracker.WebCribs.TechCracker.Controls.CommonEnums;
using WebCribs.TechCracker.WebCribs.TechCracker.GateKeeper;
using User = WebAdminStacks.DataContract.User;

namespace WebAdminStacks.Repository
{
    internal class UserRepository
    {

        private readonly IWebAdminRepository<User> _repository;
        private readonly IWebAdminRepository<UserLoginActivity> _loginActivityRepository;
        private readonly IWebAdminRepository<DeviceAccessAuthorization> _authorizationRepository;
        private readonly IWebAdminRepository<UserDevice> _deviceRepository;

        private readonly WebAdminUoWork _uoWork;

        public UserRepository()
        {
            _uoWork = new WebAdminUoWork();
            _repository = new WebAdminRepository<User>(_uoWork);
            _loginActivityRepository = new WebAdminRepository<UserLoginActivity>(_uoWork);
            _authorizationRepository = new WebAdminRepository<DeviceAccessAuthorization>(_uoWork);
            _deviceRepository = new WebAdminRepository<UserDevice>(_uoWork);
        }



        internal UserRegResponse AddUser(UserRegistrationObj agentUser)
        {
            var response = new UserRegResponse
            {
                UserId = 0,
                Email = agentUser.Email,
                Surname = agentUser.Surname,
                Othernames = agentUser.Othernames,
                MobileNumber = agentUser.MobileNumber,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {

                if (agentUser.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Registration Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(agentUser, out valResults))
                {
                    var errorDetail = new StringBuilder();
                    if (!valResults.IsNullOrEmpty())
                    {
                        errorDetail.AppendLine("Following error occurred:");
                        valResults.ForEachx(m => errorDetail.AppendLine(m.ErrorMessage));
                    }
                    else
                    {
                        errorDetail.AppendLine("Validation error occurred! Please check all supplied parameters and try again");
                    }
                    response.Status.Message.FriendlyMessage = errorDetail.ToString();
                    response.Status.Message.TechnicalMessage = errorDetail.ToString();
                    response.Status.IsSuccessful = false;
                    return response;
                }

                agentUser.MobileNumber = CleanMobile(agentUser.MobileNumber);
                string msg;
                if (IsDuplicate(agentUser.Username, agentUser.Email, out msg))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }

                var userRoles = GetUserRoles(agentUser.MyRoleIds);
                var userObj = new User
                {
                    Surname = agentUser.Surname,
                    Othernames = agentUser.Othernames,
                    Email = agentUser.Email,
                    AccessCode = Crypto.HashPassword(agentUser.Password),
                    IsFirstTimeLogin = true,
                    FailedPasswordCount = 0,
                    IsApproved = true,
                    IsDeleted = false,
                    IsLockedOut = false,
                    LastLockedOutTimeStamp = "",
                    IsMobileActive = true,
                    IsPasswordChangeRequired = true,
                    IsWebActive = true,
                    IsEmailVerified = true,
                    IsMobileNumberVerified = true,
                    LastLoginTimeStamp = "",
                    MobileNumber = CleanMobile(agentUser.MobileNumber),
                    RegisteredByUserId = agentUser.RegisteredByUserId,
                    Password = Crypto.GenerateSalt(),
                    Sex = agentUser.Sex,
                    PasswordChangeTimeStamp = "",
                    UserCode = EncryptionHelper.GenerateSalt(30, 50),
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    Username = agentUser.Username,

                };

                if (!userRoles.IsNullOrEmpty())
                {
                    userObj.UserRoles = userRoles;
                }

                var processedUser = _repository.Add(userObj);
                _uoWork.SaveChanges();
                if (processedUser.UserId > 0)
                {
                    response.UserId = processedUser.UserId;
                    response.Email = agentUser.Email;
                    response.Status.IsSuccessful = true;
                    return response;
                }

                response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                response.Status.IsSuccessful = false;
                return response;
            }
            catch (DbEntityValidationException ex)
            {
                response.Status.Message.FriendlyMessage = "Unable to complete registration at this time! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.Message;
                response.Status.IsSuccessful = false;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete registration at this time! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.Message;
                response.Status.IsSuccessful = false;
                return response;
            }
        }


        private string CleanMobile(string mobileNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(mobileNumber))
                    return string.Empty;
                if (mobileNumber.StartsWith("234"))
                    return mobileNumber;
                mobileNumber = mobileNumber.TrimStart(new char[] { '0' });
                return string.Format("234{0}", mobileNumber);
            }
            catch (Exception)
            {
                return mobileNumber;
            }
        }


        private List<UserRole> GetUserRoles(int[] roleIds)
        {
            var userRoles = new List<UserRole>();
            try
            {
                if (roleIds == null || !roleIds.Any())
                {
                    userRoles.Add(new UserRole { RoleId = 4 });
                    return userRoles;
                }
                userRoles.AddRange(from item in roleIds where item >= 1 select new UserRole { RoleId = item });
                //if (!roleIds.Contains(4))
                //{
                //    userRoles.Add(new UserRole { RoleId = 4 });
                //}
                return userRoles;
            }
            catch (Exception)
            {
                userRoles.Add(new UserRole { RoleId = 4 });
                return userRoles;
            }
        }
        
        
        internal RespStatus UpdateUser(UserRegistrationObj user)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };
            try
            {
                if (user.Equals(null))
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your registration";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(user, out valResults))
                {
                    var errorDetail = new StringBuilder();
                    if (!valResults.IsNullOrEmpty())
                    {
                        errorDetail.AppendLine("Following error occurred:");
                        valResults.ForEachx(m => errorDetail.AppendLine(m.ErrorMessage));
                    }
                    else
                    {
                        errorDetail.AppendLine("Validation error occurred! Please check all supplied parameters and try again");
                    }
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = errorDetail.ToString();
                    return response;
                }

                var userRoles = GetUserRoles(user.MyRoleIds);
                string msg;
                var thisUser = GetUser(user.UserId, out msg);
                if (thisUser == null || thisUser.Email.Length < 1)
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = string.IsNullOrEmpty(msg) ? "Invalid User Information" : msg;
                    return response;
                }


                user.MobileNumber = CleanMobile(user.MobileNumber);
                using (var db = _uoWork.BeginTransaction())
                {
                    try
                    {

                        if (!DeleteUserRoles(user.UserId, userRoles))
                        {
                            db.Rollback();
                            response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                            return response;
                        }
                        thisUser.Sex = user.Sex;
                        thisUser.Othernames = user.Othernames;
                        thisUser.Surname = user.Surname;
                        thisUser.MobileNumber = user.MobileNumber;
                        thisUser.Email = user.Email;
                        thisUser.FailedPasswordCount = 0;
                        thisUser.IsDeleted = false;
                        thisUser.IsLockedOut = false;
                        thisUser.LastLockedOutTimeStamp = "";
                        thisUser.TimeStampRegistered = DateScrutnizer.CurrentTimeStamp();
                        //thisUser.IsApproved = user.IsActive;
                        var processedUser = _repository.Update(thisUser);
                        _uoWork.SaveChanges();


                        if (processedUser.UserId < 1)
                        {
                            db.Rollback();
                            response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                            return response;
                        }
                        db.Commit();
                        response.IsSuccessful = true;
                        return response;
                    }
                    catch (DbEntityValidationException ex)
                    {
                        db.Rollback();
                        response.Message.FriendlyMessage =
                            "Unable to complete your request due to error! Please try again later";
                        response.Message.TechnicalMessage = "Error" + ex.Message;
                        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                        return response;
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        response.Message.FriendlyMessage =
                             "Unable to complete your request due to error! Please try again later";
                        response.Message.TechnicalMessage = "Error" + ex.Message;
                        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                        return response;
                    }
                }


            }
            catch (DbEntityValidationException ex)
            {
                response.Message.FriendlyMessage =
                              "Unable to complete your request due to error! Please try again later";
                response.Message.TechnicalMessage = "Error" + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return response;
            }
            catch (Exception ex)
            {
                response.Message.FriendlyMessage =
                             "Unable to complete your request due to error! Please try again later";
                response.Message.TechnicalMessage = "Error" + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return response;
            }
        }
        
        
        
        private bool DeleteUserRoles(long userId, List<UserRole> roles)
        {
            try
            {

                var sql1 =
                 String.Format(
                     "Delete FROM \"ICASWebAdmin\".\"UserRole\"  WHERE \"UserId\" = {0};", userId);

                if (_repository.RepositoryContext().Database.ExecuteSqlCommand(sql1) < 1)
                {
                    return false;
                }
                var sql2 = new StringBuilder();
                foreach (var item in roles)
                {
                    sql2.AppendLine(
                        string.Format(
                            "Insert into  \"ICASWebAdmin\".\"UserRole\"(\"UserId\", \"RoleId\") Values({0}, {1});", userId,
                            item.RoleId));
                }
                return _repository.RepositoryContext().Database.ExecuteSqlCommand(sql2.ToString()) > 0;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }



        private bool DeleteUserRoles(long userId)
        {
            try
            {

                var sql1 =
                 String.Format(
                     "Delete FROM \"ChurchWebAdmin\".\"UserRole\"  WHERE \"UserId\" = {0};", userId);

                return _repository.RepositoryContext().Database.ExecuteSqlCommand(sql1) >= 1;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        internal UserDeviceResponseObj AddUserDevice(UserDeviceRegObj agentUser)
        {
            var response = new UserDeviceResponseObj
            {
                UserId = agentUser.UserId,
                MobileNumber = agentUser.MobileNumber,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {

                if (agentUser.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to register your device";
                    response.Status.Message.TechnicalMessage = "Invalid device information";
                    return response;
                }
                if (agentUser.UserId < 1)
                {
                    response.Status.Message.FriendlyMessage = "User information is invalid";
                    response.Status.Message.TechnicalMessage = "User information is invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(agentUser, out valResults))
                {
                    var errorDetail = new StringBuilder();
                    if (!valResults.IsNullOrEmpty())
                    {
                        errorDetail.AppendLine("Following error occurred:");
                        valResults.ForEachx(m => errorDetail.AppendLine(m.ErrorMessage));
                    }
                    else
                    {
                        errorDetail.AppendLine("Validation error occurred! Please check all supplied parameters and try again");
                    }
                    response.Status.Message.FriendlyMessage = errorDetail.ToString();
                    response.Status.Message.TechnicalMessage = errorDetail.ToString();
                    response.Status.IsSuccessful = false;
                    return response;
                }

                string msg;

                var authCode = UniqueHashing.GetStandardHash(agentUser.DeviceSerialNumber);
                if (authCode.ToString().Length < 7 || Math.Abs(authCode) < 1)
                {
                    authCode = UniqueHashing.GetStandardHash(agentUser.DeviceSerialNumber + "_" + DateTime.Now.ToString("yyyyMMddhhmmss"));
                    if (authCode.ToString().Length < 7 || Math.Abs(authCode) < 1)
                    {
                        response.Status.Message.FriendlyMessage = "Unable to register your device device! Your device might not be supported";
                        response.Status.Message.TechnicalMessage = "User Device Code is invalid";
                        return response;
                    }
                }

                var thisAuthCode = Math.Abs(authCode).ToString().Substring(0, 6);
                var userDevices = GetUserDevices(agentUser.UserId) ?? new List<UserDevice>();

                if (userDevices.Count > 1)
                {
                    var thisDevice = userDevices[1];
                    thisDevice.DeviceSerialNumber = agentUser.DeviceSerialNumber;
                    thisDevice.DeviceName = agentUser.DeviceName;
                    thisDevice.TimeStampRegistered = DateScrutnizer.CurrentTimeStamp();
                    thisDevice.IsAuthorized = false;
                    thisDevice.NotificationCode = agentUser.NotificationToken;
                    thisDevice.AuthorizationCode = thisAuthCode;

                    var upVal = _deviceRepository.Update(thisDevice);
                    if (upVal.UserDeviceId < 1)
                    {
                        response.Status.Message.FriendlyMessage = "Device Registration Failed! Please try again later";
                        response.Status.Message.TechnicalMessage = "Device Registration Failed";
                        return response;
                    }

                    response.UserDeviceId = upVal.UserDeviceId;
                    response.AuthorizationCode = thisAuthCode;
                    response.Status.IsSuccessful = true;
                    return response;
                }


                using (var db = _uoWork.BeginTransaction())
                {
                    try
                    {
                        var device = new UserDevice
                        {
                            UserId = agentUser.UserId,
                            AuthorizationCode = thisAuthCode,
                            DeviceSerialNumber = agentUser.DeviceSerialNumber,
                            DeviceName = agentUser.DeviceName,
                            TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                            IsAuthorized = false,
                            NotificationCode = "",
                        };

                        var retVal = _deviceRepository.Add(device);
                        _uoWork.SaveChanges();
                        if (retVal.UserDeviceId < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Device Registration Failed! Please try again later";
                            response.Status.Message.TechnicalMessage = "Device Registration Failed";
                            return response;
                        }

                        var lastLogin = GetUserLastLogin(agentUser.UserId, out msg);
                        if (lastLogin == null || lastLogin.UserLoginActivityId < 1 || lastLogin.IsLoggedIn == false)
                        {
                            var token = TransactionRefGenerator.GenerateAccessToken().ToLower();
                            if (string.IsNullOrEmpty(token))
                            {
                                token = TransactionRefGenerator.GenerateAccessToken().ToLower();
                            }
                            if (string.IsNullOrEmpty(token))
                            {
                                db.Rollback();
                                response.Status.Message.FriendlyMessage = "Device Registration Failed! Please try again later";
                                response.Status.Message.TechnicalMessage = "Device Registration Failed";
                                return response;
                            }


                            var userActivity = new UserLoginActivity
                            {
                                IsLoggedIn = true,
                                LoginTimeStamp = DateTime.Now.ToString("yyyy/MM/dd - hh:mm:ss tt"),
                                UserId = agentUser.UserId,
                                LoginSource = UserLoginSource.Mobile,
                                LoginToken = token,
                            };

                            lastLogin = _loginActivityRepository.Add(userActivity);
                            _uoWork.SaveChanges();
                            if (lastLogin == null || lastLogin.UserLoginActivityId < 1)
                            {
                                db.Rollback();
                                response.Status.Message.FriendlyMessage = "Device Registration Failed! Please try again later";
                                response.Status.Message.TechnicalMessage = "Device Registration Failed";
                                return response;
                            }
                        }

                        var authorization = new DeviceAccessAuthorization
                        {
                            UserId = agentUser.UserId,
                            UserDeviceId = retVal.UserDeviceId,
                            AuthorizationToken = lastLogin.LoginToken,
                            AuthorizedDate = DateScrutnizer.GetLocalDate(),
                            AuthorizedDeviceSerialNumber = agentUser.DeviceSerialNumber,
                            AuthorizedTime = DateScrutnizer.GetLocalTime(),
                            IsExpired = false,
                            UserLoginActivityId = lastLogin.UserLoginActivityId,
                        };
                        var retAuth = _authorizationRepository.Add(authorization);
                        _uoWork.SaveChanges();

                        if (retAuth.DeviceAccessAuthorizationId < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Device Registration Failed! Please try again later";
                            response.Status.Message.TechnicalMessage = "Device Registration Failed";
                            return response;
                        }

                        db.Commit();
                        response.UserDeviceId = retVal.UserDeviceId;
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                        response.Status.Message.FriendlyMessage = "Device Registration Failed! Please try again later";
                        response.Status.Message.TechnicalMessage = "Error: " + ex.GetBaseException().Message;
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                }

                response.AuthorizationCode = thisAuthCode;
                response.Status.IsSuccessful = true;
                return response;
            }
            catch (DbEntityValidationException ex)
            {
                response.Status.Message.FriendlyMessage = "Device Registration Failed! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.Message;
                response.Status.IsSuccessful = false;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Device Registration Failed! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.Message;
                response.Status.IsSuccessful = false;
                return response;
            }
        }

        internal UserDeviceResponseObj AddNotificationToken(UserNotificationTokenObj agentUser)
        {
            var response = new UserDeviceResponseObj
            {
                UserId = agentUser.UserId,
                UserDeviceId = agentUser.UserDeviceId,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {

                if (agentUser.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to register token";
                    response.Status.Message.TechnicalMessage = "Invalid device information";
                    return response;
                }
                if (agentUser.UserId < 1)
                {
                    response.Status.Message.FriendlyMessage = "User information is invalid";
                    response.Status.Message.TechnicalMessage = "User information is invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(agentUser, out valResults))
                {
                    var errorDetail = new StringBuilder();
                    if (!valResults.IsNullOrEmpty())
                    {
                        errorDetail.AppendLine("Following error occurred:");
                        valResults.ForEachx(m => errorDetail.AppendLine(m.ErrorMessage));
                    }
                    else
                    {
                        errorDetail.AppendLine("Validation error occurred! Please check all supplied parameters and try again");
                    }
                    response.Status.Message.FriendlyMessage = errorDetail.ToString();
                    response.Status.Message.TechnicalMessage = errorDetail.ToString();
                    response.Status.IsSuccessful = false;
                    return response;
                }


                var userDevice = GetUserDevice(agentUser.UserId, agentUser.DeviceSerialNumber) ?? new UserDevice();
                if (userDevice.UserDeviceId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Device information is invalid";
                    response.Status.Message.TechnicalMessage = "Device information is invalid";
                    return response;
                }

                userDevice.NotificationCode = agentUser.NotificationToken;

                var upVal = _deviceRepository.Update(userDevice);
                if (upVal.UserDeviceId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Device Update Failed! Please try again later";
                    response.Status.Message.TechnicalMessage = "Device Update Failed";
                    return response;
                }

                response.UserDeviceId = upVal.UserDeviceId;
                response.Status.IsSuccessful = true;
                return response;

            }
            catch (DbEntityValidationException ex)
            {
                response.Status.Message.FriendlyMessage = "Device Update Failed! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.Message;
                response.Status.IsSuccessful = false;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Device Update Failed! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.Message;
                response.Status.IsSuccessful = false;
                return response;
            }
        }

        internal UserDeviceResponseObj AuthorizeUserDevice(UserAuthorizeCodeObj agentUser)
        {
            var response = new UserDeviceResponseObj
            {
                UserId = agentUser.UserId,
                UserDeviceId = agentUser.UserDeviceId,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {

                if (agentUser.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to register token";
                    response.Status.Message.TechnicalMessage = "Invalid device information";
                    return response;
                }
                if (agentUser.UserId < 1)
                {
                    response.Status.Message.FriendlyMessage = "User information is invalid";
                    response.Status.Message.TechnicalMessage = "User information is invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(agentUser, out valResults))
                {
                    var errorDetail = new StringBuilder();
                    if (!valResults.IsNullOrEmpty())
                    {
                        errorDetail.AppendLine("Following error occurred:");
                        valResults.ForEachx(m => errorDetail.AppendLine(m.ErrorMessage));
                    }
                    else
                    {
                        errorDetail.AppendLine("Validation error occurred! Please check all supplied parameters and try again");
                    }
                    response.Status.Message.FriendlyMessage = errorDetail.ToString();
                    response.Status.Message.TechnicalMessage = errorDetail.ToString();
                    response.Status.IsSuccessful = false;
                    return response;
                }


                var userDevice = GetUserDevice(agentUser.UserId, agentUser.DeviceSerialNumber) ?? new UserDevice();
                if (userDevice.UserDeviceId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Device information is invalid";
                    response.Status.Message.TechnicalMessage = "Device information is invalid";
                    return response;
                }

                if (userDevice.AuthorizationCode != agentUser.AuthorizationCode)
                {
                    response.Status.Message.FriendlyMessage = "Incorrect Authorization Code";
                    response.Status.Message.TechnicalMessage = "Incorrect Authorization Code";
                    return response;
                }

                userDevice.IsAuthorized = true;

                var upVal = _deviceRepository.Update(userDevice);
                _uoWork.SaveChanges();
                if (upVal.UserDeviceId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Device Update Failed! Please try again later";
                    response.Status.Message.TechnicalMessage = "Device Update Failed";
                    return response;
                }

                response.UserDeviceId = upVal.UserDeviceId;
                response.Status.IsSuccessful = true;
                return response;

            }
            catch (DbEntityValidationException ex)
            {
                response.Status.Message.FriendlyMessage = "Device Update Failed! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.Message;
                response.Status.IsSuccessful = false;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Device Update Failed! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.Message;
                response.Status.IsSuccessful = false;
                return response;
            }
        }

        private bool IsDuplicate(string userName, string email, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ICASWebAdmin\".\"User\"  WHERE lower(\"Username\") = lower('{0}')", userName.Replace("'", "''"));

                List<User> check2;
                if (!string.IsNullOrEmpty(email))
                {
                    var sql2 =
                        string.Format(
                            "Select * FROM \"ICASWebAdmin\".\"User\"  WHERE lower(\"Email\") = lower('{0}')", email);
                    check2 = _repository.RepositoryContext().Database.SqlQuery<User>(sql2).ToList();
                }
                else
                {
                    check2 = null;
                }


                var check = _repository.RepositoryContext().Database.SqlQuery<User>(sql1).ToList();

                msg = "";
                if (check.IsNullOrEmpty() && (check2 == null || check2.IsNullOrEmpty())) return false;
                if (check.Count > 0)
                {
                    msg = "Duplicate Error! Username already exist";
                    return true;
                }
                if (check2 != null)
                {
                    if (check2.Count > 0)
                    {
                        msg = "Duplicate Error! Email already exist";
                        return true;
                    }
                }

                msg = "";
                return false;
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return true;
            }
        }

        internal bool UpdateUser(User agentUser, out string msg)
        {
            try
            {

                agentUser.MobileNumber = CleanMobile(agentUser.MobileNumber);
                var processedUser = _repository.Update(agentUser);
                _uoWork.SaveChanges();
                msg = "";
                return processedUser.UserId > 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }


        private User GetUser(long userId, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ICASWebAdmin\".\"User\"  WHERE \"UserId\" = {0};", userId);


                var check = _repository.RepositoryContext().Database.SqlQuery<User>(sql1).ToList();


                if (check.IsNullOrEmpty())
                {
                    msg = "No User Record Found!";
                    return null;
                }
                if (check.Count != 1)
                {
                    msg = "Invalid User Record!";
                    return null;
                }
                msg = "";
                return check[0];
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
        
        
        private List<UserLoginActivity> GetUserLogins(int userId, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchWebAdmin\".\"UserLoginActivity\"  WHERE \"UserId\" = {0} ORDER BY \"LoginTimeStamp\";", userId);


                var check = _repository.RepositoryContext().Database.SqlQuery<UserLoginActivity>(sql1).ToList();


                if (check.IsNullOrEmpty())
                {
                    msg = "No Login Record Found!";
                    return null;
                }
                msg = "";
                return check;
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        private UserLoginActivity GetUserLastLogin(long userId, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchWebAdmin\".\"UserLoginActivity\"  WHERE \"UserId\" = {0} ORDER BY \"UserLoginActivityId\" Desc LIMIT 1;", userId);


                var check = _repository.RepositoryContext().Database.SqlQuery<UserLoginActivity>(sql1).ToList();


                if (check.IsNullOrEmpty())
                {
                    msg = "No Login Record Found!";
                    return null;
                }
                if (check.Count != 1)
                {
                    msg = "Invalid Login Record!";
                    return null;
                }
                msg = "";
                return check[0];
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        private bool UpdateRaw(User thisUser, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "UPDATE \"ChurchWebAdmin\".\"User\"  SET  \"Othernames\" = '{0}',  \"MobileNumber\" = '{1}' , \"IsApproved\" = {2}  WHERE \"UserId\" = {3};", thisUser.Othernames, thisUser.MobileNumber, thisUser.IsApproved, thisUser.UserId);


                var check = _repository.RepositoryContext().Database.ExecuteSqlCommand(sql1);
                msg = "";
                return check > 0;
            }
            catch (Exception ex)
            {
                msg = ex.GetFriendlyException("Agent User Information");
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        internal UserRegResponse UpdateUser(UserEditObj agentUser)
        {
            var response = new UserRegResponse
            {
                UserId = 0,
                Email = agentUser.Email,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {

                if (agentUser.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your request";
                    response.Status.Message.TechnicalMessage = "User Object is empty / invalid";
                    return response;
                }
                if (agentUser.UserId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your request";
                    response.Status.Message.TechnicalMessage = "User Object is invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(agentUser, out valResults))
                {
                    var errorDetail = new StringBuilder();
                    if (!valResults.IsNullOrEmpty())
                    {
                        errorDetail.AppendLine("Following error occurred:");
                        valResults.ForEachx(m => errorDetail.AppendLine(m.ErrorMessage));
                    }
                    else
                    {
                        errorDetail.AppendLine("Validation error occurred! Please check all supplied parameters and try again");
                    }
                    response.Status.Message.FriendlyMessage = errorDetail.ToString();
                    response.Status.Message.TechnicalMessage = errorDetail.ToString();
                    response.Status.IsSuccessful = false;
                    return response;
                }

                string msg;
                bool differentRequester = false;
                if (!IsTokenValid(agentUser.RegisteredByUserId, agentUser.RegistrarToken, out msg))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }
                var thisUser = GetUser(agentUser.UserId, out msg);
                if (thisUser == null)
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }
                if (agentUser.RegisteredByUserId == agentUser.UserId)
                {
                    if (thisUser.IsLockedOut || !thisUser.IsApproved)
                    {
                        response.Status.Message.FriendlyMessage =
                            "Sorry, you cannot carry out this request because you are currently locked out";
                        response.Status.Message.TechnicalMessage = "User is locked out";
                        response.Status.IsSuccessful = false;
                        return response;
                    }
                }
                else
                {
                    var requestAgent = GetUser(agentUser.RegisteredByUserId, out msg);
                    if (requestAgent == null)
                    {
                        response.Status.Message.FriendlyMessage =
                          "Sorry, you are not authourized to perform this transaction ";
                        response.Status.Message.TechnicalMessage = "Unauthourized Acces";
                        response.Status.IsSuccessful = false;
                        return response;
                    }
                    if (!requestAgent.IsApproved || requestAgent.IsLockedOut)
                    {
                        response.Status.Message.FriendlyMessage =
                            "Sorry, you cannot carry out this request because you are either not authourized or you are locked out";
                        response.Status.Message.TechnicalMessage = "User is locked out";
                        response.Status.IsSuccessful = false;
                        return response;
                    }
                    differentRequester = true;
                }

                thisUser.Othernames = agentUser.Othernames;
                thisUser.MobileNumber = agentUser.MobileNumber;
                thisUser.Email = agentUser.Email;
                if (differentRequester)
                {
                    thisUser.IsApproved = agentUser.IsApproved;
                }


                if (UpdateRaw(thisUser, out msg))
                {
                    response.UserId = thisUser.UserId;
                    response.Email = thisUser.Email;
                    response.Status.IsSuccessful = true;
                    return response;
                }

                response.Status.Message.FriendlyMessage = "Unable to complete your request! Please try again later";
                response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                response.Status.IsSuccessful = false;
                return response;
            }
            catch (DbEntityValidationException ex)
            {
                response.Status.Message.FriendlyMessage = ex.GetFriendlyException("Agent User Information");
                response.Status.Message.TechnicalMessage = "Error: " + ex.Message;
                response.Status.IsSuccessful = false;
                return response;
            }
            catch (Exception ex)
            {
                response.Status.Message.FriendlyMessage = ex.GetFriendlyException("Agent User Information");
                response.Status.Message.TechnicalMessage = "Error: " + ex.Message;
                response.Status.IsSuccessful = false;
                return response;
            }

        }
        internal bool DeleteUser(long agentUserId)
        {
            try
            {
                if (!DeleteUserRoles(agentUserId))
                {
                    return false;
                }
                var processedUser = _repository.Remove(agentUserId);
                _uoWork.SaveChanges();
                return processedUser.UserId > 0;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        internal RegisteredUserReportObj GetUserObj(long agentUserId)
        {
            try
            {
                var myItem = _repository.GetById(agentUserId);
                if (myItem == null || myItem.UserId < 1) { return null; }
                var m = myItem;
                string msg;
                var roleList = new RoleRepository().GetRolesForUser(m.Username, out msg) ?? new[] { "" };
                var roleIds = new RoleRepository().GetRoleIdsForUser(m.Username, out msg) ?? new[] { 4 };
                return new RegisteredUserReportObj
                {
                    UserId = m.UserId,
                    Othernames = m.Othernames,
                    FailedPasswordCount = m.FailedPasswordCount,
                    Email = m.Email,
                    IsApproved = m.IsApproved,
                    IsLockedOut = m.IsLockedOut,
                    IsPasswordChangeRequired = m.IsPasswordChangeRequired,
                    LastLockedOutTimeStamp = m.LastLockedOutTimeStamp,
                    LastLoginTimeStamp = m.LastLoginTimeStamp,
                    Surname = m.Surname,
                    MobileNumber = m.MobileNumber,
                    PasswordChangeTimeStamp = m.PasswordChangeTimeStamp,
                    Sex = Enum.GetName(typeof(Sex), m.Sex),
                    TimeStampRegistered = m.TimeStampRegistered,
                    Username = m.Username,
                    SelectedRoles = string.Join(",", roleList),
                    MyRoles = roleList,
                    MyRoleIds = roleIds,
                    SexId = m.Sex,
                };
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal List<RegisteredUserReportObj> GetUserObjs()
        {
            try
            {
                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) return null;

                var retList = new List<RegisteredUserReportObj>();
                myItemList.ForEachx(m =>
                {
                    string msg;
                    var roleList = new RoleRepository().GetRolesForUser(m.Username, out msg) ?? new[] { "" };
                    var roleIds = new RoleRepository().GetRoleIdsForUser(m.Username, out msg) ?? new[] { 4 };
                    retList.Add(new RegisteredUserReportObj
                    {
                        UserId = m.UserId,
                        Othernames = m.Othernames,
                        FailedPasswordCount = m.FailedPasswordCount,
                        Email = m.Email,
                        IsApproved = m.IsApproved,
                        IsLockedOut = m.IsLockedOut,
                        IsPasswordChangeRequired = m.IsPasswordChangeRequired,
                        LastLockedOutTimeStamp = m.LastLockedOutTimeStamp,
                        LastLoginTimeStamp = m.LastLoginTimeStamp,
                        Surname = m.Surname,
                        MobileNumber = m.MobileNumber,
                        PasswordChangeTimeStamp = m.PasswordChangeTimeStamp,
                        Sex = Enum.GetName(typeof(Sex), m.Sex),
                        TimeStampRegistered = m.TimeStampRegistered,
                        Username = m.Username,
                        SelectedRoles = string.Join(",", roleList),
                        MyRoles = roleList,
                        MyRoleIds = roleIds,
                        SexId = m.Sex,
                    });
                });

                return retList;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
        
        
        
        internal User GetUser(long agentUserId)
        {
            try
            {
                var myItem = _repository.GetById(agentUserId);
                if (myItem == null || myItem.UserId < 1) { return null; }
                return myItem;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal List<User> GetUsers()
        {
            try
            {
                var myItemList = _repository.GetAll();
                if (myItemList == null || !myItemList.Any()) return null;
                return myItemList.ToList();
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
        internal User GetUserByEmail(string email)
        {
            try
            {
                var myItem = _repository.GetAll(m => string.Compare(m.Email, email, StringComparison.CurrentCultureIgnoreCase) == 0).ToList();
                if (!myItem.Any()) { return null; }
                if (myItem.Any())
                {
                    return myItem[0];
                }
                return myItem[0].UserId < 1 ? null : myItem[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }



        #region USER ADMINISTRATIONS
        private List<UserDevice> GetUserDevices(long userId)
        {

            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchWebAdmin\".\"UserDevice\"  WHERE \"UserId\"= {0}", userId);

                var activity = _repository.RepositoryContext().Database.SqlQuery<UserDevice>(sql1).ToList();
                if (activity.IsNullOrEmpty())
                {
                    return null;
                }
                return activity;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }

        }
        private UserDevice GetUserDevice(long userId, string deviceId)
        {

            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ICASWebAdmin\".\"UserDevice\"  WHERE \"UserId\"= {0} AND lower(\"DeviceSerialNumber\") = lower('{1}')", userId, deviceId);

                var activity = _repository.RepositoryContext().Database.SqlQuery<UserDevice>(sql1).ToList();
                if (activity.IsNullOrEmpty())
                {
                    return null;
                }
                return activity.Count != 1 ? null : activity[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }

        }
        private UserLoginActivity GetUserLoginActivity(long userId, string token)
        {

            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ICASWebAdmin\".\"UserLoginActivity\"  WHERE \"UserId\"= {0} AND  lower(\"LoginToken\") = lower('{1}')", userId, token);

                var activity = _repository.RepositoryContext().Database.SqlQuery<UserLoginActivity>(sql1).ToList();
                if (activity.IsNullOrEmpty())
                {
                    return null;
                }
                return activity.Count != 1 ? null : activity[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }

        }
        private DeviceAccessAuthorization GetDeviceAuthorization(long userId, string token)
        {

            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ICASWebAdmin\".\"DeviceAccessAuthorization\"  WHERE \"UserId\"= {0} AND lower(\"AuthorizationToken\") = lower('{1}')", userId, token);

                var activity = _repository.RepositoryContext().Database.SqlQuery<DeviceAccessAuthorization>(sql1).ToList();
                if (activity.IsNullOrEmpty())
                {
                    return null;
                }
                return activity.Count != 1 ? null : activity[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }

        }


        internal User GetUser(string username)
        {
            try
            {
                var myItem = _repository.GetAll(m => string.Compare(m.Username, username, StringComparison.CurrentCultureIgnoreCase) == 0).ToList();
                if (!myItem.Any() || myItem.Count() != 1) { return null; }
                return myItem[0].UserId < 1 ? null : myItem[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal AdminTaskResponseObj ResetPassword(string targetUsername)
        {
            var response = new AdminTaskResponseObj
            {
                BeneficiaryUserId = 0,
                NewPassword = "",
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };
            try
            {
                string msg;


                var newPassword = EncryptionHelper.GenerateSalt(8, 10);
                var user = GetUser(targetUsername);
                if (user == null)
                {
                    response.Status.Message.FriendlyMessage = "Target User information is invalid";
                    response.Status.Message.TechnicalMessage = "Target User cannot be found!";
                    return response;
                }

                user.Password = Crypto.GenerateSalt();
                user.UserCode = EncryptionHelper.GenerateSalt(30, 50);
                user.AccessCode = Crypto.HashPassword(newPassword);
                user.PasswordChangeTimeStamp = DateScrutnizer.CurrentTimeStamp();

                if (!UpdateUser(user, out msg))
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Update Failed";
                    return response;
                }

                response.BeneficiaryUserId = user.UserId;
                response.Status.Message.FriendlyMessage = "";
                response.Status.Message.TechnicalMessage = "";
                response.Status.IsSuccessful = true;
                response.NewPassword = newPassword;
                return response;
            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }

        }


        internal AdminTaskResponseObj ResetPassword(long adminUserId, string token, string targetUsername)
        {
            var response = new AdminTaskResponseObj
            {
                AdminUserId = adminUserId,
                BeneficiaryUserId = 0,
                NewPassword = "",
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };
            try
            {
                string msg;
                if (!IsTokenValid(adminUserId, token, out msg))
                {
                    response.Status.Message.FriendlyMessage = "Invalid Access Parameter! Please Re-Login";
                    response.Status.Message.TechnicalMessage = msg;
                    return response;
                }
                var adminUser = GetUser(adminUserId);
                if (adminUser == null)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Request Sender";
                    response.Status.Message.TechnicalMessage = "Unable to retrieve Admin User information";
                    return response;
                }

                var newPassword = EncryptionHelper.GenerateSalt(8, 10);
                var user = GetUser(targetUsername);
                if (user == null)
                {
                    response.Status.Message.FriendlyMessage = "Target User information is invalid";
                    response.Status.Message.TechnicalMessage = "Target User cannot be found!";
                    return response;
                }

                user.Password = Crypto.GenerateSalt();
                user.UserCode = EncryptionHelper.GenerateSalt(30, 50);
                user.AccessCode = Crypto.HashPassword(newPassword);
                user.PasswordChangeTimeStamp = DateScrutnizer.CurrentTimeStamp();

                if (!UpdateUser(user, out msg))
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Update Failed";
                    return response;
                }

                response.AdminUserId = adminUserId;
                response.BeneficiaryUserId = user.UserId;
                response.Status.Message.FriendlyMessage = "";
                response.Status.Message.TechnicalMessage = "";
                response.Status.IsSuccessful = true;
                response.NewPassword = newPassword;
                return response;
            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }

        }
        internal bool IsTokenValid(long userId, string token, out string msg)
        {
            try
            {
                if (string.IsNullOrEmpty(token) || token.Length != 32)
                {
                    msg = "Empty / Invalid Token";
                    return false;
                }
                var activity = GetDeviceAuthorization(userId, token);
                if (activity == null)
                {
                    msg = "Empty / Invalid Token";
                    return false;
                }
                if (activity.DeviceAccessAuthorizationId < 1)
                {
                    msg = "Empty / Invalid Token";
                    return false;
                }
                if (activity.UserDeviceId < 1)
                {
                    msg = "Unauthorized Deviced";
                    return false;
                }
                if (activity.IsExpired)
                {
                    msg = "Session Expired! Please re-login";
                    return false;
                }

                msg = "";
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                msg = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = ex.Message;
                return false;
            }
        }

        internal bool IsTokenValid(long userId, string token, int transactionSource, out string msg)
        {
            try
            {
                if (string.IsNullOrEmpty(token) || token.Length != 32)
                {
                    msg = "Empty / Invalid Token";
                    return false;
                }
                switch (transactionSource)
                {
                    case 1://Mobile
                        var activity = GetDeviceAuthorization(userId, token);
                        if (activity == null)
                        {
                            msg = "Empty / Invalid Token";
                            return false;
                        }
                        if (activity.DeviceAccessAuthorizationId < 1)
                        {
                            msg = "Empty / Invalid Token";
                            return false;
                        }
                        if (activity.UserDeviceId < 1)
                        {
                            msg = "Unauthorized Deviced";
                            return false;
                        }
                        if (activity.IsExpired)
                        {
                            msg = "Session Expired! Please re-login";
                            return false;
                        }
                        break;
                    case 2: //Web
                        var loginActivity = GetUserLoginActivity(userId, token);
                        if (loginActivity == null)
                        {
                            msg = "Empty / Invalid Token";
                            return false;
                        }
                        if (loginActivity.UserLoginActivityId < 1)
                        {
                            msg = "Empty / Invalid Token";
                            return false;
                        }
                        if (!loginActivity.IsLoggedIn)
                        {
                            msg = "Session Expired! Please re-login";
                            return false;
                        }
                        break;
                    default:
                        msg = "Invalid Process Call";
                        return false;
                }


                msg = "";
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                msg = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = ex.Message;
                return false;
            }
        }
        internal bool ResetAdminPassword(string username, out string msg)
        {
            try
            {
                var newPassword = EncryptionHelper.GenerateSalt(8, 10);
                var user = GetUser(username);
                if (user == null)
                {
                    msg = "Invalid Username";
                    return false;
                }

                user.Password = Crypto.GenerateSalt();
                user.UserCode = EncryptionHelper.GenerateSalt(30, 50);
                user.AccessCode = Crypto.HashPassword(newPassword);
                user.PasswordChangeTimeStamp = DateScrutnizer.CurrentTimeStamp();

                if (UpdateUser(user, out msg))
                {
                    msg = newPassword;
                    return true;
                }

                msg = "Process Failed! Unable to Reset User's password";
                return false;
            }
            catch (DbEntityValidationException ex)
            {
                msg = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = "Process Failed! Unable to Reset User's password";
                return false;
            }

        }


        private string RegisterLoginEvent(User user, int loginSource, long deviceId, string deviceSerial, bool success, out string msg)
        {
            try
            {
                var token = TransactionRefGenerator.GenerateAccessToken().ToLower();
                if (string.IsNullOrEmpty(token))
                {
                    token = TransactionRefGenerator.GenerateAccessToken().ToLower();
                }
                if (string.IsNullOrEmpty(token))
                {
                    msg = "Unable to complete login due to error";
                    return "";
                }

                using (var db = _uoWork.BeginTransaction())
                {
                    var userActivity = new UserLoginActivity
                    {
                        IsLoggedIn = success,
                        LoginTimeStamp = DateTime.Now.ToString("yyyy/MM/dd - hh:mm:ss tt"),
                        UserId = user.UserId,
                        LoginSource = (UserLoginSource)loginSource,
                        LoginToken = token,
                        LoginAddress = deviceSerial
                    };
                    var retVal = _loginActivityRepository.Add(userActivity);
                    _uoWork.SaveChanges();
                    if (retVal.UserLoginActivityId < 1)
                    {
                        db.Rollback();
                        msg = "Unable to complete login due to error";
                        return "";
                    }
                    if (!success)
                    {
                        if (user.FailedPasswordCount >= 5)
                        {
                            user.IsLockedOut = true;
                            user.IsApproved = false;
                            user.LastLockedOutTimeStamp = DateScrutnizer.CurrentTimeStamp();

                            // If user enter wrong password up to 5 or more times, then the system lock him/her out automatically
                            var retUser = _repository.Update(user);
                            _uoWork.SaveChanges();
                            if (retUser.UserId < 1)
                            {
                                db.Rollback();
                                msg = "Unable to complete login due to error";
                                return "";
                            }
                        }
                        else
                        {
                            user.FailedPasswordCount += 1;
                            var retUser = _repository.Update(user);
                            _uoWork.SaveChanges();
                            if (retUser.UserId < 1)
                            {
                                db.Rollback();
                                msg = "Unable to complete login due to error";
                                return "";
                            }
                        }
                    }
                    if (success)
                    {
                        if (deviceId > 0)
                        {
                            var authorization = new DeviceAccessAuthorization
                            {
                                UserId = user.UserId,
                                UserDeviceId = deviceId,
                                AuthorizationToken = token,
                                AuthorizedDate = DateScrutnizer.GetLocalDate(),
                                AuthorizedDeviceSerialNumber = deviceSerial,
                                AuthorizedTime = DateScrutnizer.GetLocalTime(),
                                IsExpired = false,
                                UserLoginActivityId = retVal.UserLoginActivityId,
                            };
                            var retAuth = _authorizationRepository.Add(authorization);
                            _uoWork.SaveChanges();
                            if (retAuth.DeviceAccessAuthorizationId < 1)
                            {
                                db.Rollback();
                                msg = "Unable to complete login due to error";
                                return "";
                            }
                        }

                    }
                    db.Commit();
                }

                msg = "";
                return token;
            }
            catch (DbEntityValidationException ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return "";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return "";
            }
        }


        internal UserResponseObj ChangePassword(string username, string oldPassword, string newPassword, string token)
        {
            var response = new UserResponseObj
            {
                NewPassword = "",
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                UserId = 0,
            };

            try
            {

                var user = GetUser(username);
                if (user == null)
                {
                    response.Status.Message.FriendlyMessage = "Invalid User Information";
                    response.Status.Message.TechnicalMessage = "Invalid User Information";
                    return response;
                }
                if (!user.IsApproved || user.IsLockedOut)
                {
                    response.Status.Message.FriendlyMessage = "You are currently de-activated! You cannot carry out this transaction";
                    response.Status.Message.TechnicalMessage = "User is de-activated";
                    return response;
                }
                string msg;
                if (!IsTokenValid(user.UserId, token, out msg))
                {
                    response.Status.Message.FriendlyMessage = "Invalid Access Parameter! Please Re-Login";
                    response.Status.Message.TechnicalMessage = msg;
                    return response;
                }

                if (!Crypto.VerifyHashedPassword(user.AccessCode, oldPassword))
                {
                    response.Status.Message.FriendlyMessage = "Invalid Old Password";
                    response.Status.Message.TechnicalMessage = "Invalid Old Password";
                    return response;
                }

                user.Password = Crypto.GenerateSalt();
                user.UserCode = EncryptionHelper.GenerateSalt(30, 50);
                user.AccessCode = Crypto.HashPassword(newPassword);
                user.PasswordChangeTimeStamp = DateScrutnizer.CurrentTimeStamp();

                var retVal = UpdateUser(user, out msg);
                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Password Update Failed";
                    return response;
                }

                response.UserId = user.UserId;
                response.Status.Message.FriendlyMessage = "";
                response.Status.Message.TechnicalMessage = "";
                response.Status.IsSuccessful = true;
                response.NewPassword = newPassword;
                return response;
            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }

        }
        
        
        internal UserResponseObj ChangePassword(string username, string oldPassword, string newPassword)
        {
            var response = new UserResponseObj
            {
                NewPassword = "",
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                UserId = 0,
            };

            try
            {

                var user = GetUser(username);
                if (user == null)
                {
                    response.Status.Message.FriendlyMessage = "Invalid User Information";
                    response.Status.Message.TechnicalMessage = "Invalid User Information";
                    return response;
                }
                if (!user.IsApproved || user.IsLockedOut)
                {
                    response.Status.Message.FriendlyMessage = "You are currently de-activated! You cannot carry out this transaction";
                    response.Status.Message.TechnicalMessage = "User is de-activated";
                    return response;
                }
                string msg;

                if (!Crypto.VerifyHashedPassword(user.AccessCode, oldPassword))
                {
                    response.Status.Message.FriendlyMessage = "Invalid Old Password";
                    response.Status.Message.TechnicalMessage = "Invalid Old Password";
                    return response;
                }

                user.Password = Crypto.GenerateSalt();
                user.UserCode = EncryptionHelper.GenerateSalt(30, 50);
                user.AccessCode = Crypto.HashPassword(newPassword);
                user.PasswordChangeTimeStamp = DateScrutnizer.CurrentTimeStamp();

                var retVal = UpdateUser(user, out msg);
                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Password Update Failed";
                    return response;
                }

                response.UserId = user.UserId;
                response.Status.Message.FriendlyMessage = "";
                response.Status.Message.TechnicalMessage = "";
                response.Status.IsSuccessful = true;
                response.NewPassword = newPassword;
                return response;
            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }

        }



        internal UserResponseObj ChangeTransferCode(string username, int oldCode, int newCode, string token)
        {
            var response = new UserResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                NewPassword = "",
                UserId = 0,
            };

            try
            {
                if (oldCode.ToString(CultureInfo.InvariantCulture).Length != 5)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Old Transfer Code";
                    response.Status.Message.TechnicalMessage = "Invalid Old  Transfer Code";
                    return response;
                }
                if (newCode.ToString(CultureInfo.InvariantCulture).Length != 5)
                {
                    response.Status.Message.FriendlyMessage = "Invalid New Transfer Code";
                    response.Status.Message.TechnicalMessage = "Invalid New  Transfer Code";
                    return response;
                }
                if (oldCode == newCode)
                {
                    response.Status.Message.FriendlyMessage = "Old and New Transfer Code canno be equal";
                    response.Status.Message.TechnicalMessage = "Old and New Transfer Code canno be equal";
                    return response;
                }
                var user = GetUser(username);
                if (user == null)
                {
                    response.Status.Message.FriendlyMessage = "Invalid User Information";
                    response.Status.Message.TechnicalMessage = "Invalid User Information";
                    return response;
                }
                if (!user.IsApproved || user.IsLockedOut)
                {
                    response.Status.Message.FriendlyMessage = "You are currently de-activated! You cannot carry out this transaction";
                    response.Status.Message.TechnicalMessage = "User is de-activated";
                    return response;
                }
                string msg;
                if (!IsTokenValid(user.UserId, token, out msg))
                {
                    response.Status.Message.FriendlyMessage = "Invalid Access Parameter! Please Re-Login";
                    response.Status.Message.TechnicalMessage = msg;
                    return response;
                }

                //if (string.IsNullOrEmpty(user.TransferCode) || user.TransferCode.Length != 5 || !DataCheck.IsNumeric(user.TransferCode))
                //{
                //    response.Status.Message.FriendlyMessage = "Invalid Old Transfer Code";
                //     response.Status.Message.TechnicalMessage = "Invalid Old  Transfer Code";
                //    return response;
                //}

                //user.TransferCode = newCode.ToString(CultureInfo.InvariantCulture);

                //var retVal = UpdateUser(user, out msg);
                //if (!retVal)
                //{
                //    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                //     response.Status.Message.TechnicalMessage = "Transfer Code Update Failed";
                //    return response;
                //}

                response.UserId = user.UserId;
                response.Status.Message.FriendlyMessage = "";
                response.Status.Message.TechnicalMessage = "";
                response.Status.IsSuccessful = true;
                response.NewPassword = newCode.ToString(CultureInfo.InvariantCulture);
                return response;
            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }

        }
        internal UserResponseObj ActivateDeActivateUser(long adminUserId, string token, string targetUsername, bool status)
        {
            var response = new UserResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                NewPassword = "",
                UserId = 0,
            };

            try
            {
                string msg;

                if (!IsTokenValid(adminUserId, token, out msg))
                {
                    response.Status.Message.FriendlyMessage = "Invalid Access Parameter! Please Re-Login";
                    response.Status.Message.TechnicalMessage = msg;
                    return response;
                }

                var user = GetUser(adminUserId);
                if (user == null)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Admin User Information";
                    response.Status.Message.TechnicalMessage = "Unable to retrieve Admin User information";
                    return response;
                }
                if (!user.IsApproved || user.IsLockedOut)
                {
                    response.Status.Message.FriendlyMessage = "Admin User is currently de-activated! You cannot perform this transaction";
                    response.Status.Message.TechnicalMessage = "Admin User is disabled";
                    return response;
                }

                var thisUser = GetUser(targetUsername);
                if (thisUser == null)
                {
                    response.Status.Message.FriendlyMessage = "Target User information is invalid";
                    response.Status.Message.TechnicalMessage = "Target User cannot be found!";
                    return response;
                }

                thisUser.IsApproved = status;
                thisUser.IsLockedOut = status;


                var retVal = UpdateUser(thisUser, out msg);

                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Update Failed!";
                    return response;
                }

                response.UserId = thisUser.UserId;
                response.Status.Message.FriendlyMessage = "";
                response.Status.Message.TechnicalMessage = "";
                response.Status.IsSuccessful = true;

                return response;

            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }

        }
        internal UserResponseObj ActivateDeActivateUser(string targetUsername, bool status)
        {
            var response = new UserResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                NewPassword = "",
                UserId = 0,
            };

            try
            {
                string msg;



                var thisUser = GetUser(targetUsername);
                if (thisUser == null)
                {
                    response.Status.Message.FriendlyMessage = "Target User information is invalid";
                    response.Status.Message.TechnicalMessage = "Target User cannot be found!";
                    return response;
                }

                thisUser.IsApproved = status;
                thisUser.IsLockedOut = status;


                var retVal = UpdateUser(thisUser, out msg);

                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Update Failed!";
                    return response;
                }

                response.UserId = thisUser.UserId;
                response.Status.Message.FriendlyMessage = "";
                response.Status.Message.TechnicalMessage = "";
                response.Status.IsSuccessful = true;

                return response;

            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }

        }
        internal UserResponseObj ChangeFirstTimePassword(string username, string oldPassword, string newPassword, string token)
        {
            var response = new UserResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                NewPassword = "",
                UserId = 0,
            };

            try
            {
                var user = GetUser(username);
                if (user == null)
                {
                    response.Status.Message.FriendlyMessage = "Invalid User Information";
                    response.Status.Message.TechnicalMessage = "Invalid User Information";
                    return response;
                }
                if (!user.IsApproved || user.IsLockedOut)
                {
                    response.Status.Message.FriendlyMessage = "You are currently de-activated! You cannot carry out this transaction";
                    response.Status.Message.TechnicalMessage = "User is de-activated";
                    return response;
                }
                string msg;
                if (!IsTokenValid(user.UserId, token, out msg))
                {
                    response.Status.Message.FriendlyMessage = "Invalid Access Parameter! Please Re-Login";
                    response.Status.Message.TechnicalMessage = msg;
                    return response;
                }

                if (!Crypto.VerifyHashedPassword(user.AccessCode, oldPassword))
                {
                    response.Status.Message.FriendlyMessage = "Invalid Old Password";
                    response.Status.Message.TechnicalMessage = "Invalid Old Password";
                    return response;
                }

                user.Password = Crypto.GenerateSalt();
                user.UserCode = EncryptionHelper.GenerateSalt(30, 50);
                user.AccessCode = Crypto.HashPassword(newPassword);
                user.IsFirstTimeLogin = false;
                user.IsPasswordChangeRequired = false;
                user.PasswordChangeTimeStamp = DateScrutnizer.CurrentTimeStamp();

                var retVal = UpdateUser(user, out msg);
                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Password Update Failed";
                    return response;
                }

                response.UserId = user.UserId;
                response.Status.Message.FriendlyMessage = "";
                response.Status.Message.TechnicalMessage = "";
                response.Status.IsSuccessful = true;
                response.NewPassword = newPassword;
                return response;



            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }

        }
        internal UserResponseObj ChangeFirstTimePassword(string username, string oldPassword, string newPassword)
        {
            var response = new UserResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                NewPassword = "",
                UserId = 0,
            };

            try
            {
                var user = GetUser(username);
                if (user == null)
                {
                    response.Status.Message.FriendlyMessage = "Invalid User Information";
                    response.Status.Message.TechnicalMessage = "Invalid User Information";
                    return response;
                }
                if (!user.IsApproved || user.IsLockedOut)
                {
                    response.Status.Message.FriendlyMessage = "You are currently de-activated! You cannot carry out this transaction";
                    response.Status.Message.TechnicalMessage = "User is de-activated";
                    return response;
                }
                string msg;

                if (!Crypto.VerifyHashedPassword(user.AccessCode, oldPassword))
                {
                    response.Status.Message.FriendlyMessage = "Invalid Old Password";
                    response.Status.Message.TechnicalMessage = "Invalid Old Password";
                    return response;
                }

                user.Password = Crypto.GenerateSalt();
                user.UserCode = EncryptionHelper.GenerateSalt(30, 50);
                user.AccessCode = Crypto.HashPassword(newPassword);
                user.IsFirstTimeLogin = false;
                user.IsPasswordChangeRequired = false;
                user.PasswordChangeTimeStamp = DateScrutnizer.CurrentTimeStamp();

                var retVal = UpdateUser(user, out msg);
                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Password Update Failed";
                    return response;
                }

                response.UserId = user.UserId;
                response.Status.Message.FriendlyMessage = "";
                response.Status.Message.TechnicalMessage = "";
                response.Status.IsSuccessful = true;
                response.NewPassword = newPassword;
                return response;

            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }

        }


        private string ValidateUser(User thisUser, string password, int loginSource, string deviceSerial, long deviceId, out string msg)
        {
            try
            {

                if (thisUser == null)
                {
                    msg = "Invalid / Empty User Information";
                    return "";

                }
                if (string.IsNullOrEmpty(deviceSerial))
                {
                    deviceSerial = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.GetValue(0).ToString();

                    if (!string.IsNullOrEmpty(deviceSerial))
                    {
                        if (deviceSerial.Length > 30)
                        {
                            deviceSerial = deviceSerial.Substring(0, 30);
                        }
                    }
                }

                var validated = Crypto.VerifyHashedPassword(thisUser.AccessCode, password);
                var token = RegisterLoginEvent(thisUser, loginSource, deviceId, deviceSerial, validated && thisUser.IsApproved && !thisUser.IsLockedOut, out msg);
                if (!validated)
                {
                    msg = "Invalid Username, Password or both";
                    return "";
                }
                if (!thisUser.IsApproved)
                {
                    if (thisUser.IsLockedOut)
                    {
                        msg = "This user is currently locked out due to several wrong passwords";
                        return "";
                    }

                    msg = "This user is currently de-activated by the admin";
                    return "";
                }
                if (string.IsNullOrEmpty(token))
                {
                    msg = "Invalid Login Process";
                    return "";
                }
                msg = "";
                return token;
            }
            catch (DbEntityValidationException ex)
            {
                msg = ex.Message;
                return "";
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = "Unable to validate user";
                return "";
            }

        }
        internal bool ValidateUser(string username, string password, out string msg)
        {
            try
            {

                if (string.IsNullOrEmpty(username))
                {
                    msg = "Invalid / Empty User Name";
                    return false;

                }

                var thisUser = GetUser(username);
                if (thisUser == null)
                {
                    msg = "Login Failed! Reason: Incorrect Username or Password";
                    return false;
                }

                var validated = Crypto.VerifyHashedPassword(thisUser.AccessCode, password);
                if (!validated)
                {
                    msg = "Invalid Username, Password or both";
                    return false;
                }
                if (!thisUser.IsApproved)
                {
                    if (thisUser.IsLockedOut)
                    {
                        msg = "This user is currently locked out due to several wrong passwords";
                        return false;
                    }

                    msg = "This user is currently disabled by the admin";
                    return false;
                }
                msg = "";
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                msg = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = "Unable to validate user";
                return false;
            }

        }


        internal UserLoginResponseObj LoginUser(string username, string password, int loginSource, string deviceSerial)
        {
            var response = new UserLoginResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                AuthToken = "",
                Email = "",
                MobileNumber = "",
                NewPassword = "",
                Username = username,
                UserId = 0,
            };

            try
            {

                var user = GetUser(username);
                if (user == null)
                {
                    response.Status.Message.FriendlyMessage = "Login Failed! Reason: Incorrect Username or Password";
                    response.Status.Message.TechnicalMessage = "Login Failed! Reason: Incorrect Username or Password";
                    return response;
                }
                var device = new UserDevice();
                if (!string.IsNullOrEmpty(deviceSerial) && loginSource == 1)
                {
                    device = GetUserDevice(user.UserId, deviceSerial) ?? new UserDevice();
                    if (device.UserId < 1)
                    {
                        response.Status.Message.FriendlyMessage = "Invalid Access! Your device must be registered";
                        response.Status.Message.TechnicalMessage = "Invalid Access! Your device must be registered";
                        return response;
                    }

                    if (!device.IsAuthorized)
                    {
                        response.Status.Message.FriendlyMessage = "Your device is not yet authorized! You must authorize your device with the SMS Code sent to your phone";
                        response.Status.Message.TechnicalMessage = "Your device is not yet authorized! You must authorize your device with the SMS Code sent to your phone";
                        return response;
                    }
                }



                string msg;
                var token = ValidateUser(user, password, loginSource, deviceSerial, device.UserDeviceId, out msg);
                if (string.IsNullOrEmpty(token) || token.Length < 20)
                {
                    response.Status.Message.FriendlyMessage = "Login Failed! Reason: " + msg;
                    response.Status.Message.TechnicalMessage = "Login Failed! Reason: " + msg;
                    return response;
                }

                response.IsFirstTimeAccess = user.IsFirstTimeLogin && user.IsPasswordChangeRequired;
                response.Status.Message.FriendlyMessage = "";
                response.Status.Message.TechnicalMessage = "";
                response.MobileNumber = user.MobileNumber;
                response.Email = user.Email;
                response.UserId = user.UserId;
                response.AuthToken = token;
                response.Status.IsSuccessful = true;
                response.Othernames = user.Othernames;
                response.Surname = user.Surname;

                return response;
            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }
        }
        internal UserResponseObj UnlockUser(long adminUserId, string targetUsername, string token)
        {

            var response = new UserResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                NewPassword = "",
                UserId = 0,
            };

            try
            {
                string msg;

                if (!IsTokenValid(adminUserId, token, out msg))
                {
                    response.Status.Message.FriendlyMessage = "Invalid Access Parameter! Please Re-Login";
                    response.Status.Message.TechnicalMessage = msg;
                    return response;
                }

                var user = GetUser(adminUserId);
                if (user == null)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Admin User Information";
                    response.Status.Message.TechnicalMessage = "Unable to retrieve Admin User information";
                    return response;
                }

                if (!user.IsApproved || user.IsLockedOut)
                {
                    response.Status.Message.FriendlyMessage = "Admin User is currently de-activated! You cannot perform this transaction";
                    response.Status.Message.TechnicalMessage = "Admin User is disabled";
                    return response;
                }


                var thisUser = GetUser(targetUsername);
                if (thisUser == null)
                {
                    response.Status.Message.FriendlyMessage = "Target User information is invalid";
                    response.Status.Message.TechnicalMessage = "Target User cannot be found!";
                    return response;
                }



                thisUser.IsLockedOut = false;
                thisUser.FailedPasswordCount = 0;
                thisUser.IsApproved = true;
                thisUser.LastLockedOutTimeStamp = "";

                var retVal = UpdateUser(thisUser, out msg);

                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Update Failed!";
                    return response;
                }

                response.UserId = thisUser.UserId;
                response.Status.Message.FriendlyMessage = "";
                response.Status.Message.TechnicalMessage = "";
                response.Status.IsSuccessful = true;

                return response;

            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }

        }
        internal UserResponseObj UnlockUser(string targetUsername)
        {

            var response = new UserResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                NewPassword = "",
                UserId = 0,
            };

            try
            {
                string msg;

                var thisUser = GetUser(targetUsername);
                if (thisUser == null)
                {
                    response.Status.Message.FriendlyMessage = "Target User information is invalid";
                    response.Status.Message.TechnicalMessage = "Target User cannot be found!";
                    return response;
                }

                thisUser.IsLockedOut = false;
                thisUser.FailedPasswordCount = 0;
                thisUser.IsApproved = true;
                thisUser.LastLockedOutTimeStamp = "";

                var retVal = UpdateUser(thisUser, out msg);

                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Update Failed!";
                    return response;
                }

                response.UserId = thisUser.UserId;
                response.Status.Message.FriendlyMessage = "";
                response.Status.Message.TechnicalMessage = "";
                response.Status.IsSuccessful = true;

                return response;

            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }

        }
        internal UserResponseObj LockUser(long adminUserId, string targetUsername, string token)
        {

            var response = new UserResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                NewPassword = "",
                UserId = 0,
            };

            try
            {
                string msg;

                if (!IsTokenValid(adminUserId, token, out msg))
                {
                    response.Status.Message.FriendlyMessage = "Invalid Access Parameter! Please Re-Login";
                    response.Status.Message.TechnicalMessage = msg;
                    return response;
                }

                var user = GetUser(adminUserId);
                if (user == null)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Admin User Information";
                    response.Status.Message.TechnicalMessage = "Unable to retrieve Admin User information";
                    return response;
                }

                if (!user.IsApproved || user.IsLockedOut)
                {
                    response.Status.Message.FriendlyMessage = "Admin User is currently de-activated! You cannot perform this transaction";
                    response.Status.Message.TechnicalMessage = "Admin User is disabled";
                    return response;
                }


                var thisUser = GetUser(targetUsername);
                if (thisUser == null)
                {
                    response.Status.Message.FriendlyMessage = "Target User information is invalid";
                    response.Status.Message.TechnicalMessage = "Target User cannot be found!";
                    return response;
                }

                thisUser.IsLockedOut = true;
                thisUser.FailedPasswordCount = 0;
                thisUser.LastLockedOutTimeStamp = DateScrutnizer.CurrentTimeStamp();
                thisUser.IsApproved = false;

                var retVal = UpdateUser(thisUser, out msg);

                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Update Failed!";
                    return response;
                }

                response.UserId = thisUser.UserId;
                response.Status.Message.FriendlyMessage = "";
                response.Status.Message.TechnicalMessage = "";
                response.Status.IsSuccessful = true;

                return response;

            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }


        }
        internal UserResponseObj LockUser(string targetUsername)
        {

            var response = new UserResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                NewPassword = "",
                UserId = 0,
            };

            try
            {
                string msg;


                var thisUser = GetUser(targetUsername);
                if (thisUser == null)
                {
                    response.Status.Message.FriendlyMessage = "Target User information is invalid";
                    response.Status.Message.TechnicalMessage = "Target User cannot be found!";
                    return response;
                }

                thisUser.IsLockedOut = true;
                thisUser.FailedPasswordCount = 0;
                thisUser.LastLockedOutTimeStamp = DateScrutnizer.CurrentTimeStamp();
                thisUser.IsApproved = false;

                var retVal = UpdateUser(thisUser, out msg);

                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Update Failed!";
                    return response;
                }

                response.UserId = thisUser.UserId;
                response.Status.Message.FriendlyMessage = "";
                response.Status.Message.TechnicalMessage = "";
                response.Status.IsSuccessful = true;

                return response;

            }
            catch (DbEntityValidationException ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                response.Status.Message.TechnicalMessage = ex.Message;
                return response;
            }


        }
        internal bool IsUsernameDuplicate(string username, out string msg)
        {
            try
            {


                var thisUser = GetUser(username);
                if (thisUser == null)
                {
                    msg = "";
                    return false;
                }
                msg = "Username already exist";
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                msg = ex.Message;
                return true;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = "Unable to validate user";
                return true;
            }

        }
        internal bool IsEmailDuplicate(string email, out string msg)
        {
            try
            {


                var thisUser = GetUserByEmail(email);
                if (thisUser == null)
                {
                    msg = "";
                    return false;
                }
                msg = "Email already exist";
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                msg = ex.Message;
                return true;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                msg = "Unable to validate Email";
                return true;
            }

        }
        #endregion


    }
}
