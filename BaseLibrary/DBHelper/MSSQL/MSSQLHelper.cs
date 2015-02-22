using System;
using System.Collections.Generic;
using System.Text;

using lpp.DBAttr;

namespace lpp.DBHelper.MSSQL
{
    public class MSSQLHelper : BaseHelper, IDBHelper
    {
        public MSSQLHelper(string connStr)
            : base(new MSSQLInstance(connStr)) 
        {
            base.ParseToJoinTypeEvent += (joinType) => {
                string joinClause = string.Empty;
                switch (joinType)
                {
                    case JoinType.INNER_JOIN:
                        joinClause = " INNER JOIN ";
                        break;
                    case JoinType.RIGHT_OUTER_JOIN:
                        joinClause = " RIGHT OUTER JOIN ";
                        break;
                    case JoinType.LEFT_OUTER_JOIN:
                        joinClause = " LEFT OUTER JOIN ";
                        break;
                    case JoinType.NONE:
                        break;
                    default:
                        throw new Exception("Attribute(joinType) should be INNER_JOIN, RIGHT_OUTER_JOIN or LEFT_OUTER_JOIN!");
                }

                return joinClause;
            };

            base.WrapFnEvent += (dbFn, prop) => {
                string handled = string.Empty;
                switch (dbFn)
                {
                    case DBFn.TRIM:
                        handled = string.Format("LTRIM(RTRIM({0}))", prop);
                        break;
                    case DBFn.MAX:
                        handled = string.Format("Max({0})", prop);
                        break;
                    case DBFn.MIN:
                        handled = string.Format("Min({0})", prop);
                        break;
                }

                return handled;
            };
        }

        #region 查询
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">SQL语句缓存key</param>
        /// <param name="where">where子句，以#为类属性前缀，非#开头的均为数据表字段，语法均按数据库格式</param>
        /// <param name="orderBy">order by子句，以#为类属性前缀，非#开头的均为数据表字段，语法均按数据库格式</param>
        /// <param name="paramInfos">替换where子句中以@为前缀的入参</param>
        /// <param name="excludedProps">Select子句中不包含的数据表字段对应的类属性名称</param>
        /// <param name="propFns">Select子句中对数据表字段对应的类属性名称执行内聚函数</param>
        /// <returns></returns>
        public List<T> Query<T>(string cacheKey, string where = "", string orderBy = "", List<ParamInfo> paramInfos = null, List<string> excludedProps = null, Dictionary<string, DBFn> propFns = null, string groupBy = null, string having = null) where T : class
        {
            QueryParam queryParam = new QueryParam();
            queryParam.Where = where;
            queryParam.OrderBy = orderBy;
            queryParam.ParamInfos = paramInfos;
            queryParam.ExcludedProps = excludedProps;
            queryParam.PropFns = propFns;
            queryParam.GroupBy = groupBy;
            queryParam.Having = having;

            return base.Query<T>((sqlQueryClause, _cacheSQL, _cacheKey) =>
            {
                string selectSql = string.Empty;
                if (StringHelper.IsNullOrWhiteSpace(_cacheKey))
                {
                    selectSql = string.Format("SELECT {0} FROM {1} WHERE 1 = 1", sqlQueryClause.Select, sqlQueryClause.From);
                }
                else if (_cacheSQL.ContainsKey(_cacheKey))
                {
                    selectSql = _cacheSQL[_cacheKey];
                }
                else
                {
                    selectSql = string.Format("SELECT {0} FROM {1} WHERE 1 = 1", sqlQueryClause.Select, sqlQueryClause.From);
                    _cacheSQL[_cacheKey] = selectSql;
                }

                return string.Format("{0} {1} {2} {3} {4}"
                     , selectSql
                     , (StringHelper.IsNullOrWhiteSpace(sqlQueryClause.Where) ? string.Empty : "AND" + sqlQueryClause.Where)
                     , (StringHelper.IsNullOrWhiteSpace(sqlQueryClause.GroupBy) ? string.Empty : "GROUP BY" + sqlQueryClause.GroupBy)
                     , (StringHelper.IsNullOrWhiteSpace(sqlQueryClause.Having) ? string.Empty : "Having" + sqlQueryClause.Having)
                     , (StringHelper.IsNullOrWhiteSpace(sqlQueryClause.OrderBy) ? string.Empty : "ORDER BY" + sqlQueryClause.OrderBy));
            }, queryParam, cacheKey);
        }

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">SQL语句缓存key</param>
        /// <param name="where">where子句，以#为类属性前缀，非#开头的均为数据表字段，语法均按数据库格式</param>
        /// <param name="orderBy">order by子句，以#为类属性前缀，非#开头的均为数据表字段，语法均按数据库格式</param>
        /// <param name="paramInfos">替换where子句中以@为前缀的入参</param>
        /// <returns></returns>
        public List<T> Query<T>(string cacheKey, string where = "", string orderBy = "", params ParamInfo[] paramInfos) where T : class
        {
            List<ParamInfo> _paramInfos = null;
            if (paramInfos != null && paramInfos.Length != 0)
            {
                _paramInfos = new List<ParamInfo>();
                _paramInfos.AddRange(paramInfos);
            }
            return Query<T>(cacheKey, where, orderBy, _paramInfos);
        }

