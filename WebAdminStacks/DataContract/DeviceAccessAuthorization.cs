using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAdminStacks.Common;

namespace WebAdminStacks.DataContract
{
    [Table("ICASWebAdmin.DeviceAccessAuthorization")]
    public  class DeviceAccessAuthorization
    {
        public long DeviceAccessAuthorizationId { get; set; }

        [CheckNumber(0, ErrorMessage = "Invalid Login Activity")]
        public long UserLoginActivityId { get; set; }

        [CheckNumber(0, ErrorMessage = "Invalid User Information")]
        public long UserId { get; set; }

        [CheckNumber(0, ErrorMessage = "Invalid Device Information")]
        public long UserDeviceId { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Authorized Date is required", AllowEmptyStrings = false)]
        [StringLength(10)]
        public string AuthorizedDate { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Authorized Time is required", AllowEmptyStrings = false)]
        [StringLength(15)]
        public string AuthorizedTime { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Authorized Device Serial Number is required", AllowEmptyStrings = false)]
        [StringLength(50)]
        public string AuthorizedDeviceSerialNumber { get; set; }

        [Column(TypeName = "varchar")]
        [Required(ErrorMessage = "Authorization Token is required", AllowEmptyStrings = false)]
        [StringLength(50)]
        public string AuthorizationToken { get; set; }
        public bool IsExpired { get; set; }

        public virtual UserDevice UserDevice { get; set; }
    }
}
