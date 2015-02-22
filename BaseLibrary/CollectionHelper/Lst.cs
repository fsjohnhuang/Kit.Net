using System;
using System.Collections.Generic;
using System.Text;
using lpp.StringHelper;
using System.Reflection;

namespace lpp.CollectionHelper
{
    public sealed class Lst
    {
        public delegate bool IsFetch(object obj);
        public delegate object Deal(object obj);

        /// <summary>
        /// 检查列表中是否含关键字数组的任意一个或多个关键字
        /// </summary>
        /// <typeparam name="T">列表元素类型</typeparam>
        /// <param name="lst">列表</param>
        /// <param name="propName">属性名</param>
        /// <param name="keywords">关键字数组，多个关键字时为OR关系</param>
        /// <param name="matchType">匹配类型</param>
        /// <returns>首个匹配项在列表中的索引</returns>
        public static int IndexOf<T>(List<T> lst, string propName, string[] keywords, MatchType matchType = MatchType.ALL) where T : class
        {
            const int NOT_FOUND = -1;
            if (lst == null || lst.Count == 0
                || string.IsNullOrEmpty(propName)
                || keywords == null || keywords.Length == 0) return NOT_FOUND;

            Type lstType = typeof(T);
            PropertyInfo prop = lstType.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
            if (prop == null) return NOT_FOUND;

            int index = NOT_FOUND;
            string val = string.Empty;
            for (int i = 0, len = lst.Count; i < len; i++)
            {
                val = prop.GetValue(lst[i], null).ToString();
                if (Str.IsMatch(val, keywords, matchType))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        /// <summary>
        /// 将数据中某属性用分隔符串联成字符串
        /// </summary>
        /// <typeparam name="T">非原生类型和非string类型</typeparam>
        /// <param name="lst"></param>
        /// <param name="propName">属性名</param>
        /// <param name="seperator">分隔符，默认为,</param>
        /// <param name="hasTails">false：最后一串字符不是分隔符；true最后一串字符是分隔符</param>
        /// <returns></returns>
        public static string Join<T>(List<T> lst, string propName, string seperator = ",", bool hasTails = false) where T : class
        {
            StringBuilder joinStr = new StringBuilder(lst.Count);
            Type type = typeof(T);

            if (type != typeof(string))
            {
                PropertyInfo prop = type.GetProperty(propName);
                if (null == prop)
                    return string.Empty;

                for (int i = 0, len = lst.Count; i < len; ++i)
                {
                    object value = prop.GetValue(lst[i], new object[] { });
                    joinStr.Append(value.ToString());

                    if (i < len - 1 || hasTails)
                        joinStr.Append(seperator);
                }
            }

            return joinStr.ToString();
        }

        /// <summary>
        /// 将数据中某属性用分隔符串联成字符串
        /// </summary>
        /// <typeparam name="T">原生类型和string类型</typeparam>
        /// <param name="lst"></param>
        /// <param name="seperator">分隔符，默认为,</param>
        /// <param name="hasTails">false：最后一串字符不是分隔符；true最后一串字符是分隔符</param>
        /// <returns></returns>
        public static string Join<T>(List<T> lst, bool hasTails = false, string seperator = ",")
        {
            StringBuilder joinStr = new StringBuilder(lst.Count);
            Type type = typeof(T);

            // 对于原生类型和string类型
            if (type.IsPrimitive || type == typeof(string))
            {
                for (int i = 0, len = lst.Count; i < len; i++)
                {
                    joinStr.Append(lst[i].ToString());
                    if (i < len - 1 || hasTails)
                        joinStr.Append(seperator);
                }
            }

            return joinStr.ToString();
        }

        /// <summary>
        /// 过滤列表无效数据，返回新列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <param name="isFetch"></param>
        /// <returns></returns>
        public static List<T> Grep<T>(List<T> lst, IsFetch isFetch)
        {
            List<T> resultLst = new List<T>();
            if (lst == null || isFetch == null) return resultLst;

            foreach (T item in lst)
            {
                if (isFetch(item))
                {
                    resultLst.Add(item);
                }
            }

            return resultLst;
        }

        /// <summary>
        /// 对列表的每个元素所处理，并将处理结果作为返回的新列表的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <param name="deal"></param>
        /// <returns></returns>
        public static List<T> Map<T>(List<T> lst, Deal deal)
        {
            List<T> resultLst = new List<T>();
            if (lst == null || deal == null) return resultLst;

            foreach (T item in lst)
            {
                resultLst.Add((T)deal(item));
            }

            return resultLst;
        }

        public static List<T> Distinct<T>(List<T> lst, string prop)
        {
            List<T> resultLst = new List<T>();

            Type objType = typeof(T);
            PropertyInfo propInfo = objType.GetProperty(prop);
            if (null == propInfo) return lst;
            Type propType = propInfo.PropertyType;

            List<object> propValLst = new List<object>();
            for (int i = 0, len = lst.Count; i < len; i++)
            {
                T tmp = lst[i];
                object tmpVal = propInfo.GetValue(tmp, null);
                if (propValLst.IndexOf(tmpVal) == -1) 
                {
                    propValLst.Add(tmpVal);
                    resultLst.Add(tmp);
                }
            }

            return resultLst;
        }
    }
}
