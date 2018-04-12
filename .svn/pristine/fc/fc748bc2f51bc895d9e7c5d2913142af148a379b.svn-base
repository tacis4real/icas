using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ICASStacks.Common;

namespace ICASStacks.DataContract.BioEnroll
{
    public class UserProfile
    {
        public int UserProfileId { get; set; }

        [Required(ErrorMessage = @"* Required")]
        public int ClientStationId { get; set; }

        [Required(ErrorMessage = @"* Required")]
        [DisplayName(@"Staff Id")]
        [StringLength(10)]
        public string ProfileNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = @"Surname is required")]
        [StringLength(50, ErrorMessage = @"Surname is too short or too long", MinimumLength = 3)]
        public string Surname { get; set; }

        [StringLength(50, ErrorMessage = @"First name is too short or too long", MinimumLength = 3)]
        [Required(AllowEmptyStrings = false, ErrorMessage = @"First name is required")]
        public string FirstName { get; set; }

        //[StringLength(50, ErrorMessage = @"Other names are too short or too long", MinimumLength = 3)]
        public string OtherNames { get; set; }

        [Required(ErrorMessage = @"Sex is required")]
        [CheckNumber(0, ErrorMessage = @"Invalid sex")]
        public int Sex { get; set; }

        [StringLength(100, ErrorMessage = @"Residential address is too short or too long", MinimumLength = 10)]
        public string ResidentialAddress { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression("[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,4}", ErrorMessage = @"This email address is not valid.")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = @"Mobile number is required")]
        //[CheckMobileNumber(ErrorMessage = @"Invalid Mobile Number")]
        [StringLength(15, ErrorMessage = @"Mobile number is too short or too long", MinimumLength = 7)]
        public string MobileNumber { get; set; }

        [StringLength(10)]
        [Required(ErrorMessage = @"* Required")]
        public string DateLastModified { get; set; }

        [Required(ErrorMessage = @"* Required")]
        [StringLength(15)]
        public string TimeLastModified { get; set; }

        [DisplayName(@"Modified By")]
        [Required(ErrorMessage = @"* Required")]
        public int ModifiedBy { get; set; }

        [ScaffoldColumn(false)]
        public int Status { get; set; }


        public virtual ClientStation ClientStation { get; set; }
        
    }

}
