using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.DataContract.Enum;

namespace ICASStacks.DataContract.ChurchAdministrative
{

    [Table("ICASDB.ChurchServiceType")]
    public class ChurchServiceType
    {

        public ChurchServiceType()
        {
            ChurchServices = new HashSet<ChurchService>();
            ClientChurchServices = new HashSet<ClientChurchService>();
            ChurchServiceAttendances = new HashSet<ChurchServiceAttendance>();
        }


        [ScaffoldColumn(false)]
        public int ChurchServiceTypeId { get; set; }

        //[Key]
        //public string ChurchServiceTypeRefId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Service Name")]
        public string Name { get; set; }

        [Range(1, 2)]
        public ChurchSettingSource SourceId { get; set; }



        public virtual ICollection<ChurchService> ChurchServices { get; set; }
        public virtual ICollection<ClientChurchService> ClientChurchServices { get; set; }
        public virtual ICollection<ChurchServiceAttendance> ChurchServiceAttendances { get; set; }
    }
}
