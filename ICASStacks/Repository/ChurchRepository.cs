using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
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
    internal class ChurchRepository
    {

        private readonly IIcasRepository<Church> _repository;
        private readonly IIcasRepository<StateOfLocation> _stateOfLocationRepository;
        private readonly IcasUoWork _uoWork;

        public ChurchRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<Church>(_uoWork);
            _stateOfLocationRepository = new IcasRepository<StateOfLocation>(_uoWork);
        }
        
        internal ChurchRegResponse AddChurch(ChurchRegObj churchRegObj)
        {

            var response = new ChurchRegResponse
            {
                ChurchId = churchRegObj.ChurchId,
                Name = churchRegObj.Name,
                ShortName = churchRegObj.ShortName,
                Email = churchRegObj.Email,
                PhoneNumber = churchRegObj.PhoneNumber,
                StateOfLocationId = churchRegObj.StateOfLocationId,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {

                if (churchRegObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Church registration Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(churchRegObj, out valResults))
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
                if (churchRegObj.RegisteredByUserId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Admin User Information";
                    response.Status.Message.TechnicalMessage = "Invalid Admin User Information";
                    response.Status.IsSuccessful = false;
                    return response;
                }

                churchRegObj.PhoneNumber = CleanMobile(churchRegObj.PhoneNumber);
                string msg;
                if (IsDuplicate(churchRegObj.Name, churchRegObj.PhoneNumber, churchRegObj.Email, out msg))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }

               var churchObj = new Church
                {
                    Name = churchRegObj.Name,
                    ShortName = churchRegObj.ShortName,
                    Founder = churchRegObj.Founder,
                    PhoneNumber = churchRegObj.PhoneNumber,
                    Email = churchRegObj.Email,
                    StateOfLocationId = churchRegObj.StateOfLocationId,
                    Address = churchRegObj.Address,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = churchRegObj.RegisteredByUserId,
                };

               var processedChurch = _repository.Add(churchObj);
               _uoWork.SaveChanges();
               if (processedChurch.ChurchId > 0)
               {
                    response.ChurchId = processedChurch.ChurchId;
                    response.Name = processedChurch.Name;
                    response.ShortName = processedChurch.ShortName;
                    response.Email = processedChurch.Email;
                    response.PhoneNumber = processedChurch.PhoneNumber;
                    response.StateOfLocationId = processedChurch.StateOfLocationId;
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

        internal RespStatus UpdateChurch(ChurchRegObj church)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };
            try
            {
                if (church.Equals(null))
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your registration";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(church, out valResults))
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
                var thisChurch = GetChurch(church.ChurchId, out msg);
                if (thisChurch == null || thisChurch.Name.Length < 1)
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = string.IsNullOrEmpty(msg) ? "Invalid User Information" : msg;
                    return response;
                }

                church.PhoneNumber = CleanMobile(church.PhoneNumber);
                if (IsDuplicate(church.Name, church.PhoneNumber, church.Email, out msg, 1))
                {
                    response.Message.FriendlyMessage = msg;
                    response.Message.TechnicalMessage = msg;
                    response.IsSuccessful = false;
                    return response;
                }

                thisChurch.Name = church.Name;
                thisChurch.ShortName = church.ShortName;
                thisChurch.Founder = church.Founder;
                thisChurch.PhoneNumber = church.PhoneNumber;
                thisChurch.Email = church.Email;
                thisChurch.Address = church.Address;
                thisChurch.StateOfLocationId = church.StateOfLocationId;

                var processedChurch = _repository.Update(thisChurch);
                _uoWork.SaveChanges();
                
                if (processedChurch.ChurchId < 1)
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
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
        
        private Church GetChurch(long churchId, out string msg)
        {
            try
            {
                
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchAPPDB\".\"Church\"  WHERE \"ChurchId\" = {0};", churchId);

                var check = _repository.RepositoryContext().Database.SqlQuery<Church>(sql1).ToList();
                
                if (check.IsNullOrEmpty())
                {
                    msg = "No Church Record Found!";
                    return null;
                }
                if (check.Count != 1)
                {
                    msg = "Invalid Church Record!";
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
        
        internal Church GetChurch(long churchId)
        {
            try
            {
                var myItem = _repository.GetById(churchId);
                if (myItem == null || myItem.ChurchId < 1) { return null; }
                return myItem;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal List<Church> GetChurches()
        {
            
            try
            {
                var myItemList = _repository.GetAll().ToList();
                if (!myItemList.Any()) { return null; }
                return myItemList;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        internal List<RegisteredChurchReportObj> GetAllRegisteredChurchObjs()
        {
            try
            {
                var myItemList = GetChurches();
                if (!myItemList.Any()) return null;

                var retList = new List<RegisteredChurchReportObj>();
                myItemList.ForEachx(m =>
                {
                    string msg;
                    retList.Add(new RegisteredChurchReportObj
                    {
                        ChurchId = m.ChurchId,
                        Name = m.Name,
                        ShortName = m.ShortName,
                        Founder = m.Founder,
                        PhoneNumber = m.PhoneNumber,
                        Email = m.Email,
                        Address = m.Address,
                        StateOfLocation = _stateOfLocationRepository.GetById(m.StateOfLocationId).Name,
                        StateOfLocationId = m.StateOfLocationId,

                    });
                });

                return retList.FindAll(x => x.ChurchId > 2);
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }


        private bool IsDuplicate(string name, string phoneNo, string email, out string msg, int status = 0)
        {
            try
            {

                // Check Duplicate Phone No Duplicate
                var sql1 = string.Format("Select * FROM \"ChurchAPPDB\".\"Church\" WHERE \"PhoneNumber\" = '{0}'", phoneNo);

                // Check Duplicate Email
                List<Church> check2;
                if (!string.IsNullOrEmpty(email))
                {
                    var sql2 = string.Format("Select * FROM \"ChurchAPPDB\".\"Church\" WHERE \"Email\" = '{0}'", email);
                    check2 = _repository.RepositoryContext().Database.SqlQuery<Church>(sql2).ToList();
                }
                else
                {
                    check2 = null;
                }

                // Check Duplicate Church Name
                List<Church> check3;
                if (!string.IsNullOrEmpty(name))
                {
                    var sql2 =
                        string.Format("Select * FROM \"ChurchAPPDB\".\"Church\" WHERE lower(\"Name\") = lower('{0}')", name.Replace("'", "''"));
                    check3 = _repository.RepositoryContext().Database.SqlQuery<Church>(sql2).ToList();
                }
                else
                {
                    check3 = null;
                }

                var check = _repository.RepositoryContext().Database.SqlQuery<Church>(sql1).ToList();
                msg = "";
                if (check.IsNullOrEmpty() && (check2 == null || check2.IsNullOrEmpty()) && (check3 == null || check3.IsNullOrEmpty())) return false;

                
                
                switch (status)
                {
                    case 0:
                        if (check2 != null)
                        {
                            if (check2.Count > 0)
                            {
                                msg = "Duplicate Error! Email already exist";
                                return true;
                            }
                        }
                        if (check.Count > 0)
                        {
                            msg = "Duplicate Error! Phone Number already been used by another Church";
                            return true;
                        }
                        if (check3 != null)
                        {
                            if (check3.Count > 0)
                            {
                                msg = "Duplicate Error! Name already been used by another Church";
                                return true;
                            }
                        }
                        break;

                    case 1:
                        if (check2 != null)
                        {
                            if (check2.Count > 1)
                            {
                                msg = "Duplicate Error! Email already exist";
                                return true;
                            }
                        }
                        if (check.Count > 1)
                        {
                            msg = "Duplicate Error! Phone Number already been used by another Church";
                            return true;
                        }
                        if (check3 != null)
                        {
                            if (check3.Count > 1)
                            {
                                msg = "Duplicate Error! Name already been used by another Church";
                                return true;
                            }
                        }
                        break;
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


        private string CleanMobile(string mobileNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(mobileNumber))
                    return string.Empty;
                if (mobileNumber.StartsWith("234"))
                    return mobileNumber;
                mobileNumber = mobileNumber.TrimStart(new char[] { '0' });
                return string.Format("234{0}", mobileNumber);
            }
            catch (Exception)
            {
                return mobileNumber;
            }
        }
    }
}
