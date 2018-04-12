using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;

namespace ICASStacks.DataContract.ChurchAdministrative
{

    [Table("ICASDB.RoleInChurch")]
    public class RoleInChurch
    {

        //public RoleInChurch()
        //{
        //    ChurchMembers = new HashSet<ChurchMember>();
        //}

        [ScaffoldColumn(false)]
        public int RoleInChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent Church Information is required")]
        public long ChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church role type information is required")]
        public int ChurchRoleTypeId { get; set; }

        //[Column(TypeName = "varchar")]
        //[Required]
        //[StringLength(250)]
        //public string Name { get; set; }

        //[Column(TypeName = "varchar")]
        //[StringLength(250)]
        //public string Description { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date and Time Added is required")]
        public string TimeStampAdded { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int AddedByUserId { get; set; }



        public virtual ChurchRoleType ChurchRoleType { get; set; }
        //public virtual ICollection<ChurchMember> ChurchMembers { get; set; }

    }
}
