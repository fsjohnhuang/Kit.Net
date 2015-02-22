using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using lpp.StringHelper;

namespace lpp.Win32Helper
{
    public static class ProcessHelper
    {
        /// <summary>
        /// 通过进程的执行文件全路径和执行进程名称的关键字判断是否有匹配的进程存在
        /// </summary>
        /// <param name="path">进程的执行文件全路径</param>
        /// <param name="processName">进程名称</param>
        /// <param name="keywords">执行进程名称的关键字，多个关键字时关键字关系为OR</param>
        /// <returns></returns>
        public static Process Find(string path, string processName = null, string[] keywords = null)
        {
            Process[] processes;
            if (string.IsNullOrEmpty(processName))
                processes = Process.GetProcesses();
            else
                processes = Process.GetProcessesByName(processName);
            Process targetProcess = null;
            foreach (Process process in processes)
            {
                string fileName = string.Empty;
                try
                {
                    fileName = process.MainModule.FileName;
                }
                catch (Exception ex)
                {
                }

                if (string.Equals(fileName, path.Trim()))
                {
                    if (keywords != null && keywords.Length != 0)
                    {
                        if (Str.IsMatch(process.ProcessName, keywords, MatchType.CONTAIN))
                        {
                            targetProcess = process;
                            break;
                        }
                    }
                    else
                    {
                        targetProcess = process;
                        break;
                    }
                }
            }

            return targetProcess;
        }

        /// <summary>
        /// 根据进程ID获取进程
        /// </summary>
        /// <param name="id">进程ID</param>
        /// <returns></returns>
        public static Process FindById(int id)
        {
            Process process = null;
            Process[] processes = Process.GetProcesses();
            for (int i = 0, len = processes.Length; i < len; i++)
            {
                if (processes[i].Id == id)
                {
                    process = processes[i];
                    break;
                }
            }

            return process;
        }
    }
}
