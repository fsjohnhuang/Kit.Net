using lpp.LogHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace lpp.CommonHelper
{
    public class BaseHttpHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public virtual void ProcessRequest(HttpContext context)
        {
        }

        protected void InvokeAndHandleException(Action<HttpContext> action, HttpContext context)
        {
            try
            {
                action(context);
            }
            catch (Exception ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }
    }
}
