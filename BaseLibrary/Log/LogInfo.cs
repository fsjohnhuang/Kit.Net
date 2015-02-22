using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using lpp.DBHelper.Attr;

namespace lpp.Log
{
    [TblAttr("Log")]
    public class LogInfo<T, S>
    {
        private long? m_ID = null; // 日志ID
        private string m_SysCode = string.Empty; // 系统编码
        private string m_ModuleCode = string.Empty; // 模块编码
        private T m_UserID = default(T); // 用户信息ID
        private string m_IP = string.Empty; // 操作IP
        private string m_Desc = string.Empty; // 日志描述、结果等
        private S m_Oper = default(S); // 操作类型
        private string m_Note = string.Empty; // 备忘
        private DateTime? m_OperDateTime = null; // 操作时间

        public LogInfo(T userID = default(T), 
            string ip = "", 
            string desc = "", 
            S oper = default(S), 
            string note = "", 
            DateTime? operDateTime = null)
        {
            this.m_UserID = userID;
            this.m_IP = ip;
            this.m_Desc = desc;
            this.m_Oper = oper;
            this.m_Note = note;
            this.m_OperDateTime = operDateTime;
        }

        public LogInfo(long id,
            T userID = default(T),
            string ip = "",
            string desc = "",
            S oper = default(S),
            string note = "",
            DateTime? operDateTime = null)
        {
            this.m_ID = id;
            this.m_UserID = userID;
            this.m_IP = ip;
            this.m_Desc = desc;
            this.m_Oper = oper;
            this.m_Note = note;
            this.m_OperDateTime = operDateTime;
        }

        public LogInfo(
            string sysCode,
            T userID = default(T),
            string ip = "",
            string desc = "",
            S oper = default(S),
            string note = "",
            DateTime? operDateTime = null)
        {
            this.m_SysCode = sysCode;
            this.m_UserID = userID;
            this.m_IP = ip;
            this.m_Desc = desc;
            this.m_Oper = oper;
            this.m_Note = note;
            this.m_OperDateTime = operDateTime;
        }
        public LogInfo(
            string sysCode,
            string moduleCode,
            T userID = default(T),
            string ip = "",
            string desc = "",
            S oper = default(S),
            string note = "",
            DateTime? operDateTime = null)
        {
            this.m_SysCode = sysCode;
            this.m_ModuleCode = moduleCode;
            this.m_UserID = userID;
            this.m_IP = ip;
            this.m_Desc = desc;
            this.m_Oper = oper;
            this.m_Note = note;
            this.m_OperDateTime = operDateTime;
        }

        [ColAttr("ID", IsPrimary = true)]
        public long ID
        {
            get { return ID; }
            set { ID = value; }
        }
        [ColAttr("UserID")]
        public T UserID 
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }
        [ColAttr("SysCode")]
        public string SysCode
        {
            get { return m_SysCode; }
            set { m_SysCode = value; }
        }
        [ColAttr("ModuleCode")]
        public string ModuleCode
        {
            get { return m_ModuleCode; }
            set { m_ModuleCode = value; }
        }
        [ColAttr("IP")]
        public string IP
        {
            get { return m_IP; }
            set { m_IP = value; }
        }
        [ColAttr("Desc")]
        public string Desc
        {
            get { return m_Desc; }
            set { m_Desc = value; }
        }
        [ColAttr("Oper")]
        public S Oper
        {
            get { return m_Oper; }
            set { m_Oper = value; }
        }
        [ColAttr("Note")]
        public string Note
        {
            get { return m_Note; }
            set { m_Note = value; }
        }
        [ColAttr("OperDateTime")]
        public DateTime? OperDateTime
        {
            get { return m_OperDateTime; }
            set { m_OperDateTime = value; }
        }
    }
}
