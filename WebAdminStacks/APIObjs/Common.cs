namespace WebAdminStacks.APIObjs
{
    
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
    
    public class UserRegResponse
    {
        public long UserId { get; set; }
        public string MobileNumber { get; set; }
        public string Surname { get; set; }
        public string Othernames { get; set; }
        public string Email { get; set; }

        public RespStatus Status;
    }

    public class NotificationResponseObj
    {
        public int AgentId { get; set; }
        public int AgentUserId { get; set; }
        public string MobileNumber { get; set; }
        public string DeviceSerialNumber { get; set; }
        public string Email { get; set; }
        public RespStatus Status;
        public string AgentNumber { get; set; }

    }

    public class AdminTaskResponseObj
    {
        public long AdminUserId { get; set; }
        public long BeneficiaryUserId { get; set; }
        public string NewPassword { get; set; }
        public RespStatus Status;
    }

    public class UserResponseObj
    {

        public long UserId { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string NewPassword { get; set; }
        public RespStatus Status;
        public string AuthToken { get; set; }

    }

    public class UserDeviceResponseObj
    {
        public long UserId { get; set; }
        public long UserDeviceId { get; set; }
        public string MobileNumber { get; set; }
        public string AuthorizationCode { get; set; }
        public RespStatus Status;
    }

    public class UserLoginResponseObj
    {

        public long UserId { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string NewPassword { get; set; }
        public RespStatus Status;
        public string AuthToken { get; set; }
        public string Othernames { get; set; }
        public string Surname { get; set; }
        public int RoleId { get; set; }
        public bool IsFirstTimeAccess { get; set; }
        public string Username { get; set; }

    }

    public class PaymentResponseObj
    {
        public int AgentId { get; set; }
        public string AgentNumber { get; set; }
        public float CurrentBalance { get; set; }
        public float CreditedAmount { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string TechnicalError { get; set; }
        public string FriendlyError { get; set; }
        public string ErrorCode { get; set; }
        public string PaymentReference { get; set; }
        public int UserId { get; set; }
        public bool IsSuccessful { get; set; }
    }

    public class PaymentTransactionObj
    {
        public int AgentId { get; set; }
        public int AgentUserId { get; set; }
        public long OnlineTransactionId { get; set; }
        public float Amount { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string TechnicalError { get; set; }
        public string FriendlyError { get; set; }
        public string ErrorCode { get; set; }
        public string TransactionReference { get; set; }
        public string AgentNumber { get; set; }
        public bool IsSuccessful { get; set; }
    }

    public class CreditTransferResponseObj
    {
        public int SourceAgentId { get; set; }
        public int DestinationAgentId { get; set; }
        public long TransferedByAgentUserId { get; set; }
        public float AmountTransfered { get; set; }
        public float SourceAccountBalance { get; set; }
        public float DestinationAccountBalance { get; set; }
        public string DeviceSerialNo { get; set; }
        public string Email { get; set; }
        public string TechnicalError { get; set; }
        public string FriendlyError { get; set; }
        public string ErrorCode { get; set; }
        public string TransactionReference { get; set; }
        public bool IsSuccessful { get; set; }
    }



    #region Client

    public class ClientLoginResponseObj
    {
        public long ClientProfileId { get; set; }
        public long ClientId { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string NewPassword { get; set; }
        public RespStatus Status;
        public string AuthToken { get; set; }
        public string Fullname { get; set; }
        public int RoleId { get; set; }
        public bool IsFirstTimeAccess { get; set; }
        public string Username { get; set; }
    }


    #region Client Church

    public class ClientChurchLoginResponseObj
    {
        public long ClientChurchProfileId { get; set; }
        public long ClientChurchId { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string NewPassword { get; set; }
        public RespStatus Status;
        public string AuthToken { get; set; }
        public string Fullname { get; set; }
        public int RoleId { get; set; }
        public bool IsFirstTimeAccess { get; set; }
        public string Username { get; set; }
    }

    public class ClientChurchProfileRegResponse
    {
        public long ClientChurchProfileId { get; set; }
        public string MobileNumber { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }

        public RespStatus Status;
    }

    public class ClientChurchResponseObj
    {

        public long ClientChurchProfileId { get; set; }
        public string Username { get; set; }
        public string MobileNumber { get; set; }
        public string NewPassword { get; set; }
        public RespStatus Status;
        public string AuthToken { get; set; }

    }



    #endregion
    

    public class ClientProfileRegResponse
    {
        public long ClientProfileId { get; set; }
        public string MobileNumber { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public RespStatus Status;
    }

    public class ClientResponseObj
    {

        public long ClientProfileId { get; set; }
        public string Username { get; set; }
        public string MobileNumber { get; set; }
        public string NewPassword { get; set; }
        public RespStatus Status;
        public string AuthToken { get; set; }

    }

    

    #endregion
}
