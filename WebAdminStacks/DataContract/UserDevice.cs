using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAdminStacks.Common;

namespace WebAdminStacks.DataContract
{
    [Table("ICASWebAdmin.UserDevice")]
   public class UserDevice
    {
          public UserDevice()
          {
              DeviceAccessAuthorizations = new HashSet<DeviceAccessAuthorization>();
          }
          public long UserDeviceId { get; set; }
          public long UserId { get; set; }
      
          [Required(AllowEmptyStrings = false, ErrorMessage = "Device Serial Number is required")]
          [Column(TypeName = "varchar")]
          [StringLength(50)]
          public string DeviceSerialNumber { get; set; }
       
          [Column(TypeName = "varchar")]
          [StringLength(50)]
          public string DeviceName { get; set; }

          [Column(TypeName = "text")]
          [StringLength(800)]
          public string NotificationCode { get; set; }

          public DeviceOSType DeviceOSType { get; set; }
          public bool IsAuthorized { get; set; }

          [Column(TypeName = "varchar")]
          [StringLength(10)]
          public string AuthorizationCode { get; set; }

          [Column(TypeName = "varchar")]
          [Required(AllowEmptyStrings = false)]
          [StringLength(35)]
          public string TimeStampRegistered { get; set; }

          public virtual User User { get; set; }
          public virtual ICollection<DeviceAccessAuthorization> DeviceAccessAuthorizations { get; set; } 
    }
}
