using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using ICASStacks.APIObjs;
using ICASStacks.DataContract;
using ICASStacks.DataContract.Enum;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using ICASStacks.Repository.Helpers;
using ICASStacks.StackService;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ChurchStructureRepository
    {
        private readonly IIcasRepository<ChurchStructure> _repository;
        private readonly IIcasRepository<Church> _churchRepository;
        private readonly IIcasRepository<ChurchStructureType> _churchStructureTypeRepository; 
        private readonly IcasUoWork _uoWork;


        public ChurchStructureRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ChurchStructure>(_uoWork);
            _churchRepository = new IcasRepository<Church>(_uoWork);
            //_churchStructureHierachyRepository = new IcasRepository<ChurchStructureHierachy>(_uoWork);
            _churchStructureTypeRepository = new IcasRepository<ChurchStructureType>(_uoWork);
        }


        internal ChurchStructureRegResponse AddChurchStructure(ChurchStructureRegObj churchStructureRegObj)
        {
            var response = new ChurchStructureRegResponse
            {
                //ChurchId = 0,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {
                if (churchStructureRegObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Church Structure Hierachy Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(churchStructureRegObj, out valResults))
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
                        var myChurchStructureTypes = churchStructureRegObj.MyChurchStructureTypeIds;
                        for (var i = 0; i < myChurchStructureTypes.Length; i++)
                        {

                            string msg;
                            bool reset;
                            long resetId;
                            if (IsDuplicate(churchStructureRegObj.ChurchId, myChurchStructureTypes[i], out reset, out resetId, out msg))
                            {
                                continue;
                            }

                            // Check if the exist status is In_Active and reset it back, don't bother to add new
                            if (reset)
                            {
                                continue;
                            }

                            var thisChurchStructure = new ChurchStructure
                            {
                                ChurchId = churchStructureRegObj.ChurchId,

                                ChurchStructureTypeDetail = new List<ChurchStructureTypeDetailObj>(),

                                //ChurchStructureTypeId = myChurchStructureTypes[i],
                                //HierachyLevel = churchStructureRegObj.HierachyLevel,
                                LastModificationTimeStamp = DateScrutnizer.CurrentTimeStamp(),
                                LastModificationByUserId = churchStructureRegObj.RegisteredByUserId,
                                RegisteredByUserId = churchStructureRegObj.RegisteredByUserId,
                                Status = ChurchStructureStatus.Active,
                            };

                            var processedChurchStructure = _repository.Add(thisChurchStructure);
                            _uoWork.SaveChanges();
                            if (processedChurchStructure.ChurchStructureId < 1)
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

        internal RespStatus UpdateChurchStructureWithHierachys(ChurchStructureHierachyResetObj churchStructureHierachyResetObj)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };

            try
            {
                if (churchStructureHierachyResetObj.Equals(null))
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your processing";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(churchStructureHierachyResetObj, out valResults))
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

                // Get This Church Structure Types with ChurchId
                var thisChurchStructures = GetChurchStructuresByChurchId(churchStructureHierachyResetObj.ChurchId);
                if (thisChurchStructures == null || !thisChurchStructures.Any())
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Invalid Church Information";
                    return response;
                }

                foreach (var thisChurchStructure in thisChurchStructures)
                {
                    //var type = _churchStructureTypeRepository.GetById(thisChurchStructure.ChurchStructureTypeId).Name;
                    
                    //switch (type)
                    //{
                    //    case "Region":
                    //        thisChurchStructure.HierachyLevel = churchStructureHierachyResetObj.RegionId.GetValueOrDefault();
                    //        thisChurchStructure.LastModificationByUserId = churchStructureHierachyResetObj.LastModificationByUserId;
                    //        break;
                    //    case "Province":
                    //        thisChurchStructure.HierachyLevel = churchStructureHierachyResetObj.ProvinceId.GetValueOrDefault();
                    //        thisChurchStructure.LastModificationByUserId = churchStructureHierachyResetObj.LastModificationByUserId;
                    //        break;
                    //    case "Zone":
                    //       thisChurchStructure.HierachyLevel = churchStructureHierachyResetObj.ZoneId.GetValueOrDefault();
                    //        thisChurchStructure.LastModificationByUserId = churchStructureHierachyResetObj.LastModificationByUserId;
                    //        break;
                    //    case "Area":
                    //        thisChurchStructure.HierachyLevel = churchStructureHierachyResetObj.AreaId.GetValueOrDefault();
                    //        thisChurchStructure.LastModificationByUserId = churchStructureHierachyResetObj.LastModificationByUserId;
                    //        break;
                    //    case "Diocese":
                    //       thisChurchStructure.HierachyLevel = churchStructureHierachyResetObj.DioceseId.GetValueOrDefault();
                    //        thisChurchStructure.LastModificationByUserId = churchStructureHierachyResetObj.LastModificationByUserId;
                    //        break;
                    //    case "District":
                    //        thisChurchStructure.HierachyLevel = churchStructureHierachyResetObj.DistrictId.GetValueOrDefault();
                    //        thisChurchStructure.LastModificationByUserId = churchStructureHierachyResetObj.LastModificationByUserId;
                    //        break;
                    //    case "State":
                    //        thisChurchStructure.HierachyLevel = churchStructureHierachyResetObj.StateId.GetValueOrDefault();
                    //        thisChurchStructure.LastModificationByUserId = churchStructureHierachyResetObj.LastModificationByUserId;
                    //        break;
                    //    case "Group":
                    //        thisChurchStructure.HierachyLevel = churchStructureHierachyResetObj.GroupId.GetValueOrDefault();
                    //        thisChurchStructure.LastModificationByUserId = churchStructureHierachyResetObj.LastModificationByUserId;
                    //        break;

                    //    case "Branch":
                    //        thisChurchStructure.HierachyLevel = churchStructureHierachyResetObj.BranchId.GetValueOrDefault();
                    //        thisChurchStructure.LastModificationByUserId = churchStructureHierachyResetObj.LastModificationByUserId;
                    //        break;
                    //    case "Parish":
                    //        thisChurchStructure.HierachyLevel = churchStructureHierachyResetObj.ParishId.GetValueOrDefault();
                    //        thisChurchStructure.LastModificationByUserId = churchStructureHierachyResetObj.LastModificationByUserId;
                    //        break;
                    //}
                }


                using (var db = _uoWork.BeginTransaction())
                {
                    try
                    {
                        foreach (var thisChurchStructure in thisChurchStructures)
                        {
                            var processedChurchStructureHierachy = _repository.Update(thisChurchStructure);
                            _uoWork.SaveChanges();
                            if (processedChurchStructureHierachy.ChurchStructureId < 1)
                            {
                                db.Rollback();
                                response.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                                response.Message.TechnicalMessage = "Process Failed! Unable to update data";
                                response.IsSuccessful = false;
                                return response;
                            }
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
                response.Message.FriendlyMessage = "Unable to complete your request due to error! Please try again later";
                response.Message.TechnicalMessage = "Error" + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return response;
            }
        }


        internal bool UpdatChurchStructure(ChurchStructure agentChurchStructure, out string msg)
        {
            try
            {
                var processedChurchStructure = _repository.Update(agentChurchStructure);
                _uoWork.SaveChanges();
                msg = "";
                return processedChurchStructure.ChurchStructureId > 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        
        internal List<RegisteredChurchStructureListReportObj> GetAllRegisteredChurchStructureObjs()
        {
            try
            {
                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) return new List<RegisteredChurchStructureListReportObj>();

                long currentChurchId = 0;
                var churchIds = new List<long>();
                var churchCounter = 0;


                var retList = new List<RegisteredChurchStructureListReportObj>();
                myItemList.ForEachx(m =>
                {
                    var parentChurch = _churchRepository.GetById(m.ChurchId);
                    //var structureName = ServiceChurch.GetChurchStructureTypeNameById(m.ChurchStructureTypeId);

                    if (parentChurch != null)
                    {
                        if (parentChurch.ChurchId > 0)
                        {
                            if (parentChurch.ChurchId != currentChurchId)
                            {
                                churchCounter++;
                                churchIds.Add(churchCounter);
                            }


                            retList.Add(new RegisteredChurchStructureListReportObj
                            {
                                ChurchStructureId = m.ChurchStructureId,
                                ChurchId = m.ChurchId,
                                //ChurchStructureTypeId = m.ChurchStructureTypeId,
                                //HierachyLevel = m.HierachyLevel,
                                //ParentChurchName = parentChurch.Name,
                                //ChurchStructureTypeName = structureName,
                                Status = m.Status,
                                StatusName = m.Status.ToString().Replace("_", " "),
                            });
                        }

                        currentChurchId = parentChurch.ChurchId;
                    }
                    
                });

                return retList.FindAll(x => x.Status != ChurchStructureStatus.Remove);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RegisteredChurchStructureListReportObj>();
            }
        }

        private bool IsDuplicate(long churchId, int structureTypeId, out bool reset, out long resetId, out string msg)
        {
            try
            {
                var check = new List<ChurchStructure>();
                if (churchId > 0 && structureTypeId > 0)
                {
                    var sql =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ChurchStructure\" WHERE \"ChurchId\" = {0} AND \"ChurchStructureTypeId\" = {1}",
                            churchId, structureTypeId);
                    check = _repository.RepositoryContext().Database.SqlQuery<ChurchStructure>(sql).ToList();
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
                    if (check[0].Status == ChurchStructureStatus.In_Active || check[0].Status == ChurchStructureStatus.Remove)
                    {
                        // Reset the status to Active
                        var resetObj = EnableChurchStructure(churchId, structureTypeId);
                        if (resetObj.Status.IsSuccessful)
                        {
                            msg = "";
                            reset = true;
                            resetId = resetObj.ChurchStructureId;
                            return false;
                        }

                        //msg = "Unable to check duplicate! Please try again later";
                        msg = string.IsNullOrEmpty(resetObj.Status.Message.FriendlyMessage) ? "Unable to update this client" : resetObj.Status.Message.FriendlyMessage;
                        reset = false;
                        resetId = 0;
                        return true;
                    }

                    msg = "Duplicate Error! Church Structure already exist";
                    reset = false;
                    resetId = 0;
                    return true;
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

        #region CHURCH STRUCTURE ADMINISTRATIONS

        internal List<ChurchStructuresLookUpObj> GetChurchStructureHierachyLookUp(long churchId)
        {
            try
            {
                var retList = new List<ChurchStructuresLookUpObj>();
                if (churchId < 1) { return new List<ChurchStructuresLookUpObj>(); }

                // Get the Church Structure Hierachy record for the selected Church
                var churchStructures = _repository.GetAll().ToList();
                if (!churchStructures.Any()) return new List<ChurchStructuresLookUpObj>();

                churchStructures.ForEachx(m =>
                {
                    //var typeName =
                    //    new ChurchStructureTypeRepository().GetChurchStructureTypeNameById(m.ChurchStructureTypeId);

                    var churchStructureLookUpObj = new ChurchStructuresLookUpObj
                    {
                        //ChurchStructureTypeId = m.ChurchStructureTypeId,
                        //ChurchStructureTypeName = typeName,
                        Status = m.Status == ChurchStructureStatus.Active,
                    };

                    retList.Add(churchStructureLookUpObj);
                });

                return retList;
            }
            catch (Exception ex)
            {
                return new List<ChurchStructuresLookUpObj>();
            }
        } 

        internal List<ChurchStructureType> GetDisplayActiveChurchStructureTypes(long churchId, int structureTypeId)
        {
            try
            {
                if (churchId < 1 || structureTypeId < 1) { return new List<ChurchStructureType>(); }

                // Get the Church Structure Hierachy record for the selected Church Structure Type
                var churchStructure = GetChurchStructure(churchId, structureTypeId);
                if (churchStructure == null || churchStructure.ChurchStructureId < 1) return null;

                // Get the Church Structure Hierachy records that has high rank than the selected Church Structure Type
                //var highRankChurchStructures =
                //    GetChurchStructuresByChurchId(churchId)
                //        .FindAll(x => x.HierachyLevel <= churchStructure.HierachyLevel);

                //if (!highRankChurchStructures.Any()) return null;
                
                var retList = new List<ChurchStructureType>();
                //highRankChurchStructures.ForEachx(m =>
                //{
                //    if (m.Status == ChurchStructureStatus.Active)
                //    {
                //        var churchStructureType = _churchStructureTypeRepository.GetById(m.ChurchStructureTypeId);
                //        retList.Add(churchStructureType);
                //    }
                //});

                return retList;

            }
            catch (Exception ex)
            {
                return new List<ChurchStructureType>();
            }
        }


        internal ChurchStructureResponseObj DisableChurchStructure(long churchId, int structureTypeId)
        {

            var response = new ChurchStructureResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                NewStatus = "",
                ChurchStructureId = 0,
            };

            try
            {
                string msg;
                var thisChurchStructure = GetChurchStructure(churchId, structureTypeId);
                if (thisChurchStructure == null)
                {
                    response.Status.Message.FriendlyMessage = "Target Church structure information is invalid";
                    response.Status.Message.TechnicalMessage = "Target Church structure cannot be found!";
                    return response;
                }

                thisChurchStructure.Status = ChurchStructureStatus.In_Active;
                var retVal = UpdatChurchStructure(thisChurchStructure, out msg);

                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Update Failed!";
                    return response;
                }

                response.ChurchStructureId = thisChurchStructure.ChurchStructureId;
                response.NewStatus = ChurchStructureStatus.In_Active.ToString().Replace("_", " ");
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

        internal ChurchStructureResponseObj EnableChurchStructure(long churchId, int structureTypeId)
        {

            var response = new ChurchStructureResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                NewStatus = "",
                ChurchStructureId = 0,
            };

            try
            {
                string msg;
                var thisChurchStructure = GetChurchStructure(churchId, structureTypeId);
                if (thisChurchStructure == null)
                {
                    response.Status.Message.FriendlyMessage = "Target Church structure information is invalid";
                    response.Status.Message.TechnicalMessage = "Target Church structure cannot be found!";
                    return response;
                }

                thisChurchStructure.Status = ChurchStructureStatus.Active;
                var retVal = UpdatChurchStructure(thisChurchStructure, out msg);

                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Update Failed!";
                    return response;
                }

                response.ChurchStructureId = thisChurchStructure.ChurchStructureId;
                response.NewStatus = ChurchStructureStatus.Active.ToString().Replace("_", " ");
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

        //internal ChurchStructureResponseObj RemoveChurchStructure(long churchStructureId, int structureTypeId)
        internal ChurchStructureResponseObj RemoveChurchStructure(long churchId, int structureTypeId)
        {

            var response = new ChurchStructureResponseObj
            {
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                NewStatus = "",
                ChurchStructureId = 0,
            };

            try
            {
                string msg;
                var thisChurchStructure = GetChurchStructure(churchId, structureTypeId);
                if (thisChurchStructure == null)
                {
                    response.Status.Message.FriendlyMessage = "Target Church structure information is invalid";
                    response.Status.Message.TechnicalMessage = "Target Church structure cannot be found!";
                    return response;
                }

                thisChurchStructure.Status = ChurchStructureStatus.Remove;
                var retVal = UpdatChurchStructure(thisChurchStructure, out msg);

                if (!retVal)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete your request at this time! Please try again later";
                    response.Status.Message.TechnicalMessage = "Update Failed!";
                    return response;
                }

                response.ChurchStructureId = thisChurchStructure.ChurchStructureId;
                response.NewStatus = ChurchStructureStatus.Remove.ToString().Replace("_", " ");
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

        internal ChurchStructure GetChurchStructure(long churchStructureId)
        {
            try
            {
                var myItem = _repository.GetAll(m => m.ChurchStructureId == churchStructureId).ToList();
                if (!myItem.Any() || myItem.Count() != 1) { return null; }
                return myItem[0].ChurchStructureId < 1 ? null : myItem[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        //internal ChurchStructure GetChurchStructure(long churchStructureId, int structureTypeId)
        internal ChurchStructure GetChurchStructure(long churchId, int structureTypeId)
        {
            try
            {
                //var myItem = _repository.GetAll(m => m.ChurchId == churchId && m.ChurchStructureTypeId == structureTypeId).ToList();
                var myItem = _repository.GetAll(m => m.ChurchId == churchId).ToList();
                if (!myItem.Any() || myItem.Count() != 1) { return null; }
                return myItem[0].ChurchStructureId < 1 ? null : myItem[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal List<ChurchStructure> GetChurchStructuresByChurchId(long churchId)
        {
            try
            {
                var myItems = _repository.GetAll(m => m.ChurchId == churchId).ToList();
                if (!myItems.Any()) { return null; }
                return myItems;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal ChurchStructure GetChurchStructureType(long churchId)
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


        internal List<ChurchStructureType> GetChurchStructureTypeByChurchId(long churchId)
        {
            try
            {
                var myItems = _repository.GetAll(m => m.ChurchId == churchId).ToList();
                if (!myItems.Any()) { return null; }

                var retItems = new List<ChurchStructureType>();
                var thisChurchStructureTypes = myItems[0].ChurchStructureTypeDetail;
                
                thisChurchStructureTypes.ForEachx(x => retItems.Add(new ChurchStructureType
                {
                    ChurchStructureTypeId = x.ChurchStructureTypeId,
                    Name = x.ChurchStructureTypeName
                }));

                return retItems;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
        
        
        internal ChurchStructure CheckChurchStructureActive(long churchId, int structureTypeId)
        {
            try
            {
                //var myItem = _repository.GetAll(m => m.ChurchId == churchId && m.ChurchStructureTypeId == structureTypeId).ToList();
                var myItem = _repository.GetAll(m => m.ChurchId == churchId).ToList();
                if (!myItem.Any() || myItem.Count() != 1) { return null; }
                return myItem[0].ChurchStructureId < 1 ? null : myItem[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal bool IsChurchStructureActive(long churchId, int structureTypeId)
        {
            try
            {
                //var myItem =
                //    _repository.GetAll(
                //        m =>
                //            m.ChurchId == churchId && m.ChurchStructureTypeId == structureTypeId &&
                //            m.Status == ChurchStructureStatus.Active).ToList();

                var myItem =
                    _repository.GetAll(
                        m =>
                            m.ChurchId == churchId &&
                            m.Status == ChurchStructureStatus.Active).ToList();

                if (!myItem.Any() || myItem.Count() != 1) { return false; }
                return true;
                //return myItem[0].Status == ChurchStructureStatus.Active;
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
