using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lpp.ExtHelper.JsonConverter;
using Newtonsoft.Json;

namespace lpp.ExtHelper.Grid
{
    [JsonObject]
    public class FieldInfo
    {
        private string m_Name = string.Empty;
        private EnumCls.FieldType m_Type = EnumCls.FieldType.auto;
        private string m_DefaultValue = null;
        private string m_Mapping = string.Empty;

        [JsonProperty(IsRequired = true, PropertyName = "name")]
        public string Name { get { return m_Name; } set { m_Name = value; } }

        [JsonProperty(IsRequired = true, PropertyName = "type")]
        [JsonConverterAttribute(typeof(FieldTypeConverter))]
        public EnumCls.FieldType Type { get { return m_Type; } set { m_Type = value; } }

        [JsonProperty(PropertyName = "defaultValue")]
        public string DefaultValue { get { return m_DefaultValue; } set { m_DefaultValue = value; } }

        [JsonProperty(PropertyName = "mapping")]
        public string Mapping { get { return m_Mapping; } set { m_Mapping = value; } }
    }
}
