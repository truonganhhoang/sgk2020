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
            return ConfigurationManager.AppSettings[keyValue];
        }
    }
}
