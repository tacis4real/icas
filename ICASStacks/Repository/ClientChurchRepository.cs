using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using ICASStacks.APIObjs;
using ICASStacks.DataContract;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.DataContract.Common;
using ICASStacks.DataContract.Enum;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using ICASStacks.Repository.Helpers;
using ICASStacks.StackService;
using Newtonsoft.Json;
using WebAdminStacks.APIObjs;
using WebAdminStacks.APIs;
using WebAdminStacks.DataContract;
using WebCribs.TechCracker.WebCribs.TechCracker;
using RespMessage = ICASStacks.APIObjs.RespMessage;
using RespStatus = ICASStacks.APIObjs.RespStatus;

namespace ICASStacks.Repository
{
    internal class ClientChurchRepository
    {

        private readonly IIcasRepository<ClientChurch> _repository;
        public readonly IIcasRepository<ChurchStructureParishHeadQuarter> _churchStructureParishHeadQuarterRepository;
        public readonly IIcasRepository<ClientChurchService> _clientChurchServiceRepository;
        public readonly IIcasRepository<ClientChurchCollectionType> _clientChurchCollectionTypeRepository;
        private readonly IIcasRepository<StateOfLocation> _stateOfLocationRepository;
        private readonly IcasUoWork _uoWork;

        #region Calling & Use of Handler from another Context
        //var processedParishHqtr =
        //    new ChurchStructureParishHeadQuarterRepository()._repository.Update(
        //        existingParishHeadQuarter);
        #endregion

        public ClientChurchRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ClientChurch>(_uoWork);
            _churchStructureParishHeadQuarterRepository = new IcasRepository<ChurchStructureParishHeadQuarter>(_uoWork);

            _clientChurchServiceRepository = new IcasRepository<ClientChurchService>(_uoWork);
            _clientChurchCollectionTypeRepository = new IcasRepository<ClientChurchCollectionType>(_uoWork);
            _stateOfLocationRepository = new IcasRepository<StateOfLocation>(_uoWork);
        }



