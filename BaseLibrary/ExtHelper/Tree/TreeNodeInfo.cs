using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace lpp.ExtHelper.Tree
{
    [JsonObject]
    public class TreeNodeInfo
    {
        private string m_ID = string.Empty;
        private string m_Text = string.Empty;
        private string m_Cls = string.Empty;
        private string m_Icon = string.Empty;
        private string m_IconCls = string.Empty;
        private string m_Href = string.Empty;
        private string m_HrefTarget = string.Empty;
        private bool m_Expandable = true;
        private bool m_Expanded = false;
        private bool m_Leaf = false;
        private string m_QTip = string.Empty;
        private string m_QTitle = string.Empty;
        private bool m_AllowDrag = true;
        private bool m_AllowDrop = true;

        [JsonProperty(IsRequired = true, PropertyName = "id")]
        public string ID { get { return m_ID; } set { m_ID = value; } }

        [JsonProperty(IsRequired = true, PropertyName = "text")]
        public string Text { get { return m_Text; } set { m_Text = value; } }

        [JsonProperty(PropertyName = "cls")]
        public string Cls { get { return m_Cls; } set { m_Cls = value; } }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get { return m_Icon; } set { m_Icon = value; } }

        [JsonProperty(PropertyName = "iconCls")]
        public string IconCls { get { return m_IconCls; } set { m_IconCls = value; } }

        [JsonProperty(PropertyName = "href")]
        public string Href { get { return m_Href; } set { m_Href = value; } }

        [JsonProperty(PropertyName = "htefTarget")]
        public string HrefTarget { get { return m_HrefTarget; } set { m_HrefTarget = value; } }

        [JsonProperty(PropertyName = "expandable")]
        public bool Expandable { get { return m_Expandable; } set { m_Expandable = value; } }

        [JsonProperty(PropertyName = "expanded")]
        public bool Expanded { get { return m_Expanded; } set { m_Expanded = value; } }

        [JsonProperty(PropertyName = "leaf")]
        public bool Leaf { get { return m_Leaf; } set { m_Leaf = value; } }

        [JsonProperty(PropertyName = "qtip")]
        public string QTip { get { return m_QTip; } set { m_QTip = value; } }

        [JsonProperty(PropertyName = "qtitle")]
        public string QTitle { get { return m_QTitle; } set { m_QTitle = value; } }

        [JsonProperty(PropertyName = "allowDrag")]
        public bool AllowDrag { get { return m_AllowDrag; } set { m_AllowDrag = value; } }

        [JsonProperty(PropertyName = "allowDrop")]
        public bool AllowDrop { get { return m_AllowDrop; } set { m_AllowDrop = value; } }

        [JsonProperty(PropertyName = "checked", IsRequired = false)]
        public bool? Checked { get; set; }

        [JsonProperty(PropertyName = "children", IsRequired = false)]
        public List<TreeNodeInfo> Children { get; set; }

        public TreeNodeInfo() { }

        public TreeNodeInfo(string id, string text)
        {
            this.m_ID = id;
            this.m_Text = text;
        }

        public TreeNodeInfo(string id, string text, bool leaf)
            : this(id, text)
        {
            this.m_Leaf = leaf;
        }

        public TreeNodeInfo(string id, string text, bool leaf, string qtip, string qtitle)
            : this(id, text, leaf)
        {
            this.m_QTip = qtip;
            this.m_QTitle = qtitle;
        }

        public TreeNodeInfo(string id, string text, bool leaf, string href)
            : this(id, text, leaf)
        {
            this.m_Href = href;
            this.m_HrefTarget = "_self";
        }

        public TreeNodeInfo(string id, string text, bool leaf, bool isChecked)
            : this(id, text, leaf)
        {
            this.Checked = isChecked;
        }
    }
}
