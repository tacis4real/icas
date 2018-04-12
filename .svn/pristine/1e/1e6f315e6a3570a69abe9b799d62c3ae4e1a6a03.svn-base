using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICASStacks.DataContract.BioEnroll
{
    
    public class StationReg
    {

        public long StationRegId { get; set; }

        [Required(ErrorMessage = "Station name is required")]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "Station name must be between 5 and 150 characters")]
        public string StationName { get; set; }

        [Required(ErrorMessage = "Station Key is required", AllowEmptyStrings = false)]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "Invalid Station Key")]
        public string StationKey { get; set; }

        [Required(ErrorMessage = "Device ID is required", AllowEmptyStrings = false)]
        [StringLength(50)]
        public string DeviceId { get; set; }

        [StringLength(20)]
        public string DeviceIP { get; set; }

    }
}
