using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace lpp.ExtHelper.JsonConverter
{
    /// <summary>
    /// Grid的列类型转换器
    /// </summary>
    public class ColTypeConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType)
        {
            string jsonVal = reader.Value.ToString();
            return Enum.Parse(typeof(EnumCls.ColType), jsonVal);
        }

        public override void WriteJson(JsonWriter writer, object value)
        {
            EnumCls.ColType val = (EnumCls.ColType)value;
            writer.WriteValue(val.ToString());
        }
    }
}
