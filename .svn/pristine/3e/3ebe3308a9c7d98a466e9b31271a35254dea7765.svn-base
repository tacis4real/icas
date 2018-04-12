namespace ICASStacks.DataManager.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ICASDB.Bank",
                c => new
                    {
                        BankId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200, unicode: false),
                        Status = c.Boolean(nullable: false),
                        RegisteredBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BankId);
            
            CreateTable(
                "ICASDB.ClientAccount",
                c => new
                    {
                        ClientAccountId = c.Long(nullable: false, identity: true),
                        ClientId = c.Long(nullable: false),
                        BankId = c.Int(nullable: false),
                        AccountName = c.String(nullable: false),
                        AccountNumber = c.String(nullable: false),
                        AccountTypeId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, unicode: false),
                        RegisteredByUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClientAccountId)
                .ForeignKey("ICASDB.Bank", t => t.BankId, cascadeDelete: true)
                .Index(t => t.BankId);
            
            CreateTable(
                "ICASDB.ChurchCollectionType",
                c => new
                    {
                        ChurchCollectionTypeId = c.Int(nullable: false, identity: true),
                        ChurchId = c.Long(nullable: false),
                        CollectionTypes = c.String(unicode: false, storeType: "text"),
                        Status = c.Int(nullable: false),
                        TimeStampAdded = c.String(nullable: false, maxLength: 35, unicode: false),
                        AddedByUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChurchCollectionTypeId)
                .ForeignKey("ICASDB.Church", t => t.ChurchId, cascadeDelete: true)
                .Index(t => t.ChurchId);
            
            CreateTable(
                "ICASDB.Church",
                c => new
                    {
                        ChurchId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ShortName = c.String(nullable: false),
                        Founder = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false, maxLength: 15),
                        Email = c.String(maxLength: 100),
                        Address = c.String(nullable: false, maxLength: 200, unicode: false),
                        StateOfLocationId = c.Int(nullable: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, unicode: false),
                        RegisteredByUserId = c.Int(nullable: false),
                        ChurchThemeSetting_ChurchThemeSettingId = c.Long(),
                    })
                .PrimaryKey(t => t.ChurchId)
                .ForeignKey("ICASDB.ChurchThemeSetting", t => t.ChurchThemeSetting_ChurchThemeSettingId)
                .Index(t => t.ChurchThemeSetting_ChurchThemeSettingId);
            
            CreateTable(
                "ICASDB.ChurchService",
                c => new
                    {
                        ChurchServiceId = c.Long(nullable: false, identity: true),
                        ChurchId = c.Long(nullable: false),
                        ServiceTypeDetail = c.String(unicode: false, storeType: "text"),
                        TimeStampAdded = c.String(nullable: false, maxLength: 35, unicode: false),
                        AddedByUserId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ChurchServiceType_ChurchServiceTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.ChurchServiceId)
                .ForeignKey("ICASDB.ChurchServiceType", t => t.ChurchServiceType_ChurchServiceTypeId)
                .ForeignKey("ICASDB.Church", t => t.ChurchId, cascadeDelete: true)
                .Index(t => t.ChurchId)
                .Index(t => t.ChurchServiceType_ChurchServiceTypeId);
            
            CreateTable(
                "ICASDB.ChurchServiceType",
                c => new
                    {
                        ChurchServiceTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        SourceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChurchServiceTypeId);
            
            CreateTable(
                "ICASDB.ChurchServiceAttendance",
                c => new
                    {
                        ChurchServiceAttendanceId = c.Long(nullable: false, identity: true),
                        ClientChurchId = c.Long(nullable: false),
                        ChurchServiceTypeRefId = c.String(nullable: false),
                        ServiceTheme = c.String(maxLength: 8000, unicode: false),
                        BibleReadingText = c.String(maxLength: 8000, unicode: false),
                        Preacher = c.String(maxLength: 8000, unicode: false),
                        TotalAttendee = c.Long(nullable: false),
                        TotalCollection = c.Double(nullable: false),
                        ServiceAttendanceDetail = c.String(unicode: false, storeType: "text"),
                        DateServiceHeld = c.String(nullable: false, maxLength: 10, unicode: false),
                        TimeStampTaken = c.String(nullable: false, maxLength: 35, unicode: false),
                        TakenByUserId = c.Int(nullable: false),
                        ChurchServiceType_ChurchServiceTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.ChurchServiceAttendanceId)
                .ForeignKey("ICASDB.ChurchServiceType", t => t.ChurchServiceType_ChurchServiceTypeId)
                .ForeignKey("ICASDB.ClientChurch", t => t.ClientChurchId, cascadeDelete: true)
                .Index(t => t.ClientChurchId)
                .Index(t => t.ChurchServiceType_ChurchServiceTypeId);
            
            CreateTable(
                "ICASDB.ClientChurch",
                c => new
                    {
                        ClientChurchId = c.Long(nullable: false, identity: true),
                        ChurchId = c.Long(nullable: false),
                        StructureChurchHeadQuarterParishId = c.String(),
                        Name = c.String(nullable: false),
                        Pastor = c.String(nullable: false),
                        Title = c.Int(nullable: false),
                        Sex = c.Int(nullable: false),
                        PhoneNumber = c.String(nullable: false, maxLength: 15),
                        Email = c.String(),
                        StateOfLocationId = c.Int(nullable: false),
                        AccountInfo = c.String(unicode: false, storeType: "text"),
                        ClientStructureChurchHeadQuarter = c.String(unicode: false, storeType: "text"),
                        Address = c.String(nullable: false, maxLength: 200, unicode: false),
                        ChurchReferenceNumber = c.String(nullable: false, maxLength: 20, unicode: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, unicode: false),
                        RegisteredByUserId = c.Int(nullable: false),
                        ChurchStructureParishHeadQuarter_ChurchStructureParishHeadQuarterId = c.Long(),
                    })
                .PrimaryKey(t => t.ClientChurchId)
                .ForeignKey("ICASDB.Church", t => t.ChurchId, cascadeDelete: true)
                .ForeignKey("ICASDB.ChurchStructureParishHeadQuarter", t => t.ChurchStructureParishHeadQuarter_ChurchStructureParishHeadQuarterId)
                .Index(t => t.ChurchId)
                .Index(t => t.ChurchReferenceNumber, unique: true, name: "IX_Reg_ChurchRefNo")
                .Index(t => t.ChurchStructureParishHeadQuarter_ChurchStructureParishHeadQuarterId);
            
            CreateTable(
                "ICASDB.ChurchMember",
                c => new
                    {
                        ChurchMemberId = c.Long(nullable: false, identity: true),
                        ClientChurchId = c.Long(nullable: false),
                        FullName = c.String(nullable: false, maxLength: 200, unicode: false),
                        ProfessionId = c.Int(nullable: false),
                        ClientRoleInChurchId = c.Int(nullable: false),
                        Sex = c.Int(nullable: false),
                        Email = c.String(maxLength: 50),
                        MobileNumber = c.String(maxLength: 15, unicode: false),
                        Address = c.String(nullable: false, maxLength: 200, unicode: false),
                        DateJoined = c.String(nullable: false, maxLength: 10, unicode: false),
                        TimeStampUploaded = c.String(maxLength: 35),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, unicode: false),
                        RegisteredByUserId = c.Int(nullable: false),
                        UploadStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChurchMemberId)
                .ForeignKey("ICASDB.ClientChurch", t => t.ClientChurchId, cascadeDelete: true)
                .ForeignKey("ICASDB.ClientRoleInChurch", t => t.ClientRoleInChurchId, cascadeDelete: true)
                .ForeignKey("ICASDB.Profession", t => t.ProfessionId, cascadeDelete: true)
                .Index(t => t.ClientChurchId)
                .Index(t => t.ProfessionId)
                .Index(t => t.ClientRoleInChurchId);
            
            CreateTable(
                "ICASDB.ClientRoleInChurch",
                c => new
                    {
                        ClientRoleInChurchId = c.Int(nullable: false, identity: true),
                        ClientChurchId = c.Long(nullable: false),
                        ChurchRoleTypeId = c.Int(nullable: false),
                        TimeStampAdded = c.String(nullable: false, maxLength: 35, unicode: false),
                        AddedByUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClientRoleInChurchId)
                .ForeignKey("ICASDB.ChurchRoleType", t => t.ChurchRoleTypeId, cascadeDelete: true)
                .Index(t => t.ChurchRoleTypeId);
            
            CreateTable(
                "ICASDB.ChurchRoleType",
                c => new
                    {
                        ChurchRoleTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250, unicode: false),
                        Description = c.String(maxLength: 250, unicode: false),
                        SourceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChurchRoleTypeId);
            
            CreateTable(
                "ICASDB.RoleInChurch",
                c => new
                    {
                        RoleInChurchId = c.Int(nullable: false, identity: true),
                        ChurchId = c.Long(nullable: false),
                        ChurchRoleTypeId = c.Int(nullable: false),
                        TimeStampAdded = c.String(nullable: false, maxLength: 35, unicode: false),
                        AddedByUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoleInChurchId)
                .ForeignKey("ICASDB.ChurchRoleType", t => t.ChurchRoleTypeId, cascadeDelete: true)
                .Index(t => t.ChurchRoleTypeId);
            
            CreateTable(
                "ICASDB.Profession",
                c => new
                    {
                        ProfessionId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250, unicode: false),
                        TimeStampAdded = c.String(nullable: false, maxLength: 35, unicode: false),
                        AddedByUserId = c.Int(nullable: false),
                        SourceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProfessionId);
            
            CreateTable(
                "ICASDB.ChurchStructureParishHeadQuarter",
                c => new
                    {
                        ChurchStructureParishHeadQuarterId = c.Long(nullable: false, identity: true),
                        ChurchId = c.Long(nullable: false),
                        ChurchStructureTypeId = c.Int(nullable: false),
                        StateOfLocationId = c.Int(nullable: false),
                        Parish = c.String(nullable: false, unicode: false, storeType: "text"),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, unicode: false),
                        RegisteredByUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChurchStructureParishHeadQuarterId)
                .ForeignKey("ICASDB.ChurchStructureType", t => t.ChurchStructureTypeId, cascadeDelete: true)
                .ForeignKey("ICASDB.StateOfLocation", t => t.StateOfLocationId, cascadeDelete: true)
                .ForeignKey("ICASDB.Church", t => t.ChurchId, cascadeDelete: true)
                .Index(t => t.ChurchId)
                .Index(t => t.ChurchStructureTypeId)
                .Index(t => t.StateOfLocationId);
            
            CreateTable(
                "ICASDB.ChurchStructureType",
                c => new
                    {
                        ChurchStructureTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, unicode: false),
                        RegisteredByUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChurchStructureTypeId);
            
            CreateTable(
                "ICASDB.ChurchStructureHqtr",
                c => new
                    {
                        ChurchStructureHqtrId = c.Long(nullable: false, identity: true),
                        ChurchId = c.Long(nullable: false),
                        ClientId = c.Long(nullable: false),
                        ChurchStructureTypeId = c.Int(nullable: false),
                        StructureDetailId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ChurchStructureHqtrId)
                .ForeignKey("ICASDB.ChurchStructureType", t => t.ChurchStructureTypeId, cascadeDelete: true)
                .ForeignKey("ICASDB.Church", t => t.ChurchId, cascadeDelete: true)
                .Index(t => t.ChurchId)
                .Index(t => t.ChurchStructureTypeId);
            
            CreateTable(
                "ICASDB.ChurchStructure",
                c => new
                    {
                        ChurchStructureId = c.Long(nullable: false, identity: true),
                        ChurchId = c.Long(nullable: false),
                        ChurchStructureTypeDetail = c.String(unicode: false, storeType: "text"),
                        LastModificationTimeStamp = c.String(maxLength: 35, unicode: false),
                        LastModificationByUserId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        RegisteredByUserId = c.Int(nullable: false),
                        ChurchStructureType_ChurchStructureTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.ChurchStructureId)
                .ForeignKey("ICASDB.Church", t => t.ChurchId, cascadeDelete: true)
                .ForeignKey("ICASDB.ChurchStructureType", t => t.ChurchStructureType_ChurchStructureTypeId)
                .Index(t => t.ChurchId)
                .Index(t => t.ChurchStructureType_ChurchStructureTypeId);
            
            CreateTable(
                "ICASDB.ClientChurchStructureDetail",
                c => new
                    {
                        ClientStructureChurchDetailId = c.Long(nullable: false, identity: true),
                        ChurchId = c.Long(nullable: false),
                        ClientId = c.Long(nullable: false),
                        StructureChurchId = c.Long(nullable: false),
                        ChurchStructureTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClientStructureChurchDetailId)
                .ForeignKey("ICASDB.ChurchStructureType", t => t.ChurchStructureTypeId, cascadeDelete: true)
                .ForeignKey("ICASDB.Client", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.ChurchStructureTypeId);
            
            CreateTable(
                "ICASDB.StructureChurch",
                c => new
                    {
                        StructureChurchId = c.Long(nullable: false, identity: true),
                        ChurchId = c.Long(nullable: false),
                        ChurchStructureTypeId = c.Int(nullable: false),
                        StateOfLocationId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, unicode: false),
                        RegisteredByUserId = c.Int(nullable: false),
                        ChurchStructureHqtr_ChurchStructureHqtrId = c.Long(),
                        ClientStructureChurchDetail_ClientStructureChurchDetailId = c.Long(),
                    })
                .PrimaryKey(t => t.StructureChurchId)
                .ForeignKey("ICASDB.Church", t => t.ChurchId, cascadeDelete: true)
                .ForeignKey("ICASDB.ChurchStructureHqtr", t => t.ChurchStructureHqtr_ChurchStructureHqtrId)
                .ForeignKey("ICASDB.ChurchStructureType", t => t.ChurchStructureTypeId, cascadeDelete: true)
                .ForeignKey("ICASDB.ClientChurchStructureDetail", t => t.ClientStructureChurchDetail_ClientStructureChurchDetailId)
                .ForeignKey("ICASDB.StateOfLocation", t => t.StateOfLocationId, cascadeDelete: true)
                .Index(t => t.ChurchId)
                .Index(t => t.ChurchStructureTypeId)
                .Index(t => t.StateOfLocationId)
                .Index(t => t.ChurchStructureHqtr_ChurchStructureHqtrId)
                .Index(t => t.ClientStructureChurchDetail_ClientStructureChurchDetailId);
            
            CreateTable(
                "ICASDB.StateOfLocation",
                c => new
                    {
                        StateOfLocationId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.StateOfLocationId);
            
            CreateTable(
                "ICASDB.ClientChurchService",
                c => new
                    {
                        ClientChurchServiceId = c.Long(nullable: false, identity: true),
                        ClientChurchId = c.Long(nullable: false),
                        ServiceTypeDetail = c.String(unicode: false, storeType: "text"),
                        TimeStampAdded = c.String(nullable: false, maxLength: 35, unicode: false),
                        AddedByUserId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ChurchServiceType_ChurchServiceTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.ClientChurchServiceId)
                .ForeignKey("ICASDB.ChurchServiceType", t => t.ChurchServiceType_ChurchServiceTypeId)
                .ForeignKey("ICASDB.ClientChurch", t => t.ClientChurchId, cascadeDelete: true)
                .Index(t => t.ClientChurchId)
                .Index(t => t.ChurchServiceType_ChurchServiceTypeId);
            
            CreateTable(
                "ICASDB.ChurchThemeSetting",
                c => new
                    {
                        ChurchThemeSettingId = c.Long(nullable: false, identity: true),
                        ChurchId = c.Long(nullable: false),
                        ThemeColor = c.String(nullable: false),
                        ThemeLogo = c.String(nullable: false),
                        ThemeLogoPath = c.String(nullable: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, unicode: false),
                        RegisteredByUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChurchThemeSettingId);
            
            CreateTable(
                "ICASDB.Client",
                c => new
                    {
                        ClientId = c.Long(nullable: false, identity: true),
                        ChurchId = c.Long(nullable: false),
                        StructureChurchHeadQuarterParishId = c.Long(),
                        Name = c.String(nullable: false),
                        Pastor = c.String(nullable: false),
                        Title = c.Int(nullable: false),
                        Sex = c.Int(nullable: false),
                        PhoneNumber = c.String(nullable: false, maxLength: 15),
                        Email = c.String(),
                        StateOfLocationId = c.Int(nullable: false),
                        ClientStructureChurchHeadQuarter = c.String(unicode: false, storeType: "text"),
                        Address = c.String(nullable: false, maxLength: 200, unicode: false),
                        ChurchReferenceNumber = c.String(nullable: false, maxLength: 20, unicode: false),
                        TimeStampRegistered = c.String(nullable: false, maxLength: 35, unicode: false),
                        RegisteredByUserId = c.Int(nullable: false),
                        Account_ClientAccountId = c.Long(),
                    })
                .PrimaryKey(t => t.ClientId)
                .ForeignKey("ICASDB.ClientAccount", t => t.Account_ClientAccountId)
                .ForeignKey("ICASDB.Church", t => t.ChurchId, cascadeDelete: true)
                .Index(t => t.ChurchId)
                .Index(t => t.ChurchReferenceNumber, unique: true, name: "IX_Reg_ChurchRefNo")
                .Index(t => t.Account_ClientAccountId);
            
            CreateTable(
                "dbo.ChurchServiceAttendanceRemittance",
                c => new
                    {
                        ChurchServiceAttendanceRemittanceId = c.Long(nullable: false, identity: true),
                        ClientId = c.Long(nullable: false),
                        From = c.String(),
                        To = c.String(),
                        TimeStampRemitted = c.String(nullable: false, maxLength: 35, unicode: false),
                        RemittedByUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChurchServiceAttendanceRemittanceId)
                .ForeignKey("ICASDB.Client", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "ICASDB.ClientChurchCollectionType",
                c => new
                    {
                        ClientChurchCollectionTypeId = c.Long(nullable: false, identity: true),
                        ClientChurchId = c.Long(nullable: false),
                        CollectionTypes = c.String(unicode: false, storeType: "text"),
                        TimeStampAdded = c.String(nullable: false, maxLength: 35, unicode: false),
                        AddedByUserId = c.Int(nullable: false),
                        CollectionType_CollectionTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.ClientChurchCollectionTypeId)
                .ForeignKey("ICASDB.CollectionType", t => t.CollectionType_CollectionTypeId)
                .Index(t => t.CollectionType_CollectionTypeId);
            
            CreateTable(
                "ICASDB.CollectionType",
                c => new
                    {
                        CollectionTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        SourceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CollectionTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ICASDB.ClientChurchCollectionType", "CollectionType_CollectionTypeId", "ICASDB.CollectionType");
            DropForeignKey("ICASDB.ClientChurchStructureDetail", "ClientId", "ICASDB.Client");
            DropForeignKey("dbo.ChurchServiceAttendanceRemittance", "ClientId", "ICASDB.Client");
            DropForeignKey("ICASDB.Client", "ChurchId", "ICASDB.Church");
            DropForeignKey("ICASDB.Client", "Account_ClientAccountId", "ICASDB.ClientAccount");
            DropForeignKey("ICASDB.Church", "ChurchThemeSetting_ChurchThemeSettingId", "ICASDB.ChurchThemeSetting");
            DropForeignKey("ICASDB.ChurchStructureParishHeadQuarter", "ChurchId", "ICASDB.Church");
            DropForeignKey("ICASDB.ChurchStructureHqtr", "ChurchId", "ICASDB.Church");
            DropForeignKey("ICASDB.ChurchService", "ChurchId", "ICASDB.Church");
            DropForeignKey("ICASDB.ChurchService", "ChurchServiceType_ChurchServiceTypeId", "ICASDB.ChurchServiceType");
            DropForeignKey("ICASDB.ClientChurchService", "ClientChurchId", "ICASDB.ClientChurch");
            DropForeignKey("ICASDB.ClientChurchService", "ChurchServiceType_ChurchServiceTypeId", "ICASDB.ChurchServiceType");
            DropForeignKey("ICASDB.ClientChurch", "ChurchStructureParishHeadQuarter_ChurchStructureParishHeadQuarterId", "ICASDB.ChurchStructureParishHeadQuarter");
            DropForeignKey("ICASDB.ChurchStructureParishHeadQuarter", "StateOfLocationId", "ICASDB.StateOfLocation");
            DropForeignKey("ICASDB.StructureChurch", "StateOfLocationId", "ICASDB.StateOfLocation");
            DropForeignKey("ICASDB.StructureChurch", "ClientStructureChurchDetail_ClientStructureChurchDetailId", "ICASDB.ClientChurchStructureDetail");
            DropForeignKey("ICASDB.StructureChurch", "ChurchStructureTypeId", "ICASDB.ChurchStructureType");
            DropForeignKey("ICASDB.StructureChurch", "ChurchStructureHqtr_ChurchStructureHqtrId", "ICASDB.ChurchStructureHqtr");
            DropForeignKey("ICASDB.StructureChurch", "ChurchId", "ICASDB.Church");
            DropForeignKey("ICASDB.ClientChurchStructureDetail", "ChurchStructureTypeId", "ICASDB.ChurchStructureType");
            DropForeignKey("ICASDB.ChurchStructure", "ChurchStructureType_ChurchStructureTypeId", "ICASDB.ChurchStructureType");
            DropForeignKey("ICASDB.ChurchStructure", "ChurchId", "ICASDB.Church");
            DropForeignKey("ICASDB.ChurchStructureParishHeadQuarter", "ChurchStructureTypeId", "ICASDB.ChurchStructureType");
            DropForeignKey("ICASDB.ChurchStructureHqtr", "ChurchStructureTypeId", "ICASDB.ChurchStructureType");
            DropForeignKey("ICASDB.ChurchServiceAttendance", "ClientChurchId", "ICASDB.ClientChurch");
            DropForeignKey("ICASDB.ChurchMember", "ProfessionId", "ICASDB.Profession");
            DropForeignKey("ICASDB.RoleInChurch", "ChurchRoleTypeId", "ICASDB.ChurchRoleType");
            DropForeignKey("ICASDB.ClientRoleInChurch", "ChurchRoleTypeId", "ICASDB.ChurchRoleType");
            DropForeignKey("ICASDB.ChurchMember", "ClientRoleInChurchId", "ICASDB.ClientRoleInChurch");
            DropForeignKey("ICASDB.ChurchMember", "ClientChurchId", "ICASDB.ClientChurch");
            DropForeignKey("ICASDB.ClientChurch", "ChurchId", "ICASDB.Church");
            DropForeignKey("ICASDB.ChurchServiceAttendance", "ChurchServiceType_ChurchServiceTypeId", "ICASDB.ChurchServiceType");
            DropForeignKey("ICASDB.ChurchCollectionType", "ChurchId", "ICASDB.Church");
            DropForeignKey("ICASDB.ClientAccount", "BankId", "ICASDB.Bank");
            DropIndex("ICASDB.ClientChurchCollectionType", new[] { "CollectionType_CollectionTypeId" });
            DropIndex("dbo.ChurchServiceAttendanceRemittance", new[] { "ClientId" });
            DropIndex("ICASDB.Client", new[] { "Account_ClientAccountId" });
            DropIndex("ICASDB.Client", "IX_Reg_ChurchRefNo");
            DropIndex("ICASDB.Client", new[] { "ChurchId" });
            DropIndex("ICASDB.ClientChurchService", new[] { "ChurchServiceType_ChurchServiceTypeId" });
            DropIndex("ICASDB.ClientChurchService", new[] { "ClientChurchId" });
            DropIndex("ICASDB.StructureChurch", new[] { "ClientStructureChurchDetail_ClientStructureChurchDetailId" });
            DropIndex("ICASDB.StructureChurch", new[] { "ChurchStructureHqtr_ChurchStructureHqtrId" });
            DropIndex("ICASDB.StructureChurch", new[] { "StateOfLocationId" });
            DropIndex("ICASDB.StructureChurch", new[] { "ChurchStructureTypeId" });
            DropIndex("ICASDB.StructureChurch", new[] { "ChurchId" });
            DropIndex("ICASDB.ClientChurchStructureDetail", new[] { "ChurchStructureTypeId" });
            DropIndex("ICASDB.ClientChurchStructureDetail", new[] { "ClientId" });
            DropIndex("ICASDB.ChurchStructure", new[] { "ChurchStructureType_ChurchStructureTypeId" });
            DropIndex("ICASDB.ChurchStructure", new[] { "ChurchId" });
            DropIndex("ICASDB.ChurchStructureHqtr", new[] { "ChurchStructureTypeId" });
            DropIndex("ICASDB.ChurchStructureHqtr", new[] { "ChurchId" });
            DropIndex("ICASDB.ChurchStructureParishHeadQuarter", new[] { "StateOfLocationId" });
            DropIndex("ICASDB.ChurchStructureParishHeadQuarter", new[] { "ChurchStructureTypeId" });
            DropIndex("ICASDB.ChurchStructureParishHeadQuarter", new[] { "ChurchId" });
            DropIndex("ICASDB.RoleInChurch", new[] { "ChurchRoleTypeId" });
            DropIndex("ICASDB.ClientRoleInChurch", new[] { "ChurchRoleTypeId" });
            DropIndex("ICASDB.ChurchMember", new[] { "ClientRoleInChurchId" });
            DropIndex("ICASDB.ChurchMember", new[] { "ProfessionId" });
            DropIndex("ICASDB.ChurchMember", new[] { "ClientChurchId" });
            DropIndex("ICASDB.ClientChurch", new[] { "ChurchStructureParishHeadQuarter_ChurchStructureParishHeadQuarterId" });
            DropIndex("ICASDB.ClientChurch", "IX_Reg_ChurchRefNo");
            DropIndex("ICASDB.ClientChurch", new[] { "ChurchId" });
            DropIndex("ICASDB.ChurchServiceAttendance", new[] { "ChurchServiceType_ChurchServiceTypeId" });
            DropIndex("ICASDB.ChurchServiceAttendance", new[] { "ClientChurchId" });
            DropIndex("ICASDB.ChurchService", new[] { "ChurchServiceType_ChurchServiceTypeId" });
            DropIndex("ICASDB.ChurchService", new[] { "ChurchId" });
            DropIndex("ICASDB.Church", new[] { "ChurchThemeSetting_ChurchThemeSettingId" });
            DropIndex("ICASDB.ChurchCollectionType", new[] { "ChurchId" });
            DropIndex("ICASDB.ClientAccount", new[] { "BankId" });
            DropTable("ICASDB.CollectionType");
            DropTable("ICASDB.ClientChurchCollectionType");
            DropTable("dbo.ChurchServiceAttendanceRemittance");
            DropTable("ICASDB.Client");
            DropTable("ICASDB.ChurchThemeSetting");
            DropTable("ICASDB.ClientChurchService");
            DropTable("ICASDB.StateOfLocation");
            DropTable("ICASDB.StructureChurch");
            DropTable("ICASDB.ClientChurchStructureDetail");
            DropTable("ICASDB.ChurchStructure");
            DropTable("ICASDB.ChurchStructureHqtr");
            DropTable("ICASDB.ChurchStructureType");
            DropTable("ICASDB.ChurchStructureParishHeadQuarter");
            DropTable("ICASDB.Profession");
            DropTable("ICASDB.RoleInChurch");
            DropTable("ICASDB.ChurchRoleType");
            DropTable("ICASDB.ClientRoleInChurch");
            DropTable("ICASDB.ChurchMember");
            DropTable("ICASDB.ClientChurch");
            DropTable("ICASDB.ChurchServiceAttendance");
            DropTable("ICASDB.ChurchServiceType");
            DropTable("ICASDB.ChurchService");
            DropTable("ICASDB.Church");
            DropTable("ICASDB.ChurchCollectionType");
            DropTable("ICASDB.ClientAccount");
            DropTable("ICASDB.Bank");
        }
    }
}
