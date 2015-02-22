using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace lpp.ExtHelper.JsonConverter
{
    /// <summary>
    /// Grid的Feature类型转换器
    /// </summary>
    public class FeatureTypeNameConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType)
        {
            string jsonVal = reader.Value.ToString();
            return Enum.Parse(typeof(EnumCls.FeatureType), jsonVal);
        }

        public override void WriteJson(JsonWriter writer, object value)
        {
            EnumCls.FeatureType val = (EnumCls.FeatureType)value;
            writer.WriteValue(val.ToString());
        }
    }
}
