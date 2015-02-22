using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace lpp.ZipHelper
{
    public static class Zip
    {
        /// <summary>
        /// 压缩目录
        /// </summary>
        /// <param name="srcPath">源目录路径</param>
        /// <returns>已压缩的字节数组</returns>
        public static byte[] ZipDir(string srcPath)
        {
            if (!Directory.Exists(srcPath)) return null;

            if (srcPath[srcPath.Length - 1] != Path.DirectorySeparatorChar)
                srcPath += Path.DirectorySeparatorChar;
            MemoryStream ms = new MemoryStream();
            ZipOutputStream s = new ZipOutputStream(ms);
            s.SetLevel(6); // 0 - store only to 
            _Zip(srcPath, s, srcPath);
            s.Finish();
            byte[] zipBytes = ms.ToArray();
            s.Close();
            ms.Close();

            return zipBytes;
        }

        /// <summary>
        /// 压缩目录
        /// </summary>
        /// <param name="srcPath">源目录路径</param>
        /// <param name="destFilePath">目的文件路径</param>
        public static void ZipDir(string srcPath, string destFilePath)
        {
            if (srcPath[srcPath.Length - 1] != Path.DirectorySeparatorChar)
                srcPath += Path.DirectorySeparatorChar;
            ZipOutputStream s = new ZipOutputStream(File.Create(destFilePath));
            s.SetLevel(6); // 0 - store only to 9 - means best compression
            _Zip(srcPath, s, srcPath);
            s.Finish();
            s.Close();
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="zipFilePath">需解压的文件路径</param>
        /// <param name="destDir">目的目录路径</param>
        /// <returns></returns>
        /*public static string UnZip(string zipFilePath, string destDir)
        {
            string rootFile = " ";
            //读取压缩文件(zip文件)，准备解压缩
            ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath.Trim()));
            ZipEntry theEntry;
            string path = destDir;
            //解压出来的文件保存的路径

            string rootDir = " ";
            //根目录下的第一个子文件夹的名称
            while ((theEntry = s.GetNextEntry()) != null)
            {
                rootDir = Path.GetDirectoryName(StringHelper.FromBase64String(theEntry.Name));
                //得到根目录下的第一级子文件夹的名称
                if (rootDir.IndexOf("\\") >= 0)
                {
                    rootDir = rootDir.Substring(0, rootDir.IndexOf("\\") + 1);
                }
                string dir = Path.GetDirectoryName(StringHelper.FromBase64String(theEntry.Name));
                //根目录下的第一级子文件夹的下的文件夹的名称
                string fileName = Path.GetFileName(StringHelper.FromBase64String(theEntry.Name));
                //根目录下的文件名称
                if (dir != " ")
                //创建根目录下的子文件夹,不限制级别
                {
                    if (!Directory.Exists(destDir + "\\" + dir))
                    {
                        path = destDir + "\\" + dir;
                        //在指定的路径创建文件夹
                        Directory.CreateDirectory(path);
                    }
                }
                else if (dir == " " && fileName != "")
                //根目录下的文件
                {
                    path = destDir;
                    rootFile = fileName;
                }
                else if (dir != " " && fileName != "")
                //根目录下的第一级子文件夹下的文件
                {
                    if (dir.IndexOf("\\") > 0)
                    //指定文件保存的路径
                    {
                        path = destDir + "\\" + dir;
                    }
                }

                if (dir == rootDir)
                //判断是不是需要保存在根目录下的文件
                {
                    path = destDir + "\\" + rootDir;
                }

                //以下为解压缩zip文件的基本步骤
                //基本思路就是遍历压缩文件里的所有文件，创建一个相同的文件。
                if (fileName != String.Empty)
                {
                    if (theEntry.Size == 0)
                    {
                        Directory.CreateDirectory(path + "\\" + fileName); 
                    }
                    else
                    {
                        FileStream streamWriter = File.Create(path + "\\" + fileName);

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }

                        streamWriter.Close();
                    }
                }
            }
            s.Close();

            return rootFile;
        }*/

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="zipFilePath">需解压的文件路径</param>
        /// <param name="destDir">目的目录路径</param>
        /// <returns></returns>
        public static string UnZip(string zipFilePath, string destDir)
        {
            byte[] bytes = File.ReadAllBytes(zipFilePath.Trim());
            return UnZip(bytes, destDir);
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="zipBytes">需解压的文件字节数组</param>
        /// <param name="destDir">目的目录路径</param>
        /// <returns></returns>
        public static string UnZip(byte[] zipBytes, string destDir)
        {
            string rootFile = " ";
            //读取压缩文件(zip文件)，准备解压缩
            MemoryStream ms = new MemoryStream(zipBytes);
            ZipInputStream s = new ZipInputStream(ms);
            ZipEntry theEntry;
            string path = destDir;
            //解压出来的文件保存的路径

            string rootDir = " ";
            //根目录下的第一个子文件夹的名称
            while ((theEntry = s.GetNextEntry()) != null)
            {
                rootDir = Path.GetDirectoryName(Util.FromBase64String(theEntry.Name));
                //得到根目录下的第一级子文件夹的名称
                if (rootDir.IndexOf("\\") >= 0)
                {
                    rootDir = rootDir.Substring(0, rootDir.IndexOf("\\") + 1);
                }
                string dir = Path.GetDirectoryName(Util.FromBase64String(theEntry.Name));
                //根目录下的第一级子文件夹的下的文件夹的名称
                string fileName = Path.GetFileName(Util.FromBase64String(theEntry.Name));
                //根目录下的文件名称
                if (dir != " ")
                //创建根目录下的子文件夹,不限制级别
                {
                    if (!Directory.Exists(destDir + "\\" + dir))
                    {
                        path = destDir + "\\" + dir;
                        //在指定的路径创建文件夹
                        Directory.CreateDirectory(path);
                    }
                }
                else if (dir == " " && fileName != "")
                //根目录下的文件
                {
                    path = destDir;
                    rootFile = fileName;
                }
                else if (dir != " " && fileName != "")
                //根目录下的第一级子文件夹下的文件
                {
                    if (dir.IndexOf("\\") > 0)
                    //指定文件保存的路径
                    {
                        path = destDir + "\\" + dir;
                    }
                }

                if (dir == rootDir)
                //判断是不是需要保存在根目录下的文件
                {
                    path = destDir + "\\" + rootDir;
                }

                //以下为解压缩zip文件的基本步骤
                //基本思路就是遍历压缩文件里的所有文件，创建一个相同的文件。
                if (fileName != String.Empty)
                {
                    if (theEntry.Size == 0 && string.Equals(theEntry.Comment, "d"))
                    {
                        Directory.CreateDirectory(path + "\\" + fileName);
                    }
                    else
                    {
                        FileStream streamWriter = File.Create(path + "\\" + fileName);

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }

                        streamWriter.Close();
                    }
                }
            }
            s.Close();

            return rootFile;
        }

        private static void _Zip(string strFile, ZipOutputStream s, string staticFile)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar) strFile += Path.DirectorySeparatorChar;
            Crc32 crc = new Crc32();
            string[] filenames = Directory.GetFileSystemEntries(strFile);
            if (filenames.Length == 0)
            {
                byte[] buffer = new byte[0];
                string tempfile = strFile.Substring(staticFile.LastIndexOf("\\") + 1);
                ZipEntry entry = new ZipEntry(Util.ToBase64String(tempfile));

                entry.DateTime = DateTime.Now;
                entry.Size = 0;
                entry.Comment = "d";
                crc.Reset();
                crc.Update(buffer);
                entry.Crc = crc.Value;
                s.PutNextEntry(entry);

                s.Write(buffer, 0, buffer.Length);
                return;
            }

            foreach (string file in filenames)
            {
                if (Directory.Exists(file))
                {
                    _Zip(file, s, staticFile);
                }
                else // 否则直接压缩文件
                {
                    // 打开压缩文件
                    FileStream fs = File.OpenRead(file);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    string tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(Util.ToBase64String(tempfile));

                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);

                    s.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