        internal ClientChurchRegResponse AddClientChurch(ClientChurchRegistrationObj clientChurchRegObj)
        {

            var response = new ClientChurchRegResponse
            {
                ClientChurchId = 0,
                ChurchId = clientChurchRegObj.ChurchId,
                Name = clientChurchRegObj.Name,
                Email = clientChurchRegObj.Email,
                PhoneNumber = clientChurchRegObj.PhoneNumber,
                ChurchReferenceNumber = clientChurchRegObj.ChurchReferenceNumber,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {
                #region Model Validation
                if (clientChurchRegObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Church registration Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(clientChurchRegObj, out valResults))
                {
                    var errorDetail = new StringBuilder();
                    if (!valResults.IsNullOrEmpty())
                    {
                        errorDetail.AppendLine("Following error occurred:");
                        valResults.ForEachx(m => errorDetail.AppendLine(m.ErrorMessage));
                    }
                    else
                    {
                        errorDetail.AppendLine(
                            "Validation error occurred! Please check all supplied parameters and try again");
                    }
                    response.Status.Message.FriendlyMessage = errorDetail.ToString();
                    response.Status.Message.TechnicalMessage = errorDetail.ToString();
                    response.Status.IsSuccessful = false;
                    return response;
                }
                #endregion
                
                string msg;
                var clientChurch = ServiceChurch.GetChurch(clientChurchRegObj.ChurchId);
                var crn = GenerateCRN(out msg, clientChurch.ShortName, Convert.ToInt32(clientChurchRegObj.ChurchId));
                if (string.IsNullOrEmpty(crn))
                {
                    response.Status.Message.FriendlyMessage = "Unable to generate CRN";
                    response.Status.Message.TechnicalMessage = "Unable to generate CRN";
                    return null;
                }

                // Valid Admin who carried out this operation
                if (clientChurchRegObj.RegisteredByUserId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Admin User Information";
                    response.Status.Message.TechnicalMessage = "Invalid Admin User Information";
                    response.Status.IsSuccessful = false;
                    return response;
                }

                clientChurchRegObj.PhoneNumber = CleanMobile(clientChurchRegObj.PhoneNumber);
                if (IsDuplicate(clientChurchRegObj.ChurchId, crn, clientChurchRegObj.PhoneNumber, out msg))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }


                #region New Database Storing

                //var clientObj = new Client
                //{
                //    ChurchId = clientRegistrationObj.ChurchId,
                //    Name = clientRegistrationObj.Name,
                //    Pastor = clientRegistrationObj.Pastor,
                //    Title = clientRegistrationObj.Title,
                //    Sex = clientRegistrationObj.Sex,
                //    StateOfLocationId = clientRegistrationObj.StateOfLocationId,
                //    Address = clientRegistrationObj.Address,
                //    Email = clientRegistrationObj.Email,
                //    PhoneNumber = clientRegistrationObj.PhoneNumber,
                //    ChurchReferenceNumber = crn,
                //    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                //    RegisteredByUserId = clientRegistrationObj.RegisteredByUserId,
                //};

                //var processedClient = _repository.Add(clientObj);
                //_uoWork.SaveChanges();
                //if (processedClient.ClientId < 1)
                //{
                //    //db.Rollback();
                //    response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                //    response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                //    response.Status.IsSuccessful = false;
                //    return response;
                //}

                //response.ClientId = processedClient.ClientId;
                //response.Name = clientRegistrationObj.Name;
                //response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                //response.Status.IsSuccessful = true;
                //return response;

                #endregion

                #region Old Database Storing
                
                using (var db = _uoWork.BeginTransaction())
                {

                    #region This CLIENT AS HEADQUARTER

                    //var structureChurchHeadQuarterParishId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture);
                    string hrqtId = null;
                    var clientChurchHeadQuarterChurchStructureTypeId = clientChurchRegObj.ClientChurchHeadQuarterChurchStructureTypeId.GetValueOrDefault();
                    if (clientChurchHeadQuarterChurchStructureTypeId > 0)
                    {
                        
                        #region Compose Fresh or Existing Object From Function
                        
                        int action;
                        var retObj = AddFreshClientChurchStructureParishHeadQuarter(clientChurchHeadQuarterChurchStructureTypeId,
                            clientChurchRegObj.Name, clientChurchRegObj.ChurchId, clientChurchRegObj.StateOfLocationId,
                            out hrqtId, out action);

                        #endregion
                        

                        #region Submit With Repository Handler
                        if (retObj != null)
                        {
                            switch (action)
                            {
                                case 1:

                                    // Call It's Repository Handler
                                    var processedFreshParishHqtr = _churchStructureParishHeadQuarterRepository.Add(retObj);
                                    _uoWork.SaveChanges();
                                    if (processedFreshParishHqtr.ChurchStructureParishHeadQuarterId < 1)
                                    {
                                        db.Rollback();
                                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                                        response.Status.IsSuccessful = false;
                                        return response;
                                    }
                                    break;

                                case 2:

                                    // Call It's Repository Handler
                                    var processedParishHqtr = _churchStructureParishHeadQuarterRepository.Update(retObj);
                                    _uoWork.SaveChanges();
                                    if (processedParishHqtr.ChurchStructureParishHeadQuarterId < 1)
                                    {
                                        db.Rollback();
                                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                                        response.Status.IsSuccessful = false;
                                        return response;
                                    }
                                    break;
                            }
                        }
                        #endregion
                    }

                    #region Old Processing

                    //var structureChurchHeadQuarterParishId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture);
                    //if (clientChurchHeadQuarterChurchStructureTypeId > 0)
                    //{
                    //    var churchStructureTypeId = clientChurchRegObj.ClientChurchHeadQuarterChurchStructureTypeId.GetValueOrDefault();
                    //    var churchStructureName = new ChurchStructureTypeRepository().GetChurchStructureTypeNameById(churchStructureTypeId);

                    //    var parishes = new StructureChurchHeadQuarterParish
                    //    {
                    //        StructureChurchHeadQuarterParishId = structureChurchHeadQuarterParishId,
                    //        ChurchStructureTypeId = churchStructureTypeId,
                    //        ParishName = clientChurchRegObj.Name,
                    //        ChurchStructureTypeName = churchStructureName
                    //    };


                    //    var thisChurchStructureParishHeadQuarter = new ChurchStructureParishHeadQuarterRegObj
                    //    {
                    //        ChurchId = clientChurchRegObj.ChurchId,
                    //        StateOfLocationId = clientChurchRegObj.StateOfLocationId,
                    //        ChurchStructureTypeId = churchStructureTypeId,
                    //        Parish = new List<StructureChurchHeadQuarterParish> { parishes },
                    //        RegisteredByUserId = clientChurchRegObj.RegisteredByUserId,
                    //        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp()
                    //    };


                    //    #region Submit With Repository Handler

                    //    // Compose ChurchStructureParishHeadQuarter Obj with All Properties


                    //    // Check Exist of Composed Object to determine UPDATE or ADD Call with Handler
                    //    var churchId = thisChurchStructureParishHeadQuarter.ChurchId;
                    //    var state = thisChurchStructureParishHeadQuarter.StateOfLocationId;
                    //    var typeId = thisChurchStructureParishHeadQuarter.ChurchStructureTypeId;
                    //    var existingParishHeadQuarter = ServiceChurch.IsChurchStructureParishHeadQuarterExist(churchId, state, typeId);
                    //    if (existingParishHeadQuarter != null && existingParishHeadQuarter.ChurchStructureParishHeadQuarterId > 0)
                    //    {
                    //        var existingParish = existingParishHeadQuarter.Parish;
                    //        existingParish.AddRange(thisChurchStructureParishHeadQuarter.Parish);
                    //        existingParishHeadQuarter._Parish = JsonConvert.SerializeObject(existingParish);

                    //        // Call It's Repository Handler
                    //        var processedParishHqtr = _churchStructureParishHeadQuarterRepository.Update(existingParishHeadQuarter);
                    //        _uoWork.SaveChanges();
                    //        if (processedParishHqtr.ChurchStructureParishHeadQuarterId < 1)
                    //        {
                    //            db.Rollback();
                    //            response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                    //            response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                    //            response.Status.IsSuccessful = false;
                    //            return response;
                    //        }
                    //    }
                    //    else
                    //    {

                    //        var churchStructureParishHeadQuarter = new ChurchStructureParishHeadQuarter
                    //        {
                    //            ChurchId = clientChurchRegObj.ChurchId,
                    //            StateOfLocationId = clientChurchRegObj.StateOfLocationId,
                    //            ChurchStructureTypeId = churchStructureTypeId,
                    //            Parish = new List<StructureChurchHeadQuarterParish> { parishes },
                    //            RegisteredByUserId = clientChurchRegObj.RegisteredByUserId,
                    //            TimeStampRegistered = DateScrutnizer.CurrentTimeStamp()
                    //        };

                    //        // Call It's Repository Handler
                    //        var processedParishHqtr = _churchStructureParishHeadQuarterRepository.Add(churchStructureParishHeadQuarter);
                    //        _uoWork.SaveChanges();
                    //        if (processedParishHqtr.ChurchStructureParishHeadQuarterId < 1)
                    //        {
                    //            db.Rollback();
                    //            response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                    //            response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                    //            response.Status.IsSuccessful = false;
                    //            return response;
                    //        }
                    //    }

                    //    #endregion

                    //}


                    #endregion
                    

                    #endregion

                    #region Client Self


                    #region HeadQuarters Details

                    string _parish = null;
                    var clientHeadQuarters = clientChurchRegObj.ChurchStructureParishHeadQuarters;
                    if (clientHeadQuarters != null)
                    {
                        var parishHeadQuarters = new ChurchStructureParishHeadQuarterRepository().GetStructureParishHeadQuartersByIds(
                            clientChurchRegObj.ChurchId, clientChurchRegObj.StateOfLocationId, clientHeadQuarters.ToList());

                        _parish = JsonConvert.SerializeObject(parishHeadQuarters);
                    }

                    #endregion

                    #region Client Account

                    //clientChurchRegObj.ClientChurchAccount.BankId = clientChurchRegObj.BankId;
                    //clientChurchRegObj.ClientChurchAccount.AccountTypeId = clientChurchRegObj.AccountTypeId;

                    #endregion

                    var clientChurchObj = new ClientChurch
                    {
                        ChurchId = clientChurchRegObj.ChurchId,
                        Name = clientChurchRegObj.Name,
                        Pastor = clientChurchRegObj.Pastor,
                        Title = clientChurchRegObj.Title,
                        Sex = clientChurchRegObj.Sex,
                        StateOfLocationId = clientChurchRegObj.StateOfLocationId,
                        Address = clientChurchRegObj.Address,
                        ////StructureChurchHeadQuarterParishId = structureChurchHeadQuarterParishId, 
                        StructureChurchHeadQuarterParishId = hrqtId, 
                        Email = clientChurchRegObj.Email,
                        PhoneNumber = clientChurchRegObj.PhoneNumber,
                        ChurchReferenceNumber = crn,
                        _Account = JsonConvert.SerializeObject(clientChurchRegObj.ClientChurchAccount),
                        _Parish = _parish,
                        //_Parish = JsonConvert.SerializeObject(parishHeadQuarters),
                        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                        RegisteredByUserId = clientChurchRegObj.RegisteredByUserId,
                    };

                    var processedClient = _repository.Add(clientChurchObj);
                    _uoWork.SaveChanges();
                    if (processedClient.ClientChurchId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    #endregion

                    #region Client Profile

                    clientChurchRegObj.ClientChurchProfile.ClientChurchId = processedClient.ClientChurchId;
                    //clientChurchRegObj.ClientChurchProfile.ClientId = 3;

                    var processedClientChurchProfile = PortalClientUser.AddClientChurchProfile(clientChurchRegObj.ClientChurchProfile);
                    if (processedClientChurchProfile.ClientChurchProfileId < 1)
                    {
                        var retMsg = processedClientChurchProfile.Status.Message;
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = retMsg.FriendlyMessage.IsNullOrEmpty()
                            ? "Unable to complete registration! Please try again later"
                            : retMsg.FriendlyMessage;
                        response.Status.Message.TechnicalMessage = retMsg.FriendlyMessage.IsNullOrEmpty()
                            ? "Process Failed! Unable to save data" : retMsg.TechnicalMessage;
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    #region Submit Client Profile With Handler


                    #region Compose Client Church Profile

                    //var agentClient = clientChurchRegObj.ClientChurchProfile;

                    //var clientChurchRoles = PortalClientUser.GetClientChurchRoles(agentClient.MyRoleIds);
                    //var clientChurchProfileObj = new ClientChurchProfile
                    //{
                    //    ClientChurchId = agentClient.ClientId,
                    //    Fullname = agentClient.Fullname,
                    //    Email = agentClient.Email,
                    //    AccessCode = Crypto.HashPassword(agentClient.Password),
                    //    IsFirstTimeLogin = true,
                    //    FailedPasswordCount = 0,
                    //    IsApproved = true,
                    //    IsDeleted = false,
                    //    IsLockedOut = false,
                    //    LastLockedOutTimeStamp = "",
                    //    IsMobileActive = true,
                    //    IsPasswordChangeRequired = true,
                    //    IsWebActive = true,
                    //    IsEmailVerified = true,
                    //    IsMobileNumberVerified = true,
                    //    LastLoginTimeStamp = "",
                    //    MobileNumber = CleanMobile(agentClient.MobileNumber),
                    //    RegisteredByUserId = agentClient.RegisteredByUserId,
                    //    Password = Crypto.GenerateSalt(),
                    //    Sex = agentClient.Sex,
                    //    PasswordChangeTimeStamp = "",
                    //    UserCode = EncryptionHelper.GenerateSalt(30, 50),
                    //    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    //    Username = agentClient.Username,

                    //};

                    //if (!clientChurchRoles.IsNullOrEmpty())
                    //{
                    //    clientChurchProfileObj.ClientChurchRoles = clientChurchRoles;
                    //}
                    #endregion

                    //var processedClientChurchProfile = PortalClientUser.ClientChurchProfileHandler().Add(clientChurchProfileObj);
                    //PortalClientUser.WebAdminOpenUoWork().SaveChanges();

                    //if (processedClientChurchProfile.ClientChurchProfileId < 1)
                    //{
                    //    db.Rollback();
                    //    response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                    //    response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                    //    response.Status.IsSuccessful = false;
                    //    return response;
                    //}


                    #endregion
                    
                    #endregion


                    #region Client Church Service

                    string _ServiceTypeDetail = null;
                    var churchServiceTypeDetail =
                        new ChurchServiceRepository().GetChurchServiceServiceTypeDetail(clientChurchRegObj.ChurchId);
                    if (churchServiceTypeDetail.Count > 0 && churchServiceTypeDetail.Any())
                    {

                        foreach (var churchServiceDetailObj in churchServiceTypeDetail)
                        {
                            churchServiceDetailObj.ChurchServiceTypeRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture);
                            Thread.Sleep(3);
                        }
                        _ServiceTypeDetail = JsonConvert.SerializeObject(churchServiceTypeDetail);
                    }

                    var clientChurchService = new ClientChurchService
                    {
                        ClientChurchId = processedClient.ClientChurchId,
                        _ServiceTypeDetail = _ServiceTypeDetail,
                        AddedByUserId = clientChurchRegObj.RegisteredByUserId,
                        TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                    };

                    var processedClientChurchService = _clientChurchServiceRepository.Add(clientChurchService);
                    _uoWork.SaveChanges();
                    if (processedClientChurchService.ClientChurchServiceId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }
                    #endregion

                    #region Client Church Collection

                    string _CollectionTypeDetail = null;
                    var churchCollectionTypeDetail =
                        new ChurchCollectionTypeRepository().GetChurchCollectionTypeDetail(clientChurchRegObj.ChurchId);
                    if (churchCollectionTypeDetail.Count > 0 && churchCollectionTypeDetail.Any())
                    {
                        foreach (var collectionTypeObj in churchCollectionTypeDetail)
                        {
                            collectionTypeObj.CollectionRefId =
                                UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture);
                            Thread.Sleep(3);
                        }

                        _CollectionTypeDetail = JsonConvert.SerializeObject(churchCollectionTypeDetail);
                    }

                    var clientChurchCollectionType = new ClientChurchCollectionType
                    {
                        ClientChurchId = processedClient.ClientChurchId,
                        _Collection = _CollectionTypeDetail,
                        AddedByUserId = clientChurchRegObj.RegisteredByUserId,
                        TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                    };

                    var processedClientChurchCollectionType = _clientChurchCollectionTypeRepository.Add(clientChurchCollectionType);
                    _uoWork.SaveChanges();
                    if (processedClientChurchCollectionType.ClientChurchCollectionTypeId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    #endregion


                    db.Commit();
                    response.ClientChurchId = processedClient.ClientChurchId;
                    response.Name = clientChurchRegObj.Name;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;



                    #region Client Contact & Account

                    // Submit Client Contact
                    //var thisClientContact = new ClientContact
                    //{
                    //    ClientId = processedClient.ClientId,
                    //    Address = clientRegistrationObj.Address,
                    //    StateOfLocationId = clientRegistrationObj.StateOfLocationId,
                    //    Email = clientRegistrationObj.Email,
                    //    PhoneNumber = clientRegistrationObj.PhoneNumber,
                    //    RegisteredByUserId = clientRegistrationObj.RegisteredByUserId, // Coming back to this
                    //    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp()
                    //};

                    //var processedClientContact = _clientContactRepository.Add(thisClientContact);
                    //_uoWork.SaveChanges();
                    //if (processedClientContact.ClientContactId < 1)
                    //{
                    //    db.Rollback();
                    //    response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                    //    response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                    //    response.Status.IsSuccessful = false;
                    //    return response;
                    //}

                    // Submit Client Account
                    //var thisClientAccount = new ClientAccount
                    //{
                    //    ClientId = processedClient.ClientId,
                    //    BankId = clientRegistrationObj.Account.BankId,
                    //    AccountName = clientRegistrationObj.Account.AccountName,
                    //    AccountNumber = clientRegistrationObj.Account.AccountNumber,
                    //    AccountTypeId = clientRegistrationObj.Account.AccountTypeId,
                    //    RegisteredByUserId = clientRegistrationObj.RegisteredByUserId, // Coming back to this
                    //    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp()
                    //};

                    //var processedClientAccount = _clientAccountRepository.Add(thisClientAccount);
                    //_uoWork.SaveChanges();
                    //if (processedClientAccount.ClientAccountId < 1)
                    //{
                    //    db.Rollback();
                    //    response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                    //    response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                    //    response.Status.IsSuccessful = false;
                    //    return response;
                    //}

                    #endregion

                    #region Client Church Structure Details

                    //var clientStructureDetails = GetClientStructureDetails(clientRegistrationObj);
                    //if (clientStructureDetails != null)
                    //{
                    //    if (clientStructureDetails.Count > 0)
                    //    {
                    //        foreach (var clientStructureDetail in clientStructureDetails)
                    //        {
                    //            var processedClientChurchStructureDetail = _clientChurchStructureDetailRepository.Add(clientStructureDetail);
                    //            _uoWork.SaveChanges();
                    //            if (processedClientChurchStructureDetail.ClientStructureChurchDetailId < 1)
                    //            {
                    //                db.Rollback();
                    //                response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                    //                response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                    //                response.Status.IsSuccessful = false;
                    //                return response;
                    //            }
                    //        }
                    //    }
                    //}

                    #endregion


                }

                #endregion

            }
            catch (DbEntityValidationException ex)
            {
                response.Status.Message.FriendlyMessage =
                    "Unable to complete registration at this time! Please try again later";
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

        internal RespStatus UpdateClientChurch(ClientChurchRegistrationObj clientChurchRegObj)
        {
            
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };

            try
            {
                if (clientChurchRegObj.Equals(null))
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your registration";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(clientChurchRegObj, out valResults))
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
                
                #region Get This Client Church
                var thisClientChurch = GetClientChurch(clientChurchRegObj.ClientChurchId);
                if (thisClientChurch == null || thisClientChurch.ClientChurchId < 1)
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Invalid User Information";
                    return response;
                }
                #endregion


                clientChurchRegObj.PhoneNumber = CleanMobile(clientChurchRegObj.PhoneNumber);
                using (var db = _uoWork.BeginTransaction())
                {
                    try
                    {

                        #region This CLIENT AS HEADQUARTER
                        //var structureChurchHeadQuarterParishId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture);
                        string updateHrqtId = null;

                        var clientChurchHeadQuarterChurchStructureTypeId = clientChurchRegObj.ClientChurchHeadQuarterChurchStructureTypeId.GetValueOrDefault();
                        #region Not As CLIENT AS HEADQUARTER ANYMORE

                        if (clientChurchHeadQuarterChurchStructureTypeId == 0)
                        {

                            // Get ChurchStructureParishHeadQuarter this Client Stored Before & Remove It
                            var churchId = thisClientChurch.ChurchId;
                            var state = thisClientChurch.StateOfLocationId;
                            var hqtrId = thisClientChurch.StructureChurchHeadQuarterParishId;

                            string msg;
                            var removeThisClientAsHqtr =
                                ServiceChurch.RemoveChurchStructureChurchHeadQuarterParish(churchId, state, hqtrId, out msg);
                            if (!removeThisClientAsHqtr)
                            {
                                db.Rollback();
                                response.Message.FriendlyMessage =
                                    response.Message.TechnicalMessage =
                                        (msg.IsNullOrEmpty() ? "Process Failed! Please try again later" : msg);
                                return response;
                            }

                            // Remove this CLIENT StructureChurchHeadQuarterParishId from ClientChurch Object
                            thisClientChurch.StructureChurchHeadQuarterParishId = null;


                            #region Oldies

                            //var churchStructureTypeId = clientChurchRegObj.ClientChurchHeadQuarterChurchStructureTypeId.GetValueOrDefault();
                            //var churchStructureName = new ChurchStructureTypeRepository().GetChurchStructureTypeNameById(churchStructureTypeId);

                            //var parishes = new StructureChurchHeadQuarterParish
                            //{
                            //    StructureChurchHeadQuarterParishId = structureChurchHeadQuarterParishId,
                            //    ChurchStructureTypeId = churchStructureTypeId,
                            //    ParishName = clientChurchRegObj.Name,
                            //    ChurchStructureTypeName = churchStructureName
                            //};


                            //var thisChurchStructureParishHeadQuarter = new ChurchStructureParishHeadQuarterRegObj
                            //{
                            //    ChurchId = clientChurchRegObj.ChurchId,
                            //    StateOfLocationId = clientChurchRegObj.StateOfLocationId,
                            //    ChurchStructureTypeId = churchStructureTypeId,
                            //    Parish = new List<StructureChurchHeadQuarterParish> { parishes },
                            //    RegisteredByUserId = clientChurchRegObj.RegisteredByUserId,
                            //    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp()
                            //};

                            #endregion

                            #region Submit With Repository Handler

                            // Compose ChurchStructureParishHeadQuarter Obj with All Properties


                            // Check Exist of Composed Object to determine UPDATE or ADD Call with Handler
                            //var churchId = thisChurchStructureParishHeadQuarter.ChurchId;
                            //var state = thisChurchStructureParishHeadQuarter.StateOfLocationId;
                            //var typeId = thisChurchStructureParishHeadQuarter.ChurchStructureTypeId;
                            //var existingParishHeadQuarter = ServiceChurch.IsChurchStructureParishHeadQuarterExist(churchId, state, typeId);
                            //if (existingParishHeadQuarter != null && existingParishHeadQuarter.ChurchStructureParishHeadQuarterId > 0)
                            //{
                            //    var existingParish = existingParishHeadQuarter.Parish;
                            //    existingParish.AddRange(thisChurchStructureParishHeadQuarter.Parish);
                            //    existingParishHeadQuarter._Parish = JsonConvert.SerializeObject(existingParish);

                            //    // Call It's Repository Handler
                            //    var processedParishHqtr = _churchStructureParishHeadQuarterRepository.Update(existingParishHeadQuarter);
                            //    _uoWork.SaveChanges();
                            //    if (processedParishHqtr.ChurchStructureParishHeadQuarterId < 1)
                            //    {
                            //        db.Rollback();
                            //        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                            //        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                            //        response.Status.IsSuccessful = false;
                            //        return response;
                            //    }
                            //}
                            //else
                            //{

                            //    var churchStructureParishHeadQuarter = new ChurchStructureParishHeadQuarter
                            //    {
                            //        ChurchId = clientChurchRegObj.ChurchId,
                            //        StateOfLocationId = clientChurchRegObj.StateOfLocationId,
                            //        ChurchStructureTypeId = churchStructureTypeId,
                            //        Parish = new List<StructureChurchHeadQuarterParish> { parishes },
                            //        RegisteredByUserId = clientChurchRegObj.RegisteredByUserId,
                            //        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp()
                            //    };

                            //    // Call It's Repository Handler
                            //    var processedParishHqtr = _churchStructureParishHeadQuarterRepository.Add(churchStructureParishHeadQuarter);
                            //    _uoWork.SaveChanges();
                            //    if (processedParishHqtr.ChurchStructureParishHeadQuarterId < 1)
                            //    {
                            //        db.Rollback();
                            //        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                            //        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                            //        response.Status.IsSuccessful = false;
                            //        return response;
                            //    }
                            //}

                            #endregion

                        }
                        else
                        {

                            #region ReCompose New CLIENT As HEADQuarter

                            if (clientChurchHeadQuarterChurchStructureTypeId > 0)
                            {

                                #region Compose Fresh or Existing Object From Function

                                int action;
                                
                                var retObj = AddFreshClientChurchStructureParishHeadQuarter(clientChurchHeadQuarterChurchStructureTypeId,
                                    clientChurchRegObj.Name, clientChurchRegObj.ChurchId, clientChurchRegObj.StateOfLocationId,
                                    out updateHrqtId, out action);

                                #endregion


                                #region Submit With Repository Handler
                                if (retObj != null)
                                {
                                    switch (action)
                                    {
                                        case 1:

                                            // Call It's Repository Handler
                                            var processedFreshParishHqtr = _churchStructureParishHeadQuarterRepository.Add(retObj);
                                            _uoWork.SaveChanges();
                                            if (processedFreshParishHqtr.ChurchStructureParishHeadQuarterId < 1)
                                            {
                                                db.Rollback();
                                                response.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                                                response.Message.TechnicalMessage = "Process Failed! Unable to save data";
                                                response.IsSuccessful = false;
                                                return response;
                                            }
                                            break;

                                        case 2:

                                            // Call It's Repository Handler
                                            var processedParishHqtr = _churchStructureParishHeadQuarterRepository.Update(retObj);
                                            _uoWork.SaveChanges();
                                            if (processedParishHqtr.ChurchStructureParishHeadQuarterId < 1)
                                            {
                                                db.Rollback();
                                                response.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                                                response.Message.TechnicalMessage = "Process Failed! Unable to save data";
                                                response.IsSuccessful = false;
                                                return response;
                                            }
                                            break;
                                    }
                                }
                                #endregion
                            }

                            #endregion
                        }

                        #endregion
                        

                        #endregion

                        #region Client Self


                        #region HeadQuarters Details

                        
                        var clientHeadQuarters = clientChurchRegObj.ChurchStructureParishHeadQuarters;
                        string serializeParishHeadQuarters;
                        if (clientHeadQuarters != null)
                        {
                            var parishHeadQuarters =
                                new ChurchStructureParishHeadQuarterRepository().GetStructureParishHeadQuartersByIds(
                                    clientChurchRegObj.ChurchId, clientChurchRegObj.StateOfLocationId,
                                    clientHeadQuarters.ToList());

                            serializeParishHeadQuarters = JsonConvert.SerializeObject(parishHeadQuarters);
                        }
                        else
                        {
                            //serializeParishHeadQuarters = JsonConvert.SerializeObject(null);
                            serializeParishHeadQuarters = null;
                        }

                        

                        #endregion

                        thisClientChurch.ChurchId = clientChurchRegObj.ChurchId;
                        thisClientChurch.Name = clientChurchRegObj.Name;
                        thisClientChurch.Pastor = clientChurchRegObj.Pastor;
                        thisClientChurch.Title = clientChurchRegObj.Title;
                        thisClientChurch.Sex = clientChurchRegObj.Sex;
                        thisClientChurch.StateOfLocationId = clientChurchRegObj.StateOfLocationId;
                        thisClientChurch.Address = clientChurchRegObj.Address;
                        thisClientChurch.Email = clientChurchRegObj.Email;
                        thisClientChurch.PhoneNumber = clientChurchRegObj.PhoneNumber;
                        thisClientChurch._Account = JsonConvert.SerializeObject(clientChurchRegObj.ClientChurchAccount);

                        thisClientChurch.StructureChurchHeadQuarterParishId = updateHrqtId;

                        thisClientChurch._Parish = serializeParishHeadQuarters;

                        var processedClient = _repository.Update(thisClientChurch);
                        _uoWork.SaveChanges();
                        if (processedClient.ClientChurchId < 1)
                        {
                            db.Rollback();
                            response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                            return response;
                        }

                        #endregion
                        
                        #region Client Profile

                        clientChurchRegObj.ClientChurchProfile.ClientChurchId = processedClient.ClientChurchId;
                        //clientChurchRegObj.ClientChurchProfile.ClientId = 3;

                        var processedClientChurchProfile = PortalClientUser.UpdateClientChurchProfile(clientChurchRegObj.ClientChurchProfile);
                        if (!processedClientChurchProfile.IsSuccessful)
                        {
                            var retMsg = processedClientChurchProfile.Message;
                            db.Rollback();
                            response.Message.FriendlyMessage = retMsg.FriendlyMessage.IsNullOrEmpty()
                                ? "Unable to complete registration! Please try again later"
                                : retMsg.FriendlyMessage;
                            response.Message.TechnicalMessage = retMsg.FriendlyMessage.IsNullOrEmpty()
                                ? "Process Failed! Unable to update data" : retMsg.TechnicalMessage;
                            return response;
                        }
                        
                        #endregion
                        

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

        internal List<RegisteredClientChurchReportObj> GetAllRegisteredClientChurchObjs()
        {
            try
            {
                //var myItemLists = _repository.GetAll().ToList();

                var myItemLists = _repository.GetAll(x => x.ChurchId != 1 && x.ChurchId != 2).ToList();
                if (!myItemLists.Any()) return null;

                var retList = new List<RegisteredClientChurchReportObj>();
                
                var selectedParishIds = new string[] {};
                //var clientChurchRoles = new List<ClientChurchRole>();


                #region Old Looping

                //myItemLists.ForEachx(m =>
                //{
                //    string msg;
                //    var clientParentChurch = new ChurchRepository().GetChurch(m.ChurchId);

                //    #region Client Profile

                //    var clientChurchProfile = PortalClientUser.GetClientChurchProfileByClientChurchId(m.ClientChurchId);
                //    //clientChurchRoles = PortalClientUser.GetClientChurchRolesByClientChurchProfileId(clientChurchProfile.ClientChurchProfileId);
                //    var roleList = PortalClientRole.GetRolesForClientChurchProfile(clientChurchProfile.Username, out msg);
                //    var roleIds = PortalClientRole.GetRoleIdsForClientChurchProfile(clientChurchProfile.Username, out msg);
                //    #endregion

                //    #region Client As Headquarter Parish

                //    //m.StateOfLocationId
                //    //m.ChurchId
                //    if (!m.StructureChurchHeadQuarterParishId.IsNullOrEmpty())
                //    {
                //        structureChurchParish = ServiceChurch.GetParishByStructureChurchHeadQuarterParishId(m.ChurchId,
                //        m.StateOfLocationId, m.StructureChurchHeadQuarterParishId);
                //    }

                //    #endregion

                //    #region Compose Return Items

                //    retList.Add(new RegisteredClientChurchReportObj
                //    {
                //        ClientChurchId = m.ClientChurchId,
                //        ChurchId = m.ChurchId,
                //        Name = m.Name,
                //        ParentChurchName = clientParentChurch.Name,
                //        ParentChurchShortName = clientParentChurch.ShortName,
                //        Pastor = m.Pastor,
                //        PhoneNumber = m.PhoneNumber,
                //        Email = m.Email,
                //        Address = m.Address,
                //        State = _stateOfLocationRepository.GetById(m.StateOfLocationId).Name,
                //        StateOfLocationId = m.StateOfLocationId,
                //        TitleName = Enum.GetName(typeof(Title), m.Title),
                //        Title = m.Title,
                //        SexName = Enum.GetName(typeof(Sex), m.Sex),
                //        Sex = m.Sex,
                //        ChurchReferenceNumber = m.ChurchReferenceNumber,
                //        StructureChurchHeadQuarterParishId = m.StructureChurchHeadQuarterParishId,


                //        ClientChurchHeadQuarterChurchStructureTypeId = structureChurchParish.ChurchStructureTypeId,


                //        AccountInfo = m.AccountInfo,
                //        Parishes = m.ClientStructureChurchHeadQuarter,
                //        ClientChurchProfile = new ClientProfileRegistrationObj
                //        {
                //            ClientChurchId = clientChurchProfile.ClientChurchId,
                //            ClientChurchProfileId = clientChurchProfile.ClientChurchProfileId,
                //            Fullname = clientChurchProfile.Fullname,
                //            Username = clientChurchProfile.Username,
                //            MyRoleIds = roleIds,
                //            MyRoles = roleList,
                //            SelectedRoles = string.Join(",", roleList),
                //        },

                //        RegisteredByUserId = m.RegisteredByUserId,
                //        TimeStampRegistered = m.TimeStampRegistered
                //        //ChurchStructureHqtr = (clientChurchStructureHqtr ?? new ChurchStructureHqtr()),
                //        //HeadQuarterChurchStructureTypeId = (clientChurchStructureHqtr != null && clientChurchStructureHqtr.ChurchStructureTypeId > 0 ? clientChurchStructureHqtr.ChurchStructureTypeId : 0),
                //        //ClientStructureChurchDetails = clientStructureChurchDetails

                //    });

                //    #endregion
                    
                //});

                #endregion


                #region New Looping

                foreach (var m in myItemLists)
                {

                    string msg;
                    var clientParentChurch = new ChurchRepository().GetChurch(m.ChurchId);

                    #region Client Profile

                    var clientChurchProfile = PortalClientUser.GetClientChurchProfileByClientChurchId(m.ClientChurchId);
                    var roleList = PortalClientRole.GetRolesForClientChurchProfile(clientChurchProfile.Username, out msg);
                    var roleIds = PortalClientRole.GetRoleIdsForClientChurchProfile(clientChurchProfile.Username, out msg);

                    #endregion

                    #region Load Selected Existing StructureChurchHeadQuarterParish into Array

                    if (m.ClientStructureChurchHeadQuarter != null)
                    {
                        selectedParishIds = m.ClientStructureChurchHeadQuarter.Select(x => x.StructureChurchHeadQuarterParishId).ToArray();
                    }
                    

                    #endregion

                    #region Client As Headquarter Parish

                    var structureChurchParish = new StructureChurchHeadQuarterParish();
                    if (!m.StructureChurchHeadQuarterParishId.IsNullOrEmpty())
                    {
                        structureChurchParish = ServiceChurch.GetParishByStructureChurchHeadQuarterParishId(m.ChurchId,
                        m.StateOfLocationId, m.StructureChurchHeadQuarterParishId);
                    }

                    #endregion
                    

                    #region Compose Return Items

                    retList.Add(new RegisteredClientChurchReportObj
                    {
                        ClientChurchId = m.ClientChurchId,
                        ChurchId = m.ChurchId,
                        Name = m.Name,
                        ParentChurchName = clientParentChurch.Name,
                        ParentChurchShortName = clientParentChurch.ShortName,
                        Pastor = m.Pastor,
                        PhoneNumber = m.PhoneNumber,
                        Email = m.Email,
                        Address = m.Address,
                        State = _stateOfLocationRepository.GetById(m.StateOfLocationId).Name,
                        StateOfLocationId = m.StateOfLocationId,
                        TitleName = Enum.GetName(typeof(Title), m.Title),
                        Title = m.Title,
                        SexName = Enum.GetName(typeof(Sex), m.Sex),
                        Sex = m.Sex,
                        ChurchReferenceNumber = m.ChurchReferenceNumber,
                        StructureChurchHeadQuarterParishId = m.StructureChurchHeadQuarterParishId,


                        ClientChurchHeadQuarterChurchStructureTypeId = (structureChurchParish.ChurchStructureTypeId),


                        AccountInfo = m.AccountInfo,
                        ChurchStructureParishHeadQuarters = selectedParishIds,
                        Parishes = m.ClientStructureChurchHeadQuarter,
                        ClientChurchProfile = new ClientProfileRegistrationObj
                        {
                            ClientChurchId = clientChurchProfile.ClientChurchId,
                            ClientChurchProfileId = clientChurchProfile.ClientChurchProfileId,
                            Fullname = clientChurchProfile.Fullname,
                            Username = clientChurchProfile.Username,
                            MyRoleIds = roleIds,
                            MyRoles = roleList,
                            SelectedRoles = string.Join(",", roleList),
                        },

                        RegisteredByUserId = m.RegisteredByUserId,
                        TimeStampRegistered = m.TimeStampRegistered
                        //ChurchStructureHqtr = (clientChurchStructureHqtr ?? new ChurchStructureHqtr()),
                        //HeadQuarterChurchStructureTypeId = (clientChurchStructureHqtr != null && clientChurchStructureHqtr.ChurchStructureTypeId > 0 ? clientChurchStructureHqtr.ChurchStructureTypeId : 0),
                        //ClientStructureChurchDetails = clientStructureChurchDetails

                    });

                    #endregion
                }

                #endregion

                //return retList;

                return retList.FindAll(x => x.ChurchId != 1 && x.ChurchId != 2);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }


        internal ChurchStructureParishHeadQuarter AddFreshClientChurchStructureParishHeadQuarter(int hqtrTypeId,
            string parishName, long churchId, int stateId, out string hqtrId, out int action)
        {


            if (hqtrTypeId < 1 || parishName.IsNullOrEmpty() || churchId < 1 || stateId < 1)
            {
                hqtrId = "";
                action = 0;
                return new ChurchStructureParishHeadQuarter();
            }



            var structureChurchHeadQuarterParishId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture);
            #region Perform Operation If Church Structure Type Id of As HeadQuarter > 0

            if (hqtrTypeId > 0)
            {
                var churchStructureTypeId = hqtrTypeId;
                var churchStructureName =
                    new ChurchStructureTypeRepository().GetChurchStructureTypeNameById(churchStructureTypeId);

                var parishes = new StructureChurchHeadQuarterParish
                {
                    StructureChurchHeadQuarterParishId = structureChurchHeadQuarterParishId,
                    ChurchStructureTypeId = churchStructureTypeId,
                    ParishName = parishName,
                    ChurchStructureTypeName = churchStructureName
                };


                var thisChurchStructureParishHeadQuarter = new ChurchStructureParishHeadQuarterRegObj
                {
                    ChurchId = churchId,
                    StateOfLocationId = stateId,
                    ChurchStructureTypeId = churchStructureTypeId,
                    Parish = new List<StructureChurchHeadQuarterParish> { parishes },
                    RegisteredByUserId = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp()
                };


                #region Return Either Update or Fresh ChurchStructureParishHeadQuarter Object

                 // Check Exist of Composed Object to determine UPDATE or ADD Call with Handler
                //var churchId = thisChurchStructureParishHeadQuarter.ChurchId;
                //var state = thisChurchStructureParishHeadQuarter.StateOfLocationId;
                //var typeId = thisChurchStructureParishHeadQuarter.ChurchStructureTypeId;


                var existingParishHeadQuarter = ServiceChurch.IsChurchStructureParishHeadQuarterExist(churchId, stateId,
                    hqtrTypeId);
                if (existingParishHeadQuarter != null &&
                    existingParishHeadQuarter.ChurchStructureParishHeadQuarterId > 0)
                {
                    var existingParish = existingParishHeadQuarter.Parish;
                    existingParish.AddRange(thisChurchStructureParishHeadQuarter.Parish);
                    existingParishHeadQuarter._Parish = JsonConvert.SerializeObject(existingParish);

                    hqtrId = structureChurchHeadQuarterParishId;
                    action = 2;
                    return existingParishHeadQuarter;

                    #region Old Update
                    // Call It's Repository Handler
                    //var processedParishHqtr =
                    //    _churchStructureParishHeadQuarterRepository.Update(existingParishHeadQuarter);
                    //_uoWork.SaveChanges();
                    //if (processedParishHqtr.ChurchStructureParishHeadQuarterId < 1)
                    //{


                    //    //db.Rollback();
                    //    //response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                    //    //response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                    //    //response.Status.IsSuccessful = false;
                    //    //return response;
                    //}
                    #endregion
                   
                }

                var churchStructureParishHeadQuarter = new ChurchStructureParishHeadQuarter
                {
                    ChurchId = churchId,
                    StateOfLocationId = stateId,
                    ChurchStructureTypeId = churchStructureTypeId,
                    Parish = new List<StructureChurchHeadQuarterParish> { parishes },
                    RegisteredByUserId = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp()
                };

                hqtrId = structureChurchHeadQuarterParishId;
                action = 1;
                return churchStructureParishHeadQuarter;


                #region Old Adding
                // Call It's Repository Handler
                //var processedParishHqtr =
                //    _churchStructureParishHeadQuarterRepository.Add(churchStructureParishHeadQuarter);
                //_uoWork.SaveChanges();
                //if (processedParishHqtr.ChurchStructureParishHeadQuarterId < 1)
                //{
                //    //db.Rollback();
                //    //response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                //    //response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                //    //response.Status.IsSuccessful = false;
                //    //return response;
                //}
                #endregion

                #endregion

            }

            #endregion

            hqtrId = "";
            action = 0;
            return new ChurchStructureParishHeadQuarter();
            
        }



        internal ChurchThemeSetting GetClientChurchThemeDetail(long clientChurchId)
        {
            try
            {
                var myChurch = GetClientParentChurch(clientChurchId);
                if (myChurch == null || myChurch.ChurchId < 1) { return new ChurchThemeSetting(); }

                // Get Church Theme Info
                string msg;
                //var churchThemeInfo = ServiceChurch.GetChurchThemeSettingByChurchId(myChurch.ChurchId, out msg);
                var churchThemeInfo = new ChurchThemeSettingRepository().GetChurchThemeSettingByChurchId(myChurch.ChurchId, out msg);
                if (churchThemeInfo == null || churchThemeInfo.ChurchThemeSettingId < 1) { return new ChurchThemeSetting(); }
                return churchThemeInfo;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ChurchThemeSetting();
            }
        }



        private bool IsDuplicate(long churchId, string crn, string phoneNo, out string msg, int status = 0)
        {
            try
            {

                #region Latest Duplicate

                // Check Duplicate Phone Number & CRN
                var check = IsPhoneNumberExist(phoneNo);
                var check2 = IsCRNExist(churchId, crn);

                if ((!check.Any() || check.Count == 0) && (!check2.Any() || check2.Count == 0))
                {
                    msg = "";
                    return false;
                }

                switch (status)
                {
                    case 0:
                        if (check2 != null)
                        {
                            if (check2.Count > 0)
                            {
                                msg = "Duplicate Error! Church/Client already exist under the selected parent church";
                                return true;
                            }
                        }
                        if (check != null && check.Count > 0)
                        {
                            msg = "Duplicate Error! Phone Number already been used by another Church";
                            return true;
                        }
                        break;

                    case 1:
                        if (check2 != null)
                        {
                            if (check2.Count > 1)
                            {
                                msg = "Duplicate Error! Church/Client already exist under the selected parent church";
                                return true;
                            }
                        }

                        if (check != null)
                        {
                            if (check.Count > 1)
                            {
                                msg = "Duplicate Error! Phone Number already been used by another Church";
                                return true;
                            }
                        }
                        
                        break;
                }

                #endregion
                
                msg = "";
                return false;

                //msg = "Unable to check duplicate! Please try again later";
                //return true;
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return true;
            }
        }


        private bool IsDuplicatex(long churchId, string crn, string phoneNo, out string msg, int status = 0)
        {
            try
            {

                #region Latest Duplicate

                // Get all 
                #endregion


                // Query for Contact Phone No
                var sql1 =
                        string.Format("Select * FROM \"ChurchAPPDB\".\"Client\"  WHERE \"PhoneNumber\" = '{0}'", phoneNo);

                // Check Duplicate Reference No
                List<Client> check2;
                if (!string.IsNullOrEmpty(crn) && churchId > 0)
                {
                    var sql2 =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"Client\"  WHERE \"ChurchId\" = {0} AND \"ChurchReferenceNumber\" = '{1}'",
                            churchId, crn);
                    check2 = _repository.RepositoryContext().Database.SqlQuery<Client>(sql2).ToList();
                }
                else
                {
                    check2 = null;
                }


                var check = _repository.RepositoryContext().Database.SqlQuery<Client>(sql1).ToList();
                msg = "";
                if (check.IsNullOrEmpty() && (check2 == null || check2.IsNullOrEmpty())) return false;

                switch (status)
                {
                    case 0:
                        if (check2 != null)
                        {
                            if (check2.Count > 0)
                            {
                                msg = "Duplicate Error! Church/Client already exist under the selected parent church";
                                return true;
                            }
                        }
                        if (check.Count > 0)
                        {
                            msg = "Duplicate Error! Phone Number already been used by another Church";
                            return true;
                        }
                        break;

                    case 1:
                        if (check2 != null)
                        {
                            if (check2.Count > 1)
                            {
                                msg = "Duplicate Error! Church/Client already exist under the selected parent church";
                                return true;
                            }
                        }
                        if (check.Count > 1)
                        {
                            msg = "Duplicate Error! Phone Number already been used by another Church";
                            return true;
                        }
                        break;
                }

                msg = "";
                return false;

                //msg = "Unable to check duplicate! Please try again later";
                //return true;
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return true;
            }
        }


        #region CLIENT ADMINISTRATION

        internal List<ClientChurch> IsPhoneNumberExist(string phoneNo)
        {
            try
            {
                var myItem = GetClientChurchs().FindAll(x => x.PhoneNumber == phoneNo);
                //var retIem = myItem.FindAll(x => x.PhoneNumber == phoneNo);

                if (!myItem.Any()) { return new List<ClientChurch>(); }
                return myItem;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal List<ClientChurch> IsCRNExist(long churchId, string crn)
        {
            try
            {
                var myItem = GetClientChurchs().FindAll(x => x.ChurchId == churchId && x.ChurchReferenceNumber == crn);
                //var retItem = myItem.FindAll(x => x.ChurchId == churchId && x.ChurchReferenceNumber == crn);
                if (!myItem.Any()) { return new List<ClientChurch>(); }
                return myItem;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ClientChurch>();
            }
        }

        internal ClientChurch GetClientChurch(long clientChurchId)
        {
            try
            {
                var myItem = _repository.GetById(clientChurchId);
                if (myItem == null || myItem.ClientChurchId < 1) { return null; }
                return myItem;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }


        internal Church GetClientParentChurch(long clientChurchId)
        {
            try
            {
                var myItem = _repository.GetById(clientChurchId);
                if (myItem == null || myItem.ClientChurchId < 1) { return null; }

                // Get Church
                var church = new ChurchRepository().GetChurch(myItem.ChurchId);
                if (church == null || church.ChurchId < 1) { return null; }
                return church;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }


        //internal Church GetClientChurchzzz(long clientId)
        //{
        //    try
        //    {
        //        var myItem = _repository.GetById(clientId);
        //        if (myItem == null || myItem.ClientId < 1) { return null; }

        //        // Get Church
        //        var church = new ChurchRepository().GetChurch(myItem.ChurchId);
        //        if (church == null || church.ChurchId < 1) { return null; }
        //        return church;
        //    }
        //    catch (Exception ex)
        //    {
        //        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
        //        return null;
        //    }
        //}




        internal List<ClientChurch> GetClientChurchs()
        {
            try
            {
                var myItems = _repository.GetAll().ToList();
                if (!myItems.Any()) { return new List<ClientChurch>(); }
                return myItems;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ClientChurch>();;
            }
        }


        internal ClientChurch GetClientByChurchId(long churchId)
        {
            try
            {
                var myItems = _repository.GetAll(x => x.ChurchId == churchId).OrderByDescending(o => o.ClientChurchId).ToList();
                if (!myItems.Any()) { return new ClientChurch(); }
                return myItems[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ClientChurch();
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


        #region CRN Generation

        internal string GenerateCRN(out string msg, string churchShortName, int churchId)
        {
            try
            {
                // RCCG00000000001
                var lastChurchClientInfo = GetMaxClientInfo(out msg, churchId);
                if (lastChurchClientInfo == null || lastChurchClientInfo.ClientChurchId < 1)
                {
                    if (!string.IsNullOrEmpty(msg))
                    {
                        return "";
                    }
                    return churchShortName + "0000000001";
                }

                var retRef = lastChurchClientInfo.ChurchReferenceNumber.Replace(churchShortName, "");
                if (string.IsNullOrEmpty(retRef))
                {
                    msg = "Unable to retreive generate CRN";
                    return "";
                }

                var newRef = churchShortName + (long.Parse(retRef) + 1).ToString("D10");
                return newRef;
            }
            catch (Exception ex)
            {
                msg = "Unable to retreive generate CRN";
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return "";
            }

        }

        private ClientChurch GetMaxClientInfo(out string msg, int churchId)
        {

            try
            {
                //var sqlBuilder = new StringBuilder();
                //sqlBuilder.AppendFormat("SELECT * FROM \"ChurchAPPDB\".\"Client\" " +
                //                            " WHERE \"ChurchId\" = {0} ORDER BY \"ClientId\" DESC", churchId);
                //string sql1 = sqlBuilder.ToString();

                //var activity = _repository.RepositoryContext().Database.SqlQuery<Client>(sql1).ToList();

                var activity = GetClientByChurchId(churchId);
                if (activity == null || activity.ClientChurchId < 1)
                {
                    msg = "";
                    return null;
                }


                msg = "";
                return activity;
            }
            catch (Exception ex)
            {
                msg = "Unable to  generate CRN";
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        #endregion





        #region Mail Background Jobs

        internal void Mailing()
        {
            
            // Send Mail to Parish's Registered Email (Contents: Gen. Password, Congratulation message)
            var message = string.Format("Dear Love Gate Parish! Your registration as a parish on ICAS" +
                                  "was successful.{0}" +
                                  "Your Login Password: lovegate, {0}", Environment.NewLine);
            const string subject = "Setting Up Confirmation";

            var emailContract = new EmailContract
            {
                MailTo = "tacis4real@yahoo.com",
                MailFrom = "nassidec@yahoo.com",
                MailMessage = message,
                SenderName = "All of us from EpayPlus"
            };
            SendParishMail(emailContract, subject);
            
        }


        internal void SendParishMail(EmailContract emailContract, string subject)
        {
            const string body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var mail = new MailMessage();
            mail.To.Add(new MailAddress(emailContract.MailTo));
            mail.From = new MailAddress("ailesanmi@epayplusng.com");
            mail.Subject = subject;
            mail.Body = string.Format(body, emailContract.SenderName, emailContract.MailFrom, emailContract.MailMessage);
            mail.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "ailesanmi@epayplusng.com",
                    Password = "January1986"
                };

                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = credential;
                smtp.Host = "smtp.1and1.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }

        #endregion

    }
}
