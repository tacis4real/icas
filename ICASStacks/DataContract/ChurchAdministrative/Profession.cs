using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;
using ICASStacks.DataContract.Enum;

namespace ICASStacks.DataContract.ChurchAdministrative
{

    [Table("ICASDB.Profession")]
    public class Profession
    {
        [ScaffoldColumn(false)]
        public int ProfessionId { get; set; }

        [Column(TypeName = "varchar")]
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date and Time Added is required")]
        public string TimeStampAdded { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int AddedByUserId { get; set; }

        [Range(1, 2)]
        public ChurchSettingSource SourceId { get; set; }


        public virtual ICollection<ChurchMember> ChurchMembers { get; set; }

    }
}
