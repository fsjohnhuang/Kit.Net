using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;

namespace lpp.ZipHelper
{
    internal static class Util
    {
        /// <summary>
        /// 对字符串进行base64编码
        /// </summary>
        /// <param name="originalStr">未处理的字符串</param>
        /// <returns>base64编码后的字符串</returns>
        internal static string ToBase64String(string originalStr)
        {
            StringBuilder base64Str = new StringBuilder();
            StringWriter sw = new StringWriter(base64Str);
            LosFormatter formatter = new LosFormatter();
            formatter.Serialize(sw, originalStr);
            sw.Close();

            return base64Str.ToString();
        }

        /// <summary>
        /// 对base64编码的字符串进行解码
        /// </summary>
        /// <param name="base64Str">base64编码后的字符串</param>
        /// <returns>未处理的字符串</returns>
        internal static string FromBase64String(string base64Str)
        {
            LosFormatter formatter = new LosFormatter();
            string originalStr = formatter.Deserialize(base64Str).ToString();

            return originalStr;
        }
    }
}
