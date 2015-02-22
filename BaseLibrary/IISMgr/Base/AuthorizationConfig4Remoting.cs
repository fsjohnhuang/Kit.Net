using lpp.TplHelper.Attr;
using System;
using System.Collections.Generic;
using System.Text;

namespace IISMgr.Base
{
    /// <summary>
    /// 操作远程计算机时的身份验证信息项
    /// </summary>
    public class AuthorizationConfig4Remoting
    {
        // 在指定的远程计算机上运行脚本。键入不带反斜线的计算机名或 IP 地址。默认情况下，脚本在本地计算机上运行
        [TplVar("Server")]
        public string Server { get; set; }
        // 使用指定用户帐户的权限来运行脚本。此帐户必须是远程计算机上的管理员组成员。默认情况下，使用本地计算机当前用户的权限来运行脚本
        [TplVar("User")]
        public string User { get; set; }
        // 指定在User指定的用户帐户的密码
        [TplVar("Pwd")]
        public string Pwd { get; set; }
    }
}
