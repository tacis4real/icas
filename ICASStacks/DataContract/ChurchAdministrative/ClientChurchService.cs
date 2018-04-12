using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.APIObjs;
using ICASStacks.Common;
using Newtonsoft.Json;

namespace ICASStacks.DataContract.ChurchAdministrative
{
    [Table("ICASDB.ClientChurchService")]
    public class ClientChurchService
    {
        //public ClientChurchService()
        //{
        //    ChurchServiceAttendances = new HashSet<ChurchServiceAttendance>();
        //}

        [ScaffoldColumn(false)]
        public long ClientChurchServiceId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public long ClientChurchId { get; set; }


        internal string _ServiceTypeDetail { get; set; }

        [NotMapped]
        public List<ChurchServiceDetailObj> ServiceTypeDetail
        {
            get { return _ServiceTypeDetail == null ? null : JsonConvert.DeserializeObject<List<ChurchServiceDetailObj>>(_ServiceTypeDetail); }
            set { _ServiceTypeDetail = JsonConvert.SerializeObject(value); }
        }

        //[CheckNumber(0, ErrorMessage = "Church service type information is required")]
        //public int ChurchServiceTypeId { get; set; }

        //[Required(ErrorMessage = "* Required")]
        //[DisplayName("Service Name")]
        //public string Name { get; set; }

        //[CheckNumber(0, ErrorMessage = "Day of Service is required")]
        //public int DayOfWeekId { get; set; }

        //[Range(1, 2)]
        //public ChurchSettingSource SourceId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Added is required")]
        public string TimeStampAdded { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int AddedByUserId { get; set; }
        public int Status { get; set; }


        public virtual ClientChurch ClientChurch { get; set; }
        public virtual ChurchServiceType ChurchServiceType { get; set; }
        //public virtual ICollection<ChurchServiceAttendance> ChurchServiceAttendances { get; set; }
    }
}
