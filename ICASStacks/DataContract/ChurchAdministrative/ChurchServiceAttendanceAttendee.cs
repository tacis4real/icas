using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;

namespace ICASStacks.DataContract.ChurchAdministrative
{

    [Table("ICASDB.ChurchServiceAttendanceAttendee")]
    public class ChurchServiceAttendanceAttendee
    {
        //[ScaffoldColumn(false)]
        //public long ChurchServiceAttendanceAttendeeId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Church Service Attendance Information is required")]
        public long ChurchServiceAttendanceId { get; set; }
        public int NumberOfMen { get; set; }
        public int NumberOfWomen { get; set; }
        public int NumberOfChildren { get; set; }
        public int NewConvert { get; set; }
        public int FirstTimer { get; set; }
        public long TotalAttendee { get; set; }



        public virtual ChurchServiceAttendance ChurchServiceAttendance { get; set; }
    }
}
