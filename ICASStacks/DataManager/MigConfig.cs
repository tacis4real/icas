using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Web.Helpers;
using ICASStacks.APIObjs;
using ICASStacks.DataContract;
using ICASStacks.DataContract.BioEnroll;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.DataContract.Enum;
using ICASStacks.Repository.Helpers;
using Newtonsoft.Json;
using WebCribs.TechCracker.WebCribs.TechCracker;
using Role = WebAdminStacks.DataContract.Role;

namespace ICASStacks.DataManager
{
    
    internal partial class Configuration
    {
        public void ProcessedSeed(IcasDataContext context)
        {

            //#region Terminal API

            //LoadDefaultRoles(context);
            //LoadDefaultUserProfile(context);
            //LoadDefaultUser(context);

            //#endregion


            ProcessLookUps(context);
            ProcessStateLookUps(context);
            ProcessChurchLookUps(context);
            ProcessClientLookUps(context);
            ProcessClientChurchLookUps(context);
            ProcessChurchStructureTypes(context);
            ProcessChurchThemeLookUps(context);
            ProcessProfession(context);


            ProcessChurchServiceTypes(context);
            ProcessChurchService(context);
            ProcessCollectionTypes(context);

            ProcessChurchCollectionTypes(context);
            ProcessChurchRoleTypes(context);

            ProcessChurchStructure(context);
            //ProcessChurchServiceAttendance(context);

            ProcessChurchStructureParishHeadQuarter(context);
            //ProcessChurchService(context);
        }


        //#region Terminal API

        //private void LoadDefaultRoles(IcasDataContext context)
        //{
        //    if (!context.Roles.Any())
        //    {
        //        var role = new DataContract.BioEnroll.Role { Name = "PortalAdmin", Status = true };
        //        context.Roles.AddOrUpdate(m => m.Name, role);
        //        context.SaveChanges();

        //        role = new DataContract.BioEnroll.Role { Name = "SiteAdmin", Status = true };
        //        context.Roles.AddOrUpdate(m => m.Name, role);
        //        context.SaveChanges();

        //        role = new DataContract.BioEnroll.Role { Name = "SiteUser", Status = true };
        //        context.Roles.AddOrUpdate(m => m.Name, role);
        //        context.SaveChanges();

        //        role = new DataContract.BioEnroll.Role { Name = "*", Status = true };
        //        context.Roles.AddOrUpdate(m => m.Name, role);
        //        context.SaveChanges();

        //    }
        //}
        
        //private void LoadDefaultUserProfile(IcasDataContext context)
        //{
        //    if (!context.UserProfiles.Any())
        //    {
        //        try
        //        {
                    
        //            var defaultUser = new UserProfile
        //            {
        //                ClientStationId = 1,
        //                ProfileNumber = "0001", 
        //                Surname = "Ilesanmi",
        //                FirstName = "Adesoji",
        //                OtherNames = "",
        //                Sex = 1,
        //                ResidentialAddress = "Ajah Epayplus",
        //                Email = "useradmin1@epayplusng.com",
        //                MobileNumber = "08036975691",
        //                DateLastModified = DateScrutnizer.GetDateFromTimeStamp(DateScrutnizer.CurrentTimeStamp(), true),
        //                TimeLastModified = DateScrutnizer.GetTimeFromTimeStamp(DateScrutnizer.CurrentTimeStamp()),
        //                ModifiedBy = 1,
        //                Status = 1
        //            };
        //            context.UserProfiles.AddOrUpdate(m => m.ProfileNumber, defaultUser);
        //            context.SaveChanges();


        //            defaultUser = new UserProfile
        //            {
        //                ClientStationId = 2,
        //                ProfileNumber = "0002",
        //                Surname = "Adedunni",
        //                FirstName = "Rachael",
        //                OtherNames = "R.",
        //                Sex = 2,
        //                ResidentialAddress = "Lakowe Epayplus",
        //                Email = "useradmin2@epayplusng.com",
        //                MobileNumber = "08036975692",
        //                DateLastModified = DateScrutnizer.GetDateFromTimeStamp(DateScrutnizer.CurrentTimeStamp(), true),
        //                TimeLastModified = DateScrutnizer.GetTimeFromTimeStamp(DateScrutnizer.CurrentTimeStamp()),
        //                ModifiedBy = 1,
        //                Status = 1
        //            };
        //            context.UserProfiles.AddOrUpdate(m => m.ProfileNumber, defaultUser);
        //            context.SaveChanges();

        //            defaultUser = new UserProfile
        //            {
        //                ClientStationId = 2,
        //                ProfileNumber = "0003",
        //                Surname = "Okoro",
        //                FirstName = "Urefe",
        //                OtherNames = "O.",
        //                Sex = 1,
        //                ResidentialAddress = "Ibeju Epayplus",
        //                Email = "useradmin3@epayplusng.com",
        //                MobileNumber = "08036975693",
        //                DateLastModified = DateScrutnizer.GetDateFromTimeStamp(DateScrutnizer.CurrentTimeStamp(), true),
        //                TimeLastModified = DateScrutnizer.GetTimeFromTimeStamp(DateScrutnizer.CurrentTimeStamp()),
        //                ModifiedBy = 1,
        //                Status = 1
        //            };
        //            context.UserProfiles.AddOrUpdate(m => m.ProfileNumber, defaultUser);
        //            context.SaveChanges();

        //            defaultUser = new UserProfile
        //            {
        //                ClientStationId = 2,
        //                ProfileNumber = "0004",
        //                Surname = "Joy",
        //                FirstName = "Michael",
        //                OtherNames = "",
        //                Sex = 2,
        //                ResidentialAddress = "Lekki Epayplus",
        //                Email = "useradmin4@epayplusng.com",
        //                MobileNumber = "08036975694",
        //                DateLastModified = DateScrutnizer.GetDateFromTimeStamp(DateScrutnizer.CurrentTimeStamp(), true),
        //                TimeLastModified = DateScrutnizer.GetTimeFromTimeStamp(DateScrutnizer.CurrentTimeStamp()),
        //                ModifiedBy = 1,
        //                Status = 1
        //            };
        //            context.UserProfiles.AddOrUpdate(m => m.ProfileNumber, defaultUser);
        //            context.SaveChanges();

        //            defaultUser = new UserProfile
        //            {
        //                ClientStationId = 1,
        //                ProfileNumber = "0005",
        //                Surname = "Akande",
        //                FirstName = "Michael",
        //                OtherNames = "O.",
        //                Sex = 1,
        //                ResidentialAddress = "Adeba Epayplus",
        //                Email = "useradmin5@epayplusng.com",
        //                MobileNumber = "08036975695",
        //                DateLastModified = DateScrutnizer.GetDateFromTimeStamp(DateScrutnizer.CurrentTimeStamp(), true),
        //                TimeLastModified = DateScrutnizer.GetTimeFromTimeStamp(DateScrutnizer.CurrentTimeStamp()),
        //                ModifiedBy = 1,
        //                Status = 1
        //            };
        //            context.UserProfiles.AddOrUpdate(m => m.ProfileNumber, defaultUser);
        //            context.SaveChanges();

        //            defaultUser = new UserProfile
        //            {
        //                ClientStationId = 1,
        //                ProfileNumber = "0006",
        //                Surname = "Gloria",
        //                FirstName = "Jane",
        //                OtherNames = "O.",
        //                Sex = 2,
        //                ResidentialAddress = "Ibadan Epayplus",
        //                Email = "useradmin6@epayplusng.com",
        //                MobileNumber = "08036975696",
        //                DateLastModified = DateScrutnizer.GetDateFromTimeStamp(DateScrutnizer.CurrentTimeStamp(), true),
        //                TimeLastModified = DateScrutnizer.GetTimeFromTimeStamp(DateScrutnizer.CurrentTimeStamp()),
        //                ModifiedBy = 1,
        //                Status = 1
        //            };
        //            context.UserProfiles.AddOrUpdate(m => m.ProfileNumber, defaultUser);
        //            context.SaveChanges();

        //            defaultUser = new UserProfile
        //            {
        //                ClientStationId = 1,
        //                ProfileNumber = "0007",
        //                Surname = "Makinde",
        //                FirstName = "Owolabi",
        //                OtherNames = "",
        //                Sex = 1,
        //                ResidentialAddress = "Arepo Epayplus",
        //                Email = "useradmin7@epayplusng.com",
        //                MobileNumber = "08036975697",
        //                DateLastModified = DateScrutnizer.GetDateFromTimeStamp(DateScrutnizer.CurrentTimeStamp(), true),
        //                TimeLastModified = DateScrutnizer.GetTimeFromTimeStamp(DateScrutnizer.CurrentTimeStamp()),
        //                ModifiedBy = 1,
        //                Status = 1
        //            };
        //            context.UserProfiles.AddOrUpdate(m => m.ProfileNumber, defaultUser);
        //            context.SaveChanges();

        //            defaultUser = new UserProfile
        //            {
        //                ClientStationId = 1,
        //                ProfileNumber = "0008",
        //                Surname = "John",
        //                FirstName = "Udoh",
        //                OtherNames = "",
        //                Sex = 1,
        //                ResidentialAddress = "Ogba Epayplus",
        //                Email = "useradmin8@epayplusng.com",
        //                MobileNumber = "08036975698",
        //                DateLastModified = DateScrutnizer.GetDateFromTimeStamp(DateScrutnizer.CurrentTimeStamp(), true),
        //                TimeLastModified = DateScrutnizer.GetTimeFromTimeStamp(DateScrutnizer.CurrentTimeStamp()),
        //                ModifiedBy = 1,
        //                Status = 1
        //            };
        //            context.UserProfiles.AddOrUpdate(m => m.ProfileNumber, defaultUser);
        //            context.SaveChanges();

        //            defaultUser = new UserProfile
        //            {
        //                ClientStationId = 2,
        //                ProfileNumber = "0009",
        //                Surname = "Magret",
        //                FirstName = "Obodo",
        //                OtherNames = "J.",
        //                Sex = 2,
        //                ResidentialAddress = "Ikeja Epayplus",
        //                Email = "useradmin9@epayplusng.com",
        //                MobileNumber = "08036975699",
        //                DateLastModified = DateScrutnizer.GetDateFromTimeStamp(DateScrutnizer.CurrentTimeStamp(), true),
        //                TimeLastModified = DateScrutnizer.GetTimeFromTimeStamp(DateScrutnizer.CurrentTimeStamp()),
        //                ModifiedBy = 1,
        //                Status = 1
        //            };
        //            context.UserProfiles.AddOrUpdate(m => m.ProfileNumber, defaultUser);
        //            context.SaveChanges();

        //            defaultUser = new UserProfile
        //            {
        //                ClientStationId = 2,
        //                ProfileNumber = "00010",
        //                Surname = "Neil",
        //                FirstName = "Clara",
        //                OtherNames = "J.",
        //                Sex = 2,
        //                ResidentialAddress = "Maryland Epayplus",
        //                Email = "useradmin10@epayplusng.com",
        //                MobileNumber = "08036975690",
        //                DateLastModified = DateScrutnizer.GetDateFromTimeStamp(DateScrutnizer.CurrentTimeStamp(), true),
        //                TimeLastModified = DateScrutnizer.GetTimeFromTimeStamp(DateScrutnizer.CurrentTimeStamp()),
        //                ModifiedBy = 1,
        //                Status = 1
        //            };
        //            context.UserProfiles.AddOrUpdate(m => m.ProfileNumber, defaultUser);
        //            context.SaveChanges();

        //        }
        //        catch (Exception ex)
        //        {
        //            BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
        //        }
        //    }
        //}
        //private void LoadDefaultUser(IcasDataContext context)
        //{
        //    if (!context.StaffUsers.Any())
        //    {
        //        try
        //        {
        //            var password = Crypto.GenerateSalt();
        //            var salt = EncryptionHelper.GenerateSalt(30, 50);
        //            var accessCode = Crypto.HashPassword("EPP_User1");

        //            var defaultUser = new StaffUser
        //            {
        //                UserProfileId = 1,
        //                UserName = "useradmin1",
        //                Email = "useradmin1@epayplusng.com",
        //                IsFirstTimeLogin = true,
        //                FailedPasswordAttemptCount = 0,
        //                IsApproved = false,
        //                IsLockedOut = false,
        //                LastLockedOutTimeStamp = "",
        //                LastLoginTimeStamp = "",
        //                LastPasswordChangedTimeStamp = "",
        //                Password = password,
        //                RegisteredDateTimeStamp = DateScrutnizer.CurrentTimeStamp(),
        //                RoleId = 1,
        //                Salt = salt,
        //                UserCode = accessCode
        //            };
        //            context.StaffUsers.AddOrUpdate(m => m.UserName, defaultUser);
        //            context.SaveChanges();


        //            password = Crypto.GenerateSalt();
        //            salt = EncryptionHelper.GenerateSalt(30, 50);
        //            accessCode = Crypto.HashPassword("EPP_User2");
        //            defaultUser = new StaffUser
        //            {
        //                UserProfileId = 2,
        //                UserName = "useradmin2",
        //                Email = "useradmin2@epayplusng.com",
        //                IsFirstTimeLogin = true,
        //                FailedPasswordAttemptCount = 0,
        //                IsApproved = false,
        //                IsLockedOut = false,
        //                LastLockedOutTimeStamp = "",
        //                LastLoginTimeStamp = "",
        //                LastPasswordChangedTimeStamp = "",
        //                Password = password,
        //                RegisteredDateTimeStamp = DateScrutnizer.CurrentTimeStamp(),
        //                RoleId = 1,
        //                Salt = salt,
        //                UserCode = accessCode
        //            };
        //            context.StaffUsers.AddOrUpdate(m => m.UserName, defaultUser);
        //            context.SaveChanges();

