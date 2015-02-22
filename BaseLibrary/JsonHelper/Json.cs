using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace lpp.JsonHelper
{
    public static class Json
    {
        /// <summary>
        /// 将对象格式化为Json格式字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="model">对象</param>
        /// <returns></returns>
        public static string SerializeToJson<T>(T model)
        {
            StringBuilder json = new StringBuilder();
            StringWriter sw = new StringWriter(json);
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            JsonSerializer serializer = JsonSerializer.Create(setting);
            serializer.Serialize(sw, model);
            sw.Close();

            return json.ToString();
        }

        /// <summary>
        /// 将Json格式字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">Json格式字符串</param>
        /// <returns></returns>
        public static T DeserializeFromJson<T>(string json) where T : class
        {
            StringReader sr = new StringReader(json);
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            JsonSerializer serializer = JsonSerializer.Create(setting);
            T result = serializer.Deserialize(sr, typeof(T)) as T;

            return result;
        }

        /// <summary>
        /// 将字典对象序列化为Json格式字符串
        /// </summary>
        /// <param name="dic">字典对象</param>
        /// <param name="keyKey">Json格式字符串中字典键的键</param>
        /// <param name="valueKey">Json格式字符串中字典值的键</param>
        /// <returns></returns>
        public static string SerializeDicToJson(Dictionary<object, object> dic, string keyKey, string valueKey)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            foreach (object key in dic.Keys)
            {
                json.AppendFormat("{0}\"{2}\":\"{3}\",\"{4}\":\"{5}\"{1},"
                    , "{"
                    , "}"
                    , keyKey
                    , Convert.ToString(key)
                    , valueKey
                    , Convert.ToString(dic[key]));
            }
            if (json.Length > 2)
            {
                json.Remove(json.Length - 1, 1);
            }

            json.Append("]");

            return json.ToString();
        }
    }
}
