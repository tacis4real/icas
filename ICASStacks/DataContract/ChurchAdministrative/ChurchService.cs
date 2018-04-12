using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.APIObjs;
using ICASStacks.Common;
using Newtonsoft.Json;

namespace ICASStacks.DataContract.ChurchAdministrative
{

    [Table("ICASDB.ChurchService")]
    public class ChurchService
    {

        //public ChurchService()
        //{
        //    ChurchServiceAttendances = new HashSet<ChurchServiceAttendance>();
        //}

        [ScaffoldColumn(false)]
        public long ChurchServiceId { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent Church Information is required")]
        public long ChurchId { get; set; }

        //[CheckNumber(0, ErrorMessage = "Church service type information is required")]
        //public int ChurchServiceTypeId { get; set; }

        internal string _ServiceTypeDetail { get; set; }

        [NotMapped]
        public List<ChurchServiceDetailObj> ServiceTypeDetail
        {
            get { return _ServiceTypeDetail == null ? null : JsonConvert.DeserializeObject<List<ChurchServiceDetailObj>>(_ServiceTypeDetail); }
            set { _ServiceTypeDetail = JsonConvert.SerializeObject(value); }
        }

        //public int DayOfWeekId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Added is required")]
        public string TimeStampAdded { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int AddedByUserId { get; set; }
        public int Status { get; set; }


        public virtual ChurchServiceType ChurchServiceType { get; set; }
        //public virtual Church Church { get; set; }
        //public virtual Client Client { get; set; }
        //public virtual ICollection<Client> Clients { get; set; }
        //public virtual ICollection<ChurchServiceAttendance> ChurchServiceAttendances { get; set; }
    }
}
