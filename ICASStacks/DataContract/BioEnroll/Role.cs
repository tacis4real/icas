using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICASStacks.DataContract.BioEnroll
{
    public class Role
    {
        public int RoleId { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = @"* Required")]
        public string Name { get; set; }
        public bool Status { get; set; }


        public ICollection<StaffUser> Users { get; set; }
    }
}
