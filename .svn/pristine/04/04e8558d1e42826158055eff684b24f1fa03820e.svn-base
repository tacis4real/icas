using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.APIObjs;
using ICASStacks.Common;
using ICASStacks.DataContract.Enum;
using Newtonsoft.Json;

namespace ICASStacks.DataContract
{


    [Table("ICASDB.ChurchStructure")]
    public class ChurchStructure
    {
        
        [ScaffoldColumn(false)]
        public long ChurchStructureId { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent church information is required")]
        public long ChurchId { get; set; }
        internal string _ChurchStructureTypeDetail { get; set; }

        [NotMapped]
        public List<ChurchStructureTypeDetailObj> ChurchStructureTypeDetail
        {
            get { return _ChurchStructureTypeDetail == null ? null : JsonConvert.DeserializeObject<List<ChurchStructureTypeDetailObj>>(_ChurchStructureTypeDetail); }
            set { _ChurchStructureTypeDetail = JsonConvert.SerializeObject(value); }
        }



        //[CheckNumber(0, ErrorMessage = "Church structure type information is required")]
        //[DisplayName("Structure Type")]
        //public int ChurchStructureTypeId { get; set; }

        ////[CheckNumber(0, ErrorMessage = "Hierachy level is required")]
        //public int HierachyLevel { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        public string LastModificationTimeStamp { get; set; }
        public int LastModificationByUserId { get; set; }
        public ChurchStructureStatus Status { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int RegisteredByUserId { get; set; }





        #region Navigation Properties
        public virtual Church Church { get; set; }
        public virtual ChurchStructureType ChurchStructureType { get; set; }
        //public virtual ChurchStructureHierachy ChurchStructureHierachy { get; set; }
        #endregion
    }
}
