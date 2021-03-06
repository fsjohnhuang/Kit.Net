﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace lpp.DBHelper
{
    internal static class CollectionHelper
    {
        public delegate int Comparison<T>(T x, T y);

        /// <summary>
        /// 检查列表中是否含关键字数组的任意一个或多个关键字
        /// </summary>
        /// <typeparam name="T">列表元素类型</typeparam>
        /// <param name="lst">列表</param>
        /// <param name="propName">属性名</param>
        /// <param name="keywords">关键字数组，多个关键字时为OR关系</param>
        /// <param name="matchType">匹配类型</param>
        /// <returns>首个匹配项在列表中的索引</returns>
        public static int IndexOf<T>(List<T> lst, string propName, string[] keywords, MatchType matchType = MatchType.ALL)
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
                if (StringHelper.IsMatch(val, keywords, matchType))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        /// <summary>
        /// 数组排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="comparision"></param>
        public static void Sort<T>(T[] array, Comparison<T> comparision) where T : class
        {
            int minIndex;
            T tmp;
            for (int i = 0, len = array.Length - 1; i < len; i++)
            {
                minIndex = i;
                for (int j = i + 1, jLen = array.Length; j < jLen; j++)
                {
                    int result = comparision(array[minIndex], array[j]);
                    if (result > 0)
                        minIndex = j;
                }
                if (i != minIndex){
                    tmp = array[minIndex];
                    array[minIndex] = array[i];
                    array[i] = tmp;
                    tmp = null;
                }
            }
        }

        /// <summary>
        /// 将数据中某属性用分隔符串联成字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <param name="propName">属性名</param>
        /// <param name="seperator">分隔符，默认为,</param>
        /// <returns></returns>
        public static string Join<T>(List<T> lst, string propName, string seperator = ",") where T : class
        {
            StringBuilder joinStr = new StringBuilder(lst.Count);

            Type type = typeof(T);
            PropertyInfo prop = type.GetProperty(propName);
            if (null == prop) return string.Empty;

            for (int i = 0, len = lst.Count; i < len; ++i)
            {
                object value = prop.GetValue(lst[i], new object[] { });
                joinStr.Append(value.ToString());

                if (i < len - 1)
                    joinStr.Append(seperator);
            }

            return joinStr.ToString();
        }
    }
}
