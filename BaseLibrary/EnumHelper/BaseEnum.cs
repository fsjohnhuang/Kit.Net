using System;
using System.Collections.Generic;
using System.Text;

namespace lpp.EnumHelper
{
    // <summary>
    /// 自定义枚举基础类
    /// </summary>
    public abstract class BaseEnum
    {
        public int Value { get; private set; }
        public string Str { get; private set; }

        protected BaseEnum(int value, string str)
        {
            Value = value;
            Str = str;
        }

        public static Dictionary<int, string> ToDic(List<BaseEnum> baseEnums)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            foreach (BaseEnum baseEnum in baseEnums)
            {
                dic[baseEnum.Value] = baseEnum.Str;
            }

            return dic;
        }

        public static Dictionary<string, int> ToStr2IntDic(List<BaseEnum> baseEnums)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            foreach (BaseEnum baseEnum in baseEnums)
            {
                dic[baseEnum.Str] = baseEnum.Value;
            }

            return dic;
        }
    }
}
