using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAdminStacks.DataContract
{

    [Table("ICASWebAdmin.RoleClient")]
    public class RoleClient
    {

        public RoleClient()
        {
            ClientRoles = new HashSet<ClientRole>();
            ClientChurchRoles = new Collection<ClientChurchRole>();
        }

        public int RoleClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ClientRole> ClientRoles { get; set; }
        public virtual ICollection<ClientChurchRole> ClientChurchRoles { get; set; } 
    }
}
