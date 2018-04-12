using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using ICASStacks.APIObjs;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.DataContract.Enum;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using ICASStacks.Repository.Helpers;
using ICASStacks.StackService;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ClientChurchServiceRepository
    {

        private readonly IIcasRepository<ClientChurchService> _repository;
        private readonly IcasUoWork _uoWork;

        public ClientChurchServiceRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ClientChurchService>(_uoWork);
        }

        internal ClientChurchServiceRegResponse AddClientChurchService(ClientChurchServiceRegObj clientChurchServiceRegObj)
        {

            var response = new ClientChurchServiceRegResponse
            {
                ClientChurchServiceId = 0,
                ClientChurchId = clientChurchServiceRegObj.ClientChurchId,
                ChurchServiceTypeId = clientChurchServiceRegObj.ChurchServiceTypeId,
                DayOfWeekId = clientChurchServiceRegObj.DayOfWeekId,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {
                if (clientChurchServiceRegObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Church registration Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(clientChurchServiceRegObj, out valResults))
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
                if (clientChurchServiceRegObj.AddedByUserId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Admin User Information";
                    response.Status.Message.TechnicalMessage = "Invalid Admin User Information";
                    response.Status.IsSuccessful = false;
                    return response;
                }


                using (var db = _uoWork.BeginTransaction())
                {
                    // First Add the Client Church Service but not make ChurchServiceTypeId required
                    // Then Add Church Service Type and get its ChurchServiceTypeId, and update 
                    // Client Church Service 'ChurchServiceTypeId'
                    // But checking for duplicate will be difficult

                    #region Latest Flow

                    // Checks the existence of this Name
                    var check = new ChurchServiceTypeRepository().IsServiceNameExist(clientChurchServiceRegObj.Name);
                    if (check != null && check.ChurchServiceTypeId > 0)
                    {

                        // Checks if the check.ChurchServiceTypeId exist under this Client ChurchService record
                        var clientChurchServiceCheck = ExistWitMe(clientChurchServiceRegObj.ClientChurchId,
                            check.ChurchServiceTypeId);
                        if (clientChurchServiceCheck != null && clientChurchServiceCheck.ClientChurchServiceId > 0)
                        {

                            // Try give message of duplicate but check if the DayOfWeekId not the same, 
                            // advice to Update instead of adding new One
                            db.Commit();
                            response.Status.Message.FriendlyMessage =
                                "Duplicate Error! Service name already exist but you can modify to change the day or better still add a new service name";
                            response.Status.IsSuccessful = false;
                            return response;
                        }

                    }

                    #endregion

                    
                    // Add Church Service Type Name -----> ChurchServiceType to get ChurchServiceTypeId
                    var churchServiceTypeObj = new ChurchServiceTypeRegObj
                    {
                        Name = clientChurchServiceRegObj.Name,
                        SourceId = ChurchSettingSource.Client
                    };
                    var processedChurchServiceType = new ChurchServiceTypeRepository().AddChurchServiceType(churchServiceTypeObj);
                    if (!processedChurchServiceType.Status.IsSuccessful || processedChurchServiceType.ChurchServiceTypeId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage =
                            response.Status.Message.TechnicalMessage = processedChurchServiceType.Status.Message.FriendlyMessage;
                        return response;
                    }

                    string msg;
                    bool reset;
                    long resetId;
                    clientChurchServiceRegObj.ChurchServiceTypeId = processedChurchServiceType.ChurchServiceTypeId;
                    if (IsDuplicate(clientChurchServiceRegObj, out reset, out resetId, out msg))
                    {
                        response.Status.Message.FriendlyMessage = msg;
                        response.Status.Message.TechnicalMessage = msg;
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    // Check if the exist status is In_Active and reset it back, don't bother to add new
                    if (reset)
                    {
                        response.ClientChurchServiceId = resetId;
                        response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                        response.Status.IsSuccessful = true;
                        return response;
                    }

                    // Add the ChurchServiceTypeId gotten from  ChurchServiceType -----> ClientChurchService
                    var clientChurchServiceObj = new ClientChurchService
                    {
                        ClientChurchId = clientChurchServiceRegObj.ClientChurchId,
                        //ChurchServiceTypeId = clientChurchServiceRegObj.ChurchServiceTypeId,
                        //DayOfWeekId = clientChurchServiceRegObj.DayOfWeekId,
                        TimeStampAdded = DateScrutnizer.CurrentTimeStamp(),
                        AddedByUserId = clientChurchServiceRegObj.AddedByUserId,
                        Status = (int)ChurchServiceStatus.Active
                    };

                    var processedClientChurchService = _repository.Add(clientChurchServiceObj);
                    _uoWork.SaveChanges();
                    if (processedClientChurchService.ClientChurchServiceId < 1)
                    {
                        db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    db.Commit();
                    response.ClientChurchServiceId = processedClientChurchService.ClientChurchServiceId;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;
                }

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

        internal RespStatus UpdateClientChurchService(ClientChurchServiceRegObj clientChurchServiceRegObj)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };

            try
            {
                if (clientChurchServiceRegObj.Equals(null))
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your processing";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(clientChurchServiceRegObj, out valResults))
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


                #region Latest Flow

                // Checks the existence of this Name
                var check = new ChurchServiceTypeRepository().IsServiceNameExist(clientChurchServiceRegObj.Name);
                if (check != null && check.ChurchServiceTypeId > 0)
                {

                    // Checks if the check.ChurchServiceTypeId exist under this Client ChurchService record
                    var clientChurchServiceCheck = ExistWitMe(clientChurchServiceRegObj.ClientChurchId,
                        check.ChurchServiceTypeId);

                    //if (clientChurchServiceCheck != null && clientChurchServiceCheck.ClientChurchServiceId > 0 &&
                    //    clientChurchServiceCheck.DayOfWeekId == clientChurchServiceRegObj.DayOfWeekId)
                    //{
                    //    // Try give message of duplicate but check if the DayOfWeekId not the same, 
                    //    // advice to Update instead of adding new One
                    //    response.Message.FriendlyMessage =
                    //        "Duplicate Error! Service name already exist but you can only modify the day or better still add a new service name";
                    //    response.IsSuccessful = false;
                    //    return response;
                    //}
                }



                #endregion




                // Update the Church Service Type
                var churchServiceTypeObj = new ChurchServiceTypeRegObj
                {
                    ChurchServiceTypeId = clientChurchServiceRegObj.ChurchServiceTypeId,
                    Name = clientChurchServiceRegObj.Name,
                    SourceId = ChurchSettingSource.Client
                };
                var processedChurchServiceType = new ChurchServiceTypeRepository().UpdateChurchServiceType(churchServiceTypeObj);
                if (!processedChurchServiceType.IsSuccessful)
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage = processedChurchServiceType.Message.FriendlyMessage;
                    return response;
                }

                string msg;
                var thisClientChurchService = GetClientChurchService(clientChurchServiceRegObj.ClientChurchServiceId, out msg);
                if (thisClientChurchService == null || thisClientChurchService.ClientChurchServiceId < 1)
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage =
                            string.IsNullOrEmpty(msg) ? "Invalid Client Church Service Information" : msg;
                    return response;
                }

                bool reset;
                long resetId;

                IsDuplicate(clientChurchServiceRegObj, out reset, out resetId, out msg, 1);

                //if (IsDuplicate(clientChurchServiceRegObj, out reset, out resetId, out msg, 1))
                //{
                //    response.Message.FriendlyMessage = msg;
                //    response.Message.TechnicalMessage = msg;
                //    response.IsSuccessful = false;
                //    return response;
                //}

                // Check if the exist status is In_Active and reset it back, don't bother to add new
                if (reset)
                {
                    thisClientChurchService.Status = (int)ChurchServiceStatus.Remove;
                }
                else
                {
                    //thisClientChurchService.DayOfWeekId = clientChurchServiceRegObj.DayOfWeekId;
                }

                var processedClientChurchService = _repository.Update(thisClientChurchService);
                _uoWork.SaveChanges();
                if (processedClientChurchService.ClientChurchServiceId < 1)
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

        internal RespStatus RemoveClientChurchService(long clientChurchServiceId)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };

            try
            {
                if (clientChurchServiceId < 1)
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your processing";
                    return response;
                }

                string msg;
                var thisClientChurchService = GetClientChurchService(clientChurchServiceId, out msg);
                //if (thisChurchService == null || thisChurchService.Name.Length < 1)
                //{
                //    response.Message.FriendlyMessage =
                //        response.Message.TechnicalMessage =
                //            string.IsNullOrEmpty(msg) ? "Invalid Church Information" : msg;
                //    return response;
                //}

                thisClientChurchService.Status = (int)ChurchServiceStatus.Remove;

                var processedClientChurchService = _repository.Update(thisClientChurchService);
                _uoWork.SaveChanges();
                if (processedClientChurchService.ClientChurchServiceId < 1)
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

        internal List<RegisteredClientChurchServiceReportObj> GetAllRegisteredClientChurchServiceObjs(long clientChurchId)
        {
            try
            {
                if (clientChurchId < 1)
                {
                    return new List<RegisteredClientChurchServiceReportObj>();
                }

                long churchId = 0;
                if (new ClientChurchRepository().GetClientChurch(clientChurchId) != null)
                {
                    churchId = new ClientChurchRepository().GetClientChurch(clientChurchId).ChurchId;
                }
                
                var retList = new List<RegisteredClientChurchServiceReportObj>();
                if (churchId > 0)
                {
                    var thisClientChurchDefaultServices = new ChurchServiceRepository().GetAllRegisteredChurchServiceObjs(churchId);

                    if (thisClientChurchDefaultServices.Any())
                    {
                        thisClientChurchDefaultServices.ForEachx(m => retList.Add(new RegisteredClientChurchServiceReportObj
                        {
                            ClientChurchServiceId = 0,
                            ClientChurchId = clientChurchId,
                            ChurchServiceTypeId = m.ChurchServiceTypeId,
                            Name = m.ChurchServiceName,
                            DayOfWeekId = m.DayOfWeekId,
                            ServiceDay = m.ServiceDay
                        }));
                    }
                }

                var myItemList = _repository.GetAll(x => x.ClientChurchId == clientChurchId && x.Status == (int)ChurchServiceStatus.Active);
                if (!myItemList.Any()) return new List<RegisteredClientChurchServiceReportObj>();

                
                myItemList.ForEachx(m => retList.Add(new RegisteredClientChurchServiceReportObj
                {
                    ClientChurchServiceId = m.ClientChurchServiceId,
                    ClientChurchId = m.ClientChurchId,
                    //ChurchServiceTypeId = m.ChurchServiceTypeId,
                    //Name = ServiceChurch.GetChurchServiceTypeNameById(m.ChurchServiceTypeId),
                    //DayOfWeekId = m.DayOfWeekId,
                    //ServiceDay = Enum.GetName(typeof(WeekDays), m.DayOfWeekId)
                }));

                return retList;

                //var clientChurchServices = retList.FindAll(x => x.ClientId == clientId).ToList();
                //var defaultChurchServices = retList.FindAll(x => x.ClientId == 1).ToList();

                //defaultChurchServices.AddRange(clientChurchServices);
                //return defaultChurchServices;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RegisteredClientChurchServiceReportObj>(); ;
            }
        }


        #region For Client Church Services Type Modification

        internal ClientChurchService GetClientChurchServiceTypeReportObj(long clientChurchId)
        {
            try
            {
                var myItems = _repository.GetAll(x => x.ClientChurchId == clientChurchId).ToList();
                if (myItems.IsNullOrEmpty() || !myItems.Any())
                {
                    return new ClientChurchService();
                }

                var item = myItems[0];
                var retItem = new ClientChurchService
                {
                    ClientChurchServiceId = item.ClientChurchServiceId,
                    ClientChurchId = item.ClientChurchId,
                    ServiceTypeDetail = item.ServiceTypeDetail,
                    AddedByUserId = item.AddedByUserId,
                    TimeStampAdded = item.TimeStampAdded
                };
                return retItem;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ClientChurchService();
            }
        }

        internal RespStatus UpdateClientChurchServiceType(ClientChurchService agentObj)
        {

            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };

            try
            {
                #region Model Validation

                if (agentObj.Equals(null))
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your processing";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(agentObj, out valResults))
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

                #endregion

                string msg;
                var thisClientChurchServiceType = GetClientChurchService(agentObj.ClientChurchServiceId, out msg);
                if (thisClientChurchServiceType == null || thisClientChurchServiceType.ClientChurchServiceId < 1)
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage =
                            string.IsNullOrEmpty(msg) ? "Invalid Church service type information" : msg;
                    return response;
                }


                #region Duplication

                if (IsDuplicate(thisClientChurchServiceType.ServiceTypeDetail, agentObj.ServiceTypeDetail, out msg, 1))
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage =
                            string.IsNullOrEmpty(msg) ? "Duplicate Error! Service name already exist." : msg;
                    return response;
                }

                #endregion


                thisClientChurchServiceType.ServiceTypeDetail = agentObj.ServiceTypeDetail;
                var processedClientChurchCollectionType = _repository.Update(thisClientChurchServiceType);
                _uoWork.SaveChanges();
                if (processedClientChurchCollectionType.ClientChurchServiceId < 1)
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

        private bool IsDuplicate(List<ChurchServiceDetailObj> existingObjs, List<ChurchServiceDetailObj> freshObjs, out string msg, int status = 0)
        {
            try
            {

                #region Validate Objs

                if (!existingObjs.Any() || !freshObjs.Any())
                {
                    msg = "";
                    return false;
                }
                #endregion

                #region Checking Duplicate

                #region Raw


                #endregion

                foreach (var checker in existingObjs.Select(existingObj => freshObjs.FindAll(x => x.Name == existingObj.Name).ToList()))
                {
                    switch (status)
                    {
                        case 0:
                            if (checker.Any())
                            {
                                msg = "Duplicate Error! Service name: (" + checker[0].Name + ") already exist";
                                return true;
                            }
                            break;
                        case 1:
                            if (checker.Any())
                            {
                                if (checker.Count <= 1) continue;
                                msg = "Duplicate Error! Service name: (" + checker[0].Name + ") already exist";
                                return true;
                            }
                            break;
                    }
                }

                #endregion

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




        #endregion

        internal List<ChurchServiceDetailObj> GetClientChurchService(long clientChurchId)
        {
            try
            {
                var myItem = _repository.GetAll(x => x.ClientChurchId == clientChurchId).ToList();
                if (myItem.IsNullOrEmpty() || !myItem.Any())
                {
                    return new List<ChurchServiceDetailObj>();
                }
                return myItem[0].ServiceTypeDetail;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ChurchServiceDetailObj>();
            }
        }


        internal ChurchServiceDetailObj GetClientChurchServiceDetail(long clientChurchId, string churchServiceTypeRefId)
        {
            try
            {
                if (clientChurchId < 1 || churchServiceTypeRefId.IsNullOrEmpty() || churchServiceTypeRefId.Length == 0)
                {
                    return new ChurchServiceDetailObj();
                }

                var clientChurchService = GetClientChurchService(clientChurchId);
                if (!clientChurchService.Any() || clientChurchService.Count == 0) { return new ChurchServiceDetailObj(); }

                var thisClientChurchServiceDetail = clientChurchService.Find(x => x.ChurchServiceTypeRefId == churchServiceTypeRefId);
                if (thisClientChurchServiceDetail == null ||
                    thisClientChurchServiceDetail.ChurchServiceTypeRefId.Length == 0)
                {
                    return new ChurchServiceDetailObj();
                }

                return thisClientChurchServiceDetail;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal List<RegisteredClientChurchServiceReportObj> GetAllRegisteredClientChurchServiceObjs(long churchId, long clientChurchId)
        {
            try
            {
                if (clientChurchId < 1)
                {
                    return new List<RegisteredClientChurchServiceReportObj>();
                }

                var myItemList = _repository.GetAll(x => x.ClientChurchId == clientChurchId && x.Status == (int)ChurchServiceStatus.Active);
                if (!myItemList.Any()) return new List<RegisteredClientChurchServiceReportObj>();

                var retList = new List<RegisteredClientChurchServiceReportObj>();
                myItemList.ForEachx(m => retList.Add(new RegisteredClientChurchServiceReportObj
                {
                    ClientChurchServiceId = m.ClientChurchServiceId,
                    ClientChurchId = m.ClientChurchId,
                    //ChurchServiceTypeId = m.ChurchServiceTypeId,
                    //Name = ServiceChurch.GetChurchServiceTypeNameById(m.ChurchServiceTypeId),
                    //DayOfWeekId = m.DayOfWeekId,
                    //ServiceDay = Enum.GetName(typeof(WeekDays), m.DayOfWeekId)
                }));

                return retList;

                //var clientChurchServices = retList.FindAll(x => x.ClientId == clientId).ToList();
                //var defaultChurchServices = retList.FindAll(x => x.ClientId == 1).ToList();

                //defaultChurchServices.AddRange(clientChurchServices);
                //return defaultChurchServices;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RegisteredClientChurchServiceReportObj>(); ;
            }
        }

        private ClientChurchService GetClientChurchService(long clientChurchServiceId, out string msg)
        {
            try
            {
                // var sql1 =
                // string.Format(
                //     "Select * FROM \"ICASDB\".\"ClientChurchService\" WHERE \"ClientChurchServiceId\" = {0};", clientChurchServiceId);

                //var check = _repository.RepositoryContext().Database.SqlQuery<ClientChurchService>(sql1).ToList();
                //if (check.IsNullOrEmpty())
                //{
                //    msg = "No Client Church Service record found!";
                //    return null;
                //}
                //if (check.Count != 1)
                //{
                //    msg = "Invalid Client Church Service Record!";
                //    return null;
                //}
                //msg = "";
                //return check[0];

                var myItem = _repository.GetById(clientChurchServiceId);
                if (myItem == null || myItem.ClientChurchServiceId < 1)
                {
                    msg = "No Church Service Type record found!";
                    return new ClientChurchService();
                }

                msg = "";
                return myItem;

            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }




        private bool IsDuplicate(long clientId, long churchServiceId, string name, out string msg, int status = 0)
        {
            try
            {
                // Check Duplicate Service name
                List<ChurchService> check;
                if (clientId > 0 && !string.IsNullOrEmpty(name))
                {
                    //var sql1 =
                    //    string.Format(
                    //    "Select * FROM \"ChurchAPPDB\".\"ChurchService\" WHERE " +
                    //    " \"ClientId\" = {0} " +
                    //    "AND lower(\"Name\") = lower('{1}')" +
                    //    "AND \"Status\" = 1", clientId, name);
                    var sql1 =
                        string.Format(
                        "Select * FROM \"ICASDB\".\"ChurchService\" WHERE " +
                        " \"ChurchServiceId\" = {0} " +
                        " AND \"ClientId\" = {1} " +
                        " AND lower(\"Name\") = lower('{2}')", churchServiceId, clientId, name);
                    check = _repository.RepositoryContext().Database.SqlQuery<ChurchService>(sql1).ToList();
                }
                else
                {
                    check = null;
                }

                msg = "";
                if (check.IsNullOrEmpty()) return false;

                switch (status)
                {
                    case 0:

                        if (check != null)
                        {
                            if (check.Count > 0)
                            {
                                msg = "Duplicate Error! Service name already exist";
                                return true;
                            }
                        }
                        break;

                    case 1:
                        if (check != null)
                        {
                            if (check.Count >= 1)
                            {
                                msg = "Duplicate Error! Service name already exist";
                                return true;
                            }
                        }
                        break;
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

        private bool IsDuplicate(ClientChurchServiceRegObj clientChurchServiceRegObj, out bool reset, out long resetId, out string msg, int status = 0)
        {
            try
            {
                var check = new List<ClientChurchService>();
                if (clientChurchServiceRegObj.ClientChurchId > 0 && clientChurchServiceRegObj.ChurchServiceTypeId > 0)
                {
                    var sql =
                        string.Format(
                            "Select * FROM \"ICASDB\".\"ClientChurchService\" WHERE " +
                            " \"ClientChurchId\" = {0} " +
                            " AND \"ChurchServiceTypeId\" = {1} ", clientChurchServiceRegObj.ClientChurchId,
                            clientChurchServiceRegObj.ChurchServiceTypeId);
                    check = _repository.RepositoryContext().Database.SqlQuery<ClientChurchService>(sql).ToList();
                }

                msg = "";
                if (check.IsNullOrEmpty())
                {
                    reset = false;
                    resetId = 0;
                    return false;
                }

                switch (status)
                {
                    case 0:
                        if (check.Count > 0)
                        {

                            // Check if the exist status is In_Active and reset it back, don't bother to add new
                            if (check[0].Status == (int)ChurchServiceStatus.Remove && check[0].ClientChurchServiceId > 0)
                            {
                                // Reset the status to Active
                                check[0].Status = (int)ChurchServiceStatus.Active;
                                //check[0].TimeStampAdded = DateScrutnizer.CurrentTimeStamp();
                                //check[0].ChurchServiceTypeId = clientChurchServiceRegObj.ChurchServiceTypeId;
                                //check[0].DayOfWeekId = clientChurchServiceRegObj.DayOfWeekId;
                                //check[0].AddedByUserId = clientChurchServiceRegObj.AddedByUserId;

                                var resetService = _repository.Update(check[0]);
                                _uoWork.SaveChanges();
                                if (resetService.ClientChurchServiceId > 1)
                                {
                                    msg = "";
                                    reset = true;
                                    resetId = resetService.ClientChurchServiceId;
                                    return false;
                                }

                                msg = "Process Failed! Please try again later";
                                reset = false;
                                resetId = 0;
                                return true;
                            }

                            msg = "Duplicate Error! Church Structure already exist";
                            reset = false;
                            resetId = 0;
                            return true;
                        }
                        break;

                    case 1:
                        if (check.Count >= 1)
                        {
                            // Check if the exist status is In_Active and reset it back, don't bother to add new
                            if (check[0].Status == (int)ChurchServiceStatus.Remove)
                            {
                                // Reset the status to Active
                                check[0].Status = (int)ChurchServiceStatus.Active;
                                check[0].TimeStampAdded = DateScrutnizer.CurrentTimeStamp();
                                //check[0].ChurchServiceTypeId = clientChurchServiceRegObj.ChurchServiceTypeId;
                                //check[0].DayOfWeekId = clientChurchServiceRegObj.DayOfWeekId;
                                //check[0].AddedByUserId = clientChurchServiceRegObj.AddedByUserId;

                                var resetService = _repository.Update(check[0]);
                                _uoWork.SaveChanges();
                                if (resetService.ClientChurchServiceId > 1)
                                {
                                    msg = "";
                                    reset = true;
                                    resetId = resetService.ClientChurchServiceId;
                                    return false;
                                }

                                msg = "Process Failed! Please try again later";
                                reset = false;
                                resetId = 0;
                                return true;
                            }

                            msg = "Duplicate Error! Church Structure already exist";
                            reset = false;
                            resetId = 0;
                            return true;
                        }
                        break;

                }


                msg = "";
                reset = false;
                resetId = 0;
                return false;

                //msg = "Unable to check duplicate! Please try again later";
                //return true;
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                reset = false;
                resetId = 0;
                return true;
            }
        }

        private ClientChurchService ExistWitMe(long clientId, int churchStructureTypeId)
        {
            try
            {
                var check = new List<ClientChurchService>();
                if (clientId > 0 && churchStructureTypeId > 0)
                {
                    var sql =
                        string.Format(
                            "Select * FROM \"ICASDB\".\"ClientChurchService\" WHERE " +
                            " \"ClientChurchId\" = {0} " +
                            " AND \"ChurchServiceTypeId\" = {1} ", clientId, churchStructureTypeId);
                    check = _repository.RepositoryContext().Database.SqlQuery<ClientChurchService>(sql).ToList();
                }

                if (check.IsNullOrEmpty())
                {
                    return null;
                }

                if (check.Count > 0)
                {
                    if (check[0].ClientChurchServiceId > 0)
                    {
                        return check[0];
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
    }
}
