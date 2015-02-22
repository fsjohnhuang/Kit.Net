using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using lpp.CommonHelper;
using System.Collections.Generic;
using System.Data;
using lpp.ZipHelper;

namespace CommonHelperUnitTest
{
    [TestClass]
    public class ConvertHelperTest
    {
        enum OperType
        {
            OS = 1,
            Windows = 3
        }

        [TestMethod]
        public void ConvertEnumToDic()
        {
            Dictionary<string, int> dic = ConvertHelper.ConvertEnumToDic<int>(typeof(OperType));

            Assert.AreEqual(dic["OS"], 1);
            Assert.AreEqual(dic["Windows"], 3);
            Assert.AreEqual(dic.Keys.Count, 2);
        }

        [TestMethod]
        public void ConvertList2DT()
        {
            List<Lpp> lpps = new List<Lpp>();
            lpps.Add(new Lpp() { Name = "John", Age = 23 });
            lpps.Add(new Lpp() { Name = "Mary", Age = 12 });
            lpps.Add(new Lpp() { Name = "Tim", Age = 90 });


            Dictionary<string, Converter> dic = new Dictionary<string, Converter>();
            dic.Add("Age", (obj) => {
                int age = Convert.ToInt32(obj);
                if (age > 50)
                {
                    return "老年人";
                }
                else if (age < 20)
                {
                    return "青年人";
                }
                else
                {
                    return "中年人";
                }
            });

            DataTable dt = ConvertHelper.ConvertList2DT(lpps, new string[]{ "Name", "Age" }, null, dic);
            Assert.AreEqual(dt.Rows.Count, 3);
            Assert.AreEqual(dt.Columns.Count, 2);
            Assert.AreEqual(dt.Rows[0]["Name"].ToString(), "John");
            Assert.AreEqual(dt.Rows[0]["Age"].ToString(), "中年人");
            Assert.AreEqual(dt.Rows[1]["Age"].ToString(), "青年人");
            Assert.AreEqual(dt.Rows[2]["Age"].ToString(), "老年人");
            //Assert.AreEqual(Convert.ToInt32(dt.Rows[1]["Age"]), 12);
            //Assert.AreEqual(Convert.ToInt32(dt.Rows[2]["Age"]), 90);
        }

        [TestMethod]
        public void ConvertOne2Other()
        {
            List<Lpp> lpps = new List<Lpp>();
            lpps.Add(new Lpp() { Name = "John", Age = 23 });
            lpps.Add(new Lpp() { Name = "Mary", Age = 12 });
            lpps.Add(new Lpp() { Name = "Tim", Age = 90 });

            List<lp> lps = ConvertHelper.ConvertOne2Other<Lpp, lp>(lpps);
            Assert.AreEqual(lps.Count, 3);
            Assert.AreEqual(lps[0].Name, "John");
            Assert.AreEqual(lps[1].Name, "Mary");
            Assert.AreEqual(lps[2].Name, "Tim");

            List<One2OtherMap> map = new List<One2OtherMap>();
            map.Add(new One2OtherMap() { SrcPropName = "Name", DestPropName = "MyName" });
            map.Add(new One2OtherMap() { SrcPropName = "Age", DestPropName = "MyAge" });
            List<l> ls = ConvertHelper.ConvertOne2Other<Lpp, l>(lpps, map);
            Assert.AreEqual(ls.Count, 3);
            Assert.AreEqual(ls[0].MyName, "John");
            Assert.AreEqual(ls[1].MyName, "Mary");
            Assert.AreEqual(ls[2].MyName, "Tim");
            Assert.AreEqual(ls[0].MyAge, 23);
            Assert.AreEqual(ls[1].MyAge, 12);
            Assert.AreEqual(ls[2].MyAge, 90);

            List<l> ls2 = ConvertHelper.ConvertOne2Other<Lpp, l>(lpps);
            Assert.AreEqual(ls2.Count, 3);

            lp lp = new lp();
            Lpp lpp = new Lpp() { Name = "John", Age = 26 };
            ConvertHelper.ConvertOne2Other<Lpp, lp>(lpp, lp);
            Assert.AreEqual(lp.Name, "John");

            l l = new l();
            ConvertHelper.ConvertOne2Other<Lpp, l>(lpp, l, map);
            Assert.AreEqual(l.MyName, "John");
            Assert.AreEqual(l.MyAge, 26);

            LNull lNull1 = new LNull() { Name = "set", Age = 1 };
            Lpp lppDest1 = ConvertHelper.ConvertOne2Other<LNull, Lpp>(lNull1);
            Assert.AreEqual(lppDest1.Name, "set");
            Assert.AreEqual(lppDest1.Age, 1);

            LNull lNull2 = new LNull() { Name = "John", Age = null };
            Lpp lppDest2 = ConvertHelper.ConvertOne2Other<LNull, Lpp>(lNull2);
            Assert.AreEqual(lppDest2.Name, "John");
            Assert.AreEqual(lppDest2.Age, default(int));

        }


    }

    public class Lpp
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class lp
    {
        public string Name { get; set; }
    }

    public class l
    {
        public string MyName { get; set; }
        public int MyAge { get; set; }
    }

    public class LNull
    {
        public string Name { get; set; }
        public int? Age { get; set; }
    }
}
