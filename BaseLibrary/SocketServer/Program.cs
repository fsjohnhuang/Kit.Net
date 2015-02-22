using System;
using System.Collections.Generic;
using System.Text;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server is Running!!");

            PureSocket ps = new PureSocket(31200);
            ps.Start();

            string input = string.Empty;
            while (!string.Equals(input, "E"))
            {
                Console.WriteLine("Enter 'E' to Close Socket!!");
                input = Console.ReadLine();

                if (!string.Equals(input, "E"))
                {
                    ps.Send(input);
                }
            }

            ps.Close();
            Console.WriteLine("Socket has closed!!");

            Console.Read();
        }
    }
}
