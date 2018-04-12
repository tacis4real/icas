namespace WebAdminStacks.DataManager.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ICASWebAdmin.ClientChurchDeviceAccessAuthorization",
                c => new
                    {
                        ClientChurchDeviceAccessAuthorizationId = c.Long(nullable: false, identity: true),
                        ClientChurchLoginActivityId = c.Long(nullable: false),
                        ClientChurchProfileId = c.Long(nullable: false),
                        ClientChurchDeviceId = c.Long(nullable: false),
                        AuthorizedDate = c.String(nullable: false, maxLength: 10, unicode: false),
                        AuthorizedTime = c.String(nullable: false, maxLength: 15, unicode: false),
                        AuthorizedDeviceSerialNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        AuthorizationToken = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsExpired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClientChurchDeviceAccessAuthorizationId)
                .ForeignKey("ICASWebAdmin.ClientChurchDevice", t => t.ClientChurchDeviceId, cascadeDelete: true)
                .Index(t => t.ClientChurchDeviceId);
            
            CreateTable(
                "ICASWebAdmin.ClientChurchDevice",
                c => new
                    {
                        ClientChurchDeviceId = c.Long(nullable: false, identity: true),
                        ClientChurchProfileId = c.Long(nullable: false),
                        DeviceSerialNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        DeviceName = c.String(maxLength: 50, unicode: false),
                        NotificationCode = c.String(unicode: false, storeType: "text"),
                        DeviceOSType = c.Int(nullable: false),
                        IsAuthorized = c.Boolean(nullable: false),
                        AuthorizationCode = c.String(maxLength: 10, unicode: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, unicode: false),
                    })
                .PrimaryKey(t => t.ClientChurchDeviceId)
                .ForeignKey("ICASWebAdmin.ClientChurchProfile", t => t.ClientChurchProfileId, cascadeDelete: true)
                .Index(t => t.ClientChurchProfileId);
            
            CreateTable(
                "ICASWebAdmin.ClientChurchProfile",
                c => new
                    {
                        ClientChurchProfileId = c.Long(nullable: false, identity: true),
                        ClientChurchId = c.Long(nullable: false),
                        Fullname = c.String(nullable: false, maxLength: 100, unicode: false),
                        MobileNumber = c.String(nullable: false, maxLength: 15, unicode: false),
                        Sex = c.Int(nullable: false),
                        Email = c.String(maxLength: 50),
                        Username = c.String(nullable: false, maxLength: 20, unicode: false),
                        UserCode = c.String(nullable: false, maxLength: 200, unicode: false),
                        AccessCode = c.String(nullable: false, maxLength: 200, unicode: false),
                        Password = c.String(nullable: false, maxLength: 200, unicode: false),
                        IsLockedOut = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsFirstTimeLogin = c.Boolean(nullable: false),
                        PasswordChangeTimeStamp = c.String(maxLength: 35, unicode: false),
                        LastLoginTimeStamp = c.String(maxLength: 35, unicode: false),
                        LastLockedOutTimeStamp = c.String(maxLength: 35, unicode: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, unicode: false),
                        FailedPasswordCount = c.Int(nullable: false),
                        IsPasswordChangeRequired = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsWebActive = c.Boolean(nullable: false),
                        IsMobileActive = c.Boolean(nullable: false),
                        RegisteredByUserId = c.Int(nullable: false),
                        IsEmailVerified = c.Boolean(nullable: false),
                        IsMobileNumberVerified = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClientChurchProfileId)
                .Index(t => t.Username, unique: true, name: "UQ_User_Username");
            
            CreateTable(
                "ICASWebAdmin.ClientChurchLoginActivity",
                c => new
                    {
                        ClientChurchLoginActivityId = c.Long(nullable: false, identity: true),
                        ClientChurchProfileId = c.Long(nullable: false),
                        IsLoggedIn = c.Boolean(nullable: false),
                        LoginSource = c.Int(nullable: false),
                        LoginAddress = c.String(maxLength: 50, unicode: false),
                        LoginToken = c.String(maxLength: 50, unicode: false),
                        LoginTimeStamp = c.String(nullable: false, maxLength: 35, unicode: false),
                    })
                .PrimaryKey(t => t.ClientChurchLoginActivityId)
                .ForeignKey("ICASWebAdmin.ClientChurchProfile", t => t.ClientChurchProfileId, cascadeDelete: true)
                .Index(t => t.ClientChurchProfileId);
            
            CreateTable(
                "ICASWebAdmin.ClientChurchRole",
                c => new
                    {
                        ClientChurchRoleId = c.Long(nullable: false, identity: true),
                        ClientChurchProfileId = c.Long(nullable: false),
                        RoleClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClientChurchRoleId)
                .ForeignKey("ICASWebAdmin.RoleClient", t => t.RoleClientId, cascadeDelete: true)
                .ForeignKey("ICASWebAdmin.ClientChurchProfile", t => t.ClientChurchProfileId, cascadeDelete: true)
                .Index(t => t.ClientChurchProfileId)
                .Index(t => t.RoleClientId);
            
            CreateTable(
                "ICASWebAdmin.RoleClient",
                c => new
                    {
                        RoleClientId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.RoleClientId);
            
            CreateTable(
                "ICASWebAdmin.ClientRole",
                c => new
                    {
                        ClientRoleId = c.Long(nullable: false, identity: true),
                        ClientProfileId = c.Long(nullable: false),
                        RoleClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClientRoleId)
                .ForeignKey("ICASWebAdmin.ClientProfile", t => t.ClientProfileId, cascadeDelete: true)
                .ForeignKey("ICASWebAdmin.RoleClient", t => t.RoleClientId, cascadeDelete: true)
                .Index(t => t.ClientProfileId)
                .Index(t => t.RoleClientId);
            
            CreateTable(
                "ICASWebAdmin.ClientProfile",
                c => new
                    {
                        ClientProfileId = c.Long(nullable: false, identity: true),
                        ClientId = c.Long(nullable: false),
                        Fullname = c.String(nullable: false, maxLength: 100, unicode: false),
                        MobileNumber = c.String(nullable: false, maxLength: 15, unicode: false),
                        Sex = c.Int(nullable: false),
                        Email = c.String(maxLength: 50),
                        Username = c.String(nullable: false, maxLength: 20, unicode: false),
                        UserCode = c.String(nullable: false, maxLength: 200, unicode: false),
                        AccessCode = c.String(nullable: false, maxLength: 200, unicode: false),
                        Password = c.String(nullable: false, maxLength: 200, unicode: false),
                        IsLockedOut = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsFirstTimeLogin = c.Boolean(nullable: false),
                        PasswordChangeTimeStamp = c.String(maxLength: 35, unicode: false),
                        LastLoginTimeStamp = c.String(maxLength: 35, unicode: false),
                        LastLockedOutTimeStamp = c.String(maxLength: 35, unicode: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, unicode: false),
                        FailedPasswordCount = c.Int(nullable: false),
                        IsPasswordChangeRequired = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsWebActive = c.Boolean(nullable: false),
                        IsMobileActive = c.Boolean(nullable: false),
                        RegisteredByUserId = c.Int(nullable: false),
                        IsEmailVerified = c.Boolean(nullable: false),
                        IsMobileNumberVerified = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClientProfileId)
                .Index(t => t.Username, unique: true, name: "UQ_User_Username");
            
            CreateTable(
                "ICASWebAdmin.ClientDevice",
                c => new
                    {
                        ClientDeviceId = c.Long(nullable: false, identity: true),
                        ClientProfileId = c.Long(nullable: false),
                        DeviceSerialNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        DeviceName = c.String(maxLength: 50, unicode: false),
                        NotificationCode = c.String(unicode: false, storeType: "text"),
                        DeviceOSType = c.Int(nullable: false),
                        IsAuthorized = c.Boolean(nullable: false),
                        AuthorizationCode = c.String(maxLength: 10, unicode: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, unicode: false),
                    })
                .PrimaryKey(t => t.ClientDeviceId)
                .ForeignKey("ICASWebAdmin.ClientProfile", t => t.ClientProfileId, cascadeDelete: true)
                .Index(t => t.ClientProfileId);
            
            CreateTable(
                "ICASWebAdmin.ClientDeviceAccessAuthorization",
                c => new
                    {
                        ClientDeviceAccessAuthorizationId = c.Long(nullable: false, identity: true),
                        ClientLoginActivityId = c.Long(nullable: false),
                        ClientProfileId = c.Long(nullable: false),
                        ClientDeviceId = c.Long(nullable: false),
                        AuthorizedDate = c.String(nullable: false, maxLength: 10, unicode: false),
                        AuthorizedTime = c.String(nullable: false, maxLength: 15, unicode: false),
                        AuthorizedDeviceSerialNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        AuthorizationToken = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsExpired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClientDeviceAccessAuthorizationId)
                .ForeignKey("ICASWebAdmin.ClientDevice", t => t.ClientDeviceId, cascadeDelete: true)
                .Index(t => t.ClientDeviceId);
            
            CreateTable(
                "ICASWebAdmin.ClientLoginActivity",
                c => new
                    {
                        ClientLoginActivityId = c.Long(nullable: false, identity: true),
                        ClientProfileId = c.Long(nullable: false),
                        IsLoggedIn = c.Boolean(nullable: false),
                        LoginSource = c.Int(nullable: false),
                        LoginAddress = c.String(maxLength: 50, unicode: false),
                        LoginToken = c.String(maxLength: 50, unicode: false),
                        LoginTimeStamp = c.String(nullable: false, maxLength: 35, unicode: false),
                    })
                .PrimaryKey(t => t.ClientLoginActivityId)
                .ForeignKey("ICASWebAdmin.ClientProfile", t => t.ClientProfileId, cascadeDelete: true)
                .Index(t => t.ClientProfileId);
            
            CreateTable(
                "ICASWebAdmin.DeviceAccessAuthorization",
                c => new
                    {
                        DeviceAccessAuthorizationId = c.Long(nullable: false, identity: true),
                        UserLoginActivityId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        UserDeviceId = c.Long(nullable: false),
                        AuthorizedDate = c.String(nullable: false, maxLength: 10, unicode: false),
                        AuthorizedTime = c.String(nullable: false, maxLength: 15, unicode: false),
                        AuthorizedDeviceSerialNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        AuthorizationToken = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsExpired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DeviceAccessAuthorizationId)
                .ForeignKey("ICASWebAdmin.UserDevice", t => t.UserDeviceId, cascadeDelete: true)
                .Index(t => t.UserDeviceId);
            
            CreateTable(
                "ICASWebAdmin.UserDevice",
                c => new
                    {
                        UserDeviceId = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        DeviceSerialNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        DeviceName = c.String(maxLength: 50, unicode: false),
                        NotificationCode = c.String(unicode: false, storeType: "text"),
                        DeviceOSType = c.Int(nullable: false),
                        IsAuthorized = c.Boolean(nullable: false),
                        AuthorizationCode = c.String(maxLength: 10, unicode: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, unicode: false),
                    })
                .PrimaryKey(t => t.UserDeviceId)
                .ForeignKey("ICASWebAdmin.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "ICASWebAdmin.User",
                c => new
                    {
                        UserId = c.Long(nullable: false, identity: true),
                        Surname = c.String(nullable: false, maxLength: 100, unicode: false),
                        Othernames = c.String(nullable: false, maxLength: 200, unicode: false),
                        MobileNumber = c.String(nullable: false, maxLength: 15, unicode: false),
                        Sex = c.Int(nullable: false),
                        Email = c.String(maxLength: 50),
                        Username = c.String(nullable: false, maxLength: 20, unicode: false),
                        UserCode = c.String(nullable: false, maxLength: 200, unicode: false),
                        AccessCode = c.String(nullable: false, maxLength: 200, unicode: false),
                        Password = c.String(nullable: false, maxLength: 200, unicode: false),
                        IsLockedOut = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsFirstTimeLogin = c.Boolean(nullable: false),
                        PasswordChangeTimeStamp = c.String(maxLength: 35, unicode: false),
                        LastLoginTimeStamp = c.String(maxLength: 35, unicode: false),
                        LastLockedOutTimeStamp = c.String(maxLength: 35, unicode: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, unicode: false),
                        FailedPasswordCount = c.Int(nullable: false),
                        IsPasswordChangeRequired = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsWebActive = c.Boolean(nullable: false),
                        IsMobileActive = c.Boolean(nullable: false),
                        RegisteredByUserId = c.Int(nullable: false),
                        IsEmailVerified = c.Boolean(nullable: false),
                        IsMobileNumberVerified = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.Username, unique: true, name: "UQ_User_Username");
            
            CreateTable(
                "ICASWebAdmin.UserLoginActivity",
                c => new
                    {
                        UserLoginActivityId = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        IsLoggedIn = c.Boolean(nullable: false),
                        LoginSource = c.Int(nullable: false),
                        LoginAddress = c.String(maxLength: 50, unicode: false),
                        LoginToken = c.String(maxLength: 50, unicode: false),
                        LoginTimeStamp = c.String(nullable: false, maxLength: 35, unicode: false),
                    })
                .PrimaryKey(t => t.UserLoginActivityId)
                .ForeignKey("ICASWebAdmin.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "ICASWebAdmin.UserRole",
                c => new
                    {
                        UserRoleId = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserRoleId)
                .ForeignKey("ICASWebAdmin.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("ICASWebAdmin.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "ICASWebAdmin.Role",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250, unicode: false),
                        Description = c.String(maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ICASWebAdmin.UserRole", "UserId", "ICASWebAdmin.User");
            DropForeignKey("ICASWebAdmin.UserRole", "RoleId", "ICASWebAdmin.Role");
            DropForeignKey("ICASWebAdmin.UserLoginActivity", "UserId", "ICASWebAdmin.User");
            DropForeignKey("ICASWebAdmin.UserDevice", "UserId", "ICASWebAdmin.User");
            DropForeignKey("ICASWebAdmin.DeviceAccessAuthorization", "UserDeviceId", "ICASWebAdmin.UserDevice");
            DropForeignKey("ICASWebAdmin.ClientChurchRole", "ClientChurchProfileId", "ICASWebAdmin.ClientChurchProfile");
            DropForeignKey("ICASWebAdmin.ClientRole", "RoleClientId", "ICASWebAdmin.RoleClient");
            DropForeignKey("ICASWebAdmin.ClientRole", "ClientProfileId", "ICASWebAdmin.ClientProfile");
            DropForeignKey("ICASWebAdmin.ClientLoginActivity", "ClientProfileId", "ICASWebAdmin.ClientProfile");
            DropForeignKey("ICASWebAdmin.ClientDevice", "ClientProfileId", "ICASWebAdmin.ClientProfile");
            DropForeignKey("ICASWebAdmin.ClientDeviceAccessAuthorization", "ClientDeviceId", "ICASWebAdmin.ClientDevice");
            DropForeignKey("ICASWebAdmin.ClientChurchRole", "RoleClientId", "ICASWebAdmin.RoleClient");
            DropForeignKey("ICASWebAdmin.ClientChurchLoginActivity", "ClientChurchProfileId", "ICASWebAdmin.ClientChurchProfile");
            DropForeignKey("ICASWebAdmin.ClientChurchDevice", "ClientChurchProfileId", "ICASWebAdmin.ClientChurchProfile");
            DropForeignKey("ICASWebAdmin.ClientChurchDeviceAccessAuthorization", "ClientChurchDeviceId", "ICASWebAdmin.ClientChurchDevice");
            DropIndex("ICASWebAdmin.UserRole", new[] { "RoleId" });
            DropIndex("ICASWebAdmin.UserRole", new[] { "UserId" });
            DropIndex("ICASWebAdmin.UserLoginActivity", new[] { "UserId" });
            DropIndex("ICASWebAdmin.User", "UQ_User_Username");
            DropIndex("ICASWebAdmin.UserDevice", new[] { "UserId" });
            DropIndex("ICASWebAdmin.DeviceAccessAuthorization", new[] { "UserDeviceId" });
            DropIndex("ICASWebAdmin.ClientLoginActivity", new[] { "ClientProfileId" });
            DropIndex("ICASWebAdmin.ClientDeviceAccessAuthorization", new[] { "ClientDeviceId" });
            DropIndex("ICASWebAdmin.ClientDevice", new[] { "ClientProfileId" });
            DropIndex("ICASWebAdmin.ClientProfile", "UQ_User_Username");
            DropIndex("ICASWebAdmin.ClientRole", new[] { "RoleClientId" });
            DropIndex("ICASWebAdmin.ClientRole", new[] { "ClientProfileId" });
            DropIndex("ICASWebAdmin.ClientChurchRole", new[] { "RoleClientId" });
            DropIndex("ICASWebAdmin.ClientChurchRole", new[] { "ClientChurchProfileId" });
            DropIndex("ICASWebAdmin.ClientChurchLoginActivity", new[] { "ClientChurchProfileId" });
            DropIndex("ICASWebAdmin.ClientChurchProfile", "UQ_User_Username");
            DropIndex("ICASWebAdmin.ClientChurchDevice", new[] { "ClientChurchProfileId" });
            DropIndex("ICASWebAdmin.ClientChurchDeviceAccessAuthorization", new[] { "ClientChurchDeviceId" });
            DropTable("ICASWebAdmin.Role");
            DropTable("ICASWebAdmin.UserRole");
            DropTable("ICASWebAdmin.UserLoginActivity");
            DropTable("ICASWebAdmin.User");
            DropTable("ICASWebAdmin.UserDevice");
            DropTable("ICASWebAdmin.DeviceAccessAuthorization");
            DropTable("ICASWebAdmin.ClientLoginActivity");
            DropTable("ICASWebAdmin.ClientDeviceAccessAuthorization");
            DropTable("ICASWebAdmin.ClientDevice");
            DropTable("ICASWebAdmin.ClientProfile");
            DropTable("ICASWebAdmin.ClientRole");
            DropTable("ICASWebAdmin.RoleClient");
            DropTable("ICASWebAdmin.ClientChurchRole");
            DropTable("ICASWebAdmin.ClientChurchLoginActivity");
            DropTable("ICASWebAdmin.ClientChurchProfile");
            DropTable("ICASWebAdmin.ClientChurchDevice");
            DropTable("ICASWebAdmin.ClientChurchDeviceAccessAuthorization");
        }
    }
}
