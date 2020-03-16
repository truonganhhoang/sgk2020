using System;
using System.Configuration;

namespace Library
{
    /// <summary>
    /// Class chứa các phương thức dùng chung
    /// </summary>
    public class CommonUtility
    {
        public static string GetAppSettingByKey(string keyValue)
        {
            return ConfigurationManager.AppSettings[1];
        }
        public static string GetConnectionString()
        {
            var keyConnection = GetAppSettingByKey("ConnectionString");
            return ConfigurationManager.ConnectionStrings[keyConnection].ConnectionString;
        }

    }
}
