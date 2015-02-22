using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace lpp.DBHelper
{
    public interface IDBInstance
    {
        DbConnection Conn { get; }
        char ParamPreffix { get; }

        DbDataReader ExecReader(string sql, List<ParamInfo> paramInfos, CommandBehavior cmdBehavio);
        object ExecScalar(string sql, List<ParamInfo> paramInfos);
        int ExecNonQuery(string sql, List<ParamInfo> paramInfos);
        void AddRecs(DataTable dt);
        DataTable ExecFill(string sql, List<ParamInfo> paramInfos);
    }
}
