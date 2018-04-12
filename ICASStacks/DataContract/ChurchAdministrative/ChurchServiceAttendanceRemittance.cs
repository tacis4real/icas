using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICASStacks.APIObjs;
using ICASStacks.Common;
using Newtonsoft.Json;

namespace ICASStacks.DataContract.ChurchAdministrative
{

    //[Table("ICASDB.ChurchServiceAttendanceRemittance")]
    public class ChurchServiceAttendanceRemittance
    {
        //[ScaffoldColumn(false)]
        public long ChurchServiceAttendanceRemittanceId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public long ClientId { get; set; }
        //public int Year { get; set; }
        //public int Month { get; set; }

        public string From { get; set; }
        public string To { get; set; }

        internal string _RemittanceChurchServiceDetail { get; set; }

        [NotMapped]
        public List<RemittanceChurchServiceDetailObj> RemittanceChurchServiceDetail
        {
            get { return _RemittanceChurchServiceDetail == null ? null : JsonConvert.DeserializeObject<List<RemittanceChurchServiceDetailObj>>(_RemittanceChurchServiceDetail); }
            set { _RemittanceChurchServiceDetail = JsonConvert.SerializeObject(value); }
        }
        
        internal string _RemittanceChurchServiceAverageAttendanceDetail { get; set; }

        [NotMapped]
        public List<ChurchServiceAverageAttendanceObj> RemittanceChurchServiceAverageAttendanceDetail
        {
            get { return _RemittanceChurchServiceAverageAttendanceDetail == null ? null : JsonConvert.DeserializeObject<List<ChurchServiceAverageAttendanceObj>>(_RemittanceChurchServiceAverageAttendanceDetail); }
            set { _RemittanceChurchServiceAverageAttendanceDetail = JsonConvert.SerializeObject(value); }
        }
        
        internal string _ServiceAttendanceRemittanceCollection { get; set; }

        [NotMapped]
        public List<ChurchServiceAttendanceRemittanceCollectionObj> ChurchServiceAttendanceRemittanceCollection
        {
            get { return _ServiceAttendanceRemittanceCollection == null ? null : JsonConvert.DeserializeObject<List<ChurchServiceAttendanceRemittanceCollectionObj>>(_ServiceAttendanceRemittanceCollection); }
            set { _ServiceAttendanceRemittanceCollection = JsonConvert.SerializeObject(value); }
        }

        internal string _RemittanceCollection { get; set; }

        [NotMapped]
        public List<ClientCollectionRemittanceDetailObj> CollectionRemittanceDetail
        {
            get { return _RemittanceCollection == null ? null : JsonConvert.DeserializeObject<List<ClientCollectionRemittanceDetailObj>>(_RemittanceCollection); }
            set { _RemittanceCollection = JsonConvert.SerializeObject(value); }
        }

        //public List<CollectionRemittanceChurchStructureTypeTotalObj> CollectionRemittanceChurchStructureTypeTotal { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Remitted is required")]
        public string TimeStampRemitted { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int RemittedByUserId { get; set; }

        //public virtual Client Client { get; set; }
    }
}
