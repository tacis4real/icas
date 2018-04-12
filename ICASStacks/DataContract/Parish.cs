using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;

namespace ICASStacks.DataContract
{
    [Table("ICASDB.Parish")]
    public class Parish
    {

        [ScaffoldColumn(false)]
        public long ParishId { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent Church Information is required")]
        public long ChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Parish Name")]
        public string Name { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Registered is required")]
        public string TimeStampRegistered { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int RegisteredByUserId { get; set; }


        #region Navigation Properties
        public virtual Church Church { get; set; }
        public virtual StateOfLocation StateOfLocation { get; set; }

        #endregion
    }
}
