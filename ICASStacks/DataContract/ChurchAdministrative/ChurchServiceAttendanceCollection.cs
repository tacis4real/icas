using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;

namespace ICASStacks.DataContract.ChurchAdministrative
{

    [Table("ICASDB.ChurchServiceAttendanceCollection")]
    public class ChurchServiceAttendanceCollection
    {

        //[ScaffoldColumn(false)]
        //public long ChurchServiceAttendanceCollectionId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Church Service Attendance Information is required")]
        public long ChurchServiceAttendanceId { get; set; }

        [CheckNumber(0, ErrorMessage = "Collection Type Information is required")]
        public int CollectionTypeId { get; set; }

        [CheckAmount(0, ErrorMessage = "Collection Type Amount is required")]
        public double Amount { get; set; }


        public virtual ChurchServiceAttendance ChurchServiceAttendance { get; set; }
        public virtual CollectionType CollectionType { get; set; }

    }
}
