using lpp.TplHelper.Attr;
using System;
using System.Collections.Generic;
using System.Text;

namespace IISMgr.Base
{
    /// <summary>
    /// 删除Web虚拟目录的信息项
    /// </summary>
    public class VDirConfig4Del : AuthorizationConfig4Remoting
    {
        // 必选参数，指定网站的唯一描述性名称或元数据库路径
        [TplVar("SiteName")]
        public string SiteName { get; set; }
        // 指定网站中虚拟目录的路径。如果虚拟目录不在网站的根目录处，则此参数是必需的
        [TplVar("VirtualPath")]
        public string VirtualPath { get; set; }
        // 必选参数，指定虚拟目录的名称。虚拟目录的名称不必是唯一的。但是，当网站中包含名称相同的虚拟目录和物理目录时，物理目录在 Internet 上不可见
        [TplVar("Name")]
        public string Name { get; set; }
    }
}
