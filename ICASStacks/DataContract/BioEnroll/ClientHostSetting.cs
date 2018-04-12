using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;

namespace ICASStacks.DataContract.BioEnroll
{

    [Table("ClientStation")]
    public class ClientStation
    {


        public ClientStation()
        {
            Beneficiaries = new HashSet<Beneficiary>();
        }


        public int ClientStationId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Station name is required")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "Station name must be between 5 and 150 characters")]
        [Display(Name = "Station Name")]
        public string StationName { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Unable to generate Station Key", AllowEmptyStrings = false)]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "Invalid Station Key")]
        [Index("IX_Station_Key", IsUnique = true)]
        [Display(Name = "Station Key")]
        public string StationKey { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Unable to generate Access Key", AllowEmptyStrings = false)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Invalid Access Key")]
        [Index("IX_APIAccessKey_Key", IsUnique = true)]
        [Display(Name = "API Access Key")]
        public string APIAccessKey { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        [Display(Name = "Device Id")]
        public string DeviceId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Assignee's full name is required", AllowEmptyStrings = false)]
        [StringLength(150, MinimumLength = 8, ErrorMessage = "Assignee's full name must be between 8 and 150 characters")]
        [Display(Name = "Assignee  Name")]
        public string AssigneeFullName { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Assignee's mobile number is required", AllowEmptyStrings = false)]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Assignee's mobile must be between 7 and 15 digits")]
        [Display(Name = "Assignee Mobile No")]
        public string AssigneeMobileNumber { get; set; }

        [Required(ErrorMessage = "Local Government Area is required")]
        [CheckNumber(0, ErrorMessage = "Invalid Local Government Area Information")]
        public int LocalAreaId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(15)]
        [Display(Name = "Device IP")]
        public string DeviceIP { get; set; }
        public int Status { get; set; }



        public virtual LocalArea LocalArea { get; set; }
        public virtual ICollection<Beneficiary> Beneficiaries { get; set; }

    }
}