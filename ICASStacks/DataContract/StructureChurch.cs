using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ICASStacks.Common;

namespace ICASStacks.DataContract
{
    [Table("ICASDB.StructureChurch")]
    public class StructureChurch
    {

        //public StructureChurch()
        //{
        //    ClientChurchStructureDetails = new HashSet<ClientChurchStructureDetail>();
        //}

        [ScaffoldColumn(false)]
        public long StructureChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent church information is required")]
        public long ChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church structure type information is required")]
        [DisplayName("Structure Type")]
        public int ChurchStructureTypeId { get; set; }

        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Structure Church Name")]
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
        public virtual ChurchStructureType ChurchStructureType { get; set; }
        public virtual ChurchStructureHqtr ChurchStructureHqtr { get; set; }
        public virtual ClientStructureChurchDetail ClientStructureChurchDetail { get; set; }
        

        //public virtual ICollection<ClientChurchStructureDetail> ClientChurchStructureDetails { get; set; }
        

        #endregion
    }
}
