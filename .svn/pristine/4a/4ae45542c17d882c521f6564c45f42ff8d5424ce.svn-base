using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;

namespace ICASStacks.DataContract.ChurchAdministrative
{

    [Table("ICASDB.ClientChurchCollection")]
    public class ClientChurchCollection
    {
        //[ScaffoldColumn(false)]
        //public int ClientChurchCollectionId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Church Service Attendance Information is required")]
        public long ChurchServiceAttendanceId { get; set; }
        public double Offerring { get; set; }
        public double Tithe { get; set; }
        public double ThanksGiving { get; set; }
        public double BuildingProjectFund { get; set; }
        public double SpecialThanksGiving { get; set; }
        public double Donation { get; set; }
        public double FirstFruit { get; set; }
        public double WelfareCharity { get; set; }
        public double Others { get; set; }
        public double TotalCollection { get; set; }



        public virtual ChurchServiceAttendance ChurchServiceAttendance { get; set; }

    }
}
