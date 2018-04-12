using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;
using ICASStacks.DataContract.ChurchAdministrative;

namespace ICASStacks.DataContract
{

    [Table("ICASDB.Church")]
    public class Church
    {

        public Church()
        {
            Clients = new HashSet<Client>();
            ChurchStructureHqtrs = new HashSet<ChurchStructureHqtr>();
            ChurchStructures = new HashSet<ChurchStructure>();
            StructureChurches = new HashSet<StructureChurch>();
            ChurchStructureParishHeadQuarters = new HashSet<ChurchStructureParishHeadQuarter>();
            ChurchCollectionTypes = new HashSet<ChurchCollectionType>();

            ChurchServices = new HashSet<ChurchService>();

            //Regions = new Collection<Region>();
            //Provinces = new Collection<Province>();
            //Zones = new HashSet<Zone>();
            //Areas = new Collection<Area>();
            //Dioceses = new Collection<Diocese>();
            //Districts = new HashSet<District>();
            //Groups = new Collection<Group>();
            //States = new Collection<State>();
            //Parishes = new Collection<Parish>();
        }
        
        [ScaffoldColumn(false)]
        public long ChurchId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Church Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Short Name")]
        public string ShortName { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Founder")]
        public string Founder { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "* Required")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Church Location Address is required")]
        [DisplayName("Church Address")]
        public string Address { get; set; }

        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Registered is required")]
        public string TimeStampRegistered { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int RegisteredByUserId { get; set; }

        

        #region Navigation Properties
        //ChurchStructureHqtr
        //public virtual ChurchStructureHierachy ChurchStructureHierachy { get; set; }
        public virtual ChurchThemeSetting ChurchThemeSetting { get; set; }
        public virtual  ICollection<ChurchCollectionType> ChurchCollectionTypes { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<ChurchStructure> ChurchStructures { get; set; }
        public virtual ICollection<StructureChurch> StructureChurches { get; set; }
        public virtual ICollection<ChurchStructureHqtr> ChurchStructureHqtrs { get; set; }
        public virtual ICollection<ChurchStructureParishHeadQuarter> ChurchStructureParishHeadQuarters { get; set; }
        public virtual ICollection<ChurchService> ChurchServices { get; set; }



        //public virtual ICollection<Region> Regions { get; set; }
        //public virtual ICollection<Province> Provinces { get; set; }
        //public virtual ICollection<Zone> Zones { get; set; }
        //public virtual ICollection<Area> Areas { get; set; }
        //public virtual ICollection<Diocese> Dioceses { get; set; }
        //public virtual ICollection<District> Districts { get; set; }
        //public virtual ICollection<Group> Groups { get; set; }
        //public virtual ICollection<State> States { get; set; }
        //public virtual ICollection<Parish> Parishes { get; set; }

        #endregion


    }
    
}
