using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Principal;
using System.IO;

using lpp.Win32Helper;
using System.Security.AccessControl;
using System.Diagnostics;
using System.Management;

namespace Win32HelperTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Impersonate()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            Console.WriteLine("name:" + identity.Name);

            AccessHelper accessHelper = new AccessHelper();
            bool result = accessHelper.Impersonate("Users", "");
            if (result)
            {
                WindowsIdentity identity2 = WindowsIdentity.GetCurrent();
                Console.WriteLine("name:" + identity2.Name);
            }

            accessHelper.CancelImpersonate();
        }

        [TestMethod]
        public void CreateFile()
        {
            string ownerName = WindowsIdentity.GetCurrent().Owner.Translate(typeof(NTAccount)).Value;
            string userName = WindowsIdentity.GetCurrent().User.Translate(typeof(NTAccount)).Value;
            FileSecurity dacl = new FileSecurity();
            FileSystemAccessRule acl = new FileSystemAccessRule(WindowsIdentity.GetCurrent().User, FileSystemRights.ReadData, AccessControlType.Allow);
            dacl.AddAccessRule(acl);

            File.Create(@"d:\test.txt", 4096, FileOptions.None, dacl);
        }

        [TestMethod]
        public void GetServiceExePath()
        {
            string file = WinServiceHelper.GetExePath("DeployerServer");
            string stop = "";
        }

        [TestMethod]
        public void StopService()
        {
            WinServiceHelper.Stop("DeployerServer");
        }

        [TestMethod]
        public void StartService()
        {
            WinServiceHelper.Start("DeployerServer");
        }

        [TestMethod]
        public void Uninstall()
        {
            WinServiceHelper.Unintall("DeployerUpdater");
        }

        [TestMethod]
        public void Install()
        {
            WinServiceHelper.Install(@"F:\John's Dir\Principal Project\更新发布工具\Deployer\beta0.1\DeployerServer\bin\Debug\DeployerServer.exe", "DeployerServer");
        }

        [TestMethod]
        public void IsExistByDisplayName()
        {
            bool isExist = WinServiceHelper.IsExistByDisplayName("Windows Time");
        }

        [TestMethod]
        public void GetLaunchType()
        {
            WinServiceHelper.GetLaunchType("W32Time");
        }

        [TestMethod]
        public void CreateShortcut()
        {
            //WSHHelper.CreateShortcut("test");
        }

        [TestMethod]
        public void Process_IsExist()
        {
            //Process p = ProcessHelper.Find("", "cmd");
           
        }
    }
}
