﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AwesomeMvc;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS.Areas.Admin.Models.PortalModel
{
    
    
    [WebAdminStacks.Common.PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "Password and Confirm Password must match.")]
    public class AuthPortalUser
    {

            [DisplayName("Last Name")]
            [Required(ErrorMessage = "* Required")]
            [StringLength(50)]
            public string LastName { get; set; }

            [DisplayName("First Name")]
            [Required(ErrorMessage = "* Required")]
            [StringLength(50)]
            public string FirstName { get; set; }


            [DisplayName("Middle Name")]
            [StringLength(50)]
            public string MiddleName { get; set; }

            [Required(ErrorMessage = "Mobile Number is required", AllowEmptyStrings = false)]
            [StringLength(15, MinimumLength = 7, ErrorMessage = "Mobile number must be between 7 amd 15 digits")]
            //[CheckMobileNumber(ErrorMessage = "Invalid Mobile Number")]
            public string MobileNo { get; set; }

            [DataType(DataType.EmailAddress)]
            [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "This email address is not valid.")]
            [Required(ErrorMessage = "* Required")]
            [StringLength(50)]
            public string Email { get; set; }

            public int FailedPasswordCount { get; set; }
           
            public bool IsApproved { get; set; }
            public bool IsDeleted { get; set; }
            public bool IsFirstTimeLogin { get; set; }
            public bool IsLockedOut { get; set; }

            [StringLength(35)]
            public string LastLockedOutTimeStamp { get; set; }

            [StringLength(35)]
            public string LastLoginTimeStamp { get; set; }
            
            public string LastPasswordChangeTimeStamp { get; set; }

            [UIHint("MultiLookup")]
            public IEnumerable<int> Roles { get; set; }

            public List<NameAndValueObject> AllRoles { get; set; }

            public int[] MyRoleIds { get; set; }

            public string[] MyRoles { get; set; }


            [DataType(DataType.Password)]
            [Required(ErrorMessage = "* Required")]
            [StringLength(150)]
            [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Required(ErrorMessage = "* Required")]
            [StringLength(150)]
            [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
            public string ConfirmPassword { get; set; }

            public string SelectedRoles { get; set; }

            public string Sex
            {
                get
                {
                    if (SexId < 1)
                    {
                        return "";
                    }
                    return SexId == 1 ? "Male" : "Female";
                }
            }

            [DisplayName("Sex")]
            [Required(ErrorMessage = "* Required")]
            [UIHint("Odropdown")]
            [AweUrl(Action = "GetSexes", Controller = "ODropDownData")]
            public int SexId { get; set; }

            [StringLength(35)]
            public string TimeStampRegistered { get; set; }

            public long UserId { get; set; }

            [Required(ErrorMessage = "* Required")]
            [StringLength(50)]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            public string Action { get; set; }

    }


    #region Client Profile
    [WebAdminStacks.Common.PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "Password and Confirm Password must match.")]
    public class AuthClientPortalUser
    {
        public long ChurchId { get; set; }

        public long ClientProfileId { get; set; }

        //[CheckNumber(0, ErrorMessage = "Client/Church Information is required")]
        public long ClientId { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "* Required")]
        [StringLength(150)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Mobile Number is required", AllowEmptyStrings = false)]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Mobile number must be between 7 amd 15 digits")]
        //[CheckMobileNumber(ErrorMessage = "Invalid Mobile Number")]
        public string MobileNo { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "This email address is not valid.")]
        //[Required(ErrorMessage = "* Required")]
        [StringLength(50)]
        public string Email { get; set; }

        public int FailedPasswordCount { get; set; }

        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public bool IsLockedOut { get; set; }

        [StringLength(35)]
        public string LastLockedOutTimeStamp { get; set; }

        [StringLength(35)]
        public string LastLoginTimeStamp { get; set; }

        public string LastPasswordChangeTimeStamp { get; set; }

        [UIHint("MultiLookup")]
        public IEnumerable<int> Roles { get; set; }

        public List<NameAndValueObject> AllRoles { get; set; }

        public int[] MyRoleIds { get; set; }

        public string[] MyRoles { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "* Required")]
        [StringLength(150)]
        [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "* Required")]
        [StringLength(150)]
        [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
        public string ConfirmPassword { get; set; }

        public string SelectedRoles { get; set; }

        public string Sex
        {
            get
            {
                if (SexId < 1)
                {
                    return "";
                }
                return SexId == 1 ? "Male" : "Female";
            }
        }

        [DisplayName("Sex")]
        [Required(ErrorMessage = "* Required")]
        [UIHint("Odropdown")]
        [AweUrl(Action = "GetSexes", Controller = "ODropDownData")]
        public int SexId { get; set; }

        [StringLength(35)]
        public string TimeStampRegistered { get; set; }

        [Required(ErrorMessage = "* Required")]
        [StringLength(50)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

    }



    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "Password and Confirm Password must match.")]
    public class AuthClientChurchPortalUser
    {
        public long ChurchId { get; set; }

        public long ClientChurchProfileId { get; set; }

        //[CheckNumber(0, ErrorMessage = "Client/Church Information is required")]
        public long ClientId { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "* Required")]
        [StringLength(150)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Mobile Number is required", AllowEmptyStrings = false)]
        [StringLength(15, MinimumLength = 7, ErrorMessage = "Mobile number must be between 7 amd 15 digits")]
        //[CheckMobileNumber(ErrorMessage = "Invalid Mobile Number")]
        public string MobileNo { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "This email address is not valid.")]
        //[Required(ErrorMessage = "* Required")]
        [StringLength(50)]
        public string Email { get; set; }

        public int FailedPasswordCount { get; set; }

        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public bool IsLockedOut { get; set; }

        [StringLength(35)]
        public string LastLockedOutTimeStamp { get; set; }

        [StringLength(35)]
        public string LastLoginTimeStamp { get; set; }

        public string LastPasswordChangeTimeStamp { get; set; }

        [UIHint("MultiLookup")]
        public IEnumerable<int> Roles { get; set; }

        public List<NameAndValueObject> AllRoles { get; set; }

        public int[] MyRoleIds { get; set; }

        public string[] MyRoles { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "* Required")]
        [StringLength(150)]
        [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "* Required")]
        [StringLength(150)]
        [ValidatePasswordLength(ErrorMessage = "Password length must be at least 8 character long")]
        public string ConfirmPassword { get; set; }

        public string SelectedRoles { get; set; }

        public string Sex
        {
            get
            {
                if (SexId < 1)
                {
                    return "";
                }
                return SexId == 1 ? "Male" : "Female";
            }
        }

        [DisplayName("Sex")]
        [Required(ErrorMessage = "* Required")]
        [UIHint("Odropdown")]
        [AweUrl(Action = "GetSexes", Controller = "ODropDownData")]
        public int SexId { get; set; }

        [StringLength(35)]
        public string TimeStampRegistered { get; set; }

        [Required(ErrorMessage = "* Required")]
        [StringLength(50)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

    }
    #endregion
}