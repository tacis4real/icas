using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;

namespace ICASStacks.DataContract
{

    [Table("ICASDB.ChurchStructureType")]
    public class ChurchStructureType
    {

        public ChurchStructureType()
        {
            ChurchStructures = new HashSet<ChurchStructure>();
            StructureChurches = new HashSet<StructureChurch>();
            ClientStructureChurchDetails = new HashSet<ClientStructureChurchDetail>();
            ChurchStructureHqtrs = new HashSet<ChurchStructureHqtr>();
            ChurchStructureParishHeadQuarters = new HashSet<ChurchStructureParishHeadQuarter>();
        }
        
        [ScaffoldColumn(false)]
        public int ChurchStructureTypeId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Structure Name")]
        public string Name { get; set; }
        
        public int Status { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Registered is required")]
        public string TimeStampRegistered { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int RegisteredByUserId { get; set; }


        #region Navigation Properties
        public virtual ICollection<ChurchStructure> ChurchStructures { get; set; }
        public virtual ICollection<StructureChurch> StructureChurches { get; set; }
        public virtual ICollection<ClientStructureChurchDetail> ClientStructureChurchDetails { get; set; }
        public virtual ICollection<ChurchStructureHqtr> ChurchStructureHqtrs { get; set; }
        public virtual ICollection<ChurchStructureParishHeadQuarter> ChurchStructureParishHeadQuarters { get; set; }
        #endregion
    }
}
