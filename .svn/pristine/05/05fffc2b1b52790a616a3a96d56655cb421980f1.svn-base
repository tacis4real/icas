using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using ICASStacks.APIObjs;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using ICASStacks.Repository.Helpers;
using Newtonsoft.Json;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ClientChurchCollectionTypeRepository
    {

        private readonly IIcasRepository<ClientChurchCollectionType> _repository;
        private readonly IIcasRepository<CollectionType> _collectionTypeRepository;
        private readonly IcasUoWork _uoWork;


        public ClientChurchCollectionTypeRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ClientChurchCollectionType>(_uoWork);
        }


        #region Client Church

        internal RespStatus SetClientChurchCollectionTypeForRemittance(ClientChurchCollectionTypeSettingObjs clientChurchCollectionTypeSettingObjs)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };

            try
            {
                #region Validating Incoming Object(s)
                if (clientChurchCollectionTypeSettingObjs.Equals(null))
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your processing";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(clientChurchCollectionTypeSettingObjs, out valResults))
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

                #region Extracting the existiing Record with it's Uinique Id and Making Changes for Update

                var thisClientChurchCollectionType =
                    _repository.GetById(clientChurchCollectionTypeSettingObjs.ClientChurchColletionTypeId);

                if (thisClientChurchCollectionType == null || thisClientChurchCollectionType.ClientChurchCollectionTypeId < 1)
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage = "Invalid Client Church Collection Type Information";
                    return response;
                }

                var helperClientChurchCollectionStructureTypes = clientChurchCollectionTypeSettingObjs.ClientChurchCollectionStructureTypes;
                var helper = new ClientChurchCollectionTypeSettingObjs();

                foreach (var x in helperClientChurchCollectionStructureTypes)
                {
                    var check = x.ChurchStructureTypeObjs;
                    check.Where(i => i.Percent > 0).ToList().ForEachx(j => j.Percent = (j.Percent / 100));

                    helper.ClientChurchCollectionStructureTypes.Add(new CollectionTypeObj
                    {
                        CollectionTypeId = x.CollectionTypeId,
                        CollectionRefId = x.CollectionRefId,
                        Name = x.Name,
                        ChurchStructureTypeObjs = check
                    });
                }

                thisClientChurchCollectionType.CollectionTypes = helper.ClientChurchCollectionStructureTypes;

                #endregion

                #region Sumitting the Updated Existing Record to Database
                var processedClientChurchCollectionType = _repository.Update(thisClientChurchCollectionType);
                _uoWork.SaveChanges();
                if (processedClientChurchCollectionType.ClientChurchCollectionTypeId < 1)
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage = "Process Failed! Please try again later";
                    return response;
                }
                #endregion

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

        
        #region For Client Church Collection Type Modification

        internal ClientChurchCollectionType GetClientChurchCollectionTypeReportObj(long clientChurchId)
        {
            try
            {
                var myItems = _repository.GetAll(m => m.ClientChurchId == clientChurchId).ToList();
                if (!myItems.Any()) { return new ClientChurchCollectionType(); }

                var item = myItems[0];
                var retItem = new ClientChurchCollectionType
                {
                    ClientChurchCollectionTypeId = item.ClientChurchCollectionTypeId,
                    ClientChurchId = item.ClientChurchId,
                    CollectionTypes = new List<CollectionTypeObj>(),
                    AddedByUserId = item.AddedByUserId,
                    TimeStampAdded = item.TimeStampAdded
                };


                // Convert Collection Type Church Structure Percent
                var clientChurchCollectionTypePercentages = item.CollectionTypes;
                var collectionTypes = new List<CollectionTypeObj>(); 
                foreach (var x in clientChurchCollectionTypePercentages)
                {

                    //x.ChurchStructureTypeObjs.Where(i => i.Percent > 0).ToList().ForEachx(j => j.Percent = (100 * j.Percent));
                    var check = x.ChurchStructureTypeObjs;
                    check.Where(i => i.Percent > 0).ToList().ForEachx(j => j.Percent = (100 * j.Percent));

                    collectionTypes.Add(new CollectionTypeObj
                    {
                        CollectionTypeId = x.CollectionTypeId,
                        CollectionRefId = x.CollectionRefId,
                        Name = x.Name,
                        ChurchStructureTypeObjs = check
                    });
                }

                retItem._Collection = JsonConvert.SerializeObject(collectionTypes);
                return retItem;

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ClientChurchCollectionType();
            }
        }

        internal ClientChurchCollectionTypeReportObj GetClientChurchCollectionTypeReportObjs(long clientChurchId)
        {
            try
            {
                var myItems = _repository.GetAll(m => m.ClientChurchId == clientChurchId).ToList();
                if (!myItems.Any()) { return new ClientChurchCollectionTypeReportObj(); }

                var item = myItems[0];
                var retItem = new ClientChurchCollectionTypeReportObj
                {
                    ClientChurchCollectionTypeId = item.ClientChurchCollectionTypeId,
                    ClientChurchId = item.ClientChurchId,
                    ClientChurchServiceAttendanceCollectionObjs = new List<ClientChurchServiceAttendanceCollectionObj>()
                };
                item.CollectionTypes.ForEachx(x => retItem.ClientChurchServiceAttendanceCollectionObjs.Add(new ClientChurchServiceAttendanceCollectionObj
                {
                    CollectionRefId = x.CollectionRefId.ToString(CultureInfo.InvariantCulture),
                    CollectionTypeName = x.Name
                }));
                return retItem;

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ClientChurchCollectionTypeReportObj();
            }
        }

        internal List<ClientChurchServiceAttendanceCollectionObj> GetClientChurchCollectionObjs(long clientChurchId)
        {
            try
            {
                var myItems = _repository.GetAll(m => m.ClientChurchId == clientChurchId).ToList();
                if (!myItems.Any()) { return null; }

                var retItems = new List<ClientChurchServiceAttendanceCollectionObj>();
                myItems.ForEachx(x => x.CollectionTypes.ForEachx(n => retItems.Add(new ClientChurchServiceAttendanceCollectionObj
                {
                    CollectionRefId = n.CollectionRefId.ToString(CultureInfo.InvariantCulture),
                    CollectionTypeName = n.Name
                })));
                return retItems;


            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ClientChurchServiceAttendanceCollectionObj>();
            }
        }

        #endregion


        internal RespStatus UpdateClientChurchCollectionType(ClientChurchCollectionType agentObj)
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
                var thisClientChurchColletionType = GetClientChurchCollectionType(agentObj.ClientChurchCollectionTypeId, out msg);
                if (thisClientChurchColletionType == null || thisClientChurchColletionType.ClientChurchCollectionTypeId < 1)
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage =
                            string.IsNullOrEmpty(msg) ? "Invalid Church collection type information" : msg;
                    return response;
                }

                #region Duplication

                if (IsDuplicate(thisClientChurchColletionType.CollectionTypes, agentObj.CollectionTypes, out msg, 1))
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage =
                            string.IsNullOrEmpty(msg) ? "Duplicate Error! Collection name already exist." : msg;
                    return response;
                }

                #endregion


                thisClientChurchColletionType.CollectionTypes = agentObj.CollectionTypes;
                var processedClientChurchCollectionType = _repository.Update(thisClientChurchColletionType);
                _uoWork.SaveChanges();
                if (processedClientChurchCollectionType.ClientChurchCollectionTypeId < 1)
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

        #endregion




        private bool IsDuplicate(List<CollectionTypeObj> existingObjs, List<CollectionTypeObj> freshObjs, out string msg, int status = 0)
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
                                msg = "Duplicate Error! Collection name: (" +checker[0].Name+ ") already exist";
                                return true;
                            }
                            break;
                        case 1:
                            if (checker.Any())
                            {
                                if (checker.Count <= 1) continue;
                                msg = "Duplicate Error! Collection name: (" + checker[0].Name + ") already exist";
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




        internal  List<ChurchServiceAttendanceCollectionObj> GetClientChurchCollectionObj(long clientId)
        {
            try
            {
                if (clientId < 1) { return new List<ChurchServiceAttendanceCollectionObj>(); }
                var churchId = new ClientRepository().GetClientChurch(clientId).ChurchId;
                if (churchId < 1) { return new List<ChurchServiceAttendanceCollectionObj>(); }
                var retList = new List<ChurchServiceAttendanceCollectionObj>();

                var clientChurchCollectionTypes = GetClientChurchCollectionTypes(clientId);
                
                if (clientChurchCollectionTypes.Any())
                {
                    clientChurchCollectionTypes.ForEachx(x =>
                    {
                        //var collectionTypeName = new CollectionTypeRepository().GetChurchCollectionTypeNameById(x.CollectionTypeId);
                        //retList.Add(new ChurchServiceAttendanceCollectionObj
                        //{
                        //    CollectionTypeId = x.CollectionTypeId,
                        //    CollectionTypeName = collectionTypeName
                        //});
                    });
                }

                return retList;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ChurchServiceAttendanceCollectionObj>();
            }
        }
        
        internal List<ClientChurchCollectionType> GetClientChurchCollectionTypes(long clientId)
        {
            try
            {
                var myItems = _repository.GetAll(m => m.ClientChurchId == clientId).ToList();
                if (!myItems.Any() || myItems.Count() != 1) { return new List<ClientChurchCollectionType>(); }
                return myItems;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
        
        internal ClientChurchCollectionType GetClientChurchCollectionType(long clientChurchCollectionTypeId, out string msg)
        {
            try
            {
                var myItem = _repository.GetById(clientChurchCollectionTypeId);
                if (myItem == null || myItem.ClientChurchCollectionTypeId < 1)
                {
                    msg = "No Client Church Collection Type record found!";
                    return new ClientChurchCollectionType();
                }

                msg = "";
                return myItem;
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new ClientChurchCollectionType(); 
            }
        }

        internal ClientChurchCollectionType GetClientChurchCollectionTypeByClientChurchId(long clientChurchId)
        {
            try
            {
                var myItems = _repository.GetAll(m => m.ClientChurchId == clientChurchId).ToList();
                if (!myItems.Any()) { return null; }
                return myItems[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal ClientChurchCollectionTypeSettingObjs GetClientChurchCollectionTypeForSettings(long clientChurchId)
        {
            try
            {

                var myClientItem = GetClientChurchCollectionTypeByClientChurchId(clientChurchId);
                if (myClientItem == null || myClientItem.ClientChurchCollectionTypeId < 1)
                {
                    return new ClientChurchCollectionTypeSettingObjs();
                }

                // Iniatialize Return Object
                var retItem = new ClientChurchCollectionTypeSettingObjs
                {
                    ClientChurchColletionTypeId = myClientItem.ClientChurchCollectionTypeId,
                    ClientChurchId = clientChurchId,
                    ClientChurchCollectionStructureTypes = new List<CollectionTypeObj>()
                };


                #region Return Original Object Get from ClientChurchCollectionType TABLE

                // Return Client Church Collection Type Settings
                var clientChurchCollectionTypePercentages = myClientItem.CollectionTypes;
                foreach (var x in clientChurchCollectionTypePercentages)
                {
                    var check = x.ChurchStructureTypeObjs;
                    check.Where(i => i.Percent > 0).ToList().ForEachx(j => j.Percent = (100 * j.Percent));

                    retItem.ClientChurchCollectionStructureTypes.Add(new CollectionTypeObj
                    {
                        CollectionTypeId = x.CollectionTypeId,
                        CollectionRefId = x.CollectionRefId,
                        Name = x.Name,
                        ChurchStructureTypeObjs = check
                    });
                }

                retItem.ClientChurchColletionTypeId = myClientItem.ClientChurchCollectionTypeId;

                return retItem;

                #endregion


            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }
        
        internal List<ClientChurchServiceAttendanceCollectionObj> GetClientChurchCollectionTypesByClientChurchId(long clientChurchId)
        {
            try
            {
                var myItems = _repository.GetAll(m => m.ClientChurchId == clientChurchId).ToList();
                if (!myItems.Any()) { return new List<ClientChurchServiceAttendanceCollectionObj>(); }

                var collectionTypes = myItems[0].CollectionTypes;
                if (!collectionTypes.Any() || collectionTypes.Count == 0)
                {
                    return new List<ClientChurchServiceAttendanceCollectionObj>();
                }

                var retItems = new List<ClientChurchServiceAttendanceCollectionObj>();
                collectionTypes.ForEachx(x => retItems.Add(new ClientChurchServiceAttendanceCollectionObj
                {
                    CollectionRefId = x.CollectionRefId,
                    CollectionTypeName = x.Name
                }));
                return retItems;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<ClientChurchServiceAttendanceCollectionObj>();
            }
        }






        #region Unsed

        internal ClientChurchCollectionTypeSettingObjs GetClientChurchCollectionTypeForSetting(long clientChurchId, long churchId)
        {
            try
            {
                var myChurchItem = new ChurchCollectionTypeRepository().GetChurchCollectionType(churchId);
                var churchItems = myChurchItem == null ? new List<CollectionTypeObj>() : myChurchItem.CollectionTypes;

                var myClientItem = GetClientChurchCollectionTypeByClientChurchId(clientChurchId);
                var clientItems = myClientItem == null ? new List<CollectionTypeObj>() : myClientItem.CollectionTypes;

                if ((!churchItems.Any() || churchItems == null) && (!clientItems.Any() || clientItems == null))
                {
                    return new ClientChurchCollectionTypeSettingObjs();
                }

                var churchStructure = new ChurchStructureRepository().GetChurchStructureType(churchId);
                var retItem = new ClientChurchCollectionTypeSettingObjs
                {
                    ClientChurchId = clientChurchId,
                    ClientChurchCollectionStructureTypes = new List<CollectionTypeObj>()
                };


                #region Add Fresh Client Church Collection Types

                var churchCollectionTypePercentages = new List<CollectionTypeObj>();
                var clientChurchCollectionType = new ClientChurchCollectionType
                {
                    ClientChurchId = clientChurchId,
                    AddedByUserId = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };

                if (!clientItems.Any() || clientItems.Count < 1)
                {
                    if (churchItems != null && churchItems.Any())
                    {

                        #region Oldiesssss

                        churchItems.ForEachx(x =>
                        {
                            if (churchStructure != null && churchStructure.ChurchStructureTypeDetail.Any())
                            {
                                #region Assign and Composing - Collection Type ChurchStructures Percent

                                var churchStructureDetails = churchStructure.ChurchStructureTypeDetail;
                                var thisCollectionChurchStructurePercentages =
                                    new List<ChurchCollectionChurchStructureTypeObj>();

                                churchStructureDetails.ForEachx(c => thisCollectionChurchStructurePercentages.Add(new ChurchCollectionChurchStructureTypeObj
                                {
                                    ChurchStructureTypeId = c.ChurchStructureTypeId,
                                    Name = c.ChurchStructureTypeName,
                                    Percent = 0.0
                                }));
                                #endregion

                                #region Adding to Collection Types for Database & Returning Object
                                churchCollectionTypePercentages.Add(new CollectionTypeObj
                                {
                                    CollectionTypeId = x.CollectionTypeId,
                                    Name = x.Name,
                                    ChurchStructureTypeObjs = thisCollectionChurchStructurePercentages
                                });

                                retItem.ClientChurchCollectionStructureTypes.Add(new CollectionTypeObj
                                {
                                    CollectionTypeId = x.CollectionTypeId,
                                    Name = x.Name,
                                    ChurchStructureTypeObjs = thisCollectionChurchStructurePercentages
                                });
                                #endregion

                            }
                            else
                            {

                                #region Adding to Collection Types for Database & Returning Object
                                churchCollectionTypePercentages.Add(new CollectionTypeObj
                                {
                                    CollectionTypeId = x.CollectionTypeId,
                                    Name = x.Name,
                                });

                                retItem.ClientChurchCollectionStructureTypes.Add(new CollectionTypeObj
                                {
                                    CollectionTypeId = x.CollectionTypeId,
                                    Name = x.Name,
                                });
                                #endregion

                            }

                        });
                        #endregion

                        #region Add Fresh Client Church Collection Types to Database

                        clientChurchCollectionType._Collection =
                            JsonConvert.SerializeObject(churchCollectionTypePercentages);
                        var processedClientChurchCollectionType = _repository.Add(clientChurchCollectionType);
                        _uoWork.SaveChanges();
                        if (processedClientChurchCollectionType.ClientChurchCollectionTypeId < 1)
                        {
                            return new ClientChurchCollectionTypeSettingObjs();
                        }
                        #endregion

                        retItem.ClientChurchColletionTypeId = processedClientChurchCollectionType.ClientChurchCollectionTypeId;
                        return retItem;
                    }
                }
                #endregion

                #region Update Client Church Collection Types - If Church Collection is greater than Client

                var newChurchCollectionTypes = new List<CollectionTypeObj>();
                if (churchItems != null && clientItems != null)
                {
                    if (churchItems.Count > clientItems.Count)
                    {
                        churchItems.ForEachx(x =>
                        {
                            var check = clientItems.Find(i => i.CollectionTypeId == x.CollectionTypeId);
                            if (check == null)
                            {
                                newChurchCollectionTypes.Add(x);
                            }
                        });

                        #region Extract New Church Collection Type & Compose ClientChurchCollectionType in It
                        if (newChurchCollectionTypes.Any() || newChurchCollectionTypes.Count > 0)
                        {
                            if (myClientItem != null)
                            {
                                churchCollectionTypePercentages = myClientItem.CollectionTypes;
                            }

                            foreach (var newChurchCollectionType in newChurchCollectionTypes)
                            {
                                if (churchStructure != null && churchStructure.ChurchStructureTypeDetail.Any())
                                {
                                    #region Assign and Composing - Collection Type ChurchStructures Percent

                                    var churchStructureDetails = churchStructure.ChurchStructureTypeDetail;
                                    var thisCollectionChurchStructurePercentages =
                                        new List<ChurchCollectionChurchStructureTypeObj>();

                                    churchStructureDetails.ForEachx(c => thisCollectionChurchStructurePercentages.Add(new ChurchCollectionChurchStructureTypeObj
                                    {
                                        ChurchStructureTypeId = c.ChurchStructureTypeId,
                                        Name = c.ChurchStructureTypeName,
                                        Percent = 0.0
                                    }));
                                    #endregion

                                    churchCollectionTypePercentages.Add(new CollectionTypeObj
                                    {
                                        CollectionTypeId = newChurchCollectionType.CollectionTypeId,
                                        Name = newChurchCollectionType.Name,
                                        ChurchStructureTypeObjs = thisCollectionChurchStructurePercentages
                                    });
                                }
                                else
                                {
                                    churchCollectionTypePercentages.Add(new CollectionTypeObj
                                    {
                                        CollectionTypeId = newChurchCollectionType.CollectionTypeId,
                                        Name = newChurchCollectionType.Name,
                                    });
                                }
                            }


                            #region Update the New Client Church Collection Types to Database and Return

                            if (myClientItem != null)
                            {
                                myClientItem.CollectionTypes = churchCollectionTypePercentages;
                                var processedClientChurchCollectionType = _repository.Update(myClientItem);
                                _uoWork.SaveChanges();
                                if (processedClientChurchCollectionType.ClientChurchCollectionTypeId < 1)
                                {
                                    return new ClientChurchCollectionTypeSettingObjs();
                                }


                                // Return Client Church Collection Type Settings - With the Updated ClientItems
                                if (myClientItem.ClientChurchCollectionTypeId > 0)
                                {

                                    var clientChurchCollectionTypePercentages =
                                        myClientItem.CollectionTypes;

                                    foreach (var x in clientChurchCollectionTypePercentages)
                                    {
                                        var check = x.ChurchStructureTypeObjs;
                                        check.Where(i => i.Percent > 0).ToList().ForEachx(j => j.Percent = (100 * j.Percent));

                                        retItem.ClientChurchCollectionStructureTypes.Add(new CollectionTypeObj
                                        {
                                            CollectionTypeId = x.CollectionTypeId,
                                            Name = x.Name,
                                            ChurchStructureTypeObjs = check
                                        });
                                    }

                                    retItem.ClientChurchColletionTypeId = myClientItem.ClientChurchCollectionTypeId;
                                }
                            }

                            return retItem;
                            #endregion
                        }
                        #endregion

                    }
                }

                #endregion

                #region Return Original Object Get from ClientChurchCollectionType TABLE

                // Return Client Church Collection Type Settings
                if (myClientItem != null && myClientItem.ClientChurchCollectionTypeId > 0)
                {
                    var clientChurchCollectionTypePercentages =
                                        myClientItem.CollectionTypes;
                    foreach (var x in clientChurchCollectionTypePercentages)
                    {
                        var check = x.ChurchStructureTypeObjs;
                        check.Where(i => i.Percent > 0).ToList().ForEachx(j => j.Percent = (100 * j.Percent));

                        retItem.ClientChurchCollectionStructureTypes.Add(new CollectionTypeObj
                        {
                            CollectionTypeId = x.CollectionTypeId,
                            Name = x.Name,
                            ChurchStructureTypeObjs = check
                        });
                    }

                    retItem.ClientChurchColletionTypeId = myClientItem.ClientChurchCollectionTypeId;
                }

                return retItem;

                #endregion


            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        #endregion

    }
}
