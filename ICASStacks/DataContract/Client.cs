using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;
using ICASStacks.DataContract.ChurchAdministrative;
using Newtonsoft.Json;

namespace ICASStacks.DataContract
{

    [Table("ICASDB.Client")]
    public class Client
    {

        public Client()
        {
            ClientStructureChurchDetails = new HashSet<ClientStructureChurchDetail>();
            //ChurchMembers = new HashSet<ChurchMember>();
            //ChurchServices = new HashSet<ChurchService>();
            //ChurchServiceAttendances = new HashSet<ChurchServiceAttendance>();
            ChurchServiceAttendanceRemittances = new HashSet<ChurchServiceAttendanceRemittance>();
        }
        
        [ScaffoldColumn(false)]
        public long ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church Information is required")]
        public long ChurchId { get; set; }

        // Before before...this Id stands for this Client own HeadQuarter Id after it has been added has Headquarter 
        // that other Client can add as their own Headquarter above them
        public long? StructureChurchHeadQuarterParishId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Church Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Pastor/Leader")]
        public string Pastor { get; set; }

        [Required(ErrorMessage = "Title is required", AllowEmptyStrings = false)]
        public int Title { get; set; }

        [Range(1, 2)]
        public int Sex { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "* Required")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        
        [CheckNumber(0, ErrorMessage = "State of Church Location is required")]
        [DisplayName("State of Location")]
        public int StateOfLocationId { get; set; }

        internal string _Parish { get; set; }

        [NotMapped]
        public List<StructureChurchHeadQuarterParish> ClientStructureChurchHeadQuarter
        {
            get { return _Parish == null ? null : JsonConvert.DeserializeObject<List<StructureChurchHeadQuarterParish>>(_Parish); }
            set { _Parish = JsonConvert.SerializeObject(value); }
        }
        
        [Column(TypeName = "varchar")]
        [StringLength(200)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Church Location Address is required")]
        [DisplayName("Church Address")]
        public string Address { get; set; }  
        

        [Column(TypeName = "varchar")]
        [StringLength(20)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Church Reference Number is required")]
        [Index("IX_Reg_ChurchRefNo", IsUnique = true)]
        public string ChurchReferenceNumber { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Registered is required")]
        public string TimeStampRegistered { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int RegisteredByUserId { get; set; }



        #region Navigation Properties
        public virtual ClientAccount Account { get; set; }
        public virtual Church Church { get; set; }
        //public virtual ChurchStructureParishHeadQuarter ChurchStructureParishHeadQuarter { get; set; }
        public virtual ICollection<ClientStructureChurchDetail> ClientStructureChurchDetails { get; set; }
        //public virtual ICollection<ChurchMember> ChurchMembers { get; set; }
        
        //public virtual ICollection<ChurchService> ChurchServices { get; set; }
        //public virtual ICollection<ClientChurchService> ClientChurchServices { get; set; }
        //public virtual ICollection<ChurchServiceAttendance> ChurchServiceAttendances { get; set; }

        public virtual ICollection<ChurchServiceAttendanceRemittance> ChurchServiceAttendanceRemittances { get; set; }
        
        #endregion
       
    }
}
