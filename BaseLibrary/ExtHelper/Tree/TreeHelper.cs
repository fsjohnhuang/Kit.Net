using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using lpp.DBHelper;
using lpp.DBHelper.MSSQL;
using lpp.DBAttr;
using lpp.LogHelper;

namespace lpp.ExtHelper.Tree
{
    public static class TreeHelper
    {
        /// <summary>
        /// 将树节点转换为json
        /// </summary>
        /// <param name="root">树节点</param>
        /// <returns>json</returns>
        public static string ToJson(TreeNodeInfo root)
        {
            string treeJson = string.Empty;

            try
            {
                StringBuilder treeJsonSB = new StringBuilder();
                StringWriter sw = new StringWriter(treeJsonSB);
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                JsonSerializer serializer = JsonSerializer.Create(settings);
                serializer.Serialize(sw, root);
                sw.Close();

                // 加壳
                string shellPattern = "{1}children:[{0}]{2}";
                treeJson = string.Format(shellPattern, treeJsonSB.ToString(), "{", "}");
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }

            return treeJson;
        }

        /// <summary>
        /// 将一维树节点列表转换为json
        /// </summary>
        /// <param name="treeNodes">树节点列表</param>
        /// <returns>json</returns>
        public static string ToJson(List<TreeNodeInfo> treeNodes)
        {
            string treeJson = string.Empty;

            try
            {
                StringBuilder treeJsonSB = new StringBuilder();
                StringWriter sw = new StringWriter(treeJsonSB);
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                JsonSerializer serializer = JsonSerializer.Create(settings);
                serializer.Serialize(sw, treeNodes);
                sw.Close();

                // 加壳
                string shellPattern = "{1}children:{0}{2}";
                treeJson = string.Format(shellPattern, treeJsonSB.ToString(), "{", "}");
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }

            return treeJson;
        }

        /// <summary>
        /// 将列表树节点列表转换为json
        /// </summary>
        /// <param name="treeNodes">树节点列表</param>
        /// <returns>json</returns>
        public static string ToJson<N>(List<N> treeNodes)
        {
            string jsonResult = GetJsonContent<N>(treeNodes);
            jsonResult = string.Format("{0}\"children\":[{2}]{1}", "{", "}", jsonResult);

            return jsonResult;
        }

        /// <summary>
        /// 获取Json各式字符串
        /// </summary>
        /// <typeparam name="N">类型</typeparam>
        /// <param name="treeNodes">节点列表</param>
        /// <returns></returns>
        private static string GetJsonContent<N>(List<N> treeNodes)
        {
            StringBuilder treeJsonSB = new StringBuilder();

            try
            {

                PropertyInfo[] propertyInfos = typeof(N).GetProperties();
                object[] jsonProperties = null;
                for (int j = 0; j < treeNodes.Count; ++j)
                {
                    treeJsonSB.Append("{");
                    for (int i = 0; i < propertyInfos.Length; ++i)
                    {
                        jsonProperties = propertyInfos[i].GetCustomAttributes(typeof(JsonPropertyAttribute), false);
                        if (jsonProperties == null || jsonProperties.Length == 0) continue;

                        string jsonName = Convert.ToString(jsonProperties[0].GetType().GetProperty("PropertyName").GetValue(jsonProperties[0], null));
                        if (jsonName.Equals("children"))
                        {
                            List<N> propertyValue = propertyInfos[i].GetValue(treeNodes[j], null) as List<N>;
                            if (propertyValue == null || propertyValue.Count == 0) continue;

                            treeJsonSB.AppendFormat("\"children\":[{0}],", GetJsonContent<N>(propertyValue));
                        }
                        else
                        {
                            object propertyValue = propertyInfos[i].GetValue(treeNodes[j], null);
                            if (propertyValue == null) continue;
                            string jsonValue = string.Empty;
                            if (propertyValue.GetType() == typeof(int) || propertyValue.GetType() == typeof(int?)
                                || propertyValue.GetType() == typeof(int?) || propertyValue.GetType() == typeof(long)
                                || propertyValue.GetType() == typeof(long?) || propertyValue.GetType() == typeof(short)
                                || propertyValue.GetType() == typeof(short?) || propertyValue.GetType() == typeof(byte)
                                || propertyValue.GetType() == typeof(byte?))
                            {
                                jsonValue = propertyValue + "";
                            }
                            else if (propertyValue.GetType() == typeof(bool)
                                || propertyValue.GetType() == typeof(bool?))
                            {
                                jsonValue = (propertyValue + "").ToLower();
                            }
                            else
                            {
                                jsonValue = "\"" + propertyValue + "\"";
                            }

                            treeJsonSB.AppendFormat("\"{0}\":{1},", jsonName, jsonValue);
                        }
                    }
                    treeJsonSB.Remove(treeJsonSB.Length - 1, 1);
                    treeJsonSB.Append("},");
                }
                treeJsonSB.Remove(treeJsonSB.Length - 1, 1);
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }

            return treeJsonSB.ToString();
        }

