using System.IO;
using System.Text;
using System.Web.UI;

namespace lpp.EncryptHelper
{
    /// <summary>
    /// 编码帮助类
    /// </summary>
    public sealed class Encoder
    {
        /// <summary>
        /// 对字符串进行base64编码
        /// </summary>
        /// <param name="originalStr">未处理的字符串</param>
        /// <returns>base64编码后的字符串</returns>
        public static string Parse2Base64String(string originalStr)
        {
            StringBuilder base64Str = new StringBuilder();
            StringWriter sw = new StringWriter(base64Str);
            LosFormatter formatter = new LosFormatter();
            formatter.Serialize(sw, originalStr);
            sw.Close();

            return base64Str.ToString();
        }
    }
}
