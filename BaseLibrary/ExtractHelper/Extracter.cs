using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace lpp.ExtractHelper
{
    public sealed class Extracter
    {
        /// <summary>
        /// 获取url字符串的各部分
        /// </summary>
        /// <param name="url"></param>
        /// <param name="urlPart"></param>
        /// <returns></returns>
        public static string GetUrlPart(string url, UrlPart urlPart)
        {
            string result = string.Empty;
            Regex reg = new Regex(@"^((?<protocol>\w+)://)?(?<hostname>[^/:]+):?(?<port>\d+)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            switch (urlPart)
            {
                case UrlPart.PROTOCOL:
                    result = reg.Match(url).Result("${protocol}");
                    if (string.IsNullOrEmpty(result))
                        result = "http";
                    break;
                case UrlPart.PORT:
                    result = reg.Match(url).Result("${port}");
                    if (string.IsNullOrEmpty(result))
                        result = "80";
                    break;
                case UrlPart.HOST_NAME:
                    result = reg.Match(url).Result("${hostname}");
                    break;
            }

            return result;
        }
    }

    /// <summary>
    /// Url各部分
    /// </summary>
    public enum UrlPart
    {
        PROTOCOL,
        PORT,
        HOST_NAME
    }
}
