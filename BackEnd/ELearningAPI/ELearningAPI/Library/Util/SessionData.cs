using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Library
{
    public static class SessionData
    {
        // get data from headers key
        public static string modelName
        {
            get
            {
                string modelName = string.Empty;
                if (HttpContext.Current?.Request?.Headers?.AllKeys?.Contains("modelName") == true)
                {
                    modelName = HttpContext.Current.Request.Headers.Get("modelName");
                }
                return modelName;
            }
        }
        // get userid
        public static Guid GetCurrentUserID()
        {
            Guid guidID = Guid.Empty;
            if (HttpContext.Current?.User?.Identity?.IsAuthenticated == true)
            {
                string strUserID = HttpContext.Current.User.Identity.GetUserId();
                Guid.TryParse(strUserID, out guidID);
            }
            return guidID;
        }
        public static int GetCurrentUserAutoID()
        {
            int autoID = 0;
            if (HttpContext.Current?.User?.Identity?.IsAuthenticated == true)
            {
                string strUserID = HttpContext.Current.User.Identity.GetUserId();
                autoID = int.Parse(strUserID);
            }
            return autoID;
        }
        /// <summary>
        /// đọc từ file json
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadToEnd(string path)
        {
            string line = string.Empty;
            try
            {
                using(StreamReader sr = new StreamReader(path))
                {
                    line = sr.ReadToEnd();
                }
            }catch(Exception ex)
            {
                CommonLog.CommonErrorLog(ex, $"Error - ReadtoEnd: {path}");
            }
            return line;
        }
        public static string ReadAllText(string path)
        {
            string line = string.Empty;
            if (File.Exists(path))
            {
                line = File.ReadAllText(path);
            }
            return line;
        }
        private static Dictionary<string, Dictionary<string, string>> _queryMySQL;
        /// <summary>
        /// lấy query theo tên bảng và key
        /// </summary>
        /// <param name="type">tên bảng</param>
        /// <param name="key">tên key</param>
        /// <returns></returns>
        public static string QueryMySQL(string type, string key)
        {
            string module = type;
            if(_queryMySQL == null)
            {
                _queryMySQL = new Dictionary<string, Dictionary<string, string>>();
            }
            if (!_queryMySQL.ContainsKey(module))
            {
                string path = string.Format(CommonUtility.GetAppSettingByKey("CRM.Query"), module),
                    fullPath = HttpContext.Current.Server.MapPath(path),
                    line = SessionData.ReadToEnd(fullPath);
                Dictionary<string, string> dicData = CommonFn.DeserializeObject<Dictionary<string, string>>(line);
                if(dicData != null && dicData.Count > 0)
                {
                    _queryMySQL.Add(module, dicData);
                }
            }
            return _queryMySQL[module][key];
        }
    }
}
