using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace lpp.Win32Helper
{
    public sealed class SysEnv
    {
        public static RegistryKey OpenSysEnv()
        {
            return Registry.LocalMachine
                .OpenSubKey("SYSTEM", true)
                .OpenSubKey("ControlSet001", true)
                .OpenSubKey("Control", true)
                .OpenSubKey("Session Manager", true)
                .OpenSubKey("Environment", true);
        }

        public static string GetSysEnvByName(string name)
        {
            string result = string.Empty;
            try
            {
                result = OpenSysEnv().GetValue(name).ToString();
            }
            catch (Exception)
            {
            }

            return result;
        }

        public static bool IsExist(string name)
        {
            bool isExist = false;
            isExist = string.IsNullOrEmpty(GetSysEnvByName(name));

            return isExist;
        }

        public static void SetSysEnvByName(string name, object value)
        {
            try
            {
                OpenSysEnv().SetValue(name, value);
            }
            catch (Exception)
            {
            }
        }

    }
}
