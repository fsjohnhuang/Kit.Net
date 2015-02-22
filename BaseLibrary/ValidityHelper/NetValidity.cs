using System.Text.RegularExpressions;

namespace lpp.ValidityHelper
{
    public sealed class NetValidity
    {
        /// <summary>
        /// 检查是否为IPv4
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIPv4(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;

            string pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";
            Regex reg = new Regex(pattern);
            return reg.IsMatch(str);
        }
    }
}