        //            password = Crypto.GenerateSalt();
        //            salt = EncryptionHelper.GenerateSalt(30, 50);
        //            accessCode = Crypto.HashPassword("EPP_User3");
        //            defaultUser = new StaffUser
        //            {
        //                UserProfileId = 3,
        //                UserName = "useradmin3",
        //                Email = "useradmin3@epayplusng.com",
        //                IsFirstTimeLogin = true,
        //                FailedPasswordAttemptCount = 0,
        //                IsApproved = false,
        //                IsLockedOut = false,
        //                LastLockedOutTimeStamp = "",
        //                LastLoginTimeStamp = "",
        //                LastPasswordChangedTimeStamp = "",
        //                Password = password,
        //                RegisteredDateTimeStamp = DateScrutnizer.CurrentTimeStamp(),
        //                RoleId = 1,
        //                Salt = salt,
        //                UserCode = accessCode
        //            };
        //            context.StaffUsers.AddOrUpdate(m => m.UserName, defaultUser);
        //            context.SaveChanges();

        //            password = Crypto.GenerateSalt();
        //            salt = EncryptionHelper.GenerateSalt(30, 50);
        //            accessCode = Crypto.HashPassword("EPP_User4");
        //            defaultUser = new StaffUser
        //            {
        //                UserProfileId = 4,
        //                UserName = "useradmin4",
        //                Email = "useradmin4@epayplusng.com",
        //                IsFirstTimeLogin = true,
        //                FailedPasswordAttemptCount = 0,
        //                IsApproved = false,
        //                IsLockedOut = false,
        //                LastLockedOutTimeStamp = "",
        //                LastLoginTimeStamp = "",
        //                LastPasswordChangedTimeStamp = "",
        //                Password = password,
        //                RegisteredDateTimeStamp = DateScrutnizer.CurrentTimeStamp(),
        //                RoleId = 1,
        //                Salt = salt,
        //                UserCode = accessCode
        //            };
        //            context.StaffUsers.AddOrUpdate(m => m.UserName, defaultUser);
        //            context.SaveChanges();

        //            password = Crypto.GenerateSalt();
        //            salt = EncryptionHelper.GenerateSalt(30, 50);
        //            accessCode = Crypto.HashPassword("EPP_User5");
        //            defaultUser = new StaffUser
        //            {
        //                UserProfileId = 5,
        //                UserName = "useradmin5",
        //                Email = "useradmin5@epayplusng.com",
        //                IsFirstTimeLogin = true,
        //                FailedPasswordAttemptCount = 0,
        //                IsApproved = false,
        //                IsLockedOut = false,
        //                LastLockedOutTimeStamp = "",
        //                LastLoginTimeStamp = "",
        //                LastPasswordChangedTimeStamp = "",
        //                Password = password,
        //                RegisteredDateTimeStamp = DateScrutnizer.CurrentTimeStamp(),
        //                RoleId = 1,
        //                Salt = salt,
        //                UserCode = accessCode
        //            };
        //            context.StaffUsers.AddOrUpdate(m => m.UserName, defaultUser);
        //            context.SaveChanges();

        //            password = Crypto.GenerateSalt();
        //            salt = EncryptionHelper.GenerateSalt(30, 50);
        //            accessCode = Crypto.HashPassword("EPP_User6");
        //            defaultUser = new StaffUser
        //            {
        //                UserProfileId = 6,
        //                UserName = "useradmin6",
        //                Email = "useradmin6@epayplusng.com",
        //                IsFirstTimeLogin = true,
        //                FailedPasswordAttemptCount = 0,
        //                IsApproved = false,
        //                IsLockedOut = false,
        //                LastLockedOutTimeStamp = "",
        //                LastLoginTimeStamp = "",
        //                LastPasswordChangedTimeStamp = "",
        //                Password = password,
        //                RegisteredDateTimeStamp = DateScrutnizer.CurrentTimeStamp(),
        //                RoleId = 1,
        //                Salt = salt,
        //                UserCode = accessCode
        //            };
        //            context.StaffUsers.AddOrUpdate(m => m.UserName, defaultUser);
        //            context.SaveChanges();

        //            password = Crypto.GenerateSalt();
        //            salt = EncryptionHelper.GenerateSalt(30, 50);
        //            accessCode = Crypto.HashPassword("EPP_User7");
        //            defaultUser = new StaffUser
        //            {
        //                UserProfileId = 7,
        //                UserName = "useradmin7",
        //                Email = "useradmin7@epayplusng.com",
        //                IsFirstTimeLogin = true,
        //                FailedPasswordAttemptCount = 0,
        //                IsApproved = false,
        //                IsLockedOut = false,
        //                LastLockedOutTimeStamp = "",
        //                LastLoginTimeStamp = "",
        //                LastPasswordChangedTimeStamp = "",
        //                Password = password,
        //                RegisteredDateTimeStamp = DateScrutnizer.CurrentTimeStamp(),
        //                RoleId = 1,
        //                Salt = salt,
        //                UserCode = accessCode
        //            };
        //            context.StaffUsers.AddOrUpdate(m => m.UserName, defaultUser);
        //            context.SaveChanges();

        //            password = Crypto.GenerateSalt();
        //            salt = EncryptionHelper.GenerateSalt(30, 50);
        //            accessCode = Crypto.HashPassword("EPP_User8");
        //            defaultUser = new StaffUser
        //            {
        //                UserProfileId = 8,
        //                UserName = "useradmin8",
        //                Email = "useradmin8@epayplusng.com",
        //                IsFirstTimeLogin = true,
        //                FailedPasswordAttemptCount = 0,
        //                IsApproved = false,
        //                IsLockedOut = false,
        //                LastLockedOutTimeStamp = "",
        //                LastLoginTimeStamp = "",
        //                LastPasswordChangedTimeStamp = "",
        //                Password = password,
        //                RegisteredDateTimeStamp = DateScrutnizer.CurrentTimeStamp(),
        //                RoleId = 1,
        //                Salt = salt,
        //                UserCode = accessCode
        //            };
        //            context.StaffUsers.AddOrUpdate(m => m.UserName, defaultUser);
        //            context.SaveChanges();

        //            password = Crypto.GenerateSalt();
        //            salt = EncryptionHelper.GenerateSalt(30, 50);
        //            accessCode = Crypto.HashPassword("EPP_User9");
        //            defaultUser = new StaffUser
        //            {
        //                UserProfileId = 9,
        //                UserName = "useradmin9",
        //                Email = "useradmin9@epayplusng.com",
        //                IsFirstTimeLogin = true,
        //                FailedPasswordAttemptCount = 0,
        //                IsApproved = false,
        //                IsLockedOut = false,
        //                LastLockedOutTimeStamp = "",
        //                LastLoginTimeStamp = "",
        //                LastPasswordChangedTimeStamp = "",
        //                Password = password,
        //                RegisteredDateTimeStamp = DateScrutnizer.CurrentTimeStamp(),
        //                RoleId = 1,
        //                Salt = salt,
        //                UserCode = accessCode
        //            };
        //            context.StaffUsers.AddOrUpdate(m => m.UserName, defaultUser);
        //            context.SaveChanges();

        //            password = Crypto.GenerateSalt();
        //            salt = EncryptionHelper.GenerateSalt(30, 50);
        //            accessCode = Crypto.HashPassword("EPP_User10");
        //            defaultUser = new StaffUser
        //            {
        //                UserProfileId = 10,
        //                UserName = "useradmin10",
        //                Email = "useradmin10@epayplusng.com",
        //                IsFirstTimeLogin = true,
        //                FailedPasswordAttemptCount = 0,
        //                IsApproved = false,
        //                IsLockedOut = false,
        //                LastLockedOutTimeStamp = "",
        //                LastLoginTimeStamp = "",
        //                LastPasswordChangedTimeStamp = "",
        //                Password = password,
        //                RegisteredDateTimeStamp = DateScrutnizer.CurrentTimeStamp(),
        //                RoleId = 1,
        //                Salt = salt,
        //                UserCode = accessCode
        //            };
        //            context.StaffUsers.AddOrUpdate(m => m.UserName, defaultUser);
        //            context.SaveChanges();


        //        }
        //        catch (Exception ex)
        //        {
        //            BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
        //        }
        //    }
        //}

        //#endregion



        #region ICAS

