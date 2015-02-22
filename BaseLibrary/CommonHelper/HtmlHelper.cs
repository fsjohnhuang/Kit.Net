using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace lpp.CommonHelper
{
    public static class HtmlHelper
    {
         /// <summary>
        /// 去除所有的html格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string WipeHtml(string str)
        {
            string pattern = @"<[\s\S]*?>";
            str = Regex.Replace(str, pattern, "", RegexOptions.IgnoreCase);
            return str.Replace("&nbsp;", "");
        }

        /// <summary>
        /// 去除样式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string WipeStyle(string str)
        {
            string pattern = @"style=""[\s\S]*?""";
            str = Regex.Replace(str, pattern, "", RegexOptions.IgnoreCase);
            pattern = @"class=""[\s\S]*?""";
            str = Regex.Replace(str, pattern, "", RegexOptions.IgnoreCase);
            return str;
        }

        /// <summary>
        /// 根据文件后缀获取MIME
        /// </summary>
        /// <param name="extension">文件后缀</param>
        /// <returns>MIME</returns>
        public static string GetMime(Extension extension)
        {
            Dictionary<string, string > mimes = new Dictionary<string,string>();
            mimes.Add(Extension.CSS.ToString(), "text/css");
            mimes.Add(Extension.GIF.ToString(), "image/gif");
            mimes.Add(Extension.HTML.ToString(), "text/html");
            mimes.Add(Extension.ICO.ToString(), "image/x-icon");
            mimes.Add(Extension.JPEG.ToString(), "image/jpeg");
            mimes.Add(Extension.JPG.ToString(), "image/jpeg");
            mimes.Add(Extension.JS.ToString(), "text/javascript");
            mimes.Add(Extension.JSON.ToString(), "application/json");
            mimes.Add(Extension.PDF.ToString(), "application/pdf");
            mimes.Add(Extension.PNG.ToString(), "image/png");
            mimes.Add(Extension.SVG.ToString(), "image/svg+xml");
            mimes.Add(Extension.SWF.ToString(), "application/x-shockwave-flash");
            mimes.Add(Extension.TIFF.ToString(), "image/tiff");
            mimes.Add(Extension.TXT.ToString(), "text/plain");
            mimes.Add(Extension.WAV.ToString(), "audio/x-wav");
            mimes.Add(Extension.WMA.ToString(), "audio/x-ms-wma");
            mimes.Add(Extension.WMV.ToString(), "video/x-ms-wmv");
            mimes.Add(Extension.XML.ToString(), "text/xml");
            mimes.Add(Extension.XLS.ToString(), "application/ms-excel");
            mimes.Add(Extension.DOC.ToString(), "application/ms-Word");
            mimes.Add(Extension.XHTML.ToString(), "application/xhtml+xml");

            string mime = string.Empty;
            if (mimes.ContainsKey(extension.ToString()))
            {
                mime = mimes[extension.ToString()];
            }

            return mime;
        }

        /// <summary>
        /// 根据文件后缀和编码方式获取MIME
        /// </summary>
        /// <param name="extension">文件后缀</param>
        /// <param name="encode">编码方式</param>
        /// <returns>MIME</returns>
        public static string GetMime(Extension extension, Encoding encode)
        {
            var contentType = GetMime(extension);
            var charset = encode.WebName;

            if (string.IsNullOrEmpty(contentType) || string.IsNullOrEmpty(charset)) return string.Empty;

            var mime = string.Format("{0}; charset={1}",
                contentType,
                charset);
            return mime;
        }
    }

    /// <summary>
    /// 文件后缀
    /// </summary>
    public enum Extension
    {
        JSON,
        JS,
        XML,
        XLS,
        DOC,
        HTML,
        ICO,
        JPEG,
        JPG,
        PDF,
        PNG,
        SVG,
        SWF,
        TIFF,
        TXT,
        WAV,
        WMA,
        WMV,
        CSS,
        GIF,
        XHTML
    }
}
