using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAdminStacks.Common;

namespace WebAdminStacks.DataContract
{
    [Table("ICASWebAdmin.UserLoginActivity")]
    public class UserLoginActivity
    {
        public long UserLoginActivityId { get; set; }

        [CheckNumber(0, ErrorMessage = "User Information is required")]
        public long UserId { get; set; }

        public bool IsLoggedIn { get; set; }

        public UserLoginSource LoginSource { get; set; }
    

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string LoginAddress { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string LoginToken { get; set; }

        [Column(TypeName = "varchar")]
        [Required]
        [StringLength(35)]
        public string LoginTimeStamp { get; set; }
        public virtual User User { get; set; }
    }
}
