using lpp.CMDHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace IISMgr.Base
{
    public abstract class IIS
    {
        protected string PATH = string.Empty;

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cmds">命令集</param>
        /// <param name="sysDriver">系统分区卷名</param>
        protected ExecRsp Exec(string[] cmds, string sysDriver = "c:")
        {
            List<string> cmdLst = new List<string>(cmds);
            cmdLst.Insert(0, string.Format("cd {0}", PATH));
            cmdLst.Insert(0, sysDriver);
            return CMD.Exec(cmdLst); 
        }
    }
}
