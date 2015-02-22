using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lpp.ExtHelper.JsonConverter;
using Newtonsoft.Json;

namespace lpp.ExtHelper.Grid
{
    [JsonObject]
    public class ColumnInfo
    {
        private string m_Text = string.Empty;
        private string m_DataIndex = string.Empty;
        private EnumCls.ColType m_ColumnType = EnumCls.ColType.gridcolumn;
        private string m_Format = null;
        private int? m_Flex = null;
        private string m_TrueText = null;
        private string m_FalseText = null;
        private string m_Tpl = null;
        private string m_Renderer = null;
        private int? m_Width = null;
        private string m_SummaryType = null;
        private string m_SummaryRenderer = null;

        [JsonProperty(IsRequired = true, PropertyName = "text")]
        public string Text { get { return m_Text; } set { m_Text = value; } }

        [JsonProperty(IsRequired = true, PropertyName = "dataIndex")]
        public string DataIndex { get { return m_DataIndex; } set { m_DataIndex = value; } }

        [JsonProperty(IsRequired = true, PropertyName = "xtype")]
        [JsonConverterAttribute(typeof(ColTypeConverter))]
        public EnumCls.ColType ColumnType { get { return m_ColumnType; } set { m_ColumnType = value; } }

        [JsonProperty(PropertyName = "format")]
        public string Format { get { return m_Format; } set { m_Format = value; } }

        [JsonProperty(PropertyName = "flex")]
        public int? Flex { get { return m_Flex; } set { m_Flex = value; } }

        [JsonProperty(PropertyName = "trueText")]
        public string TrueText { get { return m_TrueText; } set { m_TrueText = value; } }

        [JsonProperty(PropertyName = "falseText")]
        public string FalseText { get { return m_FalseText; } set { m_FalseText = value; } }

        [JsonProperty(IsRequired = true, PropertyName = "tpl")]
        public string Tpl { get { return m_Tpl; } set { m_Tpl = value; } }

        [JsonProperty(IsRequired = true, PropertyName = "renderer")]
        [JsonConverter(typeof(FunctionConverter))]
        public string Renderer
        {
            get { return m_Renderer; }
            set
            {
                m_Renderer = string.Format("function(val){0}{2}{1}", "{", "}", value);
            }
        }

        [JsonProperty(PropertyName = "width")]
        public int? Width { get { return m_Width; } set { m_Width = value; } }

        [JsonProperty(PropertyName = "summaryType")]
        [JsonConverter(typeof(FunctionConverter))]
        public string SummaryType
        {
            get { return m_SummaryType; }
            set { m_SummaryType = value; }
        }

        [JsonProperty(PropertyName = "summaryRenderer")]
        [JsonConverter(typeof(FunctionConverter))]
        public string SummaryRenderer
        {
            get { return m_SummaryRenderer; }
            set { m_SummaryRenderer = value; }
        }
    }
}
