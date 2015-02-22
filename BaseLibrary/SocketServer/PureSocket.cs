using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace SocketServer
{
    public class PureSocket
    {
        private TcpListener listener;
        private Socket one;
        private bool open = true;

        public PureSocket(int port)
        {
            IPAddress addr = IPAddress.Parse("127.0.0.1");
            listener = new TcpListener(addr, port);
            listener.Start(50);
            one = listener.AcceptSocket();
        }

        public void Start()
        {
            Console.WriteLine("listenning....");

            ThreadPool.QueueUserWorkItem((state) => {
                if (one.Connected)
                {
                    Console.WriteLine("Client Connected");
                    ThreadPool.QueueUserWorkItem((socket) => {
                        while (true)
                        {
                            byte[] b = new byte[10];
                            int count = 10;
                            MemoryStream ms = new MemoryStream();
                            while (count == 10)
                            {
                                b = new byte[10];
                                count = (socket as Socket).Receive(b);
                                ms.Write(b, 0, count);
                            }
                            string content = Encoding.UTF8.GetString(ms.ToArray());
                            ms.Close();
                            Console.WriteLine("Client Request:" + content);
                        }
                    }, one);
                }
            }, null);
        }

        public void Send(string content){
            byte[] cb = Encoding.UTF8.GetBytes(content);
            one.Send(cb);
        }

        public void Close()
        {
            open = false;
        }
    }
}
