using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Reflection;

using lpp.DBHelper;
using lpp.DBHelper.MSSQL;

namespace lpp.Log
{
    public class Logger<T, S, R> : ILogger<T, S, R>
    {
        public delegate List<R> CustomQueryLog(List<QueryParamInfo> queryParams, string connStr);

        private string connStr = null;
        private MSSQLHelper dbHelper = null;
        private static Assembly queryAssembly = null;
        private CustomQueryLog customQueryLog = null;

        public Logger(string connStr, CustomQueryLog customQueryLog)
        {
            this.connStr = connStr;
            //this.dbHelper = new MSSQLHelper(connStr);
            this.customQueryLog = customQueryLog;
        }

        public bool Write(LogInfo<T, S> logInfo)
        {
            return dbHelper.Insert<LogInfo<T, S>>(logInfo);
        }

        public bool Upate(LogInfo<T, S> logInfo)
        {
            return dbHelper.Update<LogInfo<T, S>>(logInfo);
        }

        public bool Delete(long id)
        {
            return dbHelper.Del<LogInfo<T, S>>(new ParamInfo("ID", id));
        }

        public List<R> Query(List<QueryParamInfo> queryParams)
        {
            List<R> logList = new List<R>();
            if (customQueryLog != null)
            {
                logList = customQueryLog(queryParams, connStr);
            }

            return logList;
        }
    }
}
