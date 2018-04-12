using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using ICASStacks.APIObjs;
using ICASStacks.DataContract;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.DataContract.Enum;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using ICASStacks.Repository.Helpers;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ChurchCollectionTypeRepository
    {

        private readonly IIcasRepository<ChurchCollectionType> _repository;
        private readonly IIcasRepository<Church> _churchRepository;
        private readonly IIcasRepository<CollectionType> _collectionTypeRepository;
        private readonly IIcasRepository<ChurchStructureType> _churchStructureTypeRepository; 
        private readonly IcasUoWork _uoWork;


        public ChurchCollectionTypeRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ChurchCollectionType>(_uoWork);
            _churchRepository = new IcasRepository<Church>(_uoWork);
            _collectionTypeRepository = new IcasRepository<CollectionType>(_uoWork);
            _churchStructureTypeRepository = new IcasRepository<ChurchStructureType>(_uoWork);
        }

        internal ParentChurchCollectionTypeRegResponse AddParentChurchCollectionType(ParentChurchCollectionTypeRegObj parentChurchCollectionTypeRegObj)
        {
            var response = new ParentChurchCollectionTypeRegResponse
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {
                if (parentChurchCollectionTypeRegObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Church Collection Type(s) Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(parentChurchCollectionTypeRegObj, out valResults))
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

                using (var db = _uoWork.BeginTransaction())
                {
                    try
                    {
                        var myChurchCollectionTypes = parentChurchCollectionTypeRegObj.MyChurchCollectionTypeIds;
                        for (var i = 0; i < myChurchCollectionTypes.Length; i++)
                        {

                            string msg;
                            bool reset;
                            long resetId;

                            if (IsDuplicate(parentChurchCollectionTypeRegObj.ChurchId, myChurchCollectionTypes[i], out reset, out resetId, out msg))
                            {
                                continue;
                            }

                            var thisChurchCollectionType = new ChurchCollectionType
                            {
                                ChurchId = parentChurchCollectionTypeRegObj.ChurchId,
                                //CollectionTypeId = myChurchCollectionTypes[i],
                                TimeStampAdded = DateScrutnizer.CurrentTimeStamp(),
                                Status = PreferenceTypeStatus.Active,
                                AddedByUserId = parentChurchCollectionTypeRegObj.AddedByUserId,
                            };

                            var processedCollectionType = _repository.Add(thisChurchCollectionType);
                            _uoWork.SaveChanges();
                            if (processedCollectionType.ChurchCollectionTypeId < 1)
                            {
                                db.Rollback();
                                response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                                response.Status.Message.TechnicalMessage = "Process Failed! Unable to register voter";
                                response.Status.IsSuccessful = false;
                                return response;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                        response.Status.Message.FriendlyMessage = "Church Structure Type Registration Failed! Please try again later";
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

        internal bool UpdatChurchCollectionType(ChurchCollectionType agentChurchCollectionType, out string msg)
        {
            try
            {
                var processedChurchCollectionType = _repository.Update(agentChurchCollectionType);
                _uoWork.SaveChanges();
                msg = "";
                return processedChurchCollectionType.ChurchCollectionTypeId > 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        
        internal List<RegisteredChurchCollectionTypeListReportObj> GetAllRegisteredChurchCollectionTypeObjs()
        {
            try
            {
                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) return new List<RegisteredChurchCollectionTypeListReportObj>();

                long currentChurchId = 0;
                var churchIds = new List<long>();
                var churchCounter = 0;


                var retList = new List<RegisteredChurchCollectionTypeListReportObj>();
                myItemList.ForEachx(m =>
                {
                    var parentChurch = _churchRepository.GetById(m.ChurchId);
                    var collectionTypeName = _collectionTypeRepository.GetById(1).Name;
                    //var collectionTypeName = _collectionTypeRepository.GetById(m.CollectionTypeId).Name;

                    if (parentChurch != null)
                    {
                        if (parentChurch.ChurchId > 0)
                        {
                            if (parentChurch.ChurchId != currentChurchId)
                            {
                                churchCounter++;
                                churchIds.Add(churchCounter);
                            }

                            retList.Add(new RegisteredChurchCollectionTypeListReportObj
                            {
                                ChurchCollectionTypeId = m.ChurchCollectionTypeId,
                                ChurchId = m.ChurchId,
                                //CollectionTypeId = m.CollectionTypeId,
                                ParentChurchName = parentChurch.Name,
                                ChurchCollectionTypeName = collectionTypeName,
                                Status = m.Status,
                                StatusName = m.Status.ToString().Replace("_", " "),
                            });
                        }

                        currentChurchId = parentChurch.ChurchId;
                    }

                });

                return retList.FindAll(x => x.Status != PreferenceTypeStatus.Remove);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RegisteredChurchCollectionTypeListReportObj>();
            }
        }
        
        private bool IsDuplicate(long churchId, int collectionTypeId, out bool reset, out long resetId, out string msg)
        {
            try
            {
                var check = new List<ChurchCollectionType>();
                if (churchId > 0 && collectionTypeId > 0)
                {
                    var sql =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ChurchCollectionType\" WHERE \"ChurchId\" = {0} AND \"CollectionTypeId\" = {1}",
                            churchId, collectionTypeId);
                    check = _repository.RepositoryContext().Database.SqlQuery<ChurchCollectionType>(sql).ToList();
                }

                msg = "";
                if (check.IsNullOrEmpty())
                {
                    reset = false;
                    resetId = 0;
                    return false;
                }
                if (check.Count > 0)
                {

                    // Check if the exist status is In_Active and reset it back, don't bother to add new
                    if (check[0].Status == PreferenceTypeStatus.In_Active || check[0].Status == PreferenceTypeStatus.Remove)
                    {
                        // Reset the status to Active
                        var resetObj = EnableChurchCollectionType(churchId, collectionTypeId);
                        if (resetObj.Status.IsSuccessful)
                        {
                            msg = "";
                            reset = true;
                            resetId = resetObj.ChurchCollectionTypeId;
                            return false;
                        }

                        msg = string.IsNullOrEmpty(resetObj.Status.Message.FriendlyMessage) ? "Unable to update this church collection type" : resetObj.Status.Message.FriendlyMessage;
                        reset = false;
                        resetId = 0;
                        return true;
                    }

                    msg = "Duplicate Error! Church Collection Type already exist";
                    reset = false;
                    resetId = 0;
                    return true;
                }

                msg = "";
                reset = false;
                resetId = 0;
                return false;

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

        private bool IsDuplicate(long churchId, int collectionTypeId, out string msg)
        {
            try
            {
                var check = new List<ChurchCollectionType>();
                if (churchId > 0 && collectionTypeId > 0)
                {
                    var sql =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ChurchCollectionType\" WHERE \"ChurchId\" = {0} AND \"CollectionTypeId\" = {1}",
                            churchId, collectionTypeId);
                    check = _repository.RepositoryContext().Database.SqlQuery<ChurchCollectionType>(sql).ToList();
                }

                msg = "";
                if (check.IsNullOrEmpty())
                {
                    return false;
                }
                if (check.Count > 0)
                {

                    msg = "Duplicate Error! Church Collection Type already exist";
                    return true;
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



        #region CHURCH COLLECTION TYPE ADMINISTRATIONS
        internal ParentChurchCollectionTypeResponseObj EnableChurchCollectionType(long churchId, int collectionTypeId)
        {

            var response = new ParentChurchCollectionTypeResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                NewStatus = "",
                ChurchCollectionTypeId = 0,
            };

            try
            {
                string msg;
                var thisChurchCollectionType = GetChurchCollectionType(churchId, collectionTypeId);
                if (thisChurchCollectionType == null)
                {
                    response.Status.Message.FriendlyMessage = "Target Church collection type information is invalid";
                    response.Status.Message.TechnicalMessage = "Target Church collection type cannot be found!";
                    return response;
                }

                thisChurchCollectionType.Status = PreferenceTypeStatus.Active;
                var retVal = UpdatChurchCollectionType(thisChurchCollectionType, out msg);

                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Update Failed!";
                    return response;
                }

                response.ChurchCollectionTypeId = thisChurchCollectionType.ChurchCollectionTypeId;
                response.NewStatus = PreferenceTypeStatus.Active.ToString().Replace("_", " ");
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

        internal ParentChurchCollectionTypeResponseObj RemoveChurchCollectionType(long churchId, int collectionTypeId)
        {

            var response = new ParentChurchCollectionTypeResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                NewStatus = "",
                ChurchCollectionTypeId = 0,
            };

            try
            {
                string msg;
                var thisChurchCollectionType = GetChurchCollectionType(churchId, collectionTypeId);
                if (thisChurchCollectionType == null)
                {
                    response.Status.Message.FriendlyMessage = "Target Church collection type information is invalid";
                    response.Status.Message.TechnicalMessage = "Target Church collection type cannot be found!";
                    return response;
                }

                thisChurchCollectionType.Status = PreferenceTypeStatus.Remove;
                var retVal = UpdatChurchCollectionType(thisChurchCollectionType, out msg);

                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Update Failed!";
                    return response;
                }

                response.ChurchCollectionTypeId = thisChurchCollectionType.ChurchCollectionTypeId;
                response.NewStatus = PreferenceTypeStatus.Remove.ToString().Replace("_", " ");
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

        internal ChurchCollectionType GetChurchCollectionType(long churchId, int collectionTypeId)
        {
            try
            {
                //var myItem = _repository.GetAll(m => m.ChurchId == churchId && m.CollectionTypeId == collectionTypeId).ToList();
                var myItem = _repository.GetAll(m => m.ChurchId == churchId).ToList();
                if (!myItem.Any() || myItem.Count() != 1) { return null; }
                return myItem[0].ChurchCollectionTypeId < 1 ? null : myItem[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal List<ChurchServiceAttendanceCollectionObj> GetChurchCollectionTypesByChurchId(long churchId)
        {
            try
            {
                var myItems = _repository.GetAll(m => m.ChurchId == churchId).ToList();
                if (!myItems.Any()) { return null; }

                //var retItems = new List<ChurchServiceAttendanceCollectionObj>();
                //myItems.ForEachx(x =>
                //{
                //    x.ChurchCollectionTypePercentages.ForEachx(n =>
                //    {
                //        var collectionTypeName = _collectionTypeRepository.GetById(n.CollectionTypeId).Name;
                //        retItems.Add(new ChurchServiceAttendanceCollectionObj
                //        {
                //            CollectionTypeId = n.CollectionTypeId,
                //            CollectionTypeName = collectionTypeName
                //        });
                //    });
                //});

                var retItems = new List<ChurchServiceAttendanceCollectionObj>();
                myItems.ForEachx(x => x.CollectionTypes.ForEachx(n =>
                {
                    var collectionTypeName = _collectionTypeRepository.GetById(n.CollectionTypeId).Name;
                    retItems.Add(new ChurchServiceAttendanceCollectionObj
                    {
                        CollectionTypeId = n.CollectionTypeId,
                        CollectionTypeName = collectionTypeName
                    });
                }));
                return retItems;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal ChurchCollectionType GetChurchCollectionType(long churchId)
        {
            try
            {
                var myItems = _repository.GetAll(m => m.ChurchId == churchId).ToList();
                if (!myItems.Any()) { return null; }
                return myItems[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }


        internal List<CollectionTypeObj> GetChurchCollectionTypeDetail(long churchId)
        {

            try
            {
                if (churchId < 1)
                {
                    return new List<CollectionTypeObj>();
                }
                var churchCollectionType = GetChurchCollectionType(churchId);
                if (churchCollectionType == null || churchCollectionType.ChurchCollectionTypeId < 1)
                {
                    return new List<CollectionTypeObj>();
                }

                return churchCollectionType.CollectionTypes;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<CollectionTypeObj>();
            }

        }





        internal ChurchCollectionTypeSettingObjs GetChurchCollectionTypeForSetting(long churchId)
        {
            try
            {
                var myItems = _repository.GetAll(m => m.ChurchId == churchId).ToList();
                if (!myItems.Any()) { return null; }

                var retItem = new ChurchCollectionTypeSettingObjs();
               
                foreach (var churchCollectionType in myItems[0].CollectionTypes)
                {
                    var collectionName = _collectionTypeRepository.GetById(churchCollectionType.CollectionTypeId).Name;
                    var churchCollection = new ChurchCollectionTypeObj
                    {
                        CollectionTypeId = churchCollectionType.CollectionTypeId,
                        Name = collectionName
                    };

                    //foreach (var structure in churchCollectionType.ChurchStructureTypeObjs)
                    //{
                    //    var structureName = _churchStructureTypeRepository.GetById(structure.ChurchStructureTypeId).Name;
                    //    var percent = (structure.Percent * 100);
                    //    structure.Percent = percent;
                        
                    //    var churchCollectionStructure = new ChurchCollectionChurchStructureTypeObj
                    //    {
                    //        ChurchStructureTypeId = structure.ChurchStructureTypeId,
                    //        Name = structureName,
                    //        Percent = percent
                    //    };

                    //    churchCollection.ChurchStructureTypeObjs.Add(churchCollectionStructure);
                    //}

                    retItem.ChurchCollectionStructureTypes.Add(churchCollection);
                }

                return retItem;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal bool IsChurchCollectionTypeActive(long churchId, int collectionTypeId)
        {
            try
            {
                var myItem =
                    _repository.GetAll(
                        m =>
                            m.ChurchId == churchId && m.ChurchCollectionTypeId == collectionTypeId &&
                            m.Status == PreferenceTypeStatus.Active).ToList();

                if (!myItem.Any() || myItem.Count() != 1) { return false; }
                return true;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        #endregion
        
    }
}
