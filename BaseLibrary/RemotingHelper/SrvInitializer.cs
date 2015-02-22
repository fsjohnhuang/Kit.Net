using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace lpp.RemotingHelper
{
    /// <summary>
    /// 服务承载方初始化器
    /// </summary>
    public sealed class SrvInitializer
    { 
        private BinaryServerFormatterSinkProvider sinkBinaryProvider = new BinaryServerFormatterSinkProvider();
        private SoapServerFormatterSinkProvider sinkSoapProvider = new SoapServerFormatterSinkProvider();

        private List<string> httpChannels = new List<string>();
        private List<string> ipcChannels = new List<string>();

        public SrvInitializer(bool showCustErrors = true) 
        {
            sinkBinaryProvider.TypeFilterLevel = TypeFilterLevel.Full;
            sinkSoapProvider.TypeFilterLevel = TypeFilterLevel.Full;
            if (!showCustErrors) return;

            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            RemotingConfiguration.CustomErrorsEnabled(false);
        }

        /// <summary>
        /// 监听Http信道
        /// </summary>
        /// <param name="port">端口</param>
        /// <param name="channelName">信道名称</param>
        public void ListenHttpChannel(int port, string channelName, FormatterSinkProviderType fspt = FormatterSinkProviderType.Binary)
        {
            IDictionary props = new Hashtable();
            props["port"] = port;
            props["name"] = channelName;
            IServerChannelSinkProvider sckp = sinkBinaryProvider;
            if (fspt == FormatterSinkProviderType.Soap)
            {
                sckp = sinkSoapProvider;
            }
            HttpServerChannel srvChannel = new HttpServerChannel(props, sckp);
            ChannelServices.RegisterChannel(srvChannel, false);
            httpChannels.Add(channelName);
        }

        /// <summary>
        /// 监听IPC信道
        /// </summary>
        /// <param name="channelName">信道名称</param>
        /// <param name="protName">端口名称</param>
        public void ListenIPCChannel(string channelName, string protName, FormatterSinkProviderType fspt = FormatterSinkProviderType.Binary)
        {
            IDictionary props = new Hashtable();
            props["name"] = channelName;
            props["portName"] = protName;
            props["authorizedGroup"] = "Everyone";
            IServerChannelSinkProvider sckp = sinkBinaryProvider;
            if (fspt == FormatterSinkProviderType.Soap)
            {
                sckp = sinkSoapProvider;
            }
            IpcServerChannel channel = new IpcServerChannel(props, sckp);
            ChannelServices.RegisterChannel(channel, false);
            ipcChannels.Add(channelName);
        }
       
        /// <summary>
        /// 关闭已监听的信道
        /// </summary>
        public void Close()
        {
            IChannel[] channels = ChannelServices.RegisteredChannels;
            for (int i = 0, len = channels.Length; i < len; ++i)
            {
                IChannel channel = channels[i];
                if (httpChannels.IndexOf(channel.ChannelName) >= 0)
                {
                    (channel as HttpServerChannel).StopListening(null);
                    ChannelServices.UnregisterChannel(channel);
                }
                else if (ipcChannels.IndexOf(channel.ChannelName) >= 0)
                {
                    (channel as IpcServerChannel).StopListening(null);
                    ChannelServices.UnregisterChannel(channel);
                }
            }
        }

        /// <summary>
        /// 注册服务端激活对象
        /// </summary>
        /// <param name="type">对象类型</param>
        public void RegisterWellKnownObject(Type type, WellKnownObjectMode wellKnownObjectMode = WellKnownObjectMode.SingleCall)
        {
            RemotingConfiguration.RegisterWellKnownServiceType(type, type.FullName, wellKnownObjectMode);
        }

        /// <summary>
        /// 注册服务端激活对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="objectUri">对象Uri</param>
        public void RegisterWellKnownObject(Type type, string objectUri, WellKnownObjectMode wellKnownObjectMode = WellKnownObjectMode.SingleCall)
        {
            RemotingConfiguration.RegisterWellKnownServiceType(type, objectUri, wellKnownObjectMode);
        }

        /// <summary>
        /// 注册服务端初始化单例对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ObjRef RegisterMarshalObject<T>(T obj) where T : MarshalByRefObject
        {
            return RemotingServices.Marshal(obj, typeof(T).FullName);
        }

        public ObjRef RegisterMarshalObject(object obj, string objectUri = null)
        {
            return RemotingServices.Marshal((MarshalByRefObject)obj, string.IsNullOrEmpty(objectUri) ? obj.GetType().FullName : objectUri);
        }
    }

    /// <summary>
    /// 序列化提供器类型
    /// </summary>
    public enum FormatterSinkProviderType
    {
        Soap,
        Binary
    }
}
