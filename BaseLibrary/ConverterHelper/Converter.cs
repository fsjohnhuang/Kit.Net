using lpp.StringHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;

namespace lpp.ConverterHelper
{
    public sealed class Converter
    {
        public delegate object ConverterHandler(object raw);

        /// <summary>
        /// 将实体对象转换为字典
        /// </summary>
        /// <typeparam name="K">字典的键类型</typeparam>
        /// <typeparam name="V">字典的值类型</typeparam>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="models">实体集合</param>
        /// <param name="keyPropertyName">作为字典键的实体属性名称</param>
        /// <param name="valPropertyName">作为字典值的实体属性名称</param>
        /// <returns></returns>
        public static Dictionary<K, V> ConvertObjectToDic<K, V, T>(List<T> models, string keyPropertyName, string valPropertyName)
        {
            Dictionary<K, V> dic = new Dictionary<K, V>();

            Type type = typeof(T);
            for (int i = 0; i < models.Count; i++)
            {
                T model = models[i];
                PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                K key = default(K);
                V val = default(V);
                bool setKey = false;
                bool setVal = false;
                for (int j = 0; j < propertyInfos.Length; j++)
                {
                    if (propertyInfos[j].Name.Equals(keyPropertyName))
                    {
                        key = (K)propertyInfos[j].GetValue(model, null);
                        setKey = true;
                    }
                    else if (propertyInfos[j].Name.Equals(valPropertyName))
                    {
                        val = (V)propertyInfos[j].GetValue(model, null);
                        setVal = true;
                    }

                    if (setKey && setVal)
                    {
                        break;
                    }
                }

                dic.Add(key, val);
            }

            return dic;
        }

        /// <summary>
        /// 将枚举类型转换为字典
        /// </summary>
        /// <typeparam name="T">枚举的基础数据类型</typeparam>
        /// <param name="eType">枚举类型</param>
        /// <param name="props">枚举项目名称的别名映射集合</param>
        /// <param name="includedProps">包含的枚举项目名称</param>
        /// <returns>字典对象</returns>
        public static Dictionary<string, T> ConvertEnumToDic<T>(Type eType, IDictionary<string, string> props = null, string[] includedProps = null)
        {
            if (props == null)
                props = new Dictionary<string, string>();
            Dictionary<string, T> dic = new Dictionary<string, T>();
            Array vals = Enum.GetValues(eType);
            List<T> sequence = new List<T>();
            List<T> valLst = new List<T>();
            foreach (T val in vals)
            {
                valLst.Add(val);
            }

            foreach (string propKey in props.Keys)
            {
                for (int i = 0; i < valLst.Count; i++)
                {
                    string key = Enum.GetName(eType, valLst[i]);
                    if (string.Equals(key, propKey))
                    {
                        sequence.Add(valLst[i]);
                        valLst.RemoveAt(i);
                        break;
                    }
                }
            }

            List<T> finalLst = new List<T>();
            finalLst.AddRange(sequence);
            finalLst.AddRange(valLst);

            foreach (T val in finalLst)
            {
                string key = Enum.GetName(eType, val);

                if (includedProps != null)
                {
                    bool included = false;
                    foreach (string includedProp in includedProps)
                    {
                        if (included = string.Equals(includedProp, key))
                            break;
                    }
                    if (!included) continue;
                }

                if (props.ContainsKey(key))
                    dic.Add(props[key], val);
                else
                    dic.Add(key, val);
            }

            return dic;
        }

        /// <summary>
        /// 将List转换为DataTable
        /// </summary>
        /// <typeparam name="T">List元素的数据类型</typeparam>
        /// <param name="srcList">List对象</param>
        /// <param name="includedColNames">需转换到dt中的list属性名</param>
        /// <param name="colTitles">需转换到dt中的list属性名对应的显示名，若与属性名相同则设置为null即可</param>
        /// <returns>DataTable对象</returns>
        public static DataTable ConvertList2DT<T>(List<T> srcList
            , string[] includedColNames = null, string[] colTitles = null, IDictionary<string, ConverterHandler> converters = null, string tblName = null)
        {
            Type type = typeof(T);

            // 构造dt结构
            string _tblName = (tblName == null || Str.IsNullOrWhiteSpace(tblName) ? type.Name : tblName);
            PropertyInfo[] propInfos;
            if (includedColNames != null)
            {
                List<PropertyInfo> tmpPropInfos = new List<PropertyInfo>();
                foreach (string colName in includedColNames)
                {
                    tmpPropInfos.Add(type.GetProperty(colName, BindingFlags.Public | BindingFlags.Instance));
                }
                propInfos = tmpPropInfos.ToArray();
            }
            else
            {
                propInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            }
            DataTable dt = new DataTable();
            dt.TableName = _tblName;
            for (int i = 0, len = propInfos.Length; i < len; ++i)
            {
                if (colTitles != null && colTitles.Length > i && colTitles[i] != null)
                    dt.Columns.Add(colTitles[i]);
                else
                    dt.Columns.Add(propInfos[i].Name);
            }

            // 构造数据
            object propVal;
            for (int i = 0, len = srcList.Count; i < len; ++i)
            {
                List<object> rowData = new List<object>();
                for (int j = 0, jLen = propInfos.Length; j < jLen; ++j)
                {
                    propVal = propInfos[j].GetValue(srcList[i], null);
                    // 调用转换函数
                    if (converters != null && converters.ContainsKey(propInfos[j].Name))
                        propVal = converters[propInfos[j].Name](propVal);

                    rowData.Add(propVal);
                }
                dt.Rows.Add(rowData.ToArray());
            }

            return dt;
        }

