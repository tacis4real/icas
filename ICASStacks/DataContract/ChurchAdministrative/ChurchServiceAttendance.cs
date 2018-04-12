using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.APIObjs;
using ICASStacks.Common;
using ICASStacks.DataContract.JSONContract;
using Newtonsoft.Json;

namespace ICASStacks.DataContract.ChurchAdministrative
{

    [Table("ICASDB.ChurchServiceAttendance")]
    public class ChurchServiceAttendance
    {
        
        [ScaffoldColumn(false)]
        public long ChurchServiceAttendanceId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public long ClientChurchId { get; set; }

        //[CheckNumber(0, ErrorMessage = "Church Service Information is required")]
        //public int ChurchServiceTypeId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Church Service Information is required")]
        public string ChurchServiceTypeRefId { get; set; }

        [Column(TypeName = "varchar")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Service Theme is required")]
        public string ServiceTheme { get; set; }

        [Column(TypeName = "varchar")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Service Bible reading text is required")]
        public string BibleReadingText { get; set; }

        [Column(TypeName = "varchar")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Preacher is required")]
        public string Preacher { get; set; }
        public long TotalAttendee { get; set; }
        public double TotalCollection { get; set; }

        internal string _ServiceAttendanceDetail { get; set; }

        //[NotMapped]
        //public List<ChurchServiceAttendanceDetailObj> ServiceAttendanceDetail
        //{
        //    get { return _ServiceAttendanceDetail == null ? null : JsonConvert.DeserializeObject<List<ChurchServiceAttendanceDetailObj>>(_ServiceAttendanceDetail); }
        //    set { _ServiceAttendanceDetail = JsonConvert.SerializeObject(value); }
        //}


        [NotMapped]
        public ClientChurchServiceAttendanceDetailObj ServiceAttendanceDetail
        {
            get { return _ServiceAttendanceDetail == null ? null : JsonConvert.DeserializeObject<ClientChurchServiceAttendanceDetailObj>(_ServiceAttendanceDetail); }
            set { _ServiceAttendanceDetail = JsonConvert.SerializeObject(value); }
        }


        //public int NumberOfMen { get; set; }
        //public int NumberOfWomen { get; set; }
        //public int NumberOfChildren { get; set; }
        //public int NewConvert { get; set; }
        //public int FirstTimer { get; set; }
        //public long TotalAttendee { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(10)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date service hold is required")]
        public string DateServiceHeld { get; set; }
        //public int Year { get; set; }
        //public int Month { get; set; }


        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Added is required")]
        public string TimeStampTaken { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int TakenByUserId { get; set; }



        //public virtual ChurchServiceAttendanceRemittance ChurchServiceAttendanceRemittance { get; set; }
        public virtual ChurchServiceType ChurchServiceType { get; set; }
        public virtual ClientChurch ClientChurch { get; set; }


        //public virtual ChurchServiceAttendanceAttendee ChurchServiceAttendanceAttendee { get; set; }
        //public virtual ClientChurchCollection ClientChurchCollection { get; set; }

        //public virtual ICollection<ChurchServiceAttendanceCollection> ChurchServiceAttendanceCollections { get; set; }
        





    }
}
