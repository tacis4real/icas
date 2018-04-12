using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAdminStacks.DataContract
{

    [Table("ICASWebAdmin.ClientRole")]
    public class ClientRole
    {
        public long ClientRoleId { get; set; }

        public long ClientProfileId { get; set; }

        public int RoleClientId { get; set; }


        public virtual ClientProfile ClientProfile { get; set; }
        public virtual RoleClient RoleClient { get; set; }

    }
}
