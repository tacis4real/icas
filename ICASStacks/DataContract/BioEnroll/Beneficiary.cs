using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;
using ICASStacks.DataContract.Enum;

namespace ICASStacks.DataContract.BioEnroll
{
    public class Beneficiary
    {

        public long BeneficiaryId { get; set; }
        public int RecordId { get; set; }

        public int ClientStationId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Surname is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Surname is too short or too long")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Firstname is required", AllowEmptyStrings = false)]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "First name is too short or too long")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Othername is required", AllowEmptyStrings = false)]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Other name is too short or too long")]
        public string Othernames { get; set; }

        [Required(ErrorMessage = "Date of Birth is required", AllowEmptyStrings = false)]
        public DateTime DateOfBirth { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mobile Number is required")]
        [StringLength(15, MinimumLength = 7, ErrorMessage  = "Mobile Number must be between 7 and 15 characters")]
        [CheckMobileNumber(ErrorMessage = "Invalid Mobile Number")]
        public string MobileNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage  = "Residential Address is required")]
        [StringLength(200)]
        public string ResidentialAddress { get; set; }

        [StringLength(200)]
        public string OfficeAddress { get; set; }

        [Required(ErrorMessage = "State_is_required")]
        [CheckNumber(0, ErrorMessage = "Invalid State Information")]
        public int StateId { get; set; }

        //[Required(ErrorMessage = "Local Government Area is required")]
        //[CheckNumber(0,  ErrorMessage  = "Invalid Local Government Area Information")]
        //public int LocalAreaId { get; set; }

        [Required(ErrorMessage = "Sex Info is required", AllowEmptyStrings = false)]
        [CheckNumber(0, ErrorMessage = "Invalid Sex")]
        public int Sex { get; set; }

        [Required(ErrorMessage = "Marital Status is required")]
        [CheckNumber(0, ErrorMessage = "Invalid Marital Status Information")]
        public MaritalStatus MaritalStatus { get; set; }

        [Required(ErrorMessage = "Occupation is required")]
        [CheckNumber(0, ErrorMessage = "Invalid Occupation Information")]
        public int OccupationId { get; set; }
        public DateTime TimeStampRegistered { get; set; }
        public RegStatus Status { get; set; }
        public DateTime TimeStampUploaded { get; set; }
        public UploadStatus UploadStatus { get; set; }



        #region Navigation Properties
        public virtual ClientStation ClientStation { get; set; }
        //public virtual BeneficiaryBiometric BeneficiaryBiometric { get; set; }
        #endregion

    }
}
