using System;
using System.Collections.Generic;
using System.Text;

namespace lpp.Win32Helper
{
    public static class OSHelper
    {
        private static Dictionary<OS, string> OSVersionMap = new Dictionary<OS,string>();

        static OSHelper()
        {
            OSVersionMap.Add(OS.Windows7, "6.1");
            OSVersionMap.Add(OS.WindowsServer2008R2, "6.1");
            OSVersionMap.Add(OS.WindowsVista, "6.0");
            OSVersionMap.Add(OS.WindowsServer2008, "6.0");
            OSVersionMap.Add(OS.WindowsServer2003R2, "5.2");
            OSVersionMap.Add(OS.WindowsServer2003, "5.2");
            OSVersionMap.Add(OS.WindowsXP, "5.1");
            OSVersionMap.Add(OS.Windows2000, "5.0");
            OSVersionMap.Add(OS.Windows9X, "4.X");
        }

        public static OS OS { 
            get 
            {
                string version = Environment.OSVersion.Version.Major + "." + Environment.OSVersion.Version.Minor;
                OS os = OS.Unkown;
                foreach (OS _os in OSVersionMap.Keys)
                {
                    if (string.Equals(OSVersionMap[_os], version))
                    {
                        os = _os;
                        break;
                    }
                    else if (Environment.OSVersion.Version.Major == 4)
                    {
                        os = OS.Windows9X;
                        break;
                    }
                }

                return os;
            } 
        }

    }

    public enum OS
    {
        Windows7,
        WindowsServer2008R2,
        WindowsServer2008,
        WindowsVista,
        WindowsServer2003R2,
        WindowsServer2003,
        WindowsXP,
        Windows2000,
        Windows9X,
        Unkown
    }
}
