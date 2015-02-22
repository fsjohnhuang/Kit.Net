using lpp.TplHelper.Attr;
using System;
using System.Collections.Generic;
using System.Text;

namespace IISMgr.Base
{
    public class VDirConfig4Query : AuthorizationConfig4Remoting
    {
        [TplVar("SiteName")]
        public string SiteName { get; set; }
    }
}
