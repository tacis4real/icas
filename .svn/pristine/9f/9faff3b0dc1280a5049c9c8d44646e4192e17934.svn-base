using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ICASStacks.APIObjs;
using ICASStacks.DataContract;
using ICASStacks.DataContract.Enum;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using ICASStacks.Repository.Helpers;
using ICASStacks.StackService;
using Newtonsoft.Json;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ChurchStructureParishHeadQuarterRepository
    {

        //private readonly IIcasRepository<ChurchStructureParishHeadQuarter> _repository;
        public readonly IIcasRepository<ChurchStructureParishHeadQuarter> _repository;
        private readonly IIcasRepository<ChurchStructureType> _churchStructureTypeRepository;
        private readonly IcasUoWork _uoWork;

        public ChurchStructureParishHeadQuarterRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ChurchStructureParishHeadQuarter>(_uoWork);
            _churchStructureTypeRepository = new IcasRepository<ChurchStructureType>(_uoWork);
        }


        internal ChurchStructureParishHqtrRegResponse AddChurchStructureParishHeadQuarter(ChurchStructureParishHeadQuarterRegObj churchStructureParishHeadQuarterRegObj)
        {
            var response = new ChurchStructureParishHqtrRegResponse
            {
                ChurchStructureParishHeadQuarterId = 0,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {

                #region Model Validation
                if (churchStructureParishHeadQuarterRegObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Church Structure Hierachy Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(churchStructureParishHeadQuarterRegObj, out valResults))
                {
                    var errorDetail = new StringBuilder();
                    if (!valResults.IsNullOrEmpty())
                    {
                        errorDetail.AppendLine("Following error occurred: ");
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
                #endregion


                #region Check If HeadQuarter Parish Exist & Update Then Return

                var churchId = churchStructureParishHeadQuarterRegObj.ChurchId;
                var stateId = churchStructureParishHeadQuarterRegObj.StateOfLocationId;
                var churchStructureTypeId = churchStructureParishHeadQuarterRegObj.ChurchStructureTypeId;

                var existingParishHeadQuarter = IsChurchStructureParishHeadQuarterExist(churchId, stateId, churchStructureTypeId);
                if (existingParishHeadQuarter != null && existingParishHeadQuarter.ChurchStructureParishHeadQuarterId > 0)
                {

                    //existingParishHeadQuarter.Parish.Add(churchStructureParishHeadQuarterRegObj.Parish);

                    //existingParishHeadQuarter.Parish.Add(churchStructureParishHeadQuarterRegObj.Parish);
                    //existingParishHeadQuarter.Parish.Add(new StructureChurchHeadQuarterParish
                    //{
                    //    ChurchStructureTypeId = 
                    //});

                    var existingParish = existingParishHeadQuarter.Parish;
                    existingParish.AddRange(churchStructureParishHeadQuarterRegObj.Parish);

                    existingParishHeadQuarter._Parish = JsonConvert.SerializeObject(existingParish);
                    var processedChurchStructureParishHeadQuarter = _repository.Update(existingParishHeadQuarter);
                    _uoWork.SaveChanges();
                    if (processedChurchStructureParishHeadQuarter.ChurchStructureParishHeadQuarterId < 1)
                    {
                        response.Status.Message.FriendlyMessage = response.Status.Message.TechnicalMessage = "Process Failed! Please try again later";
                        return response;
                    }

                    response.ChurchStructureParishHeadQuarterId =
                        processedChurchStructureParishHeadQuarter.ChurchStructureParishHeadQuarterId;
                    response.Status.IsSuccessful = true;
                    return response;
                }

                #endregion


                #region Add FRESH Church Structure Parish HeadQuarter

                var thisChurchStructureParishHeadQuarter = new ChurchStructureParishHeadQuarter
                {
                    ChurchId = churchStructureParishHeadQuarterRegObj.ChurchId,
                    StateOfLocationId = churchStructureParishHeadQuarterRegObj.StateOfLocationId,
                    ChurchStructureTypeId = churchStructureParishHeadQuarterRegObj.ChurchStructureTypeId,
                    _Parish = JsonConvert.SerializeObject(churchStructureParishHeadQuarterRegObj.Parish),
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = churchStructureParishHeadQuarterRegObj.RegisteredByUserId,
                };

                var processedNewChurchStructureParishHeadQuarter = _repository.Add(thisChurchStructureParishHeadQuarter);
                _uoWork.SaveChanges();
                if (processedNewChurchStructureParishHeadQuarter.ChurchStructureParishHeadQuarterId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                    response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                    response.Status.IsSuccessful = false;
                    return response;
                }

                response.ChurchStructureParishHeadQuarterId =
                    processedNewChurchStructureParishHeadQuarter.ChurchStructureParishHeadQuarterId;
                response.Status.IsSuccessful = true;
                return response;
                #endregion


                #region Old Church Structure Storing To Database

                //using (var db = _uoWork.BeginTransaction())
                //{

                //    try
                //    {
                //        var myChurchStructureTypes = churchStructureRegObj.MyChurchStructureTypeIds;
                //        for (var i = 0; i < myChurchStructureTypes.Length; i++)
                //        {

                //            string msg;
                //            bool reset;
                //            long resetId;
                //            if (IsDuplicate(churchStructureRegObj.ChurchId, myChurchStructureTypes[i], out reset, out resetId, out msg))
                //            {
                //                continue;
                //            }

                //            // Check if the exist status is In_Active and reset it back, don't bother to add new
                //            if (reset)
                //            {
                //                continue;
                //            }

                //            var thisChurchStructure = new ChurchStructure
                //            {
                //                ChurchId = churchStructureRegObj.ChurchId,

                //                ChurchStructureTypeDetail = new List<ChurchStructureTypeDetailObj>(),

                //                //ChurchStructureTypeId = myChurchStructureTypes[i],
                //                //HierachyLevel = churchStructureRegObj.HierachyLevel,
                //                LastModificationTimeStamp = DateScrutnizer.CurrentTimeStamp(),
                //                LastModificationByUserId = churchStructureRegObj.RegisteredByUserId,
                //                RegisteredByUserId = churchStructureRegObj.RegisteredByUserId,
                //                Status = ChurchStructureStatus.Active,
                //            };

                //            var processedChurchStructure = _repository.Add(thisChurchStructure);
                //            _uoWork.SaveChanges();
                //            if (processedChurchStructure.ChurchStructureId < 1)
                //            {
                //                db.Rollback();
                //                response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                //                response.Status.Message.TechnicalMessage = "Process Failed! Unable to register voter";
                //                response.Status.IsSuccessful = false;
                //                return response;
                //            }

                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        db.Rollback();
                //        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                //        response.Status.Message.FriendlyMessage = "Church Structure Type Registration Failed! Please try again later";
                //        response.Status.Message.TechnicalMessage = "Error: " + ex.GetBaseException().Message;
                //        response.Status.IsSuccessful = false;
                //        return response;
                //    }

                //    db.Commit();
                //    response.Status.IsSuccessful = true;
                //    return response;

                //}

                #endregion


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

        internal List<RegisteredStructureChurchParishReportObj> GetChurchStructureParishHeadQuartersByChurchStateId(long churchId, int stateId, int churchStructureTypeId)
        {
            try
            {

                if (churchId < 1 || stateId < 1)
                {
                    return new List<RegisteredStructureChurchParishReportObj>();
                }
                
                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) return new List<RegisteredStructureChurchParishReportObj>();
                
                var check2 = myItemList.FindAll(x => x.ChurchId == churchId && x.StateOfLocationId == stateId);

                var retList = new List<RegisteredStructureChurchParishReportObj>();
                check2.ForEachx(m =>
                {
                    string msg;
                    var structureName = _churchStructureTypeRepository.GetById(m.ChurchStructureTypeId).Name;

                    retList.Add(new RegisteredStructureChurchParishReportObj
                    {
                        ChurchStructureParishHeadQuarterId = m.ChurchStructureParishHeadQuarterId,
                        ChurchStructureTypeId = m.ChurchStructureTypeId,
                        ChurchStructureTypeName = structureName,
                        ChurchId = m.ChurchId,
                        StateOfLocationId = m.StateOfLocationId,
                        StateOfLocationName = "",
                        Parishes = m.Parish
                    });
                });
                
                return retList.ToList();
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RegisteredStructureChurchParishReportObj>();
            }
        }


        internal StructureChurchHeadQuarterParish GetParishByStructureChurchHeadQuarterParishId(long churchId, int stateId, string structureChurchHeadQuarterParishId)
        {
            try
            {

                if (structureChurchHeadQuarterParishId.IsNullOrEmpty() || structureChurchHeadQuarterParishId.Length < 1)
                {
                    return new StructureChurchHeadQuarterParish();
                }

                if (churchId < 1 || stateId < 1)
                {
                    return new StructureChurchHeadQuarterParish();
                }

                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) return new StructureChurchHeadQuarterParish();

                //var checks = thisMonthChurchServiceAttendances.Select(
                //                            thisMonthChurchServiceAttendance =>
                //                                thisMonthChurchServiceAttendance.ServiceAttendanceDetail
                //                                    .ChurchServiceAttendanceCollections)
                //                            .Select(
                //                                collectionTypes =>
                //                                    collectionTypes.Find(x => x.CollectionTypeId == thisCollectionTypeId)).ToList();


                var check2 = myItemList.FindAll(x => x.ChurchId == churchId && x.StateOfLocationId == stateId);
                if(!check2.Any()){ return new StructureChurchHeadQuarterParish(); }

                // Try select only the Parish Column
                var parishes = check2.Select(x => x.Parish).ToList();

                //var parishes =
                //    check2.Select(x => x.Parish)
                //        .Select(
                //            p => p.Find(s => s.StructureChurchHeadQuarterParishId == structureChurchHeadQuarterParishId)).ToList();


                // Now Check from the above list with structureChurchHeadQuarterParishId
                var parishs =
                    parishes.Select(
                        p => p.Find(s => s.StructureChurchHeadQuarterParishId == structureChurchHeadQuarterParishId)).ToList();

                var retItem = new StructureChurchHeadQuarterParish();
                foreach (var parish in parishs)
                {
                    if (parish != null && parish.StructureChurchHeadQuarterParishId.Length > 0)
                    {
                        retItem = parish;
                        break;
                    }
                }

                //var retItem = parish[0];
                return retItem;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new StructureChurchHeadQuarterParish();
            }
        }



        internal List<RegisteredStructureChurchParishReportObj> GetChurchStructureParishHeadQuartersByChurchStateId(long churchId, int stateId)
        {
            try
            {

                if (churchId < 1 || stateId < 1)
                {
                    return new List<RegisteredStructureChurchParishReportObj>();
                }

                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) return new List<RegisteredStructureChurchParishReportObj>();

                var check2 = myItemList.FindAll(x => x.ChurchId == churchId && x.StateOfLocationId == stateId);

                var retList = new List<RegisteredStructureChurchParishReportObj>();
                check2.ForEachx(m =>
                {
                    string msg;
                    var structureName = _churchStructureTypeRepository.GetById(m.ChurchStructureTypeId).Name;

                    retList.Add(new RegisteredStructureChurchParishReportObj
                    {
                        ChurchStructureParishHeadQuarterId = m.ChurchStructureParishHeadQuarterId,
                        ChurchStructureTypeId = m.ChurchStructureTypeId,
                        ChurchStructureTypeName = structureName,
                        ChurchId = m.ChurchId,
                        StateOfLocationId = m.StateOfLocationId,
                        StateOfLocationName = "",
                        Parishes = m.Parish
                    });
                });

                return retList.ToList();
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RegisteredStructureChurchParishReportObj>();
            }
        }

        internal List<StructureChurchHeadQuarterParish> GetStructureParishHeadQuartersByIds(long churchId, int stateId, List<string> structureChurchHeadQuarterParishIds)
        {
            try
            {

                if (churchId < 1 || stateId < 1)
                {
                    return new List<StructureChurchHeadQuarterParish>();
                }

                var myItemLists = GetChurchStructureParishHeadQuartersByChurchStateId(churchId, stateId).ToList();
                if (!myItemLists.Any()) return new List<StructureChurchHeadQuarterParish>();

                #region First Method


                 //var checks = 
                 //       thisMonthChurchServiceAttendances.Select(
                 //           thisMonthChurchServiceAttendance =>
                 //               thisMonthChurchServiceAttendance.ServiceAttendanceDetail
                 //                   .ChurchServiceAttendanceCollections)
                 //           .Select(
                 //               collectionTypes =>
                 //                   collectionTypes.Find(x => x.CollectionTypeId == thisCollectionTypeId)).ToList();

                var retList = new List<StructureChurchHeadQuarterParish>();
                foreach (var structureChurchHeadQuarterParishId in structureChurchHeadQuarterParishIds)
                {
                    var hqrtObjs =
                        myItemLists.Select(x => x.Parishes)
                            .Select(
                                p =>
                                    p.Find(
                                        c => c.StructureChurchHeadQuarterParishId == structureChurchHeadQuarterParishId))
                            .ToList();


                    retList.AddRange(hqrtObjs.Where(hqrtObj => hqrtObj != null && hqrtObj.StructureChurchHeadQuarterParishId.Length > 0));

                    //foreach (var myItemList in myItemLists)
                    //{
                    //    var parish =
                    //        myItemList.Parishes.Find(
                    //            p => p.StructureChurchHeadQuarterParishId == structureChurchHeadQuarterParishId);

                    //    if (parish != null && parish.StructureChurchHeadQuarterParishId > 0)
                    //    {
                    //        retList.Add(parish);
                    //    }
                    //}
                }

                #endregion


                // Get From the List of all StructureChurch where only the ChurchStructureType 
                // met with the list of the only ChurchStructureType we used as Parameters

                //var filtered = listOfAllVenuses.Where(x=>!listOfBlockedVenues.Any(y=>y.VenueId == x.Id));

                //var check2 =
                //    myItemList.Where(
                //        x =>
                //            churchStructureTypeList.Any(i => i.ChurchStructureTypeId == x.ChurchStructureTypeId) &&
                //            x.ChurchId == churchId && x.StateOfLocationId == stateId);

                //model.AllRoles.Where(m => model.MyRoleIds.Contains(m.Id)).Select(m => m.Name).ToList();


                //var retList = (from structureChurchHeadQuarterParishId in structureChurchHeadQuarterParishIds
                //    from myItemList in myItemLists
                //    select
                //        myItemList.Parishes.Find(
                //            p => p.StructureChurchHeadQuarterParishId == structureChurchHeadQuarterParishId)).ToList();


                //var check2 =
                //    myItemList.Select(
                //        x =>
                //            x.Parishes.Select(
                //                p => structureChurchHeadQuarterParishIds.Contains(p.StructureChurchHeadQuarterParishId)));

                //var check2 = myItemList.FindAll(x => x.ChurchId == churchId && x.StateOfLocationId == stateId);

                
                //check2.ForEachx(m =>
                //{
                //    string msg;

                //    var parishes = m.Parishes;
                //    if (parishes.Any())
                //    {
                //        parishes.ForEachx(p =>
                //        {
                //            var structureName = _churchStructureTypeRepository.GetById(m.ChurchStructureTypeId).Name;

                //            retList.Add(new StructureChurchHeadQuarterParish
                //            {
                //                StructureChurchHeadQuarterParishId = p.StructureChurchHeadQuarterParishId,
                //                ParishName = p.ParishName,
                //                ChurchStructureTypeId = p.ChurchStructureTypeId,
                //                ChurchStructureTypeName = structureName
                //            });
                //        });
                //    }
                //});


                //myItemList.ForEachx(m =>
                //{
                //    string msg;
                //    var structureName = ServiceChurch.GetChurchStructureTypeNameById(m.ChurchStructureTypeId);

                //    retList.Add(new RegisteredStructureChurchParishReportObj
                //    {
                //        StructureChurchId = m.StructureChurchId,
                //        StateOfLocationId = m.StateOfLocationId,
                //        ChurchId = m.ChurchId,
                //        ChurchStructureTypeName = structureName,
                //        ChurchStructureTypeId = m.ChurchStructureTypeId,
                //        Parishes = m.Parish
                //    });
                //});
                //myItemList.ForEachx(m =>
                //{
                //    retList.ForEachx(x => retList.Add(new StructureChurch
                //    {
                //        StateOfLocationId = x.StateOfLocationId,
                //        ChurchId = m.ChurchId,
                //        ChurchStructureTypeId = m.ChurchStructureTypeId,
                //        Parish = m.Parish
                //    }));

                //});


                return retList;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<StructureChurchHeadQuarterParish>();
            }
        }


        internal List<StructureChurchHeadQuarterParish> GetRegisteredChurchStructureParishHeadQuartersByChurchStateId(long churchId, int stateId, int churchStructureTypeId)
        {
            try
            {

                if (churchId < 1 || stateId < 1)
                {
                    return new List<StructureChurchHeadQuarterParish>();
                }

                // Get the list of only the Church Structure Type to be used as parameter
                //var churchStructureTypeList =
                //    new ChurchStructureRepository().GetChurchHighLevelChurchStructureTypes(churchId,
                //        churchStructureTypeId);
                //if (!churchStructureTypeList.Any()) return new List<RegisteredStructureChurchParishReportObj>();



                //var myItemList = _churchStructurerepository.GetAll().ToList();
                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) return new List<StructureChurchHeadQuarterParish>();

                // Get From the List of all StructureChurch where only the ChurchStructureType 
                // met with the list of the only ChurchStructureType we used as Parameters

                //var filtered = listOfAllVenuses.Where(x=>!listOfBlockedVenues.Any(y=>y.VenueId == x.Id));

                //var check2 =
                //    myItemList.Where(
                //        x =>
                //            churchStructureTypeList.Any(i => i.ChurchStructureTypeId == x.ChurchStructureTypeId) &&
                //            x.ChurchId == churchId && x.StateOfLocationId == stateId);

                var check2 = myItemList.FindAll(x => x.ChurchId == churchId && x.StateOfLocationId == stateId);

                var retList = new List<StructureChurchHeadQuarterParish>();
                check2.ForEachx(m =>
                {
                    string msg;

                    var parishes = m.Parish;
                    if (parishes.Any())
                    {
                        parishes.ForEachx(p =>
                        {
                            var structureName = _churchStructureTypeRepository.GetById(m.ChurchStructureTypeId).Name;

                            retList.Add(new StructureChurchHeadQuarterParish
                            {
                                StructureChurchHeadQuarterParishId = p.StructureChurchHeadQuarterParishId,
                                ParishName = p.ParishName,
                                ChurchStructureTypeId = p.ChurchStructureTypeId,
                                ChurchStructureTypeName = structureName
                            });
                        });
                    }
                });


                //myItemList.ForEachx(m =>
                //{
                //    string msg;
                //    var structureName = ServiceChurch.GetChurchStructureTypeNameById(m.ChurchStructureTypeId);

                //    retList.Add(new RegisteredStructureChurchParishReportObj
                //    {
                //        StructureChurchId = m.StructureChurchId,
                //        StateOfLocationId = m.StateOfLocationId,
                //        ChurchId = m.ChurchId,
                //        ChurchStructureTypeName = structureName,
                //        ChurchStructureTypeId = m.ChurchStructureTypeId,
                //        Parishes = m.Parish
                //    });
                //});
                //myItemList.ForEachx(m =>
                //{
                //    retList.ForEachx(x => retList.Add(new StructureChurch
                //    {
                //        StateOfLocationId = x.StateOfLocationId,
                //        ChurchId = m.ChurchId,
                //        ChurchStructureTypeId = m.ChurchStructureTypeId,
                //        Parish = m.Parish
                //    }));

                //});


                return retList.ToList();
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<StructureChurchHeadQuarterParish>();
            }
        }


        #region Remove HeadQuarter Parish

        internal bool RemoveChurchStructureChurchHeadQuarterParish(long churchId, int stateId, string structureChurchHeadQuarterParishId, out string msg)
        {
            try
            {

                if (structureChurchHeadQuarterParishId.IsNullOrEmpty() || structureChurchHeadQuarterParishId.Length < 1)
                {
                    msg = "Invalid HeadQuarter Id";
                    return false;
                }

                if (churchId < 1 || stateId < 1)
                {
                    msg = "Invalid Parent Church Id or State of Location";
                    return false;
                }

                // Get ChurchStructureTypeId GetChurchStructureParishHeadQuarterChurchStructureTypeId(long churchId, int stateId, string structureChurchHeadQuarterParishId)
                var typeId = GetChurchStructureParishHeadQuarterChurchStructureTypeId(churchId, stateId,
                    structureChurchHeadQuarterParishId);

                // Get this ChurchStructureChurchHeadQuarterParish
                var existingParishHqtr = IsChurchStructureParishHeadQuarterExist(churchId, stateId, typeId);
                if (existingParishHqtr == null || existingParishHqtr.ChurchStructureParishHeadQuarterId < 1)
                {
                    msg = "No HeadQuarter parish info found";
                    return false;
                }
                

                // Get the Parish that match with (structureChurchHeadQuarterParishId) in this ChurchStructureChurchHeadQuarterParish
                var leftParishHqtrs = existingParishHqtr.Parish.FindAll(x => x.StructureChurchHeadQuarterParishId != structureChurchHeadQuarterParishId);
                if (!leftParishHqtrs.Any())
                {
                    msg = "No HeadQuarter parish info found";
                    return false;
                }

                //var tempExistingParishHqtr = existingParishHqtr;

                //var existingParish = tempExistingParishHqtr.Parish.Find(x => x.StructureChurchHeadQuarterParishId == structureChurchHeadQuarterParishId);
                //if (existingParish == null || existingParish.StructureChurchHeadQuarterParishId.Length == 0 ||
                //    existingParish.ChurchStructureTypeName.Length == 0)
                //{
                //    msg = "No HeadQuarter parish info found";
                //    return false;
                //}

                // Remove this matched Parish
                //tempExistingParishHqtr.Parish.RemoveAll(x => x.StructureChurchHeadQuarterParishId == existingParish.StructureChurchHeadQuarterParishId);
                //existingParishHqtr._Parish = JsonConvert.SerializeObject(tempExistingParishHqtr.Parish);
                existingParishHqtr._Parish = JsonConvert.SerializeObject(leftParishHqtrs);

                // Update this matched ChurchStructureChurchHeadQuarterParish
                var processedChurchStructureParishHeadQuarter = _repository.Update(existingParishHqtr);
                _uoWork.SaveChanges();
                if (processedChurchStructureParishHeadQuarter.ChurchStructureParishHeadQuarterId < 1)
                {
                    msg = "Process Failed! Please try again later";
                    return false;
                }

                msg = "";
                return true;

            }
            catch (Exception ex)
            {
                msg = "Unable to complete your request due to error! Please try again later: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        #endregion

        internal int GetChurchStructureParishHeadQuarterChurchStructureTypeId(long churchId, int stateId, string structureChurchHeadQuarterParishId)
        {
            try
            {
                if (churchId < 1 || stateId < 1 || structureChurchHeadQuarterParishId.Length == 0)
                {
                    return 0;
                }

                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) return 0;

                var check2 = myItemList.FindAll(x => x.ChurchId == churchId && x.StateOfLocationId == stateId).ToList();

                return (from parish in check2
                    where parish.Parish != null
                    select
                        parish.Parish.Find(
                            x => x.StructureChurchHeadQuarterParishId == structureChurchHeadQuarterParishId)
                    into matchParish
                    where matchParish != null && matchParish.StructureChurchHeadQuarterParishId.Length > 0
                    select matchParish.ChurchStructureTypeId).FirstOrDefault();

                #region Old Method to get ChurchStructureTypeId

                //foreach (var parish in check2)
                //{
                //    if (parish.Parish != null)
                //    {
                //        var matchParish =
                //            parish.Parish.Find(
                //                x => x.StructureChurchHeadQuarterParishId == structureChurchHeadQuarterParishId);
                //        if (matchParish != null && matchParish.StructureChurchHeadQuarterParishId.Length > 0)
                //        {
                //            retVal = matchParish.ChurchStructureTypeId;
                //            break;
                //        }
                //    }
                //}

                #endregion


            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        internal ChurchStructureParishHeadQuarter IsChurchStructureParishHeadQuarterExist(long churchId, int stateId, int churchStructureTypeId)
        {
            try
            {

                if (churchId < 1 || stateId < 1)
                {
                    return new ChurchStructureParishHeadQuarter();
                }

                // Get the list of only the Church Structure Type to be used as parameter
                //var churchStructureTypeList =
                //    new ChurchStructureRepository().GetChurchHighLevelChurchStructureTypes(churchId,
                //        churchStructureTypeId);
                //if (!churchStructureTypeList.Any()) return new List<RegisteredStructureChurchParishReportObj>();



                //var myItemList = _churchStructurerepository.GetAll().ToList();
                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) return new ChurchStructureParishHeadQuarter();

                // Get From the List of all StructureChurch where only the ChurchStructureType 
                // met with the list of the only ChurchStructureType we used as Parameters

                //var filtered = listOfAllVenuses.Where(x=>!listOfBlockedVenues.Any(y=>y.VenueId == x.Id));

                //var check2 =
                //    myItemList.Where(
                //        x =>
                //            churchStructureTypeList.Any(i => i.ChurchStructureTypeId == x.ChurchStructureTypeId) &&
                //            x.ChurchId == churchId && x.StateOfLocationId == stateId);

                var check2 =
                    myItemList.Find(
                        x =>
                            x.ChurchId == churchId && x.StateOfLocationId == stateId &&
                            x.ChurchStructureTypeId == churchStructureTypeId);



                //var retList = new List<StructureChurchHeadQuarterParish>();
                //check2.ForEachx(m =>
                //{
                //    string msg;

                //    var parishes = m.Parish;
                //    if (parishes.Any())
                //    {
                //        parishes.ForEachx(p =>
                //        {
                //            var structureName = _churchStructureTypeRepository.GetById(m.ChurchStructureTypeId).Name;

                //            retList.Add(new StructureChurchHeadQuarterParish
                //            {
                //                StructureChurchHeadQuarterParishId = p.StructureChurchHeadQuarterParishId,
                //                ParishName = p.ParishName,
                //                ChurchStructureTypeId = p.ChurchStructureTypeId,
                //                ChurchStructureTypeName = structureName
                //            });
                //        });
                //    }
                //});


                //myItemList.ForEachx(m =>
                //{
                //    string msg;
                //    var structureName = ServiceChurch.GetChurchStructureTypeNameById(m.ChurchStructureTypeId);

                //    retList.Add(new RegisteredStructureChurchParishReportObj
                //    {
                //        StructureChurchId = m.StructureChurchId,
                //        StateOfLocationId = m.StateOfLocationId,
                //        ChurchId = m.ChurchId,
                //        ChurchStructureTypeName = structureName,
                //        ChurchStructureTypeId = m.ChurchStructureTypeId,
                //        Parishes = m.Parish
                //    });
                //});
                //myItemList.ForEachx(m =>
                //{
                //    retList.ForEachx(x => retList.Add(new StructureChurch
                //    {
                //        StateOfLocationId = x.StateOfLocationId,
                //        ChurchId = m.ChurchId,
                //        ChurchStructureTypeId = m.ChurchStructureTypeId,
                //        Parish = m.Parish
                //    }));

                //});


                return check2;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ChurchStructureParishHeadQuarter();
            }
        }


        internal List<RegisteredStructureChurchParishReportObj> GetRegisteredChurchStructureParishHeadQuartersObjByChurchStateId(long churchId, int stateId, int churchStructureTypeId)
        {
            try
            {

                if (churchId < 1 || stateId < 1)
                {
                    return new List<RegisteredStructureChurchParishReportObj>();
                }

                // Get the list of only the Church Structure Type to be used as parameter
                //var churchStructureTypeList =
                //    new ChurchStructureRepository().GetChurchHighLevelChurchStructureTypes(churchId,
                //        churchStructureTypeId);
                //if (!churchStructureTypeList.Any()) return new List<RegisteredStructureChurchParishReportObj>();



                //var myItemList = _churchStructurerepository.GetAll().ToList();
                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) return new List<RegisteredStructureChurchParishReportObj>();

                // Get From the List of all StructureChurch where only the ChurchStructureType 
                // met with the list of the only ChurchStructureType we used as Parameters

                //var filtered = listOfAllVenuses.Where(x=>!listOfBlockedVenues.Any(y=>y.VenueId == x.Id));

                //var check2 =
                //    myItemList.Where(
                //        x =>
                //            churchStructureTypeList.Any(i => i.ChurchStructureTypeId == x.ChurchStructureTypeId) &&
                //            x.ChurchId == churchId && x.StateOfLocationId == stateId);

                var check2 = myItemList.FindAll(x => x.ChurchId == churchId && x.StateOfLocationId == stateId);

                var retList = new List<RegisteredStructureChurchParishReportObj>();
                check2.ForEachx(m =>
                {

                    //var check = churchStructureTypeList.
                    string msg;
                    var structureName = _churchStructureTypeRepository.GetById(m.ChurchStructureTypeId).Name;
                    //var structureName = ServiceChurch.GetChurchStructureTypeNameById(m.ChurchStructureTypeId);

                    retList.Add(new RegisteredStructureChurchParishReportObj
                    {
                        ChurchStructureParishHeadQuarterId = m.ChurchStructureParishHeadQuarterId,
                        StateOfLocationId = m.StateOfLocationId,
                        ChurchId = m.ChurchId,
                        ChurchStructureTypeName = structureName,
                        ChurchStructureTypeId = m.ChurchStructureTypeId,
                        Parishes = m.Parish
                    });
                });
                //myItemList.ForEachx(m =>
                //{
                //    string msg;
                //    var structureName = ServiceChurch.GetChurchStructureTypeNameById(m.ChurchStructureTypeId);

                //    retList.Add(new RegisteredStructureChurchParishReportObj
                //    {
                //        StructureChurchId = m.StructureChurchId,
                //        StateOfLocationId = m.StateOfLocationId,
                //        ChurchId = m.ChurchId,
                //        ChurchStructureTypeName = structureName,
                //        ChurchStructureTypeId = m.ChurchStructureTypeId,
                //        Parishes = m.Parish
                //    });
                //});
                //myItemList.ForEachx(m =>
                //{
                //    retList.ForEachx(x => retList.Add(new StructureChurch
                //    {
                //        StateOfLocationId = x.StateOfLocationId,
                //        ChurchId = m.ChurchId,
                //        ChurchStructureTypeId = m.ChurchStructureTypeId,
                //        Parish = m.Parish
                //    }));

                //});


                return retList.ToList();
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RegisteredStructureChurchParishReportObj>();
            }
        }

    }
}
