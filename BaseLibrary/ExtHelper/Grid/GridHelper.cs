using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Newtonsoft.Json;
using lpp.DBHelper.MSSQL;
using lpp.DBHelper;
using lpp.DBAttr;

namespace lpp.ExtHelper.Grid
{
    public static class GridHelper
    {
        /// <summary>
        /// 将实体数据转换为Grid的Json格式字符串
        /// 注意： T的实体的属性名会直接作为Json格式字符串中数据的键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="total"></param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public static string SerializeGridDataToJson<T>(int total, List<T> dataList)
        {
            string dataJson = string.Empty;
            string pattern = "{0}\"success\":true,\"total\":{2},\"items\":[{3}]{1}";
            StringBuilder dataJsonSB = new StringBuilder(dataList.Count * 5);
            for (int j = 0; j < dataList.Count; j++)
            {
                T data = dataList[j];
                dataJsonSB.Append("{");
                PropertyInfo[] properties = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyInfo property = properties[i];
                    if (property.GetValue(data, null) == null) continue;

                    bool isNum = false;
                    if (property.PropertyType == typeof(int)
                        || property.PropertyType == typeof(short)
                        || property.PropertyType == typeof(long)
                        || property.PropertyType == typeof(byte)
                        || property.PropertyType == typeof(int?)
                        || property.PropertyType == typeof(short?)
                        || property.PropertyType == typeof(long?)
                        || property.PropertyType == typeof(byte?))
                    {
                        isNum = true;
                    }
                    bool isBoolean = property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?);
                    object[] jsonProperties = property.GetCustomAttributes(typeof(JsonPropertyAttribute), false);
                    dataJsonSB.AppendFormat("\"{0}\":{1}"
                        , (jsonProperties != null && jsonProperties.Length >= 1 ? jsonProperties[0].GetType().GetProperty("PropertyName").GetValue(jsonProperties[0], null) : property.Name)
                        , (isNum ? property.GetValue(data, null) : (isBoolean ? property.GetValue(data, null).ToString().ToLower() : string.Format("\"{0}\"", property.GetValue(data, null)))));
                     dataJsonSB.Append(",");
                }
                if (properties != null && properties.Length > 0)
                {
                    dataJsonSB.Remove(dataJsonSB.Length - 1, 1);
                }
                dataJsonSB.Append("},");
            }
            if (dataJsonSB.Length > 0)
            {
                dataJsonSB.Remove(dataJsonSB.Length - 1, 1);
            }

            dataJson = string.Format(pattern, "{", "}", total, dataJsonSB.ToString());
            return dataJson;
        }

        /// <summary>
        /// 获取内置SummaryType
        /// </summary>
        /// <param name="summaryType"></param>
        /// <returns></returns>
        public static string GetSummaryType(EnumCls.SummaryType summaryType)
        {
            return summaryType.ToString();
        }

        /// <summary>
        /// 获取自定义SummaryType 函数
        /// </summary>
        /// <param name="functionBody"></param>
        /// <returns></returns>
        /// <remarks>
        ///     函数体内通过records对象来获取所有记录数据
        /// </remarks>
        public static string GetSummaryType(string functionBody)
        {
            return string.Format("function(records){0}{2}{1}", "{", "}", functionBody);
        }

        /// <summary>
        /// 获取自定义SummaryRenderer函数
        /// </summary>
        /// <param name="functionBody"></param>
        /// <returns></returns>
        /// <remarks>
        ///     函数体内通过value来获取已计算的值
        ///     通过summaryData来获取所有记录数据
        ///     通过field获取当前列名
        /// </remarks>
        public static string GetSummaryRenderer(string functionBody)
        {
            return string.Format("function(value, summaryData, field){0}{2}{1}", "{", "}", functionBody);
        }

        public static bool ChangeOrder<T>(MSSQLHelper dbHelper, string idPropName, long? id, string orderPropName, int? srcOrder, int? destOrder, string whereClause) 
            where T : class
        {
            PropertyInfo orderProp = typeof(T).GetProperty(orderPropName);
            string orderDbName = string.Empty;
            object[] colAttrs = orderProp.GetCustomAttributes(typeof(ColumnAttr), false);
            if (colAttrs != null && colAttrs.Length >= 1)
            {
                ColumnAttr colAttr = colAttrs[0] as ColumnAttr;
                orderDbName = string.Format("[{0}]",
                    typeof(ColumnAttr).GetProperty("Name").GetValue(colAttr, null));
            }
            else
            {
                throw new Exception("序列字段没有通过ColumnAttr对应到数据表的字段");
            }

            List<T> updatingItems = null;
            int i = 0;

            // 删除节点时，更新其余节点的序号
            if (!destOrder.HasValue)
            {
                updatingItems = dbHelper.Query<T>(string.Format(" {0} > {1} {2} ",
                    orderDbName,
                    srcOrder,
                    (whereClause != null && whereClause.Length >= 1 ? " AND " + whereClause : "")),
                    orderDbName, null);
                i = -1;
                dbHelper.Del<T>("#" + idPropName + "=@" + idPropName, new ParamInfo(idPropName, id));
            }
            else
            {
                // 在同一父节点下改变序号
                long dVal = destOrder.Value - srcOrder.Value;
            
                if (dVal > 0)
                {
                    // 新序号在旧序号之后
                    updatingItems = dbHelper.Query<T>(string.Format(" {0} > {1} and {0} <= {2} {3} ",
                        orderDbName, srcOrder, destOrder, (whereClause != null && whereClause.Length >= 1 ? " AND " + whereClause : "")), null, null);
                    i = -1;
                }
                else if (dVal < 0)
                {
                    // 新序号在旧序号之前
                    updatingItems = dbHelper.Query<T>(string.Format(" {0} >= {1} and {0} < {2} {3} ",
                        orderDbName, destOrder, srcOrder, (whereClause != null && whereClause.Length >= 1 ? " AND " + whereClause : "")), null, null);
                    i = 1;
                }
            }
            for (int j = 0, len = updatingItems.Count; j < len; ++j)
            {
                orderProp.SetValue(updatingItems[j], CalcOrder(orderProp.PropertyType, orderProp.GetValue(updatingItems[j], null), i), null);
            }
            if (destOrder.HasValue)
            {
                T targetTObj = typeof(T).GetConstructor(new List<Type>().ToArray()).Invoke(new List<object>().ToArray()) as T;
                targetTObj.GetType().GetProperty(idPropName).SetValue(targetTObj, id, null);
                orderProp.SetValue(targetTObj, destOrder, null);
                updatingItems.Add(targetTObj);
            }

            updatingItems.ForEach(tInfo =>
            {
                dbHelper.Update<T>(tInfo);
            });

            return true;
        }

        private static object CalcOrder(Type propType, object propVal, int dVal)
        {
            object calcedOrder = null;
            if (propType == typeof(int) || propType == typeof(int?))
            {
                calcedOrder = Convert.ToInt32(propVal) + dVal;
            }
            else if (propType == typeof(long) || propType == typeof(long?))
            {
                calcedOrder = Convert.ToInt64(propVal) + dVal;
            }
            else if (propType == typeof(short) || propType == typeof(short?))
            {
                calcedOrder = Convert.ToInt16(propVal) + dVal;
            }

            return calcedOrder;
        }
    }
}
