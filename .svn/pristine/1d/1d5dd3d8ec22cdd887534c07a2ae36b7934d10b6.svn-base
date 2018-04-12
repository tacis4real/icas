using System.Collections.Generic;
using System.ComponentModel;
using WebCribs.TechCracker.WebCribs.TechCracker;
using WebCribs.TechCracker.WebCribs.TechCracker.EnumInfo;

namespace ICAS.Models.ClientPortalModel
{
    public enum Sex
    {
        [Description("Male")]
        Male = 1,
        [Description("Female")]
        Female,
        
    }
    
    public class SexManager
    {
        public static List<NameAndValueObject> GetList()
        {
            return EnumHelper.GetEnumList(typeof(Sex));
        }
    }
}
