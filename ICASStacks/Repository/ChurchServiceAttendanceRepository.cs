using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using ICASStacks.APIObjs;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.DataContract.ChurchAdministrative.ReflectionObjs;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using ICASStacks.Repository.Helpers;
using ICASStacks.StackService;
using Newtonsoft.Json;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ChurchServiceAttendanceRepository
    {

        private readonly IIcasRepository<ChurchServiceAttendance> _repository;
        private readonly IIcasRepository<ChurchServiceAttendanceRemittance> _churchServiceAttendanceRemittanceRepository;
        private readonly IIcasRepository<ChurchServiceAttendanceAttendee> _churchServiceAttendanceAttendeeRepository;
        private readonly IIcasRepository<ClientChurchCollection> _clientChurchCollectionRepository;
        private readonly IIcasRepository<ChurchServiceType> _churchServiceTypeRepository;
        private readonly IIcasRepository<ClientChurchService> _clientChurchServiceRepository;
        private readonly IcasUoWork _uoWork;

        public ChurchServiceAttendanceRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ChurchServiceAttendance>(_uoWork);
            _churchServiceAttendanceRemittanceRepository = new IcasRepository<ChurchServiceAttendanceRemittance>(_uoWork);
            _clientChurchServiceRepository = new IcasRepository<ClientChurchService>(_uoWork);
            _churchServiceAttendanceAttendeeRepository = new IcasRepository<ChurchServiceAttendanceAttendee>(_uoWork);
            _clientChurchCollectionRepository = new IcasRepository<ClientChurchCollection>(_uoWork);
            _churchServiceTypeRepository = new IcasRepository<ChurchServiceType>(_uoWork);
            //_churchServiceRepository = new IcasRepository<ChurchService>(_uoWork);
        }


        internal ChurchServiceAttendanceRegResponse AddChurchServiceAttendance(ChurchServiceAttendanceRegObj churchServiceAttendanceRegObj)
        {

            var response = new ChurchServiceAttendanceRegResponse
            {
                ChurchServiceAttendanceId = 0,
                ChurchServiceTypeRefId = churchServiceAttendanceRegObj.ChurchServiceTypeRefId,
                ClientId = churchServiceAttendanceRegObj.ClientChurchId,
                TotalAttendee = 0,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {
                if (churchServiceAttendanceRegObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Church registration Object is empty / invalid";
                    return response;
                }

                #region Model Validation
                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(churchServiceAttendanceRegObj, out valResults))
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
                if (churchServiceAttendanceRegObj.TakenByUserId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Admin User Information";
                    response.Status.Message.TechnicalMessage = "Invalid Admin User Information";
                    response.Status.IsSuccessful = false;
                    return response;
                }
                #endregion
                
                string msg;
                churchServiceAttendanceRegObj.DateServiceHeld = DateScrutnizer.ReverseToServerDate(churchServiceAttendanceRegObj.DateServiceHeld.Replace('-', '/'));

                //churchServiceAttendanceRegObj.DateServiceHeld =
                //    DateScrutnizer.ReverseDateRangeToServerDate(churchServiceAttendanceRegObj.DateServiceHeld);
                if (IsDuplicate(churchServiceAttendanceRegObj.ClientChurchId, churchServiceAttendanceRegObj.ChurchServiceTypeRefId,
                    churchServiceAttendanceRegObj.DateServiceHeld, out msg))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }

                
                using (var db = _uoWork.BeginTransaction())
                {
                    try
                    {
                        //Total Attendee
                        var totalAttendee = (churchServiceAttendanceRegObj.ChurchServiceAttendanceDetail.NumberOfMen +
                                             churchServiceAttendanceRegObj.ChurchServiceAttendanceDetail.NumberOfWomen +
                                             churchServiceAttendanceRegObj.ChurchServiceAttendanceDetail.NumberOfChildren);
                        
                        //Total Collection
                        var totalCollection =
                            churchServiceAttendanceRegObj.ChurchServiceAttendanceDetail
                                .ClientChurchServiceAttendanceCollections.Sum(x => x.Amount);

                        var thisChurchServiceAttendance = new ChurchServiceAttendance
                        {
                            ClientChurchId = churchServiceAttendanceRegObj.ClientChurchId,
                            ChurchServiceTypeRefId = churchServiceAttendanceRegObj.ChurchServiceTypeRefId,
                            ServiceTheme = churchServiceAttendanceRegObj.ServiceTheme,
                            BibleReadingText = churchServiceAttendanceRegObj.BibleReadingText,
                            Preacher = churchServiceAttendanceRegObj.Preacher,
                            DateServiceHeld = churchServiceAttendanceRegObj.DateServiceHeld,
                            TotalAttendee = totalAttendee,
                            TotalCollection = totalCollection,
                            _ServiceAttendanceDetail = JsonConvert.SerializeObject(churchServiceAttendanceRegObj.ChurchServiceAttendanceDetail),

                            TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                            //TakenByUserId = 1,
                            TakenByUserId = churchServiceAttendanceRegObj.TakenByUserId,
                        };
                        
                        var processedChurchServiceAttendance = _repository.Add(thisChurchServiceAttendance);
                        _uoWork.SaveChanges();
                        if (processedChurchServiceAttendance.ChurchServiceAttendanceId < 1)
                        {
                            db.Rollback();
                            response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                            response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                            response.Status.IsSuccessful = false;
                            return response;
                        }
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                        response.Status.Message.FriendlyMessage = "Church Service taken failed! Please try again later";
                        response.Status.Message.TechnicalMessage = "Error: " + ex.GetBaseException().Message;
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    db.Commit();
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

        }
        
        internal RespStatus UpdateChurchServiceAttendance(ChurchServiceAttendanceRegObj churchServiceAttendanceRegObj)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };

            try
            {
                if (churchServiceAttendanceRegObj.Equals(null))
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your processing";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(churchServiceAttendanceRegObj, out valResults))
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
                var thisChurchServiceAttendance = GetChurchServiceAttendance(churchServiceAttendanceRegObj.ChurchServiceAttendanceId, out msg);
                if (thisChurchServiceAttendance == null || thisChurchServiceAttendance.ServiceTheme.Length < 1)
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage =
                            string.IsNullOrEmpty(msg) ? "Invalid Church service attendance information" : msg;
                    return response;
                }

                #region Unsed

                //if (IsDuplicate(churchServiceAttendanceRegObj.ClientId, churchServiceAttendanceRegObj.ChurchServiceId,
                //    churchServiceAttendanceRegObj.ServiceTheme, out msg, 1))
                //{
                //    response.Message.FriendlyMessage = msg;
                //    response.Message.TechnicalMessage = msg;
                //    response.IsSuccessful = false;
                //    return response;
                //}

                //churchServiceAttendanceRegObj.DateServiceHeld = DateScrutnizer.ReverseDateRangeToServerDate(churchServiceAttendanceRegObj.DateServiceHeld);
                //if (IsDuplicatex(churchServiceAttendanceRegObj.ChurchServiceTypeId, churchServiceAttendanceRegObj.Preacher,
                //    churchServiceAttendanceRegObj.ServiceTheme, churchServiceAttendanceRegObj.DateServiceHeld, out msg))
                //{
                //    response.Message.FriendlyMessage = msg;
                //    response.Message.TechnicalMessage = msg;
                //    response.IsSuccessful = false;
                //    return response;
                //}

                //var totalAttendee = (churchServiceAttendanceRegObj.NumberOfMen +
                //                    churchServiceAttendanceRegObj.NumberOfWomen +
                //                    churchServiceAttendanceRegObj.NumberOfChildren +
                //                    churchServiceAttendanceRegObj.FirstTimer + churchServiceAttendanceRegObj.NewConvert);

                //var totalCollection = (churchServiceAttendanceRegObj.Offerring +
                //                             churchServiceAttendanceRegObj.Tithe +
                //                             churchServiceAttendanceRegObj.ThanksGiving +
                //                             churchServiceAttendanceRegObj.SpecialThanksGiving +
                //                             churchServiceAttendanceRegObj.Donation +
                //                             churchServiceAttendanceRegObj.BuildingProjectFund +
                //                             churchServiceAttendanceRegObj.FirstFruit +
                //                             churchServiceAttendanceRegObj.WelfareCharity +
                //                             churchServiceAttendanceRegObj.Others);

                #endregion
                
                churchServiceAttendanceRegObj.DateServiceHeld = DateScrutnizer.ReverseToServerDate(churchServiceAttendanceRegObj.DateServiceHeld.Replace("-", "/"));
                if (IsDuplicate(churchServiceAttendanceRegObj.ClientChurchId, churchServiceAttendanceRegObj.ChurchServiceTypeRefId,
                    churchServiceAttendanceRegObj.DateServiceHeld, out msg, 1))
                {
                    response.Message.FriendlyMessage = msg;
                    response.Message.TechnicalMessage = msg;
                    response.IsSuccessful = false;
                    return response;
                }
                
                using (var db = _uoWork.BeginTransaction())
                {
                    try
                    {

                        //Total Attendee
                        var thisTotalAttendee = (churchServiceAttendanceRegObj.ChurchServiceAttendanceDetail.NumberOfMen +
                                             churchServiceAttendanceRegObj.ChurchServiceAttendanceDetail.NumberOfWomen +
                                             churchServiceAttendanceRegObj.ChurchServiceAttendanceDetail.NumberOfChildren);
                        
                        //Total Collection
                        var thisTotalCollection =
                            churchServiceAttendanceRegObj.ChurchServiceAttendanceDetail
                                .ClientChurchServiceAttendanceCollections.Sum(x => x.Amount);


                        thisChurchServiceAttendance.ServiceTheme = churchServiceAttendanceRegObj.ServiceTheme;
                        thisChurchServiceAttendance.ChurchServiceTypeRefId = churchServiceAttendanceRegObj.ChurchServiceTypeRefId;
                        thisChurchServiceAttendance.BibleReadingText = churchServiceAttendanceRegObj.BibleReadingText;
                        thisChurchServiceAttendance.Preacher = churchServiceAttendanceRegObj.Preacher;
                        thisChurchServiceAttendance.DateServiceHeld = churchServiceAttendanceRegObj.DateServiceHeld;
                        thisChurchServiceAttendance.TotalAttendee = thisTotalAttendee;
                        thisChurchServiceAttendance.TotalCollection = thisTotalCollection;

                        thisChurchServiceAttendance.ServiceAttendanceDetail =
                            churchServiceAttendanceRegObj.ChurchServiceAttendanceDetail;

                        var processedChurchServiceAttendance = _repository.Update(thisChurchServiceAttendance);
                        _uoWork.SaveChanges();
                        if (processedChurchServiceAttendance.ChurchServiceAttendanceId < 1)
                        {
                            db.Rollback();
                            response.Message.FriendlyMessage =
                                response.Message.TechnicalMessage = "Process Failed! Please try again later";
                            return response;
                        }

                        #region Unsed
                        // Submit Church Service Attendance
                        //var totalAttendee = (churchServiceAttendanceRegObj.ChurchServiceAttendanceAttendee.NumberOfMen +
                        //                     churchServiceAttendanceRegObj.ChurchServiceAttendanceAttendee.NumberOfWomen +
                        //                     churchServiceAttendanceRegObj.ChurchServiceAttendanceAttendee.NumberOfChildren);

                        //var totalCollection = (churchServiceAttendanceRegObj.ClientChurchCollection.Offerring +
                        //                     churchServiceAttendanceRegObj.ClientChurchCollection.Tithe +
                        //                     churchServiceAttendanceRegObj.ClientChurchCollection.ThanksGiving +
                        //                     churchServiceAttendanceRegObj.ClientChurchCollection.SpecialThanksGiving +
                        //                     churchServiceAttendanceRegObj.ClientChurchCollection.Donation +
                        //                     churchServiceAttendanceRegObj.ClientChurchCollection.BuildingProjectFund +
                        //                     churchServiceAttendanceRegObj.ClientChurchCollection.FirstFruit +
                        //                     churchServiceAttendanceRegObj.ClientChurchCollection.WelfareCharity +
                        //                     churchServiceAttendanceRegObj.ClientChurchCollection.Others);



                        //var thisChurchServiceAttendanceAttendee =
                        //    _churchServiceAttendanceAttendeeRepository.GetById(
                        //        churchServiceAttendanceRegObj.ChurchServiceAttendanceAttendee
                        //            .ChurchServiceAttendanceAttendeeId);

                        //var thisChurchServiceAttendanceCollection =
                        //    _clientChurchCollectionRepository.GetById(
                        //        churchServiceAttendanceRegObj.ClientChurchCollection
                        //            .ClientChurchCollectionId);

                        //var thisChurchServiceAttendanceAttendee =
                        //    _churchServiceAttendanceAttendeeRepository.GetById(
                        //        churchServiceAttendanceRegObj.ChurchServiceAttendanceAttendee
                        //            .ChurchServiceAttendanceId);

                        //var thisChurchServiceAttendanceCollection =
                        //    _clientChurchCollectionRepository.GetById(
                        //        churchServiceAttendanceRegObj.ClientChurchCollection
                        //            .ChurchServiceAttendanceId);

                        //thisChurchServiceAttendanceAttendee.NumberOfMen = churchServiceAttendanceRegObj.ChurchServiceAttendanceAttendee.NumberOfMen;
                        //thisChurchServiceAttendanceAttendee.NumberOfWomen = churchServiceAttendanceRegObj.ChurchServiceAttendanceAttendee.NumberOfWomen;
                        //thisChurchServiceAttendanceAttendee.NumberOfChildren = churchServiceAttendanceRegObj.ChurchServiceAttendanceAttendee.NumberOfChildren;
                        //thisChurchServiceAttendanceAttendee.FirstTimer = churchServiceAttendanceRegObj.ChurchServiceAttendanceAttendee.FirstTimer;
                        //thisChurchServiceAttendanceAttendee.NewConvert = churchServiceAttendanceRegObj.ChurchServiceAttendanceAttendee.NewConvert;
                        //thisChurchServiceAttendanceAttendee.TotalAttendee = totalAttendee;

                        //var processedChurchServiceAttendanceAttendee = _churchServiceAttendanceAttendeeRepository.Update(thisChurchServiceAttendanceAttendee);
                        //_uoWork.SaveChanges();
                        //if (processedChurchServiceAttendanceAttendee.ChurchServiceAttendanceId < 1)
                        //{
                        //    db.Rollback();
                        //    response.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        //    response.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        //    response.IsSuccessful = false;
                        //    return response;
                        //}
                        //if (processedChurchServiceAttendanceAttendee.ChurchServiceAttendanceAttendeeId < 1)
                        //{
                        //    db.Rollback();
                        //    response.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        //    response.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        //    response.IsSuccessful = false;
                        //    return response;
                        //}


                        //thisChurchServiceAttendanceCollection.Offerring = churchServiceAttendanceRegObj.ClientChurchCollection.Offerring;
                        //thisChurchServiceAttendanceCollection.Tithe = churchServiceAttendanceRegObj.ClientChurchCollection.Tithe;
                        //thisChurchServiceAttendanceCollection.ThanksGiving = churchServiceAttendanceRegObj.ClientChurchCollection.ThanksGiving;
                        //thisChurchServiceAttendanceCollection.SpecialThanksGiving = churchServiceAttendanceRegObj.ClientChurchCollection.SpecialThanksGiving;
                        //thisChurchServiceAttendanceCollection.Donation = churchServiceAttendanceRegObj.ClientChurchCollection.Donation;
                        //thisChurchServiceAttendanceCollection.BuildingProjectFund = churchServiceAttendanceRegObj.ClientChurchCollection.BuildingProjectFund;
                        //thisChurchServiceAttendanceCollection.FirstFruit = churchServiceAttendanceRegObj.ClientChurchCollection.FirstFruit;
                        //thisChurchServiceAttendanceCollection.WelfareCharity = churchServiceAttendanceRegObj.ClientChurchCollection.WelfareCharity;
                        //thisChurchServiceAttendanceCollection.Others = churchServiceAttendanceRegObj.ClientChurchCollection.Others;
                        //thisChurchServiceAttendanceCollection.TotalCollection = totalCollection;

                        //var processedChurchServiceAttendanceCollection = _clientChurchCollectionRepository.Update(thisChurchServiceAttendanceCollection);
                        //_uoWork.SaveChanges();
                        //if (processedChurchServiceAttendanceCollection.ChurchServiceAttendanceId < 1)
                        //{
                        //    db.Rollback();
                        //    response.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        //    response.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        //    response.IsSuccessful = false;
                        //    return response;
                        //}
                        //if (processedChurchServiceAttendanceCollection.ClientChurchCollectionId < 1)
                        //{
                        //    db.Rollback();
                        //    response.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        //    response.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        //    response.IsSuccessful = false;
                        //    return response;
                        //}
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                        response.Message.FriendlyMessage = "Church Service Attendance Modification Failed! Please try again later";
                        response.Message.TechnicalMessage = "Error: " + ex.GetBaseException().Message;
                        response.IsSuccessful = false;
                        return response;
                    }


                    db.Commit();
                    response.IsSuccessful = true;
                    return response;
                }

                #region Unsed

                //thisChurchServiceAttendance.ServiceTheme = churchServiceAttendanceRegObj.ServiceTheme;
                //thisChurchServiceAttendance.ChurchServiceTypeId = churchServiceAttendanceRegObj.ChurchServiceTypeId;
                //thisChurchServiceAttendance.BibleReadingText = churchServiceAttendanceRegObj.BibleReadingText;
                //thisChurchServiceAttendance.Preacher = churchServiceAttendanceRegObj.Preacher;
                //thisChurchServiceAttendance.DateServiceHeld = churchServiceAttendanceRegObj.DateServiceHeld;

                //thisChurchServiceAttendance.NumberOfMen = churchServiceAttendanceRegObj.NumberOfMen;
                //thisChurchServiceAttendance.NumberOfWomen = churchServiceAttendanceRegObj.NumberOfWomen;
                //thisChurchServiceAttendance.NumberOfChildren = churchServiceAttendanceRegObj.NumberOfChildren;
                //thisChurchServiceAttendance.FirstTimer = churchServiceAttendanceRegObj.FirstTimer;
                //thisChurchServiceAttendance.NewConvert = churchServiceAttendanceRegObj.NewConvert;

                //thisChurchServiceAttendance.TotalAttendee = totalAttendee;
                #endregion
                
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

        internal List<RegisteredChurchServiceAttendanceReportObj> GetAllRegisteredChurchServiceAttendanceObjs(long clientChurchId)
        {
            try
            {
                if (clientChurchId < 1)
                {
                    return new List<RegisteredChurchServiceAttendanceReportObj>();
                }

                var clientChurchServiceAttendances = GetChurchServiceAttendanceByClientId(clientChurchId).ToList();
                if (!clientChurchServiceAttendances.Any() || clientChurchServiceAttendances.Count < 1)
                {
                    return new List<RegisteredChurchServiceAttendanceReportObj>();
                }

                #region Getting Client Church Collection Types

                var clientChurchCollectionTypes = ServiceChurch.GetClientChurchCollectionTypesByClientChurchId(clientChurchId);
                var serviceAttendanceCollections = new List<ClientChurchServiceAttendanceCollectionObj>();
                var serviceDetail = new ClientChurchServiceAttendanceDetailObj();
                #endregion
                

                var retList = new List<RegisteredChurchServiceAttendanceReportObj>();
                clientChurchServiceAttendances.ForEachx(m =>
                {
                    string msg;
                    
                    #region Latest Getting Service Name

                    // Get it from Client Church Service with m.ClientChurchId & m.ChurchServiceTypeRefId
                    var clientChurchServiceName =
                        ServiceChurch.GetClientChurchServiceDetail(m.ClientChurchId, m.ChurchServiceTypeRefId).Name;
                    

                    #endregion

                    #region Unsed
                    //var churchServiceType = _churchServiceTypeRepository.GetById(m.ChurchServiceTypeRefId);

                    //var churchServiceAttendanceAttendee =
                    //    new ChurchServiceAttendanceAttendeeRepository()
                    //        .GetChurchServiceAttendanceAttendeeByChurchServiceAttendanceId(m.ChurchServiceAttendanceId,
                    //            out msg);

                    //var churchServiceAttendanceClientCollection =
                    //    new ChurchServiceAttendanceClientChurchCollectionRepository()
                    //        .GetChurchServiceAttendanceClientCollectionByChurchServiceAttendanceId(m.ChurchServiceAttendanceId,
                    //            out msg);
                    #endregion
                    

                    var dateHeld = "";
                    var heldDate = "";
                    if (!m.DateServiceHeld.IsNullOrEmpty())
                    {
                        dateHeld = DateScrutnizer.ReverseToGeneralDate(m.DateServiceHeld);
                        var dt = DateTime.ParseExact(m.DateServiceHeld, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                        heldDate = dt.ToString("dd MMMM, yyyy");
                    }



                    if (clientChurchCollectionTypes.Any())
                    {
                        //serviceAttendanceCollections.Where(c => c.CollectionRefId == x.CollectionRefId).ToList().ForEachx(s => s.CollectionTypeName = x.CollectionTypeName);
                        //
                        serviceDetail = m.ServiceAttendanceDetail;
                        serviceAttendanceCollections = m.ServiceAttendanceDetail.ClientChurchServiceAttendanceCollections;
                        clientChurchCollectionTypes.ForEachx(
                            x =>
                                serviceDetail.ClientChurchServiceAttendanceCollections.Where(c => c.CollectionRefId == x.CollectionRefId)
                                    .ToList()
                                    .ForEachx(s => s.CollectionTypeName = x.CollectionTypeName));
                        m.ServiceAttendanceDetail.ClientChurchServiceAttendanceCollections = serviceAttendanceCollections;
                        //clientChurchCollectionTypes.ForEachx(
                        //    x =>
                        //        serviceAttendanceCollections.Where(c => c.CollectionRefId == x.CollectionRefId)
                        //            .ToList()
                        //            .ForEachx(s => s.CollectionTypeName = x.CollectionTypeName));
                        //m.ServiceAttendanceDetail.ClientChurchServiceAttendanceCollections = serviceAttendanceCollections;
                    }

                    
                    
                    retList.Add(new RegisteredChurchServiceAttendanceReportObj
                    {
                        ClientChurchId = m.ClientChurchId,
                        ChurchServiceAttendanceId = m.ChurchServiceAttendanceId,
                        ChurchServiceTypeRefId = m.ChurchServiceTypeRefId,
                        ServiceName = clientChurchServiceName,
                        ServiceTheme = m.ServiceTheme,
                        BibleReadingText = m.BibleReadingText,
                        Preacher = m.Preacher,


                        TotalAttendee = m.TotalAttendee,
                        TotalCollection = m.TotalCollection,
                        ServiceAttendanceDetail = serviceDetail,
                        //ServiceAttendanceDetail = m.ServiceAttendanceDetail,
                        DateServiceHeld = dateHeld,
                        ServiceHeldDate = heldDate,
                        DateAttendanceTaken = m.TimeStampTaken,
                    });
                });
                

                return retList;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RegisteredChurchServiceAttendanceReportObj>(); ;
            }
        }

        private ChurchServiceAttendance GetChurchServiceAttendance(long churchServiceAttendanceId, out string msg)
        {
            try
            {
                //var sql1 =
                // string.Format(
                //     "Select * FROM \"ICASDB\".\"ChurchServiceAttendance\"  WHERE \"ChurchServiceAttendanceId\" = {0};", churchServiceAttendanceId);

                //var check = _repository.RepositoryContext().Database.SqlQuery<ChurchServiceAttendance>(sql1).ToList();

                var check = _repository.GetById(churchServiceAttendanceId);
                if (check == null)
                {
                    msg = "No Church Service Attendance record found!";
                    return null;
                }
                if (check.ChurchServiceAttendanceId < 1)
                {
                    msg = "Invalid Church Service Attendance Record!";
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

        internal List<ChurchServiceAttendance> GetChurchServiceAttendanceByClientId(long clientChurchId)
        {
            try
            {
                var items = _repository.GetAll(x => x.ClientChurchId == clientChurchId).ToList();
                if (!items.Any())
                {
                    return new List<ChurchServiceAttendance>();
                }
                
                return items.ToList();
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ChurchServiceAttendance>();
            }
        }

        internal List<ChurchServiceAttendance> GetMonthlyChurchServiceAttendance(long clientChurchId, int year, int month, out string msg)
        {
            try
            {
                if (clientChurchId > 0 && year > 0 && month > 0)
                {
                    //var sql1 =
                    //string.Format(
                    //"Select * FROM \"ICASDB\".\"ChurchServiceAttendance\" WHERE " +
                    //" \"ClientId\" = {0} AND \"Year\" = {1} " +
                    //"AND \"Month\" = {2} ", clientId, year, month);

                    //var myItems = _repository.RepositoryContext().Database.SqlQuery<ChurchServiceAttendanceR>(sql1).ToList<ChurchServiceAttendance>();

                    var myItems = _repository.GetAll(x => x.ClientChurchId == clientChurchId).ToList();
                    if (!myItems.Any())
                    {
                        msg = "No church service attendance record found for the selected year and month";
                        return null;
                    }

                    msg = "";
                    return myItems;
                }

                msg = "Unable to check Church Service Attendance! Please try again later";
                return null;

            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }



        internal List<ChurchServiceAttendance> GetMonthlyChurchServiceAttendanceByDateRange(long clientChurchId, string start, string end, out string msg)
        {
            try
            {
                if (!start.IsNullOrEmpty() && !end.IsNullOrEmpty())
                {
                    var myItems = _repository.GetAll().ToList();
                    if (!myItems.Any() || myItems.Count == 0)
                    {
                        msg = "No church service attendance record found for the selected date range";
                        return null;
                    }

                    var retList =
                        myItems.FindAll(
                            x =>
                                DateTime.Parse(x.DateServiceHeld) >= DateTime.Parse(start) &&
                                DateTime.Parse(x.DateServiceHeld) <= DateTime.Parse(end));
                    if (!retList.Any() || retList.Count == 0)
                    {
                        msg = "No church service attendance record found for the selected date range";
                        return null;
                    }

                    msg = "";
                    return retList;

                    //var sql1 =
                    //string.Format(
                    //"Select * FROM \"ICASDB\".\"ChurchServiceAttendance\" WHERE " +
                    //" \"ClientId\" = {0} AND \"Year\" = {1} " +
                    //"AND \"Month\" = {2} ", clientId, year, month);
                   
                }

                msg = "Unable to check Church Service Attendance! Please try again later";
                return null;

            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        private bool IsDuplicate(long clientChurchId, string churchServiceTypeRefId, string dateServiceHeld, out string msg, int status = 0)
        {
            try
            {

                #region Oldies

                // Check Duplicate Service Attendance
                //List<ChurchServiceAttendance> check;
                //if (clientId > 0 || churchServiceTypeId.Length > 0 || !string.IsNullOrEmpty(dateServiceHeld))
                //{
                //    var sql1 =
                //        string.Format(
                //        "Select * FROM \"ChurchAPPDB\".\"ChurchServiceAttendance\" WHERE " +
                //        " \"ClientId\" = {0} " +
                //        "AND \"ChurchServiceTypeId\" = {1}" +
                //        "AND \"DateServiceHeld\" = '{2}'", clientId, churchServiceTypeId, dateServiceHeld);
                //    check = _repository.RepositoryContext().Database.SqlQuery<ChurchServiceAttendance>(sql1).ToList();
                //}
                //else
                //{
                //    check = null;
                //}

                //msg = "";
                //if (check.IsNullOrEmpty()) return false;

                //switch (status)
                //{
                //    case 0:

                //        if (check != null)
                //        {
                //            if (check.Count > 0)
                //            {
                //                msg = "Duplicate Error! Service attendance already taken";
                //                return true;
                //            }
                //        }
                //        break;

                //    case 1:
                //        if (check != null)
                //        {
                //            if (check.Count > 1)
                //            {
                //                msg = "Duplicate Error! Service attendance already taken";
                //                return true;
                //            }
                //        }
                //        break;
                //}

                //msg = "";
                //return false;

                #endregion

                #region Latest

                List<ChurchServiceAttendance> check;
                if (clientChurchId > 0 || churchServiceTypeRefId.Length > 0 || !string.IsNullOrEmpty(dateServiceHeld))
                {
                    check = GetChurchServiceAttendanceByClientId(clientChurchId);
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
                                msg = "Duplicate Error! Service attendance already taken";
                                return true;
                            }
                        }
                        break;

                    case 1:
                        if (check != null)
                        {
                            if (check.Count > 1)
                            {
                                msg = "Duplicate Error! Service attendance already taken";
                                return true;
                            }
                        }
                        break;
                }

                msg = "";
                return false;

                #endregion

            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return true;
            }
        }





        private bool IsDuplicatex(long churchServiceId, string preacher, string serviceTheme, string dateServiceHeld, out string msg, int status = 0)
        {
            try
            {
                // Check Duplicate Service Attendance
                List<ChurchServiceAttendance> check;
                if (churchServiceId > 0 && !string.IsNullOrEmpty(preacher) && !string.IsNullOrEmpty(serviceTheme) && dateServiceHeld.Length == 10)
                {
                    var sql1 =
                        string.Format(
                        "Select * FROM \"ChurchAPPDB\".\"ChurchServiceAttendance\" WHERE " +
                        " \"ChurchServiceId\" = {0} " +
                        "AND lower(\"Preacher\") = lower('{1}')" +
                        "AND lower(\"ServiceTheme\") = lower('{2}')" +
                        "AND \"DateServiceHeld\" = '{3}'", churchServiceId, preacher, serviceTheme, dateServiceHeld);
                    check = _repository.RepositoryContext().Database.SqlQuery<ChurchServiceAttendance>(sql1).ToList();
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

                                msg = "Duplicate Error! Service attendance already taken";
                                return true;
                                //var existingTakenTime = DateScrutnizer.GetDateFromTimeStamp(check[0].TimeStampTaken,
                                //    false);
                                //if (DateTime.Parse(dateAttendaneTaken) == DateTime.Parse(existingTakenTime))
                                //{
                                //    msg = "Duplicate Error! Service attendance already taken";
                                //    return true;
                                //}
                            }
                        }
                        break;

                    case 1:
                        if (check != null)
                        {
                            if (check.Count >= 1)
                            {
                                msg = "Duplicate Error! Service attendance already taken";
                                return true;
                                //var existingTakenTime = DateScrutnizer.GetDateFromTimeStamp(check[0].TimeStampTaken,
                                //    false);
                                //if (DateTime.Parse(dateAttendaneTaken) == DateTime.Parse(existingTakenTime))
                                //{
                                //    msg = "Duplicate Error! Service attendance already taken";
                                //    return true;
                                //}
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
    }
}