        private void ProcessLookUps(IcasDataContext context)
        {
            if (!context.Banks.Any())
            {
                var bank = new Bank { Name = "Access Bank", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Citibank", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Diamond Bank", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Ecobank Nigeria", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Fidelity Bank Nigeria", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "First Bank of Nigeria", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "First City Monument Bank", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "FSDH Merchant Bank", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Guaranty Trust Bank", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Heritage Bank Plc.", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Keystone Bank Limited", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Rand Merchant Bank", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Skye Bank", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Stanbic IBTC Bank Nigeria Limited", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Standard Chartered Bank", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Sterling Bank", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Suntrust Bank Nigeria Limited", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Union Bank of Nigeria", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "United Bank for Africa", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Unity Bank Plc", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Wema Bank", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Zenith Bank", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();

                bank = new Bank { Name = "Jaiz Bank", Status = true, RegisteredBy = 1 };
                context.Banks.AddOrUpdate(m => m.Name, bank);
                context.SaveChanges();
            }

        }
        private void ProcessStateLookUps(IcasDataContext context)
        {
            if (!context.StateOfLocations.Any())
            {
                var state = new StateOfLocation { Name = "Abia" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Adamawa" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Anambra" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Akwa Ibom" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Bauchi" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Bayelsa" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Benue" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Borno" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Cross River" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Delta" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Ebonyi" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Enugu" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Edo" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Ekiti" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Gombe" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Imo" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Jigawa" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Kaduna" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Kano" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Katsina" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Kebbi" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Kogi" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Kwara" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Lagos" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Nasarawa" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Niger" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Ogun" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Ondo" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Osun" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Oyo" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Plateau" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Rivers" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Sokoto" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Taraba" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "Yobe" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

                state = new StateOfLocation { Name = "FCT Abuja" };
                context.StateOfLocations.AddOrUpdate(m => m.Name, state);
                context.SaveChanges();

            }

        }
        private void ProcessChurchLookUps(IcasDataContext context)
        {
            if (!context.Churches.Any())
            {

                #region App Church
                var church = new Church
                {
                    Name = "Simple Tech Church of God",
                    ShortName = "STCG",
                    Founder = "Pastor S. P. Tech",
                    Address = "3, Simple Tech, Avenue, Lagos St.",
                    PhoneNumber = "08036000000",
                    Email = "",
                    StateOfLocationId = 24,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1
                };
                context.Churches.AddOrUpdate(m => m.Name, church);
                context.SaveChanges();

                church = new Church
                {
                    Name = "Epay Church of God",
                    ShortName = "EPCG",
                    Founder = "Pastor E. P. Electronic",
                    Address = "Km 34, Ibeju-Lekki Exp Way, Lakowe, Lagos St.",
                    PhoneNumber = "08056458843",
                    Email = "",
                    StateOfLocationId = 24,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1
                };
                context.Churches.AddOrUpdate(m => m.Name, church);
                context.SaveChanges();
                #endregion

                #region RCCG - 3
                church = new Church
                {
                    Name = "Reedem Christain Church of God",
                    ShortName = "RCCG",
                    Founder = "Pastor E. A. Adeboye",
                    Address = "Km 54, Lagos-Ibadan Exp Way, Camp Ground, Ogun St.",
                    PhoneNumber = "09023458843",
                    Email = "",
                    StateOfLocationId = 27,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1
                };
                context.Churches.AddOrUpdate(m => m.Name, church);
                context.SaveChanges();
                #endregion

                #region MFM  - 4
                church = new Church
                {
                    Name = "Mountain of Fire & Miracle Ministry",
                    ShortName = "MFM",
                    Founder = "Dr. D. K. Olukoya",
                    Address = "13, Olasimbo Str, Onike, Iwaya, Sabo, Lagos",
                    PhoneNumber = "09035451243",
                    Email = "",
                    StateOfLocationId = 24,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1
                };
                context.Churches.AddOrUpdate(m => m.Name, church);
                context.SaveChanges();
                #endregion

                #region Dipper LIFE  - 5
                church = new Church
                {
                    Name = "Deeper Christian Life Ministry",
                    ShortName = "DCLM",
                    Founder = "Pastor W. F. Kumuyi",
                    Address = "2-10 Ayodele Okeowo Street, Gbagada",
                    PhoneNumber = "08177456036",
                    Email = "info@deeperlifeonline.org",
                    StateOfLocationId = 24,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1
                };
                context.Churches.AddOrUpdate(m => m.Name, church);
                context.SaveChanges();
                #endregion

                #region 4 SQUARE - 6
                church = new Church
                {
                    Name = "Foursquare Gospel Church Ngeria",
                    ShortName = "FGCN",
                    Founder = "Rev. Felix Meduoye",
                    Address = "62/66, Akinwunmi Street, Yaba",
                    PhoneNumber = "08179153727",
                    Email = "",
                    StateOfLocationId = 24,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1
                };
                context.Churches.AddOrUpdate(m => m.Name, church);
                context.SaveChanges();
                #endregion

                #region Catholic - 7
                church = new Church
                {
                    Name = "Catholic Church Nigeria",
                    ShortName = "CCN",
                    Founder = "Arch. Ignatius Ayau Kaigama",
                    Address = "Plot 459, Cadastral Zone B2, Southern Parkway, Durumi 1",
                    PhoneNumber = "09025239413",
                    Email = "",
                    StateOfLocationId = 36,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1
                };
                context.Churches.AddOrUpdate(m => m.Name, church);
                context.SaveChanges();
                #endregion

                #region Winners - 8
                church = new Church
                {
                    Name = "Living Faith Church",
                    ShortName = "LFC",
                    Founder = "Pastor David Oyedepo",
                    Address = "Km. 10, Idiroko Road, Ota",
                    PhoneNumber = "08031451243",
                    Email = "info@lfcww.org",
                    StateOfLocationId = 27,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1
                };
                context.Churches.AddOrUpdate(m => m.Name, church);
                context.SaveChanges();
                #endregion

                #region CAC - 9
                church = new Church
                {
                    Name = "Christ Apostolic Church",
                    ShortName = "CAC",
                    Founder = "Joseph Ayo Babalola",
                    Address = "Radio Nigeria Bus Stop, Anlugbua, Basorun, Ibadan",
                    PhoneNumber = "08067226220",
                    Email = "",
                    StateOfLocationId = 30,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1
                };
                context.Churches.AddOrUpdate(m => m.Name, church);
                context.SaveChanges();
                #endregion

                #region CCC - Celestial - 10
                church = new Church
                {
                    Name = "Celestial Church of Christ",
                    ShortName = "CCC",
                    Founder = "Rev. Samuel Biléhou Joseph Oshoffa",
                    Address = "2 Church St, Makoko, Lagos",
                    PhoneNumber = "09085239413",
                    Email = "",
                    StateOfLocationId = 24,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1
                };
                context.Churches.AddOrUpdate(m => m.Name, church);
                context.SaveChanges();
                #endregion

                #region TAC - 11
                church = new Church
                {
                    Name = "The Apostolic Church Nigeria",
                    ShortName = "TAC",
                    Founder = "Pastors D.P. Williams",
                    Address = "Olorunda, Ketu, Oworonsoki Expressway, Lagos, Nigeria",
                    PhoneNumber = "01-2885026",
                    Email = "info@tac-lawna.org",
                    StateOfLocationId = 24,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1
                };
                context.Churches.AddOrUpdate(m => m.Name, church);
                context.SaveChanges();
                #endregion

                #region 7th Day - 12
                church = new Church
                {
                    Name = "Seventh-Day Adventist Church",
                    ShortName = "SAC",
                    Founder = "David C. Babcock",
                    Address = "7th Day Adventist Church National Hqtr",
                    PhoneNumber = "08032347868",
                    Email = "",
                    StateOfLocationId = 24,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1
                };
                context.Churches.AddOrUpdate(m => m.Name, church);
                context.SaveChanges();
                #endregion

                #region Apostolic Faith - 13
                church = new Church
                {
                    Name = "The Apostolic Faith Church",
                    ShortName = "TAFC",
                    Founder = "Timothy Oshokoya",
                    Address = "Camp Ground Road, Anthony Village, Lagos",
                    PhoneNumber = "08033800179",
                    Email = "",
                    StateOfLocationId = 24,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1
                };
                context.Churches.AddOrUpdate(m => m.Name, church);
                context.SaveChanges();
                #endregion

            }

        }


        private void ProcessChurchStructureTypes(IcasDataContext context)
        {
            if (!context.ChurchStructureTypes.Any())
            {

                // National HeadQuarter - 1, Region - 2, Diocese - 3, Special District - 4, State - 5, Group - 6, District - 7, 
                // Arch Diocese - 8, Zone - 9, Area - 10,Assemblies - 11, Branch - 12, Parish - 13, Deanery - 14, Out Station - 15, Province - 16

                #region National HeadQuarter - 1
                var churchStructureType = new ChurchStructureType
                {
                    Name = "National HeadQuarter",
                    Status = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ChurchStructureTypes.AddOrUpdate(m => m.Name, churchStructureType);
                context.SaveChanges();
                #endregion

                #region Region - 2
                churchStructureType = new ChurchStructureType
                {
                    Name = "Region",
                    Status = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ChurchStructureTypes.AddOrUpdate(m => m.Name, churchStructureType);
                context.SaveChanges();
                #endregion

                #region Diocese - 3
                churchStructureType = new ChurchStructureType
                {
                    Name = "Diocese",
                    Status = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ChurchStructureTypes.AddOrUpdate(m => m.Name, churchStructureType);
                context.SaveChanges();
                #endregion

                #region Special District - 4
                churchStructureType = new ChurchStructureType
                {
                    Name = "Special District",
                    Status = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ChurchStructureTypes.AddOrUpdate(m => m.Name, churchStructureType);
                context.SaveChanges();
                #endregion

                #region State - 5
                churchStructureType = new ChurchStructureType
                {
                    Name = "State",
                    Status = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ChurchStructureTypes.AddOrUpdate(m => m.Name, churchStructureType);
                context.SaveChanges();
                #endregion

                #region Group - 6
                churchStructureType = new ChurchStructureType
                {
                    Name = "Group",
                    Status = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ChurchStructureTypes.AddOrUpdate(m => m.Name, churchStructureType);
                context.SaveChanges();
                #endregion

                #region District - 7
                churchStructureType = new ChurchStructureType
                {
                    Name = "District",
                    Status = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ChurchStructureTypes.AddOrUpdate(m => m.Name, churchStructureType);
                context.SaveChanges();
                #endregion

                #region Arch Diocese - 8
                churchStructureType = new ChurchStructureType
                {
                    Name = "Arch Diocese",
                    Status = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ChurchStructureTypes.AddOrUpdate(m => m.Name, churchStructureType);
                context.SaveChanges();
                #endregion

                #region Zone - 9
                churchStructureType = new ChurchStructureType
                {
                    Name = "Zone",
                    Status = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ChurchStructureTypes.AddOrUpdate(m => m.Name, churchStructureType);
                context.SaveChanges();
                #endregion

                #region Area - 10
                churchStructureType = new ChurchStructureType
                {
                    Name = "Area",
                    Status = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ChurchStructureTypes.AddOrUpdate(m => m.Name, churchStructureType);
                context.SaveChanges();
                #endregion

                #region Assemblies - 11
                churchStructureType = new ChurchStructureType
                {
                    Name = "Assemblies",
                    Status = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ChurchStructureTypes.AddOrUpdate(m => m.Name, churchStructureType);
                context.SaveChanges();
                #endregion

                #region Branch - 12
                churchStructureType = new ChurchStructureType
                {
                    Name = "Branch",
                    Status = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ChurchStructureTypes.AddOrUpdate(m => m.Name, churchStructureType);
                context.SaveChanges();
                #endregion

                #region Parish - 13
                churchStructureType = new ChurchStructureType
                {
                    Name = "Parish",
                    Status = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ChurchStructureTypes.AddOrUpdate(m => m.Name, churchStructureType);
                context.SaveChanges();
                #endregion

                #region Deanery - 14
                churchStructureType = new ChurchStructureType
                {
                    Name = "Deanery",
                    Status = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ChurchStructureTypes.AddOrUpdate(m => m.Name, churchStructureType);
                context.SaveChanges();
                #endregion

                #region Out Station - 15
                churchStructureType = new ChurchStructureType
                {
                    Name = "Out Station",
                    Status = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ChurchStructureTypes.AddOrUpdate(m => m.Name, churchStructureType);
                context.SaveChanges();
                #endregion

                #region Province - 16
                churchStructureType = new ChurchStructureType
                {
                    Name = "Province",
                    Status = 1,
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ChurchStructureTypes.AddOrUpdate(m => m.Name, churchStructureType);
                context.SaveChanges();
                #endregion
            }

        }

        private void ProcessChurchStructure(IcasDataContext context)
        {
            if (!context.ChurchStructures.Any())
            {

                // National HeadQuarter - 1, Region - 2, Zone - 9, Area - 10, Parish - 13, Province - 16

                #region EPCG - 2
                var churchStructure = new ChurchStructure
                {
                    ChurchId = 2,
                    ChurchStructureTypeDetail = new List<ChurchStructureTypeDetailObj>
                    {
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 1, ChurchStructureTypeName = "National Headquarter", HierachyLevel = 1 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 2, ChurchStructureTypeName = "Region", HierachyLevel = 2 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 16, ChurchStructureTypeName = "Province", HierachyLevel = 3 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 9, ChurchStructureTypeName = "Zone", HierachyLevel = 4 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 10, ChurchStructureTypeName = "Area", HierachyLevel = 5 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 13, ChurchStructureTypeName = "Parish", HierachyLevel = 6 },
                    },
                    LastModificationTimeStamp = DateScrutnizer.CurrentTimeStamp(),
                    LastModificationByUserId = 1,
                    RegisteredByUserId = 1,
                    Status = ChurchStructureStatus.Active,
                };
                context.ChurchStructures.AddOrUpdate(m => m.ChurchStructureId, churchStructure);
                context.SaveChanges();
                #endregion

                #region RCCG - 3
                churchStructure = new ChurchStructure
                {
                    ChurchId = 3,
                    ChurchStructureTypeDetail = new List<ChurchStructureTypeDetailObj>
                    {
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 1, ChurchStructureTypeName = "National Headquarter", HierachyLevel = 1 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 2, ChurchStructureTypeName = "Region", HierachyLevel = 2 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 16, ChurchStructureTypeName = "Province", HierachyLevel = 3 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 9, ChurchStructureTypeName = "Zone", HierachyLevel = 4 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 10, ChurchStructureTypeName = "Area", HierachyLevel = 5 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 13, ChurchStructureTypeName = "Parish", HierachyLevel = 6 },
                    },
                    LastModificationTimeStamp = DateScrutnizer.CurrentTimeStamp(),
                    LastModificationByUserId = 1,
                    RegisteredByUserId = 1,
                    Status = ChurchStructureStatus.Active,
                };
                context.ChurchStructures.AddOrUpdate(m => m.ChurchStructureId, churchStructure);
                context.SaveChanges();
                #endregion

                #region MFM - 4
                churchStructure = new ChurchStructure
                {
                    ChurchId = 4,
                    ChurchStructureTypeDetail = new List<ChurchStructureTypeDetailObj>
                    {
                        new ChurchStructureTypeDetailObj { ChurchStructureTypeId = 1, ChurchStructureTypeName = "National Headquarter", HierachyLevel = 1 },
                        new ChurchStructureTypeDetailObj { ChurchStructureTypeId = 2, ChurchStructureTypeName = "Region", HierachyLevel = 2 },
                        new ChurchStructureTypeDetailObj { ChurchStructureTypeId = 9, ChurchStructureTypeName = "Zone", HierachyLevel = 3 },
                        new ChurchStructureTypeDetailObj { ChurchStructureTypeId = 12, ChurchStructureTypeName = "Branch", HierachyLevel = 4 },
                    },
                    LastModificationTimeStamp = DateScrutnizer.CurrentTimeStamp(),
                    LastModificationByUserId = 1,
                    RegisteredByUserId = 1,
                    Status = ChurchStructureStatus.Active,
                };
                context.ChurchStructures.AddOrUpdate(m => m.ChurchStructureId, churchStructure);
                context.SaveChanges();
                #endregion

                #region DCLM - 5
                churchStructure = new ChurchStructure
                {
                    ChurchId = 5,
                    ChurchStructureTypeDetail = new List<ChurchStructureTypeDetailObj>
                    {
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 1, ChurchStructureTypeName = "National Headquarter", HierachyLevel = 1 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 2, ChurchStructureTypeName = "Region", HierachyLevel = 3 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 5, ChurchStructureTypeName = "State", HierachyLevel = 2 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 6, ChurchStructureTypeName = "Group", HierachyLevel = 4 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 7, ChurchStructureTypeName = "District", HierachyLevel = 5 },
                    },
                    LastModificationTimeStamp = DateScrutnizer.CurrentTimeStamp(),
                    LastModificationByUserId = 1,
                    RegisteredByUserId = 1,
                    Status = ChurchStructureStatus.Active,
                };
                context.ChurchStructures.AddOrUpdate(m => m.ChurchStructureId, churchStructure);
                context.SaveChanges();
                #endregion

                #region FGCN - 6
                churchStructure = new ChurchStructure
                {
                    ChurchId = 6,
                    ChurchStructureTypeDetail = new List<ChurchStructureTypeDetailObj>
                    {
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 1, ChurchStructureTypeName = "National Headquarter", HierachyLevel = 1 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 7, ChurchStructureTypeName = "District", HierachyLevel = 2 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 9, ChurchStructureTypeName = "Zone", HierachyLevel = 3 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 13, ChurchStructureTypeName = "Parish", HierachyLevel = 4 },
                    },
                    LastModificationTimeStamp = DateScrutnizer.CurrentTimeStamp(),
                    LastModificationByUserId = 1,
                    RegisteredByUserId = 1,
                    Status = ChurchStructureStatus.Active,
                };
                context.ChurchStructures.AddOrUpdate(m => m.ChurchStructureId, churchStructure);
                context.SaveChanges();
                #endregion

                #region CCN - 7
                churchStructure = new ChurchStructure
                {
                    ChurchId = 7,
                    ChurchStructureTypeDetail = new List<ChurchStructureTypeDetailObj>
                    {
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 1, ChurchStructureTypeName = "National Headquarter", HierachyLevel = 1 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 7, ChurchStructureTypeName = "District", HierachyLevel = 2 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 8, ChurchStructureTypeName = "Arch Diocese", HierachyLevel = 3 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 3, ChurchStructureTypeName = "Diocese", HierachyLevel = 4 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 14, ChurchStructureTypeName = "Deanery", HierachyLevel = 5 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 13, ChurchStructureTypeName = "Parish", HierachyLevel = 6 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 15, ChurchStructureTypeName = "Out Station", HierachyLevel = 7 },
                    },
                    LastModificationTimeStamp = DateScrutnizer.CurrentTimeStamp(),
                    LastModificationByUserId = 1,
                    RegisteredByUserId = 1,
                    Status = ChurchStructureStatus.Active,
                };
                context.ChurchStructures.AddOrUpdate(m => m.ChurchStructureId, churchStructure);
                context.SaveChanges();
                #endregion

                #region LFC - 8
                churchStructure = new ChurchStructure
                {
                    ChurchId = 8,
                    ChurchStructureTypeDetail = new List<ChurchStructureTypeDetailObj>
                    {
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 1, ChurchStructureTypeName = "National Headquarter", HierachyLevel = 1 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 2, ChurchStructureTypeName = "Region", HierachyLevel = 2 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 16, ChurchStructureTypeName = "Diocese", HierachyLevel = 3 },
                    },
                    LastModificationTimeStamp = DateScrutnizer.CurrentTimeStamp(),
                    LastModificationByUserId = 1,
                    RegisteredByUserId = 1,
                    Status = ChurchStructureStatus.Active,
                };
                context.ChurchStructures.AddOrUpdate(m => m.ChurchStructureId, churchStructure);
                context.SaveChanges();
                #endregion

                #region CAC - 9
                churchStructure = new ChurchStructure
                {
                    ChurchId = 9,
                    ChurchStructureTypeDetail = new List<ChurchStructureTypeDetailObj>
                    {
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 1, ChurchStructureTypeName = "National Headquarter", HierachyLevel = 1 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 4, ChurchStructureTypeName = "Special District", HierachyLevel = 2 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 9, ChurchStructureTypeName = "Zone", HierachyLevel = 3 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 7, ChurchStructureTypeName = "District", HierachyLevel = 4 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 11, ChurchStructureTypeName = "Assemblies", HierachyLevel = 5 },
                    },
                    LastModificationTimeStamp = DateScrutnizer.CurrentTimeStamp(),
                    LastModificationByUserId = 1,
                    RegisteredByUserId = 1,
                    Status = ChurchStructureStatus.Active,
                };
                context.ChurchStructures.AddOrUpdate(m => m.ChurchStructureId, churchStructure);
                context.SaveChanges();
                #endregion

                #region CCC - 10
                churchStructure = new ChurchStructure
                {
                    ChurchId = 3,
                    ChurchStructureTypeDetail = new List<ChurchStructureTypeDetailObj>
                    {
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 1, ChurchStructureTypeName = "National Headquarter", HierachyLevel = 1 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 3, ChurchStructureTypeName = "Diocese", HierachyLevel = 2 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 7, ChurchStructureTypeName = "District", HierachyLevel = 3 },
                        new ChurchStructureTypeDetailObj{ ChurchStructureTypeId = 13, ChurchStructureTypeName = "Parish", HierachyLevel = 4 },
                    },
                    LastModificationTimeStamp = DateScrutnizer.CurrentTimeStamp(),
                    LastModificationByUserId = 1,
                    RegisteredByUserId = 1,
                    Status = ChurchStructureStatus.Active,
                };
                context.ChurchStructures.AddOrUpdate(m => m.ChurchStructureId, churchStructure);
                context.SaveChanges();
                #endregion

            }

        }

        private void ProcessClientLookUps(IcasDataContext context)
        {
            if (!context.Clients.Any())
            {
                var client = new Client
                {
                    ChurchId = 1,
                    ClientStructureChurchHeadQuarter = new List<StructureChurchHeadQuarterParish>
                    {
                        new StructureChurchHeadQuarterParish
                        {
                            StructureChurchHeadQuarterParishId = "1",
                            //StructureChurchHeadQuarterId = 1,
                            ChurchStructureTypeId = 1,
                            ParishName = "Lagos Region 1"
                        }
                    },
                    Name = "Simple Tech Church",
                    Pastor = "Simple Tech",
                    Title = 1,
                    Sex = 1,
                    StateOfLocationId = 24,
                    Address = "3, Simple Tech, Avenue, Lagos St.",
                    Email = "simpletech@simpletechng.com",
                    PhoneNumber = "08037000000",
                    ChurchReferenceNumber = "STC0000000001",
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.Clients.AddOrUpdate(m => m.Name, client);
                context.SaveChanges();

                client = new Client
                {
                    ChurchId = 2,
                    ClientStructureChurchHeadQuarter = new List<StructureChurchHeadQuarterParish>
                    {
                        new StructureChurchHeadQuarterParish
                        {
                            StructureChurchHeadQuarterParishId = "1",
                            //StructureChurchHeadQuarterId = 1,
                            ChurchStructureTypeId = 1,
                            ParishName = "Lagos Region 1"
                        }
                    },
                    Name = "Electronic PayPlus Church",
                    Pastor = "Electronic PayPlus",
                    Title = 1,
                    Sex = 1,
                    StateOfLocationId = 24,
                    Address = "Km 34, Ibeju-Lekki Exp Way, Lakowe, Lagos St.",
                    Email = "epaychurch@epayplusng.com",
                    PhoneNumber = "08030000000",
                    ChurchReferenceNumber = "EPCG0000000001",
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.Clients.AddOrUpdate(m => m.Name, client);
                context.SaveChanges();
            }

        }

        #region Client Church

        private void ProcessClientChurchLookUps(IcasDataContext context)
        {
            if (!context.ClientChurches.Any())
            {
                var client = new ClientChurch
                {
                    ChurchId = 1,
                    #region HeadQuarter Parish(s)
                    ClientStructureChurchHeadQuarter = new List<StructureChurchHeadQuarterParish>
                    {
                        new StructureChurchHeadQuarterParish
                        {
                            StructureChurchHeadQuarterParishId = "636439266721003457",
                            ParishName = "Simple Tech Region STR 01",
                            ChurchStructureTypeId = 2,
                            ChurchStructureTypeName = "Region"
                        },
                    },
                    #endregion

                    #region Account Info
                    AccountInfo = new ClientChurchAccount
                    {
                        BankId = 9,
                        AccountTypeId = 1,
                        AccountName = "Simple Tech Church",
                        AccountNumber = "3123345566",
                        BankName = "Guaranty Trust Bank"
                    },
                    #endregion

                    StructureChurchHeadQuarterParishId = "636439266721000000",
                    Name = "Simple Tech Church",
                    Pastor = "Simple Tech",
                    Title = 1,
                    Sex = 1,
                    StateOfLocationId = 24,
                    Address = "3, Simple Tech, Avenue, Lagos St.",
                    Email = "simpletech@simpletechng.com",
                    PhoneNumber = "08037000000",
                    ChurchReferenceNumber = "STC0000000001",
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ClientChurches.AddOrUpdate(m => m.ClientChurchId, client);
                context.SaveChanges();

                client = new ClientChurch
                {
                    ChurchId = 2,
                    #region HeadQuarter Parish(s)
                    ClientStructureChurchHeadQuarter = new List<StructureChurchHeadQuarterParish>
                    {
                        new StructureChurchHeadQuarterParish
                        {
                            StructureChurchHeadQuarterParishId = "636439266721023928",
                            ParishName = "Electronic Grace Parish LEP 01",
                            ChurchStructureTypeId = 2,
                            ChurchStructureTypeName = "Region"
                        }
                    },
                    #endregion

                    #region Account Info
                    AccountInfo = new ClientChurchAccount
                    {
                        BankId = 13,
                        AccountTypeId = 2,
                        AccountName = "Electronic PayPlus Church",
                        AccountNumber = "3568315566",
                        BankName = "Skye Bank"
                    },
                    #endregion

                    StructureChurchHeadQuarterParishId = "636439266721020000",
                    Name = "Electronic PayPlus Church",
                    Pastor = "Electronic PayPlus",
                    Title = 1,
                    Sex = 1,
                    StateOfLocationId = 24,
                    Address = "Km 34, Ibeju-Lekki Exp Way, Lakowe, Lagos St.",
                    Email = "epaychurch@epayplusng.com",
                    PhoneNumber = "08030000000",
                    ChurchReferenceNumber = "EPCG0000000001",
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1,
                };
                context.ClientChurches.AddOrUpdate(m => m.ClientChurchId, client);
                context.SaveChanges();
            }

        }

        #endregion

        private void ProcessProfession(IcasDataContext context)
        {
            if (!context.Professions.Any())
            {
                var profession = new Profession
                {
                    Name = "Clergy",
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp(),
                    AddedByUserId = 1,
                    SourceId = ChurchSettingSource.App
                };
                context.Professions.AddOrUpdate(m => m.Name, profession);
                context.SaveChanges();

                profession = new Profession
                {
                    Name = "Doctor",
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp(),
                    AddedByUserId = 1,
                    SourceId = ChurchSettingSource.App
                };
                context.Professions.AddOrUpdate(m => m.Name, profession);
                context.SaveChanges();

                profession = new Profession
                {
                    Name = "Teacher",
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp(),
                    AddedByUserId = 1,
                    SourceId = ChurchSettingSource.App
                };
                context.Professions.AddOrUpdate(m => m.Name, profession);
                context.SaveChanges();

                profession = new Profession
                {
                    Name = "Civil Servant",
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp(),
                    AddedByUserId = 1,
                    SourceId = ChurchSettingSource.App
                };
                context.Professions.AddOrUpdate(m => m.Name, profession);
                context.SaveChanges();

                profession = new Profession
                {
                    Name = "Driver",
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp(),
                    AddedByUserId = 1,
                    SourceId = ChurchSettingSource.App
                };
                context.Professions.AddOrUpdate(m => m.Name, profession);
                context.SaveChanges();

                profession = new Profession
                {
                    Name = "Others",
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp(),
                    AddedByUserId = 1,
                    SourceId = ChurchSettingSource.App
                };
                context.Professions.AddOrUpdate(m => m.Name, profession);
                context.SaveChanges();

            }

        }

        #region Latest

        private void ProcessCollectionTypes(IcasDataContext context)
        {
            if (!context.CollectionTypes.Any())
            {

                // Offering - 1, Congregation's Tithe - 2, Minister's Tithe - 3, First Fruit - 4
                // Thanksgiving - 5, Sunday Love Offering - 6, Sunday School Offering - 7, House Fellowship - 8, Project and Building - 9

                #region Offering
                var collectionType = new CollectionType
                {
                    //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                    Name = "Offering",
                    SourceId = ChurchSettingSource.App,
                };
                context.CollectionTypes.AddOrUpdate(m => m.Name, collectionType);
                context.SaveChanges();
                #endregion

                #region Congregation's Tithe
                collectionType = new CollectionType
                {
                    //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                    Name = "Congregation's Tithe",
                    SourceId = ChurchSettingSource.App,
                };
                context.CollectionTypes.AddOrUpdate(m => m.Name, collectionType);
                context.SaveChanges();
                #endregion

                #region Minister's Tithe
                collectionType = new CollectionType
                {
                    //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                    Name = "Minister's Tithe",
                    SourceId = ChurchSettingSource.App,
                };
                context.CollectionTypes.AddOrUpdate(m => m.Name, collectionType);
                context.SaveChanges();
                #endregion

                #region First Fruit
                collectionType = new CollectionType
                {
                    //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                    Name = "First Fruit",
                    SourceId = ChurchSettingSource.App,
                };
                context.CollectionTypes.AddOrUpdate(m => m.Name, collectionType);
                context.SaveChanges();
                #endregion

                #region Thanksgiving
                collectionType = new CollectionType
                {
                    //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                    Name = "Thanksgiving",
                    SourceId = ChurchSettingSource.App,
                };
                context.CollectionTypes.AddOrUpdate(m => m.Name, collectionType);
                context.SaveChanges();
                #endregion

                #region Sunday Love Offering
                collectionType = new CollectionType
                {
                    //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                    Name = "Sunday Love Offering",
                    SourceId = ChurchSettingSource.App,
                };
                context.CollectionTypes.AddOrUpdate(m => m.Name, collectionType);
                context.SaveChanges();
                #endregion

                #region Sunday School Offering
                collectionType = new CollectionType
                {
                    //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                    Name = "Sunday School Offering",
                    SourceId = ChurchSettingSource.App,
                };
                context.CollectionTypes.AddOrUpdate(m => m.Name, collectionType);
                context.SaveChanges();
                #endregion

                #region House Fellowship
                collectionType = new CollectionType
                {
                    //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                    Name = "House Fellowshipg",
                    SourceId = ChurchSettingSource.App,
                };
                context.CollectionTypes.AddOrUpdate(m => m.Name, collectionType);
                context.SaveChanges();
                #endregion

                #region Project and Building
                collectionType = new CollectionType
                {
                    //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                    Name = "Project and Building",
                    SourceId = ChurchSettingSource.App,
                };
                context.CollectionTypes.AddOrUpdate(m => m.Name, collectionType);
                context.SaveChanges();
                #endregion

            }

        }

        private void ProcessChurchCollectionTypes(IcasDataContext context)
        {
            if (!context.ChurchCollectionTypes.Any())
            {

                // Offering - 1, Congregation's Tithe - 2, Minister's Tithe - 3, First Fruit - 4
                // Thanksgiving - 5, Sunday Love Offering - 6, Sunday School Offering - 7, House Fellowship - 8, Project and Building - 9

                // National HeadQuarter - 1, Region - 2, Diocese - 3, Special District - 4, State - 5, Group - 6, District - 7, 
                // Arch Diocese - 8, Zone - 9, Area - 10, Assemblies - 11, Branch - 12, Parish - 13, Deanery - 14, Out Station - 15, Province - 16


                #region EPCG
                var churchCollectionType = new ChurchCollectionType
                {
                    ChurchId = 2,
                    CollectionTypes = new List<CollectionTypeObj>
                    {
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 2,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Congregation's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name  = "National HeadQuarter", Percent = 0.20},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name  = "Region", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 9, Name  = "Zone", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name  = "Area", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name  = "Province", Percent = 0.10}
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 3,
                            CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Minister's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name  = "National HeadQuarter", Percent = 0.20},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name  = "Region", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 9, Name  = "Zone", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name  = "Area", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name  = "Province", Percent = 0.10}
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 4,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "First Fruit",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name  = "National HeadQuarter", Percent = 0.20},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name  = "Region", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 9, Name  = "Zone", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name  = "Area", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name  = "Province", Percent = 0.10}
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 5,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Thanksgiving",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name  = "National HeadQuarter", Percent = 0.20},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name  = "Region", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 9, Name  = "Zone", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name  = "Area", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name  = "Province", Percent = 0.10}
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 6,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Sunday Love Offering",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name  = "National HeadQuarter", Percent = 0.20},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name  = "Region", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 9, Name  = "Zone", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name  = "Area", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name  = "Province", Percent = 0.10}
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 7,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Sunday School Offering",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name  = "National HeadQuarter", Percent = 0.20},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name  = "Region", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 9, Name  = "Zone", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name  = "Area", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name  = "Province", Percent = 0.10}
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 8,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "House Fellowship",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name  = "National HeadQuarter", Percent = 0.20},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name  = "Region", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 9, Name  = "Zone", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name  = "Area", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name  = "Province", Percent = 0.10}
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 9,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Project and Building",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name  = "National HeadQuarter", Percent = 0.20},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name  = "Region", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 9, Name  = "Zone", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name  = "Area", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name  = "Province", Percent = 0.10}
                            }
                        }
                    },
                    Status = PreferenceTypeStatus.Active,
                    AddedByUserId = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchCollectionTypes.AddOrUpdate(m => m.ChurchCollectionTypeId, churchCollectionType);
                context.SaveChanges();
                #endregion

                #region RCCG
                churchCollectionType = new ChurchCollectionType
                {
                    ChurchId = 3,
                    CollectionTypes = new List<CollectionTypeObj>
                    {
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 2,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Congregation's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name  = "National HeadQuarter", Percent = 0.20},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name  = "Region", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 9, Name  = "Zone", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name  = "Area", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name  = "Province", Percent = 0.10}
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 3,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Minister's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name  = "National HeadQuarter", Percent = 0.20},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name  = "Region", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 9, Name  = "Zone", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name  = "Area", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name  = "Province", Percent = 0.10}
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 4,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "First Fruit",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name  = "National HeadQuarter", Percent = 0.20},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name  = "Region", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 9, Name  = "Zone", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name  = "Area", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name  = "Province", Percent = 0.10}
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 5,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Thanksgiving",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name  = "National HeadQuarter", Percent = 0.20},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name  = "Region", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 9, Name  = "Zone", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name  = "Area", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name  = "Province", Percent = 0.10}
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 6,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Sunday Love Offering",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name  = "National HeadQuarter", Percent = 0.20},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name  = "Region", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 9, Name  = "Zone", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name  = "Area", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name  = "Province", Percent = 0.10}
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 7,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Sunday School Offering",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name  = "National HeadQuarter", Percent = 0.20},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name  = "Region", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 9, Name  = "Zone", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name  = "Area", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name  = "Province", Percent = 0.10}
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 8,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "House Fellowship",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name  = "National HeadQuarter", Percent = 0.20},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name  = "Region", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 9, Name  = "Zone", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name  = "Area", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name  = "Province", Percent = 0.10}
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 9,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Project and Building",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name  = "National HeadQuarter", Percent = 0.20},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name  = "Region", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 9, Name  = "Zone", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name  = "Area", Percent = 0.0},
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name  = "Province", Percent = 0.10}
                            }
                        }
                    },
                    Status = PreferenceTypeStatus.Active,
                    AddedByUserId = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchCollectionTypes.AddOrUpdate(m => m.ChurchCollectionTypeId, churchCollectionType);
                context.SaveChanges();
                #endregion

                #region MFM
                churchCollectionType = new ChurchCollectionType
                {
                    ChurchId = 4,
                    CollectionTypes = new List<CollectionTypeObj>
                    {
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 1,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Offering",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name = "National HeadQuarter", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 8, Name = "Arch Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 11, Name = "Assemblies", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 2,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Congregation's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name = "National HeadQuarter", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 8, Name = "Arch Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 11, Name = "Assemblies", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 3,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Minister's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name = "National HeadQuarter", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 8, Name = "Arch Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 11, Name = "Assemblies", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        }
                    },
                    Status = PreferenceTypeStatus.Active,
                    AddedByUserId = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchCollectionTypes.AddOrUpdate(m => m.ChurchCollectionTypeId, churchCollectionType);
                context.SaveChanges();
                #endregion

                #region DCLM

                churchCollectionType = new ChurchCollectionType
                {
                    ChurchId = 5,
                    CollectionTypes = new List<CollectionTypeObj>
                    {
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 1,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Offering",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name = "National HeadQuarter", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 4, Name = "Special District", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 5, Name = "State", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 6, Name = "Group", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 2,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Congregation's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name = "National HeadQuarter", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 4, Name = "Special District", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 5, Name = "State", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 6, Name = "Group", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 3,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Minister's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name = "National HeadQuarter", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 4, Name = "Special District", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 5, Name = "State", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 6, Name = "Group", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        }
                    },
                    Status = PreferenceTypeStatus.Active,
                    AddedByUserId = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchCollectionTypes.AddOrUpdate(m => m.ChurchCollectionTypeId, churchCollectionType);
                context.SaveChanges();
                #endregion

                #region FGCN

                churchCollectionType = new ChurchCollectionType
                {
                    ChurchId = 6,
                    CollectionTypes = new List<CollectionTypeObj>
                    {
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 1,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Offering",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 6, Name = "National HeadQuarter", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 8, Name = "Arch Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 12, Name = "Branch", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 2,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Congregation's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 6, Name = "National HeadQuarter", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 8, Name = "Arch Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 12, Name = "Branch", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 3,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Minister's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 6, Name = "National HeadQuarter", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 8, Name = "Arch Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 12, Name = "Branch", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        }
                    },
                    Status = PreferenceTypeStatus.Active,
                    AddedByUserId = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchCollectionTypes.AddOrUpdate(m => m.ChurchCollectionTypeId, churchCollectionType);
                context.SaveChanges();
                #endregion

                #region CCN
                churchCollectionType = new ChurchCollectionType
                {
                    ChurchId = 7,
                    CollectionTypes = new List<CollectionTypeObj>
                    {
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 1,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Offering",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name = "Region", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 7, Name = "District", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 12, Name = "Branch", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 13, Name = "Parish", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 14, Name = "Deanery", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 2,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Congregation's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name = "Region", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 7, Name = "District", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 12, Name = "Branch", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 13, Name = "Parish", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 14, Name = "Deanery", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        }
                    },
                    Status = PreferenceTypeStatus.Active,
                    AddedByUserId = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchCollectionTypes.AddOrUpdate(m => m.ChurchCollectionTypeId, churchCollectionType);
                context.SaveChanges();
                #endregion

                #region LFC
                churchCollectionType = new ChurchCollectionType
                {
                    ChurchId = 8,
                    CollectionTypes = new List<CollectionTypeObj>
                    {
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 1,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Offering",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name = "National HeadQuarter", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name = "Region", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 2,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Congregation's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name = "National HeadQuarter", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name = "Region", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 3,
                            CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Minister's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 1, Name = "National HeadQuarter", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 2, Name = "Region", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        }
                    },
                    Status = PreferenceTypeStatus.Active,
                    AddedByUserId = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchCollectionTypes.AddOrUpdate(m => m.ChurchCollectionTypeId, churchCollectionType);
                context.SaveChanges();
                #endregion

                #region CAC
                churchCollectionType = new ChurchCollectionType
                {
                    ChurchId = 9,
                    CollectionTypes = new List<CollectionTypeObj>
                    {
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 1,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Offering",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 3, Name = "Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 6, Name = "Group", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 8, Name = "Arch Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name = "Area", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 2,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Congregation's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 3, Name = "Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 6, Name = "Group", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 8, Name = "Arch Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name = "Area", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 3,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Minister's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 3, Name = "Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 6, Name = "Group", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 8, Name = "Arch Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name = "Area", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        }
                    },
                    Status = PreferenceTypeStatus.Active,
                    AddedByUserId = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchCollectionTypes.AddOrUpdate(m => m.ChurchCollectionTypeId, churchCollectionType);
                context.SaveChanges();
                #endregion

                #region CCC
                churchCollectionType = new ChurchCollectionType
                {
                    ChurchId = 10,
                    CollectionTypes = new List<CollectionTypeObj>
                    {
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 1,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Offering",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 3, Name = "Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 6, Name = "Group", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 8, Name = "Arch Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name = "Area", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 2,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Congregation's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 3, Name = "Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 6, Name = "Group", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 8, Name = "Arch Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name = "Area", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        },
                        new CollectionTypeObj
                        {
                            CollectionTypeId = 3,
                            //CollectionRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                            Name = "Minister's Tithe",
                            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>
                            {
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 3, Name = "Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 6, Name = "Group", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 8, Name = "Arch Diocese", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 10, Name = "Area", Percent = 0.0 },
                                new ChurchCollectionChurchStructureTypeObj { ChurchStructureTypeId = 16, Name = "Province", Percent = 0.20 }
                            }
                        }
                    },
                    Status = PreferenceTypeStatus.Active,
                    AddedByUserId = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchCollectionTypes.AddOrUpdate(m => m.ChurchCollectionTypeId, churchCollectionType);
                context.SaveChanges();
                #endregion

            }

        }
        private void ProcessChurchRoleTypes(IcasDataContext context)
        {
            if (!context.ChurchRoleTypes.Any())
            {

                #region Usher
                var churchRoleType = new ChurchRoleType
                {
                    Name = "Usher",
                    Description = "Ushering",
                    SourceId = ChurchSettingSource.App,
                    RoleInChurches = new List<RoleInChurch>
                    {
                        new RoleInChurch
                        {
                            ChurchId = 3,
                            ChurchRoleTypeId = 1,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 4,
                            ChurchRoleTypeId = 1,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 5,
                            ChurchRoleTypeId = 1,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 6,
                            ChurchRoleTypeId = 1,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 7,
                            ChurchRoleTypeId = 1,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 8,
                            ChurchRoleTypeId = 1,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 9,
                            ChurchRoleTypeId = 1,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 10,
                            ChurchRoleTypeId = 1,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 11,
                            ChurchRoleTypeId = 1,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                    }
                };
                context.ChurchRoleTypes.AddOrUpdate(m => m.Name, churchRoleType);
                context.SaveChanges();
                #endregion

                #region Choir
                churchRoleType = new ChurchRoleType
                {
                    Name = "Choir",
                    Description = "Choir Ministration",
                    SourceId = ChurchSettingSource.App,
                    RoleInChurches = new List<RoleInChurch>
                    {
                        new RoleInChurch
                        {
                            ChurchId = 3,
                            ChurchRoleTypeId = 2,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 4,
                            ChurchRoleTypeId = 2,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 5,
                            ChurchRoleTypeId = 2,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 6,
                            ChurchRoleTypeId = 2,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 7,
                            ChurchRoleTypeId = 2,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 8,
                            ChurchRoleTypeId = 2,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 9,
                            ChurchRoleTypeId = 2,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 10,
                            ChurchRoleTypeId = 2,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 11,
                            ChurchRoleTypeId = 2,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                    }
                };
                context.ChurchRoleTypes.AddOrUpdate(m => m.Name, churchRoleType);
                context.SaveChanges();
                #endregion

                #region Sunday School Teacher
                churchRoleType = new ChurchRoleType
                {
                    Name = "Sunday School Teacher",
                    Description = "Sunday School Manual Teachings",
                    SourceId = ChurchSettingSource.App,
                    RoleInChurches = new List<RoleInChurch>
                    {
                        new RoleInChurch
                        {
                            ChurchId = 3,
                            ChurchRoleTypeId = 3,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        new RoleInChurch
                        {
                            ChurchId = 4,
                            ChurchRoleTypeId = 3,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        //new RoleInChurch
                        //{
                        //    ChurchId = 5,
                        //    ChurchRoleTypeId = 3,
                        //    AddedByUserId = 1,
                        //    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        //},
                        //new RoleInChurch
                        //{
                        //    ChurchId = 6,
                        //    ChurchRoleTypeId = 3,
                        //    AddedByUserId = 1,
                        //    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        //},
                        //new RoleInChurch
                        //{
                        //    ChurchId = 7,
                        //    ChurchRoleTypeId = 3,
                        //    AddedByUserId = 1,
                        //    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        //},
                        //new RoleInChurch
                        //{
                        //    ChurchId = 8,
                        //    ChurchRoleTypeId = 3,
                        //    AddedByUserId = 1,
                        //    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        //},
                        //new RoleInChurch
                        //{
                        //    ChurchId = 9,
                        //    ChurchRoleTypeId = 3,
                        //    AddedByUserId = 1,
                        //    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        //},
                        //new RoleInChurch
                        //{
                        //    ChurchId = 10,
                        //    ChurchRoleTypeId = 3,
                        //    AddedByUserId = 1,
                        //    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        //},
                        //new RoleInChurch
                        //{
                        //    ChurchId = 11,
                        //    ChurchRoleTypeId = 3,
                        //    AddedByUserId = 1,
                        //    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        //},
                    }
                };
                context.ChurchRoleTypes.AddOrUpdate(m => m.Name, churchRoleType);
                context.SaveChanges();
                #endregion

                #region House Fellowship Leader
                churchRoleType = new ChurchRoleType
                {
                    Name = "House Fellowship Leader",
                    Description = "Head and Teach House Fellowship",
                    SourceId = ChurchSettingSource.App,
                    RoleInChurches = new List<RoleInChurch>
                    {
                        new RoleInChurch
                        {
                            ChurchId = 3,
                            ChurchRoleTypeId = 4,
                            AddedByUserId = 1,
                            TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        },
                        //new RoleInChurch
                        //{
                        //    ChurchId = 4,
                        //    ChurchRoleTypeId = 4,
                        //    AddedByUserId = 1,
                        //    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        //},
                        //new RoleInChurch
                        //{
                        //    ChurchId = 5,
                        //    ChurchRoleTypeId = 4,
                        //    AddedByUserId = 1,
                        //    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        //},
                        //new RoleInChurch
                        //{
                        //    ChurchId = 6,
                        //    ChurchRoleTypeId = 4,
                        //    AddedByUserId = 1,
                        //    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        //},
                        //new RoleInChurch
                        //{
                        //    ChurchId = 7,
                        //    ChurchRoleTypeId = 4,
                        //    AddedByUserId = 1,
                        //    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        //},
                        //new RoleInChurch
                        //{
                        //    ChurchId = 8,
                        //    ChurchRoleTypeId = 4,
                        //    AddedByUserId = 1,
                        //    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        //},
                        //new RoleInChurch
                        //{
                        //    ChurchId = 9,
                        //    ChurchRoleTypeId = 4,
                        //    AddedByUserId = 1,
                        //    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        //},
                        //new RoleInChurch
                        //{
                        //    ChurchId = 10,
                        //    ChurchRoleTypeId = 4,
                        //    AddedByUserId = 1,
                        //    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        //},
                        //new RoleInChurch
                        //{
                        //    ChurchId = 11,
                        //    ChurchRoleTypeId = 4,
                        //    AddedByUserId = 1,
                        //    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                        //},
                    }
                };
                context.ChurchRoleTypes.AddOrUpdate(m => m.Name, churchRoleType);
                context.SaveChanges();
                #endregion

            }

        }
        #endregion

        private void ProcessChurchServiceTypes(IcasDataContext context)
        {
            if (!context.ChurchServiceTypes.Any())
            {
                
                #region Sunday Service
                var churchServiceType = new ChurchServiceType
                {
                    Name = "Sunday Service",
                    SourceId = ChurchSettingSource.App,
                };
                context.ChurchServiceTypes.AddOrUpdate(m => m.Name, churchServiceType);
                context.SaveChanges();
                #endregion

                #region House FellowShip
                churchServiceType = new ChurchServiceType
                {
                    Name = "House FellowShip",
                    SourceId = ChurchSettingSource.App,
                };
                context.ChurchServiceTypes.AddOrUpdate(m => m.Name, churchServiceType);
                context.SaveChanges();
                #endregion

                #region Bible Study
                churchServiceType = new ChurchServiceType
                {
                    //ChurchServiceTypeRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                    Name = "Bible Study",
                    SourceId = ChurchSettingSource.App,
                };
                context.ChurchServiceTypes.AddOrUpdate(m => m.Name, churchServiceType);
                context.SaveChanges();
                #endregion

                #region Faith Clinic
                churchServiceType = new ChurchServiceType
                {
                    //ChurchServiceTypeRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                    Name = "Faith Clinic",
                    SourceId = ChurchSettingSource.App,
                };
                context.ChurchServiceTypes.AddOrUpdate(m => m.Name, churchServiceType);
                context.SaveChanges();
                #endregion

                #region Prayer Meeting
                churchServiceType = new ChurchServiceType
                {
                    //ChurchServiceTypeRefId = UniqueIdentifier.GetUniqueId().ToString(CultureInfo.InvariantCulture),
                    Name = "Prayer Meeting",
                    SourceId = ChurchSettingSource.App,
                };
                context.ChurchServiceTypes.AddOrUpdate(m => m.Name, churchServiceType);
                context.SaveChanges();
                #endregion

            }

        }
        private void ProcessChurchService(IcasDataContext context)
        {

            if (!context.ChurchServices.Any())
            {

                #region EPCG - 2
                var churchService = new ChurchService
                {
                    ChurchId = 2,
                    ServiceTypeDetail = new List<ChurchServiceDetailObj>
                    {
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 1, Name = "Sunday Service", DayOfWeekId = 7, DayOfWeek = "Sunday" },
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 2, Name = "House FellowShip", DayOfWeekId = 7, DayOfWeek = "Sunday" },
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 3, Name = "Bible Study", DayOfWeekId = 2, DayOfWeek = "Tuesday" },
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 4, Name = "Faith Clinic", DayOfWeekId = 4, DayOfWeek = "Thursday" },        
                    },
                    AddedByUserId = 1,
                    Status = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchServices.AddOrUpdate(m => m.ChurchId, churchService);
                context.SaveChanges();

                #endregion

                #region RCCG - 3
                churchService = new ChurchService
                {
                    ChurchId = 3,
                    ServiceTypeDetail = new List<ChurchServiceDetailObj>
                    {
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 1, Name = "Sunday Service", DayOfWeekId = 7, DayOfWeek = "Sunday" },
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 2, Name = "House FellowShip", DayOfWeekId = 7, DayOfWeek = "Sunday" },
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 3, Name = "Bible Study", DayOfWeekId = 2, DayOfWeek = "Tuesday" },
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 4, Name = "Faith Clinic", DayOfWeekId = 4, DayOfWeek = "Thursday" },        
                    },
                    AddedByUserId = 1,
                    Status = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchServices.AddOrUpdate(m => m.ChurchId, churchService);
                context.SaveChanges();

                #endregion

                #region MFM - 4
                churchService = new ChurchService
                {
                    ChurchId = 4,
                    ServiceTypeDetail = new List<ChurchServiceDetailObj>
                    {
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 1, Name = "Sunday Service", DayOfWeekId = 7, DayOfWeek = "Sunday" },
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 3, Name = "Bible Study", DayOfWeekId = 2, DayOfWeek = "Tuesday" },
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 5, Name = "Prayer Meeting", DayOfWeekId = 4, DayOfWeek = "Thursday" },        
                    },
                    AddedByUserId = 1,
                    Status = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchServices.AddOrUpdate(m => m.ChurchId, churchService);
                context.SaveChanges();
                #endregion

                #region DCLM - 5
                churchService = new ChurchService
                {
                    ChurchId = 5,
                    ServiceTypeDetail = new List<ChurchServiceDetailObj>
                    {
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 1, Name = "Sunday Service", DayOfWeekId = 7, DayOfWeek = "Sunday" }        
                    },
                    AddedByUserId = 1,
                    Status = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchServices.AddOrUpdate(m => m.ChurchId, churchService);
                context.SaveChanges();
                #endregion

                #region FGCN - 6
                churchService = new ChurchService
                {
                    ChurchId = 6,
                    ServiceTypeDetail = new List<ChurchServiceDetailObj>
                    {
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 1, Name = "Sunday Service", DayOfWeekId = 7, DayOfWeek = "Sunday" }      
                    },
                    AddedByUserId = 1,
                    Status = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchServices.AddOrUpdate(m => m.ChurchId, churchService);
                context.SaveChanges();
                #endregion

                #region CCN - 7
                churchService = new ChurchService
                {
                    ChurchId = 7,
                    ServiceTypeDetail = new List<ChurchServiceDetailObj>
                    {
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 1, Name = "Sunday Service", DayOfWeekId = 7, DayOfWeek = "Sunday" }      
                    },
                    AddedByUserId = 1,
                    Status = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchServices.AddOrUpdate(m => m.ChurchId, churchService);
                context.SaveChanges();
                #endregion

                #region LFC - 8
                churchService = new ChurchService
                {
                    ChurchId = 8,
                    ServiceTypeDetail = new List<ChurchServiceDetailObj>
                    {
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 1, Name = "Sunday Service", DayOfWeekId = 7, DayOfWeek = "Sunday" }        
                    },
                    AddedByUserId = 1,
                    Status = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchServices.AddOrUpdate(m => m.ChurchId, churchService);
                context.SaveChanges();
                #endregion

                #region CAC - 9
                churchService = new ChurchService
                {
                    ChurchId = 9,
                    ServiceTypeDetail = new List<ChurchServiceDetailObj>
                    {
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 1, Name = "Sunday Service", DayOfWeekId = 7, DayOfWeek = "Sunday" }       
                    },
                    AddedByUserId = 1,
                    Status = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchServices.AddOrUpdate(m => m.ChurchId, churchService);
                context.SaveChanges();
                #endregion

                #region CCC - 10
                churchService = new ChurchService
                {
                    ChurchId = 10,
                    ServiceTypeDetail = new List<ChurchServiceDetailObj>
                    {
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 1, Name = "Sunday Service", DayOfWeekId = 7, DayOfWeek = "Sunday" }      
                    },
                    AddedByUserId = 1,
                    Status = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchServices.AddOrUpdate(m => m.ChurchId, churchService);
                context.SaveChanges();
                #endregion

                #region TAC - 11
                churchService = new ChurchService
                {
                    ChurchId = 11,
                    ServiceTypeDetail = new List<ChurchServiceDetailObj>
                    {
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 1, Name = "Sunday Service", DayOfWeekId = 7, DayOfWeek = "Sunday" }      
                    },
                    AddedByUserId = 1,
                    Status = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchServices.AddOrUpdate(m => m.ChurchId, churchService);
                context.SaveChanges();
                #endregion

                #region SAC - 12
                churchService = new ChurchService
                {
                    ChurchId = 12,
                    ServiceTypeDetail = new List<ChurchServiceDetailObj>
                    {
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 1, Name = "Sunday Service", DayOfWeekId = 7, DayOfWeek = "Sunday" }        
                    },
                    AddedByUserId = 1,
                    Status = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchServices.AddOrUpdate(m => m.ChurchId, churchService);
                context.SaveChanges();
                #endregion

                #region TAFC - 13
                churchService = new ChurchService
                {
                    ChurchId = 13,
                    ServiceTypeDetail = new List<ChurchServiceDetailObj>
                    {
                        new ChurchServiceDetailObj{ ChurchServiceTypeId = 1, Name = "Sunday Service", DayOfWeekId = 7, DayOfWeek = "Sunday" }      
                    },
                    AddedByUserId = 1,
                    Status = 1,
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp()
                };
                context.ChurchServices.AddOrUpdate(m => m.ChurchId, churchService);
                context.SaveChanges();
                #endregion

            }

        }


        private void ProcessChurchServiceAttendance(IcasDataContext context)
        {

            if (!context.ChurchServiceAttendances.Any())
            {

                // Offering - 1, Congregation's Tithe - 2, Minister's Tithe - 3, First Fruit - 4
                // Thanksgiving - 5, Sunday Love Offering - 6, Sunday School Offering - 7, House Fellowship - 8, Project and Building - 9

                // Sunday Service - 1, House FellowShip - 2, Bible Study - 3, Faith Clinic - 4, Prayer Meeting - 5
                #region Month - August

                #region WEEK 1

                #region Sunday Service

                var serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 7, CollectionTypeName = "Sunday School Offering", Amount = 2500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 6, CollectionTypeName = "Sunday Love Offering", Amount = 35500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 2, CollectionTypeName = "Congregation's Tithe", Amount = 50600 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 3, CollectionTypeName = "Minister's Tithe", Amount = 45500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 4, CollectionTypeName = "First Fruit", Amount = 75500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 5, CollectionTypeName = "Thanksgiving", Amount = 12500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 8, CollectionTypeName = "House Fellowship", Amount = 1050 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 9, CollectionTypeName = "Project and Building", Amount = 12000 },
                };

                var serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 56,
                    NumberOfWomen = 65,
                    NumberOfChildren = 32,
                    FirstTimer = 5,
                    NewConvert = 2,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                var churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 1,
                    ServiceTheme = "Christ's Love",
                    BibleReadingText = "John 3 vs 16",
                    Preacher = "Pst. Owoeye",
                    DateServiceHeld = "2017/08/06",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Bible Study

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 1250 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 36,
                    NumberOfWomen = 25,
                    NumberOfChildren = 12,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 3,
                    ServiceTheme = "The Holy Way",
                    BibleReadingText = "Mathew 1 vs 5",
                    Preacher = "Pst. Kingsley",
                    DateServiceHeld = "2017/08/08",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Faith Clinic

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 2230 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 36,
                    NumberOfWomen = 24,
                    NumberOfChildren = 11,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 4,
                    ServiceTheme = "Great Healer",
                    BibleReadingText = "Acts 1 vs 11",
                    Preacher = "Pst. Ojo",
                    DateServiceHeld = "2017/08/10",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #endregion

                #region WEEK 2

                #region Sunday Service


                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 7, CollectionTypeName = "Sunday School Offering", Amount = 1600 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 6, CollectionTypeName = "Sunday Love Offering", Amount = 42210 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 2, CollectionTypeName = "Congregation's Tithe", Amount = 83600 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 3, CollectionTypeName = "Minister's Tithe", Amount = 21000 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 4, CollectionTypeName = "First Fruit", Amount = 15500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 5, CollectionTypeName = "Thanksgiving", Amount = 4500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 8, CollectionTypeName = "House Fellowship", Amount = 2050 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 9, CollectionTypeName = "Project and Building", Amount = 10000 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 76,
                    NumberOfWomen = 66,
                    NumberOfChildren = 42,
                    FirstTimer = 5,
                    NewConvert = 1,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 1,
                    ServiceTheme = "Holy Redemption",
                    BibleReadingText = "II Cor. 4 vs 5",
                    Preacher = "Pst. Ibikunle John",
                    DateServiceHeld = "2017/08/13",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Bible Study

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 3250 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 46,
                    NumberOfWomen = 35,
                    NumberOfChildren = 23,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 3,
                    ServiceTheme = "Holiness",
                    BibleReadingText = "Mark 1 vs 7",
                    Preacher = "Pst. Okoro",
                    DateServiceHeld = "2017/08/15",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Faith Clinic

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 2130 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 28,
                    NumberOfWomen = 21,
                    NumberOfChildren = 19,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 4,
                    ServiceTheme = "Wonders and Signs",
                    BibleReadingText = "Mathew 3 vs 4",
                    Preacher = "Pst. Rotimi",
                    DateServiceHeld = "2017/08/17",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #endregion

                #region WEEK 3

                #region Sunday Service


                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 7, CollectionTypeName = "Sunday School Offering", Amount = 2130 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 6, CollectionTypeName = "Sunday Love Offering", Amount = 22210 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 2, CollectionTypeName = "Congregation's Tithe", Amount = 33600 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 3, CollectionTypeName = "Minister's Tithe", Amount = 21000 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 4, CollectionTypeName = "First Fruit", Amount = 11300 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 5, CollectionTypeName = "Thanksgiving", Amount = 3500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 8, CollectionTypeName = "House Fellowship", Amount = 2450 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 9, CollectionTypeName = "Project and Building", Amount = 11000 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 77,
                    NumberOfWomen = 55,
                    NumberOfChildren = 32,
                    FirstTimer = 3,
                    NewConvert = 3,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 1,
                    ServiceTheme = "Great Deliverer",
                    BibleReadingText = "Mark 2 vs 5",
                    Preacher = "Pst. Lucas Oyibo",
                    DateServiceHeld = "2017/08/20",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Bible Study

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 2250 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 46,
                    NumberOfWomen = 35,
                    NumberOfChildren = 23,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 3,
                    ServiceTheme = "His Will",
                    BibleReadingText = "John 1 vs 2",
                    Preacher = "Pst. Urefe",
                    DateServiceHeld = "2017/08/22",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Faith Clinic

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 1130 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 48,
                    NumberOfWomen = 29,
                    NumberOfChildren = 29,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 4,
                    ServiceTheme = "By His Stripes",
                    BibleReadingText = "Romans 2 vs 4",
                    Preacher = "Pst. Rotimi",
                    DateServiceHeld = "2017/08/24",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #endregion

                #region WEEK 4

                #region Sunday Service


                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 7, CollectionTypeName = "Sunday School Offering", Amount = 1730 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 6, CollectionTypeName = "Sunday Love Offering", Amount = 32340 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 2, CollectionTypeName = "Congregation's Tithe", Amount = 63600 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 3, CollectionTypeName = "Minister's Tithe", Amount = 19000 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 4, CollectionTypeName = "First Fruit", Amount = 10300 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 5, CollectionTypeName = "Thanksgiving", Amount = 2700 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 8, CollectionTypeName = "House Fellowship", Amount = 1450 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 9, CollectionTypeName = "Project and Building", Amount = 9000 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 72,
                    NumberOfWomen = 45,
                    NumberOfChildren = 22,
                    FirstTimer = 3,
                    NewConvert = 1,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 1,
                    ServiceTheme = "Spiritual Battle",
                    BibleReadingText = "Mark 4 vs 5",
                    Preacher = "Pst. Stephen Adetunji",
                    DateServiceHeld = "2017/08/27",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Bible Study

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 2250 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 33,
                    NumberOfWomen = 25,
                    NumberOfChildren = 19,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 3,
                    ServiceTheme = "God's Purpose in Our Life",
                    BibleReadingText = "Genesis 14 vs 2",
                    Preacher = "Pst. Adekeye",
                    DateServiceHeld = "2017/08/29",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Faith Clinic

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 1130 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 38,
                    NumberOfWomen = 24,
                    NumberOfChildren = 21,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 4,
                    ServiceTheme = "Breaking the Barrier",
                    BibleReadingText = "Psalm 2 vs 4",
                    Preacher = "Pst. Afred",
                    DateServiceHeld = "2017/08/31",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #endregion

                #endregion


                #region Month - September

                #region WEEK 1

                #region Sunday Service

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 7, CollectionTypeName = "Sunday School Offering", Amount = 2500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 6, CollectionTypeName = "Sunday Love Offering", Amount = 35500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 2, CollectionTypeName = "Congregation's Tithe", Amount = 50600 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 3, CollectionTypeName = "Minister's Tithe", Amount = 45500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 4, CollectionTypeName = "First Fruit", Amount = 75500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 5, CollectionTypeName = "Thanksgiving", Amount = 12500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 8, CollectionTypeName = "House Fellowship", Amount = 1050 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 9, CollectionTypeName = "Project and Building", Amount = 12000 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 56,
                    NumberOfWomen = 65,
                    NumberOfChildren = 32,
                    FirstTimer = 5,
                    NewConvert = 2,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 1,
                    ServiceTheme = "Christ's Love",
                    BibleReadingText = "John 3 vs 16",
                    Preacher = "Pst. Owoeye",
                    DateServiceHeld = "2017/09/03",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Bible Study

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 1250 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 36,
                    NumberOfWomen = 25,
                    NumberOfChildren = 12,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 3,
                    ServiceTheme = "The Holy Way",
                    BibleReadingText = "Mathew 1 vs 5",
                    Preacher = "Pst. Kingsley",
                    DateServiceHeld = "2017/09/05",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Faith Clinic

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 2230 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 36,
                    NumberOfWomen = 24,
                    NumberOfChildren = 11,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 4,
                    ServiceTheme = "Great Healer",
                    BibleReadingText = "Acts 1 vs 11",
                    Preacher = "Pst. Ojo",
                    DateServiceHeld = "2017/09/07",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #endregion

                #region WEEK 2

                #region Sunday Service


                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 7, CollectionTypeName = "Sunday School Offering", Amount = 1600 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 6, CollectionTypeName = "Sunday Love Offering", Amount = 42210 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 2, CollectionTypeName = "Congregation's Tithe", Amount = 83600 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 3, CollectionTypeName = "Minister's Tithe", Amount = 21000 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 4, CollectionTypeName = "First Fruit", Amount = 15500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 5, CollectionTypeName = "Thanksgiving", Amount = 4500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 8, CollectionTypeName = "House Fellowship", Amount = 2050 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 9, CollectionTypeName = "Project and Building", Amount = 10000 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 76,
                    NumberOfWomen = 66,
                    NumberOfChildren = 42,
                    FirstTimer = 5,
                    NewConvert = 1,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 1,
                    ServiceTheme = "Holy Redemption",
                    BibleReadingText = "II Cor. 4 vs 5",
                    Preacher = "Pst. Ibikunle John",
                    DateServiceHeld = "2017/09/10",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Bible Study

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 3250 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 46,
                    NumberOfWomen = 35,
                    NumberOfChildren = 23,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 3,
                    ServiceTheme = "Holiness",
                    BibleReadingText = "Mark 1 vs 7",
                    Preacher = "Pst. Okoro",
                    DateServiceHeld = "2017/09/12",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Faith Clinic

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 2130 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 28,
                    NumberOfWomen = 21,
                    NumberOfChildren = 19,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 4,
                    ServiceTheme = "Wonders and Signs",
                    BibleReadingText = "Mathew 3 vs 4",
                    Preacher = "Pst. Rotimi",
                    DateServiceHeld = "2017/09/14",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #endregion

                #region WEEK 3

                #region Sunday Service


                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 7, CollectionTypeName = "Sunday School Offering", Amount = 2130 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 6, CollectionTypeName = "Sunday Love Offering", Amount = 22210 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 2, CollectionTypeName = "Congregation's Tithe", Amount = 33600 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 3, CollectionTypeName = "Minister's Tithe", Amount = 21000 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 4, CollectionTypeName = "First Fruit", Amount = 11300 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 5, CollectionTypeName = "Thanksgiving", Amount = 3500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 8, CollectionTypeName = "House Fellowship", Amount = 2450 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 9, CollectionTypeName = "Project and Building", Amount = 11000 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 77,
                    NumberOfWomen = 55,
                    NumberOfChildren = 32,
                    FirstTimer = 3,
                    NewConvert = 3,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 1,
                    ServiceTheme = "Great Deliverer",
                    BibleReadingText = "Mark 2 vs 5",
                    Preacher = "Pst. Lucas Oyibo",
                    DateServiceHeld = "2017/09/17",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Bible Study

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 2250 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 46,
                    NumberOfWomen = 35,
                    NumberOfChildren = 23,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 3,
                    ServiceTheme = "His Will",
                    BibleReadingText = "John 1 vs 2",
                    Preacher = "Pst. Urefe",
                    DateServiceHeld = "2017/09/19",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Faith Clinic

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 1130 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 48,
                    NumberOfWomen = 29,
                    NumberOfChildren = 29,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 4,
                    ServiceTheme = "By His Stripes",
                    BibleReadingText = "Romans 2 vs 4",
                    Preacher = "Pst. Rotimi",
                    DateServiceHeld = "2017/09/21",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #endregion

                #region WEEK 4

                #region Sunday Service


                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 7, CollectionTypeName = "Sunday School Offering", Amount = 1730 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 6, CollectionTypeName = "Sunday Love Offering", Amount = 32340 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 2, CollectionTypeName = "Congregation's Tithe", Amount = 63600 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 3, CollectionTypeName = "Minister's Tithe", Amount = 19000 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 4, CollectionTypeName = "First Fruit", Amount = 10300 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 5, CollectionTypeName = "Thanksgiving", Amount = 2700 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 8, CollectionTypeName = "House Fellowship", Amount = 1450 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 9, CollectionTypeName = "Project and Building", Amount = 9000 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 72,
                    NumberOfWomen = 45,
                    NumberOfChildren = 22,
                    FirstTimer = 3,
                    NewConvert = 1,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 1,
                    ServiceTheme = "Spiritual Battle",
                    BibleReadingText = "Mark 4 vs 5",
                    Preacher = "Pst. Stephen Adetunji",
                    DateServiceHeld = "2017/09/24",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Bible Study

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 2250 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 33,
                    NumberOfWomen = 25,
                    NumberOfChildren = 19,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 3,
                    ServiceTheme = "God's Purpose in Our Life",
                    BibleReadingText = "Genesis 14 vs 2",
                    Preacher = "Pst. Adekeye",
                    DateServiceHeld = "2017/09/26",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Faith Clinic

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 1130 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 38,
                    NumberOfWomen = 24,
                    NumberOfChildren = 21,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 4,
                    ServiceTheme = "Breaking the Barrier",
                    BibleReadingText = "Psalm 2 vs 4",
                    Preacher = "Pst. Afred",
                    DateServiceHeld = "2017/09/28",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #endregion

                #endregion



                #region Month - October

                #region WEEK 1

                #region Sunday Service

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 7, CollectionTypeName = "Sunday School Offering", Amount = 2500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 6, CollectionTypeName = "Sunday Love Offering", Amount = 35500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 2, CollectionTypeName = "Congregation's Tithe", Amount = 50600 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 3, CollectionTypeName = "Minister's Tithe", Amount = 45500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 4, CollectionTypeName = "First Fruit", Amount = 75500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 5, CollectionTypeName = "Thanksgiving", Amount = 12500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 8, CollectionTypeName = "House Fellowship", Amount = 1050 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 9, CollectionTypeName = "Project and Building", Amount = 12000 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 56,
                    NumberOfWomen = 65,
                    NumberOfChildren = 32,
                    FirstTimer = 5,
                    NewConvert = 2,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 1,
                    ServiceTheme = "Christ's Love",
                    BibleReadingText = "John 3 vs 16",
                    Preacher = "Pst. Owoeye",
                    DateServiceHeld = "2017/10/01",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Bible Study

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 1250 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 36,
                    NumberOfWomen = 25,
                    NumberOfChildren = 12,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 3,
                    ServiceTheme = "The Holy Way",
                    BibleReadingText = "Mathew 1 vs 5",
                    Preacher = "Pst. Kingsley",
                    DateServiceHeld = "2017/10/03",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Faith Clinic

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 2230 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 36,
                    NumberOfWomen = 24,
                    NumberOfChildren = 11,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 4,
                    ServiceTheme = "Great Healer",
                    BibleReadingText = "Acts 1 vs 11",
                    Preacher = "Pst. Ojo",
                    DateServiceHeld = "2017/10/05",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #endregion

                #region WEEK 2

                #region Sunday Service


                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 7, CollectionTypeName = "Sunday School Offering", Amount = 1600 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 6, CollectionTypeName = "Sunday Love Offering", Amount = 42210 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 2, CollectionTypeName = "Congregation's Tithe", Amount = 83600 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 3, CollectionTypeName = "Minister's Tithe", Amount = 21000 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 4, CollectionTypeName = "First Fruit", Amount = 15500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 5, CollectionTypeName = "Thanksgiving", Amount = 4500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 8, CollectionTypeName = "House Fellowship", Amount = 2050 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 9, CollectionTypeName = "Project and Building", Amount = 10000 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 76,
                    NumberOfWomen = 66,
                    NumberOfChildren = 42,
                    FirstTimer = 5,
                    NewConvert = 1,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 1,
                    ServiceTheme = "Holy Redemption",
                    BibleReadingText = "II Cor. 4 vs 5",
                    Preacher = "Pst. Ibikunle John",
                    DateServiceHeld = "2017/10/08",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Bible Study

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 3250 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 46,
                    NumberOfWomen = 35,
                    NumberOfChildren = 23,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 3,
                    ServiceTheme = "Holiness",
                    BibleReadingText = "Mark 1 vs 7",
                    Preacher = "Pst. Okoro",
                    DateServiceHeld = "2017/10/10",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Faith Clinic

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 2130 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 28,
                    NumberOfWomen = 21,
                    NumberOfChildren = 19,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 4,
                    ServiceTheme = "Wonders and Signs",
                    BibleReadingText = "Mathew 3 vs 4",
                    Preacher = "Pst. Rotimi",
                    DateServiceHeld = "2017/10/12",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #endregion

                #region WEEK 3

                #region Sunday Service


                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 7, CollectionTypeName = "Sunday School Offering", Amount = 2130 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 6, CollectionTypeName = "Sunday Love Offering", Amount = 22210 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 2, CollectionTypeName = "Congregation's Tithe", Amount = 33600 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 3, CollectionTypeName = "Minister's Tithe", Amount = 21000 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 4, CollectionTypeName = "First Fruit", Amount = 11300 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 5, CollectionTypeName = "Thanksgiving", Amount = 3500 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 8, CollectionTypeName = "House Fellowship", Amount = 2450 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 9, CollectionTypeName = "Project and Building", Amount = 11000 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 77,
                    NumberOfWomen = 55,
                    NumberOfChildren = 32,
                    FirstTimer = 3,
                    NewConvert = 3,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 1,
                    ServiceTheme = "Great Deliverer",
                    BibleReadingText = "Mark 2 vs 5",
                    Preacher = "Pst. Lucas Oyibo",
                    DateServiceHeld = "2017/10/15",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Bible Study

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 2250 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 46,
                    NumberOfWomen = 35,
                    NumberOfChildren = 23,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 3,
                    ServiceTheme = "His Will",
                    BibleReadingText = "John 1 vs 2",
                    Preacher = "Pst. Urefe",
                    DateServiceHeld = "2017/10/17",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Faith Clinic

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 1130 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 48,
                    NumberOfWomen = 29,
                    NumberOfChildren = 29,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 4,
                    ServiceTheme = "By His Stripes",
                    BibleReadingText = "Romans 2 vs 4",
                    Preacher = "Pst. Rotimi",
                    DateServiceHeld = "2017/10/19",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #endregion

                #region WEEK 4

                #region Sunday Service


                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 7, CollectionTypeName = "Sunday School Offering", Amount = 1730 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 6, CollectionTypeName = "Sunday Love Offering", Amount = 32340 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 2, CollectionTypeName = "Congregation's Tithe", Amount = 63600 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 3, CollectionTypeName = "Minister's Tithe", Amount = 19000 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 4, CollectionTypeName = "First Fruit", Amount = 10300 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 5, CollectionTypeName = "Thanksgiving", Amount = 2700 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 8, CollectionTypeName = "House Fellowship", Amount = 1450 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 9, CollectionTypeName = "Project and Building", Amount = 9000 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 72,
                    NumberOfWomen = 45,
                    NumberOfChildren = 22,
                    FirstTimer = 3,
                    NewConvert = 1,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 1,
                    ServiceTheme = "Spiritual Battle",
                    BibleReadingText = "Mark 4 vs 5",
                    Preacher = "Pst. Stephen Adetunji",
                    DateServiceHeld = "2017/10/22",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Bible Study

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 2250 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 33,
                    NumberOfWomen = 25,
                    NumberOfChildren = 19,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 3,
                    ServiceTheme = "God's Purpose in Our Life",
                    BibleReadingText = "Genesis 14 vs 2",
                    Preacher = "Pst. Adekeye",
                    DateServiceHeld = "2017/10/24",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Faith Clinic

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 1130 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 38,
                    NumberOfWomen = 24,
                    NumberOfChildren = 21,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 4,
                    ServiceTheme = "Breaking the Barrier",
                    BibleReadingText = "Psalm 2 vs 4",
                    Preacher = "Pst. Afred",
                    DateServiceHeld = "2017/10/26",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #endregion

                #region WEEK 5

                #region Sunday Service


                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 7, CollectionTypeName = "Sunday School Offering", Amount = 1730 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 6, CollectionTypeName = "Sunday Love Offering", Amount = 32340 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 2, CollectionTypeName = "Congregation's Tithe", Amount = 63600 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 3, CollectionTypeName = "Minister's Tithe", Amount = 19000 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 4, CollectionTypeName = "First Fruit", Amount = 10300 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 5, CollectionTypeName = "Thanksgiving", Amount = 2700 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 8, CollectionTypeName = "House Fellowship", Amount = 1450 },
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 9, CollectionTypeName = "Project and Building", Amount = 9000 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 72,
                    NumberOfWomen = 45,
                    NumberOfChildren = 22,
                    FirstTimer = 3,
                    NewConvert = 1,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 1,
                    ServiceTheme = "Spiritual Battle",
                    BibleReadingText = "Mark 4 vs 5",
                    Preacher = "Pst. Stephen Adetunji",
                    DateServiceHeld = "2017/10/29",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Bible Study

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 2250 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 33,
                    NumberOfWomen = 25,
                    NumberOfChildren = 19,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 3,
                    ServiceTheme = "God's Purpose in Our Life",
                    BibleReadingText = "Genesis 14 vs 2",
                    Preacher = "Pst. Adekeye",
                    DateServiceHeld = "2017/10/31",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #region Faith Clinic

                serviceCollections = new List<ChurchServiceAttendanceCollectionObj>
                {
                    new ChurchServiceAttendanceCollectionObj { CollectionTypeId = 1, CollectionTypeName = "Offering", Amount = 1130 },
                };

                serviceDetail = new ChurchServiceAttendanceDetailObj
                {
                    NumberOfMen = 38,
                    NumberOfWomen = 24,
                    NumberOfChildren = 21,
                    FirstTimer = 0,
                    NewConvert = 0,
                    ChurchServiceAttendanceCollections = serviceCollections
                };
                churchServiceAttendance = new ChurchServiceAttendance
                {
                    ClientChurchId = 2,
                    //ChurchServiceTypeId = 4,
                    ServiceTheme = "Breaking the Barrier",
                    BibleReadingText = "Psalm 2 vs 4",
                    Preacher = "Pst. Afred",
                    DateServiceHeld = "2017/11/02",
                    TotalAttendee = (serviceDetail.NumberOfMen + serviceDetail.NumberOfWomen + serviceDetail.NumberOfChildren),
                    TotalCollection = (serviceCollections.Sum(x => x.Amount)),
                    _ServiceAttendanceDetail = JsonConvert.SerializeObject(serviceDetail),
                    TimeStampTaken = DateScrutnizer.CurrentTimeStamp(),
                    TakenByUserId = 1,
                };
                context.ChurchServiceAttendances.AddOrUpdate(m => m.ChurchServiceAttendanceId, churchServiceAttendance);
                context.SaveChanges();

                #endregion

                #endregion

                #endregion




            }

        }


        private void ProcessChurchStructureParishHeadQuarter(IcasDataContext context)
        {
            if (!context.ChurchStructureParishHeadQuarters.Any())
            {

                #region RCCG Region Lagos

                var parishes = new List<StructureChurchHeadQuarterParish>
                {
                    new StructureChurchHeadQuarterParish
                    {
                        StructureChurchHeadQuarterParishId = "636439266721023928", ParishName = "Abundance Grace Parish LR 20", 
                        ChurchStructureTypeId = 2, ChurchStructureTypeName = "Region"
                    },
                    new StructureChurchHeadQuarterParish
                    {
                        StructureChurchHeadQuarterParishId = "636439266721127023", ParishName = "Halleluyah Parish LR 10", 
                        ChurchStructureTypeId = 2, ChurchStructureTypeName = "Region"
                    },
                    new StructureChurchHeadQuarterParish
                    {
                        StructureChurchHeadQuarterParishId = "636439266721003457", ParishName = "Royal Diadem LR 03", 
                        ChurchStructureTypeId = 2, ChurchStructureTypeName = "Region"
                    },
                };

                var churchStructureParishHeadQuarter = new ChurchStructureParishHeadQuarter
                {
                    ChurchId = 3,
                    ChurchStructureTypeId = 2,
                    StateOfLocationId = 24,
                    _Parish = JsonConvert.SerializeObject(parishes),
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1

                };
                context.ChurchStructureParishHeadQuarters.AddOrUpdate(m => m.ChurchStructureTypeId, churchStructureParishHeadQuarter);
                context.SaveChanges();

                #endregion

                #region RCCG Province Lagos

                parishes = new List<StructureChurchHeadQuarterParish>
                {
                    new StructureChurchHeadQuarterParish
                    {
                        StructureChurchHeadQuarterParishId = "636439266721723421", ParishName = "Heritage of God Parish Lagos Province 24", 
                        ChurchStructureTypeId = 16, ChurchStructureTypeName = "Province"
                    },
                    new StructureChurchHeadQuarterParish
                    {
                        StructureChurchHeadQuarterParishId = "636439266721723422", ParishName = "Bread of Life Parish Lagos Province 34", 
                        ChurchStructureTypeId = 16, ChurchStructureTypeName = "Province"
                    },
                    new StructureChurchHeadQuarterParish
                    {
                        StructureChurchHeadQuarterParishId = "636439266721723428", ParishName = "His Majesty Lagos Province 14", 
                        ChurchStructureTypeId = 16, ChurchStructureTypeName = "Province"
                    },
                };

                churchStructureParishHeadQuarter = new ChurchStructureParishHeadQuarter
                {
                    ChurchId = 3,
                    ChurchStructureTypeId = 16,
                    StateOfLocationId = 24,
                    _Parish = JsonConvert.SerializeObject(parishes),
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1

                };
                context.ChurchStructureParishHeadQuarters.AddOrUpdate(m => m.ChurchStructureTypeId, churchStructureParishHeadQuarter);
                context.SaveChanges();

                #endregion

            }

        }
        private void ProcessChurchThemeLookUps(IcasDataContext context)
        {
            if (!context.ChurchThemeSettings.Any())
            {


                #region RCCG - 3
                var churchThemeSetting = new ChurchThemeSetting
                {
                    ChurchId = 3,
                    ThemeColor = "#408080",
                    ThemeLogo = "RCCG_RCCG.png",
                    ThemeLogoPath = "~/ChurchLogos/",
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1
                };
                context.ChurchThemeSettings.AddOrUpdate(m => m.ChurchId, churchThemeSetting);
                context.SaveChanges();
                #endregion

                #region MFM  - 4
                churchThemeSetting = new ChurchThemeSetting
                {
                    ChurchId = 4,
                    ThemeColor = "#800080",
                    ThemeLogo = "MFM_MFM.jpg",
                    ThemeLogoPath = "~/ChurchLogos/",
                    TimeStampRegistered = DateScrutnizer.CurrentTimeStamp(),
                    RegisteredByUserId = 1
                };
                context.ChurchThemeSettings.AddOrUpdate(m => m.ChurchId, churchThemeSetting);
                context.SaveChanges();
                #endregion

            }

        }
        private void ProcessRoleInChurch(IcasDataContext context)
        {
            if (!context.RoleInChurches.Any())
            {
                var roleInChurch = new RoleInChurch
                {
                    //Name = "Usher",
                    //Description = "Ushering People",
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp(),
                    AddedByUserId = 1,
                };
                //context.RoleInChurches.AddOrUpdate(m => m.Name, roleInChurch);
                context.SaveChanges();

                roleInChurch = new RoleInChurch
                {
                    //Name = "Choir",
                    //Description = "Minister to congregation through songs",
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp(),
                    AddedByUserId = 1,
                };
                //context.RoleInChurches.AddOrUpdate(m => m.Name, roleInChurch);
                context.SaveChanges();

                roleInChurch = new RoleInChurch
                {
                    //Name = "Sunday School Teacher",
                    //Description = "Teach member during sunday school service",
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp(),
                    AddedByUserId = 1,
                };
                //context.RoleInChurches.AddOrUpdate(m => m.Name, roleInChurch);
                context.SaveChanges();

                roleInChurch = new RoleInChurch
                {
                    //Name = "Others",
                    //Description = "Congregant in church",
                    TimeStampAdded = DateScrutnizer.CurrentTimeStamp(),
                    AddedByUserId = 1,
                };
                //context.RoleInChurches.AddOrUpdate(m => m.Name, roleInChurch);
                context.SaveChanges();

            }

        }


        #endregion


        

    }
}
