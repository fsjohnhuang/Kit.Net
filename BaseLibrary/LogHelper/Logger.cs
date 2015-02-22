using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace lpp.LogHelper
{
    public sealed class Logger
    {
        /// <summary>
        /// 将异常信息写到log文件
        /// </summary>
        /// <param name="ex">异常对象</param>
        public static void WriteEx2LogFile(Exception ex)
        {
            WriteMsg2LogFile(ex.ToString());
        }

        /// <summary>
        /// 将文本消息写到log文件
        /// </summary>
        /// <param name="msg">文本消息</param>
        public static void WriteMsg2LogFile(string msg)
        {
            DateTime now = DateTime.Now;
            string dirname = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OperLog"), now.ToString("yyyy-MM"));
            if (!Directory.Exists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }

            string filename = String.Format("Log{0}.txt", now.ToString("yyyyMMdd"));

            string lpath = Path.Combine(dirname, filename);

            using (FileStream fileStream = new FileStream(lpath, FileMode.Append, FileAccess.Write, FileShare.Read))
            using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                streamWriter.WriteLine(now.ToString() + "  ----  " + msg);
                streamWriter.Close();
            }
        }
    }
}
