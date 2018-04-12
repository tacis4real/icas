using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICASStacks.Common;
using Newtonsoft.Json;

namespace ICASStacks.DataContract
{

    [Table("ICASDB.ChurchStructureParishHeadQuarter")]
    public class ChurchStructureParishHeadQuarter
    {
        [ScaffoldColumn(false)]
        public long ChurchStructureParishHeadQuarterId { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent church information is required")]
        public long ChurchId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church structure type information is required")]
        [DisplayName("Structure Type")]
        public int ChurchStructureTypeId { get; set; }

        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }
        internal string _Parish { get; set; }

        [NotMapped]
        public List<StructureChurchHeadQuarterParish> Parish
        {
            get { return _Parish == null ? null : JsonConvert.DeserializeObject<List<StructureChurchHeadQuarterParish>>(_Parish); }
            set { _Parish = JsonConvert.SerializeObject(value); }
        }


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

        //public virtual Client Client { get; set; }

        #endregion
    }
}
