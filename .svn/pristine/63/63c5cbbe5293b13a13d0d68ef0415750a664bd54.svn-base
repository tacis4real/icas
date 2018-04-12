using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;

namespace ICASStacks.DataContract.ChurchAdministrative
{

    [Table("ICASDB.ClientRoleInChurch")]
    public class ClientRoleInChurch
    {
        public ClientRoleInChurch()
        {
            ChurchMembers = new HashSet<ChurchMember>();
        }

        [ScaffoldColumn(false)]
        public int ClientRoleInChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public long ClientChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church role information is required")]
        public int ChurchRoleTypeId { get; set; }

        //[Column(TypeName = "varchar")]
        //[Required]
        //[StringLength(250)]
        //public string Name { get; set; }

        //[Column(TypeName = "varchar")]
        //[StringLength(250)]
        //public string Description { get; set; }

        //[Range(1, 2)]
        //public ChurchSettingSource SourceId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date and Time Added is required")]
        public string TimeStampAdded { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int AddedByUserId { get; set; }


        public virtual ChurchRoleType ChurchRoleType { get; set; }
        public virtual ICollection<ChurchMember> ChurchMembers { get; set; }
    }
}
