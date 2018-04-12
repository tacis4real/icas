using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using ICASStacks.APIObjs;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.DataContract.Enum;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using ICASStacks.Repository.Helpers;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ChurchMemberRepository
    {

        private readonly IIcasRepository<ChurchMember> _repository;
        private readonly IIcasRepository<RoleInChurch> _roleInChurchRepository;
        private readonly IIcasRepository<Profession> _professionRepository;
        private readonly IcasUoWork _uoWork;

        public ChurchMemberRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ChurchMember>(_uoWork);
            _roleInChurchRepository = new IcasRepository<RoleInChurch>(_uoWork);
            _professionRepository = new IcasRepository<Profession>(_uoWork);
        }
        

        internal ChurchMemberRegResponse AddChurchMember(ChurchMemberRegistrationObj churchMemberRegObj)
        {

            var response = new ChurchMemberRegResponse
            {
                ChurchMemberId = 0,
                ClientChurchId = churchMemberRegObj.ClientChurchId,
                FullName = churchMemberRegObj.FullName,
                RoleInChurchId = churchMemberRegObj.RoleInChurchId,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {
                if (churchMemberRegObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Church registration Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(churchMemberRegObj, out valResults))
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
                if (churchMemberRegObj.RegisteredByUserId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Admin User Information";
                    response.Status.Message.TechnicalMessage = "Invalid Admin User Information";
                    response.Status.IsSuccessful = false;
                    return response;
                }

                string msg;
                if (IsDuplicate(churchMemberRegObj.PhoneNumber, churchMemberRegObj.Email, out msg))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }

                // Submit Church Member
                //var reverseDate = DateScrutnizer.ReverseToCoreServerDate(churchMemberRegObj.DateJoined);
                //if (!string.IsNullOrEmpty(reverseDate))
                //{
                //    churchMemberRegObj.DateJoined = reverseDate;
                //}


                churchMemberRegObj.DateJoined = DateScrutnizer.ReverseToServerDate(churchMemberRegObj.DateJoined.Replace('-', '/'));
                var thisChurchMember = new ChurchMember
                {
                    ClientChurchId = churchMemberRegObj.ClientChurchId,
                    FullName = churchMemberRegObj.FullName,
                    ProfessionId = churchMemberRegObj.ProfessionId,
                    //RoleInChurchId = churchMemberRegObj.RoleInChurchId,
                    Sex = churchMemberRegObj.Sex,
                    Email = churchMemberRegObj.Email,
                    MobileNumber = churchMemberRegObj.PhoneNumber,
                    Address = churchMemberRegObj.Address,
                    DateJoined = churchMemberRegObj.DateJoined,
                    RegisteredByUserId = churchMemberRegObj.RegisteredByUserId,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp()
                };

                var processedChurchMember = _repository.Add(thisChurchMember);
                _uoWork.SaveChanges();
                if (processedChurchMember.ChurchMemberId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Unable to complete registration! Please try again later";
                    response.Status.Message.TechnicalMessage = "Process Failed! Unable to save data";
                    response.Status.IsSuccessful = false;
                    return response;
                }

                response.ChurchMemberId = processedChurchMember.ChurchMemberId;
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
        
        internal BulkChurchMemberRegResponseObj AddBulkChurchMemberInfo(List<ChurchMemberRegistrationObj> bulkItems, HttpPostedFileBase uploadedFile)
        {
            var response = new BulkChurchMemberRegResponseObj
            {
                MainStatus = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage()
                },
                ChurchMemberRegResponses = new List<UploadChurchMemberRegResponseObj>()
            };

            try
            {
                if (bulkItems.Equals(null))
                {
                    response.MainStatus.Message.FriendlyMessage =
                        response.MainStatus.Message.TechnicalMessage =
                            "Error Occurred! Unable to proceed with your registration. Registration Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(bulkItems, out valResults))
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
                    response.MainStatus.Message.FriendlyMessage = response.MainStatus.Message.TechnicalMessage = errorDetail.ToString();
                    return response;
                }


                foreach (var item in bulkItems)
                {
                    var thisResponse = new UploadChurchMemberRegResponseObj
                    {
                        Status = new RespStatus
                        {
                            IsSuccessful = false,
                            Message = new RespMessage(),
                        },
                        FullName = item.FullName,
                        RecordId = item.RecordId,
                    };

                    try
                    {

                        // Do manipulation for getting RoleInChurchId in RoleInChurch & ProfessionId in Profession or in the Repository
                        //var reverseDate = DateScrutnizer.ReverseToCoreServerDate(item.DateJoined);
                        //if (!string.IsNullOrEmpty(reverseDate))
                        //{
                        //    item.DateJoined = reverseDate;
                        //}
                        item.DateJoined = DateScrutnizer.ReverseToServerDate(item.DateJoined.Replace('-', '/'));
                        var roleInChurchId = GetMyId(item.ClientChurchId, item.RoleInChurch, 1);
                        var professionId = GetMyId(item.ClientChurchId, item.Profession);

                        var churchMember = new ChurchMember
                        {
                            ClientChurchId = item.ClientChurchId,
                            FullName = item.FullName,
                            //RoleInChurchId = roleInChurchId,
                            ProfessionId = professionId,
                            DateJoined = item.DateJoined,
                            Address = item.Address,
                            MobileNumber = item.PhoneNumber,
                            Sex = item.Sex,
                            Email = item.Email,
                            UploadStatus = UploadStatus.Successful,
                            RegisteredByUserId = item.RegisteredByUserId,
                            TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                            TimeStampUploaded = DateScrutnizer.CurrentTimeStamp(),
                        };
                        
                        if (!EntityValidatorHelper.Validate(churchMember, out valResults))
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

                            thisResponse.Status.Message.FriendlyMessage = thisResponse.Status.Message.TechnicalMessage = errorDetail.ToString();
                            response.ChurchMemberRegResponses.Add(thisResponse);
                            continue;
                        }

                        // Try to check for duplicate here using 'Student Matric Number'
                        string msg;
                        if (IsUploadDuplicate(churchMember.MobileNumber, churchMember.Email, ref thisResponse.Status))
                        {
                            response.ChurchMemberRegResponses.Add(thisResponse);
                            continue;
                        }

                        var retVal = _repository.Add(churchMember);
                        _uoWork.SaveChanges();
                        if (retVal.ChurchMemberId < 1)
                        {
                            thisResponse.Status.Message.FriendlyMessage = thisResponse.Status.Message.TechnicalMessage = "Processing Failed! Please try again later";
                            response.ChurchMemberRegResponses.Add(thisResponse);
                            continue;
                        }

                        thisResponse.ChurchMemberId = retVal.ChurchMemberId;
                        thisResponse.FullName = retVal.FullName;
                        thisResponse.Status.IsSuccessful = true;
                        response.ChurchMemberRegResponses.Add(thisResponse);

                    }
                    catch (Exception ex)
                    {
                        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                        thisResponse.Status.Message.FriendlyMessage = "Unable to complete registration at this time! Please try again later";
                        thisResponse.Status.Message.TechnicalMessage = "Error: " + ex.Message;
                        thisResponse.Status.IsSuccessful = false;
                        response.ChurchMemberRegResponses.Add(thisResponse);
                    }

                }
            }
            catch (Exception ex)
            {
                response.MainStatus.Message.FriendlyMessage = "Processing Error Occurred! Please try again later";
                response.MainStatus.Message.TechnicalMessage = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return response;
            }
            
            response.MainStatus.IsSuccessful = true;
            return response;
        }

        internal RespStatus UpdateChurchMember(ChurchMemberRegistrationObj churchMemberRegObj)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };

            try
            {
                if (churchMemberRegObj.Equals(null))
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your processing";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(churchMemberRegObj, out valResults))
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
                var thisChurchMember = GetChurchMember(churchMemberRegObj.ChurchMemberId, out msg);
                if (thisChurchMember == null || thisChurchMember.FullName.Length < 1)
                {
                    response.Message.FriendlyMessage =
                        response.Message.TechnicalMessage =
                            string.IsNullOrEmpty(msg) ? "Invalid Church Information" : msg;
                    return response;
                }

                if (IsDuplicate(churchMemberRegObj.PhoneNumber, churchMemberRegObj.Email, out msg, 1))
                {
                    response.Message.FriendlyMessage = msg;
                    response.Message.TechnicalMessage = msg;
                    response.IsSuccessful = false;
                    return response;
                }

                //var reverseDate = DateScrutnizer.ReverseToCoreServerDate(churchMemberRegObj.DateJoined);
                //if (!string.IsNullOrEmpty(reverseDate))
                //{
                //    churchMemberRegObj.DateJoined = reverseDate;
                //}

                churchMemberRegObj.DateJoined = DateScrutnizer.ReverseToServerDate(churchMemberRegObj.DateJoined.Replace('-', '/'));

                thisChurchMember.FullName = churchMemberRegObj.FullName;
                thisChurchMember.ProfessionId = churchMemberRegObj.ProfessionId;
                //thisChurchMember.RoleInChurchId = churchMemberRegObj.RoleInChurchId;
                thisChurchMember.Sex = churchMemberRegObj.Sex;
                thisChurchMember.DateJoined = churchMemberRegObj.DateJoined;
                thisChurchMember.Email = churchMemberRegObj.Email;
                thisChurchMember.MobileNumber = churchMemberRegObj.PhoneNumber;
                thisChurchMember.Address = churchMemberRegObj.Address;

                var processedChurchMember = _repository.Update(thisChurchMember);
                _uoWork.SaveChanges();
                if (processedChurchMember.ChurchMemberId < 1)
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


        private int GetMyId(long clientId, string myName = "", int caller = 0)
        {

            var myId = 0;
            try
            {
                int retId;
                switch (caller)
                {
                    case 0:
                        var professions = _professionRepository.GetAll();

                        var firstOrDefault1 = professions.FirstOrDefault(x => x.Name == "Others");
                        if (firstOrDefault1 != null)
                            myId = firstOrDefault1.ProfessionId;

                        if (myName != null)
                        {
                            var check = new ProfessionRepository().IsProfessionExist(myName, out retId);
                            if (check)
                            {
                                myId = retId;
                            }
                            else
                            {
                                // Add the coming name and use its id
                                var profession = new Profession
                                {
                                    Name = myName,
                                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp(),
                                    AddedByUserId = Convert.ToInt32(clientId),
                                };

                                string msg;
                                var freshId = new ProfessionRepository().AddProfession(profession, out msg);
                                if (freshId > 0 && string.IsNullOrEmpty(msg))
                                {
                                    myId = freshId;
                                }

                            }
                           
                        }


                        break;
                    case 1:

                        //var roleInChurchs = _roleInChurchRepository.GetAll();
                        //var firstOrDefault = roleInChurchs.FirstOrDefault(x => x.Name == "Others");
                        //if (firstOrDefault != null)
                        //    myId = firstOrDefault.RoleInChurchId;
                        
                        if (myName != null)
                        {
                            var check = new RoleInChurchRepository().IsRoleInChurchExist(myName, out retId);
                            if (check)
                            {
                                myId = retId;
                            }
                            else
                            {
                                // Add the coming name and use its id
                                var roleInChurch = new RoleInChurch
                                {
                                    //Name = myName,
                                    //Description = myName,
                                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp(),
                                    AddedByUserId = Convert.ToInt32(clientId),
                                };

                                string msg;
                                var freshId = new RoleInChurchRepository().AddRoleInChurch(roleInChurch, out msg);
                                if (freshId > 0 && string.IsNullOrEmpty(msg))
                                {
                                    myId = freshId;
                                }

                            }

                            
                        }
                        break;
                }

                return myId;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return 1;
            }

        }
        private int GetMyIds(long clientId, string myName = "", int caller = 0)
        {

            var myId = 0;
            try
            {
                switch (caller)
                {
                    case 0:
                        var professions = _professionRepository.GetAll();

                        var firstOrDefault1 = professions.FirstOrDefault(x => x.Name == "Others");
                            if (firstOrDefault1 != null)
                                myId = firstOrDefault1.ProfessionId;


                        //if (string.IsNullOrEmpty(myName))
                        //{
                        //    var firstOrDefault = professions.FirstOrDefault(x => x.Name == "Others");
                        //    if (firstOrDefault != null)
                        //        myId = firstOrDefault.ProfessionId;
                        //    return firstOrDefault.ProfessionId;
                        //}

                        if (myName != null)
                        {

                            //var myItem = _repository.GetAll(m => string.Compare(m.Username, username, StringComparison.CurrentCultureIgnoreCase) == 0).ToList();
                            //if (!myItem.Any() || myItem.Count() != 1) { return null; }
                            //return myItem[0].ClientProfileId < 1 ? null : myItem[0];

                            var myItem = _professionRepository.GetAll(m => string.Compare(m.Name.Substring(0, 3), myName.Substring(0, 3), StringComparison.CurrentCultureIgnoreCase) == 0).ToList();
                            if (myItem.Any() || myItem.Count == 1)
                            {
                                myId = myItem[0].ProfessionId;
                            }

                            //foreach (var profession in professions)
                            //{
                            //    if (string.Compare(myName.Substring(0, 3), profession.Name.Substring(0, 3), StringComparison.OrdinalIgnoreCase) == 0)
                            //    {
                            //        myId = profession.ProfessionId;
                            //        //return profession.ProfessionId;
                            //    }
                            //}
                        }


                        break;
                    case 1:

                        //var roleInChurchs = _roleInChurchRepository.GetAll();
                        //var firstOrDefault = roleInChurchs.FirstOrDefault(x => x.Name == "Others");
                        //    if (firstOrDefault != null)
                        //        myId = firstOrDefault.RoleInChurchId;



                        //if (string.IsNullOrEmpty(myName))
                        //{
                        //    var firstOrDefault = roleInChurchs.FirstOrDefault(x => x.Name == "Others");
                        //    if (firstOrDefault != null)
                        //        myId = firstOrDefault.RoleInChurchId;
                        //        //return firstOrDefault.RoleInChurchId;
                        //}

                        if (myName != null)
                        {

                            //var myItem = _roleInChurchRepository.GetAll(m => string.Compare(m.Name.Substring(0, 3), myName.Substring(0, 3), StringComparison.CurrentCultureIgnoreCase) == 0).ToList();
                            //if (myItem.Any() || myItem.Count == 1)
                            //{
                            //    myId = myItem[0].RoleInChurchId;
                            //}


                            //foreach (var roleInChurch in roleInChurchs)
                            //{
                            //    if (string.Compare(myName.Substring(0, 3), roleInChurch.Name.Substring(0, 3), StringComparison.OrdinalIgnoreCase) == 0)
                            //    {
                            //        myId = roleInChurch.RoleInChurchId;
                            //        //return roleInChurch.RoleInChurchId;
                            //    }
                            //}
                        }
                        break;
                }

                return myId;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return 1;
            }
            
        }


        


        private ChurchMember GetChurchMember(long churchMemberId, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ChurchAPPDB\".\"ChurchMember\"  WHERE \"ChurchMemberId\" = {0};", churchMemberId);

                var check = _repository.RepositoryContext().Database.SqlQuery<ChurchMember>(sql1).ToList();
                if (check.IsNullOrEmpty())
                {
                    msg = "No Member record found!";
                    return null;
                }
                if (check.Count != 1)
                {
                    msg = "Invalid Member Record!";
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
        internal List<RegisteredChurchMemberReportObj> GetAllRegisteredChurchMemberObjs(long clientId)
        {
            try
            {
                if (clientId < 1)
                {
                    return new List<RegisteredChurchMemberReportObj>();
                }

                var myItemList = _repository.GetAll(x => x.ClientChurchId == clientId).ToList();
                if (!myItemList.Any()) return new List<RegisteredChurchMemberReportObj>();
                
                var retList = new List<RegisteredChurchMemberReportObj>();
                myItemList.ForEachx(m =>
                {

                    //var roleInChurch = _roleInChurchRepository.GetById(m.RoleInChurchId);
                    var profession = _professionRepository.GetById(m.ProfessionId);
                    //var profession = Enum.GetName(typeof(Professional), m.ProfessionId);
                    //profession = (profession != null
                    //    ? profession.ToString(CultureInfo.CurrentCulture).Replace("_", " ")
                    //    : "Others");
                    var dateJoined = DateScrutnizer.ReverseToGeneralDate(m.DateJoined);
                    var dt = DateTime.ParseExact(dateJoined, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    var joinedDate = dt.ToString("dd MMMM, yyyy");

                    retList.Add(new RegisteredChurchMemberReportObj
                    {
                        ChurchMemberId = m.ChurchMemberId,
                        ClientChurchId = m.ClientChurchId,
                        FullName = m.FullName,
                        Profession = profession.Name,
                        ProfessionId = m.ProfessionId,
                        //RoleInChurchId = m.RoleInChurchId,
                        //RoleInChurch = roleInChurch.Name,
                        Email = m.Email,
                        PhoneNumber = m.MobileNumber,
                        Address = m.Address,
                        SexId = m.Sex,
                        Sex = Enum.GetName(typeof(Sex), m.Sex),
                        DateJoined = dateJoined,
                        JoinedDate = joinedDate
                    });
                });

                return retList;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RegisteredChurchMemberReportObj>(); ;
            }
        }
        
        private bool IsUploadDuplicate(string phoneNo, string email, ref RespStatus respStatus, int status = 0)
        {
            try
            {

                // Check Duplicate Phone No 
                List<ChurchMember> check;
                if (!string.IsNullOrEmpty(phoneNo))
                {
                    var sql1 = string.Format("Select * FROM \"ChurchAPPDB\".\"ChurchMember\" WHERE \"MobileNumber\" = '{0}'", phoneNo);
                    check = _repository.RepositoryContext().Database.SqlQuery<ChurchMember>(sql1).ToList();
                }
                else
                {
                    check = null;
                }

                // Check Duplicate Email
                List<ChurchMember> check2;
                if (!string.IsNullOrEmpty(email))
                {
                    var sql2 =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ChurchMember\" WHERE \"Email\" = '{0}'", email);
                    check2 = _repository.RepositoryContext().Database.SqlQuery<ChurchMember>(sql2).ToList();
                }
                else
                {
                    check2 = null;
                }


                switch (status)
                {

                    case 0:
                        if (check2 != null)
                        {
                            if (check2.Count > 0)
                            {
                                respStatus.Message.FriendlyMessage = "Duplicate Error! Email already exist";
                                respStatus.Message.TechnicalMessage = "Duplicate Error! Email already exist";
                                return true;
                            }
                        }
                        if (check != null)
                        {
                            if (check.Count > 0)
                            {
                                respStatus.Message.FriendlyMessage = "Duplicate Error! Phone number already been used";
                                respStatus.Message.TechnicalMessage = "Duplicate Error! Phone number already been used";
                                return true;
                            }
                        }
                        break;

                    case 1:
                        if (check2 != null)
                        {
                            if (check2.Count >= 1)
                            {
                                respStatus.Message.FriendlyMessage = "Duplicate Error! Email already exist";
                                respStatus.Message.TechnicalMessage = "Duplicate Error! Email already exist";
                                return true;
                            }
                        }
                        if (check != null)
                        {
                            if (check.Count >= 1)
                            {
                                respStatus.Message.FriendlyMessage = "Duplicate Error! Phone number already been used";
                                respStatus.Message.TechnicalMessage = "Duplicate Error! Phone number already been used";
                                return true;
                            }
                        }
                        break;
                    
                }

                //msg = "";
                return false;
            }
            catch (Exception ex)
            {
                respStatus.Message.FriendlyMessage = respStatus.Message.TechnicalMessage = "Error: " + ex.Message;
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return true;
            }
        }

        private bool IsDuplicate(string phoneNo, string email, out string msg, int status = 0)
        {
            try
            {
                // Check Duplicate Phone No Duplicate
                List<ChurchMember> check;
                if (!string.IsNullOrEmpty(phoneNo))
                {
                    var sql1 =
                        string.Format(
                        "Select * FROM \"ChurchAPPDB\".\"ChurchMember\" WHERE \"MobileNumber\" = '{0}'", phoneNo);
                    check = _repository.RepositoryContext().Database.SqlQuery<ChurchMember>(sql1).ToList();
                }
                else
                {
                    check = null;
                }

                // Check Duplicate Email
                List<ChurchMember> check2;
                if (!string.IsNullOrEmpty(email))
                {
                    var sql2 =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ChurchMember\" WHERE \"Email\" = '{0}'", email);
                    check2 = _repository.RepositoryContext().Database.SqlQuery<ChurchMember>(sql2).ToList();
                }
                else
                {
                    check2 = null;
                }

                msg = "";
                if (check.IsNullOrEmpty() && (check2 == null || check2.IsNullOrEmpty())) return false;

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
                        if (check != null)
                        {
                            if (check.Count > 0)
                            {
                                msg = "Duplicate Error! Phone number already been used";
                                return true;
                            }
                        }
                        break;

                    case 1:
                        if (check2 != null)
                        {
                            if (check2.Count >= 1)
                            {
                                msg = "Duplicate Error! Email already exist";
                                return true;
                            }
                        }
                        if (check != null)
                        {
                            if (check.Count >= 1)
                            {
                                msg = "Duplicate Error! Phone number already been used";
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


        private bool IsDuplicatex(long clientId, string phoneNo, string email, out string msg, int status = 0)
        {
            try
            {

                // Check Duplicate Phone No Duplicate
                List<ChurchMember> check;
                if (!string.IsNullOrEmpty(phoneNo))
                {
                    var sql1 =
                        string.Format(
                        "Select * FROM \"ChurchAPPDB\".\"ChurchMember\" WHERE \"ClientId\" = {0} AND \"PhoneNumber\" = '{1}'",
                        clientId, phoneNo);
                    check = _repository.RepositoryContext().Database.SqlQuery<ChurchMember>(sql1).ToList();
                }
                else
                {
                    check = null;
                }

                // Check Duplicate Email
                List<ChurchMember> check2;
                if (!string.IsNullOrEmpty(email))
                {
                    var sql2 =
                        string.Format(
                            "Select * FROM \"ChurchAPPDB\".\"ChurchMember\" WHERE \"ClientId\" = {0} AND \"Email\" = '{1}'",
                            clientId, email);
                    check2 = _repository.RepositoryContext().Database.SqlQuery<ChurchMember>(sql2).ToList();
                }
                else
                {
                    check2 = null;
                }
                
                msg = "";
                if (check.IsNullOrEmpty() && (check2 == null || check2.IsNullOrEmpty())) return false;
                
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
                        if (check != null)
                        {
                            if (check.Count > 0)
                            {
                                msg = "Duplicate Error! Phone Number already been used by another Church";
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
                        if (check != null)
                        {
                            if (check.Count > 1)
                            {
                                msg = "Duplicate Error! Phone Number already been used by another Church";
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
    }
}
