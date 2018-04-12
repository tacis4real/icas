using System;
using System.Collections.Generic;
using System.Globalization;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace ICAS.Models.ClientPortalModel
{
    public class TabOrderManager
    {
        public static List<NameAndValueObject> GetList()
        {
            try
            {
                var mArry = new List<NameAndValueObject>();
                for (var i = 1; i < 16; i++)
                {
                    var mVal = new NameAndValueObject { Id = i, Name = i.ToString(CultureInfo.InvariantCulture) };
                    mArry.Add(mVal);
                }
                return mArry;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public static List<NameAndValueObject> GetTabOrders(int maxId)
        {
            var mArry = new List<NameAndValueObject>();
            try
            {
                for (var i = maxId + 1; i < 16; i++)
                {
                    var mValue = new NameAndValueObject { Id = i, Name = i.ToString(CultureInfo.InvariantCulture) };
                    mArry.Add(mValue);
                }
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
            return mArry;
        }
    }
    
}
