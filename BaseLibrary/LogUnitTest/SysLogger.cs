using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lpp.Log;

namespace lpp.LogUnitTest
{
    public class SysLogger
    {
        private ILogger<long, int, SysLogInfo> logger = new Logger<long, int, SysLogInfo>("test", (List<QueryParamInfo> queryParams, string connStr) => {
            return new List<SysLogInfo>();
        });

        public bool Write(string desc)
        {
            LogInfo<long, int> logInfo = new LogInfo<long,int>();
            logInfo.Desc = desc;
            return logger.Write(logInfo);
        }

        public bool Update(long id, string desc)
        {
            LogInfo<long, int> logInfo = new LogInfo<long, int>();
            logInfo.Desc = desc;
            logInfo.ID = id;
            return logger.Upate(logInfo);
        }

        public bool Delete(long id)
        {
            return logger.Delete(id);
        }

        public List<SysLogInfo> Query(List<QueryParamInfo> queryParams)
        {
            return logger.Query(queryParams);
        }
    }
}
