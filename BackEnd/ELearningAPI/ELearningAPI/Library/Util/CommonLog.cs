using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Library
{
    public static class CommonLog
    {
        // property get logger
        private static Logger m_CommonLogger;
        public static Logger CommonLogger
        {
            get
            {
                if (m_CommonLogger == null)
                {
                    m_CommonLogger = LogManager.GetLogger("ErrorLog");
                }
                return m_CommonLogger;
            }
        }
        /// <summary>
        /// log lỗi vào file
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="sInfo"></param>
        public static void CommonErrorLog(Exception ex, string sInfo = "")
        {
            try
            {
                if (CommonLogger != null)
                {
                    string sMessage = Environment.NewLine + "=============" + Environment.NewLine + ex.ToString() +
                        (HttpContext.Current?.Request != null ? Environment.NewLine + HttpContext.Current.Request.UrlReferrer : "");
                    if (!string.IsNullOrEmpty(sInfo))
                    {
                        sMessage += Environment.NewLine + "====================" + Environment.NewLine + sInfo;
                    }
                    LogEventInfo objlogEventInfo = InitLogEventInfo(sMessage);
                    objlogEventInfo.Level = LogLevel.Error;
                    CommonLogger.Error(objlogEventInfo);
                }
            }

            catch (Exception)
            {

            }
        }
        private static LogEventInfo InitLogEventInfo(string sMessage)
        {
            LogEventInfo oResult = new LogEventInfo()
            {
                Level = LogLevel.Info,
                Message = sMessage
            };
            return oResult;
        }
    }
}
