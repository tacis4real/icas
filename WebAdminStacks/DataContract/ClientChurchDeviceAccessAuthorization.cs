﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAdminStacks.Common;

namespace WebAdminStacks.DataContract
{

    [Table("ICASWebAdmin.ClientChurchDeviceAccessAuthorization")]
    public class ClientChurchDeviceAccessAuthorization
    {
        public long ClientChurchDeviceAccessAuthorizationId { get; set; }

        [CheckNumber(0, ErrorMessage = "Invalid Login Activity")]
        public long ClientChurchLoginActivityId { get; set; }

        [CheckNumber(0, ErrorMessage = "Invalid Client Information")]
        public long ClientChurchProfileId { get; set; }

        [CheckNumber(0, ErrorMessage = "Invalid Device Information")]
        public long ClientChurchDeviceId { get; set; }

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

        public virtual ClientChurchDevice ClientChurchDevice { get; set; }
    }
}
