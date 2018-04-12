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
    internal class StructureChurchRepository
    {

        private readonly IIcasRepository<StructureChurch> _repository;
        private readonly IIcasRepository<ChurchStructure> _churchStructurerepository;
        private readonly IIcasRepository<StateOfLocation> _stateOfLocationRepository;
        private readonly IIcasRepository<Region> _regionRepository;
        private readonly IIcasRepository<Province> _provinceRepository;
        private readonly IIcasRepository<Zone> _zoneRepository;
        private readonly IIcasRepository<Area> _areaRepository;
        private readonly IIcasRepository<Diocese> _dioceseRepository;
        private readonly IIcasRepository<District> _districtRepository;
        private readonly IIcasRepository<Group> _groupRepository;
        private readonly IIcasRepository<State> _stateRepository;

        private readonly IIcasRepository<Parish> _parishRepository;



        private readonly IIcasRepository<ChurchStructureType> _churchStructureTypeRepository;
        private readonly IcasUoWork _uoWork;

        public StructureChurchRepository()
        {
            _uoWork = new IcasUoWork();

            _repository = new IcasRepository<StructureChurch>(_uoWork);
            _churchStructurerepository = new IcasRepository<ChurchStructure>(_uoWork);
            _stateOfLocationRepository = new IcasRepository<StateOfLocation>(_uoWork);
            _regionRepository = new IcasRepository<Region>(_uoWork);
            _provinceRepository = new IcasRepository<Province>(_uoWork);
            _zoneRepository = new IcasRepository<Zone>(_uoWork);
            _areaRepository = new IcasRepository<Area>(_uoWork);
            _dioceseRepository = new IcasRepository<Diocese>(_uoWork);
            _districtRepository = new IcasRepository<District>(_uoWork);
            _groupRepository = new IcasRepository<Group>(_uoWork);
            _stateRepository = new IcasRepository<State>(_uoWork);
            _parishRepository = new IcasRepository<Parish>(_uoWork);
            _churchStructureTypeRepository = new IcasRepository<ChurchStructureType>(_uoWork);
        }

        #region Region
        internal StructureChurchRegResponse AddClientStructureChurch(ClientStructureChurchRegObj clientStructureChurchRegObj)
        {
            var response = new StructureChurchRegResponse
            {
                StructureId = 0,
                ClientId = clientStructureChurchRegObj.ClientId,
                ChurchId = clientStructureChurchRegObj.ChurchId,
                Name = clientStructureChurchRegObj.Name,
                Email = clientStructureChurchRegObj.Email,
                PhoneNumber = clientStructureChurchRegObj.PhoneNumber,
                StructureType = clientStructureChurchRegObj.StructureType,
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
                    response.Status.Message.TechnicalMessage = clientStructureChurchRegObj.StructureType + " Object is empty / invalid";
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
                if (IsDuplicate(clientStructureChurchRegObj.ChurchId,
                    clientStructureChurchRegObj.Name, out msg))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }
                
                #region Structure Region

                if (typeof(Region).Name == clientStructureChurchRegObj.Name)
                {
                    var clientStrucObj = new Region
                    {
                        //ChurchId = clientChurchStructureRegObj.ChurchId,
                        //ClientId = clientChurchStructureRegObj.ClientId,
                        //Name = clientChurchStructureRegObj.Name,
                        //PhoneNumber = clientChurchStructureRegObj.PhoneNumber,
                        //Email = clientChurchStructureRegObj.Email,
                        //Address = clientChurchStructureRegObj.Address,
                        StateOfLocationId = clientStructureChurchRegObj.StateOfLocationId,
                        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                        RegisteredByUserId = clientStructureChurchRegObj.RegisteredByUserId // Coming back to this
                    };

                    var processedClient = _regionRepository.Add(clientStrucObj);
                    _uoWork.SaveChanges();
                    if (processedClient.RegionId < 1)
                    {
                        //db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    response.StructureId = processedClient.RegionId;
                    response.Name = processedClient.Name;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;
                }

                #endregion

                #region Structure Province

                if (typeof(Province).Name == clientStructureChurchRegObj.Name)
                {
                    var clientStrucObj = new Province
                    {
                        ChurchId = clientStructureChurchRegObj.ChurchId,
                        //ClientId = clientChurchStructureRegObj.ClientId,
                        //Name = clientChurchStructureRegObj.Name,
                        //PhoneNumber = clientChurchStructureRegObj.PhoneNumber,
                        //Email = clientChurchStructureRegObj.Email,
                        //Address = clientChurchStructureRegObj.Address,
                        StateOfLocationId = clientStructureChurchRegObj.StateOfLocationId,
                        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                        RegisteredByUserId = clientStructureChurchRegObj.RegisteredByUserId // Coming back to this
                    };

                    var processedClient = _provinceRepository.Add(clientStrucObj);
                    _uoWork.SaveChanges();
                    if (processedClient.ProvinceId < 1)
                    {
                        //db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    response.StructureId = processedClient.ProvinceId;
                    response.Name = processedClient.Name;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;
                }

                #endregion

                #region Structure Zone

                if (typeof(Zone).Name == clientStructureChurchRegObj.Name)
                {
                    var clientStrucObj = new Zone
                    {
                        ChurchId = clientStructureChurchRegObj.ChurchId,
                        //ClientId = clientChurchStructureRegObj.ClientId,
                        //Name = clientChurchStructureRegObj.Name,
                        //PhoneNumber = clientChurchStructureRegObj.PhoneNumber,
                        //Email = clientChurchStructureRegObj.Email,
                        //Address = clientChurchStructureRegObj.Address,
                        //StateOfLocationId = clientChurchStructureRegObj.StateOfLocationId,
                        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                        RegisteredByUserId = clientStructureChurchRegObj.RegisteredByUserId // Coming back to this
                    };

                    var processedClient = _zoneRepository.Add(clientStrucObj);
                    _uoWork.SaveChanges();
                    if (processedClient.ZoneId < 1)
                    {
                        //db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    response.StructureId = processedClient.ZoneId;
                    response.Name = processedClient.Name;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;
                }

                #endregion

                #region Structure Area

                if (typeof(Area).Name == clientStructureChurchRegObj.Name)
                {
                    var clientStrucObj = new Area
                    {
                        ChurchId = clientStructureChurchRegObj.ChurchId,
                        //ClientId = clientChurchStructureRegObj.ClientId,
                        //Name = clientChurchStructureRegObj.Name,
                        //PhoneNumber = clientChurchStructureRegObj.PhoneNumber,
                        //Email = clientChurchStructureRegObj.Email,
                        //Address = clientChurchStructureRegObj.Address,
                        //StateOfLocationId = clientChurchStructureRegObj.StateOfLocationId,
                        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                        RegisteredByUserId = clientStructureChurchRegObj.RegisteredByUserId // Coming back to this
                    };

                    var processedClient = _areaRepository.Add(clientStrucObj);
                    _uoWork.SaveChanges();
                    if (processedClient.AreaId < 1)
                    {
                        //db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    response.StructureId = processedClient.AreaId;
                    response.Name = processedClient.Name;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;
                }

                #endregion

                #region Structure Diocese

                if (typeof(Diocese).Name == clientStructureChurchRegObj.Name)
                {
                    var clientStrucObj = new Diocese
                    {
                        ChurchId = clientStructureChurchRegObj.ChurchId,
                        //ClientId = clientChurchStructureRegObj.ClientId,
                        //Name = clientChurchStructureRegObj.Name,
                        //PhoneNumber = clientChurchStructureRegObj.PhoneNumber,
                        //Email = clientChurchStructureRegObj.Email,
                        //Address = clientChurchStructureRegObj.Address,
                        //StateOfLocationId = clientChurchStructureRegObj.StateOfLocationId,
                        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                        RegisteredByUserId = clientStructureChurchRegObj.RegisteredByUserId, // Coming back to this
                    };

                    var processedClient = _dioceseRepository.Add(clientStrucObj);
                    _uoWork.SaveChanges();
                    if (processedClient.DioceseId < 1)
                    {
                        //db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    response.StructureId = processedClient.DioceseId;
                    response.Name = processedClient.Name;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;
                }

                #endregion


                response.Status.Message.FriendlyMessage = "Unable to complete registration at this time! Please try again later";
                response.Status.Message.TechnicalMessage = "Unable to complete registration at this time! Please try again later";
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
            
        }


        internal StructureChurchRegResponse AddStructureChurch(StructureChurchRegObj structureChurchRegObj)
        {
            var response = new StructureChurchRegResponse
            {
                StructureId = 0,
                ChurchId = structureChurchRegObj.ChurchId,
                Name = structureChurchRegObj.Name,
                StructureTypeId = structureChurchRegObj.StructureTypeId,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {
                if (structureChurchRegObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = (ChurchPatternType)structureChurchRegObj.StructureTypeId + " Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(structureChurchRegObj, out valResults))
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

                //var structureName = ((ChurchPatternType) churchStructureRegObj.StructureTypeId).ToString();
                var structureTypeName =
                    _churchStructureTypeRepository.GetById(structureChurchRegObj.StructureTypeId).Name;

                string msg;
                if (IsStructureDuplicate(structureTypeName, structureChurchRegObj.ChurchId,
                    structureChurchRegObj.Name, out msg))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }

                

                #region Structure Parish

                if (typeof(Parish).Name == structureTypeName)
                {
                    var churchStrucObj = new Parish
                    {
                        ChurchId = structureChurchRegObj.ChurchId,
                        Name = structureChurchRegObj.Name,
                        StateOfLocationId = structureChurchRegObj.StateOfLocationId,
                        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                        RegisteredByUserId = structureChurchRegObj.RegisteredByUserId // Coming back to this
                        
                    };

                    var processedClient = _parishRepository.Add(churchStrucObj);
                    _uoWork.SaveChanges();
                    if (processedClient.ParishId < 1)
                    {
                        //db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    response.StructureId = processedClient.ParishId;
                    response.Name = structureTypeName;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;
                }

                #endregion

                #region Structure Region

                if (typeof(Region).Name == structureTypeName)
                {
                    var churchStrucObj = new Region
                    {
                        ChurchId = structureChurchRegObj.ChurchId,
                        Name = structureChurchRegObj.Name,
                        StateOfLocationId = structureChurchRegObj.StateOfLocationId,
                        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                        RegisteredByUserId = structureChurchRegObj.RegisteredByUserId
                    };

                    var processedClient = _regionRepository.Add(churchStrucObj);
                    _uoWork.SaveChanges();
                    if (processedClient.RegionId < 1)
                    {
                        //db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    response.StructureId = processedClient.RegionId;
                    response.Name = structureTypeName;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;
                }

                #endregion

                #region Structure Province

                if (typeof(Province).Name == structureTypeName)
                {
                    var churchStrucObj = new Province
                    {
                        ChurchId = structureChurchRegObj.ChurchId,
                        Name = structureChurchRegObj.Name,
                        StateOfLocationId = structureChurchRegObj.StateOfLocationId,
                        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                        RegisteredByUserId = structureChurchRegObj.RegisteredByUserId
                    };

                    var processedClient = _provinceRepository.Add(churchStrucObj);
                    _uoWork.SaveChanges();
                    if (processedClient.ProvinceId < 1)
                    {
                        //db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    response.StructureId = processedClient.ProvinceId;
                    response.Name = structureTypeName;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;
                }

                #endregion

                #region Structure Zone

                if (typeof(Zone).Name == structureTypeName)
                {
                    var churchStrucObj = new Zone
                    {
                        ChurchId = structureChurchRegObj.ChurchId,
                        Name = structureChurchRegObj.Name,
                        StateOfLocationId = structureChurchRegObj.StateOfLocationId,
                        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                        RegisteredByUserId = structureChurchRegObj.RegisteredByUserId
                    };

                    var processedClient = _zoneRepository.Add(churchStrucObj);
                    _uoWork.SaveChanges();
                    if (processedClient.ZoneId < 1)
                    {
                        //db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    response.StructureId = processedClient.ZoneId;
                    response.Name = structureTypeName;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;
                }

                #endregion

                #region Structure Area

                if (typeof(Area).Name == structureTypeName)
                {
                    var churchStrucObj = new Area
                    {
                        ChurchId = structureChurchRegObj.ChurchId,
                        Name = structureChurchRegObj.Name,
                        StateOfLocationId = structureChurchRegObj.StateOfLocationId,
                        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                        RegisteredByUserId = structureChurchRegObj.RegisteredByUserId
                    };

                    var processedClient = _areaRepository.Add(churchStrucObj);
                    _uoWork.SaveChanges();
                    if (processedClient.AreaId < 1)
                    {
                        //db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    response.StructureId = processedClient.AreaId;
                    response.Name = structureTypeName;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;
                }

                #endregion

                #region Structure Diocese

                if (typeof(Diocese).Name == structureTypeName)
                {
                    var churchStrucObj = new Diocese
                    {
                        ChurchId = structureChurchRegObj.ChurchId,
                        Name = structureChurchRegObj.Name,
                        StateOfLocationId = structureChurchRegObj.StateOfLocationId,
                        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                        RegisteredByUserId = structureChurchRegObj.RegisteredByUserId
                    };

                    var processedClient = _dioceseRepository.Add(churchStrucObj);
                    _uoWork.SaveChanges();
                    if (processedClient.DioceseId < 1)
                    {
                        //db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    response.StructureId = processedClient.DioceseId;
                    response.Name = structureTypeName;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;
                }

                #endregion

                #region Structure District

                if (typeof(District).Name == structureTypeName)
                {
                    var churchStrucObj = new District
                    {
                        ChurchId = structureChurchRegObj.ChurchId,
                        Name = structureChurchRegObj.Name,
                        StateOfLocationId = structureChurchRegObj.StateOfLocationId,
                        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                        RegisteredByUserId = structureChurchRegObj.RegisteredByUserId
                    };

                    var processedClient = _districtRepository.Add(churchStrucObj);
                    _uoWork.SaveChanges();
                    if (processedClient.DistrictId < 1)
                    {
                        //db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    response.StructureId = processedClient.DistrictId;
                    response.Name = structureTypeName;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;
                }

                #endregion

                #region Structure State

                if (typeof(State).Name == structureTypeName)
                {
                    var churchStrucObj = new State
                    {
                        ChurchId = structureChurchRegObj.ChurchId,
                        Name = structureChurchRegObj.Name,
                        StateOfLocationId = structureChurchRegObj.StateOfLocationId,
                        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                        RegisteredByUserId = structureChurchRegObj.RegisteredByUserId
                    };

                    var processedClient = _stateRepository.Add(churchStrucObj);
                    _uoWork.SaveChanges();
                    if (processedClient.StateId < 1)
                    {
                        //db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    response.StructureId = processedClient.StateId;
                    response.Name = structureTypeName;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;
                }

                #endregion

                #region Structure Group

                if (typeof(Group).Name == structureTypeName)
                {
                    var churchStrucObj = new Group
                    {
                        ChurchId = structureChurchRegObj.ChurchId,
                        Name = structureChurchRegObj.Name,
                        StateOfLocationId = structureChurchRegObj.StateOfLocationId,
                        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                        RegisteredByUserId = structureChurchRegObj.RegisteredByUserId
                    };

                    var processedClient = _groupRepository.Add(churchStrucObj);
                    _uoWork.SaveChanges();
                    if (processedClient.GroupId < 1)
                    {
                        //db.Rollback();
                        response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                        response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    response.StructureId = processedClient.GroupId;
                    response.Name = structureTypeName;
                    response.Status.Message.FriendlyMessage = "Registration Complete Succesfully!";
                    response.Status.IsSuccessful = true;
                    return response;
                }

                #endregion


                response.Status.Message.FriendlyMessage = "Unable to complete registration at this time! Please try again later";
                response.Status.Message.TechnicalMessage = "Unable to complete registration at this time! Please try again later";
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

        }


        internal RespStatus UpdateStructureChurch(StructureChurchRegObj structureChurchRegObj)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };
            try
            {
                if (structureChurchRegObj.Equals(null))
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your registration";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(structureChurchRegObj, out valResults))
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

                string msg;
                using (var db = _uoWork.BeginTransaction())
                {
                    try
                    {

                        var typeName = ServiceChurch.GetChurchStructureTypeNameById(structureChurchRegObj.StructureTypeId);
                        switch (typeName)
                        {
                            
                            #region Region
                            case "Region":

                                var thisRegionChurch = _regionRepository.GetById(structureChurchRegObj.TypeStructureChurchId);
                                if (thisRegionChurch == null || thisRegionChurch.RegionId < 1)
                                {
                                    db.Rollback();
                                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! No Structure Record Found";
                                    return response;
                                }
                                thisRegionChurch.ChurchId = structureChurchRegObj.ChurchId;
                                thisRegionChurch.StateOfLocationId = structureChurchRegObj.StateOfLocationId;
                                thisRegionChurch.Name = structureChurchRegObj.Name;

                                var processedRegion = _regionRepository.Update(thisRegionChurch);
                                _uoWork.SaveChanges();
                                if (processedRegion.RegionId < 1)
                                {
                                    db.Rollback();
                                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                                    return response;
                                }
                                break;
                            #endregion

                            #region Province
                            case "Province":
                                var thisProvinceChurch = _provinceRepository.GetById(structureChurchRegObj.TypeStructureChurchId);
                                if (thisProvinceChurch == null || thisProvinceChurch.ProvinceId < 1)
                                {
                                    db.Rollback();
                                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! No Structure Record Found";
                                    return response;
                                }
                                thisProvinceChurch.ChurchId = structureChurchRegObj.ChurchId;
                                thisProvinceChurch.StateOfLocationId = structureChurchRegObj.StateOfLocationId;
                                thisProvinceChurch.Name = structureChurchRegObj.Name;

                                var processedProvince = _provinceRepository.Update(thisProvinceChurch);
                                _uoWork.SaveChanges();
                                if (processedProvince.ProvinceId < 1)
                                {
                                    db.Rollback();
                                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                                    return response;
                                }
                                break;
                            #endregion

                            #region Zone
                            case "Zone":
                                var thisZoneChurch = _zoneRepository.GetById(structureChurchRegObj.TypeStructureChurchId);
                                if (thisZoneChurch == null || thisZoneChurch.ZoneId < 1)
                                {
                                    db.Rollback();
                                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! No Structure Record Found";
                                    return response;
                                }
                                thisZoneChurch.ChurchId = structureChurchRegObj.ChurchId;
                                thisZoneChurch.StateOfLocationId = structureChurchRegObj.StateOfLocationId;
                                thisZoneChurch.Name = structureChurchRegObj.Name;

                                var processedZone = _zoneRepository.Update(thisZoneChurch);
                                _uoWork.SaveChanges();
                                if (processedZone.ZoneId < 1)
                                {
                                    db.Rollback();
                                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                                    return response;
                                }
                                break;
                            #endregion

                            #region Area
                            case "Area":
                                var thisAreaChurch = _areaRepository.GetById(structureChurchRegObj.TypeStructureChurchId);
                                if (thisAreaChurch == null || thisAreaChurch.AreaId < 1)
                                {
                                    db.Rollback();
                                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! No Structure Record Found";
                                    return response;
                                }
                                thisAreaChurch.ChurchId = structureChurchRegObj.ChurchId;
                                thisAreaChurch.StateOfLocationId = structureChurchRegObj.StateOfLocationId;
                                thisAreaChurch.Name = structureChurchRegObj.Name;

                                var processedArea = _areaRepository.Update(thisAreaChurch);
                                _uoWork.SaveChanges();
                                if (processedArea.AreaId < 1)
                                {
                                    db.Rollback();
                                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                                    return response;
                                }
                                break;
                            #endregion

                            #region Diocese
                            case "Diocese":
                                var thisDioceseChurch = _dioceseRepository.GetById(structureChurchRegObj.TypeStructureChurchId);
                                if (thisDioceseChurch == null || thisDioceseChurch.DioceseId < 1)
                                {
                                    db.Rollback();
                                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! No Structure Record Found";
                                    return response;
                                }
                                thisDioceseChurch.ChurchId = structureChurchRegObj.ChurchId;
                                thisDioceseChurch.StateOfLocationId = structureChurchRegObj.StateOfLocationId;
                                thisDioceseChurch.Name = structureChurchRegObj.Name;

                                var processedDiocese = _dioceseRepository.Update(thisDioceseChurch);
                                _uoWork.SaveChanges();
                                if (processedDiocese.DioceseId < 1)
                                {
                                    db.Rollback();
                                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                                    return response;
                                }
                                break;
                            #endregion

                            #region District
                            case "District":
                                var thisDistrictChurch = _districtRepository.GetById(structureChurchRegObj.TypeStructureChurchId);
                                if (thisDistrictChurch == null || thisDistrictChurch.DistrictId < 1)
                                {
                                    db.Rollback();
                                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! No Structure Record Found";
                                    return response;
                                }
                                thisDistrictChurch.ChurchId = structureChurchRegObj.ChurchId;
                                thisDistrictChurch.StateOfLocationId = structureChurchRegObj.StateOfLocationId;
                                thisDistrictChurch.Name = structureChurchRegObj.Name;

                                var processedDistrict = _districtRepository.Update(thisDistrictChurch);
                                _uoWork.SaveChanges();
                                if (processedDistrict.DistrictId < 1)
                                {
                                    db.Rollback();
                                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                                    return response;
                                }
                                break;
                            #endregion

                            #region State
                            case "State":
                                var thisStateChurch = _stateRepository.GetById(structureChurchRegObj.TypeStructureChurchId);
                                if (thisStateChurch == null || thisStateChurch.StateId < 1)
                                {
                                    db.Rollback();
                                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! No Structure Record Found";
                                    return response;
                                }
                                thisStateChurch.ChurchId = structureChurchRegObj.ChurchId;
                                thisStateChurch.StateOfLocationId = structureChurchRegObj.StateOfLocationId;
                                thisStateChurch.Name = structureChurchRegObj.Name;

                                var processedState = _stateRepository.Update(thisStateChurch);
                                _uoWork.SaveChanges();
                                if (processedState.StateId < 1)
                                {
                                    db.Rollback();
                                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                                    return response;
                                }
                                break;
                            #endregion

                            #region Group
                            case "Group":
                                var thisGroupChurch = _groupRepository.GetById(structureChurchRegObj.TypeStructureChurchId);
                                if (thisGroupChurch == null || thisGroupChurch.GroupId < 1)
                                {
                                    db.Rollback();
                                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! No Structure Record Found";
                                    return response;
                                }
                                thisGroupChurch.ChurchId = structureChurchRegObj.ChurchId;
                                thisGroupChurch.StateOfLocationId = structureChurchRegObj.StateOfLocationId;
                                thisGroupChurch.Name = structureChurchRegObj.Name;

                                var processedGroup = _groupRepository.Update(thisGroupChurch);
                                _uoWork.SaveChanges();
                                if (processedGroup.GroupId < 1)
                                {
                                    db.Rollback();
                                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                                    return response;
                                }
                                break;
                            #endregion
                            
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


        private StructureChurchMain GetStructureChurch(StructureChurchRegObj structureChurchRegObj, out string msg)
        {
            try
            {
                var typeName = ServiceChurch.GetChurchStructureTypeNameById(structureChurchRegObj.StructureTypeId);
                var typeChurchId = structureChurchRegObj.TypeStructureChurchId;

                switch (typeName)
                {
                    case "Region":

                        #region Region
                        var myRegion = _regionRepository.GetById(typeChurchId);
                        //var retRegion = new StructureChurchMain();
                        if (myRegion == null || myRegion.RegionId < 1)
                        {
                            msg = "No Structure Record Found";
                            return null;
                        }

                        msg = "";
                        return new StructureChurchMain
                        {
                            StructureChurchMainId = myRegion.RegionId,
                            StateOfLocationId = myRegion.StateOfLocationId,
                            ChurchId = myRegion.ChurchId,
                            Name = myRegion.Name,
                            RegisteredByUserId = myRegion.RegisteredByUserId,
                            TimeStampRegistered = myRegion.TimeStampRegistered
                        };
                        #endregion
                        break;

                    case "Province":

                        #region Province
                        var myProvince = _provinceRepository.GetById(typeChurchId);
                        if (myProvince == null || myProvince.ProvinceId < 1)
                        {
                            msg = "No Structure Record Found";
                            return null;
                        }

                        msg = "";
                        return new StructureChurchMain
                        {
                            StructureChurchMainId = myProvince.ProvinceId,
                            StateOfLocationId = myProvince.StateOfLocationId,
                            ChurchId = myProvince.ChurchId,
                            Name = myProvince.Name,
                            RegisteredByUserId = myProvince.RegisteredByUserId,
                            TimeStampRegistered = myProvince.TimeStampRegistered
                        };
                        #endregion
                        break;

                    case "Zone":

                        #region Zone
                        var myZone = _zoneRepository.GetById(typeChurchId);
                        if (myZone == null || myZone.ZoneId < 1)
                        {
                            msg = "No Structure Record Found";
                            return null;
                        }

                        msg = "";
                        return new StructureChurchMain
                        {
                            StructureChurchMainId = myZone.ZoneId,
                            StateOfLocationId = myZone.StateOfLocationId,
                            ChurchId = myZone.ChurchId,
                            Name = myZone.Name,
                            RegisteredByUserId = myZone.RegisteredByUserId,
                            TimeStampRegistered = myZone.TimeStampRegistered
                        };
                        #endregion
                        break;

                    case "Area":

                        #region Area
                        var myArea = _areaRepository.GetById(typeChurchId);
                        if (myArea == null || myArea.AreaId < 1)
                        {
                            msg = "No Structure Record Found";
                            return null;
                        }

                        msg = "";
                        return new StructureChurchMain
                        {
                            StructureChurchMainId = myArea.AreaId,
                            StateOfLocationId = myArea.StateOfLocationId,
                            ChurchId = myArea.ChurchId,
                            Name = myArea.Name,
                            RegisteredByUserId = myArea.RegisteredByUserId,
                            TimeStampRegistered = myArea.TimeStampRegistered
                        };
                        #endregion
                        break;

                    case "Diocese":

                        #region Diocese
                        var myDiocese = _dioceseRepository.GetById(typeChurchId);
                        if (myDiocese == null || myDiocese.DioceseId < 1)
                        {
                            msg = "No Structure Record Found";
                            return null;
                        }

                        msg = "";
                        return new StructureChurchMain
                        {
                            StructureChurchMainId = myDiocese.DioceseId,
                            StateOfLocationId = myDiocese.StateOfLocationId,
                            ChurchId = myDiocese.ChurchId,
                            Name = myDiocese.Name,
                            RegisteredByUserId = myDiocese.RegisteredByUserId,
                            TimeStampRegistered = myDiocese.TimeStampRegistered
                        };
                        #endregion
                        break;

                    case "District":

                        #region District
                        var myDistrict = _districtRepository.GetById(typeChurchId);
                        if (myDistrict == null || myDistrict.DistrictId < 1)
                        {
                            msg = "No Structure Record Found";
                            return null;
                        }

                        msg = "";
                        return new StructureChurchMain
                        {
                            StructureChurchMainId = myDistrict.DistrictId,
                            StateOfLocationId = myDistrict.StateOfLocationId,
                            ChurchId = myDistrict.ChurchId,
                            Name = myDistrict.Name,
                            RegisteredByUserId = myDistrict.RegisteredByUserId,
                            TimeStampRegistered = myDistrict.TimeStampRegistered
                        };
                        #endregion
                        break;

                    case "State":

                        #region State
                        var myState = _stateRepository.GetById(typeChurchId);
                        if (myState == null || myState.StateId < 1)
                        {
                            msg = "No Structure Record Found";
                            return null;
                        }

                        msg = "";
                        return new StructureChurchMain
                        {
                            StructureChurchMainId = myState.StateId,
                            StateOfLocationId = myState.StateOfLocationId,
                            ChurchId = myState.ChurchId,
                            Name = myState.Name,
                            RegisteredByUserId = myState.RegisteredByUserId,
                            TimeStampRegistered = myState.TimeStampRegistered
                        };
                        #endregion
                        break;

                    case "Group":

                        #region Group
                        var myGroup = _groupRepository.GetById(typeChurchId);
                        if (myGroup == null || myGroup.GroupId < 1)
                        {
                            msg = "No Structure Record Found";
                            return null;
                        }

                        msg = "";
                        return new StructureChurchMain
                        {
                            StructureChurchMainId = myGroup.GroupId,
                            StateOfLocationId = myGroup.StateOfLocationId,
                            ChurchId = myGroup.ChurchId,
                            Name = myGroup.Name,
                            RegisteredByUserId = myGroup.RegisteredByUserId,
                            TimeStampRegistered = myGroup.TimeStampRegistered
                        };
                        #endregion
                        break;
                }

                msg = "Unable to complete your request ";
                return null;
               
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        

        internal List<RegisteredStructureChurchReportObj> GetAllRegisteredStructureChurchObjs()
        {
            try
            {
                var myItemList = _churchStructurerepository.GetAll().ToList();
                if (!myItemList.Any()) return new List<RegisteredStructureChurchReportObj>(); 

                var retList = new List<RegisteredStructureChurchReportObj>();
                myItemList.ForEachx(m =>
                {
                    var parentChurch = new ChurchRepository().GetChurch(m.ChurchId);
                    //var structureName = ServiceChurch.GetChurchStructureTypeNameById(m.ChurchStructureTypeId);
                    var structureName = ServiceChurch.GetChurchStructureTypeNameById(1);



                    switch (structureName)
                    {
                        case "Region":
                            
                            #region Region
                            var myRegionList = _regionRepository.GetAll(x => x.ChurchId == m.ChurchId).ToList();
                            myRegionList.ForEachx(x => retList.Add(new RegisteredStructureChurchReportObj
                            {
                                TypeStructureChurchId = x.RegionId,
                                StructureChurchName = x.Name,
                                StateOfLocationId = x.StateOfLocationId,
                                StateOfLocationName = _stateOfLocationRepository.GetById(x.StateOfLocationId).Name,
                                ChurchId = m.ChurchId,
                                //ChurchStructureTypeId = m.ChurchStructureTypeId,
                                ParentChurchName = parentChurch.Name,
                                ChurchStructureTypeName = structureName,
                            }));
                            #endregion
                            break;

                        case "Province":

                            #region Province
                            var myProvinceList = _provinceRepository.GetAll(x => x.ChurchId == m.ChurchId).ToList();
                            myProvinceList.ForEachx(x => retList.Add(new RegisteredStructureChurchReportObj
                            {
                                TypeStructureChurchId = x.ProvinceId,
                                StructureChurchName = x.Name,
                                StateOfLocationId = x.StateOfLocationId,
                                StateOfLocationName = _stateOfLocationRepository.GetById(x.StateOfLocationId).Name,
                                ChurchId = m.ChurchId,
                               // ChurchStructureTypeId = m.ChurchStructureTypeId,
                                ParentChurchName = parentChurch.Name,
                                ChurchStructureTypeName = structureName,
                            }));
                            #endregion

                            break;
                        case "Zone":

                            #region Zone
                            var myZoneList = _zoneRepository.GetAll(x => x.ChurchId == m.ChurchId).ToList();
                            myZoneList.ForEachx(x => retList.Add(new RegisteredStructureChurchReportObj
                            {
                                TypeStructureChurchId = x.ZoneId,
                                StructureChurchName = x.Name,
                                StateOfLocationId = x.StateOfLocationId,
                                StateOfLocationName = _stateOfLocationRepository.GetById(x.StateOfLocationId).Name,
                                ChurchId = m.ChurchId,
                                //ChurchStructureTypeId = m.ChurchStructureTypeId,
                                ParentChurchName = parentChurch.Name,
                                ChurchStructureTypeName = structureName,
                            }));
                            #endregion

                            break;
                        case "Area":

                            #region Area
                            var myAreaList = _areaRepository.GetAll(x => x.ChurchId == m.ChurchId).ToList();
                            myAreaList.ForEachx(x => retList.Add(new RegisteredStructureChurchReportObj
                            {
                                TypeStructureChurchId = x.AreaId,
                                StructureChurchName = x.Name,
                                StateOfLocationId = x.StateOfLocationId,
                                StateOfLocationName = _stateOfLocationRepository.GetById(x.StateOfLocationId).Name,
                                ChurchId = m.ChurchId,
                                //ChurchStructureTypeId = m.ChurchStructureTypeId,
                                ParentChurchName = parentChurch.Name,
                                ChurchStructureTypeName = structureName,
                            }));
                            #endregion

                            break;
                        case "Diocese":

                            #region Diocese
                            var myDioceseList = _dioceseRepository.GetAll(x => x.ChurchId == m.ChurchId).ToList();
                            myDioceseList.ForEachx(x => retList.Add(new RegisteredStructureChurchReportObj
                            {
                                TypeStructureChurchId = x.DioceseId,
                                StructureChurchName = x.Name,
                                StateOfLocationId = x.StateOfLocationId,
                                StateOfLocationName = _stateOfLocationRepository.GetById(x.StateOfLocationId).Name,
                                ChurchId = m.ChurchId,
                                //ChurchStructureTypeId = m.ChurchStructureTypeId,
                                ParentChurchName = parentChurch.Name,
                                ChurchStructureTypeName = structureName,
                            }));
                            #endregion

                            break;
                        case "District":

                            #region District
                            var myDistrictList = _districtRepository.GetAll(x => x.ChurchId == m.ChurchId).ToList();
                            myDistrictList.ForEachx(x => retList.Add(new RegisteredStructureChurchReportObj
                            {
                                TypeStructureChurchId = x.DistrictId,
                                StructureChurchName = x.Name,
                                StateOfLocationId = x.StateOfLocationId,
                                StateOfLocationName = _stateOfLocationRepository.GetById(x.StateOfLocationId).Name,
                                ChurchId = m.ChurchId,
                                //ChurchStructureTypeId = m.ChurchStructureTypeId,
                                ParentChurchName = parentChurch.Name,
                                ChurchStructureTypeName = structureName,
                            }));
                            #endregion

                            break;
                        case "State":

                            #region State
                            var myStateList = _stateRepository.GetAll(x => x.ChurchId == m.ChurchId).ToList();
                            myStateList.ForEachx(x => retList.Add(new RegisteredStructureChurchReportObj
                            {
                                TypeStructureChurchId = x.StateId,
                                StructureChurchName = x.Name,
                                StateOfLocationId = x.StateOfLocationId,
                                StateOfLocationName = _stateOfLocationRepository.GetById(x.StateOfLocationId).Name,
                                ChurchId = m.ChurchId,
                                //ChurchStructureTypeId = m.ChurchStructureTypeId,
                                ParentChurchName = parentChurch.Name,
                                ChurchStructureTypeName = structureName,
                            }));
                            #endregion

                            break;
                        case "Group":

                            #region Group
                            var myGroupList = _groupRepository.GetAll(x => x.ChurchId == m.ChurchId).ToList();
                            myGroupList.ForEachx(x => retList.Add(new RegisteredStructureChurchReportObj
                            {
                                TypeStructureChurchId = x.GroupId,
                                StructureChurchName = x.Name,
                                StateOfLocationId = x.StateOfLocationId,
                                StateOfLocationName = _stateOfLocationRepository.GetById(x.StateOfLocationId).Name,
                                ChurchId = m.ChurchId,
                                //ChurchStructureTypeId = m.ChurchStructureTypeId,
                                ParentChurchName = parentChurch.Name,
                                ChurchStructureTypeName = structureName,
                            }));
                            #endregion

                            break;
                    }
                });

                var count = 1;
                foreach (var registeredStructureChurchReportObj in retList)
                {
                    registeredStructureChurchReportObj.StructureChurchId = count;
                    count++;
                }

                return retList.ToList();
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }



        internal List<ChurchStructureType> GetChurchStructureTypeByTypeStructureChurchId(long typeStructureChurchId)
        {
            try
            {
                if (typeStructureChurchId < 1)
                {
                    return new List<ChurchStructureType>();
                }

                // Get the Structure Church record for the selected TypeStructureChurchId
                var structureChurch = _repository.GetById(typeStructureChurchId);
                if (structureChurch.ChurchStructureTypeId < 1) { return new List<ChurchStructureType>(); }

                // Get the Church Structure Type
                var structureType = _churchStructureTypeRepository.GetById(structureChurch.ChurchStructureTypeId);
                if (structureType.ChurchStructureTypeId < 1) { return new List<ChurchStructureType>(); }


                return new List<ChurchStructureType>()
                {
                    new ChurchStructureType
                    {
                        ChurchStructureTypeId = structureType.ChurchStructureTypeId,
                        Name = structureType.Name,
                    }
                };
                
            }
            catch (Exception ex)
            {
                return new List<ChurchStructureType>();
            }
        }





        internal bool IsStructureDuplicate(string typeName, long churchId, string name, out string msg)
        {
            try
            {

                if (!string.IsNullOrEmpty(name) && churchId > 0)
                {

                    #region Parish
                    if (typeof(Parish).Name == typeName)
                    {
                        var sql = string.Format("Select * FROM \"ChurchAPPDB\".\"Parish\" ");
                        var thisChurchStructures = _parishRepository.RepositoryContext().Database.SqlQuery<Parish>(sql).ToList();
                        var parish = thisChurchStructures.FindAll(x => x.ChurchId == churchId && string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

                        msg = "";
                        if (parish.IsNullOrEmpty()) return false;
                        if (parish.Any())
                        {
                            if (parish.Count > 0)
                            {
                                msg = "Parish name already exist!";
                                return true;
                            }
                        }

                    }
                    #endregion
                    
                    #region Region
                    if (typeof(Region).Name == typeName)
                    {
                        var sql = string.Format("Select * FROM \"ChurchAPPDB\".\"Region\" ");
                        var thisChurchStructures = _regionRepository.RepositoryContext().Database.SqlQuery<Region>(sql).ToList();
                        var region = thisChurchStructures.FindAll(x => x.ChurchId == churchId && string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

                        msg = "";
                        if (region.IsNullOrEmpty()) return false;
                        if (region.Any())
                        {
                            if (region.Count > 0)
                            {
                                msg = "Region name already exist";
                                return true;
                            }
                        }

                    }
                    #endregion

                    #region Province
                    if (typeof(Province).Name == typeName)
                    {
                        var sql = string.Format("Select * FROM \"ChurchAPPDB\".\"Province\" ");
                        var thisChurchStructures = _provinceRepository.RepositoryContext().Database.SqlQuery<Province>(sql).ToList();
                        var province = thisChurchStructures.FindAll(x => x.ChurchId == churchId && string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

                        msg = "";
                        if (province.IsNullOrEmpty()) return false;
                        if (province.Any())
                        {
                            if (province.Count > 0)
                            {
                                msg = "Province name already exist";
                                return true;
                            }
                        }

                    }
                    #endregion

                    #region Zone
                    if (typeof(Zone).Name == typeName)
                    {
                        var sql = string.Format("Select * FROM \"ChurchAPPDB\".\"Zone\" ");
                        var thisChurchStructures = _zoneRepository.RepositoryContext().Database.SqlQuery<Zone>(sql).ToList();
                        var zone = thisChurchStructures.FindAll(x => x.ChurchId == churchId && string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

                        msg = "";
                        if (zone.IsNullOrEmpty()) return false;
                        if (zone.Any())
                        {
                            if (zone.Count > 0)
                            {
                                msg = "Zone name already exist";
                                return true;
                            }
                        }

                    }
                    #endregion

                    #region Area
                    if (typeof(Area).Name == typeName)
                    {
                        var sql = string.Format("Select * FROM \"ChurchAPPDB\".\"Area\" ");
                        var thisChurchStructures = _areaRepository.RepositoryContext().Database.SqlQuery<Area>(sql).ToList();
                        var area = thisChurchStructures.FindAll(x => x.ChurchId == churchId && string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

                        msg = "";
                        if (area.IsNullOrEmpty()) return false;
                        if (area.Any())
                        {
                            if (area.Count > 0)
                            {
                                msg = "Area name already exist";
                                return true;
                            }
                        }

                    }
                    #endregion

                    #region Diocese
                    if (typeof(Diocese).Name == typeName)
                    {
                        var sql = string.Format("Select * FROM \"ChurchAPPDB\".\"Diocese\" ");
                        var thisChurchStructures = _dioceseRepository.RepositoryContext().Database.SqlQuery<Diocese>(sql).ToList();
                        var diocese = thisChurchStructures.FindAll(x => x.ChurchId == churchId && string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

                        msg = "";
                        if (diocese.IsNullOrEmpty()) return false;
                        if (diocese.Any())
                        {
                            if (diocese.Count > 0)
                            {
                                msg = "Diocese name already exist";
                                return true;
                            }
                        }

                    }
                    #endregion

                    #region District
                    if (typeof(District).Name == typeName)
                    {
                        var sql = string.Format("Select * FROM \"ChurchAPPDB\".\"District\" ");
                        var thisChurchStructures = _zoneRepository.RepositoryContext().Database.SqlQuery<District>(sql).ToList();
                        var district = thisChurchStructures.FindAll(x => x.ChurchId == churchId && string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

                        msg = "";
                        if (district.IsNullOrEmpty()) return false;
                        if (district.Any())
                        {
                            if (district.Count > 0)
                            {
                                msg = "District name already exist";
                                return true;
                            }
                        }

                    }
                    #endregion

                    #region State
                    if (typeof(State).Name == typeName)
                    {
                        var sql = string.Format("Select * FROM \"ChurchAPPDB\".\"State\" ");
                        var thisChurchStructures = _areaRepository.RepositoryContext().Database.SqlQuery<State>(sql).ToList();
                        var state = thisChurchStructures.FindAll(x => x.ChurchId == churchId && string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

                        msg = "";
                        if (state.IsNullOrEmpty()) return false;
                        if (state.Any())
                        {
                            if (state.Count > 0)
                            {
                                msg = "State name already exist";
                                return true;
                            }
                        }

                    }
                    #endregion

                    #region Group
                    if (typeof(Group).Name == typeName)
                    {
                        var sql = string.Format("Select * FROM \"ChurchAPPDB\".\"Group\" ");
                        var thisChurchStructures = _dioceseRepository.RepositoryContext().Database.SqlQuery<Group>(sql).ToList();
                        var group = thisChurchStructures.FindAll(x => x.ChurchId == churchId && string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

                        msg = "";
                        if (group.IsNullOrEmpty()) return false;
                        if (group.Any())
                        {
                            if (group.Count > 0)
                            {
                                msg = "Group name already exist";
                                return true;
                            }
                        }

                    }
                    #endregion


                }
                else
                {
                    msg = "";
                    return false;
                }

                msg = "Unable to check duplicate! Please try again later";
                return false;

            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return true;
            }
        }


        internal bool IsDuplicate(long churchId, string name, out string msg) 
        {
            try
            {

                if (!string.IsNullOrEmpty(name) && churchId > 0)
                {
                    
                    #region Region
                    //if (typeof (Region).Name == typeof (T).Name)
                    //{
                    //    var sql = string.Format("Select * FROM \"ChurchApp\".\"Region\" ");
                    //    var thisChurchStructures = _repository.RepositoryContext().Database.SqlQuery<Region>(sql).ToList();
                    //    var region = thisChurchStructures.FindAll(x => x.ChurchId == churchId &&  string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase) == 0 );

                    //    msg = "";
                    //    if(region.IsNullOrEmpty()) return false;
                    //    if (region.Any())
                    //    {
                    //        if (region.Count > 0)
                    //        {
                    //            msg = "Region name already been used by another client/church";
                    //            return true;
                    //        }
                    //    }
                        
                    //}
                    #endregion

                    #region Province
                    //if (typeof(Province).Name == typeof(T).Name)
                    //{
                    //    var sql = string.Format("Select * FROM \"ChurchApp\".\"Province\" ");
                    //    var thisChurchStructures = _repository.RepositoryContext().Database.SqlQuery<Province>(sql).ToList();
                    //    var province = thisChurchStructures.FindAll(x => x.ChurchId == churchId && string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

                    //    msg = "";
                    //    if (province.IsNullOrEmpty()) return false;
                    //    if (province.Any())
                    //    {
                    //        if (province.Count > 0)
                    //        {
                    //            msg = "Province name already been used by another client/church";
                    //            return true;
                    //        }
                    //    }

                    //}
                    #endregion

                    #region Zone
                    //if (typeof(Zone).Name == typeof(T).Name)
                    //{
                    //    var sql = string.Format("Select * FROM \"ChurchApp\".\"Zone\" ");
                    //    var thisChurchStructures = _repository.RepositoryContext().Database.SqlQuery<Zone>(sql).ToList();
                    //    var zone = thisChurchStructures.FindAll(x => x.ChurchId == churchId && string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

                    //    msg = "";
                    //    if (zone.IsNullOrEmpty()) return false;
                    //    if (zone.Any())
                    //    {
                    //        if (zone.Count > 0)
                    //        {
                    //            msg = "Zone name already been used by another client/church";
                    //            return true;
                    //        }
                    //    }

                    //}
                    #endregion

                    #region Area
                    //if (typeof(Area).Name == typeof(T).Name)
                    //{
                    //    var sql = string.Format("Select * FROM \"ChurchApp\".\"Area\" ");
                    //    var thisChurchStructures = _repository.RepositoryContext().Database.SqlQuery<Area>(sql).ToList();
                    //    var area = thisChurchStructures.FindAll(x => x.ChurchId == churchId && string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

                    //    msg = "";
                    //    if (area.IsNullOrEmpty()) return false;
                    //    if (area.Any())
                    //    {
                    //        if (area.Count > 0)
                    //        {
                    //            msg = "Area name already been used by another client/church";
                    //            return true;
                    //        }
                    //    }

                    //}
                    #endregion

                    #region Diocese
                    //if (typeof(Diocese).Name == typeof(T).Name)
                    //{
                    //    var sql = string.Format("Select * FROM \"ChurchApp\".\"Diocese\" ");
                    //    var thisChurchStructures = _repository.RepositoryContext().Database.SqlQuery<Diocese>(sql).ToList();
                    //    var diocese = thisChurchStructures.FindAll(x => x.ChurchId == churchId && string.Compare(x.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

                    //    msg = "";
                    //    if (diocese.IsNullOrEmpty()) return false;
                    //    if (diocese.Any())
                    //    {
                    //        if (diocese.Count > 0)
                    //        {
                    //            msg = "Diocese name already been used by another client/church";
                    //            return true;
                    //        }
                    //    }

                    //}
                    #endregion
                    
                    
                }
                else
                {
                    msg = "";
                    return false;
                }
                

                
                //var structureTbl = type.ToString();
                //List<T> check;
                //if (!string.IsNullOrEmpty(name) && churchId > 0)
                //{
                //    //var sql = string.Format("Select * FROM \"ChurchApp\".\"" + structureTbl);
                //        //"\"  WHERE \"ChurchId\" = {0} AND \"Name\" = "+ "'" + "{1}" + "'", churchId, name);
                    
                //    //var sql = string.Format(
                //    //    "Select * FROM \"ChurchApp\".\"" +structureTbl +
                //    //    "\"  WHERE \"ChurchId\" = {0} AND \"Name\" = \'{1}\'", churchId, name);
                //    //var thisChurchStructures = _repository.RepositoryContext().Database.SqlQuery<T>(sql).ToList();
                //    thisChurchStructures.Find()

                //}
                //else
                //{
                //    check = null;
                //}

                //msg = "";
                //if (check.IsNullOrEmpty()) return false;
                //if (check != null)
                //{
                //    if (check.Count > 0)
                //    {
                //        msg = structureTbl + " Name already been used by another client/church";
                //        return true;
                //    }
                //}

                msg = "Unable to check duplicate! Please try again later";
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

    }
}
