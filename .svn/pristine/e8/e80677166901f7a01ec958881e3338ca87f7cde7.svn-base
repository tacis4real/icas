using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICASStacks.DataContract.BioEnroll
{

    public class StationRegObj
    {

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

    public class StationRespObj
    {
        public string APIAccessKey;
        public string DeviceId;
        public string StationKey;
        public long ClientStationId;
        //public int BusinessLocationTypeId;
        //public int BusinessLocationId;
        public ResponseStatus ResponseStatus;
    }

    public class ResponseStatus
    {
        public bool IsSuccessful;
        public ResponseMessage Message;
        public long ReturnedId;
        public string ReturnedValue;
    }

    public class ResponseMessage
    {
        public string FriendlyMessage;
        public string TechnicalMessage;
        public string MessageId;
    }


    public class RemoteUserInformation
    {

        public List<StaffUser> UserInfos;
        public List<UserProfile> UserProfileInfos;
        public List<ClientStation> ClientStationInfos;

    }


    public class RemoteUserInfo
    {
        public StaffUser UserInfo;
        public UserProfile UserProfileInfo;
    }
}
