using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using System.Management;
using System.Xml;
using System.IO;
using System.Web.Security;
using System.Data.SQLite;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection();
            System.Data.SQLite.SQLiteConnectionStringBuilder connStr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
            connStr.DataSource = @"F:\John's Dir\Principal Project\更新发布工具\Deployer\beta0.3\srvDB";
            conn.ConnectionString = connStr.ToString();
            conn.Open();
            string str = "select * from Deployer_WeblogicAppInfo_ML";
            System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand();
            cmd.CommandText = str;
            cmd.Connection = conn;
            SQLiteDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                System.Console.WriteLine(reader.GetValue(1));
            }

            System.Console.Read();
        }
    }
}
