using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using ICASStacks.APIObjs;
using ICASStacks.DataContract;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using ICASStacks.Repository.Helpers;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ChurchStructureHqtrRepository
    {
        private readonly IIcasRepository<ChurchStructureHqtr> _repository;
        private readonly IcasUoWork _uoWork;

        public ChurchStructureHqtrRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ChurchStructureHqtr>(_uoWork);
        }


        internal ChurchStructureHqtrRegResponse AddChurchStructureHqtr(ChurchStructureHqtr churchStructureHqtrObj)
        {

            var response = new ChurchStructureHqtrRegResponse
            {
                ChurchStructureHqtrId = 0,
                ClientId = churchStructureHqtrObj.ClientId,
                ChurchId = churchStructureHqtrObj.ChurchId,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {
                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(churchStructureHqtrObj, out valResults))
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

                // Valid Admin who carried out this operation
                //if (churchStructureHqtrObj.RegisteredByUserId < 1)
                //{
                //    response.Status.Message.FriendlyMessage = "Invalid Admin User Information";
                //    response.Status.Message.TechnicalMessage = "Invalid Admin User Information";
                //    response.Status.IsSuccessful = false;
                //    return response;
                //}

                string msg;
                if (IsDuplicate(churchStructureHqtrObj, out msg))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }

                var processedchurchStructureHqtr = _repository.Add(churchStructureHqtrObj);
                _uoWork.SaveChanges();
                if (processedchurchStructureHqtr.ChurchStructureHqtrId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                    response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                    response.Status.IsSuccessful = false;
                    return response;
                }

                response.ChurchStructureHqtrId = processedchurchStructureHqtr.ChurchStructureHqtrId;
                response.Status.Message.FriendlyMessage = "";
                response.Status.IsSuccessful = true;
                return response;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal ChurchStructureHqtr GetChurchStructureHqtrByClientId(long clientId)
        {
            try
            {
                if (clientId < 1) return null;
                var myItemList = _repository.GetAll(x => x.ClientId == clientId).ToList();
                return !myItemList.Any() ? null : myItemList[0];
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }


        internal bool IsDuplicatex(long churchId, long structureHqrtId, long structureTypeId, out string msg)
        {
            try
            {
                List<ChurchStructureHqtr> check;
                if (churchId > 0 && structureHqrtId > 0 && structureTypeId > 0)
                {
                    var sql =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ChurchStructureHqtr\" WHERE \"ChurchId\" = {0} AND \"StructureDetailId\" = {1} AND \"ChurchStructureTypeId\" = {2}",
                            churchId, structureHqrtId, structureTypeId);
                    check = _repository.RepositoryContext().Database.SqlQuery<ChurchStructureHqtr>(sql).ToList();
                }
                else
                {
                    check = null;
                }

                if (check.IsNullOrEmpty())
                {
                    msg = "";
                    return false;
                }

                if (check != null)
                {
                    if (check.Count > 0)
                    {
                        msg = "Duplicate Error! Another Church already had been used as the Headquarter";
                        return true;
                    }
                    msg = "";
                    return false;
                }

                msg = "Unable to check duplicate! Please try again later";
                return true;
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }



        internal bool IsDuplicate(ChurchStructureHqtr hqtrObj, out string msg, int callerType = 0)
        {

            try
            {
                //List<ChurchStructureHqtr> check;
                var check =  new List<ChurchStructureHqtr>();
                if (hqtrObj == null)
                {
                    msg = "";
                    return false;
                }

                if (hqtrObj.ChurchStructureTypeId > 0 && hqtrObj.ChurchId > 0 && hqtrObj.StructureDetailId > 0)
                {
                    var churchId = hqtrObj.ChurchId;
                    var structureHqrtId = hqtrObj.StructureDetailId;
                    var structureTypeId = hqtrObj.ChurchStructureTypeId;
                    var churchStructureHqtrId = hqtrObj.ChurchStructureHqtrId;
                    //var sql =
                    //    string.Format(
                    //        "Select * FROM \"ChurchApp\".\"ChurchStructureHqtr\" WHERE \"ChurchId\" = {0} AND \"StructureDetailId\" = {1} AND \"ChurchStructureTypeId\" = {2}",
                    //        churchId, structureHqrtId, structureTypeId);

                    var sql = "";

                    switch (callerType)
                    {
                        case 0:
                            sql =
                               string.Format(
                                   "Select * FROM \"ChurchAPPDB\".\"ChurchStructureHqtr\" WHERE " +
                                   "\"ChurchId\" = {0} " +
                                   "AND \"StructureDetailId\" = {1} " +
                                   "AND \"ChurchStructureTypeId\" = {2} ",
                                   churchId, structureHqrtId, structureTypeId);
                            check = _repository.RepositoryContext().Database.SqlQuery<ChurchStructureHqtr>(sql).ToList();
                            break;

                        case 1:
                            sql =
                               string.Format(
                                   "Select * FROM \"ChurchAPPDB\".\"ChurchStructureHqtr\" WHERE " +
                                   "\"ChurchId\" = {0} " +
                                   "AND \"StructureDetailId\" = {1} " +
                                   "AND \"ChurchStructureTypeId\" = {2} " +
                                   "AND \"ChurchStructureHqtrId\" != {3}",
                                   churchId, structureHqrtId, structureTypeId, churchStructureHqtrId);
                            check = _repository.RepositoryContext().Database.SqlQuery<ChurchStructureHqtr>(sql).ToList();
                            break;
                    }
                    
                    
                    //check = _repository.RepositoryContext().Database.SqlQuery<ChurchStructureHqtr>(sql).ToList();
                }
                else
                {
                    check = null;
                }

                if (check.IsNullOrEmpty())
                {
                    msg = "";
                    return false;
                }

                if (check != null)
                {
                   
                    if (check.Count > 0)
                    {
                        msg = "Duplicate Error! Another Church already had been used as the Headquarter";
                        return true;
                    }
                    
                }

                msg = "";
                return false;

                #region Old Method
                //if (check != null)
                //{
                //    switch (status)
                //    {
                //        case 0:
                //            if (check.Count > 0)
                //            {
                //                msg = "Duplicate Error! Another Church already had been used as the Headquarter";
                //                return true;
                //            }
                //            msg = "";
                //            return false;

                //        case 1:
                //            if (check.Count >= 1)
                //            {
                //                msg = "Duplicate Error! Another Church already had been used as the Headquarter";
                //                return true;
                //            }
                //            msg = "";
                //            return false;
                //    }
                    
                //}

                //msg = "Unable to check duplicate! Please try again later";
                //return true;
                #endregion
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        internal bool IsDuplicatexx(ChurchStructureHqtr hqtrObj, out string msg)
        {

            try
            {
                List<ChurchStructureHqtr> check;
                if (hqtrObj == null)
                {
                    msg = "";
                    return false;
                }

                if (hqtrObj.ChurchId > 0 && hqtrObj.StructureDetailId > 0 && hqtrObj.ChurchStructureTypeId > 0)
                {
                    var churchId = hqtrObj.ChurchId;
                    var structureHqrtId = hqtrObj.StructureDetailId;
                    var structureTypeId = hqtrObj.ChurchStructureTypeId;

                    var sql =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ChurchStructureHqtr\" WHERE \"ChurchId\" = {0} AND \"StructureDetailId\" = {1} AND \"ChurchStructureTypeId\" = {2}",
                            churchId, structureHqrtId, structureTypeId);
                    check = _repository.RepositoryContext().Database.SqlQuery<ChurchStructureHqtr>(sql).ToList();
                }
                else
                {
                    check = null;
                }

                if (check.IsNullOrEmpty())
                {
                    msg = "";
                    return false;
                }

                if (check != null)
                {
                    if (check.Count > 0)
                    {
                        msg = "Duplicate Error! Another Church already had been used as the Headquarter";
                        return true;
                    }
                    msg = "";
                    return false;
                }

                msg = "Unable to check duplicate! Please try again later";
                return true;
            }
            catch (Exception ex)
            {
                msg = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        internal bool IsHqtrDuplicate(long churchId, long structureHqrtId, long structureTypeId, out string msg)
        {
            try
            {
                List<ChurchStructureHqtr> check;
                if (churchId > 0 && structureHqrtId > 0 && structureTypeId > 0)
                {
                    var sql =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ChurchStructureHqtr\" WHERE \"ChurchId\" = {0} AND \"StructureHqtrId\" = {1} AND \"StructureTypeId\" = {2}",
                            churchId, structureHqrtId, structureTypeId);
                    check = _repository.RepositoryContext().Database.SqlQuery<ChurchStructureHqtr>(sql).ToList();
                }
                else
                {
                    check = null;
                }

                if (check != null)
                {
                    if (check.Count > 0)
                    {
                        msg = "Duplicate Error! Another Church already had been used as the Headquarter";
                        return true;
                    }
                    else
                    {
                        msg = "";
                        return false;
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
    }
}
