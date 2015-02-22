using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace lpp.JsonHelper.Converter
{
    /// <summary>
    /// 长时间类型数据转换器
    /// 格式：HH:mm:ss
    /// </summary>
    public class LongTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            DateTime? result;
            DateTime tmpResult;
            if (!DateTime.TryParse(reader.Value.ToString(), out tmpResult))
            {
                result = null;
            }
            else
            {
                result = tmpResult;
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string result = "\"\"";
            if (null != value && value is DateTime)
            {
                result = string.Format("\"{0}\"", ((DateTime)value).ToLongTimeString());
            }
            else
            {
                throw new ArgumentException("异常：LongTimeConvert特性需设置在数据类型为DateTime的公开属性上！");
            }

            writer.WriteRawValue(result);
        }
    }
}
