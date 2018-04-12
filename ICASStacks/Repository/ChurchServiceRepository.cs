using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using ICASStacks.APIObjs;
using ICASStacks.DataContract;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.DataContract.Enum;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using ICASStacks.Repository.Helpers;
using ICASStacks.StackService;
using WebCribs.TechCracker.WebCribs.TechCracker;
using WebCribs.TechCracker.WebCribs.TechCracker.EnumInfo;

namespace ICASStacks.Repository
{
    internal class ChurchServiceRepository
    {
        private readonly IIcasRepository<ChurchService> _repository;
        private readonly IIcasRepository<Church> _churchRepository;
        private readonly IIcasRepository<ChurchServiceType> _churchServiceTypeRepository;
        private readonly IcasUoWork _uoWork;

        public ChurchServiceRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ChurchService>(_uoWork);
            _churchRepository = new IcasRepository<Church>(_uoWork);
            _churchServiceTypeRepository = new IcasRepository<ChurchServiceType>(_uoWork);
        }


        #region Latest Church Services

        internal ParentChurchServiceRegObj GetParentChurchServiceRegObj()
        {
            try
            {

                var myChurchServiceTypeItemLists = _churchServiceTypeRepository.GetAll(x => x.SourceId == ChurchSettingSource.App).ToList();
                var myDayOfWeekItemLists = EnumHelper.GetEnumList(typeof(WeekDays));
                if ((myDayOfWeekItemLists == null || !myDayOfWeekItemLists.Any()) &&
                    (myChurchServiceTypeItemLists.Any()))
                {
                    return null;
                }

                var retObj = new ParentChurchServiceRegObj
                {
                    ChurchServices = new List<ChurchServiceObj>()
                };
                myChurchServiceTypeItemLists.ForEachx(x =>
                {

                    var dayOfWeekObjs = new List<NameAndValueObject>();
                    if (myDayOfWeekItemLists != null)
                    {
                         dayOfWeekObjs = myDayOfWeekItemLists.Select(m => new NameAndValueObject
                        {
                            Name = m.Name,
                            Id = m.Id
                        }).ToList();
                    }

                    retObj.ChurchServices.Add(new ChurchServiceObj
                    {
                        ChurchServiceTypeId = x.ChurchServiceTypeId,
                        Name = x.Name,
                        WeekDays = dayOfWeekObjs
                    });
                    
                });

                return retObj;

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ParentChurchServiceRegObj();
            }
        }
        internal ParentChurchServiceRegObj GetParentChurchServiceRegObj(long churchId)
        {
            try
            {

                if (churchId < 1)
                {
                    return new ParentChurchServiceRegObj();
                }

                var myChurchServiceTypeItemLists = _churchServiceTypeRepository.GetAll(x => x.SourceId == ChurchSettingSource.App).ToList();
                var myDayOfWeekItemLists = EnumHelper.GetEnumList(typeof(WeekDays));
                if ((myDayOfWeekItemLists == null || !myDayOfWeekItemLists.Any()) &&
                    (myChurchServiceTypeItemLists.Any()))
                {
                    return null;
                }

                var retObj = new ParentChurchServiceRegObj
                {
                    ChurchServices = new List<ChurchServiceObj>()
                };

                myChurchServiceTypeItemLists.ForEachx(x =>
                {
                    var dayOfWeekObjs = new List<NameAndValueObject>();
                    if (myDayOfWeekItemLists != null)
                    {
                        dayOfWeekObjs = myDayOfWeekItemLists.Select(m => new NameAndValueObject
                        {
                            Name = m.Name,
                            Id = m.Id
                        }).ToList();
                    }


                    // Check ChurchService if thisChurchServiceType with the ChurchId exist
                    // If Exist, then
                    var thisTypeChurchService = GetChurchServiceByChurchIdWithTypeId(churchId, x.ChurchServiceTypeId);

                    retObj.ChurchServices.Add(new ChurchServiceObj
                    {
                        ChurchServiceId = thisTypeChurchService.ChurchServiceId,
                        ChurchServiceTypeId = x.ChurchServiceTypeId,
                        Name = x.Name,
                        //WeekDay = Enum.GetName(typeof(WeekDays), Int32.Parse(thisTypeChurchService.DayOfWeekId.ToString(CultureInfo.InvariantCulture))),
                        //SelectedDayOfWeek = thisTypeChurchService.DayOfWeekId.ToString(CultureInfo.InvariantCulture),
                        WeekDays = dayOfWeekObjs
                    });
                   

                   
                });

                return retObj;

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ParentChurchServiceRegObj();
            }
        }
        internal ParentChurchServiceRegObj GetParentChurchServiceDetailObj(long churchId)
        {
            try
            {

                if (churchId < 1)
                {
                    return new ParentChurchServiceRegObj();
                }

                var myChurchServiceTypeItemLists = _churchServiceTypeRepository.GetAll(x => x.SourceId == ChurchSettingSource.App).ToList();
                var myDayOfWeekItemLists = EnumHelper.GetEnumList(typeof(WeekDays));
                if ((myDayOfWeekItemLists == null || !myDayOfWeekItemLists.Any()) &&
                    (myChurchServiceTypeItemLists.Any()))
                {
                    return null;
                }

                var retObj = new ParentChurchServiceRegObj
                {
                    ChurchServices = new List<ChurchServiceObj>()
                };

                myChurchServiceTypeItemLists.ForEachx(x =>
                {
                    var dayOfWeekObjs = new List<NameAndValueObject>();
                    if (myDayOfWeekItemLists != null)
                    {
                        dayOfWeekObjs = myDayOfWeekItemLists.Select(m => new NameAndValueObject
                        {
                            Name = m.Name,
                            Id = m.Id
                        }).ToList();
                    }


                    // Check ChurchService if thisChurchServiceType with the ChurchId exist
                    // If Exist, then
                    var thisTypeChurchService = GetChurchServiceByChurchIdWithTypeId(churchId, x.ChurchServiceTypeId);

                    if (thisTypeChurchService != null && thisTypeChurchService.ChurchServiceId > 0)
                    {
                        retObj.ChurchServices.Add(new ChurchServiceObj
                        {
                            ChurchServiceId = thisTypeChurchService.ChurchServiceId,
                            ChurchServiceTypeId = x.ChurchServiceTypeId,
                            Name = x.Name,
                            //WeekDay = Enum.GetName(typeof(WeekDays), Int32.Parse(thisTypeChurchService.DayOfWeekId.ToString(CultureInfo.InvariantCulture))),
                            //SelectedDayOfWeek = thisTypeChurchService.DayOfWeekId.ToString(CultureInfo.InvariantCulture),
                            WeekDays = dayOfWeekObjs
                        });
                    }

                });

                return retObj;

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ParentChurchServiceRegObj();
            }
        }

