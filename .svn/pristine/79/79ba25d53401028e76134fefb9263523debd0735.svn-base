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

namespace ICASStacks.DataContract.ChurchAdministrative.ReflectionObjs
{
    public class ChurchServiceAttendanceR {

        public long ChurchServiceAttendanceId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public long ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church Service Information is required")]
        public int ChurchServiceTypeId { get; set; }

        [Column(TypeName = "varchar")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Service Theme is required")]
        public string ServiceTheme { get; set; }

        [Column(TypeName = "varchar")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Service Theme is required")]
        public string BibleReadingText { get; set; }
        public string Preacher { get; set; }
        public long TotalAttendee { get; set; }
        public double TotalCollection { get; set; }
        
        [NotMapped]
        //public ChurchServiceAttendanceDetailObj ServiceAttendanceDetail
        //{
        //    get { return _ServiceAttendanceDetail == null ? null : JsonConvert.DeserializeObject<ChurchServiceAttendanceDetailObj>(_ServiceAttendanceDetail); }
        //    set { _ServiceAttendanceDetail = JsonConvert.SerializeObject(value); }
        //}
        
        public string DateServiceHeld { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string TimeStampTaken { get; set; }
        public int TakenByUserId { get; set; }
    }
}
