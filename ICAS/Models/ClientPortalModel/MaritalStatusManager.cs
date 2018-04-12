using System.Collections.Generic;
using System.ComponentModel;
using WebCribs.TechCracker.WebCribs.TechCracker;
using WebCribs.TechCracker.WebCribs.TechCracker.EnumInfo;

namespace ICAS.Models.ClientPortalModel
{
    
    public enum MaritalStatus
    {
        None=0,
        [Description("Single")]
        Single = 1,
        [Description("Married")]
        Married,
        [Description("Divorced")]
        Divorced,
        [Description("Widow")]
        Widow,
        [Description("Widower")]
        Widower,
    }

    public class MaritalStatusManager
    {
        public static List<NameAndValueObject> GetList()
        {
            return EnumHelper.GetEnumList(typeof(Sex));
        }
    }
}
