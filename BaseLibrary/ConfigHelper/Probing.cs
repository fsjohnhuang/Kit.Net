using System;
using System.Collections.Generic;
using System.Text;

using lpp.CollectionHelper;
using lpp.StringHelper;
using System.Reflection;

namespace lpp.ConfigHelper
{
    /// <summary>
    /// 程序集搜索帮助类
    /// </summary>
    public sealed class Probing
    {
        /// <summary>
        /// 配置程序集搜索位置，默认是从exe文件的同级目录和GAC中搜索所依赖的程序集
        /// </summary>
        /// <remarks>
        /// 只有在AppDomain未创建时设置才有效
        /// </remarks>
        /// <param name="assemblyDirs">程序集搜索位置集合</param>
        public static void ConfigProbing(List<string> assemblyDirs)
        {
            string assemblyProbingPath = Lst.Join<string>(assemblyDirs, true, ";");
            if (Str.IsNullOrWhiteSpace(assemblyProbingPath)) return;

            AppDomain.CurrentDomain.SetData("PRIVATE_BINPATH", assemblyProbingPath);
            AppDomain.CurrentDomain.SetData("BINPATH_PROBE_ONLY", assemblyProbingPath);
            var m = typeof(AppDomainSetup).GetMethod("UpdateContextProperty", BindingFlags.NonPublic | BindingFlags.Static);
            var funsion = typeof(AppDomain).GetMethod("GetFusionContext", BindingFlags.NonPublic | BindingFlags.Instance);
            m.Invoke(null, new object[] { funsion.Invoke(AppDomain.CurrentDomain, null), "PRIVATE_BINPATH", assemblyProbingPath });
        }
    }
}
