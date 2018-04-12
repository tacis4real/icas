using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using WebAdminStacks.APIObjs;
using WebAdminStacks.APIs;
using WebAdminStacks.Common;
using WebAdminStacks.DataContract;
using WebAdminStacks.Infrastructure;
using WebAdminStacks.Infrastructure.Contract;
using WebAdminStacks.Repository.Helper;
using WebCribs.TechCracker.WebCribs.TechCracker;
using WebCribs.TechCracker.WebCribs.TechCracker.Controls.CommonEnums;
using WebCribs.TechCracker.WebCribs.TechCracker.GateKeeper;

namespace WebAdminStacks.Repository
{
    internal class ClientAdminRepository
    {


        #region Open Access
        private readonly IWebAdminOpenRepository<ClientChurchProfile> _openHandlerClientChurchRepository;
        private readonly WebAdminOpenUoWork _openUoWork;
        #endregion

        private readonly IWebAdminRepository<ClientProfile> _repository;
        private readonly IWebAdminRepository<ClientChurchProfile> _clientChurchRepository;
        private readonly IWebAdminRepository<ClientChurchRole> _clientChurchRoleRepository; 
        private readonly IWebAdminRepository<ClientRole> _clientRoleRepository; 
        private readonly IWebAdminRepository<ClientLoginActivity> _loginActivityRepository;

        private readonly IWebAdminRepository<ClientChurchLoginActivity> _loginClientActivityRepository;
        private readonly IWebAdminRepository<ClientChurchDeviceAccessAuthorization> _clientAuthorizationRepository;


        private readonly IWebAdminRepository<ClientDeviceAccessAuthorization> _authorizationRepository;
        private readonly IWebAdminRepository<ClientDevice> _deviceRepository; 
        private readonly WebAdminUoWork _uoWork;

        public ClientAdminRepository()
        {
            _uoWork = new WebAdminUoWork();
            _openUoWork = new WebAdminOpenUoWork();
            _repository = new WebAdminRepository<ClientProfile>(_uoWork);
            _clientChurchRepository = new WebAdminRepository<ClientChurchProfile>(_uoWork);
            _loginClientActivityRepository = new WebAdminRepository<ClientChurchLoginActivity>(_uoWork);
            _clientAuthorizationRepository = new WebAdminRepository<ClientChurchDeviceAccessAuthorization>(_uoWork);

            _openHandlerClientChurchRepository = new WebAdminOpenRepository<ClientChurchProfile>(_openUoWork);

            _clientChurchRoleRepository = new WebAdminRepository<ClientChurchRole>(_uoWork);
            _clientRoleRepository = new WebAdminRepository<ClientRole>(_uoWork);
            _loginActivityRepository = new WebAdminRepository<ClientLoginActivity>(_uoWork);
            _authorizationRepository = new WebAdminRepository<ClientDeviceAccessAuthorization>(_uoWork);
            _deviceRepository = new WebAdminRepository<ClientDevice>(_uoWork);
        }



        #region Client Church