        /// <summary>
        /// 将指定类列表转换为TreeNodeInfo列表(多层树结构)
        /// </summary>
        /// <typeparam name="T">指定类</typeparam>
        /// <typeparam name="O">指定类的排序字段类型</typeparam>
        /// <param name="sourceList">指定类列表</param>
        /// <param name="idProperty">指定类的主键属性名称</param>
        /// <param name="pIDProperty">指定类的父主键属性名称</param>
        /// <param name="textProperty">指定类的名称属性名称</param>
        /// <param name="orderProperty">指定类的排序属性名称</param>
        /// <param name="rootId">指定类的父主键属性值</param>
        /// <returns>TreeNodeInfo列表</returns>
        public static List<TreeNodeInfo> ToTreeNodeInfoList<T, O>(List<T> sourceList, 
            string idProperty, string pIDProperty, string textProperty, string orderProperty,
            object rootId)
        {
            List<TreeNodeInfo> treeNodeInfos = new List<TreeNodeInfo>();
            if (null == sourceList || sourceList.Count == 0) return treeNodeInfos;

            // 获取第一层节点
            List<T> rootNodes = new List<T>();
            List<T> otherNodes = new List<T>();
            PropertyInfo pIDPropertyInfo = typeof(T).GetProperty(pIDProperty, BindingFlags.Public | BindingFlags.Instance);
            for (int i = sourceList.Count - 1; i >= 0; --i)
            {
                if (rootId == null)
                {
                    if (pIDPropertyInfo.GetValue(sourceList[i], null) == null)
                    {
                        rootNodes.Add(sourceList[i]);
                    }
                    else
                    {
                        otherNodes.Add(sourceList[i]);
                    }
                }
                else if (rootId == string.Empty)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(pIDPropertyInfo.GetValue(sourceList[i], null)).Trim()))
                    {
                        rootNodes.Add(sourceList[i]);
                    }
                    else
                    {
                        otherNodes.Add(sourceList[i]);
                    }
                }
                else
                {
                    if (pIDPropertyInfo.GetValue(sourceList[i], null).Equals(rootId))
                    {
                        rootNodes.Add(sourceList[i]);
                    }
                    else
                    {
                        otherNodes.Add(sourceList[i]);
                    }
                }
            }
            // 排序
            PropertyInfo orderPropertyInfo = typeof(T).GetProperty(orderProperty, BindingFlags.Public | BindingFlags.Instance);
            List<T> orderedSourceList = rootNodes.OrderBy<T, O>(source=>{
                return (O)orderPropertyInfo.GetValue(source, null);
            }).ToList<T>();
            for (int i = 0; i < orderedSourceList.Count; ++i)
            {
                treeNodeInfos.Add(RecursionTree<T, O>(otherNodes, orderedSourceList[i], idProperty, pIDProperty, textProperty, orderProperty));
            }


           return treeNodeInfos;
        }

        /// <summary>
        /// 将指定类列表转换为TreeNodeInfo列表(单层树结构)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="O"></typeparam>
        /// <param name="sourceList"></param>
        /// <param name="idProperty"></param>
        /// <param name="textProperty"></param>
        /// <param name="orderProperty"></param>
        /// <returns></returns>
        public static List<TreeNodeInfo> ToTreeNodeInfoList<T, O>(List<T> sourceList,
            string idProperty, string textProperty, string orderProperty)
        {
            List<TreeNodeInfo> treeNodeInfos = new List<TreeNodeInfo>();
            if (null == sourceList || sourceList.Count == 0) return treeNodeInfos;
            
            Type sourceType = typeof(T);
            PropertyInfo idPropInfo = sourceType.GetProperty(idProperty, BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo textPropInfo = sourceType.GetProperty(textProperty, BindingFlags.Public | BindingFlags.Instance);

            // 排序
            PropertyInfo orderPropertyInfo = typeof(T).GetProperty(orderProperty, BindingFlags.Public | BindingFlags.Instance);
            List<T> orderedSourceList = sourceList.OrderBy<T, O>(source =>
            {
                return (O)orderPropertyInfo.GetValue(source, null);
            }).ToList<T>();
            for (int i = 0; i < orderedSourceList.Count; ++i)
            {
                treeNodeInfos.Add(new TreeNodeInfo(Convert.ToString(idPropInfo.GetValue(orderedSourceList[i], null)), Convert.ToString(textPropInfo.GetValue(orderedSourceList[i], null)), true));
            }

            return treeNodeInfos;
        }

        private static TreeNodeInfo RecursionTree<T, O>(List<T> otherNodes, T source, string idProperty, string pIDProperty, string textProperty, string orderProperty)
        {
            TreeNodeInfo node = new TreeNodeInfo();

            PropertyInfo idPropertyInfo = typeof(T).GetProperty(idProperty, BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo pIDPropertyInfo = typeof(T).GetProperty(pIDProperty, BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo textPropertyInfo = typeof(T).GetProperty(textProperty, BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo orderPropertyInfo = typeof(T).GetProperty(orderProperty, BindingFlags.Instance | BindingFlags.Public);

            node.ID = Convert.ToString(idPropertyInfo.GetValue(source, null));
            node.Text = Convert.ToString(textPropertyInfo.GetValue(source, null));

            if (null == otherNodes || otherNodes.Count == 0)
            {
                node.Leaf = true;
                return node;
            }
        
            // 筛选出source的子节点并排序
            List<T> childNodes = otherNodes.FindAll(n => {
                return pIDPropertyInfo.GetValue(n, null)
                    .Equals(idPropertyInfo.GetValue(source, null));
            });
            if (childNodes.Count == 0)
            {
                node.Leaf = true;
                return node;
            }
            childNodes = childNodes.OrderBy<T, O>(n =>{
                return (O)orderPropertyInfo.GetValue(n, null);
            }).ToList<T>();
            node.Children = new List<TreeNodeInfo>();
            for (int i = 0; i < childNodes.Count; ++i)
            {
                node.Children.Add(RecursionTree<T, O>(otherNodes, childNodes[i], idProperty, pIDProperty, textProperty, orderProperty));
            }

            return node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="R">目标类</typeparam>
        /// <typeparam name="T">指定类</typeparam>
        /// <typeparam name="O">指定类的排序字段类型</typeparam>
        /// <param name="sourceList">指定类列表</param>
        /// <param name="idProperty">指定类的主键属性名称</param>
        /// <param name="pIDProperty">指定类的父主键属性名称</param>
        /// <param name="textProperty">指定类的名称属性名称</param>
        /// <param name="orderProperty">指定类的排序属性名称</param>
        /// <param name="rootId">指定类的父主键属性值</param>
        /// <returns></returns>
        public static List<R> ToCustomTreeNodeInfoList<R, T, O>(List<T> sourceList, string idProperty, string pIDProperty, string orderProperty,
            object rootId)
        {
            List<R> treeNodeInfos = new List<R>();
            if (null == sourceList || sourceList.Count == 0) return treeNodeInfos;

            // 获取第一层节点
            List<T> rootNodes = new List<T>();
            List<T> otherNodes = new List<T>();
            PropertyInfo pIDPropertyInfo = typeof(T).GetProperty(pIDProperty, BindingFlags.Public | BindingFlags.Instance);
            for (int i = sourceList.Count - 1; i >= 0; --i)
            {
                if (rootId == null)
                {
                    if (pIDPropertyInfo.GetValue(sourceList[i], null) == null)
                    {
                        rootNodes.Add(sourceList[i]);
                    }
                    else
                    {
                        otherNodes.Add(sourceList[i]);
                    }
                }
                else if (rootId == string.Empty)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(pIDPropertyInfo.GetValue(sourceList[i], null)).Trim()))
                    {
                        rootNodes.Add(sourceList[i]);
                    }
                    else
                    {
                        otherNodes.Add(sourceList[i]);
                    }
                }
                else {
                    if (pIDPropertyInfo.GetValue(sourceList[i], null).Equals(rootId))
                    {
                        rootNodes.Add(sourceList[i]);
                    }
                    else
                    {
                        otherNodes.Add(sourceList[i]);
                    }
                }
            }
            // 排序
            PropertyInfo orderPropertyInfo = typeof(T).GetProperty(orderProperty, BindingFlags.Public | BindingFlags.Instance);
            List<T> orderedSourceList = rootNodes.OrderBy<T, O>(source =>
            {
                return (O)orderPropertyInfo.GetValue(source, null);
            }).ToList<T>();
            for (int i = 0; i < orderedSourceList.Count; ++i)
            {
                treeNodeInfos.Add(RecursionTree<R, T, O>(otherNodes, orderedSourceList[i], idProperty, pIDProperty, orderProperty));
            }


            return treeNodeInfos;
        }

        private static R RecursionTree<R, T, O>(List<T> otherNodes, T source, string idProperty, string pIDProperty, string orderProperty)
        {
            R node = (R)typeof(R).GetConstructor(new List<Type>().ToArray()).Invoke(null);

            PropertyInfo idPropertyInfo = typeof(T).GetProperty(idProperty, BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo pIDPropertyInfo = typeof(T).GetProperty(pIDProperty, BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo orderPropertyInfo = typeof(T).GetProperty(orderProperty, BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo leafProperty = typeof(R).GetProperty("Leaf", BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo childrenProperty = typeof(R).GetProperty("Children", BindingFlags.Public | BindingFlags.Instance);

            PropertyInfo[] tPropertyInfos = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo[] rPropertyInfos = typeof(R).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < tPropertyInfos.Length; ++i)
            {
                for (int j = 0; j < rPropertyInfos.Length; ++j)
                {
                    if (tPropertyInfos[i].Name.Equals(rPropertyInfos[j].Name))
                    {
                        rPropertyInfos[j].SetValue(node, tPropertyInfos[i].GetValue(source, null), null);
                        break;
                    }
                }
            }

            if (null == otherNodes || otherNodes.Count == 0)
            {
                leafProperty.SetValue(node, true, null);
                return node;
            }

            // 筛选出source的子节点并排序
            List<T> childNodes = otherNodes.FindAll(n =>
            {
                return pIDPropertyInfo.GetValue(n, null)
                    .Equals(idPropertyInfo.GetValue(source, null));
            });
            if (childNodes.Count == 0)
            {
                leafProperty.SetValue(node, true, null);
                return node;
            }
            childNodes = childNodes.OrderBy<T, O>(n =>
            {
                return (O)orderPropertyInfo.GetValue(n, null);
            }).ToList<T>();
            List<R> children = new List<R>();
            for (int i = 0; i < childNodes.Count; ++i)
            {
                children.Add(RecursionTree<R, T, O>(otherNodes, childNodes[i], idProperty, pIDProperty, orderProperty));
            }
            childrenProperty.SetValue(node, children, null);

            return node;
        }

        /// <summary>
        /// 改变树节点序号
        /// </summary>
        /// <typeparam name="T">树节点类</typeparam>
        /// <param name="dbInstance">数据库操作对象</param>
        /// <param name="dbHelper">数据库操作帮助对象</param>
        /// <param name="idProperty">主键属性名</param>
        /// <param name="pIdProperty">父节点主键属性名</param>
        /// <param name="orderProperty">序号属性名</param>
        /// <param name="id">主键</param>
        /// <param name="sourcePID">源父节点主键</param>
        /// <param name="sourceIndex">源序号</param>
        /// <param name="newPID">新父节点主键</param>
        /// <param name="newIndex">新序号</param>
        /// <returns></returns>
        public static bool ChangeOrder<T>(IDBInstance dbInstance, MSSQLHelper dbHelper, string idProperty, string pIdProperty, string orderProperty,
            long? id, long? sourcePID, long? sourceIndex, long? newPID, long? newIndex) where T : class
        {
            PropertyInfo orderProp = typeof(T).GetProperty(orderProperty);

            if (newPID == null || newPID == sourcePID)
            {
                // 在同一父节点下改变序号
                long dVal = newIndex.Value - sourceIndex.Value;
                List<T> tObjs = null;
                if (dVal > 0)
                {
                    // 新序号在旧序号之后
                    tObjs = dbHelper.Query<T>(string.Format(" [{0}] > {1} and [{0}] <= {2} and [{3}] " + (sourcePID == 0 ? "IS NULL" : "=" + sourcePID) + " ", 
                        orderProperty, sourceIndex, newIndex, pIdProperty), null, null);
                    for (int i = 0, len = tObjs.Count; i < len; ++i)
                    {
                        orderProp.SetValue(tObjs[i], CalcOrder(orderProp.PropertyType, orderProp.GetValue(tObjs[i], null), -1), null);
                    }
                }
                else if (dVal < 0)
                {
                    // 新序号在旧序号之前
                    tObjs = dbHelper.Query<T>(string.Format(" [{0}] >= {1} and [{0}] < {2} and [{3}] " + (sourcePID == 0 ? "IS NULL" : "=" + sourcePID) + " ",
                        orderProperty, newIndex, sourceIndex, pIdProperty), null, null);
                    for (int i = 0, len = tObjs.Count; i < len; ++i)
                    {
                        orderProp.SetValue(tObjs[i], CalcOrder(orderProp.PropertyType, orderProp.GetValue(tObjs[i], null), 1), null);
                    }
                }


                T targetTObj = typeof(T).GetConstructor(new List<Type>().ToArray()).Invoke(new List<object>().ToArray()) as T;
                targetTObj.GetType().GetProperty(idProperty).SetValue(targetTObj, id, null);
                orderProp.SetValue(targetTObj, newIndex, null);
                tObjs.Add(targetTObj);
                tObjs.ForEach(tInfo =>
                {
                    dbHelper.Update<T>(tInfo);
                });
                return true;
            }
            else
            {
                // 在不同父节点间下改变序号
                List<T> sourceParentChildInfos = dbHelper.Query<T>(string.Format(" [{0}] > {1} and [{2}] " + (sourcePID == 0 ? "IS NULL" : "=" + sourcePID) + " ",
                    orderProperty, sourceIndex, pIdProperty), null, null);
                List<T> targetParentChildInfos = dbHelper.Query<T>(string.Format(" [{0}] >= {1} and [{2}] " + (newPID == 0 ? "IS NULL" : "=" + newPID) + " ",
                    orderProperty, newIndex, pIdProperty), null, null);
                for (int i = 0, len = sourceParentChildInfos.Count; i < len; ++i)
                {
                    orderProp.SetValue(sourceParentChildInfos[i], CalcOrder(orderProp.PropertyType, orderProp.GetValue(sourceParentChildInfos[i], null), -1), null);
                }
                for (int i = 0, len = targetParentChildInfos.Count; i < len; ++i)
                {
                    orderProp.SetValue(targetParentChildInfos[i], CalcOrder(orderProp.PropertyType, orderProp.GetValue(targetParentChildInfos[i], null), 1), null);
                }

                List<T> totalTInfos = new List<T>();
                totalTInfos.AddRange(sourceParentChildInfos);
                totalTInfos.AddRange(targetParentChildInfos);
                totalTInfos.ForEach(tInfo =>
                {
                    dbHelper.Update<T>(tInfo);
                });

                TblAttr tblAttr = typeof(T).GetCustomAttributes(typeof(TblAttr), false)[0] as TblAttr;
                int count = dbInstance.ExecNonQuery(string.Format("UPDATE {0} SET [{1}]=" + (newPID == 0 ? "NULL" : newPID.Value.ToString()) + ",[{2}]=" + newIndex + " WHERE [{3}]=" + id.Value,
                    tblAttr.Name, pIdProperty, orderProperty, idProperty), null);
                return count > 0;
            }
        }

        private static object CalcOrder(Type propType, object propVal, int dVal){
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
