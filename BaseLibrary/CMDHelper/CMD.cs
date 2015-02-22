using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using lpp.StringHelper;
using System.Threading;
using lpp.CollectionHelper;
using System.Text.RegularExpressions;

namespace lpp.CMDHelper
{
    public sealed class CMD
    {
        private static Dictionary<string, List<string>> cmdTailors = new Dictionary<string, List<string>>();

        /// <summary>
        /// 执行CMD命令，并返回执行结果
        /// </summary>
        /// <param name="cmd">CMD命令</param>
        /// <param name="tailor">裁剪的内容</param>
        /// <returns>执行结果</returns>
        public static ExecRsp Exec(string cmd, List<string> tailors = null)
        {
            return Exec(new List<string>() { cmd }, tailors);
        }

        /// <summary>
        /// 执行CMD命令，并返回执行结果
        /// </summary>
        /// <param name="cmd">CMD命令</param>
        /// <param name="tailor">裁剪的内容</param>
        /// <returns>执行结果</returns>
        public static ExecRsp Exec(List<string> cmds, List<string> tailors = null, bool waitForExit = true)
        {
            ExecRsp execRsp = new ExecRsp();

            ProcessStartInfo psi = new ProcessStartInfo("CMD");
            psi.UseShellExecute = false;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;

            Process proc = new Process();
            proc.StartInfo = psi;
            proc.Start();
            for (int i = 0, len = cmds.Count; i < len; i++)
            {
                proc.StandardInput.WriteLine(cmds[i]);
            }
            string stdout = string.Empty;
            string err = string.Empty;
            if (waitForExit)
            {
                proc.StandardInput.WriteLine("exit");
                proc.WaitForExit();
                stdout = proc.StandardOutput.ReadToEnd();
                err = proc.StandardError.ReadToEnd();
                if (tailors != null)
                {
                    foreach (string tailor in tailors)
                    {
                        stdout = stdout.Replace(tailor, string.Empty);
                    }
                }
            }
            else
            {
                execRsp.PID = proc.Id;
            }

            execRsp.Stdout = stdout;
            execRsp.Err = err;
            return execRsp;
        }

        /// <summary>
        /// 获取执行CMD命令后需裁剪的部分
        /// </summary>
        /// <param name="cmd">CMD命令</param>
        /// <returns></returns>
        public static List<string> GetTailors(string cmd, string splitKeyword = @"\s*(\r\n)+\s*")
        {
            ProcessStartInfo psi = new ProcessStartInfo("CMD");
            psi.UseShellExecute = false;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;

            Process proc = new Process();
            proc.StartInfo = psi;
            proc.Start();
            proc.StandardInput.WriteLine(cmd + " >nul 2>nul");
            proc.StandardInput.WriteLine("exit");
            proc.WaitForExit();
            string echo = proc.StandardOutput.ReadToEnd();
            echo = echo.Replace(" >nul 2>nul", string.Empty);

            Regex splitKWRegex = new Regex(splitKeyword);
            Regex ignoreRegex = new Regex("^" + splitKeyword + "$");
            string[] origTailors = splitKWRegex.Split(echo);
            List<string> tailors = Lst.Grep<string>(new List<string>(origTailors), (origVal) => {
                string val = origVal.ToString();
                return !Str.IsNullOrWhiteSpace(val) && !ignoreRegex.IsMatch(val);
            });

            return tailors;
        }

        /// <summary>
        /// 检查端口号是否被占用
        /// </summary>
        /// <param name="port">端口号</param>
        /// <returns></returns>
        public static bool IsOccupiedPort(int port)
        {
            bool isOccupied = false;
            string strPort = port.ToString();
            string key = "IsExistedPort_" + port;
            string cmd = "netstat -ona | findstr " + strPort;
            string result = Exec(cmd).Stdout;
            List<string> tailors;
            if (cmdTailors.ContainsKey(key))
            {
                tailors = cmdTailors[key];
            }
            else
            {
                tailors = GetTailors(cmd);
                cmdTailors[key] = tailors;
            }

            string echo = Exec(cmd, tailors).Stdout;
            isOccupied = Str.Some(echo
                , Str.Format(new string[] { "127.0.0.1:{0}", "[::1]:{0}", "0.0.0.0:{0}" }, strPort));

            return isOccupied;
        }

        /// <summary>
        /// 使用Explorer浏览指定路径
        /// </summary>
        /// <param name="path"></param>
        public static void Open(string path)
        {
            Exec("start \"\" \"" + path + "\"");
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="path">路径</param>
        public static void DelDir(string path)
        {
            Exec(new List<string>{ "rmdir /S /Q " + path }, null, false);
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="src">源路径</param>
        /// <param name="dest">目标路径</param>
        public static void Copy(string src, string dest)
        {
            Exec(new List<string> { string.Format("xcopy {0} {1} /E /R /H /Y >nul 2>nul" ) }, null, false);
        }

        /// <summary>
        /// 获取可用端口号
        /// </summary>
        /// <param name="defaultPort">默认端口号</param>
        /// <param name="step">寻找步长</param>
        /// <returns>可用端口号</returns>
        public static int? FindAvailablePort(int defaultPort = 1, int step = 1)
        {
            int? curPort = defaultPort;
            bool isAvailable = false;
            while (curPort > 1 && curPort < 100000)
            {
                isAvailable = !IsOccupiedPort(curPort.Value);
                if (isAvailable) break;
                curPort += step;
            }

            if (!isAvailable)
                curPort = null;
            return curPort;
        }

        /// <summary>
        /// 关闭计算机
        /// </summary>
        public static void ShutdownOS()
        {
            ProcessStartInfo ps = new ProcessStartInfo();
            ps.FileName = "shutdown.exe";
            ps.Arguments = "-s -t 1";
            Process.Start(ps);
        }

        /// <summary>
        /// 重启计算机
        /// </summary>
        public static void RestartOS()
        {
            ProcessStartInfo ps = new ProcessStartInfo();
            ps.FileName = "shutdown.exe";
            ps.Arguments = "-r -t 1";
            Process.Start(ps);
        }
    }

    public class ExecRsp
    {
        private byte m_Code = 0;
        public byte Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }

        public string Stdout { get; set; }

        private string m_Err;
        public string Err 
        {
            get
            {
                return m_Err;
            }
            set 
            {
                m_Code = (Str.IsNullOrWhiteSpace(value) ? (byte)0 : (byte)1);
                m_Err = value;
            } 
        }
        public int PID { get; set; }
    }
}
