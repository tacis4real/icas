using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;
using ICASStacks.DataContract.Enum;

namespace ICASStacks.DataContract.ChurchAdministrative
{

    [Table("ICASDB.ChurchMember")]
    public class ChurchMember
    {
        [ScaffoldColumn(false)]
        public long ChurchMemberId { get; set; }

        [CheckNumber(0, ErrorMessage = "This member church Information is required")]
        public long ClientChurchId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Fullname is required", AllowEmptyStrings = false)]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Fullname must be between 2 and 200 characters")]
        public string FullName { get; set; }

        [CheckNumber(0, ErrorMessage = "Profession is required")]
        public int ProfessionId { get; set; }

        [CheckNumber(0, ErrorMessage = "Member role in church is required")]
        public int ClientRoleInChurchId { get; set; }
        //public int RoleInChurchId { get; set; }

        [Range(1, 2)]
        public int Sex { get; set; }

        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Mobile Number must be between 7 and 15 digits")]
        public string MobileNumber { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Church Location Address is required")]
        [DisplayName("Member Address")]
        public string Address { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(10)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Joined is required")]
        public string DateJoined { get; set; }

        [StringLength(35)]
        public string TimeStampUploaded { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Registered is required")]
        public string TimeStampRegistered { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int RegisteredByUserId { get; set; }
        public UploadStatus UploadStatus { get; set; }




        #region Navigation Properties
        public virtual ClientChurch ClientChurch { get; set; }
        public virtual ClientRoleInChurch ClientRoleInChurch { get; set; }
        public virtual Profession Profession { get; set; }
        #endregion
        

    }
}
