using lpp.TplHelper.Attr;
using System;
using System.Collections.Generic;
using System.Text;

namespace IISMgr.Base
{
    /// <summary>
    /// 操作多个网站的配置信息项
    /// </summary>
    public class WebsiteConfig4SiteNames : AuthorizationConfig4Remoting
    {
        // 必选参数，指定网站名称
        string[] m_SiteNames;
        [TplVar("SiteNames")]
        public string[] SiteNames
        {
            get { return m_SiteNames; }
            set { m_SiteNames = value; }
        }
    }
}
