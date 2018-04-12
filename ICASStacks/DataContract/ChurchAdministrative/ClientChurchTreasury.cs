using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;

namespace ICASStacks.DataContract.ChurchAdministrative
{


    [Table("ICASDB.ClientChurchTreasury")]
    public class ClientChurchTreasury
    {
        [ScaffoldColumn(false)]
        public long ClientChurchTreasuryId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Information is required")]
        public long ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church Service Information is required")]
        public long ClientChurchServiceId { get; set; }

        [CheckNumber(0, ErrorMessage = "Collection Type Information is required")]
        public int CollectionTypeId { get; set; }

        [CheckAmount(0, ErrorMessage = "Collection type amount is required")]
        public float Amount { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(10)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date treasury added is required")]
        public string DateAdded { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int AddedByUserId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Added is required")]
        public string TimeStampAdded { get; set; }

        
    }
}
