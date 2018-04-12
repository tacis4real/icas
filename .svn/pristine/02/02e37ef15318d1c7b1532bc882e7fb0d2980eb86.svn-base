using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICASStacks.Common;
using ICASStacks.DataContract.Enum;

namespace ICASStacks.DataContract
{
    public class ClientChurchAccount
    {

        //public long ClientAccountId { get; set; }

        //[CheckNumber(0, ErrorMessage = "Client Information is required")]
        //public long ClientId { get; set; }

        //[DisplayName("Bank Name")]
        //[CheckNumber(0, ErrorMessage = "Bank Information is required")]
        public int BankId { get; set; }
        public string BankName { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Account Name")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Account Type")]
        public int AccountTypeId { get; set; }

        //public ClientAccountStatus Status { get; set; }

        //[Column(TypeName = "varchar")]
        //[StringLength(35)]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Date Registered is required")]
        //public string TimeStampRegistered { get; set; }

        //public int RegisteredByUserId { get; set; }


        #region Navigation Properties
        //public virtual Client Client { get; set; }
        //public virtual Bank Bank { get; set; }
        #endregion
    }
}
