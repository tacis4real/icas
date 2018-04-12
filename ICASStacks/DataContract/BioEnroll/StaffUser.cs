using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ICASStacks.DataContract.BioEnroll
{
    public class StaffUser
    {
        public int StaffUserId { get; set; }

        [Required(ErrorMessage = @"* Required")]
        public int UserProfileId { get; set; }

        [StringLength(20, ErrorMessage = @"Username is too short or too long", MinimumLength = 8)]
        [Required(AllowEmptyStrings = false, ErrorMessage = @"Username is required")]
        public string UserName { get; set; }

        [StringLength(50)]
        [RegularExpression("[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,4}", ErrorMessage = @"This email address is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public int FailedPasswordAttemptCount { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }

        [StringLength(35)]
        public string LastLockedOutTimeStamp { get; set; }

        [StringLength(35)]
        public string LastLoginTimeStamp { get; set; }

        [StringLength(35)]
        public string LastPasswordChangedTimeStamp { get; set; }

        [DisplayName(@"Password")]
        [StringLength(50)]
        [Required(ErrorMessage = @"* Required")]
        public string Password { get; set; }

        [StringLength(35)]
        [Required(ErrorMessage = @"* Required")]
        public string RegisteredDateTimeStamp { get; set; }
        public int RoleId { get; set; }
        public string Salt { get; set; }

        [ScaffoldColumn(false)]
        public string UserCode { get; set; }

        
        public virtual UserProfile UserProfile { get; set; }
        public virtual Role Role { get; set; }
        public ICollection<UserLoginTrail> UserLoginTrails { get; set; }

    }
}
