using System;
using lpp.CommonHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using lpp.CollectionHelper;
using System.Collections;

namespace CommonHelperUnitTest
{
    [TestClass]
    public class CmdHelperTest
    {
        [TestMethod]
        public void Exec()
        {
            NGBossService srv = new NGBossService();
            ResultOfchgnumbasicinforsp r;
            try
            {
                r = srv.ChangeNumberBasicInfor("__FSMobile-WSC__", "wsc", "Wsc*ri2W", "15011665629");
            }
            catch (Exception)
            {
                string strs = "ss";
            }
            string str = "";
        }
    }
}
