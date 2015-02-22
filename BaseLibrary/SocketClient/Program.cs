using System;
using System.Collections.Generic;
using System.Text;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client is Running!!");
            PureSocket ps = new PureSocket(32000);
            while (true)
            {
                Console.Write("Input content pls:");
                string content = Console.ReadLine();
                ps.Send(content, "127.0.0.1", 31200);
            }
        }
    }
}
