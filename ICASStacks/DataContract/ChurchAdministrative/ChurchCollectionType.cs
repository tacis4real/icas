using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.APIObjs;
using ICASStacks.Common;
using ICASStacks.DataContract.Enum;
using ICASStacks.DataContract.JSONContract;
using Newtonsoft.Json;

namespace ICASStacks.DataContract.ChurchAdministrative
{

    [Table("ICASDB.ChurchCollectionType")]
    public class ChurchCollectionType
    {
        [ScaffoldColumn(false)]
        public int ChurchCollectionTypeId { get; set; }

        [CheckNumber(0, ErrorMessage = "Parent Church Information is required")]
        public long ChurchId { get; set; }

        //[CheckNumber(0, ErrorMessage = "Collection type information is required")]
        //public int CollectionTypeId { get; set; }
        //public float NationalPercent { get; set; }
        //public float OtherPercent { get; set; }

        internal string _Collection { get; set; }

        [NotMapped]
        public List<CollectionTypeObj> CollectionTypes
        {
            get { return _Collection == null ? null : JsonConvert.DeserializeObject<List<CollectionTypeObj>>(_Collection); }
            set { _Collection = JsonConvert.SerializeObject(value); }
        }

        public PreferenceTypeStatus Status { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date and Time Added is required")]
        public string TimeStampAdded { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int AddedByUserId { get; set; }


        public virtual Church Church { get; set; }
        //public virtual CollectionType CollectionType { get; set; }

    }
}

