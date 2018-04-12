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
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ChurchServiceTypeRepository
    {
        private readonly IIcasRepository<ChurchServiceType> _repository;
        private readonly IcasUoWork _uoWork;

        public ChurchServiceTypeRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ChurchServiceType>(_uoWork);
        }


        internal ChurchServiceTypeRegResponse AddChurchServiceType(ChurchServiceTypeRegObj churchServiceTypeRegObj)
        {
            var response = new ChurchServiceTypeRegResponse
            {
                ChurchServiceTypeId = 0,
                Name = churchServiceTypeRegObj.Name,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {
                if (churchServiceTypeRegObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Church registration Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(churchServiceTypeRegObj, out valResults))
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
                if (IsDuplicate(churchServiceTypeRegObj.Name, out msg))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }

                var churchServiceTypeObj = new ChurchServiceType
                {
                    Name = churchServiceTypeRegObj.Name,
                    SourceId = churchServiceTypeRegObj.SourceId,
                };

                var processedChurchServiceType = _repository.Add(churchServiceTypeObj);
                _uoWork.SaveChanges();
                if (churchServiceTypeObj.ChurchServiceTypeId > 0)
                {
                    response.ChurchServiceTypeId = churchServiceTypeObj.ChurchServiceTypeId;
                    response.Name = processedChurchServiceType.Name;
                    response.Status.IsSuccessful = true;
                    return response;
                }

                response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
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

        internal RespStatus UpdateChurchServiceType(ChurchServiceTypeRegObj churchServiceTypeRegObj)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };

            try
            {
                if (churchServiceTypeRegObj.Equals(null))
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your processing";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(churchServiceTypeRegObj, out valResults))
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
                var thisChurchServiceType = GetChurchServiceType(churchServiceTypeRegObj.ChurchServiceTypeId, out msg);
                if (thisChurchServiceType == null || thisChurchServiceType.Name.Length < 1)
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage =
                            string.IsNullOrEmpty(msg) ? "Invalid Church Service Type Info" : msg;
                    return response;
                }

                // Change the Name
                thisChurchServiceType.Name = churchServiceTypeRegObj.Name;

                if (IsDuplicate(churchServiceTypeRegObj.Name, out msg, 1))
                {
                    response.Message.FriendlyMessage = msg;
                    response.Message.TechnicalMessage = msg;
                    response.IsSuccessful = false;
                    return response;
                }

                var processedChurchServiceType = _repository.Update(thisChurchServiceType);
                _uoWork.SaveChanges();
                if (processedChurchServiceType.ChurchServiceTypeId < 1)
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

        internal ChurchServiceType IsServiceNameExist(string name)
        {
            try
            {
                List<ChurchServiceType> check3;
                if (!string.IsNullOrEmpty(name))
                {
                    var sql2 =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ChurchServiceType\" WHERE lower(\"Name\") = lower('{0}')",
                            name.Replace("'", "''"));
                    check3 = _repository.RepositoryContext().Database.SqlQuery<ChurchServiceType>(sql2).ToList();
                }
                else
                {
                    check3 = null;
                }

                if ((check3 == null || check3.IsNullOrEmpty()) || check3[0].ChurchServiceTypeId < 1) return null;
                return check3[0];

            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }


        internal List<RegisteredChurchServiceTypeReportObj> GetAllRegisteredChurchServiceTypeObjs()
        {
            try
            {
                var myItemList = GetChurchServiceTypes();
                if (!myItemList.Any()) return null;

                var retList = new List<RegisteredChurchServiceTypeReportObj>();
                myItemList.ForEachx(m =>
                {
                    string msg;
                    retList.Add(new RegisteredChurchServiceTypeReportObj
                    {
                        ChurchServiceTypeId = m.ChurchServiceTypeId,
                        Name = m.Name,
                        SourceId = (int)m.SourceId,
                    });
                });

                return retList.FindAll(x => x.SourceId == (int)ChurchSettingSource.App);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal string GetChurchServiceTypeNameById(int serviceId)
        {
            try
            {
                if (serviceId < 1) return string.Empty;
                var myItem = _repository.GetById(serviceId);
                if (myItem == null || myItem.ChurchServiceTypeId < 1) return string.Empty;

                return myItem.Name;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return string.Empty;
            }
        }

        internal List<ChurchServiceType> GetChurchServiceTypes()
        {
            var myItemList = _repository.GetAll().ToList();
            if (!myItemList.Any()) return null;
            return myItemList.FindAll(x => x.SourceId == ChurchSettingSource.App);
        }

        private ChurchServiceType GetChurchServiceType(long churchServiceTypeId, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchAPPDB\".\"ChurchServiceType\"  WHERE \"ChurchServiceTypeId\" = {0};", churchServiceTypeId);

                var check = _repository.RepositoryContext().Database.SqlQuery<ChurchServiceType>(sql1).ToList();
                if (check.IsNullOrEmpty())
                {
                    msg = "No Church Service Type record found!";
                    return null;
                }
                if (check.Count != 1)
                {
                    msg = "Invalid Church Service Type  Record!";
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
        private bool IsDuplicate(string name, out string msg, int status = 0)
        {
            try
            {
                // " AND \"SourceId\" = {1})",
                // Check Duplicate Church Name
                List<ChurchServiceType> check3;
                if (!string.IsNullOrEmpty(name))
                {
                    var sql2 =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ChurchServiceType\" WHERE lower(\"Name\") = lower('{0}')" +
                            " AND \"SourceId\" = {1}", name.Replace("'", "''"), (int)ChurchSettingSource.App);
                    check3 = _repository.RepositoryContext().Database.SqlQuery<ChurchServiceType>(sql2).ToList();
                }
                else
                {
                    check3 = null;
                }
               
                msg = "";
                if ((check3 == null || check3.IsNullOrEmpty())) return false;

                switch (status)
                {
                    case 0:
                        if (check3.Count > 0)
                        {
                            msg = "Duplicate Error! church service type already in the list";
                            return true;
                        }
                        break;

                    case 1:
                        if (check3.Count >= 1)
                        {
                            msg = "Duplicate Error! church service type already in the list";
                            return true;
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
