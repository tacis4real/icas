using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAdminStacks.Common;

namespace WebAdminStacks.DataContract
{
    [Table("ICASWebAdmin.User")]
    public class User
    {
        public User()
        {
            UserDevices = new HashSet<UserDevice>();
            UserLoginActivities = new HashSet<UserLoginActivity>();
            UserRoles = new HashSet<UserRole>();
        }


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

        [Column(TypeName = "varchar")]
        [StringLength(20)]
        [Required(ErrorMessage = "Login User name is required")]
        [Index("UQ_User_Username", IsUnique = true)]
        public string Username { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200)]
        [Required(ErrorMessage = "Login Password is required")]
        public string UserCode { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200)]
        [Required(ErrorMessage = "Login Password is required")]
        public string AccessCode { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200)]
        [Required(ErrorMessage = "Login Password is required")]
        public string Password { get; set; }

        public bool IsLockedOut { get; set; }
        public bool IsApproved { get; set; }
        public bool IsFirstTimeLogin { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        public string PasswordChangeTimeStamp { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        public string LastLoginTimeStamp { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        public string LastLockedOutTimeStamp { get; set; }

        [Column(TypeName = "varchar")]
        [Required]
        [StringLength(35)]
        public string TimeStampRegistered { get; set; }
        
        public int FailedPasswordCount { get; set; }

        public bool IsPasswordChangeRequired { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsWebActive { get; set; }
        public bool IsMobileActive { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int RegisteredByUserId { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsMobileNumberVerified { get; set; }


        public virtual ICollection<UserDevice> UserDevices { get; set; }
        public virtual ICollection<UserLoginActivity> UserLoginActivities { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } 
    }
}
