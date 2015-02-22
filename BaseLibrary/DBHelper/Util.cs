using System;
using System.Collections.Generic;
using System.Text;

namespace lpp.DBHelper
{
    public static class Util
    {
        /// <summary>
        /// 根据数据类型获取默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefaultVal(Type type)
        {
            object result = null;

            if (type.Equals(typeof(string)))
            {
                result = string.Empty;
            }
            else if (type.Equals(typeof(int))
                || type.Equals(typeof(long))
                || type.Equals(typeof(short)))
            {
                result = 0;
            }
            else if (type.Equals(typeof(bool)))
            {
                result = false;
            }
            else if (type.Equals(typeof(byte)))
            {
                result = (byte)0;
            }
            else if (type.Equals(typeof(char)))
            {
                result = ' ';
            }

            return result;
        }

        /// <summary>
        /// 组装Where子句
        /// </summary>
        /// <param name="isAssemble">true：进行组装，false：返回空字符串</param>
        /// <param name="text">where子句内容</param>
        /// <param name="isAddJoinKeyword">是否添加如AND、OR等连接关键词</param>
        /// <param name="joinKeyword">连接关键词</param>
        /// <returns></returns>
        public static string AssembleWhereClause(bool isAssemble, string text, bool isAddJoinKeyword = false, string joinKeyword = "AND")
        {
            string whereClause = string.Empty;

            if (isAssemble)
            {
                if (isAddJoinKeyword)
                {
                    whereClause = string.Format(" {0} {1} ", joinKeyword, text);
                }
                else
                {
                    whereClause = string.Format(" {0} ", text);
                }
            }

            return whereClause;
        }
    }
}
