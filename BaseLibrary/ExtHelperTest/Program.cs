using lpp.ExtractHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtHelperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string url1 = "http://www.baidu.com";
            string protocol1 = Extracter.GetUrlPart(url1, UrlPart.PROTOCOL);
            string hostName1 = Extracter.GetUrlPart(url1, UrlPart.HOST_NAME);
            string port1 = Extracter.GetUrlPart(url1, UrlPart.PORT);
            Console.WriteLine("protocol1: " + string.Equals(protocol1, "http"));
            Console.WriteLine("hostName1: " + string.Equals(hostName1, "www.baidu.com"));
            Console.WriteLine("port1: " + string.Equals(port1, "80"));

            string url2 = "http://www.163.com/test.aspx";
            string protocol2 = Extracter.GetUrlPart(url2, UrlPart.PROTOCOL);
            string hostName2 = Extracter.GetUrlPart(url2, UrlPart.HOST_NAME);
            string port2 = Extracter.GetUrlPart(url2, UrlPart.PORT);
            Console.WriteLine("protocol2: " + string.Equals(protocol2, "http"));
            Console.WriteLine("hostName2: " + string.Equals(hostName2, "www.163.com"));
            Console.WriteLine("port2: " + string.Equals(port2, "80"));

            string url3 = "file://www.163.com:808/test.aspx";
            string protocol3 = Extracter.GetUrlPart(url3, UrlPart.PROTOCOL);
            string hostName3 = Extracter.GetUrlPart(url3, UrlPart.HOST_NAME);
            string port3 = Extracter.GetUrlPart(url3, UrlPart.PORT);
            Console.WriteLine("protocol3: " + string.Equals(protocol3, "file"));
            Console.WriteLine("hostName3: " + string.Equals(hostName3, "www.163.com"));
            Console.WriteLine("port3: " + string.Equals(port3, "808"));

            string url4 = "www.163.com:808/test.aspx";
            string protocol4 = Extracter.GetUrlPart(url4, UrlPart.PROTOCOL);
            string hostName4 = Extracter.GetUrlPart(url4, UrlPart.HOST_NAME);
            string port4 = Extracter.GetUrlPart(url4, UrlPart.PORT);
            Console.WriteLine("protocol4: " + string.Equals(protocol4, "http"));
            Console.WriteLine("hostName4: " + string.Equals(hostName4, "www.163.com"));
            Console.WriteLine("port4: " + string.Equals(port4, "808"));

            string url5 = "192.168.10.12:808/test.aspx";
            string protocol5 = Extracter.GetUrlPart(url5, UrlPart.PROTOCOL);
            string hostName5 = Extracter.GetUrlPart(url5, UrlPart.HOST_NAME);
            string port5 = Extracter.GetUrlPart(url5, UrlPart.PORT);
            Console.WriteLine("protocol5: " + string.Equals(protocol5, "http"));
            Console.WriteLine("hostName5: " + string.Equals(hostName5, "192.168.10.12"));
            Console.WriteLine("port5: " + string.Equals(port5, "808"));

            Console.Read();
        }
    }

}

