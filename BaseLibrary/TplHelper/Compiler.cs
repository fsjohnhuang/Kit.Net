using lpp.TplHelper.Attr;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace lpp.TplHelper
{
    /// <summary>
    /// 字符串解释器
    /// version beta 0.1
    /// 1. 变量以@开头
    /// </summary>
    public static class Compiler
    {
        private class PropAttrComparer : IComparer<PropAttr>
        {
            public int Compare(PropAttr x, PropAttr y)
            {
                int dLen = x.AttrName.Length - x.AttrName.Length;
                return dLen;
            }
        }

        private struct PropAttr{
            public string PropName { get; set; }
            public string AttrName { get; set; }
            public string Format { get; set; }
        }

        // 用于保存已解释过的数据实体类型结构
        private static Dictionary<Type, List<PropAttr>> DataEntityStructures = new Dictionary<Type, List<PropAttr>>();

        /// <summary>
        /// 对命名参数进行处理
        /// </summary>
        /// <param name="attrName">命名参数</param>
        /// <param name="orig">命令参数已经过Format处理的的原始值</param>
        /// <returns>处理后的值</returns>
        public delegate object Map(string attrName, object orig);

        /// <summary>
        /// 预编译数据实体类型结构
        /// </summary>
        /// <param name="type">数据实体类型</param>
        public static void Recompile(Type type)
        {
            List<PropAttr> propAttrs = new List<PropAttr>();
            PropertyInfo[] propInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int i = propInfos.Length - 1; i >= 0; --i)
            {
                PropertyInfo propInfo = propInfos[i];
                object[] attrs = propInfo.GetCustomAttributes(typeof(TplVar), false);
                if (null == attrs || 0 == attrs.Length) continue;

                TplVar tplVarAttr = attrs[0] as TplVar;
                PropAttr propAttr = new PropAttr();
                propAttr.PropName = propInfo.Name;
                propAttr.AttrName = tplVarAttr.Name;
                propAttr.Format = tplVarAttr.Format;
                propAttrs.Add(propAttr);
            }

            // 按AttrName内容长度由长到短排序
            propAttrs.Sort(new PropAttrComparer());

            DataEntityStructures[type] = propAttrs;
        }

        /// <summary>
        /// 生成字符串
        /// </summary>
        /// <typeparam name="T">数据实体类型</typeparam>
        /// <param name="sc">字符串模板</param>
        /// <param name="de">数据实体实例</param>
        /// <returns>填充数据的字符</returns>
        public static string Compile<T>(string sc, T de, Map map = null) where T : class
        {
            Type type = typeof(T);
            List<PropAttr> propAttrs = null;
            if (DataEntityStructures.ContainsKey(type))
            {
                propAttrs = DataEntityStructures[type];
            }
            else
            {
                Recompile(type);
                propAttrs = DataEntityStructures[type];
            }

            for (int i = 0, len = propAttrs.Count; i < len; ++i)
            {
                PropAttr propAttr = propAttrs[i];

                PropertyInfo propInfo = type.GetProperty(propAttr.PropName);
                object val = propInfo.GetValue(de, null);
                if ((propInfo.PropertyType == typeof(DateTime) || propInfo.PropertyType == typeof(DateTime?)) && !string.IsNullOrEmpty(propAttr.Format))
                {
                    sc = sc.Replace("@" + propAttr.AttrName, Convert.ToDateTime(val).ToString(propAttr.Format));
                }
                else
                {
                    sc = sc.Replace("@" + propAttr.AttrName, (null != map ? map(propAttr.AttrName, val).ToString() : val.ToString()));
                }
            }

            return sc;
        }
    }
}
