using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using ICASStacks.Common;
using ICASStacks.DataContract;
using ICASStacks.DataContract.ChurchAdministrative;
using ICASStacks.DataContract.Enum;
using Newtonsoft.Json;
using WebAdminStacks.APIObjs;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICASStacks.APIObjs
{


    #region Make PAY

    public class ChurchPayeePayObj
    {

        [CheckNumber(0, ErrorMessage = "Denomination is required")]
        public long ChurchId { get; set; }

        [Required(ErrorMessage = "*Full name is Required")]
        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [CheckAmount(0, ErrorMessage = "Collection type amount is required")]
        public float Amount { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "*Phone number Required")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "*Rquest Reference Required")]
        [DisplayName("Rquest Reference")]
        public string RquestReference { get; set; }

        [StringLength(100)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }
    }

    #endregion



    public class ChurchRegObj
    {

        public long ChurchId { get; set; }

        [Required(ErrorMessage = "*Church name is Required")]
        [DisplayName("Church Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*Short name Required")]
        [DisplayName("Short Name")]
        public string ShortName { get; set; }

        [Required(ErrorMessage = "*Founder name Required")]
        [DisplayName("Founder")]
        public string Founder { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "*Phone number Required")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Church Location Address is required")]
        [DisplayName("Church Address")]
        public string Address { get; set; }

        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }
        
        public int RegisteredByUserId { get; set; }

    }

    public class ParentChurchCollectionTypeRegObj
    {

        public long[] ChurchCollectionTypeIds { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent church information is required")]
        public long ChurchId { get; set; }
        public List<NameAndValueObject> AllChurchCollectionTypes { get; set; }
        public int[] MyChurchCollectionTypeIds { get; set; }
        public string[] MyChurchCollectionTypes { get; set; }
        public string SelectedChurchCollectionTypes { get; set; }
        public int AddedByUserId { get; set; }

    }

    public class ClientRegistrationObj
    {

        #region Personal Info
        
        public long ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent Church Information is required")]
        public long ChurchId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Name is required", AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Pastor/Leader")]
        public string Pastor { get; set; }
        
        [Required(ErrorMessage = "Title is required", AllowEmptyStrings = false)]
        public int Title { get; set; }

        [Range(1, 2)]
        public int Sex { get; set; }

        public string ChurchReferenceNumber { get; set; }

        #endregion


        #region Contact Info
       
        [StringLength(15)]
        [Required(ErrorMessage = "* Required")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Church Location Address is required")]
        [DisplayName("Church Address")]
        public string Address { get; set; }  

        //public ClientContact Contact { get; set; }

        #endregion

        #region Church Structure Info

        //public List<long> AllStructureChurchIds { get; set; }

        //public List<long> ClientStructureChurchIds { get; set; }

        //public long? StructureChurchId { get; set; }
        
        //public long? ParishId { get; set; }

        //public long? RegionId { get; set; }

        //public long? ProvinceId { get; set; }

        //public long? ZoneId { get; set; }

        //public long? AreaId { get; set; }

        //public long? DioceseId { get; set; }

        //public long? DistrictId { get; set; }

        //public long? StateId { get; set; }

        //public long? GroupId { get; set; }

        //public List<NameAndValueObject> AllStructures { get; set; }

        //[CheckNumber(0, ErrorMessage = "Church structure type information is required")]
        //public int ChurchStructureTypeId { get; set; }
        
        public int? HeadQuarterChurchStructureTypeId { get; set; }

        //public Region Region { get; set; }

        //public ClientChurchStructureRegObj Province { get; set; }

        //public ClientChurchStructureRegObj Zone { get; set; }

        //public ClientChurchStructureRegObj Area { get; set; }

        //public ClientChurchStructureRegObj Diocese { get; set; }

        //public ClientChurchStructureRegObj District { get; set; }

        //public ClientChurchStructureRegObj State { get; set; }

        //public ClientChurchStructureRegObj Group { get; set; }

        #endregion

        #region Client Roles
        //public int[] ChurchRoleId { get; set; }

        //public List<NameAndValueObject> AllRoles { get; set; }

        //public int[] ClientRoleId { get; set; }

        //public int[] MyRoleIds { get; set; }

        //public string[] MyRoles { get; set; }
        #endregion

        
        [Column(TypeName = "varchar")]
        [StringLength(35)]
        public string TimeStampRegistered { get; set; }

        public int RegisteredByUserId { get; set; }
    }


    #region CLIENT SETTING UP

    public class ChurchStructureParishHeadQuarterRegObj
    {

        public long ChurchStructureParishHeadQuarterId { get; set; }
        
        public long ChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }

        [DisplayName("Church Structure Type")]
        public int ChurchStructureTypeId { get; set; }

        public List<StructureChurchHeadQuarterParish> Parish { get; set; }

        //public StructureChurchHeadQuarterParish Parish { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        public string TimeStampRegistered { get; set; }

        public int RegisteredByUserId { get; set; }
    }

    public class RegisteredClientChurchReportObj
    {

        public long ClientChurchId { get; set; }
        public string Name { get; set; }
        public long ChurchId { get; set; }
        public string ParentChurchName { get; set; }
        public string ParentChurchShortName { get; set; }
        public string StructureChurchHeadQuarterParishId { get; set; }
        public string Pastor { get; set; }
        public int Title { get; set; }
        public int Sex { get; set; }
        public string TitleName { get; set; }
        public string SexName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int StateOfLocationId { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string ChurchReferenceNumber { get; set; }
        public string TimeStampRegistered { get; set; }
        public int RegisteredByUserId { get; set; }
        public int? ClientChurchHeadQuarterChurchStructureTypeId { get; set; }

        public ClientChurchAccount AccountInfo { get; set; }
        public string[] ChurchStructureParishHeadQuarters { get; set; }
        public List<StructureChurchHeadQuarterParish> Parishes { get; set; }
        public ClientProfileRegistrationObj ClientChurchProfile { get; set; }

        //internal string _Account { get; set; }
        //public ClientChurchAccount AccountInfo
        //{
        //    get { return _Account == null ? null : JsonConvert.DeserializeObject<ClientChurchAccount>(_Account); }
        //    set { _Account = JsonConvert.SerializeObject(value); }
        //}
        //internal string _Parish { get; set; }
        //public List<StructureChurchHeadQuarterParish> ClientStructureChurchHeadQuarter
        //{
        //    get { return _Parish == null ? null : JsonConvert.DeserializeObject<List<StructureChurchHeadQuarterParish>>(_Parish); }
        //    set { _Parish = JsonConvert.SerializeObject(value); }
        //}

    }

    public class ClientChurchRegistrationObj
    {

        #region Personal Info

        public long ClientChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent Church Information is required")]
        public long ChurchId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Name is required", AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Pastor/Leader")]
        public string Pastor { get; set; }

        [Required(ErrorMessage = "Title is required", AllowEmptyStrings = false)]
        public int Title { get; set; }

        [Range(1, 2)]
        public int Sex { get; set; }
        public string ChurchReferenceNumber { get; set; }

        #endregion


        #region Contact Info

        [StringLength(15)]
        [Required(ErrorMessage = "* Required")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Church Location Address is required")]
        [DisplayName("Church Address")]
        public string Address { get; set; }

        //public ClientContact Contact { get; set; }

        #endregion

        #region Church Structure Parish HeadQuarter Info

        public string[] ChurchStructureParishHeadQuarters { get; set; }
        public int? ClientChurchHeadQuarterChurchStructureTypeId { get; set; }
        

        #endregion

        #region Client Profile
        public int[] MyRoleIds { get; set; }
        //public ClientChurchProfileRegObj ClientChurchProfile { get; set; }

        public List<StructureChurchHeadQuarterParish> Parishes { get; set; } 
        public ClientProfileRegistrationObj ClientChurchProfile { get; set; }
        public ClientChurchAccount ClientChurchAccount { get; set; }

        //ClientProfileRegistrationObj
        #endregion

        #region Client Account Info

        //[DisplayName("Bank Name")]
        //[CheckNumber(0, ErrorMessage = "Bank Information is required")]
        public int BankId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Account Type")]
        public int AccountTypeId { get; set; }

        

        #endregion


        [Column(TypeName = "varchar")]
        [StringLength(35)]
        public string TimeStampRegistered { get; set; }

        public int RegisteredByUserId { get; set; }

        public string Action { get; set; }

        public ClientChurchRegistrationObj()
        {
            ClientChurchProfile = new ClientProfileRegistrationObj();
            ClientChurchAccount = new ClientChurchAccount();
            Parishes = new List<StructureChurchHeadQuarterParish>();
        }

    }


    public class ClientChurchProfileRegObj
    {
        
        [UIHint("MultiLookup")]
        public IEnumerable<int> Roles { get; set; }
        public List<NameAndValueObject> AllRoles { get; set; }
        public int[] MyRoleIds { get; set; }
        public string[] MyRoles { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "* Required")]
        [StringLength(150)]
        [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "* Required")]
        [StringLength(150)]
        [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
        public string ConfirmPassword { get; set; }
        public string SelectedRoles { get; set; }

        [Required(ErrorMessage = "* Required")]
        [StringLength(50)]
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }


    #region Registered Church Structure Parishes
    public class RegisteredStructureChurchParishReportObj
    {
        public long ChurchStructureParishHeadQuarterId { get; set; }
        public int StateOfLocationId { get; set; }
        public string StateOfLocationName { get; set; }
        public long ChurchId { get; set; }
        public int ChurchStructureTypeId { get; set; }
        public string ChurchStructureTypeName { get; set; }
        public string ParentChurchName { get; set; }
        public List<StructureChurchHeadQuarterParish> Parishes { get; set; }

    }

    #endregion
    #endregion





    #region Administrative Module


    #region Church Collection Types


    #region Single Collection Types Object


    public class CollectionTypeObj
    {
        public int CollectionTypeId { get; set; }
        public string CollectionRefId { get; set; }
        public string Name { get; set; }
        
        public List<ChurchCollectionChurchStructureTypeObj> ChurchStructureTypeObjs { get; set; }

        public CollectionTypeObj()
        {
            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>();
        }
    }


    #endregion





    public class ChurchCollectionTypeSettingObjs
    {

        public long ClientChurchColletionTypeId { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent Church Information is required")]
        public long ChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public long ClientId { get; set; }

        public List<ChurchCollectionTypeObj> ChurchCollectionStructureTypes { get; set; }

        //public int CollectionTypeId { get; set; }
        //public string Name { get; set; }
        //public List<ChurchCollectionChurchStructureTypeObj> ChurchStructureTypes { get; set; }
        public int SetByUserId { get; set; }

        public ChurchCollectionTypeSettingObjs()
        {
            ChurchCollectionStructureTypes = new List<ChurchCollectionTypeObj>();
        }
    }

    public class ClientChurchCollectionTypeSettingObjs
    {

        [CheckNumber(0, ErrorMessage = "Client Church Collection Type information is required")]
        public long ClientChurchColletionTypeId { get; set; }
        
        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public long ClientChurchId { get; set; }
        public List<CollectionTypeObj> ClientChurchCollectionStructureTypes { get; set; }
        public int SetByUserId { get; set; }

        public ClientChurchCollectionTypeSettingObjs()
        {
            ClientChurchCollectionStructureTypes = new List<CollectionTypeObj>();
        }
    }


    public class ChurchCollectionChurchStructureTypePercentObj
    {
        public int ChurchStructureTypeId { get; set; }
        public double Percent { get; set; }
    }


    public class ClientChurchCollectionTypeObjs
    {
        public int CollectionTypeId { get; set; }
        public string Name { get; set; }
        public List<ChurchCollectionChurchStructureTypeObj> ChurchStructureTypeObjs { get; set; }

        public ClientChurchCollectionTypeObjs()
        {
            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>();
        }
    }

    public class ChurchCollectionTypeObj
    {
        public int CollectionTypeId { get; set; }
        public string Name { get; set; }


        //public float[] Percentages { get; set; }
        public List<ChurchCollectionChurchStructureTypeObj> ChurchStructureTypeObjs { get; set; }

        public ChurchCollectionTypeObj()
        {
            ChurchStructureTypeObjs = new List<ChurchCollectionChurchStructureTypeObj>();
        }
    }

    public class ChurchCollectionChurchStructureTypeObj
    {
        public int ChurchStructureTypeId { get; set; }
        public string Name { get; set; }
        public double Percent { get; set; }
    }


    #endregion
 

    #region Latest Church Service

    public class ParentChurchServiceRegObj
    {
        public long ChurchId { get; set; }
        public int AddedByUserId { get; set; }
        public List<ChurchServiceObj> ChurchServices { get; set; }

        public ParentChurchServiceRegObj()
        {
            ChurchServices = new List<ChurchServiceObj>();
        }
    }


    public class ClientChurchServiceDetailObj
    {
        public int ChurchServiceTypeId { get; set; }
        public string ChurchServiceTypeRefId { get; set; }
        public string Name { get; set; }
        public int DayOfWeekId { get; set; }
        public string DayOfWeek { get; set; }

        public string SelectedDayOfWeek { get; set; }
        public List<NameAndValueObject> WeekDays { get; set; }
        public int[] MyWeekDayIds { get; set; }
        public ClientChurchServiceDetailObj()
        {
            WeekDays = new List<NameAndValueObject>();
        }
    }

    public class ChurchServiceDetailObj
    {
        public int ChurchServiceTypeId { get; set; }
        public string ChurchServiceTypeRefId { get; set; }
        public string Name { get; set; }
        public int DayOfWeekId { get; set; }
        public string DayOfWeek { get; set; }
    }
    
    public class ChurchServiceAverageAttendanceObj
    {
        public int ChurchServiceTypeId { get; set; }
        public string ServiceName { get; set; }
        public int AverageAttendeeMen { get; set; }
        public int AverageAttendeeWomen { get; set; }
        public int AverageAttendeeChildren { get; set; }
        public int TotalAverageAttendee { get; set; }
    }

    public class ChurchServiceObj
    {
        public long ChurchServiceId { get; set; }
        public int ChurchServiceTypeId { get; set; }
        public string Name { get; set; }
        public string WeekDay { get; set; }
        public string SelectedDayOfWeek { get; set; }
        public List<NameAndValueObject> WeekDays { get; set; }
        public int[] MyWeekDayIds { get; set; }

        public ChurchServiceObj()
        {
            WeekDays = new List<NameAndValueObject>();
        }

    }

    #endregion

    public class ChurchServiceTypeRegObj
    {
        public int ChurchServiceTypeId { get; set; }

        [Required(ErrorMessage = "*Church service type name is required")]
        [DisplayName("Church Service Type Name")]
        public string Name { get; set; }
        public ChurchSettingSource SourceId { get; set; }

    }

    public class ClientChurchTreasuryRegistrationObj
    {
        public long ClientChurchId { get; set; }

        public List<ClientChurchCollectionTypeObj> ChurchCollectionTypeObjs { get; set; }

        public ClientChurchTreasuryRegistrationObj()
        {
            ChurchCollectionTypeObjs = new List<ClientChurchCollectionTypeObj>();
        }
        
    }
    
    public class ClientChurchCollectionTypeObj
    {
        [CheckNumber(0, ErrorMessage = "Collection type information is required")]
        public int CollectionTypeId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Service Name")]
        public string Name { get; set; }

        [CheckAmount(0, ErrorMessage = "Collection type amount is required")]
        public float Amount { get; set; }

    }
    
    public class TreasuryObj
    {
        [CheckNumber(0, ErrorMessage = "Collection type information is required")]
        public int CollectionTypeId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Service Name")]
        public string Name { get; set; }

        [CheckAmount(0, ErrorMessage = "Collection type amount is required")]
        public float Amount { get; set; }

    }


    public class ChurchMemberRegistrationObj
    {
        public long ChurchMemberId { get; set; }

        public int RecordId { get; set; }

        [CheckNumber(0, ErrorMessage = "This member church Information is required")]
        public long ClientChurchId { get; set; }

        [Required(ErrorMessage = "Name is required", AllowEmptyStrings = false)]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 200 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Profession is required", AllowEmptyStrings = false)]
        public int ProfessionId { get; set; }

        //[Required(ErrorMessage = "Profession is required", AllowEmptyStrings = false)]
        //[StringLength(200, MinimumLength = 2, ErrorMessage = "Profession must be between 2 and 200 characters")]
        public string Profession { get; set; }

        [CheckNumber(0, ErrorMessage = "Member role in church is required")]
        public int RoleInChurchId { get; set; }

        //[Required(ErrorMessage = "Member role in church is required", AllowEmptyStrings = false)]
        //[StringLength(200, MinimumLength = 2, ErrorMessage = "Role must be between 2 and 200 characters")]
        public string RoleInChurch { get; set; }

        [Range(1, 2)]
        public int Sex { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [StringLength(15)]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Member Address is required")]
        [DisplayName("Member Address")]
        public string Address { get; set; }

        [StringLength(10)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Joined is required")]
        public string DateJoined { get; set; }

        public int RegisteredByUserId { get; set; }

        public int UploadStatus { get; set; }
        public string UploadStatusLabel
        {
            get
            {
                if (UploadStatus < 1)
                {
                    return "";
                }
                var name = Enum.GetName(typeof(UploadStatus), UploadStatus);
                if (name != null)
                {
                    return name.Replace("_", " ");
                }
                return "";
            }
        }
    }

    public class RoleInChurchRegObj
    {
        public int RoleInChurchId { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [DisplayName("Role in Church Description")]
        [StringLength(250)]
        public string Description { get; set; }
    }

    public class ChurchServiceRegObj
    {
        public long ChurchServiceId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church Information is required")]
        public long ClientId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Service Name")]
        public string Name { get; set; }

        [CheckNumber(0, ErrorMessage = "Day of Service is required")]
        public int DayOfWeekId { get; set; }
        public int AddedByUserId { get; set; }
    }


    #region Latest Client Church Service

    public class ClientChurchServiceRegistrationObj
    {
        public long ClientChurchServiceId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church Information is required")]
        public long ClientChurchId { get; set; }
        public int ChurchServiceTypeId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Service Name")]
        public string Name { get; set; }
        public List<NameAndValueObject> AllDaysOfWeek { get; set; }
        public int[] SelectedDaysOfWeekIds { get; set; }

        //[CheckNumber(0, ErrorMessage = "Day of Service is required")]
        public int DayOfWeekId { get; set; }
        public int AddedByUserId { get; set; }
    }

    #endregion


    public class ClientChurchServiceRegObj
    {
        public long ClientChurchServiceId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church Information is required")]
        public long ClientChurchId { get; set; }
        public int ChurchServiceTypeId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Service Name")]
        public string Name { get; set; }
        public List<NameAndValueObject> AllDaysOfWeek { get; set; }
        public int[] SelectedDaysOfWeekIds { get; set; }

        //[CheckNumber(0, ErrorMessage = "Day of Service is required")]
        public int DayOfWeekId { get; set; }
        public int AddedByUserId { get; set; }
    }


    



    #region ChurchServiceAttendance

    public class ChurchServiceAttendanceRegObj
    {
        public long ChurchServiceAttendanceId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public long ClientChurchId { get; set; }

        //[CheckNumber(0, ErrorMessage = "Church Service Information is required")]
        //public int ChurchServiceTypeId { get; set; }

        //[CheckNumber(0, ErrorMessage = "Church Service Information is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Church Service Information is required")]
        public string ChurchServiceTypeRefId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Service Theme is required")]
        [DisplayName("Theme")]
        public string ServiceTheme { get; set; }

        [DisplayName("Bible Reading")]
        public string BibleReadingText { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Preacher is required")]
        public string Preacher { get; set; }

        [StringLength(10)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date service hold is required")]
        public string DateServiceHeld { get; set; }

        public ClientChurchServiceAttendanceDetailObj ChurchServiceAttendanceDetail { get; set; }

        //public int NumberOfMen { get; set; }
        //public int NumberOfWomen { get; set; }
        //public int NumberOfChildren { get; set; }
        //public int NewConvert { get; set; }
        //public int FirstTimer { get; set; }
        //public long TotalAttendee { get; set; }


        //public double Offerring { get; set; }
        //public double Tithe { get; set; }
        //public double ThanksGiving { get; set; }
        //public double BuildingProjectFund { get; set; }
        //public double SpecialThanksGiving { get; set; }
        //public double Donation { get; set; }
        //public double FirstFruit { get; set; }
        //public double WelfareCharity { get; set; }
        //public double Others { get; set; }


        public ChurchServiceAttendanceAttendee ChurchServiceAttendanceAttendee { get; set; }
        public ClientChurchCollection ClientChurchCollection { get; set; }
        //public List<ChurchServiceAttendanceCollectionObj> ChurchServiceAttendanceCollections { get; set; }
        public int TakenByUserId { get; set; }

        public ChurchServiceAttendanceRegObj()
        {
            ClientChurchCollection = new ClientChurchCollection(); 
            ChurchServiceAttendanceAttendee = new ChurchServiceAttendanceAttendee();
            ChurchServiceAttendanceDetail = new ClientChurchServiceAttendanceDetailObj();
        }
    }
    
    public class ChurchServiceAttendanceDetailObj
    {
        public int NumberOfMen { get; set; }
        public int NumberOfWomen { get; set; }
        public int NumberOfChildren { get; set; }
        public int NewConvert { get; set; }
        public int FirstTimer { get; set; }

        public List<ChurchServiceAttendanceCollectionObj> ChurchServiceAttendanceCollections { get; set; }

        public ChurchServiceAttendanceDetailObj()
        {
            ChurchServiceAttendanceCollections = new List<ChurchServiceAttendanceCollectionObj>();
        }

    }
    
    public class ChurchServiceAttendanceCollectionObj
    {
        //[CheckNumber(0, ErrorMessage = "Client Church Service Attendance Information is required")]
        //public long ChurchServiceAttendanceId { get; set; }

        //[CheckNumber(0, ErrorMessage = "Collection Type Information is required")]
        public int CollectionTypeId { get; set; }
        // string[] Percentages { get; set; }
        public string CollectionTypeName { get; set; }

        //[CheckAmount(0, ErrorMessage = "Collection Type Amount is required")]
        public double Amount { get; set; }
    }


    public class ClientChurchServiceAttendanceDetailObj
    {
        public int NumberOfMen { get; set; }
        public int NumberOfWomen { get; set; }
        public int NumberOfChildren { get; set; }
        public int NewConvert { get; set; }
        public int FirstTimer { get; set; }

        public List<ClientChurchServiceAttendanceCollectionObj> ClientChurchServiceAttendanceCollections { get; set; }

        public ClientChurchServiceAttendanceDetailObj()
        {
            ClientChurchServiceAttendanceCollections = new List<ClientChurchServiceAttendanceCollectionObj>();
        }

    }

    public class ClientChurchCollectionTypeReportObj
    {
        public long ClientChurchCollectionTypeId { get; set; }
        public long ClientChurchId { get; set; }

        public List<ClientChurchServiceAttendanceCollectionObj> ClientChurchServiceAttendanceCollectionObjs { get; set; }

        public ClientChurchCollectionTypeReportObj()
        {
            ClientChurchServiceAttendanceCollectionObjs = new List<ClientChurchServiceAttendanceCollectionObj>();
        }
    }

    public class ClientChurchServiceAttendanceCollectionObj
    {
        public string CollectionRefId { get; set; }
        public string CollectionTypeName { get; set; }
        public double Amount { get; set; }
    }


    public class ChurchServiceAttendanceRemittanceReportObj
    {
        public long ChurchServiceAttendanceRemittanceId { get; set; }
        public string DateRange { get; set; }
        public long TotalMonthlyAttendee { get; set; }
        public double TotalMonthlyAmountCaptured { get; set; }
        public double TotalMonthlyAmountRemitted { get; set; }
        public double TotalMonthlyBalanceLeft { get; set; }

        public List<ChurchServiceAverageAttendanceObj> RemittanceChurchServiceAverageAttendanceDetail { get; set; }
        public RemittanceChurchServiceMonthlyTotalAttendeeObj RemittanceChurchServiceMonthlyTotalAttendee { get; set; }
        public List<CollectionTypeMonetaryTotalObj> CollectionTypeMonetaryTotals { get; set; }
        public List<CollectionRemittanceChurchStructureTypeTotalObj> ChurchStructureTypeCollectionTotals { get; set; }

        public int RemittanceType { get; set; }
        public RemittanceDetailReportObj RemittanceDetailReport { get; set; }


        public ChurchServiceAttendanceRemittanceReportObj()
        {

            RemittanceChurchServiceAverageAttendanceDetail = new List<ChurchServiceAverageAttendanceObj>();
            ChurchStructureTypeCollectionTotals = new List<CollectionRemittanceChurchStructureTypeTotalObj>();
            RemittanceDetailReport = new RemittanceDetailReportObj
            {
                RemittanceChurchServiceDetails = new List<RemittanceChurchServiceDetailObj>(),
                ChurchServiceAttendanceRemittanceCollections = new List<ChurchServiceAttendanceRemittanceCollectionObj>(),
                CollectionRemittanceDetails = new List<ClientCollectionRemittanceDetailObj>()
            };
        }
        
    }
    
    public class ComputedChurchServiceAttendanceRemittanceObj
    {
        public long ChurchServiceAttendanceRemittanceId { get; set; }
        public long ClientId { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        //CollectionTypeMonetaryTotalObj
        public List<CollectionTypeMonetaryTotalObj> CollectionTypeMonetaryTotalObjs { get; set; }
        public List<RemittanceChurchServiceDetailObj> RemittanceChurchServiceDetail { get; set; }
        public RemittanceChurchServiceMonthlyTotalAttendeeObj RemittanceChurchServiceMonthlyTotalAttendee { get; set; }
        public List<ChurchServiceAverageAttendanceObj> RemittanceChurchServiceAverageAttendanceDetail { get; set; }

        public List<ChurchServiceAttendanceRemittanceCollectionObj> ChurchServiceAttendanceRemittanceCollection { get; set; }
        //ClientCollectionRemittanceDetailObj

        public List<ClientCollectionRemittanceDetailObj> CollectionRemittanceDetail { get; set; }
        //public List<CollectionRemittanceDetailObj> CollectionRemittanceDetail { get; set; }

        public List<CollectionRemittanceChurchStructureTypeTotalObj> CollectionRemittanceChurchStructureTypeTotal { get; set; }

        public string TimeStampRemitted { get; set; }

        public int RemittedByUserId { get; set; }

    }



    public class RemittanceChurchServiceMonthlyTotalAttendeeObj
    {
        public int TotalMen { get; set; }
        public int TotalWomen { get; set; }
        public int TotalChildren { get; set; }
    }
    public class CollectionRemittanceChurchStructureTypeTotalObj
    {
        public int ChurchStructureTypeId { get; set; }
        public double TotalCollectionAmountRemitted { get; set; }
    }
    public class ParishLevelRemittanceDetailReportObj
    {
        public RemittanceDetailReportObj RemittanceDetailReportObj { get; set; }
    }
    public class RemittanceDetailReportObj
    {
        public List<RemittanceChurchServiceDetailObj> RemittanceChurchServiceDetails { get; set; }
        public List<ChurchServiceAttendanceRemittanceCollectionObj> ChurchServiceAttendanceRemittanceCollections { get; set; }
        public List<ClientCollectionRemittanceDetailObj> CollectionRemittanceDetails { get; set; }
    }



    public class RemittanceChurchServiceDetailObj
    {
        public long ChurchServiceAttendanceId { get; set; }

        public int ChurchServiceTypeId { get; set; }
        public string ChurchServiceTypeRefId { get; set; }

        public string ChurchServiceTypeName { get; set; }
        public string DateServiceHeld { get; set; }
        public string Preacher { get; set; }

        //public double TotalPercentRemitted { get; set; }
        //public double TotalBalanceLeft { get; set; }

        public RemittanceChurchServiceAttendeeDetailObj RemittanceChurchServiceAttendeeDetail { get; set; }

        public RemittanceChurchServiceDetailObj()
        {
            RemittanceChurchServiceAttendeeDetail = new RemittanceChurchServiceAttendeeDetailObj();
        }
    }
    
    public class RemittanceChurchServiceAttendeeDetailObj
    {
        public int NumberOfMen { get; set; }
        public int NumberOfWomen { get; set; }
        public int NumberOfChildren { get; set; }
        public int NewConvert { get; set; }
        public int FirstTimer { get; set; }
        public long TotalAttendee { get; set; }
    }

    //collectionTypeMonetaryTotals

    public class CollectionTypeMonetaryTotalObj
    {
        //public int CollectionTypeId { get; set; }
        public string CollectionRefId { get; set; }
        public string CollectionTypeName { get; set; }
        public double TotalRemittance { get; set; }
        public CollectionRemittanceChurchStructureTypeObj MonetaryTotalChurchStructureTypes { get; set; }

        //public List<CollectionRemittanceChurchStructureTypeObj> MonetaryTotalChurchStructureTypes { get; set; }
        //public CollectionTypeMonetaryTotalObj()
        //{
        //    MonetaryTotalChurchStructureTypes = new List<CollectionRemittanceChurchStructureTypeObj>();
        //}
    }


    public class MonetaryTotalCollectionTypeObj
    {
        public int CollectionTypeId { get; set; }
        public string CollectionTypeName { get; set; }
        public double TotalRemittance { get; set; }

        public List<CollectionRemittanceChurchStructureTypeObj> MonetaryTotalChurchStructureTypes { get; set; }

        public MonetaryTotalCollectionTypeObj()
        {
            MonetaryTotalChurchStructureTypes = new List<CollectionRemittanceChurchStructureTypeObj>();
        }
    }



    #region Client Church

    public class ClientChurchServiceAttendanceRemittanceCollectionObj
    {
        public string CollectionRefId { get; set; }
        public string CollectionTypeName { get; set; }
        public double TotalRemittance { get; set; }
    }


    public class ClientCollectionRemittanceDetailObj
    {
        public string CollectionRefId { get; set; }
        public string CollectionTypeName { get; set; }
        public double TotalMonthlyCaptured { get; set; }
        public double TotalPercentRemitted { get; set; }
        public double TotalBalanceLeft { get; set; }
        public double Percent { get; set; }

        public List<CollectionRemittanceChurchStructureTypeObj> CollectionRemittanceChurchStructureType { get; set; }

        public ClientCollectionRemittanceDetailObj()
        {
            CollectionRemittanceChurchStructureType = new List<CollectionRemittanceChurchStructureTypeObj>();
        }
    }

    #endregion


    public class ChurchServiceAttendanceRemittanceCollectionObj
    {
        public int CollectionTypeId { get; set; }
        public string CollectionTypeName { get; set; }
        public double TotalRemittance { get; set; }
    }

    public class CollectionRemittanceDetailObj
    {
        public int CollectionTypeId { get; set; }
        public string CollectionTypeName { get; set; }
        public double TotalMonthlyCaptured { get; set; }
        public double TotalPercentRemitted { get; set; }
        public double TotalBalanceLeft { get; set; }
        public double Percent { get; set; }

        public List<CollectionRemittanceChurchStructureTypeObj> CollectionRemittanceChurchStructureType { get; set; }

        public CollectionRemittanceDetailObj()
        {
            CollectionRemittanceChurchStructureType = new List<CollectionRemittanceChurchStructureTypeObj>();
        }
    }

    public class CollectionRemittanceChurchStructureTypeObj
    {
        public int ChurchStructureTypeId { get; set; }
        public string ChurchStructureTypeName { get; set; }
        public double Percent { get; set; }
        public double AmountRemitted { get; set; }
        public string AmountRemittedCurrency { get; set; }
    }

    

    #endregion
    



    #region Administrative Reports Objs

    public class RegisteredChurchServiceTypeReportObj
    {
        public int ChurchServiceTypeId { get; set; }
        public string Name { get; set; }
        public int SourceId { get; set; }

    }
    public class RegisteredChurchMemberReportObj
    {
        public long ChurchMemberId { get; set; }
        public long ClientChurchId { get; set; }
        public string FullName { get; set; }
        public int ProfessionId { get; set; }
        public string Profession { get; set; }
        public int RoleInChurchId { get; set; }
        public string RoleInChurch { get; set; }
        public string Sex { get; set; }
        public int SexId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string DateJoined { get; set; }
        public string JoinedDate { get; set; }

    }
    public class RegisteredChurchServiceReportObj
    {
        public long ChurchServiceId { get; set; }
        public long ChurchId { get; set; }
        public string ParentChurchName { get; set; }
        public int ChurchServiceTypeId { get; set; }
        public string ChurchServiceName { get; set; }
        public int DayOfWeekId { get; set; }
        public string ServiceDay { get; set; }
        public ChurchServiceStatus Status { get; set; }
        public string StatusName { get; set; }
        public string DateServiceHold { get; set; }

    }
    public class RegisteredClientChurchServiceReportObj
    {
        public long ClientChurchServiceId { get; set; }
        public long ClientChurchId { get; set; }
        public int ChurchServiceTypeId { get; set; }
        public string Name { get; set; }
        public int DayOfWeekId { get; set; }
        public string ServiceDay { get; set; }
        public string DateServiceHold { get; set; }

    }

    public class RegisteredChurchServiceAttendanceReportObj
    {
        public long ChurchServiceAttendanceId { get; set; }
        public long ClientChurchId { get; set; }
        //public int ChurchServiceTypeId { get; set; }
        public string ChurchServiceTypeRefId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTheme { get; set; }
        public string BibleReadingText { get; set; }
        public string Preacher { get; set; }
        
        internal string _ServiceAttendanceDetail { get; set; }
        public long TotalAttendee { get; set; }
        public double TotalCollection { get; set; }

        public ClientChurchServiceAttendanceDetailObj ServiceAttendanceDetail
        {

            //get; set;
            get { return _ServiceAttendanceDetail == null ? null : JsonConvert.DeserializeObject<ClientChurchServiceAttendanceDetailObj>(_ServiceAttendanceDetail); }
            set { _ServiceAttendanceDetail = JsonConvert.SerializeObject(value); }
        }




        public ChurchServiceAttendanceAttendee ChurchServiceAttendanceAttendee { get; set; }
        public ClientChurchCollection ClientChurchCollection { get; set; }

        //public int NumberOfMen { get; set; }
        //public int NumberOfWomen { get; set; }
        //public int NumberOfChildren { get; set; }
        //public int NewConvert { get; set; }
        //public int FirstTimer { get; set; }
        //public long TotalAttendee { get; set; }



        public string DateServiceHeld { get; set; }
        public string ServiceHeldDate { get; set; }
        public string DateAttendanceTaken { get; set; }


    }

    public class RegisteredRoleInChurchReportObj
    {
        public int RoleInChurchId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    #endregion

    

    #endregion

    

    public class ClientStructureChurchRegistrationObj
    {

        #region Personal Info

        public long ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent Church Information is required")]
        public long ChurchId { get; set; }

        #endregion


        #region Contact Info
        
        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }

        //public ClientContact Contact { get; set; }

        #endregion

        #region Church Structure Info

        public List<ClientStructureChurchDetailReportObj> ClientStructureChurchDetails { get; set; }
        //public List<long> AllStructureChurchIds { get; set; }

        //public List<long> ClientStructureChurchIds { get; set; }

        //public long? StructureChurchId { get; set; }

        //public long? ParishId { get; set; }

        public long? RegionId { get; set; }

        public long? ProvinceId { get; set; }

        public long? ZoneId { get; set; }

        public long? AreaId { get; set; }

        public long? DioceseId { get; set; }

        public long? DistrictId { get; set; }

        public long? StateId { get; set; }

        public long? GroupId { get; set; }

        //public List<NameAndValueObject> AllStructures { get; set; }

        //[CheckNumber(0, ErrorMessage = "Church structure type information is required")]
        //public int ChurchStructureTypeId { get; set; }

        public int? HeadQuarterChurchStructureTypeId { get; set; }

        //public Region Region { get; set; }

        //public ClientChurchStructureRegObj Province { get; set; }

        //public ClientChurchStructureRegObj Zone { get; set; }

        //public ClientChurchStructureRegObj Area { get; set; }

        //public ClientChurchStructureRegObj Diocese { get; set; }

        //public ClientChurchStructureRegObj District { get; set; }

        //public ClientChurchStructureRegObj State { get; set; }

        //public ClientChurchStructureRegObj Group { get; set; }

        #endregion

        #region Client Roles
        //public int[] ChurchRoleId { get; set; }

        //public List<NameAndValueObject> AllRoles { get; set; }

        //public int[] ClientRoleId { get; set; }

        //public int[] MyRoleIds { get; set; }

        //public string[] MyRoles { get; set; }
        #endregion


        [Column(TypeName = "varchar")]
        [StringLength(35)]
        public string TimeStampRegistered { get; set; }

        public int RegisteredByUserId { get; set; }
    }
    public class ClientAccountRegistrationObj
    {

        public long ClientAccountId { get; set; }

        //[CheckNumber(0, ErrorMessage = "Client Information is required")]
        public long ChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public long ClientId { get; set; }

        [DisplayName("Bank Name")]
        [CheckNumber(0, ErrorMessage = "Bank Information is required")]
        public int BankId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Account Name")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Account Type")]
        public int AccountTypeId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        public string TimeStampRegistered { get; set; }

        public int RegisteredByUserId { get; set; }
    }

    public class ClientStructureChurchDetailRegObj
    {
        public long ClientStructureChurchDetailId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church Information is required")]
        public long ChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public long ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }  
        
        public int ChurchStructureTypeId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        public string TimeStampRegistered { get; set; }

        public int RegisteredByUserId { get; set; }


        #region Church Structure Info

        public List<long> AllStructureChurchIds { get; set; }

        public List<long> ClientStructureChurchIds { get; set; }

        public long? StructureChurchId { get; set; }

        public long? ParishId { get; set; }

        public long? RegionId { get; set; }

        public long? ProvinceId { get; set; }

        public long? ZoneId { get; set; }

        public long? AreaId { get; set; }

        public long? DioceseId { get; set; }

        public long? DistrictId { get; set; }

        public long? StateId { get; set; }

        public long? GroupId { get; set; }

        public List<NameAndValueObject> AllStructures { get; set; }

        public int? HeadQuarterChurchStructureTypeId { get; set; }


        #endregion

    }

    public class ChurchStructureHierachyRegObj
    {
        public int ChurchStructureHierachyId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church structure information is required")]
        public long ChurchStructureId { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent church information is required")]
        public long ChurchId { get; set; }

        //[CheckNumber(0, ErrorMessage = "Church structure type information is required")]
        //public int ChurchStructureTypeId { get; set; }

        [DisplayName("Structure Type")]
        public string ChurchStructureTypeName { get; set; }

        public int HierachyLevel { get; set; }

        public int LastModificationByUserId { get; set; }

        public int RegisteredByUserId { get; set; }



        #region Church Structure Info

        public int? ParishId { get; set; }

        public int? RegionId { get; set; }

        public int? ProvinceId { get; set; }

        public int? ZoneId { get; set; }

        public int? AreaId { get; set; }

        public int? DioceseId { get; set; }

        public int? DistrictId { get; set; }

        public int? StateId { get; set; }

        public int? GroupId { get; set; }

        #endregion

    }

    public class ChurchStructureHierachyLookUpObj
    {
        public int ChurchStructureTypeId { get; set; }

        public string Name { get; set; }

        public bool Status { get; set; }
    }
    public class ChurchThemeSettingRegObj
    {

        public long ChurchThemeSettingId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church information is required")]
        public long ChurchId { get; set; }

        [DisplayName("Church Theme")]
        public string ThemeColor { get; set; }

        [DisplayName("Church Logo")]
        public string ThemeLogo { get; set; }
        
        public string ThemeLogoPath { get; set; }

        public HttpPostedFileBase UploadedFile { get; set; }

        public int RegisteredByUserId { get; set; }

    }
    public class ChurchStructuresLookUpObj
    {
        public long ChurchStructureId { get; set; }
        public long ChurchId { get; set; }
        public int ChurchStructureTypeId { get; set; }
        public string ChurchStructureTypeName { get; set; }
        public int HierachyLevel { get; set; }
        public bool Status { get; set; }
    }
    public class ChurchServicesRegObj
    {
        public long ChurchServiceId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church Information is required")]
        public long ClientId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Service Name")]
        public string Name { get; set; }

        [CheckNumber(0, ErrorMessage = "Day of Service is required")]
        public int DayOfWeekId { get; set; }
        public int AddedByUserId { get; set; }



        public long[] ChurchServiceIds { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent church information is required")]
        public long ChurchId { get; set; }
        public List<NameAndValueObject> AllChurchServiceTypes { get; set; }
        public int[] MyChurchServiceTypeIds { get; set; }
        public string[] MyChurchServiceTypes { get; set; }
        public string SelectedChurchServiceTypes { get; set; }
        public int HierachyLevel { get; set; }
        public int RegisteredByUserId { get; set; }

    }

    public class ChurchStructureRegObj
    {

        public long[] ChurchStructureIds { get; set; }
        //public long ChurchStructureId { get; set; }
        
        [CheckNumber(0, ErrorMessage = "Parent church information is required")]
        public long ChurchId { get; set; }

        //[CheckNumber(0, ErrorMessage = "Church structure type information is required")]
        //[DisplayName("Structure Type")]
        //public int ChurchStructureTypeId { get; set; }
        public List<NameAndValueObject> AllChurchStructureTypes { get; set; }
        public int[] MyChurchStructureTypeIds { get; set; }
        public string[] MyChurchStructureTypes { get; set; }
        public string SelectedChurchStructureTypes { get; set; }

        //[CheckNumber(0, ErrorMessage = "Hierachy level is required")]
        public int HierachyLevel { get; set; }
        public int RegisteredByUserId { get; set; }

    }

    public class ChurchStructureTypeDetailObj
    {
        public int ChurchStructureTypeId { get; set; }
        public string ChurchStructureTypeName { get; set; }
        public int HierachyLevel { get; set; }
    }


    public class ChurchStructureHierachyResetObj
    {

        //[CheckNumber(0, ErrorMessage = "Church structure information is required")]
        //public List<long> ChurchStructureIds { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent church information is required")]
        public long ChurchId { get; set; }

        //[CheckNumber(0, ErrorMessage = "Church structure type information is required")]
        //public int ChurchStructureTypeId { get; set; }

        [DisplayName("Structure Type")]
        public string ChurchStructureTypeName { get; set; }
        public int HierachyLevel { get; set; }
        public int LastModificationByUserId { get; set; }
        public int RegisteredByUserId { get; set; }


        #region Church Structure Info

        public int? ParishId { get; set; }
        public int? RegionId { get; set; }
        public int? ProvinceId { get; set; }
        public int? ZoneId { get; set; }
        public int? AreaId { get; set; }
        public int? DioceseId { get; set; }
        public int? DistrictId { get; set; }
        public int? StateId { get; set; }
        public int? GroupId { get; set; }
        public int? BranchId { get; set; }


        #endregion

    }



    public class StructureChurchRegistrationObj
    {

        public long StructureChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church Structure Type Information is required")]
        public long ChurchStructureTypeId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church Information is required")]
        public long ChurchId { get; set; }

        [Required(ErrorMessage = "* Required")]
        public string Name { get; set; }

        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        public string TimeStampRegistered { get; set; }

        public int RegisteredByUserId { get; set; }
    }

    public class ChangeChurchStructureStatusContract
    {
        public long ChurchStructureId { get; set; }
        public long ChurchId { get; set; }
        public int ChurchStructureTypeId { get; set; }
        public ChurchStructureStatus Status { get; set; }
        //[Required(ErrorMessage = "* Required")]
        //[DisplayName("User Name")]
        //[StringLength(50)]
        //public string StructureName { get; set; }
        public string ParentChurchName { get; set; }
        public string ChurchStructureTypeName { get; set; }
        public string ProcessType { get; set; }
        public int CallerType { get; set; }

    }




    public class StructureChurchRegObj
    {

        public long StructureChurchId { get; set; }

        public long TypeStructureChurchId { get; set; }

        public long ChurchId { get; set; }

        [Required(ErrorMessage = "* Required")]
        public string Name { get; set; }

        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        public string TimeStampRegistered { get; set; }

        [DisplayName("Structure Type")]
        public int StructureTypeId { get; set; }

        public int RegisteredByUserId { get; set; }
    }

    public class ClientStructureChurchRegObj
    {

        public long StructureId { get; set; }

        public long ChurchId { get; set; }

        public long ClientId { get; set; }

        [Required(ErrorMessage = "* Required")]
        public string Name { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "* Required")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Church Location Address is required")]
        [DisplayName("Church Address")]
        public string Address { get; set; }

        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        public string TimeStampRegistered { get; set; }

        [DisplayName("Structure Type")]
        public ChurchStructureType StructureType { get; set; }

        public int RegisteredByUserId { get; set; }

    }

    #region Reports Objs


    public class RegisteredChurchCollectionTypeListReportObj
    {
        public long ChurchCollectionTypeId { get; set; }
        public long ChurchId { get; set; }
        public int CollectionTypeId { get; set; }
        public string ParentChurchName { get; set; }
        public string ChurchCollectionTypeName { get; set; }
        public PreferenceTypeStatus Status { get; set; }
        public string StatusName { get; set; }
        public int RegisteredByUserId { get; set; }

        public IEnumerable<long> ChurchIds { get; set; }
        public List<Church> Churches { get; set; }
    }

    public class RegisteredChurchReportObj
    {
        public long ChurchId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Founder { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string StateOfLocation { get; set; }
        public int StateOfLocationId { get; set; }
        public string Address { get; set; }
        
    }

    public class RegisteredChurchThemeReportObj
    {
        public long ChurchThemeSettingId { get; set; }
        public long ChurchId { get; set; }
        public string Church { get; set; }
        public string Logo { get; set; }
        public string LogoPath { get; set; }
        public string Color { get; set; }

    }

    public class RegisteredClientReportObj
    {
        public long ClientId { get; set; }
        public long ChurchId { get; set; }
        public string Name { get; set; }
        public string ParentChurchName { get; set; }
        public string Pastor { get; set; }
        public int TitleId { get; set; }
        public string Sex { get; set; }
        public string Title { get; set; }
        public int SexId { get; set; }
        public string ChurchReferenceNumber { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string StateOfLocation { get; set; }
        public int StateOfLocationId { get; set; }

        public string BankName { get; set; }
        public int BankId { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public int AccountTypeId { get; set; }

        public ClientContact Contact { get; set; }

        public ClientAccount Account { get; set; }

        public List<ClientStructureChurchDetail> ClientStructureChurchDetail { get; set; }

        public ChurchStructureHqtr ChurchStructureHqtr { get; set; }

        public ClientStructureChurchRegObj Region { get; set; }

        public ClientStructureChurchRegObj Province { get; set; }

        public ClientStructureChurchRegObj Zone { get; set; }

        public ClientStructureChurchRegObj Area { get; set; }

        public ClientStructureChurchRegObj Diocese { get; set; }

        public ClientStructureChurchRegObj District { get; set; }

        public ClientStructureChurchRegObj State { get; set; }

        public ClientStructureChurchRegObj Group { get; set; }

        public string Username { get; set; }

    }

    public class RegisteredChurchStructureListReportObj
    {
        public long ChurchStructureId { get; set; }
        public long ChurchId { get; set; }
        public int ChurchStructureTypeId { get; set; }
        public int HierachyLevel { get; set; }
        public string ParentChurchName { get; set; }
        public string ChurchStructureTypeName { get; set; }
        public ChurchStructureStatus Status { get; set; }
        public string StatusName { get; set; }
        public int RegisteredByUserId { get; set; }

        public IEnumerable<long> ChurchIds { get; set; }
        public List<Church> Churches { get; set; }
    }
    

    public class RegisteredClientStructureChurchListReportObj
    {
        public long ClientId { get; set; }
        public long ChurchId { get; set; }
        public string ParentChurchName { get; set; }
        public string Name { get; set; }

        public long StructureChurchId { get; set; }
        public string StructureChurchName { get; set; }
        public string StructureChurchType { get; set; }
        public List<ClientStructureChurchDetail> ClientStructureChurchDetail { get; set; }


        public ChurchStructureHqtr ChurchStructureHqtr { get; set; }

        public int HeadQuarterChurchStructureTypeId { get; set; }

        public Region Region { get; set; }

        public Province Province { get; set; }

        public Zone Zone { get; set; }

        public Area Area { get; set; }

        public Diocese Diocese { get; set; }

        public District District { get; set; }

        public State State { get; set; }

        public Group Group { get; set; }
        
    }


    public class ClientStructureChurchDetailReportObj
    {
        public long ClientId { get; set; }
        public long StructureChurchId { get; set; }
        public string StructureChurchName { get; set; }
        public long StructureChurchTypeId { get; set; }
        public string StructureChurchType { get; set; }
        public Region Region { get; set; }

        public Province Province { get; set; }

        public Zone Zone { get; set; }

        public Area Area { get; set; }

        public Diocese Diocese { get; set; }

        public District District { get; set; }

        public State State { get; set; }

        public Group Group { get; set; }
    }

    public class ThisClientRegisteredStructureChurchListReportObj
    {
        //public long ClientId { get; set; }
        //public long ChurchId { get; set; }
        //public string ParentChurchName { get; set; }
        //public string Name { get; set; }
        public ChurchStructureHqtr ChurchStructureHqtr { get; set; }
        public int HeadQuarterChurchStructureTypeId { get; set; }
        public List<ClientStructureChurchDetailReportObj> ClientStructureChurchDetails { get; set; }
    }



    public class RegisteredChurchStructureHierachyReportObj
    {
        public int ChurchStructureHierachyId { get; set; }
        public long ChurchStructureId { get; set; }
        public long ChurchId { get; set; }
        public int ChurchStructureTypeId { get; set; }
        public string ParentChurchName { get; set; }
        public string ChurchStructureTypeName { get; set; }
        public int HierachyLevel { get; set; }
        public int RegisteredByUserId { get; set; }
        public IEnumerable<long> ChurchIds { get; set; }
        public List<Church> Churches { get; set; }
    }

    public class RegisteredStructureChurchReportObj
    {
        public long StructureChurchId { get; set; }

        public long TypeStructureChurchId { get; set; }

        public string StructureChurchName { get; set; }

        public int StateOfLocationId { get; set; }

        public string StateOfLocationName { get; set; }

        public long ChurchId { get; set; }

        public int ChurchStructureTypeId { get; set; }

        public string ParentChurchName { get; set; }

        public string ChurchStructureTypeName { get; set; }


        public int RegisteredByUserId { get; set; }
    }

    public class RegisteredClientListReportObj
    {
        public long ClientId { get; set; }
        public long ChurchId { get; set; }
        public string Name { get; set; }
        public string ParentChurchName { get; set; }
        public string ParentChurchShortName { get; set; }
        public string Pastor { get; set; }
        public int TitleId { get; set; }
        public string Sex { get; set; }
        public string Title { get; set; }
        public int SexId { get; set; }
        public string ChurchReferenceNumber { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string StateOfLocation { get; set; }
        public int StateOfLocationId { get; set; }

        public List<ClientStructureChurchDetailReportObj> ClientStructureChurchDetails { get; set; }

        //public List<ThisClientRegisteredStructureChurchListReportObj> ClientStructureChurchDetails { get; set; }

        //public ClientContact Contact { get; set; }

        //public ClientAccount Account { get; set; }

        //public List<ClientStructureChurchDetail> ClientStructureChurchDetail { get; set; }

        public ChurchStructureHqtr ChurchStructureHqtr { get; set; }

        public int HeadQuarterChurchStructureTypeId { get; set; }

        //public Region Region { get; set; }

        //public Province Province { get; set; }

        //public Zone Zone { get; set; }

        //public Area Area { get; set; }

        //public Diocese Diocese { get; set; }

        //public District District { get; set; }

        //public State State { get; set; }

        //public Group Group { get; set; }

        //public string Username { get; set; }

    }

    public class RegisteredClientAccountListReportObj
    {
        public long ClientAccountId { get; set; }
        public long ChurchId { get; set; }
        public string Church { get; set; }
        public long ClientId { get; set; }
        public string Client { get; set; }
        public int BankId { get; set; }
        public string Bank { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public int AccountTypeId { get; set; }
        public string AccountType { get; set; }
        public int RegisteredByUserId { get; set; }
        public ClientAccountStatus Status { get; set; }
        public string StatusType { get; set; }

    }



    public class RegisteredClientStructureChurchListReportObjx
    {
        public long ClientId { get; set; }

        public long ChurchId { get; set; }

        public long StructureChurchId { get; set; }

        public string StructureChurchName { get; set; }

        public int ChurchStructureTypeId { get; set; }

        public string ParentChurchName { get; set; }

        public string ChurchStructureTypeName { get; set; }

        public ChurchStructureStatus Status { get; set; }

        public string StatusName { get; set; }

        public int RegisteredByUserId { get; set; }
    }


    #region Old Registered Client Report Obj

    


    public class RegisteredClientListReportObjxx
    {
        public long ClientId { get; set; }
        public long ChurchId { get; set; }
        public string Name { get; set; }
        public string ParentChurchName { get; set; }
        public string Pastor { get; set; }
        public int TitleId { get; set; }
        public string Sex { get; set; }
        public string Title { get; set; }
        public int SexId { get; set; }
        public string ChurchReferenceNumber { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string StateOfLocation { get; set; }
        public int StateOfLocationId { get; set; }

        //public string BankName { get; set; }
        //public int BankId { get; set; }
        //public string AccountName { get; set; }
        //public string AccountNumber { get; set; }
        //public string AccountType { get; set; }
        //public int AccountTypeId { get; set; }

        //public ClientContact Contact { get; set; }

        //public ClientAccount Account { get; set; }

        public List<ClientStructureChurchDetail> ClientStructureChurchDetail { get; set; }

        public ChurchStructureHqtr ChurchStructureHqtr { get; set; }

        public int HeadQuarterChurchStructureTypeId { get; set; }

        public Region Region { get; set; }

        public Province Province { get; set; }

        public Zone Zone { get; set; }

        public Area Area { get; set; }

        public Diocese Diocese { get; set; }

        public District District { get; set; }

        public State State { get; set; }

        public Group Group { get; set; }

        public string Username { get; set; }

    }


    #endregion



    public class ClientRegistrationObjx
    {

        #region Personal Info

        public long ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent Church Information is required")]
        public long ChurchId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Name is required", AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Pastor/Leader")]
        public string Pastor { get; set; }

        [Required(ErrorMessage = "Title is required", AllowEmptyStrings = false)]
        public int Title { get; set; }

        [Range(1, 2)]
        public int Sex { get; set; }

        public string ChurchReferenceNumber { get; set; }

        #endregion


        #region Contact Info

        [StringLength(15)]
        [Required(ErrorMessage = "* Required")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Church Location Address is required")]
        [DisplayName("Church Address")]
        public string Address { get; set; }

        //public ClientContact Contact { get; set; }

        #endregion

        #region Church Structure Info

        //public List<long> AllStructureChurchIds { get; set; }

        //public List<long> ClientStructureChurchIds { get; set; }

        //public long? StructureChurchId { get; set; }

        //public long? ParishId { get; set; }

        //public long? RegionId { get; set; }

        //public long? ProvinceId { get; set; }

        //public long? ZoneId { get; set; }

        //public long? AreaId { get; set; }

        //public long? DioceseId { get; set; }

        //public long? DistrictId { get; set; }

        //public long? StateId { get; set; }

        //public long? GroupId { get; set; }

        //public List<NameAndValueObject> AllStructures { get; set; }

        //[CheckNumber(0, ErrorMessage = "Church structure type information is required")]
        //public int ChurchStructureTypeId { get; set; }

        public int? HeadQuarterChurchStructureTypeId { get; set; }

        //public Region Region { get; set; }

        //public ClientChurchStructureRegObj Province { get; set; }

        //public ClientChurchStructureRegObj Zone { get; set; }

        //public ClientChurchStructureRegObj Area { get; set; }

        //public ClientChurchStructureRegObj Diocese { get; set; }

        //public ClientChurchStructureRegObj District { get; set; }

        //public ClientChurchStructureRegObj State { get; set; }

        //public ClientChurchStructureRegObj Group { get; set; }

        #endregion

        #region Client Roles
        //public int[] ChurchRoleId { get; set; }

        //public List<NameAndValueObject> AllRoles { get; set; }

        //public int[] ClientRoleId { get; set; }

        //public int[] MyRoleIds { get; set; }

        //public string[] MyRoles { get; set; }
        #endregion


        [Column(TypeName = "varchar")]
        [StringLength(35)]
        public string TimeStampRegistered { get; set; }

        public int RegisteredByUserId { get; set; }
    }



    #endregion

    


}
 