using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Ipc;
using System.Text;

namespace lpp.RemotingHelper
{
    /// <summary>
    /// 服务调用方初始化器
    /// </summary>
    public sealed class ClientInitializer
    {
        private BinaryClientFormatterSinkProvider sinkBinaryProvider = new BinaryClientFormatterSinkProvider();
        private SoapClientFormatterSinkProvider sinkSoapProvider = new SoapClientFormatterSinkProvider();

        private List<string> httpChannels = new List<string>();
        private List<string> ipcChannels = new List<string>();

        public ClientInitializer() { }

        /// <summary>
        /// 监听Http信道
        /// </summary>
        /// <param name="port">端口</param>
        /// <param name="channelName">信道名称</param>
        public void ListenHttpChannel(string channelName, FormatterSinkProviderType fspt = FormatterSinkProviderType.Binary)
        {
            IDictionary props = new Hashtable();
            props["port"] = 0;
            props["name"] = channelName;
            IClientChannelSinkProvider cckp = sinkBinaryProvider;
            if (fspt == FormatterSinkProviderType.Soap)
            {
                cckp = sinkSoapProvider;
            }
            HttpClientChannel srvChannel = new HttpClientChannel(props, cckp);
            ChannelServices.RegisterChannel(srvChannel, false);
            httpChannels.Add(channelName);
        }

        /// <summary>
        /// 监听IPC信道
        /// </summary>
        /// <param name="channelName">信道名称</param>
        /// <param name="protName">端口名称</param>
        public void ListenIPCChannel(string channelName, FormatterSinkProviderType fspt = FormatterSinkProviderType.Binary)
        {
            IDictionary props = new Hashtable();
            props["name"] = channelName;
            IClientChannelSinkProvider cckp = sinkBinaryProvider;
            if (fspt == FormatterSinkProviderType.Soap)
            {
                cckp = sinkSoapProvider;
            }
            IpcClientChannel channel = new IpcClientChannel(props, cckp);
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
                    (channel as HttpChannel).StopListening(null);
                    ChannelServices.UnregisterChannel(channel);
                }
                else if (ipcChannels.IndexOf(channel.ChannelName) >= 0)
                {
                    (channel as IpcChannel).StopListening(null);
                    ChannelServices.UnregisterChannel(channel);
                }
            }
        }

        /// <summary>
        /// 获取服务端激活对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="protocal">协议类型</param>
        /// <param name="host">服务承载方名称</param>
        /// <param name="objectUri">服务资源标识符</param>
        /// <returns></returns>
        public T GetObject<T>(Protocal protocal, string host, string objectUri) where T : class
        {
            string uri = string.Format("{0}://{1}/{2}", protocal.Str, host, objectUri);
            return Activator.GetObject(typeof(T), uri) as T;
        }

        /// <summary>
        /// 从IPC信道获取服务端激活对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="portName">服务承载方名称</param>
        /// <param name="objectUri">服务资源标识符</param>
        /// <returns></returns>
        public T GetObjectFromIPC<T>(string portName, string objectUri) where T : class
        {
            return GetObject<T>(Protocal.IPC, portName, objectUri);
        }

        /// <summary>
        /// 从Http信道获取服务端激活对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">服务承载方地址</param>
        /// <param name="objectUri">服务资源标识符</param>
        /// <returns></returns>
        public T GetObjectFromHttp<T>(string host, string objectUri) where T : class
        {
            return GetObject<T>(Protocal.HTTP, host, objectUri);
        }

        /// <summary>
        /// 从TCP信道获取服务端激活对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">服务承载方地址</param>
        /// <param name="objectUri">服务资源标识符</param>
        /// <returns></returns>
        public T GetObjectFromTcp<T>(string host, string objectUri) where T : class
        {
            return GetObject<T>(Protocal.TCP, host, objectUri);
        }
    }
}
