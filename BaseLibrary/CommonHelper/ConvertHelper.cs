using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace lpp.CommonHelper
{
    public static class ConvertHelper
    {
        
    }

    /// <summary>
    /// ConvertOne2Other的对象属性映射类
    /// </summary>
    public class One2OtherMap
    {
        public string SrcPropName { get; set; }
        public string DestPropName { get; set; }
    }

    public delegate object Converter(object raw);
}
