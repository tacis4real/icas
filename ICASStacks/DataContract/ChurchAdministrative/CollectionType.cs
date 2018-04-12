using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.DataContract.Enum;

namespace ICASStacks.DataContract.ChurchAdministrative
{

    [Table("ICASDB.CollectionType")]
    public class CollectionType
    {

        public CollectionType()
        {
            //ChurchCollectionTypes = new HashSet<ChurchCollectionType>();
            ClientChurchCollectionTypes = new HashSet<ClientChurchCollectionType>();
            //ChurchServiceAttendanceCollections = new HashSet<ChurchServiceAttendanceCollection>();
        }

        [ScaffoldColumn(false)]
        public int CollectionTypeId { get; set; }

        //[Key]
        //public string CollectionRefId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Service Name")]
        public string Name { get; set; }

        [Range(1, 2)]
        public ChurchSettingSource SourceId { get; set; }



        //public virtual ICollection<ChurchCollectionType> ChurchCollectionTypes { get; set; }
        public virtual ICollection<ClientChurchCollectionType> ClientChurchCollectionTypes { get; set; }
        //public virtual ICollection<ChurchServiceAttendanceCollection> ChurchServiceAttendanceCollections { get; set; }
    }
}
