using System.Text.RegularExpressions;

namespace lpp.ValidityHelper
{
    public sealed class OtherValidity
    {
        /// <summary>
        /// 正则表达式验证身份证号码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsIDCard(string id)
        {
            Regex reg = new Regex(@"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$");
            return reg.IsMatch(id);
        }
    }
}
