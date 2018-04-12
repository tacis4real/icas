using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using ICASStacks.APIObjs;
using ICASStacks.DataContract;
using ICASStacks.Infrastructure;
using ICASStacks.Infrastructure.Contract;
using ICASStacks.Repository.Helpers;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.Repository
{
    internal class ChurchThemeSettingRepository
    {
        private readonly IIcasRepository<ChurchThemeSetting> _repository;
        private readonly IcasUoWork _uoWork;


        public ChurchThemeSettingRepository()
        {
            _uoWork = new IcasUoWork();
            _repository = new IcasRepository<ChurchThemeSetting>(_uoWork);
        }


        internal ChurchThemeSettingRegResponse AddChurchThemeSetting(ChurchThemeSettingRegObj churchThemeSettingRegObj)
        {

            var response = new ChurchThemeSettingRegResponse
            {
                ChurchThemeSettingId = 0,
                ChurchId = churchThemeSettingRegObj.ChurchId,
                ThemeColor = churchThemeSettingRegObj.ThemeColor,
                ThemeLogo = churchThemeSettingRegObj.ThemeLogo,
                Status = new RespStatus
                {
                    IsSuccessful = false,
                    Message = new RespMessage(),
                }
            };

            try
            {

                if (churchThemeSettingRegObj.Equals(null))
                {
                    response.Status.Message.FriendlyMessage = "Error Occurred! Unable to proceed with your registration";
                    response.Status.Message.TechnicalMessage = "Church registration Object is empty / invalid";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(churchThemeSettingRegObj, out valResults))
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
                if (churchThemeSettingRegObj.RegisteredByUserId < 1)
                {
                    response.Status.Message.FriendlyMessage = "Invalid Admin User Information";
                    response.Status.Message.TechnicalMessage = "Invalid Admin User Information";
                    response.Status.IsSuccessful = false;
                    return response;
                }

                var unique = new ChurchRepository().GetChurch(churchThemeSettingRegObj.ChurchId).ShortName ?? "";
                string logoPath = "~/ChurchLogos/";
                var logoName = churchThemeSettingRegObj.ThemeLogo;
                var ext = Path.GetExtension(logoName);
                
                var cleanFileName = logoName.Replace(" ", "_");
                cleanFileName = cleanFileName.Split('.')[0];
                cleanFileName = cleanFileName + "_" +unique +ext;

                var dirPath = HostingEnvironment.MapPath(logoPath);
                if (dirPath == null || !Directory.Exists(dirPath))
                {
                    var newDirPath = Directory.CreateDirectory(logoPath);
                    logoPath = newDirPath.ToString();
                }

                string msg;
                if (IsDuplicate(churchThemeSettingRegObj.ChurchId, out msg))
                {
                    response.Status.Message.FriendlyMessage = msg;
                    response.Status.Message.TechnicalMessage = msg;
                    response.Status.IsSuccessful = false;
                    return response;
                }

                var churchThemeSettingObj = new ChurchThemeSetting
                {
                    ChurchId = churchThemeSettingRegObj.ChurchId,
                    ThemeColor = churchThemeSettingRegObj.ThemeColor,
                    ThemeLogo = cleanFileName,
                    ThemeLogoPath = logoPath,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = churchThemeSettingRegObj.RegisteredByUserId,
                };

                var processedChurchTheme = _repository.Add(churchThemeSettingObj);
                _uoWork.SaveChanges();
                if (processedChurchTheme.ChurchThemeSettingId > 0)
                {
                    
                    // Write the church image logo to file
                    var relativePath = processedChurchTheme.ThemeLogoPath + processedChurchTheme.ThemeLogo;
                    var physicalPath = HostingEnvironment.MapPath(relativePath) ?? (processedChurchTheme.ThemeLogoPath + "/"+ processedChurchTheme.ThemeLogo);
                    churchThemeSettingRegObj.UploadedFile.SaveAs(physicalPath);


                    //if (Path.GetFileName(churchLogo.FileName) != null)
                    //{
                    //    cleanFileName = Path.GetFileName(churchLogo.FileName);
                    //    if (!string.IsNullOrEmpty(cleanFileName))
                    //    {
                    //        cleanFileName = cleanFileName.Replace(" ", "_");
                    //        var relativePath = logoPath + cleanFileName;
                    //        var physicalPath = Server.MapPath(relativePath);
                    //        churchLogo.SaveAs(physicalPath);
                    //    }

                    //}
                    
                    
                    
                    response.ChurchThemeSettingId = processedChurchTheme.ChurchThemeSettingId;
                    response.ChurchId = processedChurchTheme.ChurchId;
                    response.ThemeColor = processedChurchTheme.ThemeColor;
                    response.ThemeLogo = processedChurchTheme.ThemeLogo;
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

        internal RespStatus UpdateChurchThemeSetting(ChurchThemeSettingRegObj churchThemeSettingRegObj)
        {
            var response = new RespStatus
            {
                IsSuccessful = false,
                Message = new RespMessage()
            };
            try
            {
                if (churchThemeSettingRegObj.Equals(null))
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Error Occurred! Unable to proceed with your registration";
                    return response;
                }

                List<ValidationResult> valResults;
                if (!EntityValidatorHelper.Validate(churchThemeSettingRegObj, out valResults))
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
                var thisChurchThemeSetting = GetChurchThemeSetting(churchThemeSettingRegObj.ChurchThemeSettingId, out msg);
                if (thisChurchThemeSetting == null || thisChurchThemeSetting.ThemeLogo.Length < 1)
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = string.IsNullOrEmpty(msg) ? "Invalid User Information" : msg;
                    return response;
                }

                if (IsDuplicate(thisChurchThemeSetting.ChurchId, out msg, 1))
                {
                    response.Message.FriendlyMessage = msg;
                    response.Message.TechnicalMessage = msg;
                    response.IsSuccessful = false;
                    return response;
                }
                

                // Checking the new and existing file name, before splitting and add unique
                var cleanFileName = "";
                string logoPath = churchThemeSettingRegObj.ThemeLogoPath;
                if (churchThemeSettingRegObj.ThemeLogo != thisChurchThemeSetting.ThemeLogo)
                {
                    var unique = new ChurchRepository().GetChurch(churchThemeSettingRegObj.ChurchId).ShortName ?? "";
                    
                    var logoName = churchThemeSettingRegObj.ThemeLogo;
                    var ext = Path.GetExtension(logoName);

                    cleanFileName = logoName.Replace(" ", "_");
                    cleanFileName = cleanFileName.Split('.')[0];
                    cleanFileName = cleanFileName + "_" + unique + ext;
                }
                else
                {
                    cleanFileName = churchThemeSettingRegObj.ThemeLogo;
                }
                
                var dirPath = HostingEnvironment.MapPath(logoPath);
                if (dirPath == null || !Directory.Exists(dirPath))
                {
                    var newDirPath = Directory.CreateDirectory(logoPath);
                    logoPath = newDirPath.ToString();
                }

                thisChurchThemeSetting.ChurchId = churchThemeSettingRegObj.ChurchId;
                thisChurchThemeSetting.ThemeColor = churchThemeSettingRegObj.ThemeColor;
                thisChurchThemeSetting.ThemeLogo = cleanFileName;
                thisChurchThemeSetting.ThemeLogoPath = logoPath;


                var processedChurchTheme = _repository.Update(thisChurchThemeSetting);
                _uoWork.SaveChanges();

                if (processedChurchTheme.ChurchThemeSettingId < 1)
                {
                    response.Message.FriendlyMessage = response.Message.TechnicalMessage = "Process Failed! Please try again later";
                    return response;
                }


                // Write the church image logo to file, if its not existing
                var relativePath = processedChurchTheme.ThemeLogoPath + processedChurchTheme.ThemeLogo;
                var physicalPath = HostingEnvironment.MapPath(relativePath);

                if (!File.Exists(physicalPath) || churchThemeSettingRegObj.UploadedFile != null)
                {
                    var storePhysicalPath = physicalPath ?? (processedChurchTheme.ThemeLogoPath + "/" + processedChurchTheme.ThemeLogo);
                    churchThemeSettingRegObj.UploadedFile.SaveAs(storePhysicalPath);
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

        private ChurchThemeSetting GetChurchThemeSetting(long churchThemeSettingId, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ICASDB\".\"ChurchThemeSetting\"  WHERE \"ChurchThemeSettingId\" = {0};", churchThemeSettingId);

                var check = _repository.RepositoryContext().Database.SqlQuery<ChurchThemeSetting>(sql1).ToList();

                if (check.IsNullOrEmpty())
                {
                    msg = "No Church Theme Setting Record Found!";
                    return null;
                }
                if (check.Count != 1)
                {
                    msg = "Invalid Church Theme Setting Record!";
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


        internal ChurchThemeSetting GetChurchThemeSettingByChurchId(long churchId, out string msg)
        {
            try
            {
                var sql1 =
                 string.Format(
                     "Select * FROM \"ICASDB\".\"ChurchThemeSetting\"  WHERE \"ChurchId\" = {0};", churchId);

                var check = _repository.RepositoryContext().Database.SqlQuery<ChurchThemeSetting>(sql1).ToList();

                if (check.IsNullOrEmpty())
                {
                    msg = "No Church Theme Setting Record Found!";
                    return null;
                }
                if (check.Count != 1)
                {
                    msg = "Invalid Church Theme Setting Record!";
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


        internal List<RegisteredChurchThemeReportObj> GetAllRegisteredChurchThemeSettingObjs()
        {
            try
            {
                var myItemList = GetChurchThemeSettings();
                if (!myItemList.Any()) new List<RegisteredChurchThemeReportObj>();

                var retList = new List<RegisteredChurchThemeReportObj>();
                myItemList.ForEachx(m =>
                {
                    string msg;
                    var churchName = new ChurchRepository().GetChurch(m.ChurchId).Name;
                    retList.Add(new RegisteredChurchThemeReportObj
                    {
                        ChurchThemeSettingId = m.ChurchThemeSettingId,
                        ChurchId = m.ChurchId,
                        Church = churchName,
                        Color = m.ThemeColor,
                        Logo = m.ThemeLogo,
                        LogoPath = m.ThemeLogoPath,
                    });
                });

                return retList;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return new List<RegisteredChurchThemeReportObj>();
            }
        }


        internal List<ChurchThemeSetting> GetChurchThemeSettings()
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


        private bool IsDuplicate(long churchId, out string msg, int status = 0)
        {
            try
            {

                if (churchId < 1)
                {
                    msg = "No church theme setting record pass for checking";
                    return false;
                }

                // Check Duplicate Church No Duplicate
                var sql1 = string.Format("Select * FROM \"ChurchAPPDB\".\"ChurchThemeSetting\" WHERE \"ChurchId\" = {0}", churchId);
                var check = _repository.RepositoryContext().Database.SqlQuery<ChurchThemeSetting>(sql1).ToList();

                msg = "";
                if (check.IsNullOrEmpty()) return false;
                

                switch (status)
                {
                    case 0:
                        
                        if (check.Count > 0)
                        {
                            msg = "Duplicate Error! Church Theme Setting already exist";
                            return true;
                        }
                        break;

                    case 1:
                        
                        if (check.Count > 1)
                        {
                            msg = "Duplicate Error! Church Theme Setting already exist";
                            return true;
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
