using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lpp.Log
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">用户信息ID的数据类型</typeparam>
    /// <typeparam name="S">操作的数据类型</typeparam>
    /// <typeparam name="R">系统实际日志对象类型</typeparam>
    public interface ILogger<T,S,R>
    {

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logInfo">日志对象</param>
        /// <returns></returns>
        bool Write(LogInfo<T, S> logInfo);

        /// <summary>
        /// 更新日志
        /// </summary>
        /// <param name="logInfo">日志对象</param>
        /// <returns></returns>
        bool Upate(LogInfo<T, S> logInfo);

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="id">日志ID</param>
        /// <returns></returns>
        bool Delete(long id);

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="queryParams">查询参数列表</param>
        /// <returns></returns>
        List<R> Query(List<QueryParamInfo> queryParams);
    }
}