        /// <summary>
        /// 查询单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">SQL语句缓存key</param>
        /// <param name="where">where子句，以#为类属性前缀，非#开头的均为数据表字段，语法均按数据库格式</param>
        /// <param name="orderBy">order by子句，以#为类属性前缀，非#开头的均为数据表字段，语法均按数据库格式</param>
        /// <param name="paramInfos">替换where子句中以@为前缀的入参</param>
        /// <param name="excludedProps">Select子句中不包含的数据表字段对应的类属性名称</param>
        /// <param name="propFns">Select子句中对数据表字段对应的类属性名称执行内聚函数</param>
        /// <returns></returns>
        public T QuerySingle<T>(string cacheKey, string where = "", string orderBy = "", List<ParamInfo> paramInfos = null, List<string> excludedProps = null, Dictionary<string, DBFn> propFns = null) where T : class
        {
            QueryParam queryParam = new QueryParam();
            queryParam.Where = where;
            queryParam.OrderBy = orderBy;
            queryParam.ParamInfos = paramInfos;
            queryParam.ExcludedProps = excludedProps;
            queryParam.PropFns = propFns;

            return base.QuerySingle<T>((sqlQueryClause, _cacheSQL, _cacheKey) =>
            {
                string selectSql = string.Empty;
                if (StringHelper.IsNullOrWhiteSpace(_cacheKey))
                {
                    selectSql = string.Format("SELECT TOP 1 {0} FROM {1} WHERE 1 = 1", sqlQueryClause.Select, sqlQueryClause.From);
                }
                else if (_cacheSQL.ContainsKey(_cacheKey))
                {
                    selectSql = _cacheSQL[_cacheKey];
                }
                else
                {
                    selectSql = string.Format("SELECT TOP 1 {0} FROM {1} WHERE 1 = 1", sqlQueryClause.Select, sqlQueryClause.From);
                    _cacheSQL[_cacheKey] = selectSql;
                }

                return string.Format("{0} {1} {2} {3} {4}"
                   , selectSql
                   , (StringHelper.IsNullOrWhiteSpace(sqlQueryClause.Where) ? string.Empty : "AND" + sqlQueryClause.Where)
                   , (StringHelper.IsNullOrWhiteSpace(sqlQueryClause.GroupBy) ? string.Empty : "GROUP BY" + sqlQueryClause.GroupBy)
                   , (StringHelper.IsNullOrWhiteSpace(sqlQueryClause.Having) ? string.Empty : "Having" + sqlQueryClause.Having)
                   , (StringHelper.IsNullOrWhiteSpace(sqlQueryClause.OrderBy) ? string.Empty : "ORDER BY" + sqlQueryClause.OrderBy));
            }, queryParam, cacheKey);
        }

