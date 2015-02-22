using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using lpp.CommonHelper;

namespace CommonHelperUnitTest
{
    [TestClass]
    public class StringHelperTest
    {
        [TestMethod]
        public void IndexOf1()
        {
            string originalStr = "c:\\text.sssc";
            string[] keywords = { ".c", "t." };

            Assert.IsFalse(StringHelper.IsMatch(originalStr, keywords, MatchType.SUFFIX), "Should be false.");
        }

        [TestMethod]
        public void IndexOf2()
        {
            string originalStr = "c:\\text.cs";
            string[] keywords = { ".c", ".cs" };

            Assert.IsTrue(StringHelper.IsMatch(originalStr, keywords, MatchType.SUFFIX), "Should be true.");
        }

        [TestMethod]
        public void IndexOf3()
        {
            string originalStr = "c:\text.cs";
            string[] keywords = { "c:\text.cs" };

            Assert.IsTrue(StringHelper.IsMatch(originalStr, keywords, MatchType.ALL), "Should be true.");
        }

        [TestMethod]
        public void GetUrlPath()
        {
            string url = "https://192.1.1.1:99";
            string hostName = StringHelper.GetUrlPart(url, UrlPart.HOST_NAME);
            string port = StringHelper.GetUrlPart(url, UrlPart.PORT);
            string protocol = StringHelper.GetUrlPart(url, UrlPart.PROTOCOL);

            Assert.AreEqual(protocol, "https");
            Assert.AreEqual(port, "99");
            Assert.AreEqual(hostName, "192.1.1.1");
        }
    }
}
