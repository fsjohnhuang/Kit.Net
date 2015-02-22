using lpp.CMDHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace IISMgr
{
    public sealed class Util
    {
        /// <summary>
        /// 获取未填必填项的返回值信息
        /// </summary>
        /// <param name="paramName">必填项名</param>
        /// <returns></returns>
        public static ExecRsp GetRequiredErrRsp(string paramName)
        {
            return new ExecRsp() { Code = 1, Err = new ArgumentNullException("paramName", string.Format("{0} is required.", paramName)).ToString() };
        }
    }
}
