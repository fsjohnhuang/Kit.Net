using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Management;
using lpp.LogHelper;
using lpp.StringHelper;
using lpp.CMDHelper;
using System.Text.RegularExpressions;
using lpp.CollectionHelper;

namespace DBHelperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //bool isMatch = Str.IsMatch(@"D:\System Volume Information", new string[] { "C:\\Documents and Settings", "System Volume Information" }, MatchType.CONTAIN);
            //string s = lpp.EncryptHelper.Encoder.Parse2Base64String("Data Source=172.100.1.18;Initial Catalog=Updater;User ID=updater@123;Password=yzywupdater");
            //string c = "";
            SelectQuery query = new SelectQuery("SELECT * FROM Win32_ComputerSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject mo in searcher.Get())
            {
                foreach (PropertyData pd in mo.Properties)
                {
                    Console.WriteLine(pd.Name + ": " + pd.Value);
                }
            }

            Console.Read();
            //List<string> tailors = CMD.GetTailors("exit");
            //string r = "";

            //string[] keywords = "".Split('|');
            //bool result = Str.IsMatch("test.txt", keywords, MatchType.SUFFIX);
            //string sss = "emergencytest:601";
            //string port = Extracter.GetUrlPart(sss, lpp.ExtractHelper.UrlPart.PORT);

            //string st = "";
            //ManagementClass cimobject = new ManagementClass("Win32_ComputerSystem");
            //ManagementObjectCollection moc = cimobject.GetInstances();
            //foreach (ManagementObject mo in moc)
            //{
            //    st = mo.ToString();
            //    string s = "";

            //    foreach (var item in mo.Properties)
            //    {
            //        s = "";
            //    }
            //} 

            //st = HardwareHelper.GetDiskDriverInfo(DiskDriverProperty.Capabilities);
            //string sc = "";


            //try
            //{
            //    SQLiteHelper helper = new SQLiteHelper(AppDomain.CurrentDomain.BaseDirectory + "AgentDB");
            //    MServiceInfo s = new MServiceInfo() { Type = 1, FullName = "test" };
            //    helper.Insert<MServiceInfo>(s);
            //    s = helper.QuerySingle<MServiceInfo>("S", "#FullName='test'");
            //    s.Type = -1;
            //    helper.Update<MServiceInfo>(s);
            //    string ss = s.FullName;
            //    Console.WriteLine(ss);
            //}
            //catch (Exception ex)
            //{
            //    Logger.WriteEx2LogFile(ex);
            //}


            //List<WAI> wai = helper.Query<WAI>("test");
            //List<MServiceInfo> ws = helper.Query<MServiceInfo>("Lst");
            //MServiceInfo ws = new MServiceInfo();
            //ws.Type = 14;
            //ws.FullName = "fn14";
            //helper.Insert<MServiceInfo>(ws);
            //string e = "";
            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            ////W wais = helper.QuerySingle<W>("w");
            ////int count1 = helper.QueryCount<W>("w");
            //List<W> ws1 = helper.QueryByPaging<W>("w", 10, 50);
            //watch.Stop();
            //Console.WriteLine("Without cache:" + watch.ElapsedMilliseconds.ToString());
            //watch.Reset();
            //SQLiteHelper helper1 = new SQLiteHelper(@"F:\John's Dir\Principal Project\更新发布工具\Deployer\beta0.3\srvDB");
            //watch.Start();
            //List<W> ws2 = helper1.QueryByPaging<W>("w", 20, 50);
            ////W ws = helper1.QuerySingle<W>("w");
            ////int count2 = helper.QueryCount<W>("w");
            //watch.Stop();
            //Console.WriteLine("With cache:" + watch.ElapsedMilliseconds.ToString());
            //watch.Reset();
            //SQLiteHelper helper2 = new SQLiteHelper(@"F:\John's Dir\Principal Project\更新发布工具\Deployer\beta0.3\srvDB");
            //watch.Start();
            //List<W> ws3 = helper2.QueryByPaging<W>("w", 30, 50);
            ////W ws = helper1.QuerySingle<W>("w");
            ////int count2 = helper.QueryCount<W>("w");
            //watch.Stop();
            //Console.WriteLine("With cache:" + watch.ElapsedMilliseconds.ToString());
            ////WAI w = helper.QuerySingle<WAI>(null, "#DFullName DESC");
            ////int count = helper.QueryCount<W>();

            //Zip.ZipDir(@"F:\John's Dir\Principal Project\更新发布工具\Deployer\beta0.5\安装包\Agent"
            //    , @"F:\John's Dir\Principal Project\更新发布工具\Deployer\beta0.5\安装包\Agent.tar.gz");
            //Zip.ZipDir(@"F:\John's Dir\Principal Project\更新发布工具\Deployer\beta0.5\安装包\AgentUpdater"
            //    , @"F:\John's Dir\Principal Project\更新发布工具\Deployer\beta0.5\安装包\AgentUpdater.tar.gz");
            //Zip.ZipDir(@"F:\John's Dir\Principal Project\更新发布工具\Deployer\beta0.5\安装包\AgentConsole"
            //    , @"F:\John's Dir\Principal Project\更新发布工具\Deployer\beta0.5\安装包\AgentConsole.tar.gz");
            //Zip.ZipDir(@"F:\John's Dir\Principal Project\更新发布工具\Deployer\beta0.5\安装包\Uninstaller"
            //    , @"F:\John's Dir\Principal Project\更新发布工具\Deployer\beta0.5\安装包\Uninstaller.tar.gz");
            //string webappPath = @"D:\dzh22";
            //string backupPath = @"D:\dzh2";
            //string strPID = CMD.Exec(new List<string>() { "rmdir /S /Q " + webappPath
            //        , "mkdir " + webappPat
            //        , "xcopy " + backupPath + " " + webappPath + " /E /R /H /Y >>nul 2>nul" }
            //       , null, false);

            //Console.Read();
        }
    }

    /// <summary>
    /// 服务实体
    /// </summary>
    //[TblAttr("Updater_ServiceInfo_ML")]
    //public class MServiceInfo
    //{
    //    [ColumnAttr(IsAutoGenerate = true, IsPrimary = true)]
    //    public long? ID { get; set; }
    //    [ColumnAttr]
    //    public string FullName { get; set; }
    //    [ColumnAttr]
    //    public string ServiceName { get; set; }
    //    [ColumnAttr(IgnoreValue = -1)]
    //    public int? Type { get; set; }
    //    [ColumnAttr]
    //    public int? Code { get; set; }
    //    [ColumnAttr]
    //    public string ServiceUri { get; set; }
    //}

    //[TblAttr("Updater_ServiceDependencyRel_ML")]
    //[TblAttr("Updater_Dependency_ML", "B")]
    //public class VDependency
    //{
    //    [ColumnAttr]
    //    public long? ServiceID { get; set; }
    //    [ColumnAttr(RelatedColName = "ID", RelatedTblAlias = "B", JoinType = JoinType.INNER_JOIN)]
    //    public long? DependencyID { get; set; }
    //    [ColumnAttr(TblAlias = "B")]
    //    public string FullName { get; set; }
    //}
}
