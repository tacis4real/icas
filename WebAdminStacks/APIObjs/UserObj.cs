using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAdminStacks.Common;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace WebAdminStacks.APIObjs
{
    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "Password and Confirm Password must match.")]
    public class UserRegistrationObj
    {
        public long UserId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Surname is required", AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Surname must be between 2 and 100 characters")]
        public string Surname { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Othernames are required", AllowEmptyStrings = false)]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Othernames must be between 2 and 100 characters")]
        public string Othernames { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Mobile Number must be between 7 and 15 digits")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mobile Number is required")]
        //[CheckMobileNumber(ErrorMessage = "Invalid Mobile Number")]
        public string MobileNumber { get; set; }

        [Range(1, 2)]
        public int Sex { get; set; }

        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, ErrorMessage = "Username cannot be longer than 20 character length")]
        public string Username { get; set; }

        [Required]
        [StringLength(150)]
        [DataType(DataType.Password)]
        [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
        public string Password { get; set; }

        [Required]
        [StringLength(150)]
        [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public int RegisteredByUserId { get; set; }

        [StringLength(50)]
        public string DeviceSerialNumber { get; set; }

        [StringLength(50)]
        public string DeviceName { get; set; }

        public string SelectedRoles { get; set; }
        public int[] MyRoleIds { get; set; }

        public string[] MyRoles { get; set; }
        public bool IsActive { get; set; }
    }
    
    public class UserEditObj
    {

        [Required(ErrorMessage = "User Id is required")]
        [CheckNumber(0, ErrorMessage = "Invalid User Information")]
        public int UserId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Surname is required", AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Surname must be between 2 and 100 characters")]
        public string Surname { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Othernames are required", AllowEmptyStrings = false)]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Othernames must be between 2 and 100 characters")]
        public string Othernames { get; set; }

        [Required(ErrorMessage = "Sex is required")]
        [Range(1, 2, ErrorMessage = "Selected Sex is invalid")]
        public int Sex { get; set; }


        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Supplied email address is not valid.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Mobile Number is required", AllowEmptyStrings = false)]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Mobile number must be between 7 amd 15 digits")]
        [CheckMobileNumber(ErrorMessage = "Invalid Mobile Number")]
        public string MobileNumber { get; set; }

        public bool IsApproved { get; set; }


        [Required(ErrorMessage = "Invalid Request Information")]
        [CheckNumber(0, ErrorMessage = "Invalid Registrar Information")]
        public int RegisteredByUserId { get; set; }

        [Required(ErrorMessage = "Device Id is required")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Invalid Device Id")]
        public string DeviceId { get; set; }

        [StringLength(50)]
        public string DeviceName { get; set; }

        [Required(ErrorMessage = "Invalid Request Session! Please re-login")]
        [StringLength(32, MinimumLength = 32, ErrorMessage = "Invalid Request Session! Please re-login")]
        public string RegistrarToken { get; set; }


    }
    
    public class UserDeviceRegObj
    {

        [Required(ErrorMessage = "User is required")]
        [CheckNumber(0, ErrorMessage = "Invalid User Information")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "Device Serial Number is required")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Invalid Serial Number")]
        public string DeviceSerialNumber { get; set; }

        [StringLength(50)]
        public string DeviceName { get; set; }


        [Required(ErrorMessage = "Mobile Number is required", AllowEmptyStrings = false)]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Mobile Number must be between 7 and 15 digits")]
        [CheckMobileNumber(ErrorMessage = "Invalid Mobile Number")]
        public string MobileNumber { get; set; }



        [StringLength(600)]
        public string NotificationToken { get; set; }

    }

    public class UserNotificationTokenObj
    {

        [Required(ErrorMessage = "User Id is required")]
        [CheckNumber(0, ErrorMessage = "Invalid User Information")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Invalid User Device Id")]
        [CheckNumber(0, ErrorMessage = "Invalid User Device Id")]
        public int UserDeviceId { get; set; }

        [Required(ErrorMessage = "Device Id is required")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Invalid Device Id")]
        public string DeviceSerialNumber { get; set; }

        [StringLength(800)]
        [Required(ErrorMessage = "Notification Token is required")]
        public string NotificationToken { get; set; }

    }

    public class UserAuthorizeCodeObj
    {

        [Required(ErrorMessage = "User Id is required")]
        [CheckNumber(0, ErrorMessage = "Invalid User Information")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Invalid User Device Id")]
        [CheckNumber(0, ErrorMessage = "Invalid User Device Id")]
        public int UserDeviceId { get; set; }

        [Required(ErrorMessage = "Device Id is required")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Invalid Device Id")]
        public string DeviceSerialNumber { get; set; }

        [StringLength(10)]
        [Required(ErrorMessage = "Authorization Code is required")]
        public string AuthorizationCode { get; set; }

    }
    
    public class RegisteredUserReportObj
    {
        public long UserId { get; set; }
        public string Othernames { get; set; }
        public string Surname { get; set; }
        public string MiddleName { get; set; }
        public string Sex { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsApproved { get; set; }
        public int SexId { get; set; }
        public string SelectedRoles { get; set; }
        public string[] MyRoles { get; set; }
        public int[] MyRoleIds { get; set; }
        public string PasswordChangeTimeStamp { get; set; }
        public string LastLoginTimeStamp { get; set; }
        public string LastLockedOutTimeStamp { get; set; }
        public int FailedPasswordCount { get; set; }
        public string TimeStampRegistered { get; set; }
        public bool IsPasswordChangeRequired { get; set; }

    }


    #region Client

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "Password and Confirm Password must match.")]
    public class ClientProfileRegistrationObj
    {
        public long ClientChurchProfileId { get; set; }
        public long ClientChurchId { get; set; }
        public string Fullname { get; set; }
        public string MobileNumber { get; set; }
        public int Sex { get; set; }

        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [StringLength(20, ErrorMessage = "Username cannot be longer than 20 character length")]
        public string Username { get; set; }

        [Required]
        [StringLength(150)]
        [DataType(DataType.Password)]
        [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
        public string Password { get; set; }

        [Required]
        [StringLength(150)]
        [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public int RegisteredByUserId { get; set; }

        [StringLength(50)]
        public string DeviceSerialNumber { get; set; }

        [StringLength(50)]
        public string DeviceName { get; set; }

        public List<NameAndValueObject> AllRoles { get; set; }

        [UIHint("MultiLookup")]
        public IEnumerable<int> Roles { get; set; }
        public string SelectedRoles { get; set; }
        public int[] MyRoleIds { get; set; }
        public string[] MyRoles { get; set; }
        public bool IsActive { get; set; }

    }

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "Password and Confirm Password must match.")]
    public class ClientProfileRegObj
    {
        public long ClientProfileId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client/Church Information is required")]
        public long ClientId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Fullname is required", AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Fullname must be between 2 and 100 characters")]
        public string Fullname { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Mobile Number must be between 7 and 15 digits")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mobile Number is required")]
        //[CheckMobileNumber(ErrorMessage = "Invalid Mobile Number")]
        public string MobileNumber { get; set; }

        [Range(1, 2)]
        public int Sex { get; set; }

        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, ErrorMessage = "Username cannot be longer than 20 character length")]
        public string Username { get; set; }
        
        [Required]
        [StringLength(150)]
        [DataType(DataType.Password)]
        [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
        public string Password { get; set; }

        [Required]
        [StringLength(150)]
        [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public int RegisteredByUserId { get; set; }

        [StringLength(50)]
        public string DeviceSerialNumber { get; set; }

        [StringLength(50)]
        public string DeviceName { get; set; }

        public string SelectedRoles { get; set; }
        public int[] MyRoleIds { get; set; }

        public string[] MyRoles { get; set; }
        public bool IsActive { get; set; }

    }

    public class RegisteredClientProfileReportObj
    {
        public long ClientProfileId { get; set; }
        public long ChurchId { get; set; }
        public string ParentChurch { get; set; }
        public string ParentChurchShortName { get; set; }
        public long ClientId { get; set; }
        public string Client { get; set; }
        public string Fullname { get; set; }
        public string MobileNumber { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsApproved { get; set; }
        public int SexId { get; set; }
        public string SelectedRoles { get; set; }
        public int[] MyRoleIds { get; set; }
        public string[] MyRoles { get; set; }
        public string PasswordChangeTimeStamp { get; set; }
        public string LastLoginTimeStamp { get; set; }
        public string LastLockedOutTimeStamp { get; set; }
        public int FailedPasswordCount { get; set; }
        public string TimeStampRegistered { get; set; }
        public bool IsPasswordChangeRequired { get; set; }

    }

    #endregion

    #region Client Church

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "Password and Confirm Password must match.")]
    public class ClientChurchProfileRegistrationObj
    {
        public long ClientChurchProfileId { get; set; }
        public long ClientChurchId { get; set; }
        public string Fullname { get; set; }
        public string MobileNumber { get; set; }
        public int Sex { get; set; }

        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [StringLength(20, ErrorMessage = "Username cannot be longer than 20 character length")]
        public string Username { get; set; }

        [Required]
        [StringLength(150)]
        [DataType(DataType.Password)]
        [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
        public string Password { get; set; }

        [Required]
        [StringLength(150)]
        [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public int RegisteredByUserId { get; set; }

        [StringLength(50)]
        public string DeviceSerialNumber { get; set; }

        [StringLength(50)]
        public string DeviceName { get; set; }
        public List<NameAndValueObject> AllRoles { get; set; }
        public string SelectedRoles { get; set; }
        public int[] MyRoleIds { get; set; }

        public string[] MyRoles { get; set; }
        public bool IsActive { get; set; }

    }

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "Password and Confirm Password must match.")]
    public class ClientChurchProfileRegObj
    {
        public long ClientChurchProfileId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client/Church Information is required")]
        public long ClientChurchId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Fullname is required", AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Fullname must be between 2 and 100 characters")]
        public string Fullname { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Mobile Number must be between 7 and 15 digits")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mobile Number is required")]
        public string MobileNumber { get; set; }

        [Range(1, 2)]
        public int Sex { get; set; }

        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, ErrorMessage = "Username cannot be longer than 20 character length")]
        public string Username { get; set; }

        [Required]
        [StringLength(150)]
        [DataType(DataType.Password)]
        [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
        public string Password { get; set; }

        [Required]
        [StringLength(150)]
        [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public int RegisteredByUserId { get; set; }

        [StringLength(50)]
        public string DeviceSerialNumber { get; set; }

        [StringLength(50)]
        public string DeviceName { get; set; }

        public string SelectedRoles { get; set; }
        public int[] MyRoleIds { get; set; }

        public string[] MyRoles { get; set; }
        public bool IsActive { get; set; }

    }

    public class RegisteredClientChurchProfileReportObj
    {
        public long ClientChurchProfileId { get; set; }
        public long ChurchId { get; set; }
        public string ParentChurch { get; set; }
        public string ParentChurchShortName { get; set; }
        public long ClientChurchId { get; set; }
        public string ClientChurch { get; set; }
        public string Fullname { get; set; }
        public string MobileNumber { get; set; }
        public string Sex { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsApproved { get; set; }
        public int SexId { get; set; }
        public string SelectedRoles { get; set; }
        public int[] MyRoleIds { get; set; }
        public string[] MyRoles { get; set; }
        public string PasswordChangeTimeStamp { get; set; }
        public string LastLoginTimeStamp { get; set; }
        public string LastLockedOutTimeStamp { get; set; }
        public int FailedPasswordCount { get; set; }
        public string TimeStampRegistered { get; set; }
        public bool IsPasswordChangeRequired { get; set; }

    }

    #endregion
}