        /// <summary>
        /// 将DataTable内容转为某类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="propColNameDic"></param>
        /// <returns></returns>
        public static List<T> ConvertDT2List<T>(DataTable dt
            , IDictionary<string, One2OtherDic> propColNameDic = null) where T : new()
        {
            Type type = typeof(T);
            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<T> resultLst = new List<T>();
            if (props == null || props.Length == 0) return resultLst;

            if (propColNameDic == null)
            {
                propColNameDic = new Dictionary<string, One2OtherDic>();
            }
            PropertyInfo prop;
            One2OtherDic one2OtherItem;
            string dtColName;
            int colIndex;
            One2OtherDic.ConverterHandler converter;
            T resultItem;
            foreach (DataRow dr in dt.Rows)
            {
                resultItem = new T();
                for (int i = 0, len = props.Length; i < len; i++)
                {
                    prop = props[i];
                    if (propColNameDic.ContainsKey(prop.Name))
                    {
                        one2OtherItem = propColNameDic[prop.Name];
                        dtColName = (Str.IsNullOrWhiteSpace(one2OtherItem.SrcPropName) ? prop.Name : one2OtherItem.SrcPropName);
                        converter = one2OtherItem.Convert;
                    }
                    else
                    {
                        dtColName = prop.Name;
                        converter = null;
                    }

                    colIndex = dt.Columns.IndexOf(dtColName);
                    if (colIndex == -1 && null == converter) continue;

                    object value = null;
                    if (colIndex != -1)
                    {
                        value = dr[colIndex];
                    }
                    if (converter != null)
                    {
                        value = converter(value);
                    }
                    prop.SetValue(resultItem, ChangeType(value, (prop.PropertyType)), null);
                }

                resultLst.Add(resultItem);
            }

            return resultLst;
        }

        /// <summary>
        /// 将类型S的数据映射到类型D对象中
        /// </summary>
        /// <typeparam name="S">源类型</typeparam>
        /// <typeparam name="D">目的类型</typeparam>
        /// <param name="srcList">源类型List</param>
        /// <param name="propMap">源、目标类型属性映射</param>
        /// <returns>目的类型List</returns>
        public static List<D> ConvertOne2Other<S, D>(List<S> srcList, List<One2OtherMap> propMap = null)
            where S : class
            where D : class
        {
            List<D> dList = new List<D>();
            for (int i = 0, iLen = srcList.Count; i < iLen; ++i)
            {
                dList.Add(ConvertOne2Other<S, D>(srcList[i], propMap));
            }

            return dList;
        }

        /// <summary>
        /// 将类型S的数据映射到类型D对象中
        /// </summary>
        /// <typeparam name="S">源类型</typeparam>
        /// <typeparam name="D">目的类型</typeparam>
        /// <param name="srcItem">源类型对象</param>
        /// <param name="propMap">源、目标类型属性映射</param>
        /// <returns>目的类型对象</returns>
        public static D ConvertOne2Other<S, D>(S srcItem, List<One2OtherMap> propMap = null)
            where S : class
            where D : class
        {
            Type typeofS = typeof(S);
            Type typeofD = typeof(D);
            PropertyInfo[] propsofD = typeofD.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            D dItem = Activator.CreateInstance(typeofD) as D;

            for (int j = 0, jLen = propsofD.Length; j < jLen; ++j)
            {
                string curPropName = propsofD[j].Name;
                string srcPropName = curPropName;
                if (propMap != null)
                {
                    One2OtherMap map = propMap.Find((one) =>
                    {
                        return string.Equals(one.DestPropName, curPropName);
                    });
                    if (map != null)
                        srcPropName = map.SrcPropName;
                }

                PropertyInfo srcProp = typeofS.GetProperty(srcPropName, BindingFlags.Public | BindingFlags.Instance);
                if (srcProp != null)
                {
                    propsofD[j].SetValue(dItem, srcProp.GetValue(srcItem, null), null);
                }
            }

            return dItem;
        }

        /// <summary>
        /// 将类型S的数据映射到类型D对象中
        /// </summary>
        /// <typeparam name="S">源类型</typeparam>
        /// <typeparam name="D">目的类型</typeparam>
        /// <param name="srcItem">源类型对象</param>
        /// <param name="destItem">目的类型对象</param>
        /// <param name="propMap">源、目标类型属性映射</param>
        public static void ConvertOne2Other<S, D>(S srcItem, D destItem, List<One2OtherMap> propMap = null)
            where S : class
            where D : class
        {
            Type typeofS = typeof(S);
            Type typeofD = typeof(D);
            PropertyInfo[] propsofD = typeofD.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            for (int j = 0, jLen = propsofD.Length; j < jLen; ++j)
            {
                string curPropName = propsofD[j].Name;
                string srcPropName = curPropName;
                if (propMap != null)
                {
                    One2OtherMap map = propMap.Find((one) =>
                    {
                        return string.Equals(one.DestPropName, curPropName);
                    });
                    if (map != null)
                        srcPropName = map.SrcPropName;
                }

                PropertyInfo srcProp = typeofS.GetProperty(srcPropName, BindingFlags.Public | BindingFlags.Instance);
                if (srcProp != null)
                {
                    propsofD[j].SetValue(destItem, srcProp.GetValue(srcItem, null), null);
                }
            }
        }

        /// <summary>
        /// 类型转换，可处理object到int?等可空对象的转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value != null)
                {
                    NullableConverter nullableConverter = new NullableConverter(conversionType);
                    conversionType = nullableConverter.UnderlyingType;
                }
                else
                {
                    return null;
                }
            }

            return Convert.ChangeType(value, conversionType);
        }
    }
}
