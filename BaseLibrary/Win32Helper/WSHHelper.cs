using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using IWshRuntimeLibrary;

namespace lpp.Win32Helper
{
    /// <summary>
    /// 使用Windows Script Host操作
    /// </summary>
    public static class WSHHelper
    {
        /// <summary>
        /// 创建桌面快捷方便方式
        /// </summary>
        /// <param name="name">快捷方便方式名称</param>
        /// <param name="exePath">执行文件路径（默认为调用的程序集路径）</param>
        /// <param name="iconPath">图标路径(默认使用执行文件的图标)</param>
        /// <param name="windowStyle">窗口风格（默认为普通窗口风格）</param>
        public static void CreateShortcut(string name, string exePath = "", string iconPath = "",WindowStyle windowStyle = WindowStyle.NORMAL)
        {
            WshShell wshShell = new WshShell();
            IWshShortcut shortcut = wshShell.CreateShortcut(string.Format("{0}{1}{2}.lnk"
                , Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                , Path.DirectorySeparatorChar
                , name)) as IWshShortcut;
            shortcut.TargetPath = (string.IsNullOrEmpty(exePath) ? Assembly.GetCallingAssembly().Location : exePath);
            shortcut.WorkingDirectory = Environment.CurrentDirectory;
            shortcut.WindowStyle = (int)windowStyle;
            if (!string.IsNullOrEmpty(iconPath))
                shortcut.IconLocation = iconPath;
            shortcut.Save();
        }
    }

    /// <summary>
    /// 窗口风格
    /// </summary>
    public enum WindowStyle : int
    {
        NORMAL = 1, // 普通窗口
        MAXIMIZED = 3, // 最大化
        MINIMIZED = 7 //最小化
    }
}
