using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ICASStacks.APIObjs;
using ICASStacks.DataContract;
using ICASStacks.DataContract.Enum;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using ICASStacks.Repository.Helpers;
using ICASStacks.StackService;
using WebAdminStacks.APIObjs;
using WebAdminStacks.APIs;
using WebCribs.TechCracker.WebCribs.TechCracker;
using RespMessage = ICASStacks.APIObjs.RespMessage;
using RespStatus = ICASStacks.APIObjs.RespStatus;

namespace ICASStacks.Repository
{
    internal class ClientRepository
    {

        private readonly IIcasRepository<Client> _repository;
        private readonly IIcasRepository<ClientContact> _clientContactRepository;
        private readonly IIcasRepository<ClientAccount> _clientAccountRepository;
        private readonly IIcasRepository<Region> _regionRepository;
        private readonly IIcasRepository<Province> _provinceRepository;
        private readonly IIcasRepository<Zone> _zoneRepository;
        private readonly IIcasRepository<Area> _areaRepository;
        private readonly IIcasRepository<Diocese> _dioceseRepository;
        private readonly IIcasRepository<District> _districtRepository;
        private readonly IIcasRepository<State> _stateRepository;
        private readonly IIcasRepository<Group> _groupRepository;
        private readonly IIcasRepository<ChurchStructureHqtr> _churchStructureHqtrRepository;
        private readonly IIcasRepository<Bank> _bankRepository;
        private readonly IIcasRepository<StateOfLocation> _stateOfLocationRepository;

        private readonly IIcasRepository<ClientStructureChurchDetail> _clientChurchStructureDetailRepository;
        private readonly IcasUoWork _uoWork;


        //long ticks = DateTime.Now.Ticks;
        //byte[] bytes = BitConverter.GetBytes(ticks);
        //string id = Convert.ToBase64String(bytes)
        //                        .Replace('+', '_')
        //                        .Replace('/', '-')
        //                        .TrimEnd('=');

        //var serachClientId = '%' + clientId + '%';
        //var churchId = 0;
        //var stateId = 0;
        //var sql =
        //         string.Format(
        //             "Select * FROM \"ICASDB\".\"StructureChurchHeadQuarter\" " +
        //             "WHERE \"ChurchId\" = {0} " +
        //             "AND \"StateOfLocationId\" = {1} AND " +
        //             "\"Parish\" LIKE {2};", churchId, stateId, serachClientId);

        //var sql =
        //         string.Format(
        //             "Select * FROM \"ICASDB\".\"StructureChurchHeadQuarter\" " +
        //             "WHERE \"Parish\" LIKE '%'{0}'%';", churchId);


        public ClientRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<Client>(_uoWork);
            _churchStructureHqtrRepository = new IcasRepository<ChurchStructureHqtr>(_uoWork);
            _clientContactRepository = new IcasRepository<ClientContact>(_uoWork);
            _clientAccountRepository = new IcasRepository<ClientAccount>(_uoWork);
            _regionRepository = new IcasRepository<Region>(_uoWork);
            _provinceRepository = new IcasRepository<Province>(_uoWork);
            _zoneRepository = new IcasRepository<Zone>(_uoWork);
            _areaRepository = new IcasRepository<Area>(_uoWork);
            _dioceseRepository = new IcasRepository<Diocese>(_uoWork);
            _districtRepository = new IcasRepository<District>(_uoWork);
            _stateRepository = new IcasRepository<State>(_uoWork);
            _groupRepository = new IcasRepository<Group>(_uoWork);
            _bankRepository = new IcasRepository<Bank>(_uoWork);
            _stateOfLocationRepository = new IcasRepository<StateOfLocation>(_uoWork);

            _clientChurchStructureDetailRepository = new IcasRepository<ClientStructureChurchDetail>(_uoWork);
        }

        internal ClientRegResponse AddClient(ClientRegistrationObj clientRegistrationObj)
        {

            var response = new ClientRegResponse
            {
                ClientId = 0,
                ChurchId = clientRegistrationObj.ChurchId,
                Name = clientRegistrationObj.Name,
                Email = clientRegistrationObj.Email,
                PhoneNumber = clientRegistrationObj.PhoneNumber,
                ChurchReferenceNumber = clientRegistrationObj.ChurchReferenceNumber,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {

                if (clientRegistrationObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Church registration Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(clientRegistrationObj, out valResults))
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

                string msg;
                var clientChurch = ServiceChurch.GetChurch(clientRegistrationObj.ChurchId);
                var crn = GenerateCRN(out msg, clientChurch.ShortName, Convert.ToInt32(clientRegistrationObj.ChurchId));
                if (string.IsNullOrEmpty(crn))
                {
                    response.Status.Message.FriendlyMessage = "Unable to generate CRN";
                    response.Status.Message.TechnicalMessage = "Unable to generate CRN";
                    return null;
                }

                // Valid Admin who carried out this operation
                if (clientRegistrationObj.RegisteredByUserId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Admin User Information";
                    response.Status.Message.TechnicalMessage = "Invalid Admin User Information";
                    response.Status.IsSuccessful = false;
                    return response;
                }

                clientRegistrationObj.PhoneNumber = CleanMobile(clientRegistrationObj.PhoneNumber);
                if (IsDuplicate(clientRegistrationObj.ChurchId, crn, clientRegistrationObj.PhoneNumber, out msg))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }


                #region Old Duplicate Check

                //if (IsDuplicate(clientRegistrationObj.ChurchId, crn,
                //    clientRegistrationObj.Contact.PhoneNumber, clientRegistrationObj.Account.AccountNumber, out msg))
                //{
                //    response.Status.Message.FriendlyMessage = msg;
                //    response.Status.Message.TechnicalMessage = msg;
                //    response.Status.IsSuccessful = false;
                //    return response;
                //}

                #endregion


                var clientObj = new Client
                {
                    ChurchId = clientRegistrationObj.ChurchId,
                    Name = clientRegistrationObj.Name,
                    Pastor = clientRegistrationObj.Pastor,
                    Title = clientRegistrationObj.Title,
                    Sex = clientRegistrationObj.Sex,
                    StateOfLocationId = clientRegistrationObj.StateOfLocationId,
                    Address = clientRegistrationObj.Address,
                    Email = clientRegistrationObj.Email,
                    PhoneNumber = clientRegistrationObj.PhoneNumber,
                    ChurchReferenceNumber = crn,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = clientRegistrationObj.RegisteredByUserId,
                };

                var processedClient = _repository.Add(clientObj);
                _uoWork.SaveChanges();
                if (processedClient.ClientId < 1)
                {
                    //db.Rollback();
                    response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                    response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                    response.Status.IsSuccessful = false;
                    return response;
                }

                response.ClientId = processedClient.ClientId;
                response.Name = clientRegistrationObj.Name;
                response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                response.Status.IsSuccessful = true;
                return response;


                #region Old Database Storing

                //using (var db = _uoWork.BeginTransaction())
                //{

                #region Client Self

                //var clientObj = new Client
                //{
                //    ChurchId = clientRegistrationObj.ChurchId,
                //    Name = clientRegistrationObj.Name,
                //    Pastor = clientRegistrationObj.Pastor,
                //    Title = clientRegistrationObj.Title,
                //    Sex = clientRegistrationObj.Sex,
                //    ChurchReferenceNumber = crn,
                //    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                //    RegisteredByUserId = clientRegistrationObj.RegisteredByUserId,
                //};

                //var processedClient = _repository.Add(clientObj);
                //_uoWork.SaveChanges();
                //if (processedClient.ClientId < 1)
                //{
                //    db.Rollback();
                //    response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                //    response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                //    response.Status.IsSuccessful = false;
                //    return response;
                //}

                #endregion


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

                #region HeadQuarters Details

                //bool duplicate;
                //var churchStructureHqtr = GetChurchStructureHqtr(clientRegistrationObj, out duplicate, out msg, new ChurchStructureHqtr());

                //if (churchStructureHqtr != null && churchStructureHqtr.StructureDetailId > 0)
                //{
                //    if (duplicate)
                //    {
                //        db.Rollback();
                //        response.Status.Message.FriendlyMessage = msg;
                //        response.Status.Message.TechnicalMessage = msg;
                //        response.Status.IsSuccessful = false;
                //        return response;
                //    }

                //    var processedChurchStructureHqtr = _churchStructureHqtrRepository.Add(churchStructureHqtr);
                //    _uoWork.SaveChanges();
                //    if (processedChurchStructureHqtr.ChurchStructureHqtrId < 1)
                //    {
                //        db.Rollback();
                //        response.Status.Message.FriendlyMessage = "Unable to complete registration from Headquarter! Please try again later";
                //        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                //        response.Status.IsSuccessful = false;
                //        return response;
                //    }
                //}


                #endregion

                #region Client Profile


                //var clientRole = GetClientRoleByName("Pastor", out msg);
                //var clientRole = PortalClientUser.GetClientRoleByName("Pastor", out msg);
                //const string password = "ChurchAdmin";
                //const string username = "ChurchAdmin";
                //var salt = EncryptionHelper.GenerateSalt(30, 50);
                //var accessCode = Crypto.HashPassword(clientRegistrationObj.Password);

                //var thisClientProfile = new ClientProfileRegistrationObj
                //{
                //    ClientId = processedClient.ClientId,
                //    Fullname = clientRegistrationObj.Pastor,
                //    Email = clientRegistrationObj.Email,
                //    MobileNumber = clientRegistrationObj.PhoneNumber,
                //    RegisteredByUserId = clientRegistrationObj.RegisteredByUserId,
                //    Password = password,
                //    Sex = clientRegistrationObj.Sex,
                //    //Username = clientRegistrationObj.Username,
                //};

                // Check Duplicate before calling 'AddClientProile' method, But it will be done within it's Repository 
                // because is not within the same DatabaseContext with ChurchAppStack
                //var processedClientProfile = PortalClientUser.AddClientProile(thisClientProfile);
                //if (processedClientProfile.ClientProfileId < 1)
                //{
                //    db.Rollback();
                //    response.Status.Message.FriendlyMessage = string.IsNullOrEmpty(processedClientProfile.Status.Message.FriendlyMessage) ? "Unable to complete registration! Please try again later" : processedClientProfile.Status.Message.FriendlyMessage;
                //    response.Status.Message.TechnicalMessage = string.IsNullOrEmpty(processedClientProfile.Status.Message.TechnicalMessage) ? "Process Failed! Unable to save data" : processedClientProfile.Status.Message.TechnicalMessage;
                //    response.Status.IsSuccessful = false;
                //    return response;
                //}

                #endregion

                //db.Commit();
                //response.ClientId = processedClient.ClientId;
                //response.Name = clientRegistrationObj.Name;
                //response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                //response.Status.IsSuccessful = true;
                //return response;

                //}

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
            //catch (Exception ex)
            //{
            //    BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
            //    response.Status.Message.FriendlyMessage = "Unable to complete registration at this time! Please try again later";
            //    response.Status.Message.TechnicalMessage = "Error: " + ex.Message;
            //    response.Status.IsSuccessful = false;
            //    return response;
            //}
        }
        
        internal ClientStructureChurchRegResponse AddClientStructureChurch(ClientStructureChurchRegistrationObj clientStructureChurchRegObj)
        {

            var response = new ClientStructureChurchRegResponse
            {
                ClientId = clientStructureChurchRegObj.ClientId,
                ChurchId = clientStructureChurchRegObj.ChurchId,
                ClientChurchStructureDetails = new List<ClientStructureChurchDetail>(),
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {

                if (clientStructureChurchRegObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Structure Church registration Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(clientStructureChurchRegObj, out valResults))
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
                
                // Valid Admin who carried out this operation
                //if (clientStructureChurchRegObj.RegisteredByUserId < 1)
                //{
                //    response.Status.Message.FriendlyMessage = "Invalid Admin User Information";
                //    response.Status.Message.TechnicalMessage = "Invalid Admin User Information";
                //    response.Status.IsSuccessful = false;
                //    return response;
                //}

                //clientRegistrationObj.PhoneNumber = CleanMobile(clientRegistrationObj.PhoneNumber);
                //clientRegistrationObj.Contact.PhoneNumber = CleanMobile(clientRegistrationObj.Contact.PhoneNumber);
                //if (IsDuplicate(clientRegistrationObj.ChurchId, crn,
                //    clientRegistrationObj.Contact.PhoneNumber, clientRegistrationObj.Account.AccountNumber, out msg))
                //{
                //    response.Status.Message.FriendlyMessage = msg;
                //    response.Status.Message.TechnicalMessage = msg;
                //    response.Status.IsSuccessful = false;
                //    return response;
                //}
                var thisClientStructureDetails = new List<ClientStructureChurchDetail>();
                using (var db = _uoWork.BeginTransaction())
                {

                    #region Client Church Structure Details

                    var clientStructureDetails = GetClientStructureDetails(clientStructureChurchRegObj);
                    if (clientStructureDetails != null && clientStructureDetails.Count > 0)
                    {

                        if (!DeleteClientChurchStructureDetails(clientStructureChurchRegObj.ClientId, clientStructureDetails))
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                            response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                            response.Status.IsSuccessful = false;
                            return response;

                        }

                        //if (clientStructureDetails.Count > 0)
                        //{

                        //    if(!DeleteClientChurchStructureDetails(clientStructureChurchRegObj.ClientId, clientStructureDetails))
                        //    {
                        //        db.Rollback();
                        //        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        //        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        //        response.Status.IsSuccessful = false;
                        //        return response;
                        //    }
                        //    //foreach (var clientStructureDetail in clientStructureDetails)
                        //    //{
                        //    //    var processedClientChurchStructureDetail = _clientChurchStructureDetailRepository.Add(clientStructureDetail);
                        //    //    _uoWork.SaveChanges();
                        //    //    if (processedClientChurchStructureDetail.ClientStructureChurchDetailId < 1)
                        //    //    {
                        //    //        db.Rollback();
                        //    //        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        //    //        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        //    //        response.Status.IsSuccessful = false;
                        //    //        return response;
                        //    //    }
                        //    //    thisClientStructureDetails.Add(processedClientChurchStructureDetail);
                        //    //}
                        //}

                        #region HeadQuarters Details

                        bool duplicate;
                        //var churchStructureHqtr = GetChurchStructureHqtr(processedClient.ClientId, clientRegistrationObj, out duplicate, out msg);
                        var churchStructureHqtr = GetChurchStructureHqtr(clientStructureChurchRegObj, out msg, new ChurchStructureHqtr());
                        if (churchStructureHqtr != null && churchStructureHqtr.StructureDetailId > 0)
                        {
                            //if (duplicate)
                            //{
                            //    db.Rollback();
                            //    response.Status.Message.FriendlyMessage = msg;
                            //    response.Status.Message.TechnicalMessage = msg;
                            //    response.Status.IsSuccessful = false;
                            //    return response;
                            //}

                            if (!DeleteClientChurchStructureHqtrDetails(clientStructureChurchRegObj.ClientId, churchStructureHqtr))
                            {
                                db.Rollback();
                                response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                                response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                                response.Status.IsSuccessful = false;
                                return response;
                            }


                            //var processedChurchStructureHqtr = _churchStructureHqtrRepository.Add(churchStructureHqtr);
                            //_uoWork.SaveChanges();
                            //if (processedChurchStructureHqtr.ChurchStructureHqtrId < 1)
                            //{
                            //    db.Rollback();
                            //    response.Status.Message.FriendlyMessage = "Unable to complete registration from Headquarter! Please try again later";
                            //    response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                            //    response.Status.IsSuccessful = false;
                            //    return response;
                            //}

                            //response.ChurchStructureHqtrId = processedChurchStructureHqtr.ChurchStructureHqtrId;
                        }
                        else
                        {
                            response.Status.Message.FriendlyMessage = "Empty Structure Info! No structure church info to set.";
                            response.Status.Message.TechnicalMessage = "Structure Church registration Object is empty / invalid!";
                            return response;
                        }


                        #endregion

                    }
                    else
                    {
                        response.Status.Message.FriendlyMessage = "Empty Structure Info! No structure church info to set.";
                        response.Status.Message.TechnicalMessage = "Structure Church registration Object is empty / invalid!";
                        return response;
                    }

                    #endregion
                    

                    db.Commit();
                    response.ClientId = clientStructureChurchRegObj.ClientId;
                    response.ClientChurchStructureDetails = thisClientStructureDetails;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;

                }

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
        
        internal ClientAccountRegResponse AddClientAccount(ClientAccountRegistrationObj clientAccountRegObj)
        {

            var response = new ClientAccountRegResponse
            {
                ClientAccountId = 0,
                ClientId = clientAccountRegObj.ClientId,
                AccountNumber = clientAccountRegObj.AccountNumber,
                AccountTypeId = clientAccountRegObj.AccountTypeId,
                AccountName = clientAccountRegObj.AccountName,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {
                if (clientAccountRegObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Church registration Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(clientAccountRegObj, out valResults))
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

                // Valid Admin who carried out this operation
                if (clientAccountRegObj.RegisteredByUserId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Admin User Information";
                    response.Status.Message.TechnicalMessage = "Invalid Admin User Information";
                    response.Status.IsSuccessful = false;
                    return response;
                }

                string msg;
                if (IsAccountDuplicate(clientAccountRegObj.AccountNumber, out msg))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }

                // Submit Client Account
                var thisClientAccount = new ClientAccount
                {
                    ClientId = clientAccountRegObj.ClientId,
                    BankId = clientAccountRegObj.BankId,
                    AccountName = clientAccountRegObj.AccountName,
                    AccountNumber = clientAccountRegObj.AccountNumber,
                    AccountTypeId = clientAccountRegObj.AccountTypeId,
                    RegisteredByUserId = clientAccountRegObj.RegisteredByUserId, 
                    Status = ClientAccountStatus.Active,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp()
                };

                var processedClientAccount = _clientAccountRepository.Add(thisClientAccount);
                _uoWork.SaveChanges();
                if (processedClientAccount.ClientAccountId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                    response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                    response.Status.IsSuccessful = false;
                    return response;
                }

                response.ClientAccountId = processedClientAccount.ClientAccountId;
                response.ClientId = processedClientAccount.ClientId;
                response.AccountNumber = processedClientAccount.AccountNumber;
                response.AccountTypeId = processedClientAccount.AccountTypeId;
                response.AccountName = processedClientAccount.AccountName;
                response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                response.Status.IsSuccessful = true;
                return response;
                
            }
            catch (DbEntityValidationException ex)
            {
                response.Status.Message.FriendlyMessage =
                    "Unable to complete registration at this time! Please try again later";
                response.Status.Message.TechnicalMessage = "Error: " + ex.Message;
                response.Status.IsSuccessful = false;
                return response;
            }

        }
        
        internal RespStatus UpdateClient(ClientRegistrationObj client)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };

            try
            {
                if (client.Equals(null))
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your processing";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(client, out valResults))
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
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = errorDetail.ToString();
                    return response;
                }

                string msg;
                var thisClient = GetClient(client.ClientId, out msg);
                if (thisClient == null || thisClient.ChurchReferenceNumber.Length < 1)
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage =
                            string.IsNullOrEmpty(msg) ? "Invalid Church Information" : msg;
                    return response;
                }

                client.PhoneNumber = CleanMobile(client.PhoneNumber);
                if (IsDuplicate(client.ChurchId, client.ChurchReferenceNumber, client.PhoneNumber, out msg, 1))
                {
                    response.Message.FriendlyMessage = msg;
                    response.Message.TechnicalMessage = msg;
                    response.IsSuccessful = false;
                    return response;
                }
                
                thisClient.ChurchId = client.ChurchId;
                thisClient.Name = client.Name;
                thisClient.Pastor = client.Pastor;
                thisClient.Title = client.Title;
                thisClient.Sex = client.Sex;
                thisClient.Email = client.Email;
                thisClient.Address = client.Address;
                thisClient.PhoneNumber = client.PhoneNumber;
                thisClient.StateOfLocationId = client.StateOfLocationId;

                var processedClient = _repository.Update(thisClient);
                _uoWork.SaveChanges();
                if (processedClient.ClientId < 1)
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage = "Process Failed! Please try again later";
                    return response;
                }

