using System.Text.RegularExpressions;

namespace lpp.ValidityHelper
{
    public sealed class CharValidity
    {
        /// <summary>
        /// 检查是否全是中文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsChinese(string str)
        {
            string pattern = @"^[\u4e00-\u9fa5]$";
            Regex reg = new Regex(pattern);
            return reg.IsMatch(str);
        }
    }
}
