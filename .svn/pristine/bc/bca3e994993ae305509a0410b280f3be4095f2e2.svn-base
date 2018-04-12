using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;

namespace ICASStacks.DataContract
{
    [Table("ICASDB.ChurchStructureHqtr")]
    public class ChurchStructureHqtr
    {
        [ScaffoldColumn(false)]
        public long ChurchStructureHqtrId { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent church information is required")]
        public long ChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church/Client Information is required")]
        public long ClientId { get; set; }
        
        [CheckNumber(0, ErrorMessage = "Church structure type information is required")]
        [DisplayName("Structure Type")]
        public int ChurchStructureTypeId { get; set; }

        //[CheckNumber(0, ErrorMessage = "Structure Headquarter Information is required")]
        //public long StructureHqtrId { get; set; }

        //[CheckNumber(0, ErrorMessage = "Structure church detail information is required")]
        //[DisplayName("Structure Church")]
        //public long StructureChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "Structure Detail(i.e. Region, Zone etc Information is required")]
        public long StructureDetailId { get; set; }

        //[Column(TypeName = "varchar")]
        //[StringLength(35)]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Date Registered is required")]
        //public string TimeStampRegistered { get; set; }

        //[CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        //public int RegisteredByUserId { get; set; }


        #region Navigation Properties
        //public virtual Client Client { get; set; }
        //public virtual StructureChurch StructureChurch { get; set; }
        public virtual ChurchStructureType ChurchStructureType { get; set; }
        #endregion
    }
}
