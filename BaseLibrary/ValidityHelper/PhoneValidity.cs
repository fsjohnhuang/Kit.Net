using System.Text.RegularExpressions;

namespace lpp.ValidityHelper
{
    public sealed class PhoneValidity
    {
        /// <summary>
        /// 正则表达式验证“移动”手机号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsMobileNum(string num)
        {
            if (string.IsNullOrEmpty(num)) return false;

            string pattern = @"^(13[0-9]|15[0-9]|18[0-9])\d{8}$";
            return Regex.IsMatch(num, pattern);
        }

        /// <summary>
        /// 正则表达式验证“联通”手机号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsUnicomNum(string num)
        {
            if (string.IsNullOrEmpty(num)) return false;

            string pattern = @"^(132\d{8}$";
            return Regex.IsMatch(num, pattern);
        }

        /// <summary>
        /// 正则表达式验证固话号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPhoneNum(string num)
        {
            if (string.IsNullOrEmpty(num)) return false;

            string pattern = @"^[1-9][0-9]{7}$";
            return Regex.IsMatch(num, pattern);
        }
    }
}
