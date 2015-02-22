using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace lpp.ExtHelper.JsonConverter
{
    public class FunctionConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType)
        {
            string jsonVal = reader.Value.ToString();
            return jsonVal;
        }

        public override void WriteJson(JsonWriter writer, object value)
        {
            writer.WriteRaw(value.ToString());
        }
    }
}
