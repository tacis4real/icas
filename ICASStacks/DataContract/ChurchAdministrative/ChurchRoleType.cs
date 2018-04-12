using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.DataContract.Enum;

namespace ICASStacks.DataContract.ChurchAdministrative
{

    [Table("ICASDB.ChurchRoleType")]
    public class ChurchRoleType
    {

        public ChurchRoleType()
        {
            RoleInChurches = new HashSet<RoleInChurch>();
            ClientRoleInChurches = new HashSet<ClientRoleInChurch>();
        }
        
        
        [ScaffoldColumn(false)]
        public int ChurchRoleTypeId { get; set; }

        [Column(TypeName = "varchar")]
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(250)]
        public string Description { get; set; }

        [Range(1, 2)]
        public ChurchSettingSource SourceId { get; set; }


        public virtual ICollection<RoleInChurch> RoleInChurches { get; set; }
        public virtual ICollection<ClientRoleInChurch> ClientRoleInChurches { get; set; }

    }
}
