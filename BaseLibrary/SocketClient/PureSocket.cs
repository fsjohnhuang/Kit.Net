using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketClient
{
    public class PureSocket
    {
        private TcpClient listener;

        public PureSocket(int port)
        {
            //listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
            //listener.Bind(ipEndPoint);

            listener = new TcpClient(ipEndPoint);
        }

        public void Send(string content, string ip, int port)
        {
            if (!listener.Connected)
            {
                IPAddress ipAddr = IPAddress.Parse(ip);
                listener.Connect(ipAddr, port);
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    while (true)
                    {
                        byte[] b = new byte[10];
                        int count = 10;
                        MemoryStream ms = new MemoryStream();
                        while (count == 10)
                        {
                            b = new byte[10];
                            count = listener.Client.Receive(b);
                            ms.Write(b, 0, count);
                        }
                        string content1 = Encoding.UTF8.GetString(ms.ToArray());
                        ms.Close();
                        Console.WriteLine("Server Request:" + content1);
                    }
                }, null);
            }
            if (listener.Connected)
            {
                byte[] cb = Encoding.UTF8.GetBytes(content);
                listener.Client.Send(cb);
            }
        }
    }
}
