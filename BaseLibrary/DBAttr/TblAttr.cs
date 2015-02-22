using System;
using System.Collections.Generic;
using System.Text;

namespace lpp.DBAttr
{
    /// <summary>
    /// 数据表、实体映射特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class TblAttr : Attribute
    {
        public string Name { get; set; }
        public string Alias { get; set; }

        public TblAttr(string name, string alias = "A")
        {
            this.Name = name;
            this.Alias = alias;
        }
    }
}
