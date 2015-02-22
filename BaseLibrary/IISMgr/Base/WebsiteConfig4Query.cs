using lpp.TplHelper.Attr;
using System;
using System.Collections.Generic;
using System.Text;

namespace IISMgr.Base
{
    public class WebsiteConfig4Query : AuthorizationConfig4Remoting
    {
        // 网站名称
        [TplVar("SiteName")]
        public string SiteName { get; set; }
    }
}
