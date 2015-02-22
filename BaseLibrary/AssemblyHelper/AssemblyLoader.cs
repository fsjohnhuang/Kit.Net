using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using lpp.LogHelper;
using lpp.StringHelper;
using System.IO;

namespace lpp.AssemblyHelper
{
    /// <summary>
    /// 程序集加载器
    /// </summary>
    public sealed class AssemblyLoader
    {
        private static Dictionary<string, Assembly> loadedAssemblies = new Dictionary<string, Assembly>();
        private static List<string> assemblyDirs = new List<string>();

        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="alias"></param>
        public static void Load(AssemblyName assemblyName, string alias = "")
        {
            if (Str.IsNullOrWhiteSpace(alias))
            {
                alias = assemblyName.FullName;
            }
            if (loadedAssemblies.ContainsKey(alias)) return;

            try
            {
                loadedAssemblies.Add(alias, Assembly.Load(assemblyName));
            }
            catch (TypeLoadException ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="path"></param>
        /// <param name="alias"></param>
        public static void Load(string path, string alias = "")
        {
            if (Str.IsNullOrWhiteSpace(path)) return;
            if (Str.IsNullOrWhiteSpace(alias))
            {
                alias = path;
            }
            if (loadedAssemblies.ContainsKey(alias)) return;

            try
            {
                loadedAssemblies.Add(alias, Assembly.LoadFile(path));
                string dir = Path.GetDirectoryName(path);
                if (assemblyDirs.IndexOf(dir) == -1)
                {
                    assemblyDirs.Add(dir);
                }
            }
            catch (TypeLoadException ex)
            {
                Logger.WriteEx2LogFile(ex);
            }
        }

        /// <summary>
        /// 获取某程序集下的目标类型的类型对象
        /// </summary>
        /// <param name="assemblyAlias">程序集别名</param>
        /// <param name="type">目标类型全名</param>
        /// <returns>类型对象</returns>
        public static Type GetType(string assemblyAlias, string type)
        {
            if (!loadedAssemblies.ContainsKey(assemblyAlias)) return null;

            Assembly assembly = loadedAssemblies[assemblyAlias];
            return assembly.GetType(type);
        }

        /// <summary>
        /// 获取某程序集下的所有类型对象
        /// </summary>
        /// <param name="assemblyAlias">程序集别名</param>
        /// <returns></returns>
        public static Type[] GetTypes(string assemblyAlias)
        {
            if (!loadedAssemblies.ContainsKey(assemblyAlias)) return null;

            Assembly assembly = loadedAssemblies[assemblyAlias];
            return assembly.GetTypes();
        }

        /// <summary>
        /// 获取已加载的程序集的目录
        /// </summary>
        /// <returns></returns>
        public static string[] GetAssemblyDirs()
        {
            return assemblyDirs.ToArray();
        }
    }
}