        /// <summary>
        /// 查询单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">SQL语句缓存key</param>
        /// <param name="where">where子句，以#为类属性前缀，非#开头的均为数据表字段，语法均按数据库格式</param>
        /// <param name="orderBy">order by子句，以#为类属性前缀，非#开头的均为数据表字段，语法均按数据库格式</param>
        /// <param name="paramInfos">替换where子句中以@为前缀的入参</param>
        /// <returns></returns>
        public T QuerySingle<T>(string cacheKey, string where = "", string orderBy = "", params ParamInfo[] paramInfos) where T : class
        {
            List<ParamInfo> _paramInfos = null;
            if (paramInfos != null && paramInfos.Length != 0)
            {
                _paramInfos = new List<ParamInfo>();
                _paramInfos.AddRange(paramInfos);
            }
            return QuerySingle<T>(cacheKey, where, orderBy, _paramInfos);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">SQL语句缓存key</param>
        /// <param name="startIndex">起始索引，从0开始</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="where">Where子句</param>
        /// <param name="orderBy">Order By子句</param>
        /// <param name="paramInfos">替换where子句中以@为前缀的入参</param>
        /// <param name="excludedProps">Select子句中不包含的数据表字段对应的类属性名称</param>
        /// <param name="propFns">Select子句中对数据表字段对应的类属性名称执行内聚函数</param>
        /// <returns></returns>
        public List<T> QueryByPaging<T>(string cacheKey, int startIndex = 0, int pageSize = 30, string where = "", string orderBy = "", List<ParamInfo> paramInfos = null, List<string> excludedProps = null, Dictionary<string, DBFn> propFns = null) where T : class
        {
            QueryByPagingParam queryByPagingParam = new QueryByPagingParam();
            queryByPagingParam.StartIndex = startIndex;
            queryByPagingParam.PageSize = pageSize;
            queryByPagingParam.Where = where;
            queryByPagingParam.OrderBy = orderBy;
            queryByPagingParam.ParamInfos = paramInfos;
            queryByPagingParam.ExcludedProps = excludedProps;
            queryByPagingParam.PropFns = propFns;

            return base.QueryByPaging<T>((sqlQueryByPagingClause, _cacheSQL, _cacheKey) =>
            {
                string selectSql = string.Empty;
                if (StringHelper.IsNullOrWhiteSpace(_cacheKey))
                {
                    selectSql = string.Format("{0} FROM {1} WHERE 1=1 ", sqlQueryByPagingClause.Select, sqlQueryByPagingClause.From);
                }
                else if (_cacheSQL.ContainsKey(_cacheKey))
                {
                    selectSql = _cacheSQL[_cacheKey];
                }
                else
                {
                    selectSql = string.Format("{0} FROM {1} WHERE 1=1 ", sqlQueryByPagingClause.Select, sqlQueryByPagingClause.From);
                    _cacheSQL[_cacheKey] = selectSql;
                }

                return string.Format("WITH R AS (SELECT TOP " + (sqlQueryByPagingClause.StartIndex + sqlQueryByPagingClause.PageSize) + " {0} {1} {2} {3} {4})"
                   + " SELECT * FROM R "
                   + " EXCEPT "
                   + " SELECT TOP " + startIndex + " * FROM R {4}"
                   , selectSql
                   , (StringHelper.IsNullOrWhiteSpace(sqlQueryByPagingClause.Where) ? string.Empty : "AND " + sqlQueryByPagingClause.Where)
                   , (StringHelper.IsNullOrWhiteSpace(sqlQueryByPagingClause.GroupBy) ? string.Empty : "GROUP BY" + sqlQueryByPagingClause.GroupBy)
                   , (StringHelper.IsNullOrWhiteSpace(sqlQueryByPagingClause.Having) ? string.Empty : "Having" + sqlQueryByPagingClause.Having)
                   , (StringHelper.IsNullOrWhiteSpace(sqlQueryByPagingClause.OrderBy) ? string.Empty : "Order By " + sqlQueryByPagingClause.OrderBy));
            }, queryByPagingParam, cacheKey);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">SQL语句缓存key</param>
        /// <param name="startIndex">起始索引，从0开始</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="where">Where子句</param>
        /// <param name="orderBy">Order By子句</param>
        /// <param name="paramInfos">替换where子句中以@为前缀的入参</param>
        /// <returns></returns>
        public List<T> QueryByPaging<T>(string cacheKey, int startIndex = 0, int pageSize = 30, string where = "", string orderBy = "", params ParamInfo[] paramInfos) where T : class
        {
            List<ParamInfo> _paramInfos = null;
            if (paramInfos != null && paramInfos.Length != 0)
            {
                _paramInfos = new List<ParamInfo>();
                _paramInfos.AddRange(paramInfos);
            }
            return QueryByPaging<T>(cacheKey, startIndex, pageSize, where, orderBy, _paramInfos);
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">SQL语句缓存key</param>
        /// <param name="where">Where子句</param>
        /// <param name="paramInfos">替换where子句中以@为前缀的入参</param>
        /// <returns></returns>
        public int QueryCount<T>(string cacheKey, string where = "", List<ParamInfo> paramInfos = null) where T : class
        {
            QueryCountParam queryCountParam = new QueryCountParam();
            queryCountParam.Where = where;
            queryCountParam.ParamInfos = paramInfos;

            return base.QueryCount<T>((sqlQueryCountClause, _cacheSQL, _cacheKey) =>
            {
                string selectSql = string.Empty;
                if (StringHelper.IsNullOrWhiteSpace(_cacheKey))
                {
                    selectSql = string.Format("SELECT COUNT({0}) FROM {1} WHERE 1 = 1", sqlQueryCountClause.Select, sqlQueryCountClause.From);
                }
                else if (_cacheSQL.ContainsKey(_cacheKey))
                {
                    selectSql = _cacheSQL[_cacheKey];
                }
                else
                {
                    selectSql = string.Format("SELECT COUNT({0}) FROM {1} WHERE 1 = 1", sqlQueryCountClause.Select, sqlQueryCountClause.From);
                    _cacheSQL[_cacheKey] = selectSql;
                }

                return string.Format("{0} {1} {2} {3}"
                    , selectSql
                    , (StringHelper.IsNullOrWhiteSpace(sqlQueryCountClause.Where) ? string.Empty : "AND" + sqlQueryCountClause.Where)
                    , (StringHelper.IsNullOrWhiteSpace(sqlQueryCountClause.GroupBy) ? string.Empty : "GROUP BY" + sqlQueryCountClause.GroupBy)
                    , (StringHelper.IsNullOrWhiteSpace(sqlQueryCountClause.Having) ? string.Empty : "Having" + sqlQueryCountClause.Having)
                    );
            }, queryCountParam, cacheKey);
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">SQL语句缓存key</param>
        /// <param name="where">Where子句</param>
        /// <param name="paramInfos">替换where子句中以@为前缀的入参</param>
        /// <returns></returns>
        public int QueryCount<T>(string cacheKey, string where = "", params ParamInfo[] paramInfos) where T : class
        {
            List<ParamInfo> _paramInfos = null;
            if (paramInfos != null && paramInfos.Length != 0)
            {
                _paramInfos = new List<ParamInfo>();
                _paramInfos.AddRange(paramInfos);
            }
            return QueryCount<T>(cacheKey, where, _paramInfos);
        }
        #endregion

        #region 新增
        /// <summary>
        /// 插入记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert<T>(T entity) where T : class
        {
            return base.Insert<T>((sqlInsertClause) =>
            {
                return string.Format("INSERT INTO {0}({1}) VALUES({2})"
                , sqlInsertClause.Tbl
                , sqlInsertClause.Cols
                , sqlInsertClause.Values);
            }, entity);
        }

        /// <summary>
        /// 插入记录并返回新记录的自增长主键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public object InsertThenReturnID<T>(T entity) where T : class
        {
            return base.InsertThenReturnID<T>((sqlInsertClause) =>
            {
                return string.Format("INSERT INTO {0}({1}) VALUES({2}); SELECT @@IDENTITY"
                , sqlInsertClause.Tbl
                , sqlInsertClause.Cols
                , sqlInsertClause.Values);
            }, entity);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">Where子句</param>
        /// <param name="paramInfos">Where子句的参数</param>
        /// <returns></returns>
        public bool Del<T>(string where = "", List<ParamInfo> paramInfos = null) where T : class
        {
            return base.Del<T>((sqlDelClause) =>
            {
                return string.Format("DELETE FROM {0} WHERE 1=1 {1}"
                , sqlDelClause.From
                , (StringHelper.IsNullOrWhiteSpace(sqlDelClause.Where) ? string.Empty : "AND" + sqlDelClause.Where));
            }, where, paramInfos);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where">Where子句</param>
        /// <param name="paramInfos">Where子句的参数</param>
        /// <returns></returns>
        public bool Del<T>(string where = "", params ParamInfo[] paramInfos) where T : class
        {
            List<ParamInfo> _paramInfos = null;
            if (paramInfos != null && paramInfos.Length != 0)
            {
                _paramInfos = new List<ParamInfo>();
                _paramInfos.AddRange(paramInfos);
            }
            return Del<T>(where, _paramInfos);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <param name="paramInfos"></param>
        /// <returns></returns>
        public bool Update<T>(T entity, string where = "", List<ParamInfo> paramInfos = null) where T : class
        {
            return base.Update<T>((sqlUpdateClause) =>
            {
                return string.Format("UPDATE {0} SET {1} WHERE 1=1 {2}"
                , sqlUpdateClause.Tbl
                , sqlUpdateClause.Set
                , (StringHelper.IsNullOrWhiteSpace(sqlUpdateClause.Where) ? string.Empty : "AND" + sqlUpdateClause.Where));
            }, entity, where, paramInfos);
        }

        public bool Update<T>(T entity, string where = "", params ParamInfo[] paramInfos) where T : class
        {
            List<ParamInfo> _paramInfos = null;
            if (paramInfos != null && paramInfos.Length != 0)
            {
                _paramInfos = new List<ParamInfo>();
                _paramInfos.AddRange(paramInfos);
            }
            return Update<T>(entity, where, _paramInfos);
        }
        #endregion
    }
}
