using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

using lpp.DBAttr;
using lpp.LogHelper;
using System.Text.RegularExpressions;
using lpp.ConverterHelper;

namespace lpp.DBHelper
{
    public class BaseHelper
    {
        // 开发模式
        private bool m_IsDebug = false;
        public bool IsDebug 
        {
            get { return m_IsDebug; }
            set { m_IsDebug = value; }
        }

        // 缓存Query的sql
        private static Dictionary<string, string> cacheQuerySQL = new Dictionary<string, string>();
        private static Dictionary<string, string> cacheQuerySingleSQL = new Dictionary<string, string>();
        private static Dictionary<string, string> cacheQueryByPagingSQL = new Dictionary<string, string>();
        private static Dictionary<string, string> cacheQueryCountSQL = new Dictionary<string, string>();

        protected delegate string Parse2QuerySQL(SQLQueryClause sqlQueryClause, Dictionary<string, string> _cacheSQL, string _cacheKey); // 生成查询SQL
        protected delegate string Parse2QueryByPagingSQL(SQLQueryByPagingClause sqlQueryByPagingClause, Dictionary<string, string> _cacheSQL, string _cacheKey); // 生成分页查询SQL
        protected delegate string Parse2QueryCountSQL(SQLQueryCountClause sqlQueryCountClause, Dictionary<string, string> _cacheSQL, string _cacheKey); // 生成查询记录总数SQL
        protected delegate string Parse2InsertSQL(SQLInsertClause sqlInsertClause); // 生成插入SQL
        protected delegate string Parse2DelSQL(SQLDelClause sqlDelClause); // 生成删除SQL
        protected delegate string Parse2UpdateSQL(SQLUpdateClause sqlUpdateClause); // 生成更新SQL

        private delegate void Parse2SelectClause(int i, ColumnAttr colAttr); // 生成SELECT子句

        private IDBInstance m_Db;

        // 关键字的引用符，MS用[]或",其他数据库用"
        private char m_QuotingStart = '"';
        private char m_QuotingEnd = '"';
        protected char QuotingStart 
        { 
            set 
            { 
                m_QuotingStart = value;
                switch (value)
                {
                    case '"':
                        m_QuotingEnd = '"';
                        break;
                    case '[':
                        m_QuotingEnd = ']';
                        break;
                }
            } 
        }

        // 别名命名时的关键字
        private string m_AliasSymbol = "AS";
        protected string AliasSymbol { set { m_AliasSymbol = value; } }

        // 表的联接关系类型
        protected delegate string ParseToJoinType(JoinType joinType);
        protected event ParseToJoinType ParseToJoinTypeEvent;

        // 根据DBFn值来对字段添加聚集函数
        protected delegate string WrapFn(DBFn dbFn, string prop);
        protected event WrapFn WrapFnEvent;

        public BaseHelper(IDBInstance db)
        {
            m_Db = db;
        }

        public BaseHelper(IDBInstance db, char quoting) : this(db)
        {
            m_QuotingStart = quoting;
            switch (quoting)
            {
                case '"':
                    m_QuotingEnd = '"';
                    break;
                case '[':
                    m_QuotingEnd = ']';
                    break;
            }
        }

