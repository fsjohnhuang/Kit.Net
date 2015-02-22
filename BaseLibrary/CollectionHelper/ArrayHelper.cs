using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace lpp.CollectionHelper
{
    public sealed class ArrayHelper
    {
        public delegate int Comparison<T>(T x, T y);

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
                if (i != minIndex)
                {
                    tmp = array[minIndex];
                    array[minIndex] = array[i];
                    array[i] = tmp;
                    tmp = null;
                }
            }
        }

        /// <summary>
        /// 连接多个数组并返回新数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">二维数组</param>
        /// <returns></returns>
        public static T[] Concat<T>(params T[][] array)
        {
            List<T> goalLst = new List<T>();

            for (int i = 0, iLen = array.Length; i < iLen; i++)
            {
                T[] innerArray = array[i];
                for (int j = 0, jLen = innerArray.Length; j < jLen; j++)
                {
                    goalLst.Add(innerArray[j]);
                }
            }

            return goalLst.ToArray();
        }

        /// <summary>
        /// 将数组项目的某属性提取到新数组中，并返回该数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static T[] Extract2Array<T, O>(O[] objs, string propName)
        {
            List<T> targetLst = new List<T>();
            Type oType = typeof(O);
            MethodInfo methodInfo = oType.GetMethod(propName, BindingFlags.Instance | BindingFlags.Public);
            if (null == methodInfo) throw new Exception("Argument propName of Extract2Array is invalid!");

            for (int i = 0, len = objs.Length; i < len; i++)
            {
                targetLst.Add((T)methodInfo.Invoke(objs[i], null));
            }

            return targetLst.ToArray();
        }

        /// <summary>
        /// 将数组元素用分隔符串联成字符串
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="array">数组对象</param>
        /// <param name="seperator">分隔符</param>
        /// <param name="hasTails">false：最后一串字符不是分隔符；true最后一串字符是分隔符</param>
        /// <returns></returns>
        public static string Join<T>(T[] array, string seperator = ",", bool hasTails = false)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0, len = array.Length; i < len; i++)
            {
                result.Append(array[i].ToString());
                if (i < len - 1 || hasTails)
                {
                    result.Append(seperator);
                }
            }

            return result.ToString();
        }
    }
}
