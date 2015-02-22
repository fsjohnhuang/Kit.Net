using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using lpp.CommonHelper;
using System.IO;

namespace CommonHelperUnitTest
{
    [TestClass]
    public class ZipHelperTest
    {
        [TestMethod]
        public void Zip()
        {
            ZipHelper.ZipDir(@"F:\John's Dir\Principal Project\更新发布工具\Deployer\beta0.2发布包\DeployerServer", @"F:\DeployerServer");
        }

        [TestMethod]
        public void UnZip()
        {
            ZipHelper.UnZip(@"F:\tts2", @"F:\tts3");
        }

        [TestMethod]
        public void ZipStream() 
        {
            byte[] s = ZipHelper.ZipDir(@"F:\muisc");
            FileStream file = File.Create(@"F:\zip");
            file.Write(s, 0, s.Length);
            file.Close();
        }

        [TestMethod]
        public void Del()
        {
            Directory.Delete(@"F:\deployerNew\DeployerServer", true);
        }
    }
}
