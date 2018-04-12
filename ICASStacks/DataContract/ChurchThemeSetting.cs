using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;

namespace ICASStacks.DataContract
{
    [Table("ICASDB.ChurchThemeSetting")]
    public class ChurchThemeSetting
    {
        [ScaffoldColumn(false)]
        public long ChurchThemeSettingId { get; set; }

        [CheckNumber(0, ErrorMessage = "Church Information is required")]
        public long ChurchId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Church Color")]
        public string ThemeColor { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Church Logo")]
        public string ThemeLogo { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Church Logo Path")]
        public string ThemeLogoPath { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(35)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Registered is required")]
        public string TimeStampRegistered { get; set; }

        [CheckNumber(0, ErrorMessage = "Admin User Information is required")]
        public int RegisteredByUserId { get; set; }

        #region Navigation Properties
        //public virtual Church Church { get; set; }
        #endregion
    }
}
