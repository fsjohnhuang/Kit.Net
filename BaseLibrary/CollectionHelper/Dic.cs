using System;
using System.Collections.Generic;
using System.Text;

namespace lpp.CollectionHelper
{
    /// <summary>
    /// 字典帮助类
    /// </summary>
    public sealed class Dic
    {
        /// <summary>
        /// 创建新的字典对象
        /// </summary>
        /// <typeparam name="K">键类型</typeparam>
        /// <typeparam name="V">值类型</typeparam>
        /// <param name="keys">键数组</param>
        /// <param name="values">值数组</param>
        /// <param name="cover">是否覆盖已有的键值，默认是覆盖</param>
        /// <returns></returns>
        public static IDictionary<K, V> CreateDic<K, V>(K[] keys, V[] values, bool cover = true)
        {
            IDictionary<K, V> dic = new Dictionary<K, V>();

            int indexOfValues;
            int lengthOfValues = values.Length;
            for (int i = 0, len = keys.Length; i < len; i++)
            {
                if (!cover && dic.ContainsKey(keys[i])) continue;

                indexOfValues = i;
                if (indexOfValues >= lengthOfValues)
                {
                    indexOfValues = lengthOfValues - 1;
                }
                dic[keys[i]] = values[indexOfValues];
            }

            return dic;
        }

        /// <summary>
        /// 创建新的字典对象
        /// </summary>
        /// <typeparam name="K">键类型</typeparam>
        /// <typeparam name="V">值类型</typeparam>
        /// <param name="keys">键</param>
        /// <param name="values">值</param>
        /// <returns></returns>
        public static IDictionary<K, V> CreateDic<K, V>(K key, V value)
        {
            return CreateDic<K, V>(new K[] { key }, new V[] { value });
        }

        /// <summary>
        /// 融合多个字典成一个新字典对象，相同的键值会被覆盖
        /// </summary>
        /// <typeparam name="K">键类型</typeparam>
        /// <typeparam name="V">值类型</typeparam>
        /// <param name="dics">多个字典</param>
        /// <returns></returns>
        public static IDictionary<K, V> Concat<K, V>(params IDictionary<K, V>[] dics)
        {
            IDictionary<K, V> dic = new Dictionary<K, V>();

            for (int i = 0, len = dics.Length; i < len; i++)
            {
                IDictionary<K, V> curDic = dics[i];
                foreach (K key in curDic.Keys)
                {
                    dic[key] = curDic[key];
                }
            }

            return dic;
        }
    }
}
