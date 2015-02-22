using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace lpp.ExtHelper.JsonConverter
{
    class FieldTypeConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType)
        {
            string jsonVal = reader.Value.ToString();
            return Enum.Parse(typeof(EnumCls.FieldType), jsonVal);
        }

        public override void WriteJson(JsonWriter writer, object value)
        {
            EnumCls.FieldType val = (EnumCls.FieldType)value;
            writer.WriteValue(val.ToString());
        }
    }
}
