using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICASStacks.DataContract
{
    [Table("ICASDB.Bank")]
    public class Bank
    {
        [ScaffoldColumn(false)]
        public int BankId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Bank Name is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Bank Name must be between 3 and 200 characters")]

        [DisplayName("Bank Name")]
        public string Name { get; set; }
        
        public bool Status { get; set; }

        public int RegisteredBy { get; set; }


        public virtual ICollection<ClientAccount> ClientAccounts { get; set; }

    }
}
