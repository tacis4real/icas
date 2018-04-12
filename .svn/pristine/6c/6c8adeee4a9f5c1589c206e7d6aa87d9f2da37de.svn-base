using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAdminStacks.DataContract
{

    [Table("ICASWebAdmin.ClientChurchRole")]
    public class ClientChurchRole
    {
        public long ClientChurchRoleId { get; set; }
        public long ClientChurchProfileId { get; set; }
        public int RoleClientId { get; set; }


        public virtual ClientChurchProfile ClientChurchProfile { get; set; }
        public virtual RoleClient RoleClient { get; set; }
    }
}
