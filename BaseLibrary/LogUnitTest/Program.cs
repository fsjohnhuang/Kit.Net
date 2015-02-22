using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lpp.Log;
using lpp.LogUnitTest;

namespace LogUnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SysLogger logger = new SysLogger();
            List<QueryParamInfo> list = new List<QueryParamInfo>();
            list.Add(new QueryParamInfo(){ Key = "Name", Type = typeof(string), Value = "John"});
            logger.Query(list);
        }
    }
}
