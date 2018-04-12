using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAdminStacks.DataContract
{
    [Table("ICASWebAdmin.Role")]
    public partial class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int RoleId { get; set; }

        [Column(TypeName = "varchar")]
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200)]
        public string Description { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
