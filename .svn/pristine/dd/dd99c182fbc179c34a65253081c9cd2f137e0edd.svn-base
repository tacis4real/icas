using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ICASStacks.DataContract;

namespace ICASStacks.APIObjs
{

    public class ErrorMsgObj
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
    }


    public class RespStatus
    {
        public bool IsSuccessful;
        public RespMessage Message;
    }

    public class RespMessage
    {
        public string FriendlyMessage;
        public string TechnicalMessage;
        public string ErrorCode;
    }

    #region Church Administrative

    public class ChurchServiceTypeRegResponse
    {
        public int ChurchServiceTypeId { get; set; }
        public string Name { get; set; }

        public RespStatus Status;
    }

    public class ChurchMemberRegResponse
    {
        public long ChurchMemberId { get; set; }
        public long ClientChurchId { get; set; }
        public string FullName { get; set; }
        public int RoleInChurchId { get; set; }

        public RespStatus Status;
    }

    public class ChurchServiceRegResponse
    {
        public long ChurchServiceId { get; set; }
        public long ClientId { get; set; }
        public string Name { get; set; }
        public int DayOfWeekId { get; set; }

        public RespStatus Status;
    }

    public class ParentChurchServiceRegResponse
    {
        public long ChurchId { get; set; }
        public List<ParentChurchServiceObj> ChurchServices { get; set; }

        public RespStatus Status;
    }


    public class ParentChurchServiceObj
    {
        public long ChurchServiceId { get; set; }
        public long ChurchServiceTypeId { get; set; }
        public string SelectedDayOfWeek { get; set; }

    }


    public class ClientChurchServiceRegResponse
    {
        public long ClientChurchServiceId { get; set; }
        public int ChurchServiceTypeId { get; set; }
        public long ClientChurchId { get; set; }
        public string Name { get; set; }
        public int DayOfWeekId { get; set; }

        public RespStatus Status;
    }

    public class ChurchServiceAttendanceRegResponse
    {
        public long ChurchServiceAttendanceId { get; set; }
        public long ClientId { get; set; }
        public string ChurchServiceTypeRefId { get; set; }
        //public int ChurchServiceTypeId { get; set; }
        public long TotalAttendee { get; set; }

        public RespStatus Status;
    }

    public class UploadChurchMemberRegResponseObj
    {
        public long ChurchMemberId;
        public string FullName;
        public int RecordId;
        public RespStatus Status;
    }

    public class BulkChurchMemberRegResponseObj
    {
        public List<UploadChurchMemberRegResponseObj> ChurchMemberRegResponses;
        public RespStatus MainStatus;
    }

    public class RemoveChurchServiceContract
    {

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Service Name")]
        public string SeriveName { get; set; }
        public string ProcessType { get; set; }
        public long ChurchServiceId { get; set; }
        public int CallerType { get; set; }

    }

    public class RemoveParentChurchServiceContract
    {

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Service Name")]
        public string ServiceName { get; set; }
        public string ProcessType { get; set; }
        public long ChurchServiceId { get; set; }
        public int CallerType { get; set; }

    }

    public class RemoveClientChurchServiceContract
    {

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Service Name")]
        public string SeriveName { get; set; }
        public string ProcessType { get; set; }
        public long ChurchServiceId { get; set; }
        public int CallerType { get; set; }

    }

    #endregion

    #region Clients


    public class ChurchStructureParishHqtrRegResponse
    {
        public long ChurchStructureParishHeadQuarterId { get; set; }
        public long ChurchId { get; set; }
        public long ChurchStructureTypeId { get; set; }
        public int StateOfLocationId { get; set; }

        public RespStatus Status;
    }



    public class ClientChurchRegResponse
    {
        public long ClientChurchId { get; set; }
        public long ChurchId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ChurchReferenceNumber { get; set; }

        public RespStatus Status;
    }

    public class ClientRegResponse
    {
        public long ClientId { get; set; }
        public long ChurchId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ChurchReferenceNumber { get; set; }

        public RespStatus Status;
    }

    public class ClientStructureChurchRegResponse
    {
        public long ClientId { get; set; }
        public long ChurchId { get; set; }
        public int StructureTypeId { get; set; }
        public long ChurchStructureHqtrId { get; set; }
        public List<ClientStructureChurchDetail> ClientChurchStructureDetails { get; set; }

        public RespStatus Status;
    }

    public class ClientAccountRegResponse
    {
        public long ClientAccountId { get; set; }
        public long ClientId { get; set; }
        public int BankId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string AccountBank { get; set; }
        public int AccountTypeId { get; set; }

        public RespStatus Status;
    }



    public class ChurchStructureHqtrRegResponse
    {
        public long ChurchStructureHqtrId { get; set; }
        public long ChurchId { get; set; }
        public long ClientId { get; set; }

        public RespStatus Status;
    }


    public class StructureChurchRegResponse
    {
        public long StructureId { get; set; }
        public long ClientId { get; set; }
        public long ChurchId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int StructureTypeId { get; set; }
        public ChurchStructureType StructureType { get; set; }
        //public string ChurchReferenceNumber { get; set; }

        public RespStatus Status;
    }


    public class StructureChurchMain
    {
        public long StructureChurchMainId { get; set; }
        public long ChurchId { get; set; }
        //public int StructureTypeId { get; set; }
        public int StateOfLocationId { get; set; }
        public string Name { get; set; }
        public string TimeStampRegistered { get; set; }
        public int RegisteredByUserId { get; set; }

    }

    public class ChurchStructureHierachyRegResponse
    {
        public int ChurchStructureHierachyId { get; set; }
        public RespStatus Status;
    }

    public class ChurchStructureRegResponse
    {
        public long ChurchStructureId { get; set; }
        public long ChurchId { get; set; }
        public RespStatus Status;
    }

    public class ChurchStructureResponseObj
    {
        public long ChurchStructureId { get; set; }
        public long ChurchId { get; set; }
        public string NewStatus { get; set; }
        public RespStatus Status;
    }


    public class ClientStructureTaskResponseObj
    {
        public long AdminUserId { get; set; }
        public long ClientId { get; set; }
        public RespStatus Status;
    }
    
    #endregion

    #region Churches



    public class ChurchRegResponse
    {
        public long ChurchId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int StateOfLocationId { get; set; }

        public RespStatus Status;
    }
    
    public class ChurchThemeSettingRegResponse
    {
        public long ChurchThemeSettingId { get; set; }
        public long ChurchId { get; set; }
        public string ThemeColor { get; set; }
        public string ThemeLogo { get; set; }

        public RespStatus Status;
    }




    #region Parent Church Colletion Types

    public class ParentChurchCollectionTypeRegResponse
    {
        public long ChurchCollectionTypeId { get; set; }
        public long ChurchId { get; set; }

        public RespStatus Status;
    }

    public class ParentChurchCollectionTypeResponseObj
    {
        public long ChurchCollectionTypeId { get; set; }
        public long ChurchId { get; set; }
        public string NewStatus { get; set; }

        public RespStatus Status;
    }


    public class RemoveParentChurchCollectionTypeContract
    {

        public long ChurchId { get; set; }
        public long ChurchCollectionTypeId { get; set; }
        public int CollectionTypeId { get; set; }
        public string ParentChurchName { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Collection Type Name")]
        public string CollectionTypeName { get; set; }
        public string ProcessType { get; set; }
        public int CallerType { get; set; }

    }


    #endregion


    


    #endregion


    #region Editing

    public class ClientModificationResponse
    {

        public long ClientId { get; set; }
        public long ChurchId { get; set; }
        public string Name { get; set; }
        public string ParentChurchName { get; set; }
        public string PhoneNumber { get; set; }
        public string ChurchReferenceNumber { get; set; }

        public RespStatus Status;
    }
    #endregion


    #region Dashoboard

    public class DashboardObj
    {
        public long Client { get; set; }
        public int ParentChurch { get; set; }
        public long ChurchMember { get; set; }
    }


    public class ClientDashboardObj
    {
        public long ChurchMember { get; set; }
        public long MaleMember { get; set; }
        public long FemaleMember { get; set; }
        public double MalePercent { get; set; }
        public double FemalePercent { get; set; }
        public int ChurchService { get; set; }

    }
    #endregion
}