        internal ClientChurchProfileRegResponse AddClientChurchProfile(ClientProfileRegistrationObj agentClient)
        {
            var response = new ClientChurchProfileRegResponse
            {
                ClientChurchProfileId = 0,
                Email = agentClient.Email,
                Fullname = agentClient.Fullname,
                MobileNumber = agentClient.MobileNumber,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {
                if (agentClient.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Registration Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(agentClient, out valResults))
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

                agentClient.MobileNumber = CleanMobile(agentClient.MobileNumber);
                string msg;
                if (IsDuplicate(agentClient.Username, agentClient.MobileNumber, out msg, 0))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }

                var clientChurchRoles = GetClientChurchRoles(agentClient.MyRoleIds);
                var clientChurchObj = new ClientChurchProfile
                {
                    ClientChurchId = agentClient.ClientChurchId,
                    Fullname = agentClient.Fullname,
                    Email = agentClient.Email,
                    AccessCode = Crypto.HashPassword(agentClient.Password),
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
                    MobileNumber = CleanMobile(agentClient.MobileNumber),
                    RegisteredByUserId = agentClient.RegisteredByUserId,
                    Password = Crypto.GenerateSalt(),
                    Sex = agentClient.Sex,
                    PasswordChangeTimeStamp = "",
                    UserCode = EncryptionHelper.GenerateSalt(30, 50),
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    Username = agentClient.Username,

                };

                if (!clientChurchRoles.IsNullOrEmpty())
                {
                    clientChurchObj.ClientChurchRoles = clientChurchRoles;
                }

                var processedClientProfile = _clientChurchRepository.Add(clientChurchObj);
                _uoWork.SaveChanges();
                if (processedClientProfile.ClientChurchProfileId > 0)
                {
                    //db.Rollback();
                    response.ClientChurchProfileId = processedClientProfile.ClientChurchProfileId;
                    response.Email = processedClientProfile.Email;
                    response.Status.IsSuccessful = true;
                    return response;
                }

                response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
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

        internal RespStatus UpdateClientChurchProfile(ClientProfileRegistrationObj agentClient)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };
            try
            {
                if (agentClient.Equals(null))
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your registration";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(agentClient, out valResults))
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

                var clientChurchRoles = GetClientChurchRoles(agentClient.MyRoleIds);
                string msg;
                var thisClientChurchProfile = GetClientChurchProfile(agentClient.ClientChurchProfileId, out msg);
                if (thisClientChurchProfile == null || thisClientChurchProfile.Username.Length < 1)
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = string.IsNullOrEmpty(msg) ? "Invalid Client Church Profile Information" : msg;
                    return response;
                }

                using (var db = _uoWork.BeginTransaction())
                {
                    try
                    {
                        if (!DeleteClientChurchProfileRoles(agentClient.ClientChurchProfileId, clientChurchRoles))
                        {
                            db.Rollback();
                            response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                            return response;
                        }

                        //thisClientChurchProfile.Username = agentClient.Username;
                        //thisClientChurchProfile.Password = agentClient.Password;

                        thisClientChurchProfile.Fullname = agentClient.Fullname;
                        thisClientChurchProfile.MobileNumber = agentClient.MobileNumber;
                        thisClientChurchProfile.Sex = agentClient.Sex;
                        thisClientChurchProfile.Email = agentClient.Email;
                        thisClientChurchProfile.FailedPasswordCount = 0;
                        thisClientChurchProfile.IsDeleted = false;
                        thisClientChurchProfile.IsLockedOut = false;
                        thisClientChurchProfile.LastLockedOutTimeStamp = "";
                        thisClientChurchProfile.TimeStampRegistered = DateScrutnizer.CurrentTimeStamp();
                        //thisClientChurchProfile.IsApproved = agentClient.IsActive;
                        var processedClientProfile = _clientChurchRepository.Update(thisClientChurchProfile);
                        _uoWork.SaveChanges();

                        if (processedClientProfile.ClientChurchProfileId < 1)
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

        internal bool UpdateClientChurchProfile(ClientChurchProfile agentUser, out string msg)
        {
            try
            {
                var processedClientProfile = _clientChurchRepository.Update(agentUser);
                _uoWork.SaveChanges();
                msg = "";
                return processedClientProfile.ClientChurchProfileId > 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }



        internal RegisteredClientChurchProfileReportObj GetClientChurchAdminUserProfileObj(long agentUserId)
        {
            try
            {
                var myItem = _clientChurchRepository.GetById(agentUserId);
                if (myItem == null || myItem.ClientChurchProfileId < 1) { return null; }
                var m = myItem;
                string msg;
                var roleList = new RoleClientRepository().GetRolesForClientChurchProfile(m.Username, out msg) ?? new[] { "" };
                var roleIds = new RoleClientRepository().GetRoleIdsForClientChurchProfile(m.Username, out msg) ?? new[] { 1 };

                return new RegisteredClientChurchProfileReportObj
                {
                    ClientChurchProfileId = m.ClientChurchProfileId,
                    Fullname = m.Fullname,
                    FailedPasswordCount = m.FailedPasswordCount,
                    Email = m.Email,
                    IsApproved = m.IsApproved,
                    IsLockedOut = m.IsLockedOut,
                    IsPasswordChangeRequired = m.IsPasswordChangeRequired,
                    LastLockedOutTimeStamp = m.LastLockedOutTimeStamp,
                    LastLoginTimeStamp = m.LastLoginTimeStamp,
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




        #endregion
        


        #region Open Access
        internal IWebAdminOpenRepository<ClientChurchProfile> ClientChurchProfileHandler()
        {
            return _openHandlerClientChurchRepository;
        }

        internal WebAdminOpenUoWork WebAdminOpenUoWork()
        {
            return _openUoWork;
        }
        #endregion
        

        internal ClientProfileRegResponse AddClientProfile(ClientProfileRegObj agentClient)
        {
            var response = new ClientProfileRegResponse
            {
                ClientProfileId = 0,
                Email = agentClient.Email,
                Fullname = agentClient.Fullname,
                MobileNumber = agentClient.MobileNumber,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {
                if (agentClient.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Registration Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(agentClient, out valResults))
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

                agentClient.MobileNumber = CleanMobile(agentClient.MobileNumber);
                string msg;
                if (IsDuplicate(agentClient.Username, agentClient.MobileNumber, out msg, 0))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }

                var clientRoles = GetClientRoles(agentClient.MyRoleIds);
                var clientObj = new ClientProfile
                {
                    ClientId = agentClient.ClientId,
                    Fullname = agentClient.Fullname,
                    Email = agentClient.Email,
                    AccessCode = Crypto.HashPassword(agentClient.Password),
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
                    MobileNumber = CleanMobile(agentClient.MobileNumber),
                    RegisteredByUserId = agentClient.RegisteredByUserId,
                    Password = Crypto.GenerateSalt(),
                    Sex = agentClient.Sex,
                    PasswordChangeTimeStamp = "",
                    UserCode = EncryptionHelper.GenerateSalt(30, 50),
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    Username = agentClient.Username,

                };

                if (!clientRoles.IsNullOrEmpty())
                {
                    clientObj.ClientRoles = clientRoles;
                }

                var processedClientProfile = _repository.Add(clientObj);
                _uoWork.SaveChanges();
                if (processedClientProfile.ClientProfileId > 0)
                {
                    //db.Rollback();
                    response.Email = processedClientProfile.Email;
                    response.Status.IsSuccessful = true;
                    return response;
                }

                response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                response.Status.IsSuccessful = false;
                return response;

                //using (var db = _uoWork.BeginTransaction())
                //{
                //    var clientObj = new ClientProfile
                //    {
                //        ClientId = agentClient.ClientId,
                //        Fullname = agentClient.Fullname,
                //        Email = agentClient.Email,
                //        AccessCode = Crypto.HashPassword(agentClient.Password),
                //        IsFirstTimeLogin = true,
                //        FailedPasswordCount = 0,
                //        IsApproved = true,
                //        IsDeleted = false,
                //        IsLockedOut = false,
                //        LastLockedOutTimeStamp = "",
                //        IsMobileActive = true,
                //        IsPasswordChangeRequired = true,
                //        IsWebActive = true,
                //        IsEmailVerified = true,
                //        IsMobileNumberVerified = true,
                //        LastLoginTimeStamp = "",
                //        MobileNumber = CleanMobile(agentClient.MobileNumber),
                //        RegisteredByUserId = agentClient.RegisteredByUserId,
                //        Password = Crypto.GenerateSalt(),
                //        Sex = agentClient.Sex,
                //        PasswordChangeTimeStamp = "",
                //        UserCode = EncryptionHelper.GenerateSalt(30, 50),
                //        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                //        Username = agentClient.Username,

                //    };

                //    if (clientRoles.IsNullOrEmpty())
                //    {
                //        clientObj.ClientRoles = clientRoles;
                //    }

                //    var processedClientProfile = _repository.Add(clientObj);
                //    _uoWork.SaveChanges();
                //    if (processedClientProfile.ClientProfileId < 1)
                //    {
                //        db.Rollback();
                //        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                //        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                //        response.Status.IsSuccessful = false;
                //        return response;
                //    }

                //    //if (!clientRoles.IsNullOrEmpty())
                //    //{
                //    //    var clientRoleObjs = new List<ClientRole>();
                //    //    clientRoles.ForEachx(x =>
                //    //        clientRoleObjs.Add(new ClientRole
                //    //        {
                //    //            ClientProfileId = processedClientProfile.ClientProfileId,
                //    //            RoleClientId = x.RoleClientId
                //    //        })
                //    //    );


                //    //    //if (
                //    //    //    clientRoleObjs.Select(clientRoleObj => _clientRoleRepository.Add(clientRoleObj))
                //    //    //        .Any(processedClientRole => processedClientRole.ClientRoleId < 1))
                //    //    //{
                //    //    //    db.Rollback();
                //    //    //    response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                //    //    //    response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                //    //    //    response.Status.IsSuccessful = false;
                //    //    //    return response;
                //    //    //}


                //    //    foreach (var clientRoleObj in clientRoleObjs)
                //    //    {
                //    //        //var processedClientRole = _clientRoleRepository.Add(clientRoleObj);
                //    //        var processedClientRole = new ClientRoleRepository().AddClientRole(clientRoleObj);
                //    //        if (processedClientRole < 1)
                //    //        {
                //    //            db.Rollback();
                //    //            response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                //    //            response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                //    //            response.Status.IsSuccessful = false;
                //    //            return response;
                //    //        }
                //    //    }
                //    //}

                //    db.Commit();
                //    response.ClientProfileId = processedClientProfile.ClientProfileId;
                //    response.Fullname = agentClient.Fullname;
                //    response.Email = agentClient.Email;
                //    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                //    response.Status.IsSuccessful = true;
                //    return response;


                //    //if (processedClient.ClientProfileId > 0)
                //    //{
                //    //    response.ClientProfileId = processedClient.ClientProfileId;
                //    //    response.Email = agentClient.Email;
                //    //    response.Status.IsSuccessful = true;
                //    //    return response;
                //    //}
                //}

                
            }
            //catch (DbEntityValidationException ex)
            //{
            //    response.Status.Message.FriendlyMessage = "Unable to complete registration at this time! Please try again later";
            //    response.Status.Message.TechnicalMessage = "Error: " + ex.Message;
            //    response.Status.IsSuccessful = false;
            //    return response;
            //}
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                response.Status.Message.FriendlyMessage = "Unable to complete registration at this time! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.Message;
                response.Status.IsSuccessful = false;
                return response;
            }
        }

        internal ClientProfileRegResponse AddClientProfilex(ClientProfileRegistrationObj agentClient)
        {
            var response = new ClientProfileRegResponse
            {
                ClientProfileId = 0,
                Email = agentClient.Email,
                Fullname = agentClient.Fullname,
                MobileNumber = agentClient.MobileNumber,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {
                if (agentClient.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Registration Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(agentClient, out valResults))
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

                agentClient.MobileNumber = CleanMobile(agentClient.MobileNumber);
                string msg;
                if (IsDuplicate(agentClient.Username, agentClient.MobileNumber, out msg, 0))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }

                //var clientRole = new RoleClient();
                //if (agentClient.MyRoleIds == null)
                //{
                //    clientRole = PortalClientUser.GetClientRoleByName("Pastor", out msg);
                //}
                
                //var clientRole = new Role();
                //if (agentClient.MyRoleIds == null)
                //{
                //    clientRole = ClientUser.GetClientRoleByName("Pastor", out msg);
                //}
                //if (!agentClient.MyRoleIds.Any())
                //{
                //    clientRole = ClientUser.GetClientRoleByName("Pastor", out msg);
                //}
                
                var clientObj = new ClientProfile
                {
                    ClientId = agentClient.ClientChurchId,
                    Fullname = agentClient.Fullname,
                    Email = agentClient.Email,
                    AccessCode = Crypto.HashPassword(agentClient.Password),
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
                    MobileNumber = CleanMobile(agentClient.MobileNumber),
                    RegisteredByUserId = agentClient.RegisteredByUserId,
                    Password = Crypto.GenerateSalt(),
                    Sex = agentClient.Sex,
                    PasswordChangeTimeStamp = "",
                    UserCode = EncryptionHelper.GenerateSalt(30, 50),
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    Username = agentClient.Username,

                };

                //if (clientRole != null)
                //{
                //    clientObj.ClientRoles.Add(new ClientRole{ RoleClientId = clientRole.RoleClientId});
                //    //clientObj.ClientRoles = new Collection<ClientRole>
                //    //{
                //    //    new ClientRole
                //    //    {
                //    //        RoleId = clientRole.RoleId
                //    //    }
                //    //};
                //}

                var processedClient = _repository.Add(clientObj);
                _uoWork.SaveChanges();
                if (processedClient.ClientProfileId > 0)
                {
                    response.ClientProfileId = processedClient.ClientProfileId;
                    response.Email = agentClient.Email;
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

        internal RespStatus UpdateClientProfile(ClientProfileRegObj agentClient)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };
            try
            {
                if (agentClient.Equals(null))
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your registration";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(agentClient, out valResults))
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

                var clientRoles = GetClientRoles(agentClient.MyRoleIds);
                string msg;
                var thisClientProfile = GetClientProfile(agentClient.ClientProfileId, out msg);
                if (thisClientProfile == null || thisClientProfile.Username.Length < 1)
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = string.IsNullOrEmpty(msg) ? "Invalid Client Profile Information" : msg;
                    return response;
                }

                using (var db = _uoWork.BeginTransaction())
                {
                    try
                    {

                        if (!DeleteClientProfileRoles(agentClient.ClientProfileId, clientRoles))
                        {
                            db.Rollback();
                            response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                            return response;
                        }

                        thisClientProfile.Fullname = agentClient.Fullname;
                        thisClientProfile.MobileNumber = agentClient.MobileNumber;
                        thisClientProfile.Sex = agentClient.Sex;
                        thisClientProfile.Email = agentClient.Email;
                        thisClientProfile.FailedPasswordCount = 0;
                        thisClientProfile.IsDeleted = false;
                        thisClientProfile.IsLockedOut = false;
                        thisClientProfile.LastLockedOutTimeStamp = "";
                        thisClientProfile.TimeStampRegistered = DateScrutnizer.CurrentTimeStamp();
                        thisClientProfile.IsApproved = agentClient.IsActive;
                        var processedClientProfile = _repository.Update(thisClientProfile);
                        _uoWork.SaveChanges();

                        if (processedClientProfile.ClientProfileId < 1)
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

        internal bool IsDuplicate(string userName, string mobileNo, out string msg, int status)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ICASWebAdmin\".\"ClientProfile\"  WHERE lower(\"Username\") = lower('{0}')", userName.Replace("'", "''"));

                List<ClientProfile> check2;
                if (!string.IsNullOrEmpty(mobileNo))
                {
                    var sql2 =
                        string.Format(
                            "Select * FROM \"ICASWebAdmin\".\"ClientProfile\"  WHERE \"MobileNumber\" = '{0}'", mobileNo);
                    check2 = _repository.RepositoryContext().Database.SqlQuery<ClientProfile>(sql2).ToList();
                }
                else
                {
                    check2 = null;
                }

                var check = _repository.RepositoryContext().Database.SqlQuery<ClientProfile>(sql1).ToList();

                msg = "";
                if (check.IsNullOrEmpty() && (check2 == null || check2.IsNullOrEmpty())) return false;


                switch (status)
                {
                    case 0:
                        if (check.Count > 0)
                        {
                            msg = "Duplicate Error! Username already exist";
                            return true;
                        }
                        if (check2 != null)
                        {
                            if (check2.Count > 0)
                            {
                                msg = "Duplicate Error! Phone number already exist";
                                return true;
                            }
                        }
                        break;
                        //msg = "";
                        //return false;

                    case 1:
                        if (check.Count > 1)
                        {
                            msg = "Duplicate Error! Username already exist";
                            return true;
                        }
                        if (check2 != null)
                        {
                            if (check2.Count > 1)
                            {
                                msg = "Duplicate Error! Phone number already exist";
                                return true;
                            }
                        }
                        break;
                        //msg = "";
                        //return false;
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

        internal bool UpdateClientProfile(ClientProfile agentUser, out string msg)
        {
            try
            {
                var processedClientProfile = _repository.Update(agentUser);
                _uoWork.SaveChanges();
                msg = "";
                return processedClientProfile.ClientProfileId > 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        private bool DeleteClientProfileRoles(long clientProfileId, List<ClientRole> roles)
        {
            try
            {

                var sql1 =
                 String.Format(
                     "Delete FROM \"ChurchWebAdmin\".\"ClientRole\"  WHERE \"ClientProfileId\" = {0};", clientProfileId);

                if (_repository.RepositoryContext().Database.ExecuteSqlCommand(sql1) < 1)
                {
                    return false;
                }
                var sql2 = new StringBuilder();
                foreach (var item in roles)
                {
                    sql2.AppendLine(
                        string.Format(
                            "Insert into  \"ChurchWebAdmin\".\"ClientRole\"(\"ClientProfileId\", \"RoleClientId\") Values({0}, {1});", clientProfileId,
                            item.RoleClientId));
                }
                return _repository.RepositoryContext().Database.ExecuteSqlCommand(sql2.ToString()) > 0;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        private List<ClientRole> GetClientRoles(int[] roleClientIds)
        {
            var clientRoles = new List<ClientRole>();
            try
            {
                if (roleClientIds == null || !roleClientIds.Any())
                {
                    clientRoles.Add(new ClientRole { RoleClientId = 2 });
                    return clientRoles;
                }
                clientRoles.AddRange(from item in roleClientIds where item >= 1 select new ClientRole { RoleClientId = item });
                return clientRoles;
            }
            catch (Exception)
            {
                clientRoles.Add(new ClientRole { RoleClientId = 4 });
                return clientRoles;
            }
        }

        internal List<RegisteredClientProfileReportObj> GetAllRegisteredClientProfileObjs()
        {
            try
            {
                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) return null;

                var retList = new List<RegisteredClientProfileReportObj>();
                myItemList.ForEachx(m =>
                {
                    string msg;
                    var roleList = new RoleClientRepository().GetRolesForClient(m.Username, out msg) ?? new[] { "" };
                    var roleIds = new RoleClientRepository().GetRoleIdsForClient(m.Username, out msg) ?? new[] { 1 };
                    // How to get Client From ChurchAppStacks
                    //var client = new ClientAdminRepository().

                    retList.Add(new RegisteredClientProfileReportObj
                    {
                        ClientProfileId = m.ClientProfileId,
                        ClientId = m.ClientId,
                        Fullname = m.Fullname,
                        FailedPasswordCount = m.FailedPasswordCount,
                        Email = m.Email,
                        IsApproved = m.IsApproved,
                        IsLockedOut = m.IsLockedOut,
                        IsPasswordChangeRequired = m.IsPasswordChangeRequired,
                        LastLockedOutTimeStamp = m.LastLockedOutTimeStamp,
                        LastLoginTimeStamp = m.LastLoginTimeStamp,
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

                return retList.FindAll(x => x.ClientId > 2);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }





        #region Client Church

        internal List<ClientChurchRole> GetClientChurchRoles(int[] roleClientIds)
        {
            var clientChurchRoles = new List<ClientChurchRole>();
            try
            {
                if (roleClientIds == null || !roleClientIds.Any())
                {
                    clientChurchRoles.Add(new ClientChurchRole { RoleClientId = 2 });
                    return clientChurchRoles;
                }
                clientChurchRoles.AddRange(from item in roleClientIds where item >= 1 select new ClientChurchRole { RoleClientId = item });
                return clientChurchRoles;
            }
            catch (Exception)
            {
                clientChurchRoles.Add(new ClientChurchRole { RoleClientId = 4 });
                return clientChurchRoles;
            }
        }

        private bool DeleteClientChurchProfileRoles(long clientChurchProfileId, IEnumerable<ClientChurchRole> roles)
        {
            try
            {

                var sql1 =
                 String.Format(
                     "Delete FROM \"ICASWebAdmin\".\"ClientChurchRole\"  WHERE \"ClientChurchProfileId\" = {0};", clientChurchProfileId);

                if (_repository.RepositoryContext().Database.ExecuteSqlCommand(sql1) < 1)
                {
                    return false;
                }
                var sql2 = new StringBuilder();
                foreach (var item in roles)
                {
                    sql2.AppendLine(
                        string.Format(
                            "Insert into  \"ICASWebAdmin\".\"ClientChurchRole\"(\"ClientChurchProfileId\", \"RoleClientId\") Values({0}, {1});", clientChurchProfileId,
                            item.RoleClientId));
                }
                return _repository.RepositoryContext().Database.ExecuteSqlCommand(sql2.ToString()) > 0;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        internal bool UpdateClientChurchAdminUser(ClientChurchProfile agentUser, out string msg)
        {
            try
            {

                agentUser.MobileNumber = CleanMobile(agentUser.MobileNumber);
                var processedUser = _clientChurchRepository.Update(agentUser);
                _uoWork.SaveChanges();
                msg = "";
                return processedUser.ClientChurchProfileId > 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        #endregion


        #region Client Church Admin Mgnt

        internal List<RegisteredClientProfileReportObj> GetAllRegisteredClientProfileObjsByClientId(long clientId)
        {
            try
            {
                var items = GetAllRegisteredClientProfileObjs();
                if (!items.Any() || items == null)
                {
                    return new List<RegisteredClientProfileReportObj>();
                }

                return items.FindAll(x => x.ClientId == clientId);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal bool UpdateClientAdminUser(ClientProfile agentUser, out string msg)
        {
            try
            {
                var processedUser = _repository.Update(agentUser);
                _uoWork.SaveChanges();
                msg = "";
                return processedUser.ClientProfileId > 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }




        #endregion







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


        #region CLIENT USERS ADMINISTRATIONS

        #region Client Church

        internal ClientChurchProfile GetClientChurchProfileByClientChurchId(long clientChurchId)
        {
            try
            {
                var myItem = _clientChurchRepository.GetAll(m => m.ClientChurchId == clientChurchId).ToList();
                if (!myItem.Any() || myItem.Count() != 1) { return null; }
                return myItem[0].ClientChurchProfileId < 1 ? null : myItem[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal List<ClientChurchRole> GetClientChurchRolesByClientChurchProfileId(long clientChurchProfileId)
        {
            var clientChurchRoles = new List<ClientChurchRole>();
            try
            {
                if (clientChurchProfileId < 1)
                {
                    //clientChurchRoles.Add(new ClientChurchRole { RoleClientId = 2 });
                    return clientChurchRoles;
                }

                //var myItemList = _repository.GetAll().Where(m => m.UserId == userId);
                //var myItems = _clientChurchRoleRepository.GetAll().ToList();
                //clientChurchRoles = myItems.FindAll(x => x.ClientChurchProfileId == clientChurchProfileId);
                var myItems =
                    _clientChurchRoleRepository.GetAll().Where(x => x.ClientChurchProfileId == clientChurchProfileId).ToList();
                if (!myItems.Any()){ return clientChurchRoles; }

                clientChurchRoles = myItems;
                //clientChurchRoles.AddRange(from item in roleClientIds where item >= 1 select new ClientChurchRole { RoleClientId = item });
                return clientChurchRoles;
            }
            catch (Exception)
            {
                clientChurchRoles.Add(new ClientChurchRole { RoleClientId = 4 });
                return clientChurchRoles;
            }
        }
        
        internal ClientChurchProfile GetClientChurchProfile(string username)
        {
            try
            {
                var myItem = _clientChurchRepository.GetAll(m => string.Compare(m.Username, username, StringComparison.CurrentCultureIgnoreCase) == 0).ToList();
                if (!myItem.Any() || myItem.Count() != 1) { return null; }
                return myItem[0].ClientChurchProfileId < 1 ? null : myItem[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal ClientChurchProfile GetClientChurchProfile(long clientChurchProfileId, out string msg)
        {
            try
            {
                var myItem = _clientChurchRepository.GetById(clientChurchProfileId);
                if (myItem == null || myItem.ClientChurchProfileId < 1)
                {
                    msg = "No Client Church Profile Record Found!";
                    return new ClientChurchProfile();
                }

                msg = "";
                return myItem;
                
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ClientChurchProfile();
            }
        }


        internal ClientChurchResponseObj ChangeClientChurchPassword(string username, string oldPassword, string newPassword)
        {
            var response = new ClientChurchResponseObj
            {
                NewPassword = "",
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                ClientChurchProfileId = 0,
            };

            try
            {

                var clientChurchProfile = GetClientChurchProfile(username);
                if (clientChurchProfile == null)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Client Information";
                    response.Status.Message.TechnicalMessage = "Invalid Client Information";
                    return response;
                }
                if (!clientChurchProfile.IsApproved || clientChurchProfile.IsLockedOut)
                {
                    response.Status.Message.FriendlyMessage = "You are currently de-activated! You cannot carry out this transaction";
                    response.Status.Message.TechnicalMessage = "Client is de-activated";
                    return response;
                }
                string msg;

                if (!Crypto.VerifyHashedPassword(clientChurchProfile.AccessCode, oldPassword))
                {
                    response.Status.Message.FriendlyMessage = "Invalid Old Password";
                    response.Status.Message.TechnicalMessage = "Invalid Old Password";
                    return response;
                }

                clientChurchProfile.Password = Crypto.GenerateSalt();
                clientChurchProfile.UserCode = EncryptionHelper.GenerateSalt(30, 50);
                clientChurchProfile.AccessCode = Crypto.HashPassword(newPassword);
                clientChurchProfile.IsFirstTimeLogin = false;
                clientChurchProfile.IsPasswordChangeRequired = false;
                clientChurchProfile.PasswordChangeTimeStamp = DateScrutnizer.CurrentTimeStamp();

                var retVal = UpdateClientChurchProfile(clientChurchProfile, out msg);
                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Password Update Failed";
                    return response;
                }

                response.ClientChurchProfileId = clientChurchProfile.ClientChurchProfileId;
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


        internal ClientChurchProfile GetClientChurchUserProfile(long clientChurchProfileId)
        {
            try
            {
                var myItem = _clientChurchRepository.GetById(clientChurchProfileId);
                if (myItem == null || myItem.ClientChurchProfileId < 1) { return null; }
                return myItem;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }




        #region Logins

        internal ClientChurchLoginResponseObj LoginClientChurchUser(string username, string password, int loginSource, string deviceSerial)
        {
            var response = new ClientChurchLoginResponseObj
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
                ClientChurchId = 0,
                ClientChurchProfileId = 0,
            };

            try
            {
                var clientChurch = GetClientChurchProfile(username);
                if (clientChurch == null)
                {
                    response.Status.Message.FriendlyMessage = "Login Failed! Reason: Incorrect Username or Password";
                    response.Status.Message.TechnicalMessage = "Login Failed! Reason: Incorrect Username or Password";
                    return response;
                }
                var device = new ClientChurchDevice();
                if (!string.IsNullOrEmpty(deviceSerial) && loginSource == 1)
                {
                    device = GetClientChurchDevice(clientChurch.ClientChurchProfileId, deviceSerial) ?? new ClientChurchDevice();
                    if (device.ClientChurchDeviceId < 1)
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
                var token = ValidateClientChurch(clientChurch, password, loginSource, deviceSerial, device.ClientChurchDeviceId, out msg);
                if (string.IsNullOrEmpty(token) || token.Length < 20)
                {
                    response.Status.Message.FriendlyMessage = "Login Failed! Reason: " + msg;
                    response.Status.Message.TechnicalMessage = "Login Failed! Reason: " + msg;
                    return response;
                }

                response.IsFirstTimeAccess = clientChurch.IsFirstTimeLogin && clientChurch.IsPasswordChangeRequired;
                response.Status.Message.FriendlyMessage = "";
                response.Status.Message.TechnicalMessage = "";
                response.MobileNumber = clientChurch.MobileNumber;
                response.Email = clientChurch.Email;
                response.ClientChurchProfileId = clientChurch.ClientChurchProfileId;
                response.ClientChurchId = clientChurch.ClientChurchId;
                response.AuthToken = token;
                response.Status.IsSuccessful = true;
                response.Fullname = clientChurch.Fullname;

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

        #endregion





        private string ValidateClientChurch(ClientChurchProfile thisClientChurch, string password, int loginSource, string deviceSerial, long deviceId, out string msg)
        {
            try
            {

                if (thisClientChurch == null)
                {
                    msg = "Invalid / Empty Client Information";
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

                var validated = Crypto.VerifyHashedPassword(thisClientChurch.AccessCode, password);
                var token = RegisterClientLoginEvent(thisClientChurch, loginSource, deviceId, deviceSerial, validated && thisClientChurch.IsApproved && !thisClientChurch.IsLockedOut, out msg);
                if (!validated)
                {
                    msg = "Invalid Username, Password or both";
                    return "";
                }
                if (!thisClientChurch.IsApproved)
                {
                    if (thisClientChurch.IsLockedOut)
                    {
                        msg = "This user is currently locked out due to several wrong passwords or by the super admin";
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



        private ClientChurchDevice GetClientChurchDevice(long clientChurchId, string deviceId)
        {

            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ICASWebAdmin\".\"ClientChurchDevice\"  WHERE \"ClientChurchProfileId\"= {0} AND lower(\"DeviceSerialNumber\") = lower('{1}')", clientChurchId, deviceId);

                var activity = _clientChurchRepository.RepositoryContext().Database.SqlQuery<ClientChurchDevice>(sql1).ToList();
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


        private string RegisterClientLoginEvent(ClientChurchProfile clientChurch, int loginSource, long deviceId, string deviceSerial, bool success, out string msg)
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
                    var clientActivity = new ClientChurchLoginActivity
                    {
                        IsLoggedIn = success,
                        LoginTimeStamp = DateTime.Now.ToString("yyyy/MM/dd - hh:mm:ss tt"),
                        ClientChurchProfileId = clientChurch.ClientChurchProfileId,
                        LoginSource = (UserLoginSource)loginSource,
                        LoginToken = token,
                        LoginAddress = deviceSerial
                    };
                    var retVal = _loginClientActivityRepository.Add(clientActivity);
                    _uoWork.SaveChanges();
                    if (retVal.ClientChurchLoginActivityId < 1)
                    {
                        db.Rollback();
                        msg = "Unable to complete login due to error";
                        return "";
                    }
                    if (!success)
                    {
                        if (clientChurch.FailedPasswordCount >= 5)
                        {
                            clientChurch.IsLockedOut = true;
                            clientChurch.IsApproved = false;
                            clientChurch.LastLockedOutTimeStamp = DateScrutnizer.CurrentTimeStamp();

                            // If user enter wrong password up to 5 or more times, then the system lock him/her out automatically
                            var retClient = _clientChurchRepository.Update(clientChurch);
                            _uoWork.SaveChanges();
                            if (retClient.ClientChurchProfileId < 1)
                            {
                                db.Rollback();
                                msg = "Unable to complete login due to error";
                                return "";
                            }
                        }
                        else
                        {
                            clientChurch.FailedPasswordCount += 1;
                            var retUser = _clientChurchRepository.Update(clientChurch);
                            _uoWork.SaveChanges();
                            if (retUser.ClientChurchProfileId < 1)
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
                            var authorization = new ClientChurchDeviceAccessAuthorization
                            {
                                ClientChurchProfileId = clientChurch.ClientChurchProfileId,
                                ClientChurchDeviceId = deviceId,
                                AuthorizationToken = token,
                                AuthorizedDate = DateScrutnizer.GetLocalDate(),
                                AuthorizedDeviceSerialNumber = deviceSerial,
                                AuthorizedTime = DateScrutnizer.GetLocalTime(),
                                IsExpired = false,
                                ClientChurchLoginActivityId = retVal.ClientChurchLoginActivityId,
                            };
                            var retAuth = _clientAuthorizationRepository.Add(authorization);
                            _uoWork.SaveChanges();
                            if (retAuth.ClientChurchDeviceAccessAuthorizationId < 1)
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


        #endregion










        internal ClientLoginResponseObj LoginClientUser(string username, string password, int loginSource, string deviceSerial)
        {
            var response = new ClientLoginResponseObj
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
                ClientId = 0,
                ClientProfileId = 0,
            };

            try
            {
                var client = GetClientProfile(username);
                if (client == null)
                {
                    response.Status.Message.FriendlyMessage = "Login Failed! Reason: Incorrect Username or Password";
                    response.Status.Message.TechnicalMessage = "Login Failed! Reason: Incorrect Username or Password";
                    return response;
                }
                var device = new ClientDevice();
                if (!string.IsNullOrEmpty(deviceSerial) && loginSource == 1)
                {
                    device = GetClientDevice(client.ClientProfileId, deviceSerial) ?? new ClientDevice();
                    if (device.ClientDeviceId < 1)
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
                var token = ValidateClient(client, password, loginSource, deviceSerial, device.ClientDeviceId, out msg);
                if (string.IsNullOrEmpty(token) || token.Length < 20)
                {
                    response.Status.Message.FriendlyMessage = "Login Failed! Reason: " + msg;
                    response.Status.Message.TechnicalMessage = "Login Failed! Reason: " + msg;
                    return response;
                }

                response.IsFirstTimeAccess = client.IsFirstTimeLogin && client.IsPasswordChangeRequired;
                response.Status.Message.FriendlyMessage = "";
                response.Status.Message.TechnicalMessage = "";
                response.MobileNumber = client.MobileNumber;
                response.Email = client.Email;
                response.ClientProfileId = client.ClientProfileId;
                response.ClientId = client.ClientId;
                response.AuthToken = token;
                response.Status.IsSuccessful = true;
                response.Fullname = client.Fullname;

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
        internal ClientResponseObj LockClientAdminUser(string targetUsername)
        {

            var response = new ClientResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                NewPassword = "",
                ClientProfileId = 0,
            };

            try
            {
                string msg;


                var thisClientAdminUser = GetClientProfile(targetUsername);
                if (thisClientAdminUser == null)
                {
                    response.Status.Message.FriendlyMessage = "Target User information is invalid";
                    response.Status.Message.TechnicalMessage = "Target User cannot be found!";
                    return response;
                }

                thisClientAdminUser.IsLockedOut = true;
                thisClientAdminUser.FailedPasswordCount = 0;
                thisClientAdminUser.LastLockedOutTimeStamp = DateScrutnizer.CurrentTimeStamp();
                thisClientAdminUser.IsApproved = false;

                var retVal = UpdateClientAdminUser(thisClientAdminUser, out msg);

                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Update Failed!";
                    return response;
                }

                response.ClientProfileId = thisClientAdminUser.ClientProfileId;
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

        internal ClientResponseObj UnLockClientAdminUser(string targetUsername)
        {

            var response = new ClientResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                NewPassword = "",
                ClientProfileId = 0,
            };

            try
            {
                string msg;


                var thisClientAdminUser = GetClientProfile(targetUsername);
                if (thisClientAdminUser == null)
                {
                    response.Status.Message.FriendlyMessage = "Target User information is invalid";
                    response.Status.Message.TechnicalMessage = "Target User cannot be found!";
                    return response;
                }

                thisClientAdminUser.IsLockedOut = false;
                thisClientAdminUser.FailedPasswordCount = 0;
                thisClientAdminUser.LastLockedOutTimeStamp = "";
                thisClientAdminUser.IsApproved = true;

                var retVal = UpdateClientAdminUser(thisClientAdminUser, out msg);

                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Update Failed!";
                    return response;
                }

                response.ClientProfileId = thisClientAdminUser.ClientProfileId;
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

        internal ClientProfile GetClientProfileByClientId(long clientId)
        {
            try
            {
                var myItem = _repository.GetAll(m => m.ClientId == clientId).ToList();
                if (!myItem.Any() || myItem.Count() != 1) { return null; }
                return myItem[0].ClientProfileId < 1 ? null : myItem[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal ClientProfile GetClientUserProfile(long clientProfileId)
        {
            try
            {
                var myItem = _repository.GetById(clientProfileId);
                if (myItem == null || myItem.ClientProfileId < 1) { return null; }
                return myItem;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
        internal ClientProfile GetClientProfile(string username)
        {
            try
            {
                var myItem = _repository.GetAll(m => string.Compare(m.Username, username, StringComparison.CurrentCultureIgnoreCase) == 0).ToList();
                if (!myItem.Any() || myItem.Count() != 1) { return null; }
                return myItem[0].ClientProfileId < 1 ? null : myItem[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
        internal ClientProfile GetClientProfile(long clientProfileId, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchWebAdmin\".\"ClientProfile\"  WHERE \"ClientProfileId\" = {0};", clientProfileId);


                var check = _repository.RepositoryContext().Database.SqlQuery<ClientProfile>(sql1).ToList();


                if (check.IsNullOrEmpty())
                {
                    msg = "No Client Profile Record Found!";
                    return null;
                }
                if (check.Count != 1)
                {
                    msg = "Invalid Client Profile Record!";
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
        
        internal RegisteredClientProfileReportObj GetClientAdminUserProfileObj(long agentUserId)
        {
            try
            {
                var myItem = _repository.GetById(agentUserId);
                if (myItem == null || myItem.ClientProfileId < 1) { return null; }
                var m = myItem;
                string msg;
                var roleList = new RoleClientRepository().GetRolesForClient(m.Username, out msg) ?? new[] { "" };
                var roleIds = new RoleClientRepository().GetRoleIdsForClient(m.Username, out msg) ?? new[] { 1 };
                
                return new RegisteredClientProfileReportObj
                {
                    ClientProfileId = m.ClientProfileId,
                    Fullname = m.Fullname,
                    FailedPasswordCount = m.FailedPasswordCount,
                    Email = m.Email,
                    IsApproved = m.IsApproved,
                    IsLockedOut = m.IsLockedOut,
                    IsPasswordChangeRequired = m.IsPasswordChangeRequired,
                    LastLockedOutTimeStamp = m.LastLockedOutTimeStamp,
                    LastLoginTimeStamp = m.LastLoginTimeStamp,
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
        

        private ClientDevice GetClientDevice(long clientId, string deviceId)
        {

            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchWebAdmin\".\"ClientDevice\"  WHERE \"ClientProfileId\"= {0} AND lower(\"DeviceSerialNumber\") = lower('{1}')", clientId, deviceId);

                var activity = _repository.RepositoryContext().Database.SqlQuery<ClientDevice>(sql1).ToList();
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
        private string ValidateClient(ClientProfile thisClient, string password, int loginSource, string deviceSerial, long deviceId, out string msg)
        {
            try
            {

                if (thisClient == null)
                {
                    msg = "Invalid / Empty Client Information";
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

                var validated = Crypto.VerifyHashedPassword(thisClient.AccessCode, password);
                var token = RegisterLoginEvent(thisClient, loginSource, deviceId, deviceSerial, validated && thisClient.IsApproved && !thisClient.IsLockedOut, out msg);
                if (!validated)
                {
                    msg = "Invalid Username, Password or both";
                    return "";
                }
                if (!thisClient.IsApproved)
                {
                    if (thisClient.IsLockedOut)
                    {
                        msg = "This user is currently locked out due to several wrong passwords or by the super admin";
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
        private string RegisterLoginEvent(ClientProfile client, int loginSource, long deviceId, string deviceSerial, bool success, out string msg)
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
                    var clientActivity = new ClientLoginActivity
                    {
                        IsLoggedIn = success,
                        LoginTimeStamp = DateTime.Now.ToString("yyyy/MM/dd - hh:mm:ss tt"),
                        ClientProfileId = client.ClientProfileId,
                        LoginSource = (UserLoginSource)loginSource,
                        LoginToken = token,
                        LoginAddress = deviceSerial
                    };
                    var retVal = _loginActivityRepository.Add(clientActivity);
                    _uoWork.SaveChanges();
                    if (retVal.ClientLoginActivityId < 1)
                    {
                        db.Rollback();
                        msg = "Unable to complete login due to error";
                        return "";
                    }
                    if (!success)
                    {
                        if (client.FailedPasswordCount >= 5)
                        {
                            client.IsLockedOut = true;
                            client.IsApproved = false;
                            client.LastLockedOutTimeStamp = DateScrutnizer.CurrentTimeStamp();

                            // If user enter wrong password up to 5 or more times, then the system lock him/her out automatically
                            var retClient = _repository.Update(client);
                            _uoWork.SaveChanges();
                            if (retClient.ClientProfileId < 1)
                            {
                                db.Rollback();
                                msg = "Unable to complete login due to error";
                                return "";
                            }
                        }
                        else
                        {
                            client.FailedPasswordCount += 1;
                            var retUser = _repository.Update(client);
                            _uoWork.SaveChanges();
                            if (retUser.ClientProfileId < 1)
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
                            var authorization = new ClientDeviceAccessAuthorization
                            {
                                ClientProfileId = client.ClientProfileId,
                                ClientDeviceId = deviceId,
                                AuthorizationToken = token,
                                AuthorizedDate = DateScrutnizer.GetLocalDate(),
                                AuthorizedDeviceSerialNumber = deviceSerial,
                                AuthorizedTime = DateScrutnizer.GetLocalTime(),
                                IsExpired = false,
                                ClientLoginActivityId = retVal.ClientLoginActivityId,
                            };
                            var retAuth = _authorizationRepository.Add(authorization);
                            _uoWork.SaveChanges();
                            if (retAuth.ClientDeviceAccessAuthorizationId < 1)
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

        internal ClientResponseObj ChangeClientPassword(string username, string oldPassword, string newPassword)
        {
            var response = new ClientResponseObj
            {
                NewPassword = "",
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                ClientProfileId = 0,
            };

            try
            {

                var clientProfile = GetClientProfile(username);
                if (clientProfile == null)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Client Information";
                    response.Status.Message.TechnicalMessage = "Invalid Client Information";
                    return response;
                }
                if (!clientProfile.IsApproved || clientProfile.IsLockedOut)
                {
                    response.Status.Message.FriendlyMessage = "You are currently de-activated! You cannot carry out this transaction";
                    response.Status.Message.TechnicalMessage = "Client is de-activated";
                    return response;
                }
                string msg;

                if (!Crypto.VerifyHashedPassword(clientProfile.AccessCode, oldPassword))
                {
                    response.Status.Message.FriendlyMessage = "Invalid Old Password";
                    response.Status.Message.TechnicalMessage = "Invalid Old Password";
                    return response;
                }

                clientProfile.Password = Crypto.GenerateSalt();
                clientProfile.UserCode = EncryptionHelper.GenerateSalt(30, 50);
                clientProfile.AccessCode = Crypto.HashPassword(newPassword);
                clientProfile.IsFirstTimeLogin = false;
                clientProfile.IsPasswordChangeRequired = false;
                clientProfile.PasswordChangeTimeStamp = DateScrutnizer.CurrentTimeStamp();

                var retVal = UpdateClientProfile(clientProfile, out msg);
                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Password Update Failed";
                    return response;
                }

                response.ClientProfileId = clientProfile.ClientProfileId;
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


        internal AdminTaskResponseObj ResetChurchAdminUserPassword(string targetUsername)
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
                var churchAdminUser = GetClientProfile(targetUsername);
                if (churchAdminUser == null)
                {
                    response.Status.Message.FriendlyMessage = "Target User information is invalid";
                    response.Status.Message.TechnicalMessage = "Target User cannot be found!";
                    return response;
                }

                churchAdminUser.Password = Crypto.GenerateSalt();
                churchAdminUser.UserCode = EncryptionHelper.GenerateSalt(30, 50);
                churchAdminUser.AccessCode = Crypto.HashPassword(newPassword);
                churchAdminUser.PasswordChangeTimeStamp = DateScrutnizer.CurrentTimeStamp();

                if (!UpdateClientAdminUser(churchAdminUser, out msg))
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Update Failed";
                    return response;
                }

                response.BeneficiaryUserId = churchAdminUser.ClientProfileId;
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


        #endregion





        private bool IsDuplicatex(string userName, string email, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchWebAdmin\".\"ClientProfile\"  WHERE lower(\"Username\") = lower('{0}')", userName.Replace("'", "''"));

                List<ClientProfile> check2;
                if (!string.IsNullOrEmpty(email))
                {
                    var sql2 =
                        string.Format(
                            "Select * FROM \"ChurchWebAdmin\".\"ClientProfile\"  WHERE lower(\"Email\") = lower('{0}')", email);
                    check2 = _repository.RepositoryContext().Database.SqlQuery<ClientProfile>(sql2).ToList();
                }
                else
                {
                    check2 = null;
                }

                var check = _repository.RepositoryContext().Database.SqlQuery<ClientProfile>(sql1).ToList();

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

                msg = "Unable to check duplicate! Please try again later";
                return true;
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return true;
            }
        }

        // private ClientProfile GetClientProfile(ClientRegistrationObj regObj, out bool duplicate, out string msg, int status = 0)
    }
}
