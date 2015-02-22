using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Collections;
using Service;
using System.Runtime.Remoting.Channels.Http;
using Deployer.IFileService;


namespace IPCRemotingClient
{
     class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("启动IPC客户端,回车生成远程代理");

            //IDictionary props = new Hashtable();
            //props["name"] = "IpcClient";
            //BinaryClientFormatterSinkProvider bProvider = new BinaryClientFormatterSinkProvider();
            //IpcClientChannel channel = new IpcClientChannel(props, bProvider);
            //ChannelServices.RegisterChannel(channel, false);

            //FileService fs = Activator.GetObject(typeof(FileService), "ipc://TestChannel/FileService") as FileService;
            //string content = fs.GetFileName();
            //Console.WriteLine(content);

            //IDictionary props = new Hashtable();
            //props["name"] = "IpcClient";
            //BinaryClientFormatterSinkProvider bProvider = new BinaryClientFormatterSinkProvider();
            //IpcClientChannel channel = new IpcClientChannel(props, bProvider);
            //ChannelServices.RegisterChannel(channel, false);

            //IDBMgr sss = Activator.GetObject(typeof(IDBMgr), "ipc://dbChannel/DBMgr") as IDBMgr;

            //List<MServiceInfo> s = sss.GetServices();
            //string content = fs.GetFileName();

            Hashtable props = new Hashtable();
            props["name"] = "httpClient";
            BinaryClientFormatterSinkProvider client = new BinaryClientFormatterSinkProvider();
            HttpClientChannel http = new HttpClientChannel(props, client);
            ChannelServices.RegisterChannel(http, false);
            IFileMgr fileMgr = Activator.GetObject(typeof(IFileMgr), "http://localhost:31000/FileMgr") as IFileMgr;
            string test = fileMgr.Test();
            for (int i = 0; i < ChannelServices.RegisteredChannels.Length; i++)
            {
                (ChannelServices.RegisteredChannels[i] as HttpChannel).StopListening(null);
                ChannelServices.UnregisterChannel(ChannelServices.RegisteredChannels[i]);
            }

            Console.Read();
        }
    }

     
}
