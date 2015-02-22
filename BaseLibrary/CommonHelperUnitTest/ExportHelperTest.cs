using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using lpp.CommonHelper;

namespace CommonHelperUnitTest
{
    [TestClass]
    public class ExportHelperTest
    {
        [TestMethod]
        public void ExportToExcel()
        {
            DataTable dt = new DataTable();
            dt.TableName = "测试";
            dt.Columns.Add("IP");
            dt.Columns.Add("姓名");
            dt.Rows.Add(new object[] { "127.0.0.1", "John" });
            dt.Rows.Add(new object[] { "172.0.0.1", "Mary" });
            dt.Rows.Add(new object[] { "192.0.0.1", "Kite" });

            ExportHelper.ExportToExcel(dt, "d:\\t.xls");
        }
    }
}
