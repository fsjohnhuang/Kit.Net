using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace lpp.DBHelper
{
    internal static class StringHelper
    {
        public delegate bool Handle(string value);

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
                for (int i = 0, len = keyword.Length; i < len; ++i){
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
                dealedKeywords.Add(dealedKeyword.ToString());
            }

            string exp = string.Join("|", dealedKeywords.ToArray());
            string prefix = string.Empty;
            string suffix = string.Empty;
            switch (matchType){
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
    }

    /// <summary>
    /// 匹配类型
    /// </summary>
    internal enum MatchType
    {
        CONTAIN, // 包含
        PREFIX, // 开头
        SUFFIX, // 结尾
        ALL // 完全匹配
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
