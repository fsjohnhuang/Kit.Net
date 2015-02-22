using System.Web.UI;

namespace lpp.EncryptHelper
{
    public sealed class Decoder
    {
        /// <summary>
        /// 对base64编码的字符串进行解码
        /// </summary>
        /// <param name="base64Str">base64编码后的字符串</param>
        /// <returns>未处理的字符串</returns>
        public static string ParseFromBase64String(string base64Str)
        {
            LosFormatter formatter = new LosFormatter();
            string originalStr = formatter.Deserialize(base64Str).ToString();

            return originalStr;
        }
    }
}
