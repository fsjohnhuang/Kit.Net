using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using Service;
using lpp.DBService;

namespace IPCRemoting
{
    class Program
    {
        private static string DBCHANNEL_NAME = "dbService";

        static void Main(string[] args)
        {
            Console.WriteLine("启动IPC服务中");

            //IDictionary props = new Hashtable();
            //props["portName"] = "TestChannel";
            //props["name"] = "ServiceBus";
            //props["authorizedGroup"] = "Everyone";
            //BinaryServerFormatterSinkProvider bProvider = new BinaryServerFormatterSinkProvider();
            //bProvider.TypeFilterLevel = TypeFilterLevel.Full;
            //IpcServerChannel channel = new IpcServerChannel(props, bProvider);
            //ChannelServices.RegisterChannel(channel, false);
            //RemotingConfiguration.RegisterWellKnownServiceType(typeof(FileService), "FileService", WellKnownObjectMode.SingleCall);

            BinaryServerFormatterSinkProvider dbSrvProvider = new BinaryServerFormatterSinkProvider();
            dbSrvProvider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary dbProps = new Hashtable();
            dbProps["portName"] = "dbChannel";
            dbProps["name"] = DBCHANNEL_NAME;
            dbProps["authorizedGroup"] = "Everyone";
            IpcServerChannel channel = new IpcServerChannel(dbProps, dbSrvProvider);
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(FileService), "FileService", WellKnownObjectMode.SingleCall);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(DBMgr), "DBMgr", WellKnownObjectMode.SingleCall);
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            RemotingConfiguration.CustomErrorsEnabled(false);

            Console.WriteLine("启动完成");
            Console.Read();
        }
    }

}