        #endregion

        internal ParentChurchServiceRegResponse AddParentChurchService(ParentChurchServiceRegObj churchServiceRegObj)
        {

            var response = new ParentChurchServiceRegResponse
            {
                ChurchId = churchServiceRegObj.ChurchId,
                ChurchServices = new List<ParentChurchServiceObj>(),
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            churchServiceRegObj.ChurchServices.ForEachx(x => response.ChurchServices.Add(new ParentChurchServiceObj
            {
                ChurchServiceId = 0,
                ChurchServiceTypeId = x.ChurchServiceTypeId,
                SelectedDayOfWeek = x.SelectedDayOfWeek
            }));

            try
            {
                if (churchServiceRegObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Church registration Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(churchServiceRegObj, out valResults))
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
                if (churchServiceRegObj.AddedByUserId < 1 && churchServiceRegObj.ChurchId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Admin User Information";
                    response.Status.Message.TechnicalMessage = "Invalid Admin User Information";
                    response.Status.IsSuccessful = false;
                    return response;
                }


                using (var db = _uoWork.BeginTransaction())
                {
                    try
                    {

                        foreach (var thisService in churchServiceRegObj.ChurchServices)
                        {
                            if (thisService != null && thisService.ChurchServiceTypeId > 0 && !string.IsNullOrEmpty(thisService.SelectedDayOfWeek))
                            {
                                var churchServiceObj = new ChurchService
                                {
                                    ChurchId = churchServiceRegObj.ChurchId,
                                    //ChurchServiceTypeId = thisService.ChurchServiceTypeId,
                                    //DayOfWeekId = Int32.Parse(thisService.SelectedDayOfWeek),
                                    AddedByUserId = churchServiceRegObj.AddedByUserId,
                                    Status = (int) ChurchServiceStatus.Active,
                                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp(),
                                };

                                string msg;
                                bool reset;
                                long resetId;

                                if (IsDuplicate(churchServiceObj, out reset, out resetId, out msg))
                                {
                                    response.Status.Message.FriendlyMessage = msg;
                                    response.Status.Message.TechnicalMessage = msg;
                                    response.Status.IsSuccessful = false;
                                    return response;
                                }

                                // Check if the exist status is In_Active and reset it back, don't bother to add new
                                if (reset)
                                {
                                    continue;
                                }

                                // Submit Church Service
                                var processedChurchService = _repository.Add(churchServiceObj);
                                _uoWork.SaveChanges();
                                if (processedChurchService.ChurchServiceId < 1)
                                {
                                    db.Rollback();
                                    response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                                    response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                                    response.Status.IsSuccessful = false;
                                    return response;
                                }

                                response.ChurchServices.Add(new ParentChurchServiceObj
                                {
                                    ChurchServiceId = processedChurchService.ChurchServiceId,
                                });

                            }
                        }

                        db.Commit();
                        response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                        response.Status.IsSuccessful = true;
                        return response;
                        
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                        response.Status.Message.FriendlyMessage = "Parent Church Services registration Failed! Please try again later";
                        response.Status.Message.TechnicalMessage = "Error: " + ex.GetBaseException().Message;
                        response.Status.IsSuccessful = false;
                        return response;
                    }
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
        internal ChurchServiceRegResponse AddChurchService(ChurchServiceRegObj churchServiceRegObj)
        {

            var response = new ChurchServiceRegResponse
            {
                ChurchServiceId = 0,
                ClientId = churchServiceRegObj.ClientId,
                Name = churchServiceRegObj.Name,
                DayOfWeekId = churchServiceRegObj.DayOfWeekId,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {
                if (churchServiceRegObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Church registration Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(churchServiceRegObj, out valResults))
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
                if (churchServiceRegObj.AddedByUserId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Admin User Information";
                    response.Status.Message.TechnicalMessage = "Invalid Admin User Information";
                    response.Status.IsSuccessful = false;
                    return response;
                }

                string msg;
                bool reset;
                long resetId;

                if (IsDuplicate(churchServiceRegObj, out reset, out resetId, out msg))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }

                // Check if the exist status is In_Active and reset it back, don't bother to add new
                if (reset)
                {
                    response.ChurchServiceId = resetId;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;
                }

                //if (IsDuplicate(churchServiceRegObj.ClientId, churchServiceRegObj.ChurchServiceId, churchServiceRegObj.Name, out msg))
                //{
                //    response.Status.Message.FriendlyMessage = msg;
                //    response.Status.Message.TechnicalMessage = msg;
                //    response.Status.IsSuccessful = false;
                //    return response;
                //}

                // Submit Church Service
                var thisChurchService = new ChurchService
                {
                    //ClientId = churchServiceRegObj.ClientId,
                    //Name = churchServiceRegObj.Name,
                    //DayOfWeekId = churchServiceRegObj.DayOfWeekId,
                    AddedByUserId = churchServiceRegObj.AddedByUserId,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp(),
                    Status = (int)ChurchServiceStatus.Active
                };

                var processedChurchService = _repository.Add(thisChurchService);
                _uoWork.SaveChanges();
                if (processedChurchService.ChurchServiceId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                    response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                    response.Status.IsSuccessful = false;
                    return response;
                }

                response.ChurchServiceId = processedChurchService.ChurchServiceId;
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

        internal RespStatus UpdateChurchService(ChurchServiceRegObj churchServiceRegObj)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };

            try
            {
                if (churchServiceRegObj.Equals(null))
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your processing";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(churchServiceRegObj, out valResults))
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
                var thisChurchService = GetChurchService(churchServiceRegObj.ChurchServiceId, out msg);
                //if (thisChurchService == null || thisChurchService.Name.Length < 1)
                //{
                //    response.Message.FriendlyMessage =
                //        response.Message.TechnicalMessage =
                //            string.IsNullOrEmpty(msg) ? "Invalid Church Information" : msg;
                //    return response;
                //}

                //if (IsDuplicate(churchServiceRegObj.ClientId, churchServiceRegObj.Name, out msg, 1))
                //{
                //    response.Message.FriendlyMessage = msg;
                //    response.Message.TechnicalMessage = msg;
                //    response.IsSuccessful = false;
                //    return response;
                //}

                bool reset;
                long resetId;

                if (IsDuplicate(churchServiceRegObj, out reset, out resetId, out msg, 1))
                {
                    response.Message.FriendlyMessage = msg;
                    response.Message.TechnicalMessage = msg;
                    response.IsSuccessful = false;
                    return response;
                }

                // Check if the exist status is In_Active and reset it back, don't bother to add new
                if (reset)
                {
                    thisChurchService.Status = (int)ChurchServiceStatus.Remove;
                }
                else
                {
                    //thisChurchService.Name = churchServiceRegObj.Name;
                    //thisChurchService.DayOfWeekId = churchServiceRegObj.DayOfWeekId;
                }

                var processedChurchService = _repository.Update(thisChurchService);
                _uoWork.SaveChanges();
                if (processedChurchService.ChurchServiceId < 1)
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
                //myItemList.ForEachx(m =>
                //{
                //    var parentChurch = _churchRepository.GetById(m.ChurchId);
                //    var structureName = ServiceChurch.GetChurchStructureTypeNameById(m.ChurchStructureTypeId);

                //    if (parentChurch != null)
                //    {
                //        if (parentChurch.ChurchId > 0)
                //        {
                //            if (parentChurch.ChurchId != currentChurchId)
                //            {
                //                churchCounter++;
                //                churchIds.Add(churchCounter);
                //            }


                //            retList.Add(new RegisteredChurchStructureListReportObj
                //            {
                //                ChurchStructureId = m.ChurchStructureId,
                //                ChurchId = m.ChurchId,
                //                ChurchStructureTypeId = m.ChurchStructureTypeId,
                //                HierachyLevel = m.HierachyLevel,
                //                ParentChurchName = parentChurch.Name,
                //                ChurchStructureTypeName = structureName,
                //                Status = m.Status,
                //                StatusName = m.Status.ToString().Replace("_", " "),
                //            });
                //        }

                //        currentChurchId = parentChurch.ChurchId;
                //    }

                //});

                return retList.FindAll(x => x.Status != ChurchStructureStatus.Remove);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RegisteredChurchStructureListReportObj>();
            }
        }


        internal List<RegisteredChurchServiceReportObj> GetAllRegisteredChurchServiceObjs()
        {
            try
            {
                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) return new List<RegisteredChurchServiceReportObj>();

                long currentChurchId = 0;
                var churchIds = new List<long>();
                var churchCounter = 0;

                var retList = new List<RegisteredChurchServiceReportObj>();
                myItemList.ForEachx(m =>
                {
                    var parentChurch = _churchRepository.GetById(m.ChurchId);
                    //var serviceType = ServiceChurch.GetChurchServiceTypeNameById(m.ChurchServiceTypeId);

                    if (parentChurch != null)
                    {
                        if (parentChurch.ChurchId > 0)
                        {
                            if (parentChurch.ChurchId != currentChurchId)
                            {
                                churchCounter++;
                                churchIds.Add(churchCounter);
                            }

                            retList.Add(new RegisteredChurchServiceReportObj
                            {
                                ChurchServiceId = m.ChurchServiceId,
                                //ChurchServiceTypeId = m.ChurchServiceTypeId,
                                //ChurchServiceName = serviceType,
                                //ChurchServiceName = new ChurchServiceTypeRepository().GetChurchServiceTypeNameById(m.ChurchServiceTypeId),
                                ChurchId = m.ChurchId,
                                ParentChurchName = parentChurch.Name,
                                //DayOfWeekId = m.DayOfWeekId,
                                //Status = Enum.GetValues(typeof(ChurchServiceStatus), m.Status),
                                StatusName = m.Status.ToString(CultureInfo.InvariantCulture).Replace("_", " "),
                                //ServiceDay = Enum.GetName(typeof(WeekDays), m.DayOfWeekId)
                            });
                        }

                        currentChurchId = parentChurch.ChurchId;
                    }

                });

                return retList.FindAll(x => x.Status != ChurchServiceStatus.Remove); ;

                //var clientChurchServices = retList.FindAll(x => x.ChurchId == churchId).ToList();
                //var defaultChurchServices = retList.FindAll(x => x.ChurchId == 1).ToList();

                //defaultChurchServices.AddRange(clientChurchServices);
                //return defaultChurchServices;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RegisteredChurchServiceReportObj>(); ;
            }
        }

        internal RespStatus RemoveClientChurchService(long churchServiceId)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };

            try
            {
                if (churchServiceId < 1)
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your processing";
                    return response;
                }

                string msg;
                var thisClientChurchService = GetChurchService(churchServiceId, out msg);
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
                if (processedClientChurchService.ChurchServiceId < 1)
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
        
        internal ChurchService GetChurchServiceByChurchIdWithTypeId(long churchId, int churchServiceTypeId)
        {
            try
            {
                if (churchId < 1)
                {
                    return new ChurchService();
                }

                //var myItem =
                //    _repository.GetAll(
                //        x =>
                //            x.ChurchId == churchId && x.ChurchServiceTypeId == churchServiceTypeId &&
                //            x.Status == (int) ChurchServiceStatus.Active).ToList();
                //if (!myItem.Any() || myItem.Count != 1) return new ChurchService();

                //return myItem[0];
                return null;

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ChurchService();
            }
        }

        internal List<RegisteredChurchServiceReportObj> GetAllRegisteredChurchServiceObjs(long churchId)
        {
            try
            {
                if (churchId < 1)
                {
                    return new List<RegisteredChurchServiceReportObj>();
                }

                var myItemList = _repository.GetAll(x => x.ChurchId == churchId && x.Status == (int)ChurchServiceStatus.Active);
                if (!myItemList.Any()) return new List<RegisteredChurchServiceReportObj>();

                var retList = new List<RegisteredChurchServiceReportObj>();
                myItemList.ForEachx(m => retList.Add(new RegisteredChurchServiceReportObj
                {
                    ChurchServiceId = m.ChurchServiceId,
                    //ChurchServiceTypeId = m.ChurchServiceTypeId,
                    //ChurchServiceName = new ChurchServiceTypeRepository().GetChurchServiceTypeNameById(m.ChurchServiceTypeId),
                    ChurchId = m.ChurchId,
                    //DayOfWeekId = m.DayOfWeekId,
                    //ServiceDay = Enum.GetName(typeof(WeekDays), m.DayOfWeekId)
                }));

                return retList;

                //var clientChurchServices = retList.FindAll(x => x.ChurchId == churchId).ToList();
                //var defaultChurchServices = retList.FindAll(x => x.ChurchId == 1).ToList();

                //defaultChurchServices.AddRange(clientChurchServices);
                //return defaultChurchServices;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RegisteredChurchServiceReportObj>();
            }
        }

        internal ChurchService GetChurchService(long churchId)
        {
            try
            {
                //var sql1 =
                // string.Format(
                //     "Select * FROM \"ChurchAPPDB\".\"ChurchService\"  WHERE \"ChurchId\" = {0};", churchId);

                //var check = _repository.RepositoryContext().Database.SqlQuery<ChurchService>(sql1).ToList();

                var myItem = _repository.GetAll(x => x.ChurchId == churchId).ToList();
                if (myItem.IsNullOrEmpty() || !myItem.Any())
                {
                    return new ChurchService();
                }

                //if (check.IsNullOrEmpty())
                //{
                //    return null;
                //}
                //if (check.Count != 1)
                //{
                //    return null;
                //}
                return myItem[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ChurchService();
            }
        }
        private ChurchService GetChurchService(long churchServiceId, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchAPPDB\".\"ChurchService\"  WHERE \"ChurchServiceId\" = {0};", churchServiceId);

                var check = _repository.RepositoryContext().Database.SqlQuery<ChurchService>(sql1).ToList();
                if (check.IsNullOrEmpty())
                {
                    msg = "No Church Service record found!";
                    return null;
                }
                if (check.Count != 1)
                {
                    msg = "Invalid Church Service Record!";
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



        internal List<ChurchServiceDetailObj> GetChurchServiceServiceTypeDetail(long churchId)
        {

            try
            {
                if (churchId < 1)
                {
                    return new List<ChurchServiceDetailObj>();
                }
                var churchService = GetChurchService(churchId);
                if (churchService == null || churchService.ChurchServiceId < 1)
                {
                    return new List<ChurchServiceDetailObj>();
                }

                return churchService.ServiceTypeDetail;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ChurchServiceDetailObj>();
            }
            
        }



        private bool IsDuplicate(ChurchService churchService, out bool reset, out long resetId, out string msg, int status = 0)
        {
            try
            {
                var check = new List<ChurchService>();
                var churchId = churchService.ChurchId;
                //var churchServiceTypeId = churchService.ChurchServiceTypeId;
                //var dayOfWeekId = churchService.DayOfWeekId;
                //if (churchId > 0 && churchServiceTypeId > 0 && dayOfWeekId > 0)
                //{
                //    var sql =
                //        string.Format(
                //        "Select * FROM \"ChurchAPPDB\".\"ChurchService\" WHERE " +
                //        " \"ChurchId\" = {0} " +
                //        " AND \"ChurchServiceTypeId\" = {1}", churchId, churchServiceTypeId);
                //    check = _repository.RepositoryContext().Database.SqlQuery<ChurchService>(sql).ToList();
                //}

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
                            if (check[0].Status == (int) ChurchServiceStatus.Remove)
                            {
                                check[0].Status = (int)ChurchServiceStatus.Active;
                            }
                            //check[0].DayOfWeekId = dayOfWeekId;
                            check[0].TimeStampAdded = DateScrutnizer.CurrentTimeStamp();
                            check[0].AddedByUserId = churchService.AddedByUserId;

                            var resetService = _repository.Update(check[0]);
                            _uoWork.SaveChanges();
                            if (resetService.ChurchServiceId > 1)
                            {
                                msg = "";
                                reset = true;
                                resetId = resetService.ChurchServiceId;
                                return false;
                            }

                            msg = "Process Failed! Please try again later";
                            reset = false;
                            resetId = 0;
                            return true;


                            

                            //msg = "Duplicate Error! Church Structure already exist";
                            //reset = false;
                            //resetId = 0;
                            //return true;
                        }
                        break;

                    case 1:
                        if (check.Count >= 1)
                        {
                            // Check if the exist status is In_Active and reset it back, don't bother to add new
                            if (check[0].Status == (int)ChurchServiceStatus.Remove)
                            {
                                check[0].Status = (int)ChurchServiceStatus.Active;
                            }
                            //check[0].DayOfWeekId = dayOfWeekId;
                            check[0].TimeStampAdded = DateScrutnizer.CurrentTimeStamp();
                            check[0].AddedByUserId = churchService.AddedByUserId;

                            var resetService = _repository.Update(check[0]);
                            _uoWork.SaveChanges();
                            if (resetService.ChurchServiceId > 1)
                            {
                                msg = "";
                                reset = true;
                                resetId = resetService.ChurchServiceId;
                                return false;
                            }

                            msg = "Process Failed! Please try again later";
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
                        "Select * FROM \"ChurchAPPDB\".\"ChurchService\" WHERE " +
                        " \"ChurchServiceId\" = {0} " +
                        " AND \"ClientId\" = {1} "+
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

        private bool IsDuplicate(ChurchServiceRegObj churchServiceRegObj, out bool reset, out long resetId, out string msg, int status = 0)
        {
            try
            {
                var check = new List<ChurchService>();
                if (churchServiceRegObj.ClientId > 0 && !string.IsNullOrEmpty(churchServiceRegObj.Name))
                {
                    var sql =
                        string.Format(
                        "Select * FROM \"ChurchAPPDB\".\"ChurchService\" WHERE " +
                        " \"ChurchServiceId\" = {0} " +
                        " AND \"ClientId\" = {1} "+
                        " AND lower(\"Name\") = lower('{2}')", churchServiceRegObj.ClientId, churchServiceRegObj.ChurchServiceId, churchServiceRegObj.Name);
                    check = _repository.RepositoryContext().Database.SqlQuery<ChurchService>(sql).ToList();
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
                            if (check[0].Status == (int)ChurchServiceStatus.Remove && check[0].ChurchServiceId > 1)
                            {
                                // Reset the status to Active
                                check[0].Status = (int)ChurchServiceStatus.Active;
                                check[0].TimeStampAdded = DateScrutnizer.CurrentTimeStamp();
                                //check[0].DayOfWeekId = churchServiceRegObj.DayOfWeekId;
                                check[0].AddedByUserId = churchServiceRegObj.AddedByUserId;

                                var resetService = _repository.Update(check[0]);
                                _uoWork.SaveChanges();
                                if (resetService.ChurchServiceId > 1)
                                {
                                    msg = "";
                                    reset = true;
                                    resetId = resetService.ChurchServiceId;
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
                                //check[0].DayOfWeekId = churchServiceRegObj.DayOfWeekId;
                                check[0].AddedByUserId = churchServiceRegObj.AddedByUserId;

                                var resetService = _repository.Update(check[0]);
                                _uoWork.SaveChanges();
                                if (resetService.ChurchServiceId > 1)
                                {
                                    msg = "";
                                    reset = true;
                                    resetId = resetService.ChurchServiceId;
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
    }
}
