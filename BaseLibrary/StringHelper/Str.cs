using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace lpp.StringHelper
{
    public sealed class Str
    {
        public delegate bool Handle(string value);

        /// <summary>
        /// 避免字符串过长，超过指定位数后面的用"..."代替
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="length">最大字符数</param>
        /// <param name="AddPoints">是否在截取后的字符串末加上省略号</param>
        /// <returns>截取后的字符串</returns>
        public static string CutString(string input, int length, bool AddPoints)
        {
            if (input == null || input.Length <= length || length < 0)
            {
                return input;
            }
            else
            {
                return String.Format("{0}{1}", input.Substring(0, length), AddPoints ? "..." : String.Empty);
            }
        }

        /// <summary>
        /// 检查字符串是否含关键字数组的任意一个或多个关键字
        /// </summary>
        /// <param name="originalStr">目标字符串</param>
        /// <param name="keywords">关键字数组，多个关键字时为OR关系</param>
        /// <param name="matchType">匹配类型</param>
        /// <returns></returns>
        public static bool IsMatch(string originalStr, string[] keywords, MatchType matchType = MatchType.ALL)
        {
            // 加工关键字，使其符合正则表达式格式
            List<string> dealedKeywords = new List<string>();
            foreach (string keyword in keywords)
            {
                StringBuilder dealedKeyword = new StringBuilder();
                for (int i = 0, len = keyword.Length; i < len; ++i)
                {
                    switch (keyword[i])
                    {
                        case '.':
                        case '?':
                        case '+':
                        case '{':
                        case '}':
                        case '$':
                        case '^':
                        case '[':
                        case ']':
                        case '|':
                        case '\\':
                            dealedKeyword.Append("\\" + keyword[i]);
                            break;
                        default:
                            dealedKeyword.Append(keyword[i]);
                            break;
                    }
                }
                if (string.IsNullOrEmpty(dealedKeyword.ToString())) continue;

                dealedKeywords.Add(dealedKeyword.ToString());
            }

            if (dealedKeywords.Count == 0) return false;
            
            string exp = string.Join("|", dealedKeywords.ToArray());
            string prefix = string.Empty;
            string suffix = string.Empty;
            switch (matchType)
            {
                case MatchType.ALL:
                    prefix = "^";
                    suffix = "$";
                    break;
                case MatchType.PREFIX:
                    prefix = "^";
                    break;
                case MatchType.SUFFIX:
                    suffix = "$";
                    break;
                case MatchType.CONTAIN:
                    break;
            }
            Regex reg = new Regex(string.Format("{0}({1}){2}", prefix, exp, suffix));

            return reg.IsMatch(originalStr);
        }

        /// <summary>
        /// 获取在原字符串中匹配正则表达式的字符起始索引
        /// </summary>
        /// <param name="origStr">原字符串</param>
        /// <param name="regEx">正则表达式</param>
        /// <param name="startIndex">检测起始位置</param>
        /// <returns></returns>
        public static int IndexOf(string origStr, string regEx, int startIndex = 0, RegexOptions regexOptions = RegexOptions.None)
        {
            Regex regex = new Regex(regEx, regexOptions);
            Match match = regex.Match(origStr, startIndex);
            int index = -1;
            if (match != null)
                index = match.Index;
            return index;
        }

        /// <summary>
        /// 替换第一个匹配的值
        /// </summary>
        /// <param name="originalStr">操作字符串</param>
        /// <param name="oldRegex">旧值正则表达式</param>
        /// <param name="newStr">新值</param>
        /// <returns>替换后的字符串</returns>
        public static string ReplaceFirst(string originalStr, string oldRegex, string newStr)
        {
            Regex regex = new Regex(oldRegex);
            Match match = regex.Match(originalStr);
            int index = match.Index;
            if (index == -1) return originalStr;

            string prefixPart = originalStr.Substring(0, index);
            string suffixPart = originalStr.Substring(index + match.Length);
            string resultStr = prefixPart + newStr + suffixPart;

            return resultStr;
        }

        /// <summary>
        /// 判断字符串是否全为空格
        /// </summary>
        /// <param name="originalStr">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(string originalStr)
        {
            return string.IsNullOrEmpty(originalStr) || string.IsNullOrEmpty(originalStr.Trim());
        }

        /// <summary>
        /// 判断字符串是否全为空格
        /// </summary>
        /// <param name="originalStr">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(StringBuilder originalStr)
        {
            return originalStr == null || originalStr.Length == 0 || string.IsNullOrEmpty(originalStr.ToString().Trim());
        }

        /// <summary>
        /// handle处理数组各元素后均返回true则返回true，否则返回false
        /// </summary>
        /// <param name="handle">处理函数</param>
        /// <param name="strs">字符串数组</param>
        /// <returns></returns>
        public static bool Every(Handle handle, params string[] strs)
        {
            bool isSuccess = true;
            foreach (string str in strs)
            {
                isSuccess = handle(str);
                if (!isSuccess) break;
            }

            return isSuccess;
        }

        /// <summary>
        /// 检测目标字符串中是否含关键字数组中的任意一个或多个，匹配任意一个或多个则返回true，否则返回false
        /// </summary>
        /// <param name="str">目标字符串</param>
        /// <param name="keywords">关键字数组</param>
        /// <returns></returns>
        public static bool Some(string str, string[] keywords)
        {
            bool isSuccess = false;

            foreach (string keyword in keywords)
            {
                isSuccess = str.IndexOf(keyword) > -1;
                if (isSuccess) break;
            }

            return isSuccess;
        }

        /// <summary>
        /// 通过模板批量生成字符串
        /// </summary>
        /// <param name="formats">模板数组，{0}、{1}等将被实际值替换掉</param>
        /// <param name="values">实际值</param>
        /// <returns></returns>
        public static string[] Format(string[] formats, params string[] values)
        {
            string[] strs = new string[formats.Length];
            for (int i = 0, len = formats.Length; i < len; i++)
            {
                strs[i] = string.Format(formats[i], values);
            }

            return strs;
        }
    }

    /// <summary>
    /// 匹配类型
    /// </summary>
    public enum MatchType
    {
        CONTAIN, // 包含
        PREFIX, // 开头
        SUFFIX, // 结尾
        ALL // 完全匹配
    }
}
