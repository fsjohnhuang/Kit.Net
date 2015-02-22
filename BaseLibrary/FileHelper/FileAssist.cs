using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace lpp.FileHelper
{
    public sealed class FileAssist
    {
        /// <summary>          
        /// Copy文件夹          
        /// </summary>          
        /// <param name="sPath">源文件夹路径</param>          
        /// <param name="dPath">目的文件夹路径</param>          
        /// <returns>完成状态：success-完成；其他-报错</returns>          
        public static void CopyFolder(string sPath, string dPath)
        {
            // 创建目的文件夹
            if (!Directory.Exists(dPath))
            {
                Directory.CreateDirectory(dPath);
            }
            // 拷贝文件
            DirectoryInfo sDir = new DirectoryInfo(sPath);
            FileInfo[] fileArray = sDir.GetFiles();
            foreach (FileInfo file in fileArray)
            {
                file.CopyTo(dPath + "\\" + file.Name, true);
            }
            // 循环子文件夹
            DirectoryInfo dDir = new DirectoryInfo(dPath);
            DirectoryInfo[] subDirArray = sDir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirArray)
            {
                CopyFolder(subDir.FullName, dPath + Path.DirectorySeparatorChar + subDir.Name);
            }
        }

        /// <summary>
        /// 获取指定目录下的所有子孙目录名称
        /// </summary>
        /// <param name="path">指定的目录</param>
        /// <returns></returns>
        public static string[] GetSubDirs(string path)
        {
            if (!Directory.Exists(path)) return new string[0];

            return Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
        }

        /// <summary>
        /// 获取指定目录下的所有子孙文件名称
        /// </summary>
        /// <param name="path">指定的目录</param>
        /// <returns></returns>
        public static string[] GetSubFiles(string path)
        {
            if (!Directory.Exists(path)) return new string[0];

            return Directory.GetFiles(path, "*", SearchOption.AllDirectories);
        }

        /// <summary>
        /// 删除指定目录下的所有子文件和子目录
        /// </summary>
        /// <param name="path">目录</param>
        public static void DelSubItems(string path)
        {
            if (!Directory.Exists(path)) return;

            string[] subFiles = Directory.GetFiles(path);
            string[] subDirs = Directory.GetDirectories(path);
            for (int i = 0, len = subFiles.Length; i < len; i++)
            {
                File.Delete(subFiles[i]);
            }
            for (int i = 0, len = subDirs.Length; i < len; i++)
            {
                Directory.Delete(subDirs[i], true);
            }
        }
    }
}
