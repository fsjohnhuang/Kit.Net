using System;
using System.Collections.Generic;
using System.Text;

namespace lpp.DBHelper
{
    public interface IDBHelper
    {
        #region 查询
        List<T> Query<T>(string cacheKey, string where = "", string orderBy = "", List<ParamInfo> paramInfos = null, List<string> excludedProps = null, Dictionary<string, DBFn> propFns = null, string groupBy = null, string having = null) where T : class;
        List<T> Query<T>(string cacheKey, string where = "", string orderBy = "", params ParamInfo[] paramInfos) where T : class;
        T QuerySingle<T>(string cacheKey, string where = "", string orderBy = "", List<ParamInfo> paramInfos = null, List<string> excludedProps = null, Dictionary<string, DBFn> propFns = null) where T : class;
        T QuerySingle<T>(string cacheKey, string where = "", string orderBy = "", params ParamInfo[] paramInfos) where T : class;
        List<T> QueryByPaging<T>(string cacheKey, int startIndex = 0, int pageSize = 30, string where = "", string orderBy = "", List<ParamInfo> paramInfos = null, List<string> excludedProps = null, Dictionary<string, DBFn> propFns = null) where T : class;
        List<T> QueryByPaging<T>(string cacheKey, int startIndex = 0, int pageSize = 30, string where = "", string orderBy = "", params ParamInfo[] paramInfos) where T : class;
        int QueryCount<T>(string cacheKey, string where = "", List<ParamInfo> paramInfos = null) where T : class;
        int QueryCount<T>(string cacheKey, string where = "", params ParamInfo[] paramInfos) where T : class;
        #endregion

        #region 新增
        bool Insert<T>(T entity) where T : class;
        object InsertThenReturnID<T>(T entity) where T : class;
        #endregion

        #region 删除
        bool Del<T>(string where = "", List<ParamInfo> paramInfos = null) where T : class;
        bool Del<T>(string where = "", params ParamInfo[] paramInfos) where T : class;
        #endregion

        #region 更新
        bool Update<T>(T entity, string where = "", List<ParamInfo> paramInfos = null) where T : class;
        bool Update<T>(T entity, string where = "", params ParamInfo[] paramInfos) where T : class;
        #endregion
    }
}
