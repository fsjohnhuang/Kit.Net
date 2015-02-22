using System;
using System.Collections.Generic;
using System.Text;

namespace lpp.FlowHelper
{
    public sealed class PipingItemReturnValue
    {
        private bool isSuccess = true;

        public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }

        private IDictionary<string, object> objs = new Dictionary<string, object>();

        public IDictionary<string, object> Objs
        {
            get { return objs; }
            set { objs = value; }
        }

        private Exception ex;

        public Exception Ex
        {
            get { return ex; }
            set { ex = value; }
        }
    }
}
