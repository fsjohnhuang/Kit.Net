using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace lpp.DBHelper
{
    public class ParamInfo
    {
        /// <summary>
        /// 属性参数名（与数据表字段相同，但不能添加"["和"]"）
        /// </summary>
        public string TblAlias { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public DbType? DBType { get; set; }

        public ParamInfo(string name, object val)
        {
            this.Name = name;
            this.Value = val;
        }

        public ParamInfo(string tblAlias, string name, object val)
            : this(name, val)
        {
            this.TblAlias = tblAlias;
        }

        public ParamInfo(string name, object val, DbType dbType) : this(name, val)
        {
            this.DBType = dbType;
        }

        public ParamInfo(string tblAlias, string name, object val, DbType dbType)
            : this(tblAlias, name, val)
        {
            this.DBType = dbType;
        }
    }
}