        #region 查询
        /// <summary>
        /// 对单表执行查询操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parse2QuerySql">生成完整SQL语句的函数</param>
        /// <param name="queryParam">查询子句集合</param>
        /// <param name="cacheKey">缓存键</param>
        /// <returns></returns>
        protected List<T> Query<T>(Parse2QuerySQL parse2QuerySql, QueryParam queryParam, string cacheKey) where T : class
        {
            if (queryParam.PropFns == null)
                queryParam.PropFns = new Dictionary<string, DBFn>();
            List<T> recs = new List<T>();

            // 组装Sql
            Dictionary<string, string> tblNames = new Dictionary<string, string>();
            Type modelType = typeof(T);

            // 获取表名
            FilterTbls(tblNames, modelType);

            StringBuilder fromClause = new StringBuilder(); // FROM子句
           
            StringBuilder cols = new StringBuilder(); // 字段名称
            PropertyInfo[] propertyInfos = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (StringHelper.IsNullOrWhiteSpace(cacheKey) || !cacheQuerySQL.ContainsKey(cacheKey))
            {
                RelAttr[] relAttrs = (RelAttr[])modelType.GetCustomAttributes(typeof(RelAttr), true);
                DistinctAttr[] distinctAttrs = (DistinctAttr[])modelType.GetCustomAttributes(typeof(DistinctAttr), true);
                GetFormAndSelectClause(fromClause, cols, propertyInfos, tblNames, queryParam.ExcludedProps, queryParam.PropFns, relAttrs, distinctAttrs);
            }

            SQLQueryClause sqlQueryClause = new SQLQueryClause();
            sqlQueryClause.Select = cols.ToString();
            sqlQueryClause.From = string.Format(" {0} ", fromClause.ToString());
            sqlQueryClause.Where = string.Format(" {0} ", Parse2Sql(queryParam.Where, propertyInfos));
            sqlQueryClause.OrderBy = string.Format(" {0} ", Parse2Sql(queryParam.OrderBy, propertyInfos));
            sqlQueryClause.GroupBy = string.Format(" {0} ", Parse2Sql(queryParam.GroupBy, propertyInfos));
            sqlQueryClause.Having = string.Format(" {0} ", Parse2Sql(queryParam.Having, propertyInfos));

            string sql = parse2QuerySql(sqlQueryClause, cacheQuerySQL, cacheKey);
            if (m_IsDebug)
            {
                Logger.WriteMsg2LogFile(sql);
            }

            IDataReader reader = m_Db.ExecReader(sql, queryParam.ParamInfos, CommandBehavior.CloseConnection);
            if (null != reader)
            {
                try
                {
                    ConvertRO2DO<T>(reader, recs, queryParam.ExcludedProps);
                }
                catch (Exception ex)
                {
                    Logger.WriteEx2LogFile(ex);
                }
                finally
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return recs;
        }

        /// <summary>
        /// 对单表执行查询单条记录操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parse2QuerySql">生成完整SQL语句的函数</param>
        /// <param name="queryParam">查询子句集合</param>
        /// <returns></returns>
        protected T QuerySingle<T>(Parse2QuerySQL parse2QuerySql, QueryParam queryParam, string cacheKey) where T : class
        {
            List<T> lst = Query<T>(parse2QuerySql, queryParam, cacheKey);
            if (lst == null || lst.Count == 0) return null;

            return lst[0];
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parse2QueryByPagingSQL">生成完整SQL语句的函数</param>
        /// <param name="queryByPagingParam">分页查询子句集合</param>
        /// <returns></returns>
        protected List<T> QueryByPaging<T>(Parse2QueryByPagingSQL parse2QueryByPagingSQL, QueryByPagingParam queryByPagingParam, string cacheKey) where T : class
        {
            if (queryByPagingParam.PropFns == null)
                queryByPagingParam.PropFns = new Dictionary<string, DBFn>();
            List<T> recs = new List<T>();

            // 组装Sql
            Dictionary<string, string> tblNames = new Dictionary<string, string>();
            Type modelType = typeof(T);

            // 获取表名
            FilterTbls(tblNames, modelType);

            StringBuilder fromClause = new StringBuilder(); // FROM子句
            string onClause = string.Empty; // 连接时的ON子句
            string joinClause = string.Empty; // Join子句类型
            StringBuilder cols = new StringBuilder(); // 字段名称
            PropertyInfo[] propertyInfos = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (StringHelper.IsNullOrWhiteSpace(cacheKey) || !cacheQueryByPagingSQL.ContainsKey(cacheKey))
            {
                RelAttr[] relAttrs = (RelAttr[])modelType.GetCustomAttributes(typeof(RelAttr), true);
                DistinctAttr[] distinctAttrs = (DistinctAttr[])modelType.GetCustomAttributes(typeof(DistinctAttr), true);
                GetFormAndSelectClause(fromClause, cols, propertyInfos, tblNames, queryByPagingParam.ExcludedProps, queryByPagingParam.PropFns, relAttrs, distinctAttrs);
            }

            SQLQueryByPagingClause sqlQueryByPagingClause = new SQLQueryByPagingClause();
            sqlQueryByPagingClause.Select = cols.ToString();
            sqlQueryByPagingClause.From = string.Format(" {0} ", fromClause.ToString());
            sqlQueryByPagingClause.Where = string.Format(" {0} ", Parse2Sql(queryByPagingParam.Where, propertyInfos));
            sqlQueryByPagingClause.OrderBy = string.Format(" {0} ", Parse2Sql(queryByPagingParam.OrderBy, propertyInfos, true, true));
            sqlQueryByPagingClause.PageSize = queryByPagingParam.PageSize;
            sqlQueryByPagingClause.StartIndex = queryByPagingParam.StartIndex;
            sqlQueryByPagingClause.GroupBy = string.Format(" {0} ", Parse2Sql(queryByPagingParam.GroupBy, propertyInfos));
            sqlQueryByPagingClause.Having = string.Format(" {0} ", Parse2Sql(queryByPagingParam.Having, propertyInfos));

            string sql = parse2QueryByPagingSQL(sqlQueryByPagingClause, cacheQueryByPagingSQL, cacheKey);
            if (m_IsDebug)
            {
                Logger.WriteMsg2LogFile(sql);
            }

            IDataReader reader = m_Db.ExecReader(sql, queryByPagingParam.ParamInfos, CommandBehavior.CloseConnection);
            if (null != reader)
            {
                try
                {
                    ConvertRO2DO<T>(reader, recs, queryByPagingParam.ExcludedProps);
                }
                catch (Exception ex)
                {
                    Logger.WriteEx2LogFile(ex);
                }
                finally
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return recs;
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parse2QueryCountSQL">生成完整SQL语句的函数</param>
        /// <param name="queryCountParam">获取记录总数的查询子句集合</param>
        /// <returns></returns>
        protected int QueryCount<T>(Parse2QueryCountSQL parse2QueryCountSQL, QueryCountParam queryCountParam, string cacheKey) where T : class
        {
            int count = 0;

            // 组装Sql
            Dictionary<string, string> tblNames = new Dictionary<string, string>();
            Type modelType = typeof(T);

            // 获取表名
            FilterTbls(tblNames, modelType);

            // 获取主键列名
            StringBuilder fromClause = new StringBuilder(); // FROM子句
            string onClause = string.Empty; // 连接时的ON子句
            string joinClause = string.Empty; // Join子句类型
            StringBuilder primaryCol = new StringBuilder();
            PropertyInfo[] propertyInfos = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (StringHelper.IsNullOrWhiteSpace(cacheKey) || !cacheQueryCountSQL.ContainsKey(cacheKey))
            {
                RelAttr[] relAttrs = (RelAttr[])modelType.GetCustomAttributes(typeof(RelAttr), true);
                DistinctAttr[] distinctAttrs = (DistinctAttr[])modelType.GetCustomAttributes(typeof(DistinctAttr), true);
                GetFormAndSelectClause(fromClause, primaryCol, propertyInfos, tblNames, null, null, relAttrs, distinctAttrs, true);
            }

            SQLQueryCountClause sqlQueryCouontClause = new SQLQueryCountClause();
            sqlQueryCouontClause.Select = (StringHelper.IsNullOrWhiteSpace(primaryCol) ? "*" : primaryCol.ToString());
            sqlQueryCouontClause.From = fromClause.ToString();
            sqlQueryCouontClause.Where = string.Format(" {0} ", Parse2Sql(queryCountParam.Where, propertyInfos));
            sqlQueryCouontClause.GroupBy = string.Format(" {0} ", Parse2Sql(queryCountParam.GroupBy, propertyInfos));
            sqlQueryCouontClause.Having = string.Format(" {0} ", Parse2Sql(queryCountParam.Having, propertyInfos));

            string sql = parse2QueryCountSQL(sqlQueryCouontClause, cacheQueryCountSQL, cacheKey);
            if (m_IsDebug)
            {
                Logger.WriteMsg2LogFile(sql);
            }

            object countObj = m_Db.ExecScalar(sql, queryCountParam.ParamInfos);
            if (null != countObj)
            {
                count = Convert.ToInt32(countObj);
            }

            return count;
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parse2InsertSql">生成完整SQL语句的函数</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        protected bool Insert<T>(Parse2InsertSQL parse2InsertSql, T entity)
        {
            bool isSuccess = false;
            if (entity == null)
            {
                return isSuccess;
            }

            List<ParamInfo> paramInfos = new List<ParamInfo>(); // 入参实体集合
            string sql = AssembleInsertSql<T>(parse2InsertSql, entity, paramInfos);
            if (m_IsDebug)
            {
                Logger.WriteMsg2LogFile(sql);
            }

            object countObj = m_Db.ExecNonQuery(sql, paramInfos);
            if (null != countObj)
            {
                isSuccess = Convert.ToInt32(countObj) != 0;
            }

            return isSuccess;
        }

        /// <summary>
        /// 新增记录并返回自增长主键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parse2InsertSql">生成完整SQL语句的函数</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        protected object InsertThenReturnID<T>(Parse2InsertSQL parse2InsertSql, T entity)
        {
            object id = null;
            if (entity == null)
            {
                return id;
            }

            List<ParamInfo> paramInfos = new List<ParamInfo>(); // 入参实体集合
            string sql = AssembleInsertSql<T>(parse2InsertSql, entity, paramInfos);
            if (m_IsDebug)
            {
                Logger.WriteMsg2LogFile(sql);
            }

            id = m_Db.ExecScalar(sql, paramInfos);
            return id;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parse2DelSQL">生成完整SQL语句的函数</param>
        /// <param name="where">Where子句</param>
        /// <param name="paramInfos">Where子句参数值</param>
        /// <returns></returns>
        protected bool Del<T>(Parse2DelSQL parse2DelSQL, string where = "", List<ParamInfo> paramInfos = null)
        {
            bool isSuccess = false;

            // 组装Sql
            string tblName = string.Empty; // 表名称
            Type modelType = typeof(T);
            TblAttr tblAttr = (TblAttr)modelType.GetCustomAttributes(typeof(TblAttr), true)[0];
            tblName = tblAttr.Name;

            // 获取where sql子句
            PropertyInfo[] props = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            string whereSql = Parse2Sql(where, props, false);

            SQLDelClause sqlDelClause = new SQLDelClause();
            sqlDelClause.From = tblName;
            sqlDelClause.Where = string.Format(" {0} ", whereSql);

            string sql = parse2DelSQL(sqlDelClause);
            if (m_IsDebug)
            {
                Logger.WriteMsg2LogFile(sql);
            }

            object countObj = m_Db.ExecNonQuery(sql, paramInfos);
            if (null != countObj)
            {
                isSuccess = Convert.ToInt32(countObj) != 0;
            }

            return isSuccess;
        }
        #endregion

        #region 修改
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parse2UpdateSQL">生成完整SQL语句的函数</param>
        /// <param name="entity">实体对象</param>
        /// <param name="where">Where子句</param>
        /// <returns></returns>
        protected bool Update<T>(Parse2UpdateSQL parse2UpdateSQL, T entity, string where = "", List<ParamInfo> _paramInfos = null)
        {
            bool result = false;

            // 组装Sql
            string tblName = string.Empty; // 表名称
            Type modelType = typeof(T);
            TblAttr tblAttr = (TblAttr)modelType.GetCustomAttributes(typeof(TblAttr), true)[0];
            tblName = tblAttr.Name;

            PropertyInfo[] propertyInfos = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            StringBuilder cols = new StringBuilder(); // set子句
            StringBuilder wheres = new StringBuilder(); // where子句
            if (!StringHelper.IsNullOrWhiteSpace(where))
            {
                wheres = new StringBuilder(" " + Parse2Sql(where, propertyInfos, false) + " ");
            }
            List<ParamInfo> paramInfos = new List<ParamInfo>(); // 入参实体集合
            if (_paramInfos != null)
            {
                paramInfos = _paramInfos;
            }
            string realColName = string.Empty;
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                ColumnAttr[] colAttrs = (ColumnAttr[])propertyInfos[i].GetCustomAttributes(typeof(ColumnAttr), true);
                if (colAttrs == null || colAttrs.Length == 0) continue;
                ColumnAttr colAttr = colAttrs[0];
                realColName = (StringHelper.IsNullOrWhiteSpace(colAttr.ColName) ? propertyInfos[i].Name : colAttr.ColName);
                if (colAttr.IsPrimary)
                {
                    if (propertyInfos[i].GetValue(entity, null) != null
                        && StringHelper.IsNullOrWhiteSpace(where))
                    {
                        wheres.AppendFormat(" {3} {0}={1}{2} "
                            , realColName
                            , m_Db.ParamPreffix
                            , propertyInfos[i].Name
                            , (wheres.Length == 0 ? string.Empty : "AND"));
                        paramInfos.Add(new ParamInfo(propertyInfos[i].Name, propertyInfos[i].GetValue(entity, null)));
                    }
                    continue;
                }
                if (propertyInfos[i].GetValue(entity, null) == colAttr.IgnoreValue || object.Equals(propertyInfos[i].GetValue(entity, null), colAttr.IgnoreValue)) continue;

                cols.AppendFormat(" {0}={1}{2},"
                    , (realColName.IndexOf(m_QuotingStart) >= 0 ? realColName : m_QuotingStart + realColName + m_QuotingEnd)
                    , m_Db.ParamPreffix
                    , propertyInfos[i].Name);
                paramInfos.Add(new ParamInfo(propertyInfos[i].Name, propertyInfos[i].GetValue(entity, null)));
            }
            if (cols.Length > 0)
            {
                cols.Remove(cols.Length - 1, 1);
            }

            SQLUpdateClause sqlUpdateClause = new SQLUpdateClause();
            sqlUpdateClause.Tbl = tblName;
            sqlUpdateClause.Set = cols.ToString();
            sqlUpdateClause.Where = wheres.ToString();

            string sql = parse2UpdateSQL(sqlUpdateClause);
            if (m_IsDebug)
            {
                Logger.WriteMsg2LogFile(sql);
            }

            object countOjb = m_Db.ExecNonQuery(sql, paramInfos);
            if (null != countOjb)
            {
                result = Convert.ToInt32(countOjb) != 0;
            }

            return result;
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 过滤父类中与子类使用同一个表别名的表格特性，并获取别名对应命名表别名的SQL语句
        /// </summary>
        /// <param name="filteredTbls">别名对应命名表别名的SQL语句</param>
        /// <param name="modelType">对象类型</param>
        private void FilterTbls(IDictionary<string, string> filteredTbls, Type modelType)
        {
            object[] allTblAttrs = modelType.GetCustomAttributes(typeof(TblAttr), true);
            object[] selfTblAttrs = modelType.GetCustomAttributes(typeof(TblAttr), false);
            TblAttr tblAttr = null;
            for (int i = selfTblAttrs.Length - 1; i >= 0; i--)
            {
                tblAttr = selfTblAttrs[i] as TblAttr;
                filteredTbls.Add(tblAttr.Alias, string.Format("{0} {1} {2}", tblAttr.Name, m_AliasSymbol, tblAttr.Alias));
            }
            for (int i = allTblAttrs.Length - 1; i >= 0; i--)
            {
                tblAttr = allTblAttrs[i] as TblAttr;
                if (filteredTbls.ContainsKey(tblAttr.Alias)) continue;

                filteredTbls.Add(tblAttr.Alias, string.Format("{0} {1} {2}", tblAttr.Name, m_AliasSymbol, tblAttr.Alias));
            }
        }

        /// <summary>
        /// 将带#属性名的字符串转换为数据库字段名
        /// </summary>
        /// <param name="colExp">将带#属性名的字符串</param>
        /// <param name="props">类属性数组</param>
        /// <param name="withTblAlias">字段是否带表别名(仅对以#作前缀的属性名称作处理)</param>
        /// <param name="parse2ColAlias">在withTblAlias为true的情况下，决定是否将字段变换为"表别名_字段名"的形式，如"A_ID"(仅对以#作前缀的属性名称作处理)</param>
        /// <returns>SQL</returns>
        private string Parse2Sql(string colExp, PropertyInfo[] props, bool withTblAlias = true, bool parse2ColAlias = false)
        {
            if (StringHelper.IsNullOrWhiteSpace(colExp)) return string.Empty;

            StringBuilder sql = new StringBuilder(colExp);
            string realColName = string.Empty;
            for (int i = 0, len = props.Length; i < len; i++)
            {
                PropertyInfo prop = props[i];
                Regex reg = new Regex(string.Format(@"#{0}\W", prop.Name));
                if (!reg.IsMatch(colExp)) continue;
                ColumnAttr[] columnAttrs = (ColumnAttr[])props[i].GetCustomAttributes(typeof(ColumnAttr), true);
                if (columnAttrs == null || columnAttrs.Length == 0) continue;

                ColumnAttr columnAttr = columnAttrs[0];
                string colFullName = string.Empty;
                if (withTblAlias && parse2ColAlias)
                {
                    colFullName = string.Format("\"{0}_{1}\"", (StringHelper.IsNullOrWhiteSpace(columnAttr.TblAlias) ? string.Empty : columnAttr.TblAlias), (StringHelper.IsNullOrWhiteSpace(columnAttr.ColName) ? prop.Name : columnAttr.ColName));
                }
                else
                {
                    realColName = string.Format("\"{0}\"", (StringHelper.IsNullOrWhiteSpace(columnAttr.ColName) ? prop.Name : columnAttr.ColName));
                    colFullName = realColName;
                    if (withTblAlias)
                    {
                        colFullName = (StringHelper.IsNullOrWhiteSpace(columnAttr.TblAlias) ? string.Empty : columnAttr.TblAlias + ".") + colFullName;
                    }
                }
                sql.Replace("#" + prop.Name, colFullName);
            }

            return sql.ToString();
        }

        /// <summary>
        /// 获取Select子句和From子句
        /// </summary>
        /// <param name="fromClause"></param>
        /// <param name="cols"></param>
        /// <param name="propertyInfos"></param>
        /// <param name="tblNames"></param>
        /// <param name="excludedProps"></param>
        /// <param name="propFns"></param>
        /// <param name="relAttrs"></param>
        /// <param name="distinctAttrs"></param>
        /// <param name="isQueryCount"></param>
        private void GetFormAndSelectClause(StringBuilder fromClause
            , StringBuilder cols
            , PropertyInfo[] propertyInfos
            , Dictionary<string, string> tblNames
            , List<string> excludedProps
            , Dictionary<string, DBFn> propFns
            , RelAttr[] relAttrs = null
            , DistinctAttr[] distinctAttrs = null
            , bool isQueryCount = false)
        {
            bool existRelAttrs = relAttrs != null && relAttrs.Length != 0;
            string onClause = string.Empty; // 连接时的ON子句
            string joinClause = string.Empty; // Join子句类型
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                ColumnAttr[] colAttrs = (ColumnAttr[])propertyInfos[i].GetCustomAttributes(typeof(ColumnAttr), true);
                if (colAttrs.Length == 0) continue;
                ColumnAttr colAttr = null;
                string realColName = string.Empty;// 当ColumnAttr的ColName为null、空字符串或全空格字符串时，即表示数据表字段名与属性名相同
                for (int j = 0, len = colAttrs.Length; j < len; ++j)
                {
                    colAttr = colAttrs[j];
                    if (colAttr.JoinType != JoinType.NONE)
                    {
                        joinClause = ParseToJoinTypeEvent(colAttr.JoinType);
                    }

                    // 组装FROM子句
                    if (!StringHelper.IsNullOrWhiteSpace(colAttr.RelatedColName) && !existRelAttrs)
                    {
                        realColName = (StringHelper.IsNullOrWhiteSpace(colAttr.ColName) ? propertyInfos[i].Name : colAttr.ColName); 

                        onClause = string.Format("ON ({0}.{1} = {2}.{3})",
                                colAttr.TblAlias, realColName,
                                colAttr.RelatedTblAlias, colAttr.RelatedColName);
                        if (fromClause.Length == 0)
                        {
                            fromClause.AppendFormat("{0} {1} {2} {3}",
                                tblNames[colAttr.TblAlias],
                                joinClause,
                                tblNames[colAttr.RelatedTblAlias],
                                onClause);
                        }
                        else
                        {
                            int relatedTblIndex, tblIndex;
                            string fromClauseStr = fromClause.ToString();
                            if ((relatedTblIndex = fromClauseStr.IndexOf(tblNames[colAttr.RelatedTblAlias])) == -1)
                            {
                                fromClause.AppendFormat(" {0} {1} {2}",
                                    joinClause,
                                    tblNames[colAttr.RelatedTblAlias],
                                    onClause);
                            }
                            else if ((tblIndex = fromClauseStr.IndexOf(tblNames[colAttr.TblAlias])) == -1)
                            {
                                fromClause.AppendFormat(" {0} {1} {2}",
                                    joinClause,
                                    tblNames[colAttr.TblAlias],
                                    onClause);
                            }
                            else
                            {
                                // 因两个表格的信息已存在，所以只需追加ON子句即可
                                int minIndex = AlgorithmHelper.Min(new int[] { relatedTblIndex, tblIndex });
                                int onClauseStartIndex = StringHelper.IndexOf(fromClauseStr, @"\bON \(", minIndex);
                                int onClauseEndIndex = StringHelper.IndexOf(fromClauseStr, @"\)", onClauseStartIndex);

                                fromClause.Insert(onClauseEndIndex
                                    , string.Format(" AND {0}.{1} = {2}.{3}"
                                        , colAttr.TblAlias
                                        , realColName
                                        , colAttr.RelatedTblAlias
                                        , colAttr.RelatedColName));
                            }
                        }
                    }
                }

                if (colAttr == null) continue;

                realColName = (StringHelper.IsNullOrWhiteSpace(colAttr.ColName) ? propertyInfos[i].Name : colAttr.ColName); 
                if (isQueryCount)
                {
                    if (StringHelper.IsNullOrWhiteSpace(cols) && colAttr.IsPrimary)
                    {
                        cols.Append((StringHelper.IsNullOrWhiteSpace(colAttr.TblAlias) ? string.Empty : colAttr.TblAlias + ".") + m_QuotingStart + realColName + m_QuotingEnd);
                        cols.Append(",");
                    }
                }
                else
                {
                    if (null != excludedProps && excludedProps.Exists((excludedName) => { return excludedName.Equals(propertyInfos[i].Name); })) continue; // 排除不包含的Select子句的列

                    string colName = (realColName.IndexOf(m_QuotingStart) >= 0 ? realColName.Replace(m_QuotingStart.ToString(), string.Empty).Replace(m_QuotingEnd.ToString(), string.Empty) : realColName);
                    if (propFns.ContainsKey(propertyInfos[i].Name))
                        colName = WrapFnEvent(propFns[propertyInfos[i].Name], Parse2Sql("#" + propertyInfos[i].Name, propertyInfos)) + " " + m_AliasSymbol + " " + m_QuotingStart + colAttr.TblAlias + "_" + colName + m_QuotingEnd;
                    else if (!StringHelper.IsNullOrWhiteSpace(colAttr.TblAlias))
                        colName = string.Format("{0}.{1} {2} {3}"
                            , colAttr.TblAlias
                            , m_QuotingStart + colName + m_QuotingEnd
                            , m_AliasSymbol
                            , m_QuotingStart + colAttr.TblAlias + "_" + colName + m_QuotingEnd);
                    cols.Append(colName + ",");
                }
            }
            if (cols.Length >= 1)
            {
                cols.Remove(cols.Length - 1, 1);
            }
            if (existRelAttrs)
            {
                // 排序
                CollectionHelper.Sort<RelAttr>(relAttrs, (x, y) => {
                    return  x.Index - y.Index;
                });
                foreach (RelAttr relAttr in relAttrs)
                {
                    joinClause = ParseToJoinTypeEvent(relAttr.JoinType);
                    onClause = string.Format("ON ({0})", relAttr.JoinConstraint);
                    if (fromClause.Length == 0)
                    {
                        fromClause.AppendFormat("{0} {1} {2} {3}",
                            tblNames[relAttr.TblAlias],
                            joinClause,
                            tblNames[relAttr.RelatedTblAlias],
                            onClause);
                    }
                    else
                    {
                        int relatedTblIndex, tblIndex;
                        string fromClauseStr = fromClause.ToString();
                        if ((relatedTblIndex = fromClauseStr.IndexOf(tblNames[relAttr.RelatedTblAlias])) == -1)
                        {
                            fromClause.AppendFormat(" {0} {1} {2}",
                                joinClause,
                                tblNames[relAttr.RelatedTblAlias],
                                onClause);
                        }
                        else if ((tblIndex = fromClauseStr.IndexOf(tblNames[relAttr.TblAlias])) == -1)
                        {
                            fromClause.AppendFormat(" {0} {1} {2}",
                                joinClause,
                                tblNames[relAttr.TblAlias],
                                onClause);
                        }
                        else
                        {
                            // 因两个表格的信息已存在，所以只需追加ON子句即可
                            int minIndex = AlgorithmHelper.Min(new int[] { relatedTblIndex, tblIndex });
                            int onClauseStartIndex = StringHelper.IndexOf(fromClauseStr, @"\bON \(", minIndex);
                            int onClauseEndIndex = StringHelper.IndexOf(fromClauseStr, @"\)", onClauseStartIndex);

                            fromClause.Insert(onClauseEndIndex
                                , string.Format(" AND {0}", relAttr.JoinConstraint));
                        }
                    }
                }
            }
            if (fromClause.Length == 0)
            {
                foreach (string key in tblNames.Keys)
                {
                    fromClause.Append(tblNames[key]);
                    break;
                }
            }

            if (distinctAttrs != null && distinctAttrs.Length != 0)
            {
                cols.Insert(0, "DISTINCT ");
            }
        }

        /// <summary>
        /// 将数据库的值保存到实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="recs">记录集</param>
        /// <param name="excludedProps">不包含在Select子句的属性名</param>
        private void ConvertRO2DO<T>(IDataReader reader, List<T> recs, List<string> excludedProps)
        {
            Type modelType = typeof(T);
            PropertyInfo[] propertyInfos = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            T rec = default(T);
            while (reader.Read())
            {
                rec = (T)modelType.GetConstructor(new Type[] { }).Invoke(null);

                for (int i = 0; i < propertyInfos.Length; i++)
                {
                    PropertyInfo curPropertyInfo = propertyInfos[i];
                    ColumnAttr[] attrs = (ColumnAttr[])propertyInfos[i].GetCustomAttributes(typeof(ColumnAttr), true);
                    ColumnAttr colAttr = null;
                    if (attrs.Length == 0 || ((colAttr = (ColumnAttr)attrs[0]) == null)) continue;
                    if (null != excludedProps && excludedProps.Exists((excludedName) => { return excludedName.Equals(propertyInfos[i].Name); })) continue; // 排除不包含的Select子句的列

                    string colName = (StringHelper.IsNullOrWhiteSpace(colAttr.ColName) ? propertyInfos[i].Name : colAttr.ColName);
                    colName = (colAttr.ColName.IndexOf(m_QuotingStart) >= 0 ? colAttr.ColName.Replace(m_QuotingStart.ToString(), string.Empty).Replace(m_QuotingEnd.ToString(), string.Empty) : colName);
                    int colIndex = reader.GetOrdinal((StringHelper.IsNullOrWhiteSpace(colAttr.TblAlias) ? string.Empty : colAttr.TblAlias + "_") + colName);
                    if (reader.IsDBNull(colIndex)) continue;
                    object valObj = (reader.IsDBNull(colIndex) ? Util.GetDefaultVal(curPropertyInfo.PropertyType) : reader.GetValue(colIndex));
                    curPropertyInfo.SetValue(rec, Converter.ChangeType(valObj, curPropertyInfo.PropertyType), null);
                }

                recs.Add(rec);
            }
        }

        /// <summary>
        /// 组装Insert语句的Sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parse2InsertSql"></param>
        /// <param name="entity"></param>
        /// <param name="paramInfos"></param>
        /// <returns>Insert语句的Sql</returns>
        private string AssembleInsertSql<T>(Parse2InsertSQL parse2InsertSql, T entity, List<ParamInfo> paramInfos)
        {
            // 组装Sql
            string tblName = string.Empty; // 表名称
            Type modelType = typeof(T);
            TblAttr tblAttr = (TblAttr)modelType.GetCustomAttributes(typeof(TblAttr), true)[0];
            tblName = tblAttr.Name;

            StringBuilder cols = new StringBuilder(); // 字段名称
            StringBuilder colParams = new StringBuilder(); // 字段参数
            PropertyInfo[] propertyInfos = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            string realColName = string.Empty; // 实际的字段名称
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                ColumnAttr[] colAttrs = (ColumnAttr[])propertyInfos[i].GetCustomAttributes(typeof(ColumnAttr), true);
                if (colAttrs == null || colAttrs.Length == 0) continue;
                ColumnAttr colAttr = colAttrs[0];
                if (colAttr.IsPrimary && colAttr.IsAutoGenerate) continue;
                if (propertyInfos[i].GetValue(entity, null) == colAttr.IgnoreValue || object.Equals(propertyInfos[i].GetValue(entity, null), colAttr.IgnoreValue)) continue;

                realColName = (StringHelper.IsNullOrWhiteSpace(colAttr.ColName) ? propertyInfos[i].Name : colAttr.ColName);
                cols.AppendFormat("{0},", (realColName.IndexOf(m_QuotingStart) >= 0 ? realColName : m_QuotingStart + realColName + m_QuotingEnd));
                colParams.AppendFormat("{0}{1},"
                    , m_Db.ParamPreffix
                    , realColName.Replace(m_QuotingStart.ToString(), "").Replace(m_QuotingEnd.ToString(), ""));
                paramInfos.Add(new ParamInfo(realColName.Replace(m_QuotingStart.ToString(), "").Replace(m_QuotingEnd.ToString(), ""), propertyInfos[i].GetValue(entity, null)));
            }
            if (cols.Length > 0)
            {
                cols.Remove(cols.Length - 1, 1);
            }
            if (colParams.Length > 0)
            {
                colParams.Remove(colParams.Length - 1, 1);
            }

            SQLInsertClause sqlInsertClause = new SQLInsertClause();
            sqlInsertClause.Tbl = tblName;
            sqlInsertClause.Cols = cols.ToString();
            sqlInsertClause.Values = colParams.ToString();

            string sql = parse2InsertSql(sqlInsertClause);

            return sql;
        }
        #endregion
    }

    /// <summary>
    /// 全表查询入参
    /// </summary>
    public class QueryParam
    {
        protected string where = ""; // where子句, 使用属性名代替DB字段名
        protected string orderBy = ""; // order by子句, 使用属性名代替DB字段名
        protected List<string> excludedProps = null; // select子句中不包含的属性名
        protected Dictionary<string, DBFn> propFns = null; // select子句中的列处理函数键值对，如Index : MAX({0})
        protected List<ParamInfo> paramInfos = null; // // where子句中的参数
        protected string groupBy = ""; // group by子句，使用属性名代替DB字段名
        protected string having = ""; // having 子句，使用属性名代替DB字段名

        public string Where { get { return where; } set { where = value; } }
        public string OrderBy { get { return orderBy; } set { orderBy = value; } }
        public List<string> ExcludedProps { get { return excludedProps; } set { excludedProps = value; } }
        public Dictionary<string, DBFn> PropFns { get { return propFns; } set { propFns = value; } }
        public List<ParamInfo> ParamInfos { get { return paramInfos; } set { paramInfos = value; } }
        public string GroupBy { get { return groupBy; } set { groupBy = value; } }
        public string Having { get { return having; } set { having = value; } }
    }

    /// <summary>
    /// 分页查询入参
    /// </summary>
    public class QueryByPagingParam : QueryParam
    {
        public int StartIndex { get; set; }
        public int PageSize { get; set; }
    }

    /// <summary>
    /// 查询总记录数入参
    /// </summary>
    public class QueryCountParam
    {
        private string where = ""; // where子句, 使用属性名代替DB字段名
        private List<ParamInfo> paramInfos = null; // // where子句中的参数
        private string groupBy = ""; // group by子句，使用属性名代替DB字段名
        private string having = ""; // having 子句，使用属性名代替DB字段名

        public string Where { get { return where; } set { where = value; } }
        public List<ParamInfo> ParamInfos { get { return paramInfos; } set { paramInfos = value; } }
        public string GroupBy { get { return groupBy; } set { groupBy = value; } }
        public string Having { get { return having; } set { having = value; } }
    }

    /// <summary>
    /// 生成全表查询SQL的子句集合
    /// </summary>
    public class SQLQueryClause
    {
        public string Select { get; set; }
        public string From { get; set; }
        public string Where { get; set; }
        public string OrderBy { get; set; }
        public string GroupBy { get; set; }
        public string Having { get; set; }
    }

    /// <summary>
    /// 生成分页查询SQL的子句集合
    /// </summary>
    public class SQLQueryByPagingClause : SQLQueryClause
    {
        public int StartIndex { get; set; }
        public int PageSize { get; set; }
    }

    /// <summary>
    /// 获取记录总数SQL的子句集合
    /// </summary>
    public class SQLQueryCountClause
    {
        public string Select { get; set; }
        public string From { get; set; }
        public string Where { get; set; }
        public string GroupBy { get; set; }
        public string Having { get; set; }
    }

    /// <summary>
    /// 生成全表插入SQL的子句集合
    /// </summary>
    public class SQLInsertClause
    {
        public string Tbl { get; set; }
        public string Cols { get; set; }
        public string Values { get; set; }
    }

    /// <summary>
    /// 生成全表删除SQL的子句集合
    /// </summary>
    public class SQLDelClause
    {
        public string From { get; set; }
        public string Where { get; set; }
    }

    /// <summary>
    /// 生成全表更新SQL的子句集合
    /// </summary>
    public class SQLUpdateClause
    {
        public string Tbl { get; set; }
        public string Set { get; set; }
        public string Where { get; set; }
    }

    /// <summary>
    /// DB聚集函数
    /// </summary>
    public enum DBFn
    {
        TRIM,
        MAX,
        MIN,
        AVG,
        COUNT,
        SUM,
        TOTAL,
        GROUP_CONCAT
    }
}