                response.IsSuccessful = true;
                return response;
            }
            catch (DbEntityValidationException ex)
            {
                response.Message.FriendlyMessage =
                    "Unable to complete your request due to error! Please try again later";
                response.Message.TechnicalMessage = "Error" + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return response;
            }

                #region Personal Info, Contact & Account

                

                // Get this Client Contact
                //var thisClientContact = GetClientContact(client.Contact, out msg);
                //if (thisClientContact == null || string.IsNullOrEmpty(thisClientContact.PhoneNumber))
                //{
                //    response.Message.FriendlyMessage = response.Message.TechnicalMessage = string.IsNullOrEmpty(msg) ? "Invalid Church Contact Information" : msg;
                //    return response;
                //}

                // Get this Client Account
                //var thisClientAccount = GetClientAccount(client.Account, out msg);
                //if (thisClientAccount == null || string.IsNullOrEmpty(thisClientAccount.AccountNumber))
                //{
                //    response.Message.FriendlyMessage = response.Message.TechnicalMessage = string.IsNullOrEmpty(msg) ? "Invalid Church Account Information" : msg;
                //    return response;
                //}

                #endregion

                #region Client Church Structure Details

                //var clientStructureDetails = GetClientStructureDetails(client);

                #endregion

                #region Client HeadQuarter Detail

                // Get this Client HeadQuarter Detail
                //var updateClientChurchStructureHqtr = new ChurchStructureHqtr();
                //if (client.HeadQuarterChurchStructureTypeId > 0)
                //{
                //    var thisHqtrObj = GetClientChurchStructureHeadQuarterDetail(client.ClientId);
                //    if (thisHqtrObj != null && thisHqtrObj.ChurchStructureHqtrId > 0)
                //    {
                //        bool duplicate;
                //        updateClientChurchStructureHqtr = GetChurchStructureHqtr(client, out duplicate, out msg, thisHqtrObj, 1);
                //        if (updateClientChurchStructureHqtr != null)
                //        {
                //            updateClientChurchStructureHqtr.ChurchStructureHqtrId =
                //            thisHqtrObj.ChurchStructureHqtrId;
                //        }

                //        if (duplicate)
                //        {
                //            response.Message.FriendlyMessage =
                //                response.Message.TechnicalMessage =
                //                    string.IsNullOrEmpty(msg)
                //                        ? "Duplicate church headquarter! Another church has been the headquarter"
                //                        : msg;
                //            return response;
                //        }
                //    }
                //}

                #endregion


                #region Client Profile

                //var thisClientProfile = GetClientProfile(client, out msg);
                //if (thisClientProfile == null || string.IsNullOrEmpty(thisClientProfile.Username) || thisClientProfile.ClientProfileId < 1)
                //{
                //    response.Message.FriendlyMessage = response.Message.TechnicalMessage = string.IsNullOrEmpty(msg) ? "Invalid Client Profile Information" : msg;
                //    return response;
                //}

                //var duplicateClientProfile = PortalClientUser.IsDuplicate(client.Username, client.PhoneNumber, out msg, 1);
                //if (duplicateClientProfile)
                //{
                //    response.Message.FriendlyMessage =
                //        response.Message.TechnicalMessage =
                //            string.IsNullOrEmpty(msg) ? "Duplicate Error! Client profile exist already" : msg;
                //    return response;
                //}

                #endregion
                

                #region Old Duplicate Check

                //if (IsDuplicate(client.ChurchId, thisClient.ChurchReferenceNumber, thisClientContact.PhoneNumber, thisClientAccount.AccountNumber, out msg, 1))
                //{
                //    response.Message.FriendlyMessage = msg;
                //    response.Message.TechnicalMessage = msg;
                //    response.IsSuccessful = false;
                //    return response;
                //}

                #endregion



                #region Old Database Storing

                //using (var db = _uoWork.BeginTransaction())
                //{
                //try
                //{

                //if (clientStructureDetails != null)
                //{
                //    if (clientStructureDetails.Any())
                //    {
                //        if (!DeleteClientChurchStructureDetails(client.ClientId, clientStructureDetails))
                //        {
                //            db.Rollback();
                //            response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                //            return response;
                //        }
                //    }
                //}

                //thisClient.ChurchId = client.ChurchId;
                //thisClient.Name = client.Name;
                //thisClient.Pastor = client.Pastor;
                //thisClient.Title = client.Title;
                //thisClient.Sex = client.Sex;

                //var processedClient = _repository.Update(thisClient);
                //_uoWork.SaveChanges();
                //if (processedClient.ClientId < 1)
                //{
                //    db.Rollback();
                //    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                //    return response;
                //}

                #region Client Contact & Account

                // Updating Client Contact
                //var processedClientContact = _clientContactRepository.Update(thisClientContact);
                //_uoWork.SaveChanges();
                //if (processedClientContact.ClientContactId < 1)
                //{
                //    db.Rollback();
                //    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                //    return response;
                //}

                // Updating Client Account
                //var processedClientAccount = _clientAccountRepository.Update(thisClientAccount);
                //_uoWork.SaveChanges();
                //if (processedClientAccount.ClientAccountId < 1)
                //{
                //    db.Rollback();
                //    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                //    return response;
                //}

                #endregion

                #region Church Structure Headquarter

                // Updating Client Church Structure HeadQuarter
                //if (updateClientChurchStructureHqtr != null && updateClientChurchStructureHqtr.ChurchStructureHqtrId > 0)
                //{
                //    var processedChurchStructureHqtr = _churchStructureHqtrRepository.Update(updateClientChurchStructureHqtr);
                //    _uoWork.SaveChanges();
                //    if (processedChurchStructureHqtr.ChurchStructureHqtrId < 1)
                //    {
                //        db.Rollback();
                //        response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                //        return response;
                //    }
                //}

                #endregion

                #region Client Profile

                // Updating Client Profile
                //var processedClientProfile = PortalClientUser.UpdateClientProfile(thisClientProfile);
                //_uoWork.SaveChanges();
                //if (!processedClientProfile.IsSuccessful)
                //{
                //    db.Rollback();
                //    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                //    return response;
                //}

                #endregion

                //db.Commit();
                //response.IsSuccessful = true;
                //return response;
        

            //catch (DbEntityValidationException ex)
            //{
            //    db.Rollback();
            //    response.Message.FriendlyMessage =
            //        "Unable to complete your request due to error! Please try again later";
            //    response.Message.TechnicalMessage = "Error" + ex.Message;
            //    BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
            //    return response;
            //}
            //catch (Exception ex)
            //{
            //    db.Rollback();
            //    response.Message.FriendlyMessage =
            //         "Unable to complete your request due to error! Please try again later";
            //    response.Message.TechnicalMessage = "Error" + ex.Message;
            //    BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
            //    return response;
            //}
            //}

            //}
            //catch (DbEntityValidationException ex)
            //{
            //    response.Message.FriendlyMessage =
            //                  "Unable to complete your request due to error! Please try again later";
            //    response.Message.TechnicalMessage = "Error" + ex.Message;
            //    BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
            //    return response;
            //}
            //catch (Exception ex)
            //{
            //    response.Message.FriendlyMessage = "Unable to complete your request due to error! Please try again later";
            //    response.Message.TechnicalMessage = "Error" + ex.Message;
            //    BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
            //    return response;
            //}

            #endregion
    

        }

        internal RespStatus UpdateClientAccount(ClientAccountRegistrationObj clientAccount)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };

            try
            {
                if (clientAccount.Equals(null))
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your processing";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(clientAccount, out valResults))
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
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = errorDetail.ToString();
                    return response;
                }

                string msg;
                var thisClientAccount = GetClientAccount(clientAccount.ClientAccountId, out msg);
                if (thisClientAccount == null || thisClientAccount.AccountName.Length < 1)
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage =
                            string.IsNullOrEmpty(msg) ? "Invalid Church Information" : msg;
                    return response;
                }

                if (IsAccountDuplicate(clientAccount.AccountNumber, out msg, 1))
                {
                    response.Message.FriendlyMessage = msg;
                    response.Message.TechnicalMessage = msg;
                    response.IsSuccessful = false;
                    return response;
                }

                thisClientAccount.ClientId = clientAccount.ClientId;
                thisClientAccount.BankId = clientAccount.BankId;
                thisClientAccount.AccountName = clientAccount.AccountName;
                thisClientAccount.AccountNumber = clientAccount.AccountNumber;
                thisClientAccount.AccountTypeId = clientAccount.AccountTypeId;

                var processedClientAccount = _clientAccountRepository.Update(thisClientAccount);
                _uoWork.SaveChanges();
                if (processedClientAccount.ClientAccountId < 1)
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage = "Process Failed! Please try again later";
                    return response;
                }

                response.IsSuccessful = true;
                return response;
            }
            catch (DbEntityValidationException ex)
            {
                response.Message.FriendlyMessage =
                    "Unable to complete your request due to error! Please try again later";
                response.Message.TechnicalMessage = "Error" + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return response;
            }
            
        }

        internal List<RegisteredClientListReportObj> GetAllRegisteredClientObjs()
        {
            try
            {
                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) return null;

                var retList = new List<RegisteredClientListReportObj>();
                myItemList.ForEachx(m =>
                {
                    string msg;
                    var clientParentChurch = new ChurchRepository().GetChurch(m.ChurchId);
                    var clientChurchStructureHqtr = ServiceChurch.GetChurchStructureHqtrByClientId(m.ClientId);
                    var clientStructureChurchDetails = GetThisClientRegisteredStructureChurchListObjs(m.ClientId);
                    
                    retList.Add(new RegisteredClientListReportObj
                    {
                        ClientId = m.ClientId,
                        ChurchId = m.ChurchId,
                        Name = m.Name,
                        ParentChurchName = clientParentChurch.Name,
                        ParentChurchShortName = clientParentChurch.ShortName,
                        Pastor = m.Pastor,
                        PhoneNumber = m.PhoneNumber,
                        Email = m.Email,
                        Address = m.Address,
                        StateOfLocation = _stateOfLocationRepository.GetById(m.StateOfLocationId).Name,
                        StateOfLocationId = m.StateOfLocationId,
                        
                        Title = Enum.GetName(typeof(Title), m.Title),
                        TitleId = m.Title,
                        Sex = Enum.GetName(typeof(Sex), m.Sex),
                        SexId = m.Sex,
                        ChurchReferenceNumber = m.ChurchReferenceNumber,

                        ChurchStructureHqtr = (clientChurchStructureHqtr ?? new ChurchStructureHqtr()),
                        HeadQuarterChurchStructureTypeId = (clientChurchStructureHqtr != null && clientChurchStructureHqtr.ChurchStructureTypeId > 0 ? clientChurchStructureHqtr.ChurchStructureTypeId : 0),
                        ClientStructureChurchDetails = clientStructureChurchDetails
                        
                    });
                });

                
                    
                return retList.FindAll(x => x.ChurchId != 1 && x.ChurchId != 2);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal List<RegisteredClientAccountListReportObj> GetAllRegisteredClientAccountObjs()
        {
            try
            {
                var myItemList = _clientAccountRepository.GetAll().ToList();
                if (!myItemList.Any()) return new List<RegisteredClientAccountListReportObj>();

                var retList = new List<RegisteredClientAccountListReportObj>();
                myItemList.ForEachx(m =>
                {
                    string msg;
                    var client = GetClient(m.ClientId, out msg);
                    var church = GetClientChurch(m.ClientId);
                    
                    retList.Add(new RegisteredClientAccountListReportObj
                    {
                        ClientAccountId = m.ClientAccountId,
                        ChurchId = church.ChurchId,
                        ClientId = m.ClientId,
                        Church = church.ShortName,
                        Client = client.Name,
                        BankId = m.BankId,
                        Bank = _bankRepository.GetById(m.BankId).Name,
                        AccountName = m.AccountName,
                        AccountNumber = m.AccountNumber,
                        AccountTypeId = m.AccountTypeId,
                        AccountType = Enum.GetName(typeof(AccountType), m.AccountTypeId),
                        Status =  m.Status,
                        StatusType = Enum.GetName(typeof(ClientAccountStatus), m.Status),
                    });
                });

                return retList;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RegisteredClientAccountListReportObj>();
            }
        }
        
        internal Church GetClientChurch(long clientId)
        {
            try
            {
                var myItem = _repository.GetById(clientId);
                if (myItem == null || myItem.ClientId < 1) { return null; }

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

        internal ChurchThemeSetting GetClientChurchThemeInfo(long clientId)
        {
            try
            {
                var myChurch = GetClientChurch(clientId);
                if (myChurch == null || myChurch.ChurchId < 1) { return null; }

                // Get Church Theme Info
                string msg;
                //var churchThemeInfo = ServiceChurch.GetChurchThemeSettingByChurchId(myChurch.ChurchId, out msg);
                var churchThemeInfo = new ChurchThemeSettingRepository().GetChurchThemeSettingByChurchId(myChurch.ChurchId, out msg);
                if (churchThemeInfo == null || churchThemeInfo.ChurchThemeSettingId < 1) { return null; }
                return churchThemeInfo;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
        
        private Client GetClient(long clientId, out string msg)
        {
            try
            {

                var serachClientId = '%' + clientId + '%';
                var churchId = 0;
                var stateId = 0;
                var sql =
                         string.Format(
                             "Select * FROM \"ICASDB\".\"StructureChurchHeadQuarter\" " +
                             "WHERE \"ChurchId\" = {0} " +
                             "AND \"StateOfLocationId\" = {1} AND " +
                             "\"Parish\" LIKE {2};", churchId, stateId, serachClientId);
                
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchAPPDB\".\"Client\"  WHERE \"ClientId\" = {0};", clientId);

                var check = _repository.RepositoryContext().Database.SqlQuery<Client>(sql1).ToList();
                if (check.IsNullOrEmpty())
                {
                    msg = "No Church Record Found!";
                    return null;
                }
                if (check.Count != 1)
                {
                    msg = "Invalid Church Record!";
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

        internal Client GetClient(long clientId)
        {
            try
            {
                var myItem = _repository.GetById(clientId);
                if (myItem == null || myItem.ClientId < 1) { return null; }
                return myItem;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
        

        private ClientAccount GetClientAccount(long clientAccountId, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchAPPDB\".\"ClientAccount\"  WHERE \"ClientAccountId\" = {0};", clientAccountId);

                var check = _repository.RepositoryContext().Database.SqlQuery<ClientAccount>(sql1).ToList();
                if (check.IsNullOrEmpty())
                {
                    msg = "No Client account record found!";
                    return null;
                }
                if (check.Count != 1)
                {
                    msg = "Invalid Client account Record!";
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

        private bool IsDuplicate(long churchId, string crn, string phoneNo, out string msg, int status = 0)
        {
            try
            {
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
                if (check.IsNullOrEmpty() && (check2 == null || check2.IsNullOrEmpty()) ) return false;

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
        
        private bool IsAccountDuplicate(string accountNo, out string msg, int status = 0)
        {
            try
            {
                // Query for Client Account No
                var sql1 =
                        string.Format("Select * FROM \"ChurchAPPDB\".\"ClientAccount\"  WHERE \"AccountNumber\" = '{0}'", accountNo);

                var check = _repository.RepositoryContext().Database.SqlQuery<ClientAccount>(sql1).ToList();
                msg = "";
                if (check.IsNullOrEmpty()) return false;

                switch (status)
                {
                    case 0:
                        
                        if (check.Count > 0)
                        {
                            msg = "Duplicate Error! Account Number already exist/been used by another church";
                            return true;
                        }
                        break;

                    case 1:
                        
                        if (check.Count > 1)
                        {
                            msg = "Duplicate Error! Account Number already exist/been used by another church";
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



        internal List<ClientStructureChurchDetailReportObj> GetThisClientRegisteredStructureChurchListObjs(long clientId)
        {
            try
            {
                var myItemList = _clientChurchStructureDetailRepository.GetAll(x => x.ClientId == clientId).ToList();
                if (!myItemList.Any() || myItemList.Count < 1) return new List<ClientStructureChurchDetailReportObj>();

                var retList = new List<ClientStructureChurchDetailReportObj>();
                myItemList.ForEachx(m =>
                {

                    var region = new Region();
                    var province = new Province();
                    var zone = new Zone();
                    var area = new Area();
                    var diocese = new Diocese();
                    var district = new District();
                    var state = new State();
                    var group = new Group();


                    var structureType = ServiceChurch.GetChurchStructureTypeNameById(m.ChurchStructureTypeId);
                    var structureChurchName = "";
                    switch (structureType)
                    {
                        case "Region":
                            var regionObj = _regionRepository.GetById(m.StructureChurchId);
                            region = regionObj;
                            structureChurchName = regionObj.Name;
                            break;
                        case "Province":
                            var provinceObj = _provinceRepository.GetById(m.StructureChurchId);
                            province = provinceObj;
                            structureChurchName = provinceObj.Name;
                            break;
                        case "Zone":
                            var zoneObj = _zoneRepository.GetById(m.StructureChurchId);
                            zone = zoneObj;
                            structureChurchName = zoneObj.Name;
                            break;
                        case "Area":
                            var areaObj = _areaRepository.GetById(m.StructureChurchId);
                            area = areaObj;
                            structureChurchName = areaObj.Name;
                            break;
                        case "Diocese":
                            var dioceseObj = _dioceseRepository.GetById(m.StructureChurchId);
                            diocese = dioceseObj;
                            structureChurchName = dioceseObj.Name;
                            break;
                        case "District":
                            var districtObj = _districtRepository.GetById(m.StructureChurchId);
                            district = districtObj;
                            structureChurchName = districtObj.Name;
                            break;
                        case "State":
                            var stateObj = _stateRepository.GetById(m.StructureChurchId);
                            state = stateObj;
                            structureChurchName = stateObj.Name;
                            break;
                        case "Group":
                            var groupObj = _groupRepository.GetById(m.StructureChurchId);
                            group = groupObj;
                            structureChurchName = groupObj.Name;
                            break;
                    }

                    retList.Add(new ClientStructureChurchDetailReportObj
                    {
                        ClientId = clientId,
                        StructureChurchId = m.StructureChurchId,
                        StructureChurchTypeId = m.ChurchStructureTypeId,
                        StructureChurchType = structureType,
                        StructureChurchName = structureChurchName,
                        Region = region,
                        Province = province,
                        Zone = zone,
                        Area = area,
                        Diocese = diocese,
                        District = district,
                        State = state,
                        Group = group,
                    });
                });

                return retList;

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ClientStructureChurchDetailReportObj>();
            }
        }
        
        private List<ClientStructureChurchDetail> GetClientStructureDetails(ClientStructureChurchRegistrationObj clientStructureChurchRegObj)
        {
            var clientChurchStructureDetails = new List<ClientStructureChurchDetail>();
            try
            {
                if (clientStructureChurchRegObj == null || clientStructureChurchRegObj.ClientId < 1)
                {
                    return null;
                }

                var structureIds = new List<long>();
                var structureTypeIds = new List<int>();

                if (clientStructureChurchRegObj.RegionId > 0)
                {
                    structureIds.Add(clientStructureChurchRegObj.RegionId.GetValueOrDefault());
                    structureTypeIds.Add(ServiceChurch.GetChurchStructureTypeIdByName("Region"));
                }
                if (clientStructureChurchRegObj.ProvinceId > 0)
                {
                    structureIds.Add(clientStructureChurchRegObj.ProvinceId.GetValueOrDefault());
                    structureTypeIds.Add(ServiceChurch.GetChurchStructureTypeIdByName("Province"));
                }
                if (clientStructureChurchRegObj.ZoneId > 0)
                {
                    structureIds.Add(clientStructureChurchRegObj.ZoneId.GetValueOrDefault());
                    structureTypeIds.Add(ServiceChurch.GetChurchStructureTypeIdByName("Zone"));
                }
                if (clientStructureChurchRegObj.AreaId > 0)
                {
                    structureIds.Add(clientStructureChurchRegObj.AreaId.GetValueOrDefault());
                    structureTypeIds.Add(ServiceChurch.GetChurchStructureTypeIdByName("Area"));
                }
                if (clientStructureChurchRegObj.DioceseId > 0)
                {
                    structureIds.Add(clientStructureChurchRegObj.DioceseId.GetValueOrDefault());
                    structureTypeIds.Add(ServiceChurch.GetChurchStructureTypeIdByName("Diocese"));
                }
                if (clientStructureChurchRegObj.DistrictId > 0)
                {
                    structureIds.Add(clientStructureChurchRegObj.DistrictId.GetValueOrDefault());
                    structureTypeIds.Add(ServiceChurch.GetChurchStructureTypeIdByName("District"));
                }
                if (clientStructureChurchRegObj.StateId > 0)
                {
                    structureIds.Add(clientStructureChurchRegObj.StateId.GetValueOrDefault());
                    structureTypeIds.Add(ServiceChurch.GetChurchStructureTypeIdByName("State"));
                }
                if (clientStructureChurchRegObj.GroupId > 0)
                {
                    structureIds.Add(clientStructureChurchRegObj.GroupId.GetValueOrDefault());
                    structureTypeIds.Add(ServiceChurch.GetChurchStructureTypeIdByName("Group"));
                }


                for (var i = 0; i <= structureIds.Count - 1; i++)
                {
                    var structureTypeId = structureTypeIds[i];
                    var structureDetail = new ClientStructureChurchDetail
                    {
                        ClientId = clientStructureChurchRegObj.ClientId,
                        ChurchId = clientStructureChurchRegObj.ChurchId,
                        StructureChurchId = structureIds[i],
                        ChurchStructureTypeId = structureTypeId,
                    };

                    //var churchId = structureDetail.ChurchId;
                    //var clientId = structureDetail.ClientId;
                    //var churchStructureId = structureDetail.StructureChurchId;
                    //var typeId = structureDetail.ChurchStructureTypeId;


                    //IsClientChurchStructureDetailDuplicate(long churchId, long clientId, long churchStructureId, int structureTypeId, out string msg)
                    //string msg;
                    //if (ServiceChurch.IsClientChurchStructureDetailDuplicate(churchId, clientId, churchStructureId,
                    //    typeId, out msg))
                    //{
                    //    continue;
                    //}
                    clientChurchStructureDetails.Add(structureDetail);
                }

                return clientChurchStructureDetails;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ChurchStructureHqtr GetChurchStructureHqtr(ClientStructureChurchRegistrationObj regObj, out string msg, ChurchStructureHqtr hqrtObj, int callerType = 0)
        {
            try
            {

                if (hqrtObj == null || regObj.HeadQuarterChurchStructureTypeId < 1)
                {
                    //duplicate = false;
                    msg = "";
                    return null;
                }
                var structureName = ServiceChurch.GetChurchStructureTypeNameById(regObj.HeadQuarterChurchStructureTypeId.GetValueOrDefault());
                long hqtrStructureId = 0;

                switch (structureName)
                {
                    case "Region":
                        if (regObj.RegionId > 0)
                        {
                            hqtrStructureId = regObj.RegionId.GetValueOrDefault();
                        }
                        break;

                    case "Province":
                        if (regObj.ProvinceId > 0)
                        {
                            hqtrStructureId = regObj.ProvinceId.GetValueOrDefault();
                        }
                        break;

                    case "Area":
                        if (regObj.AreaId > 0)
                        {
                            hqtrStructureId = regObj.AreaId.GetValueOrDefault();
                        }
                        break;

                    case "Zone":
                        if (regObj.ZoneId > 0)
                        {
                            hqtrStructureId = regObj.ZoneId.GetValueOrDefault();
                        }
                        break;

                    case "Diocese":
                        if (regObj.DioceseId > 0)
                        {
                            hqtrStructureId = regObj.DioceseId.GetValueOrDefault();
                        }
                        break;

                    case "District":
                        if (regObj.DistrictId > 0)
                        {
                            hqtrStructureId = regObj.DistrictId.GetValueOrDefault();
                        }
                        break;

                    case "State":
                        if (regObj.StateId > 0)
                        {
                            hqtrStructureId = regObj.StateId.GetValueOrDefault();
                        }
                        break;

                    case "Group":
                        if (regObj.GroupId > 0)
                        {
                            hqtrStructureId = regObj.GroupId.GetValueOrDefault();
                        }
                        break;
                }

                switch (callerType)
                {
                    case 0:
                        hqrtObj.ChurchId = regObj.ChurchId;
                        hqrtObj.ClientId = regObj.ClientId;
                        hqrtObj.StructureDetailId = hqtrStructureId;
                        hqrtObj.ChurchStructureTypeId = regObj.HeadQuarterChurchStructureTypeId.GetValueOrDefault();
                        //hqrtObj.RegisteredByUserId = regObj.RegisteredByUserId;
                        //hqrtObj.TimeStampRegistered = DateScrutnizer.CurrentTimeStamp();
                        break;

                    case 1:
                        hqrtObj.StructureDetailId = hqtrStructureId;
                        hqrtObj.ChurchStructureTypeId = regObj.HeadQuarterChurchStructureTypeId.GetValueOrDefault();
                        break;
                }

                // Check Duplicate 
                //var checkDuplicate = ServiceChurch.IsChurchStructureHqtrDuplicate(hqrtObj, out msg, callerType);
                //duplicate = checkDuplicate;
                msg = "";
                return hqrtObj;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                //duplicate = true;
                msg = ex.Message;
                return null;
            }
        }

        internal List<RegisteredClientStructureChurchListReportObj> GetAllRegisteredClientStructureChurchListObjs()
        {
            try
            {
                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) return new List<RegisteredClientStructureChurchListReportObj>();

                var retList = new List<RegisteredClientStructureChurchListReportObj>();
                //var thisClientStructureDetails = new List<ClientChurchStructureDetail>();
                //var clientChurchStructureHqtr = new ChurchStructureHqtr();
                myItemList.ForEachx(m =>
                {
                    string msg;
                    //var clientContact = _clientContactRepository.GetById(m.ClientId);
                    //var clientAccount = _clientAccountRepository.GetById(m.ClientId);
                    //var clientProfile = PortalClientUser.GetClientProfileByClientId(m.ClientId);

                    // Needed One
                    var clientParentChurch = new ChurchRepository().GetChurch(m.ChurchId);


                    // Needed One
                    // Get Client Structure Headquarter Status & Info
                    var clientChurchStructureHqtr = ServiceChurch.GetChurchStructureHqtrByClientId(m.ClientId);


                    // Needed One
                    #region ClientStructureDetails Collection

                    // Get Client Structure Details
                    var thisClientStructureDetails = ServiceChurch.GetClientChurchStructureDetailsByClientId(m.ClientId);

                    var region = new Region();
                    var province = new Province();
                    var zone = new Zone();
                    var area = new Area();
                    var diocese = new Diocese();
                    var district = new District();
                    var state = new State();
                    var group = new Group();
                    if (thisClientStructureDetails != null)
                    {

                        foreach (var clientChurchStructureDetail in thisClientStructureDetails)
                        {
                            var structureType = ServiceChurch.GetChurchStructureTypeNameById(clientChurchStructureDetail.ChurchStructureTypeId);
                            switch (structureType)
                            {
                                case "Region":
                                    var regionObj = _regionRepository.GetById(clientChurchStructureDetail.StructureChurchId);
                                    region = new Region { RegionId = regionObj.RegionId, ChurchId = regionObj.ChurchId, Name = regionObj.Name };
                                    break;
                                case "Province":
                                    var provinceObj = _provinceRepository.GetById(clientChurchStructureDetail.StructureChurchId);
                                    province = new Province { ProvinceId = provinceObj.ProvinceId, ChurchId = provinceObj.ChurchId, Name = provinceObj.Name };
                                    break;
                                case "Zone":
                                    var zoneObj = _zoneRepository.GetById(clientChurchStructureDetail.StructureChurchId);
                                    zone = new Zone { ZoneId = zoneObj.ZoneId, ChurchId = zoneObj.ChurchId, Name = zoneObj.Name };
                                    break;
                                case "Area":
                                    var areaObj = _areaRepository.GetById(clientChurchStructureDetail.StructureChurchId);
                                    area = new Area { AreaId = areaObj.AreaId, ChurchId = areaObj.ChurchId, Name = areaObj.Name };
                                    break;
                                case "Diocese":
                                    var dioceseObj = _dioceseRepository.GetById(clientChurchStructureDetail.StructureChurchId);
                                    diocese = new Diocese { DioceseId = dioceseObj.DioceseId, ChurchId = dioceseObj.ChurchId, Name = dioceseObj.Name };
                                    break;
                                case "District":
                                    var districtObj = _districtRepository.GetById(clientChurchStructureDetail.StructureChurchId);
                                    district = new District { DistrictId = districtObj.DistrictId, ChurchId = districtObj.ChurchId, Name = districtObj.Name };
                                    break;
                                case "State":
                                    var stateObj = _stateRepository.GetById(clientChurchStructureDetail.StructureChurchId);
                                    state = new State { StateId = stateObj.StateId, ChurchId = stateObj.ChurchId, Name = stateObj.Name };
                                    break;
                                case "Group":
                                    var groupObj = _groupRepository.GetById(clientChurchStructureDetail.StructureChurchId);
                                    group = new Group { GroupId = groupObj.GroupId, ChurchId = groupObj.ChurchId, Name = groupObj.Name };
                                    break;

                            }
                        }

                    }
                    #endregion

                    retList.Add(new RegisteredClientStructureChurchListReportObj
                    {
                        ClientId = m.ClientId,
                        ChurchId = m.ChurchId,
                        ParentChurchName = clientParentChurch.Name,
                        Name = m.Name,

                        // Needed One
                        #region ClientStructures
                        ChurchStructureHqtr = (clientChurchStructureHqtr ?? new ChurchStructureHqtr()),
                        HeadQuarterChurchStructureTypeId = (clientChurchStructureHqtr != null && clientChurchStructureHqtr.ChurchStructureTypeId > 0 ? clientChurchStructureHqtr.ChurchStructureTypeId : 0),
                        ClientStructureChurchDetail = (thisClientStructureDetails ?? new List<ClientStructureChurchDetail>()),
                        Region = region,
                        Province = province,
                        Zone = zone,
                        Area = area,
                        Diocese = diocese,
                        District = district,
                        State = state,
                        Group = group,

                        #endregion

                        
                        //Pastor = m.Pastor,
                        //PhoneNumber = m.PhoneNumber,
                        //Email = m.Email,
                        //Address = m.Address,
                        //StateOfLocation = _stateOfLocationRepository.GetById(m.StateOfLocationId).Name,
                        //StateOfLocationId = m.StateOfLocationId,
                        //BankId = clientAccount.BankId,
                        //BankName = _bankRepository.GetById(clientAccount.BankId).Name,
                        //AccountName = clientAccount.AccountName,
                        //AccountNumber = clientAccount.AccountNumber,
                        //AccountType = Enum.GetName(typeof(AccountType), clientAccount.AccountTypeId),
                        //AccountTypeId = clientAccount.AccountTypeId,
                        //Title = Enum.GetName(typeof(Title), m.Title),
                        //TitleId = m.Title,
                        //Sex = Enum.GetName(typeof(Sex), m.Sex),
                        //SexId = m.Sex,
                        //ChurchReferenceNumber = m.ChurchReferenceNumber,

                        #region Contact
                        //Contact = clientContact,
                        #endregion
                        #region Account
                        //Account = clientAccount,
                        #endregion
                        #region ClientProfile
                        //Username = clientProfile.Username,
                        #endregion


                        
                    });
                });

                return retList.FindAll(x => x.ChurchId > 2);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RegisteredClientStructureChurchListReportObj>();
            }
        }

        
        
        internal ClientStructureTaskResponseObj ResetClientStructureChurch(long clientId)
        {
            var response = new ClientStructureTaskResponseObj
            {
                ClientId = clientId,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };
            try
            {

                if (clientId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Target Client information is invalid";
                    response.Status.Message.TechnicalMessage = "Target Client cannot be found!";
                    return response;
                }


                using (var db = _uoWork.BeginTransaction())
                {
                    try
                    {

                        #region Structure Church Details
                        if (!IsClientStructureChurchDetailExist(clientId))
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Target Client structure information cannot be found!";
                            response.Status.Message.TechnicalMessage = "Target Client structure information cannot be found!";
                            return response;
                        }

                        var sql1 =
                            String.Format(
                                "Delete FROM \"ChurchAPPDB\".\"ClientChurchStructureDetail\"  WHERE \"ClientId\" = {0}",
                                clientId);

                        if (_clientChurchStructureDetailRepository.RepositoryContext().Database.ExecuteSqlCommand(sql1) < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                            response.Status.Message.TechnicalMessage = "Reset Failed";
                            return response;
                        }
                        #endregion

                        #region Structure Church Hqtr Details
                        if (!IsClientStructureChurchHqtrDetailExist(clientId))
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Target Client structure information cannot be found!";
                            response.Status.Message.TechnicalMessage = "Target Client structure information cannot be found!";
                            return response;
                        }

                        var sql2 =
                            String.Format(
                                "Delete FROM \"ChurchAPPDB\".\"ChurchStructureHqtr\"  WHERE \"ClientId\" = {0};", clientId);

                        if (_churchStructureHqtrRepository.RepositoryContext().Database.ExecuteSqlCommand(sql2) < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                            response.Status.Message.TechnicalMessage = "Reset Failed";
                            return response;
                        }
                        #endregion
                        

                        db.Commit();
                        response.Status.Message.FriendlyMessage = "";
                        response.Status.Message.TechnicalMessage = "";
                        response.Status.IsSuccessful = true;
                        return response;
                        
                    }
                    catch (DbEntityValidationException ex)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage =
                            "Unable to complete your request due to error! Please try again later";
                        response.Status.Message.TechnicalMessage = "Error" + ex.Message;
                        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                        return response;
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage =
                             "Unable to complete your request due to error! Please try again later";
                        response.Status.Message.TechnicalMessage = "Error" + ex.Message;
                        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                        return response;
                    }
                }
                
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


        private bool DeleteClientChurchStructureDetails(long clientId, IEnumerable<ClientStructureChurchDetail> clientChurchStructureDetails)
        {
            try
            {

                if (clientId > 0 && clientChurchStructureDetails != null)
                {

                    if (IsClientStructureChurchDetailExist(clientId))
                    {
                        var sql1 =
                             String.Format(
                                 "Delete FROM \"ChurchAPPDB\".\"ClientChurchStructureDetail\"  WHERE \"ClientId\" = {0};", clientId);

                        if (_clientChurchStructureDetailRepository.RepositoryContext().Database.ExecuteSqlCommand(sql1) < 1)
                        {
                            return false;
                        }
                    }

                    var sql2 = new StringBuilder();
                    foreach (var item in clientChurchStructureDetails)
                    {
                        sql2.AppendLine(
                            string.Format(
                                "Insert into  \"ChurchAPPDB\".\"ClientChurchStructureDetail\"(\"ChurchId\", \"ClientId\", \"StructureChurchId\", \"ChurchStructureTypeId\") Values({0}, {1}, {2}, {3});",
                                item.ChurchId, clientId, item.StructureChurchId, item.ChurchStructureTypeId));
                    }
                    return _clientChurchStructureDetailRepository.RepositoryContext().Database.ExecuteSqlCommand(sql2.ToString()) > 0;
                }

                return false;

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        private bool IsClientStructureChurchDetailExist(long clientId)
        {
            if (clientId < 1)
            {
                return false;
            }

            var check = _clientChurchStructureDetailRepository.GetAll(x => x.ClientId == clientId).ToList();
            if (!check.Any() || check.Count < 1)
            {
                return false;
            }

            return true;

        }

        private bool IsClientStructureChurchHqtrDetailExist(long clientId)
        {
            if (clientId < 1)
            {
                return false;
            }

            var check = _churchStructureHqtrRepository.GetAll(x => x.ClientId == clientId).ToList();
            if (!check.Any() || check.Count < 1)
            {
                return false;
            }

            return true;

        }

        private bool DeleteClientChurchStructureHqtrDetails(long clientId, ChurchStructureHqtr churchStructureHqtrObj)
        {
            try
            {

                if (clientId > 0 && churchStructureHqtrObj != null)
                {


                    if (IsClientStructureChurchHqtrDetailExist(clientId))
                    {
                        var sql1 =
                             String.Format(
                                 "Delete FROM \"ChurchAPPDB\".\"ChurchStructureHqtr\"  WHERE \"ClientId\" = {0};", clientId);

                        if (_churchStructureHqtrRepository.RepositoryContext().Database.ExecuteSqlCommand(sql1) < 1)
                        {
                            return false;
                        }
                    }


                    // Work on Reseting Database Table Index
                    //if (ResetTable("ChurchStructureHqtr"))
                    //{
                    //    var sql2 = string.Format(
                    //            "Insert into  \"ChurchAPPDB\".\"ChurchStructureHqtr\"(\"ChurchId\", \"ClientId\", \"ChurchStructureTypeId\", \"StructureDetailId\") Values({0}, {1}, {2}, {3});",
                    //            churchStructureHqtrObj.ChurchId, clientId, churchStructureHqtrObj.ChurchStructureTypeId, churchStructureHqtrObj.StructureDetailId);

                    //    return _churchStructureHqtrRepository.RepositoryContext().Database.ExecuteSqlCommand(sql2) > 0;
                    //}

                    var sql2 = string.Format(
                                "Insert into  \"ChurchAPPDB\".\"ChurchStructureHqtr\"(\"ChurchId\", \"ClientId\", \"ChurchStructureTypeId\", \"StructureDetailId\") Values({0}, {1}, {2}, {3});",
                                churchStructureHqtrObj.ChurchId, clientId, churchStructureHqtrObj.ChurchStructureTypeId, churchStructureHqtrObj.StructureDetailId);

                    return _churchStructureHqtrRepository.RepositoryContext().Database.ExecuteSqlCommand(sql2) > 0;


                }

                return false;

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }






        #region Old Functions
        internal RespStatus UpdateClientx(ClientRegistrationObj client)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };

            try
            {
                if (client.Equals(null))
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your processing";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(client, out valResults))
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


                #region Personal Info, Contact & Account

                string msg;
                var thisClient = GetClient(client.ClientId, out msg);
                if (thisClient == null || thisClient.ChurchReferenceNumber.Length < 1)
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = string.IsNullOrEmpty(msg) ? "Invalid Church Information" : msg;
                    return response;
                }

                // Get this Client Contact
                //var thisClientContact = GetClientContact(client.Contact, out msg);
                //if (thisClientContact == null || string.IsNullOrEmpty(thisClientContact.PhoneNumber))
                //{
                //    response.Message.FriendlyMessage = response.Message.TechnicalMessage = string.IsNullOrEmpty(msg) ? "Invalid Church Contact Information" : msg;
                //    return response;
                //}

                // Get this Client Account
                //var thisClientAccount = GetClientAccount(client.Account, out msg);
                //if (thisClientAccount == null || string.IsNullOrEmpty(thisClientAccount.AccountNumber))
                //{
                //    response.Message.FriendlyMessage = response.Message.TechnicalMessage = string.IsNullOrEmpty(msg) ? "Invalid Church Account Information" : msg;
                //    return response;
                //}

                #endregion

                #region Client Church Structure Details

                //var clientStructureDetails = GetClientStructureDetails(client);

                #endregion

                #region Client HeadQuarter Detail

                // Get this Client HeadQuarter Detail
                var updateClientChurchStructureHqtr = new ChurchStructureHqtr();
                if (client.HeadQuarterChurchStructureTypeId > 0)
                {
                    var thisHqtrObj = GetClientChurchStructureHeadQuarterDetail(client.ClientId);
                    if (thisHqtrObj != null && thisHqtrObj.ChurchStructureHqtrId > 0)
                    {
                        bool duplicate;
                        //updateClientChurchStructureHqtr = GetChurchStructureHqtr(client, out duplicate, out msg, thisHqtrObj, 1);
                        //if (updateClientChurchStructureHqtr != null)
                        //{
                        //    updateClientChurchStructureHqtr.ChurchStructureHqtrId =
                        //    thisHqtrObj.ChurchStructureHqtrId;
                        //}

                        //if (duplicate)
                        //{
                        //    response.Message.FriendlyMessage =
                        //        response.Message.TechnicalMessage =
                        //            string.IsNullOrEmpty(msg)
                        //                ? "Duplicate church headquarter! Another church has been the headquarter"
                        //                : msg;
                        //    return response;
                        //}
                    }
                }
                #endregion


                #region Client Profile

                var thisClientProfile = GetClientProfile(client, out msg);
                if (thisClientProfile == null || string.IsNullOrEmpty(thisClientProfile.Username) || thisClientProfile.ClientChurchProfileId < 1)
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = string.IsNullOrEmpty(msg) ? "Invalid Client Profile Information" : msg;
                    return response;
                }

                //var duplicateClientProfile = PortalClientUser.IsDuplicate(client.Username, client.PhoneNumber, out msg, 1);
                //if (duplicateClientProfile)
                //{
                //    response.Message.FriendlyMessage =
                //        response.Message.TechnicalMessage =
                //            string.IsNullOrEmpty(msg) ? "Duplicate Error! Client profile exist already" : msg;
                //    return response;
                //}
                #endregion

                client.PhoneNumber = CleanMobile(client.PhoneNumber);
                //if (IsDuplicate(client.ChurchId, thisClient.ChurchReferenceNumber, thisClientContact.PhoneNumber, thisClientAccount.AccountNumber, out msg, 1))
                //{
                //    response.Message.FriendlyMessage = msg;
                //    response.Message.TechnicalMessage = msg;
                //    response.IsSuccessful = false;
                //    return response;
                //}

                using (var db = _uoWork.BeginTransaction())
                {
                    try
                    {

                        //if (clientStructureDetails != null)
                        //{
                        //    if (clientStructureDetails.Any())
                        //    {
                        //        if (!DeleteClientChurchStructureDetails(client.ClientId, clientStructureDetails))
                        //        {
                        //            db.Rollback();
                        //            response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                        //            return response;
                        //        }
                        //    }
                        //}

                        thisClient.ChurchId = client.ChurchId;
                        thisClient.Name = client.Name;
                        thisClient.Pastor = client.Pastor;
                        thisClient.Title = client.Title;
                        thisClient.Sex = client.Sex;

                        var processedClient = _repository.Update(thisClient);
                        _uoWork.SaveChanges();
                        if (processedClient.ClientId < 1)
                        {
                            db.Rollback();
                            response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                            return response;
                        }

                        #region Client Contact & Account

                        // Updating Client Contact
                        //var processedClientContact = _clientContactRepository.Update(thisClientContact);
                        //_uoWork.SaveChanges();
                        //if (processedClientContact.ClientContactId < 1)
                        //{
                        //    db.Rollback();
                        //    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                        //    return response;
                        //}

                        // Updating Client Account
                        //var processedClientAccount = _clientAccountRepository.Update(thisClientAccount);
                        //_uoWork.SaveChanges();
                        //if (processedClientAccount.ClientAccountId < 1)
                        //{
                        //    db.Rollback();
                        //    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                        //    return response;
                        //}
                        #endregion

                        #region Church Structure Headquarter

                        // Updating Client Church Structure HeadQuarter
                        if (updateClientChurchStructureHqtr != null && updateClientChurchStructureHqtr.ChurchStructureHqtrId > 0)
                        {
                            var processedChurchStructureHqtr = _churchStructureHqtrRepository.Update(updateClientChurchStructureHqtr);
                            _uoWork.SaveChanges();
                            if (processedChurchStructureHqtr.ChurchStructureHqtrId < 1)
                            {
                                db.Rollback();
                                response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                                return response;
                            }
                        }
                        #endregion

                        #region Client Profile
                        // Updating Client Profile
                        //var processedClientProfile = PortalClientUser.UpdateClientProfile(thisClientProfile);
                        //_uoWork.SaveChanges();
                        //if (!processedClientProfile.IsSuccessful)
                        //{
                        //    db.Rollback();
                        //    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                        //    return response;
                        //}
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
                response.Message.FriendlyMessage = "Unable to complete your request due to error! Please try again later";
                response.Message.TechnicalMessage = "Error" + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return response;
            }
        }

        private ClientContact GetClientContact(ClientContact clientContact, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchAPPDB\".\"ClientContact\"  WHERE \"ClientId\" = {0};", clientContact.ClientId);

                var check = _clientContactRepository.RepositoryContext().Database.SqlQuery<ClientContact>(sql1).ToList();
                if (check.IsNullOrEmpty())
                {
                    msg = "No Church Contact Record Found!";
                    return null;
                }
                if (check.Count != 1)
                {
                    msg = "Invalid Church Contact Record!";
                    return null;
                }

                check[0].ClientId = clientContact.ClientId;
                check[0].Address = clientContact.Address;
                check[0].StateOfLocationId = clientContact.StateOfLocationId;
                check[0].Email = clientContact.Email;
                check[0].PhoneNumber = clientContact.PhoneNumber;

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

        private ClientAccount GetClientAccount(ClientAccount clientAccount, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchAPPDB\".\"ClientAccount\"  WHERE \"ClientId\" = {0};", clientAccount.ClientId);

                var check = _clientAccountRepository.RepositoryContext().Database.SqlQuery<ClientAccount>(sql1).ToList();
                if (check.IsNullOrEmpty())
                {
                    msg = "No Church Account Record Found!";
                    return null;
                }
                if (check.Count != 1)
                {
                    msg = "Invalid Church Account Record!";
                    return null;
                }

                check[0].ClientId = clientAccount.ClientId;
                check[0].BankId = clientAccount.BankId;
                check[0].AccountName = clientAccount.AccountName;
                check[0].AccountNumber = clientAccount.AccountNumber;
                check[0].AccountTypeId = clientAccount.AccountTypeId;
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
        
        public static bool ResetTable(string table)
        {
            try
            {
                var connection = ConfigurationManager.ConnectionStrings["ChurchAppDBEntities"].ConnectionString;
                if (string.IsNullOrEmpty(connection))
                {
                    return false;
                }

                using (var sqlConnection = new SqlConnection(connection))
                {
                    sqlConnection.Open();
                    var sqlBuilder = new StringBuilder();

                    sqlBuilder.AppendFormat("DBCC CHECKIDENT(\"ChurchAPPDB\".\"" + table + ", RESEED, 1");
                    string sql = sqlBuilder.ToString();
                    var command = new SqlCommand(sql, sqlConnection);
                    var dr = command.ExecuteReader();
                    if (dr.RecordsAffected > 0)
                    {
                        return true;
                    }
                    sqlConnection.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        
        internal class DefaultMigrationSqlGenerator : SqlServerMigrationSqlGenerator
        {
            protected override void Generate(AlterTableOperation alterTableOperation)
            {
                base.Generate(alterTableOperation);
                // If the tables you want to reseed have an Id primary key...
                if (alterTableOperation.Columns.Any(c => c.Name == "Id"))
                {
                    string sqlSeedReset = string.Format("DBCC CHECKIDENT ({0}, RESEED, 1000) ", alterTableOperation.Name.Replace("dbo.", ""));

                    base.Generate(new SqlOperation(sqlSeedReset));
                }
            }
        }
        
        private ClientProfileRegistrationObj GetClientProfile(ClientRegistrationObj regObj, out string msg)
        {
            try
            {
                if (regObj == null || regObj.ClientId < 1)
                {
                    //duplicate = false;
                    msg = "";
                    return null;
                }

                // Get the already registered Client Profile
                var thisClientProfile = PortalClientUser.GetClientProfile(regObj.ClientId, out msg);
                if (thisClientProfile == null || thisClientProfile.ClientProfileId < 1)
                {
                    //duplicate = false;
                    msg = "No Client Profile Record Found!";
                    return null;
                }

                msg = "";

                var clientProfile = new ClientProfileRegistrationObj
                {
                    //ClientProfileId = thisClientProfile.ClientProfileId,
                    //ClientId = thisClientProfile.ClientId,
                    Fullname = thisClientProfile.Fullname,
                    MobileNumber = thisClientProfile.MobileNumber,
                    Sex = thisClientProfile.Sex,
                    Email = thisClientProfile.Email,
                    Username = thisClientProfile.Username,
                    Password = "ChurchAdmin",
                    RegisteredByUserId = thisClientProfile.RegisteredByUserId,
                };


                return new ClientProfileRegistrationObj
                {
                    //ClientProfileId = clientProfile.ClientProfileId,
                    //ClientId = clientProfile.ClientId,
                    Fullname = clientProfile.Fullname,
                    MobileNumber = clientProfile.MobileNumber,
                    Sex = clientProfile.Sex,
                    Email = clientProfile.Email,
                    Username = clientProfile.Username,
                    Password = clientProfile.Password,
                    RegisteredByUserId = clientProfile.RegisteredByUserId
                };

                //thisClientProfile.Fullname = regObj.Pastor;
                //thisClientProfile.MobileNumber = regObj.PhoneNumber;
                //thisClientProfile.Sex = regObj.Sex;
                //thisClientProfile.Email = regObj.Email;
                //thisClientProfile.Username = regObj.Username;
                //return thisClientProfile;
                // Check Duplicate before calling next method, 
                //var duplicateCheck = ClientUser.IsDuplicate(regObj.Username, regObj.PhoneNumber, out msg, status);
                //if (duplicateCheck)
                //{
                //    duplicate = true;
                //    return null;
                //}

                //return churchStructureHqtr;


                //duplicate = true;
                //msg = "Unable to get Client profile & check duplicate! Please try again later";
                //return null;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                //duplicate = true;
                msg = ex.Message;
                return null;
            }
        }

        private ChurchStructureHqtr GetClientChurchStructureHeadQuarterDetail(long clientId)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchAPPDB\".\"ChurchStructureHqtr\"  WHERE \"ClientId\" = {0};", clientId);

                var check = _repository.RepositoryContext().Database.SqlQuery<ChurchStructureHqtr>(sql1).ToList();
                if (check.IsNullOrEmpty())
                {
                    //msg = "No Church Account Record Found!";
                    return null;
                }
                if (check.Count != 1)
                {
                    //msg = "Invalid Church Account Record!";
                    return null;
                }
                //msg = "";
                return check[0];
            }
            catch (Exception ex)
            {
                //msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }


        internal List<RegisteredClientReportObj> GetClientObjs()
        {
            try
            {
                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) return null;

                var retList = new List<RegisteredClientReportObj>();
                myItemList.ForEachx(m =>
                {
                    string msg;
                    var clientContact = _clientContactRepository.GetById(m.ClientId);
                    var clientAccount = _clientAccountRepository.GetById(m.ClientId);
                    var clientProfile = PortalClientUser.GetClientProfileByClientId(m.ClientId);
                    var clientParentChurch = new ChurchRepository().GetChurch(m.ChurchId);

                    //var regionItmes = _regionRepository.GetAll(x => x.ClientId == m.ClientId).ToList();
                    var region = GetClientThisStructure("Region", m.ClientId);
                    var province = GetClientThisStructure("Province", m.ClientId);
                    var zone = GetClientThisStructure("Zone", m.ClientId);
                    var area = GetClientThisStructure("Area", m.ClientId);
                    var diocese = GetClientThisStructure("Diocese", m.ClientId);
                    retList.Add(new RegisteredClientReportObj
                    {
                        ClientId = m.ClientId,
                        ChurchId = m.ChurchId,
                        Name = m.Name,
                        ParentChurchName = clientParentChurch.Name,
                        Pastor = m.Pastor,
                        PhoneNumber = clientContact.PhoneNumber,
                        Email = clientContact.Email,
                        Address = clientContact.Address,
                        StateOfLocation = _stateOfLocationRepository.GetById(clientContact.StateOfLocationId).Name,
                        StateOfLocationId = clientContact.StateOfLocationId,
                        BankId = clientAccount.BankId,
                        BankName = _bankRepository.GetById(clientAccount.BankId).Name,
                        AccountName = clientAccount.AccountName,
                        AccountNumber = clientAccount.AccountNumber,
                        AccountType = Enum.GetName(typeof(AccountType), clientAccount.AccountTypeId),
                        AccountTypeId = clientAccount.AccountTypeId,
                        Title = Enum.GetName(typeof(Title), m.Title),
                        TitleId = m.Title,
                        Sex = Enum.GetName(typeof(Sex), m.Sex),
                        SexId = m.Sex,
                        ChurchReferenceNumber = m.ChurchReferenceNumber,

                        #region Contact
                        Contact = clientContact,
                        #endregion
                        #region Contact
                        Account = clientAccount,
                        #endregion
                        #region ClientProfile
                        Username = clientProfile.Username,
                        #endregion

                        #region ClientStructures
                        Region = region,
                        Province = province,
                        Zone = zone,
                        Area = area,
                        Diocese = diocese,
                        //Region = new ClientChurchStructureRegObj
                        //{
                        //    ClientId = region.ClientId,
                        //    Name = region.Name,
                        //    StructureId = region.RegionId,
                        //    PhoneNumber = region.PhoneNumber,
                        //    Email = region.Email,
                        //    Address = region.Address,
                        //    StateOfLocationId = region.StateOfLocationId,
                        //},
                        //Province = new ClientChurchStructureRegObj
                        //{
                        //    ClientId = province.ClientId,
                        //    Name = province.Name,
                        //    StructureId = province.ProvinceId,
                        //    PhoneNumber = province.PhoneNumber,
                        //    Email = province.Email,
                        //    Address = province.Address,
                        //    StateOfLocationId = province.StateOfLocationId,
                        //}
                        //Region = AutoMapper.Mapper.Map<ClientChurchStructureRegObj>(region),
                        //Province = AutoMapper.Mapper.Map<ClientChurchStructureRegObj>(province),


                        #endregion

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
        
        internal List<RegisteredClientListReportObjxx> GetAllRegisteredClientObjxx()
        {
            try
            {
                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) return null;

                var retList = new List<RegisteredClientListReportObjxx>();
                //var thisClientStructureDetails = new List<ClientChurchStructureDetail>();
                //var clientChurchStructureHqtr = new ChurchStructureHqtr();
                myItemList.ForEachx(m =>
                {
                    string msg;
                    var clientContact = _clientContactRepository.GetById(m.ClientId);
                    var clientAccount = _clientAccountRepository.GetById(m.ClientId);
                    var clientProfile = PortalClientUser.GetClientProfileByClientId(m.ClientId);
                    var clientParentChurch = new ChurchRepository().GetChurch(m.ChurchId);


                    // Get Client Structure Headquarter Status & Info
                    //clientChurchStructureHqtr = ServiceChurch.GetChurchStructureHqtrByClientId(m.ClientId);
                    var clientChurchStructureHqtr = ServiceChurch.GetChurchStructureHqtrByClientId(m.ClientId);
                    #region ClientStructureDetails Collection

                    // Get Client Structure Details
                    //thisClientStructureDetails = ServiceChurch.GetClientChurchStructureDetailsByClientId(m.ClientId);
                    var thisClientStructureDetails = ServiceChurch.GetClientChurchStructureDetailsByClientId(m.ClientId);

                    var region = new Region();
                    var province = new Province();
                    var zone = new Zone();
                    var area = new Area();
                    var diocese = new Diocese();
                    var district = new District();
                    var state = new State();
                    var group = new Group();
                    if (thisClientStructureDetails != null)
                    {

                        foreach (var clientChurchStructureDetail in thisClientStructureDetails)
                        {
                            var structureType = ServiceChurch.GetChurchStructureTypeNameById(clientChurchStructureDetail.ChurchStructureTypeId);
                            switch (structureType)
                            {
                                case "Region":
                                    var regionObj = _regionRepository.GetById(clientChurchStructureDetail.StructureChurchId);
                                    region = new Region { RegionId = regionObj.RegionId, ChurchId = regionObj.ChurchId, Name = regionObj.Name };
                                    break;
                                case "Province":
                                    var provinceObj = _provinceRepository.GetById(clientChurchStructureDetail.StructureChurchId);
                                    province = new Province { ProvinceId = provinceObj.ProvinceId, ChurchId = provinceObj.ChurchId, Name = provinceObj.Name };
                                    break;
                                case "Zone":
                                    var zoneObj = _zoneRepository.GetById(clientChurchStructureDetail.StructureChurchId);
                                    zone = new Zone { ZoneId = zoneObj.ZoneId, ChurchId = zoneObj.ChurchId, Name = zoneObj.Name };
                                    break;
                                case "Area":
                                    var areaObj = _areaRepository.GetById(clientChurchStructureDetail.StructureChurchId);
                                    area = new Area { AreaId = areaObj.AreaId, ChurchId = areaObj.ChurchId, Name = areaObj.Name };
                                    break;
                                case "Diocese":
                                    var dioceseObj = _dioceseRepository.GetById(clientChurchStructureDetail.StructureChurchId);
                                    diocese = new Diocese { DioceseId = dioceseObj.DioceseId, ChurchId = dioceseObj.ChurchId, Name = dioceseObj.Name };
                                    break;
                                case "District":
                                    var districtObj = _districtRepository.GetById(clientChurchStructureDetail.StructureChurchId);
                                    district = new District { DistrictId = districtObj.DistrictId, ChurchId = districtObj.ChurchId, Name = districtObj.Name };
                                    break;
                                case "State":
                                    var stateObj = _stateRepository.GetById(clientChurchStructureDetail.StructureChurchId);
                                    state = new State { StateId = stateObj.StateId, ChurchId = stateObj.ChurchId, Name = stateObj.Name };
                                    break;
                                case "Group":
                                    var groupObj = _groupRepository.GetById(clientChurchStructureDetail.StructureChurchId);
                                    group = new Group { GroupId = groupObj.GroupId, ChurchId = groupObj.ChurchId, Name = groupObj.Name };
                                    break;

                            }
                        }

                    }
                    #endregion

                    retList.Add(new RegisteredClientListReportObjxx
                    {
                        ClientId = m.ClientId,
                        ChurchId = m.ChurchId,
                        Name = m.Name,
                        ParentChurchName = clientParentChurch.Name,
                        Pastor = m.Pastor,
                        PhoneNumber = clientContact.PhoneNumber,
                        Email = clientContact.Email,
                        Address = clientContact.Address,
                        StateOfLocation = _stateOfLocationRepository.GetById(clientContact.StateOfLocationId).Name,
                        StateOfLocationId = clientContact.StateOfLocationId,
                        //BankId = clientAccount.BankId,
                        //BankName = _bankRepository.GetById(clientAccount.BankId).Name,
                        //AccountName = clientAccount.AccountName,
                        //AccountNumber = clientAccount.AccountNumber,
                        //AccountType = Enum.GetName(typeof(AccountType), clientAccount.AccountTypeId),
                        //AccountTypeId = clientAccount.AccountTypeId,
                        Title = Enum.GetName(typeof(Title), m.Title),
                        TitleId = m.Title,
                        Sex = Enum.GetName(typeof(Sex), m.Sex),
                        SexId = m.Sex,
                        ChurchReferenceNumber = m.ChurchReferenceNumber,

                        #region Contact
                        //Contact = clientContact,
                        #endregion
                        #region Account
                        //Account = clientAccount,
                        #endregion
                        #region ClientProfile
                        Username = clientProfile.Username,
                        #endregion

                        #region ClientStructures

                        ChurchStructureHqtr = (clientChurchStructureHqtr ?? new ChurchStructureHqtr()),
                        HeadQuarterChurchStructureTypeId = (clientChurchStructureHqtr != null && clientChurchStructureHqtr.ChurchStructureTypeId > 0 ? clientChurchStructureHqtr.ChurchStructureTypeId : 0),
                        ClientStructureChurchDetail = (thisClientStructureDetails ?? new List<ClientStructureChurchDetail>()),
                        Region = region,
                        Province = province,
                        Zone = zone,
                        Area = area,
                        Diocese = diocese,
                        District = district,
                        State = state,
                        Group = group,

                        #endregion

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
        
        internal ClientStructureChurchRegObj GetThisClientChurchStructure(string structureType, long structureId)
        {
            try
            {
                switch (structureType)
                {
                    case "Region":
                        var regionItem = _regionRepository.GetById(structureId);
                        if (regionItem == null || regionItem.RegionId < 1) { return null; }

                        var region = new ClientStructureChurchRegObj
                        {
                            ChurchId = regionItem.ChurchId,
                            Name = regionItem.Name,
                            StructureId = regionItem.RegionId,
                            StateOfLocationId = regionItem.StateOfLocationId,
                        };
                        return region;

                    case "Province":
                        var provinceItem = _provinceRepository.GetById(structureId);
                        if (provinceItem == null || provinceItem.ProvinceId < 1) { return null; }

                        var province = new ClientStructureChurchRegObj
                        {
                            ChurchId = provinceItem.ChurchId,
                            Name = provinceItem.Name,
                            StructureId = provinceItem.ProvinceId,
                            StateOfLocationId = provinceItem.StateOfLocationId,
                        };
                        return province;

                    case "Zone":
                        var zoneItem = _zoneRepository.GetById(structureId);
                        if (zoneItem == null || zoneItem.ZoneId < 1) { return null; }

                        var zone = new ClientStructureChurchRegObj
                        {
                            ChurchId = zoneItem.ChurchId,
                            Name = zoneItem.Name,
                            StructureId = zoneItem.ZoneId,
                            StateOfLocationId = zoneItem.StateOfLocationId,
                        };
                        return zone;

                    case "Area":
                        var areaItem = _areaRepository.GetById(structureId);
                        if (areaItem == null || areaItem.AreaId < 1) { return null; }

                        var area = new ClientStructureChurchRegObj
                        {
                            ChurchId = areaItem.ChurchId,
                            Name = areaItem.Name,
                            StructureId = areaItem.AreaId,
                            StateOfLocationId = areaItem.StateOfLocationId,
                        };
                        return area;

                    case "Diocese":
                        var dioceseItem = _dioceseRepository.GetById(structureId);
                        if (dioceseItem == null || dioceseItem.DioceseId < 1) { return null; }

                        var diocese = new ClientStructureChurchRegObj
                        {
                            ChurchId = dioceseItem.ChurchId,
                            Name = dioceseItem.Name,
                            StructureId = dioceseItem.DioceseId,
                            StateOfLocationId = dioceseItem.StateOfLocationId,
                        };
                        return diocese;

                    case "District":
                        var districtItem = _districtRepository.GetById(structureId);
                        if (districtItem == null || districtItem.DistrictId < 1) { return null; }

                        var district = new ClientStructureChurchRegObj
                        {
                            ChurchId = districtItem.ChurchId,
                            Name = districtItem.Name,
                            StructureId = districtItem.DistrictId,
                            StateOfLocationId = districtItem.StateOfLocationId,
                        };
                        return district;

                    case "State":
                        var stateItem = _stateRepository.GetById(structureId);
                        if (stateItem == null || stateItem.StateId < 1) { return null; }

                        var state = new ClientStructureChurchRegObj
                        {
                            ChurchId = stateItem.ChurchId,
                            Name = stateItem.Name,
                            StructureId = stateItem.StateId,
                            StateOfLocationId = stateItem.StateOfLocationId,
                        };
                        return state;

                    case "Group":
                        var groupItem = _groupRepository.GetById(structureId);
                        if (groupItem == null || groupItem.GroupId < 1) { return null; }

                        var group = new ClientStructureChurchRegObj
                        {
                            ChurchId = groupItem.ChurchId,
                            Name = groupItem.Name,
                            StructureId = groupItem.GroupId,
                            StateOfLocationId = groupItem.StateOfLocationId,
                        };
                        return group;

                }

                return null;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal ClientStructureChurchRegObj GetThisClientChurchStructures(long structureId, int structureTypeId)
        {

            var structureTypeName = ServiceChurch.GetChurchStructureTypeNameById(structureTypeId);

            try
            {
                switch (structureTypeName)
                {
                    case "Region":
                        var regionItem = _regionRepository.GetById(structureId);
                        if (regionItem == null || regionItem.RegionId < 1) { return null; }

                        var region = new ClientStructureChurchRegObj
                        {
                            ChurchId = regionItem.ChurchId,
                            Name = regionItem.Name,
                            StructureId = regionItem.RegionId,
                            StateOfLocationId = regionItem.StateOfLocationId,
                        };
                        return region;

                    case "Province":
                        var provinceItem = _provinceRepository.GetById(structureId);
                        if (provinceItem == null || provinceItem.ProvinceId < 1) { return null; }

                        var province = new ClientStructureChurchRegObj
                        {
                            ChurchId = provinceItem.ChurchId,
                            Name = provinceItem.Name,
                            StructureId = provinceItem.ProvinceId,
                            StateOfLocationId = provinceItem.StateOfLocationId,
                        };
                        return province;

                    case "Zone":
                        var zoneItem = _zoneRepository.GetById(structureId);
                        if (zoneItem == null || zoneItem.ZoneId < 1) { return null; }

                        var zone = new ClientStructureChurchRegObj
                        {
                            ChurchId = zoneItem.ChurchId,
                            Name = zoneItem.Name,
                            StructureId = zoneItem.ZoneId,
                            StateOfLocationId = zoneItem.StateOfLocationId,
                        };
                        return zone;

                    case "Area":
                        var areaItem = _areaRepository.GetById(structureId);
                        if (areaItem == null || areaItem.AreaId < 1) { return null; }

                        var area = new ClientStructureChurchRegObj
                        {
                            ChurchId = areaItem.ChurchId,
                            Name = areaItem.Name,
                            StructureId = areaItem.AreaId,
                            StateOfLocationId = areaItem.StateOfLocationId,
                        };
                        return area;

                    case "Diocese":
                        var dioceseItem = _dioceseRepository.GetById(structureId);
                        if (dioceseItem == null || dioceseItem.DioceseId < 1) { return null; }

                        var diocese = new ClientStructureChurchRegObj
                        {
                            ChurchId = dioceseItem.ChurchId,
                            Name = dioceseItem.Name,
                            StructureId = dioceseItem.DioceseId,
                            StateOfLocationId = dioceseItem.StateOfLocationId,
                        };
                        return diocese;

                    case "District":
                        var districtItem = _districtRepository.GetById(structureId);
                        if (districtItem == null || districtItem.DistrictId < 1) { return null; }

                        var district = new ClientStructureChurchRegObj
                        {
                            ChurchId = districtItem.ChurchId,
                            Name = districtItem.Name,
                            StructureId = districtItem.DistrictId,
                            StateOfLocationId = districtItem.StateOfLocationId,
                        };
                        return district;

                    case "State":
                        var stateItem = _stateRepository.GetById(structureId);
                        if (stateItem == null || stateItem.StateId < 1) { return null; }

                        var state = new ClientStructureChurchRegObj
                        {
                            ChurchId = stateItem.ChurchId,
                            Name = stateItem.Name,
                            StructureId = stateItem.StateId,
                            StateOfLocationId = stateItem.StateOfLocationId,
                        };
                        return state;

                    case "Group":
                        var groupItem = _groupRepository.GetById(structureId);
                        if (groupItem == null || groupItem.GroupId < 1) { return null; }

                        var group = new ClientStructureChurchRegObj
                        {
                            ChurchId = groupItem.ChurchId,
                            Name = groupItem.Name,
                            StructureId = groupItem.GroupId,
                            StateOfLocationId = groupItem.StateOfLocationId,
                        };
                        return group;

                }

                return null;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal ClientStructureChurchRegObj GetClientThisStructure(string structure, long clientId)
        {
            try
            {
                switch (structure)
                {
                    case "Region":
                    //var regionItem = _regionRepository.GetAll(m => m.ClientId == clientId).ToList();
                    //if (!regionItem.Any() || regionItem.Count() != 1) { return null; }

                    //var region = new ClientChurchStructureRegObj
                    //{
                    //    ClientId = regionItem[0].ClientId,
                    //    Name = regionItem[0].Name,
                    //    StructureId = regionItem[0].RegionId,
                    //    PhoneNumber = regionItem[0].PhoneNumber,
                    //    Email = regionItem[0].Email,
                    //    Address = regionItem[0].Address,
                    //    StateOfLocationId = regionItem[0].StateOfLocationId,
                    //};
                    //return region;

                    case "Province":
                    //var provinceItem = _provinceRepository.GetAll(m => m.ClientId == clientId).ToList();
                    //if (!provinceItem.Any() || provinceItem.Count() != 1) { return null; }

                    //var province = new ClientChurchStructureRegObj
                    //{
                    //    ClientId = provinceItem[0].ClientId,
                    //    Name = provinceItem[0].Name,
                    //    StructureId = provinceItem[0].ProvinceId,
                    //    PhoneNumber = provinceItem[0].PhoneNumber,
                    //    Email = provinceItem[0].Email,
                    //    Address = provinceItem[0].Address,
                    //    StateOfLocationId = provinceItem[0].StateOfLocationId,
                    //};
                    //return province;

                    case "Zone":
                    //var zoneItem = _zoneRepository.GetAll(m => m.ClientId == clientId).ToList();
                    //if (!zoneItem.Any() || zoneItem.Count() != 1) { return null; }

                    //var zone = new ClientChurchStructureRegObj
                    //{
                    //    ClientId = zoneItem[0].ClientId,
                    //    Name = zoneItem[0].Name,
                    //    StructureId = zoneItem[0].ZoneId,
                    //    PhoneNumber = zoneItem[0].PhoneNumber,
                    //    Email = zoneItem[0].Email,
                    //    Address = zoneItem[0].Address,
                    //    StateOfLocationId = zoneItem[0].StateOfLocationId,
                    //};
                    //return zone;

                    case "Area":
                        //var areaItem = _areaRepository.GetAll(m => m.ClientId == clientId).ToList();
                        //if (!areaItem.Any() || areaItem.Count() != 1) { return null; }

                        //var area = new ClientChurchStructureRegObj
                        //{
                        //    ClientId = areaItem[0].ClientId,
                        //    Name = areaItem[0].Name,
                        //    StructureId = areaItem[0].AreaId,
                        //    PhoneNumber = areaItem[0].PhoneNumber,
                        //    Email = areaItem[0].Email,
                        //    Address = areaItem[0].Address,
                        //    StateOfLocationId = areaItem[0].StateOfLocationId,
                        //};
                        //return area;

                        //case "Diocese":
                        //    var dioceseItem = _areaRepository.GetAll(m => m.ClientId == clientId).ToList();
                        //    if (!dioceseItem.Any() || dioceseItem.Count() != 1) { return null; }

                        //    var diocese = new ClientChurchStructureRegObj
                        //    {
                        //        ClientId = dioceseItem[0].ClientId,
                        //        Name = dioceseItem[0].Name,
                        //        StructureId = dioceseItem[0].AreaId,
                        //        PhoneNumber = dioceseItem[0].PhoneNumber,
                        //        Email = dioceseItem[0].Email,
                        //        Address = dioceseItem[0].Address,
                        //        StateOfLocationId = dioceseItem[0].StateOfLocationId,
                        //    };
                        //    return diocese;

                        //case "Diocese":
                        //    var dioceseItem = _areaRepository.GetAll(m => m.ClientId == clientId).ToList();
                        //    if (!dioceseItem.Any() || dioceseItem.Count() != 1) { return null; }

                        //    var diocese = new ClientChurchStructureRegObj
                        //    {
                        //        ClientId = dioceseItem[0].ClientId,
                        //        Name = dioceseItem[0].Name,
                        //        StructureId = dioceseItem[0].AreaId,
                        //        PhoneNumber = dioceseItem[0].PhoneNumber,
                        //        Email = dioceseItem[0].Email,
                        //        Address = dioceseItem[0].Address,
                        //        StateOfLocationId = dioceseItem[0].StateOfLocationId,
                        //    };
                        //    return diocese;

                        return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        private bool IsDuplicate1(long churchId, string crn, string phoneNo, string acctNo, out string msg, int status = 0)
        {
            try
            {
                // Query for Contact Duplicate
                var sql1 =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ClientContact\"  WHERE \"PhoneNumber\" = '{0}'",
                            phoneNo);

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

                // Check Duplicate Account Details
                List<ClientAccount> check3;
                if (!string.IsNullOrEmpty(acctNo))
                {
                    var sql2 =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ClientAccount\"  WHERE  \"AccountNumber\" = '{0}'", acctNo);
                    check3 = _repository.RepositoryContext().Database.SqlQuery<ClientAccount>(sql2).ToList();
                }
                else
                {
                    check3 = null;
                }

                var check = _repository.RepositoryContext().Database.SqlQuery<ClientContact>(sql1).ToList();
                msg = "";
                if (check.IsNullOrEmpty() && (check2 == null || check2.IsNullOrEmpty()) && (check3 == null || check3.IsNullOrEmpty())) return false;

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
                        if (check3 != null)
                        {
                            if (check3.Count > 0)
                            {
                                msg = "Duplicate Error! Account Number already been used by another Church";
                                return true;
                            }
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
                        if (check3 != null)
                        {
                            if (check3.Count > 1)
                            {
                                msg = "Duplicate Error! Account Number already been used by another Church";
                                return true;
                            }
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

        
        #endregion

        
       
        
        #region CRN Generation

        internal string GenerateCRN(out string msg, string churchShortName, int churchId)
        {
            try
            {
                // RCCG00000000001
                var lastChurchClientInfo = GetMaxClientInfo(out msg, churchId);
                if (lastChurchClientInfo == null || lastChurchClientInfo.ClientId < 1)
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

        private Client GetMaxClientInfo(out string msg, int churchId)
        {

            try
            {

                var sqlBuilder = new StringBuilder();
                sqlBuilder.AppendFormat("SELECT * FROM \"ChurchAPPDB\".\"Client\" " +
                                            " WHERE \"ChurchId\" = {0} ORDER BY \"ClientId\" DESC", churchId);
                string sql1 = sqlBuilder.ToString();

                var activity = _repository.RepositoryContext().Database.SqlQuery<Client>(sql1).ToList();
                if (activity.IsNullOrEmpty())
                {
                    msg = "";
                    return null;
                }
                msg = "";
                return activity[0];
            }
            catch (Exception ex)
            {
                msg = "Unable to  generate CRN";
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        #endregion



        #region Used Functions

        private List<ClientStructureChurchDetail> GetClientChurchStructureDetails(long clientId)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchAPPDB\".\"ClientChurchStructureDetail\"  WHERE \"ClientId\" = {0};", clientId);

                var check = _repository.RepositoryContext().Database.SqlQuery<ClientStructureChurchDetail>(sql1).ToList();
                if (check.IsNullOrEmpty())
                {
                    //msg = "No Church Account Record Found!";
                    return null;
                }
                if (check.Count != 1)
                {
                    //msg = "Invalid Church Account Record!";
                    return null;
                }
                //msg = "";
                return check;
            }
            catch (Exception ex)
            {
                //msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
        private bool IsDuplicates(long churchId, string crn, string phoneNo, int bankId, string acctNo, out string msg)
        {
            try
            {
                // Query for Contact Duplicate
                var sql1 =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ClientContact\"  WHERE \"PhoneNumber\" = '{0}'",
                            phoneNo);

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

                // Check Duplicate Account Details
                List<ClientAccount> check3;
                if (!string.IsNullOrEmpty(acctNo))
                {
                    var sql2 =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ClientAccount\"  WHERE  \"AccountNumber\" = '{0}'", acctNo);
                    check3 = _repository.RepositoryContext().Database.SqlQuery<ClientAccount>(sql2).ToList();
                }
                else
                {
                    check3 = null;
                }

                var check = _repository.RepositoryContext().Database.SqlQuery<ClientContact>(sql1).ToList();
                msg = "";
                if (check.IsNullOrEmpty() && (check2 == null || check2.IsNullOrEmpty()) && (check3 == null || check3.IsNullOrEmpty())) return false;
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
                if (check3 != null)
                {
                    if (check3.Count > 0)
                    {
                        msg = "Duplicate Error! Account Number already been used by another Church";
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
        private bool IsChurchInfoDuplicate(long churchId, string crn, out string msg)
        {
            try
            {
                List<Client> check;
                if (!string.IsNullOrEmpty(crn) && churchId > 0)
                {
                    var sql1 =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"Client\"  WHERE \"ChurchId\" = {0} AND \"ChurchReferenceNumber\" = '{1}'",
                            churchId, crn);
                    check = _repository.RepositoryContext().Database.SqlQuery<Client>(sql1).ToList();
                }
                else
                {
                    check = null;
                }

                msg = "";
                if (check.IsNullOrEmpty()) return false;
                if (check != null)
                {
                    if (check.Count > 0)
                    {
                        msg = "Church already exist";
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
        private bool IsChurchContactDuplicate(string phoneNo, out string msg)
        {
            try
            {
                List<ClientContact> check;
                if (!string.IsNullOrEmpty(phoneNo))
                {
                    var sql1 =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ClientContact\"  WHERE \"PhoneNumber\" = '{0}'",
                            phoneNo);
                    check = _repository.RepositoryContext().Database.SqlQuery<ClientContact>(sql1).ToList();
                }
                else
                {
                    check = null;
                }

                msg = "";
                if (check.IsNullOrEmpty()) return false;
                if (check != null)
                {
                    if (check.Count > 0)
                    {
                        msg = "Phone Number already been used by another client/church";
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
        private bool IsChurchAccountDuplicate(int bankId, string acctNo, out string msg)
        {
            try
            {
                List<ClientAccount> check;
                if (!string.IsNullOrEmpty(acctNo) && bankId > 0)
                {
                    var sql1 =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ClientAccount\"  WHERE \"BankId\" = {0} AND \"AccountNumber\" = '{1}'",
                            bankId, acctNo);
                    check = _repository.RepositoryContext().Database.SqlQuery<ClientAccount>(sql1).ToList();
                }
                else
                {
                    check = null;
                }

                msg = "";
                if (check.IsNullOrEmpty()) return false;
                if (check != null)
                {
                    if (check.Count > 0)
                    {
                        msg = "Bank Account already been used by another client/church";
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
        private ChurchStructureHqtr GetChurchStructureHqtrx(long clientId, ClientRegistrationObj regObj, out bool duplicate)
        {
            try
            {
                if (regObj == null || regObj.HeadQuarterChurchStructureTypeId < 1)
                {
                    duplicate = false;
                    return null;
                }
                var structureName = ServiceChurch.GetChurchStructureTypeNameById(regObj.HeadQuarterChurchStructureTypeId.GetValueOrDefault());
                long hqtrStructureId = 0;

                //switch (structureName)
                //{
                //    case "Region":
                //        if (regObj.RegionId > 0)
                //        {
                //            hqtrStructureId = regObj.RegionId.GetValueOrDefault();
                //        }
                //        break;

                //    case "Province":
                //        if (regObj.ProvinceId > 0)
                //        {
                //            hqtrStructureId = regObj.ProvinceId.GetValueOrDefault();
                //        }
                //        break;

                //    case "Area":
                //        if (regObj.AreaId > 0)
                //        {
                //            hqtrStructureId = regObj.AreaId.GetValueOrDefault();
                //        }
                //        break;

                //    case "Zone":
                //        if (regObj.ZoneId > 0)
                //        {
                //            hqtrStructureId = regObj.ZoneId.GetValueOrDefault();
                //        }
                //        break;

                //    case "Diocese":
                //        if (regObj.DioceseId > 0)
                //        {
                //            hqtrStructureId = regObj.DioceseId.GetValueOrDefault();
                //        }
                //        break;

                //    case "District":
                //        if (regObj.DistrictId > 0)
                //        {
                //            hqtrStructureId = regObj.DistrictId.GetValueOrDefault();
                //        }
                //        break;

                //    case "State":
                //        if (regObj.StateId > 0)
                //        {
                //            hqtrStructureId = regObj.StateId.GetValueOrDefault();
                //        }
                //        break;

                //    case "Group":
                //        if (regObj.GroupId > 0)
                //        {
                //            hqtrStructureId = regObj.GroupId.GetValueOrDefault();
                //        }
                //        break;
                //}


                var churchStructureHqtr = new ChurchStructureHqtr
                {
                    ChurchId = regObj.ChurchId,
                    ClientId = clientId,
                    StructureDetailId = hqtrStructureId,
                    ChurchStructureTypeId = regObj.HeadQuarterChurchStructureTypeId.GetValueOrDefault(),
                    //RegisteredByUserId = regObj.RegisteredByUserId,
                    //TimeStampRegistered = DateScrutnizer.CurrentTimeStamp()
                };

                // Check Duplicate before calling 'Add' method, so that Church Structure Hqtr Details will not be 
                // added while the last module (Client Profile Failed)
                string msg;
                var checkDuplicate = ServiceChurch.IsChurchStructureHqtrDuplicate(churchStructureHqtr, out msg);
                duplicate = checkDuplicate;
                return churchStructureHqtr;

                //if (checkDuplicate)
                //{
                //    db.Rollback();
                //    response.Status.Message.FriendlyMessage = msg;
                //    response.Status.Message.TechnicalMessage = msg;
                //    response.Status.IsSuccessful = false;
                //    return response;
                //}


            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                duplicate = false;
                return null;
            }
        }


        #endregion
        
        
        
    }
}
