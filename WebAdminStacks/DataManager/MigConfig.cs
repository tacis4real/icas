using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Helpers;
using WebAdminStacks.DataContract;
using WebAdminStacks.Repository.Helper;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace WebAdminStacks.DataManager
{
    internal partial class Configuration
    {
        public void ProcessSeed(WebAdminStackEntities context)
        {
            

            #region Admin Users

            if (!context.Roles.Any())
            {

                var role = new Role { Name = "PortalAdmin", Description = "Portal Admin Role" };
                context.Roles.AddOrUpdate(m => m.Name, role);
                context.SaveChanges();

                role = new Role { Name = "SiteAdmin", Description = "Site Admin Role" };
                context.Roles.AddOrUpdate(m => m.Name, role);
                context.SaveChanges();

                role = new Role { Name = "SiteUser", Description = "Site User Role" };
                context.Roles.AddOrUpdate(m => m.Name, role);
                context.SaveChanges();

                role = new Role { Name = "*", Description = "All Access" };
                context.Roles.AddOrUpdate(m => m.Name, role);
                context.SaveChanges();

            }

            LoadDefaultUser(context);
            if (!context.UserRoles.Any())
            {
                var userRole = new UserRole
                {
                    UserId = 1,
                    RoleId = 1
                };
                context.UserRoles.AddOrUpdate(m => m.UserId, userRole);
                context.SaveChanges();
                
            }

            #endregion

            #region Client Church

            if (!context.RoleClients.Any())
            {
                var roleClient = new RoleClient { Name = "Pastor", Description = "Pastor In Charge Role" };
                context.RoleClients.AddOrUpdate(m => m.Name, roleClient);
                context.SaveChanges();

                roleClient = new RoleClient { Name = "ChurchAdmin", Description = "Church Admin Role" };
                context.RoleClients.AddOrUpdate(m => m.Name, roleClient);
                context.SaveChanges();
            }
            
            
            #endregion

        }

        private void LoadDefaultUser(WebAdminStackEntities context)
        {
            if (!context.Users.Any())
            {
                try
                {
                    var password = Crypto.GenerateSalt();
                    var salt = EncryptionHelper.GenerateSalt(30, 50);
                    var accessCode = Crypto.HashPassword("Password1");

                    var defaultUser = new DataContract.User
                    {
                        Surname = "Epay",
                        Othernames = "Plus",
                        Email = "helpdesk@epayplusng.com",
                        AccessCode = accessCode,
                        IsFirstTimeLogin = true,
                        FailedPasswordCount = 0,
                        IsApproved = true,
                        IsDeleted = false,
                        IsLockedOut = false,
                        LastLockedOutTimeStamp = "",
                        IsMobileActive = true,
                        IsPasswordChangeRequired = true,
                        IsWebActive = true,
                        IsEmailVerified = true,
                        IsMobileNumberVerified = true,
                        LastLoginTimeStamp = "",
                        MobileNumber = "2348030000000",
                        RegisteredByUserId = 1,
                        Password = password,
                        Sex = 1,
                        PasswordChangeTimeStamp = "",
                        UserCode = salt,
                        TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                        Username = "EpayUser",
                    };
                    context.Users.AddOrUpdate(m => m.Username, defaultUser);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                }
            }
        }
       
    }
}
