using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;

namespace ICASStacks.DataContract
{

    [Table("ICASDB.ClientContact")]
    public class ClientContact
    {
        
        [ScaffoldColumn(false)]
        public long ClientContactId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church Information is required")]
        public long ClientId { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "* Required")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Church Location Address is required")]
        [DisplayName("Church Address")]
        public string Address { get; set; }

        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Registered is required")]
        public string TimeStampRegistered { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int RegisteredByUserId { get; set; }


        #region Navigation Properties
        public virtual StateOfLocation StateOfLocation { get; set; }
        #endregion

    }
}
