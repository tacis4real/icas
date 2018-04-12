using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ICAS.Areas.Helpers
{
    public class Helper
    {
        public static bool Compare<T>(List<T> objectList1, List<T> objectList2, List<string> excludeList)
        {
            try
            {
                var type = typeof(T);
                if (objectList1.Count != objectList2.Count)
                    return false;
                for (var i = 0; i < objectList1.Count; i++)
                {
                    var object1 = objectList1[i];
                    var object2 = objectList2[i];
                    if (Equals(object1, default(T)) || Equals(object2, default(T)))
                        return false;

                    foreach (var property in type.GetProperties())
                    {
                        if (excludeList != null)
                        {
                            if (excludeList.Any(m => m == property.Name))
                                continue;
                        }
                        var object1Value = string.Empty;
                        var object2Value = string.Empty;
                        if (type.GetProperty(property.Name).GetValue(object1, null) != null)
                            object1Value = type.GetProperty(property.Name).GetValue(object1, null).ToString();
                        if (type.GetProperty(property.Name).GetValue(object2, null) != null)
                            object2Value = type.GetProperty(property.Name).GetValue(object2, null).ToString();
                        if (object1Value.Trim() != object2Value.Trim())
                        {
                            return false;
                        }
                    }

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public static bool Compare<T>(T object1, T object2, List<string> excludeList)
        {
            try
            {
                var type = typeof(T);
                if (Equals(object1, default(T)) || Equals(object2, default(T)))
                    return false;

                foreach (var property in type.GetProperties())
                {
                    if (excludeList != null)
                    {
                        if (excludeList.Any(m => m == property.Name))
                            continue;
                    }
                    var object1Value = string.Empty;
                    var object2Value = string.Empty;
                    if (type.GetProperty(property.Name).GetValue(object1, null) != null)
                        object1Value = type.GetProperty(property.Name).GetValue(object1, null).ToString();
                    if (type.GetProperty(property.Name).GetValue(object2, null) != null)
                        object2Value = type.GetProperty(property.Name).GetValue(object2, null).ToString();
                    if (object1Value.Trim() != object2Value.Trim())
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public static void AddOrUpdateAppSettings(string key, string value)
        {
            try
            {
                var config = WebConfigurationManager.OpenWebConfiguration((HttpContext.Current.Request.ApplicationPath));
                var settings = config.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                return;
            }
            catch (Exception)
            {
                return;
            }
        }

        public static string GetSettingValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings.Get(key);
            }
            catch (Exception)
            {
                AddOrUpdateAppSettings(key, "0");
                return null;
            }
        }

        public static void DeleteFile(string fileLoc)
        {
            try
            {
                if (System.IO.File.Exists(fileLoc))
                {
                    System.IO.File.Delete(fileLoc);
                }
            }
            catch (Exception)
            {

            }

        }
    }
}