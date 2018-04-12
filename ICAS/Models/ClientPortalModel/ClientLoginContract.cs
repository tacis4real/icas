using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ICAS.Models.ClientPortalModel
{
    #region Clients Data Model Center

    public class ClientLoginContract
    {
        [Required(ErrorMessage = "* Required")]
        [DisplayName("User name")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }
    }

    public class ClientData
    {
        public long ClientId { get; set; }
        public long ClientProfileId { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string AuthToken { get; set; }
        public string[] Roles { get; set; }
    }



    #region Client Church

    public class ClientChurchUserProfileInfo
    {
        [ScaffoldColumn(false)]
        public long ClientChurchProfileId { get; set; }

        [DisplayName("Full Name")]
        //[ReadOnly(true)]
        public string FullName { get; set; }

        public int SexId { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }

        public string Email { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "* Required")]
        public string MobileNo { get; set; }

        public string TimeStampRegistered { get; set; }

        public string[] MyRoles { get; set; }

        [DisplayName("Roles")]
        public string Roles { get; set; }

        public string LandPhone { get; set; }
        public string DateOfBirth { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string ResidentialAddress { get; set; }
        public string FaceBookId { get; set; }
        public string TwitterId { get; set; }
        public string BBPin { get; set; }
        public string GooglePlusId { get; set; }

    }

    public class ClientChurchProfileInfo
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        //public string LastName { get; set; }
        //public string MiddleName { get; set; }
        public Sex Sex { get; set; }
        public string MobileNo { get; set; }
        public string LandPhone { get; set; }
        public string DateOfBirth { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string ResidentialAddress { get; set; }
        public string FaceBookId { get; set; }
        public string TwitterId { get; set; }
        public string BBPin { get; set; }
        public string GooglePlusId { get; set; }
    }

    #endregion




    public class ClientChurchData
    {
        public long ClientId { get; set; }
        public long ChurchId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string LogoPath { get; set; }
        public string Theme { get; set; }
    }

    public class ClientProfileInfo
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        //public string LastName { get; set; }
        //public string MiddleName { get; set; }
        public Sex Sex { get; set; }
        public string MobileNo { get; set; }
        public string LandPhone { get; set; }
        public string DateOfBirth { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string ResidentialAddress { get; set; }
        public string FaceBookId { get; set; }
        public string TwitterId { get; set; }
        public string BBPin { get; set; }
        public string GooglePlusId { get; set; }
    }

    public class ResetChurchAdminPasswordContract
    {

        [Required(ErrorMessage = "* Required")]
        [DisplayName("User Name")]
        [StringLength(50)]
        public string UserName { get; set; }
        public string ProcessType { get; set; }
        public string FullName { get; set; }
        public long ClientProfileId { get; set; }
        public int CallerType { get; set; }

    }

    public class ClientUserProfileInfo
    {
        [ScaffoldColumn(false)]
        public long ClientProfileId { get; set; }

        [DisplayName("Full Name")]
        //[ReadOnly(true)]
        public string FullName { get; set; }

        public int SexId { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }

        public string Email { get; set; }

        public string MobileNo { get; set; }

        public string TimeStampRegistered { get; set; }

        public string[] MyRoles { get; set; }

        [DisplayName("Roles")]
        public string Roles { get; set; }

        public string LandPhone { get; set; }
        public string DateOfBirth { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string ResidentialAddress { get; set; }
        public string FaceBookId { get; set; }
        public string TwitterId { get; set; }
        public string BBPin { get; set; }
        public string GooglePlusId { get; set; }
        
    }


    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "Password and Confirm Password must match.")]
    public class ClientPortalUserContract
    {
        [ScaffoldColumn(false)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [StringLength(50)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "* Required")]
        [StringLength(50)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [StringLength(50)]
        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Sex")]
        [Range(1, 2)]
        public int SexId { get; set; }

        public string Sex
        {
            get
            {
                switch (SexId)
                {
                    case 1:
                        return "Male";
                    case 2:
                        return "Female";
                    default:
                        return "N/A";

                }
            }
        }


        [Required(ErrorMessage = "* Required")]
        [StringLength(50)]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "This email address is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(11)]
        [DisplayName("Mobile Number")]
        public string MobileNo { get; set; }

        [DisplayName("Time Stamp Registered")]
        public string TimeStampRegistered { get; set; }

        public string[] MyRoles { get; set; }



        [DisplayName("Roles")]
        public string Roles { get; set; }


        public bool Status { get; set; }


        [ScaffoldColumn(false)]
        public int UserFirstLogin { get; set; }


        [Required(ErrorMessage = "* Required")]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "* Required")]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [DisplayName("Full Name")]
        [ReadOnly(true)]

        public string FullName
        {
            get { return LastName + " " + FirstName + " " + MiddleName; }
        }
    }


    [PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "Password and Confirm Password must match.")]
    public class ChangeClientPasswordContract
    {
        [Required(ErrorMessage = "* Required")]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("Old Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "* Required")]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm New Password")]
        public string ConfirmPassword { get; set; }

        [ScaffoldColumn(false)]
        public string UserName { get; set; }
    }

    #endregion
}