using System;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Management;

namespace lpp.Win32Helper
{
    public class AccessHelper
    {
        [DllImport("Advapi32.Dll")]
        static extern bool LogonUser(string userName, string domain, string userPassword,
            uint logonType, uint logonProvider, out IntPtr token);

        [DllImport("Kernel32.Dll")]
        static extern void CloseHandle(IntPtr token);

        private IntPtr token = IntPtr.Zero;

        /// <summary>
        /// 模拟用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public bool Impersonate(string userName, string password, string domain = "")
        {
            if (token != IntPtr.Zero) return false;
            bool result = LogonUser(userName, domain, password, 2, 0, out token);
            if (!result) return result;

            WindowsIdentity.Impersonate(token);
            return result;
        }

        /// <summary>
        /// 取消模拟用户
        /// </summary>
        public void CancelImpersonate()
        {
            CloseHandle(token);
            token = IntPtr.Zero;
        }

        /// <summary>
        /// 获取计算机工作组
        /// </summary>
        /// <returns></returns>
        public static string GetWorkgroup()
        {
            string workgroup = string.Empty;
            ManagementObjectSearcher compSyses = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
            foreach (ManagementObject compSys in compSyses.Get())
            {
                workgroup = compSys["Workgroup"].ToString();
                break;
            }

            return workgroup;
        }
    }
} 
