using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lpp.ExtHelper.JsonConverter;
using Newtonsoft.Json;

namespace lpp.ExtHelper.Grid
{
    [JsonObject]
    public class FeatureInfo
    {
        private EnumCls.FeatureType? m_FType = null;
        private string m_GetAdditionData = null;

        [JsonProperty(IsRequired = true, PropertyName = "ftype")]
        [JsonConverter(typeof(FeatureTypeNameConverter))]
        public EnumCls.FeatureType? FType { get { return m_FType; } set { m_FType = value; } }

        /// <summary>
        /// 设置值是格式为result[rowBody] = ".......";result[rowBodyCls] = ".......";
        /// 通过data对象获取当前行的原始数据
        /// 通过rowIndex对象获取当前行索引
        /// 通过record对象获取当前行的Model对象
        /// 通过orig对象获取当前行的？？
        /// </summary>
        [JsonProperty(IsRequired = true, PropertyName = "getAdditionalData")]
        [JsonConverter(typeof(FunctionConverter))]
        public string GetAdditionData
        {
            get { return m_GetAdditionData; }
            set
            {
                m_GetAdditionData = string.Format("function(data, rowIndex, record, orig){0}"
                   + " var headerCt = this.view.headerCt; "
                   + " var colspan = headerCt.getColumnCount(); "
                   + " var result = {rowBodyColspan:colspan}; "
                   + " {2} {1}", "{", "}", value);
            }
        }
    }
}
