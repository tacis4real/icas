using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAdminStacks.Common;

namespace WebAdminStacks.DataContract
{
    [Table("ICASWebAdmin.ClientChurchProfile")]
    public class ClientChurchProfile
    {
        public ClientChurchProfile()
        {
            ClientChurchDevices = new HashSet<ClientChurchDevice>();
            ClientChurchLoginActivities = new HashSet<ClientChurchLoginActivity>();
            ClientChurchRoles = new HashSet<ClientChurchRole>();
        }

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

        public virtual ICollection<ClientChurchDevice> ClientChurchDevices { get; set; }
        public virtual ICollection<ClientChurchLoginActivity> ClientChurchLoginActivities { get; set; }
        public virtual ICollection<ClientChurchRole> ClientChurchRoles { get; set; }
    }
}
