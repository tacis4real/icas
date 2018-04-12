using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAdminStacks.Common;

namespace WebAdminStacks.DataContract
{

    [Table("ICASWebAdmin.ClientLoginActivity")]
    public class ClientLoginActivity
    {
        public long ClientLoginActivityId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public long ClientProfileId { get; set; }

        public bool IsLoggedIn { get; set; }

        public UserLoginSource LoginSource { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string LoginAddress { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string LoginToken { get; set; }

        [Column(TypeName = "varchar")]
        [Required]
        [StringLength(35)]
        public string LoginTimeStamp { get; set; }


        public virtual ClientProfile ClientProfile { get; set; }
    }
}
