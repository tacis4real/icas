using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;

namespace ICASStacks.DataContract
{
    [Table("ICASDB.Area")]
    public class Area
    {

        [ScaffoldColumn(false)]
        public long AreaId { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent Church Information is required")]
        public long ChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Area Name")]
        public string Name { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Registered is required")]
        public string TimeStampRegistered { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int RegisteredByUserId { get; set; }

        #region Old Properties
        //[CheckNumber(0, ErrorMessage = "Church/Client Information is required")]
        //public long ClientId { get; set; }

        //[StringLength(15)]
        //[Required(ErrorMessage = "* Required")]
        //[DisplayName("Phone Number")]
        //public string PhoneNumber { get; set; }

        //[StringLength(100)]
        //[DisplayName("Email")]
        //public string Email { get; set; }

        //[Column(TypeName = "varchar")]
        //[StringLength(200)]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Church Location Address is required")]
        //[DisplayName("Address")]
        //public string Address { get; set; }
        #endregion


        #region Navigation Properties
        public virtual Church Church { get; set; }
        public virtual StateOfLocation StateOfLocation { get; set; }
        #endregion
    }
}
