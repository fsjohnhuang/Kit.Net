using System;
using System.Collections.Generic;
using System.Text;

namespace lpp.DBHelper
{
    internal static class AlgorithmHelper
    {
        /// <summary>
        /// 求最大值
        /// </summary>
        /// <param name="nums">数组</param>
        /// <returns>最大值</returns>
        public static int Max(int[] nums)
        {
            int max = 0;
            foreach (int num in nums)
            {
                if (max < num)
                    max = num;
            }

            return max;
        }

        /// <summary>
        /// 求最小值
        /// </summary>
        /// <param name="nums">数组</param>
        /// <returns>最小值</returns>
        public static int Min(int[] nums)
        {
            int min = 0;
            foreach (int num in nums)
            {
                if (min > num)
                    min = num;
            }

            return min;
        }
    }
}
