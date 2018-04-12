using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAdminStacks.Common;

namespace WebAdminStacks.DataContract
{

    [Table("ICASWebAdmin.ClientDevice")]
    public class ClientDevice
    {

          public ClientDevice()
          {
              ClientDeviceAccessAuthorizations = new HashSet<ClientDeviceAccessAuthorization>();
          }

          public long ClientDeviceId { get; set; }

          public long ClientProfileId { get; set; }
      
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


          public virtual ClientProfile ClientProfile { get; set; }
          public virtual ICollection<ClientDeviceAccessAuthorization> ClientDeviceAccessAuthorizations { get; set; } 
    }
}
