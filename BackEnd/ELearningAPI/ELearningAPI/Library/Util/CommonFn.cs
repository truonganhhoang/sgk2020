using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class CommonFn
    {
        /// <summary>
        /// lấy setting để convert json
        /// </summary>
        /// <param name="timezone"></param>
        /// <returns></returns>
        public static JsonSerializerSettings GetJsonSetting(DateTimeZoneHandling timezone = DateTimeZoneHandling.Local)
        {
            return new JsonSerializerSettings()
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = timezone,
                DateFormatString = "yyyy-MM-dd'T'HH:mm:ss.fffzzz",
                NullValueHandling = NullValueHandling.Ignore
            };
        }
        /// <summary>
        /// Nhận vào Object và trả lại Json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, GetJsonSetting());
        }
        /// <summary>
        /// nhận vào chuỗi json và trả về đối tượng dạng T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData, GetJsonSetting());
        }
        /// <summary>
        /// nhận vào chuỗi json và trả về đối tượng dạng bất kì
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static object DeserializeObject(string jsonData, Type boType)
        {
            return JsonConvert.DeserializeObject(jsonData,boType ,GetJsonSetting());
        }
    }
}
