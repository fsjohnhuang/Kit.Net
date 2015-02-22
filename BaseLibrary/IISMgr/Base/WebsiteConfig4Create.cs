using lpp.TplHelper.Attr;
using System;
using System.Collections.Generic;
using System.Text;

namespace IISMgr.Base
{
    /// <summary>
    /// 创建网站的配置信息项
    /// </summary>
    public class WebsiteConfig4Create : AuthorizationConfig4Remoting
    {
        // 指定在本地计算机上网站内容文件的位置。如果指定的路径不存在，Iisweb 将创建它
        [TplVar("Path")]
        public string Path { get; set; }
        // 必选参数，指定网站名称
        [TplVar("SiteName")]
        public string SiteName { get; set; }
        // 指定网站的 TCP 端口号。默认端口为 80
        [TplVar("Port")]
        public string Port { get; set; }
        // 定网站的 IP 地址。默认设置为全部未分配，此设置将计算机上所有未分配给其他站点的 IP 地址都分配给该站点
        [TplVar("IPAddress")]
        public string IPAddress { get; set; }
        // 指定网站的主机头名称。默认情况下，站点没有主机头名称，必须根据其 IP 地址或端口号才能识别该站点
        [TplVar("HostHeader")]
        public string HostHeader { get; set; }
        // 此参数指定网站在创建后是否自动启动
        private bool m_AutoStart = true;
        [TplVar("AutoStart")]
        public bool AutoStart
        {
            get { return m_AutoStart; }
            set { m_AutoStart = value; }
        }
        // 分配的应用程序池
        [TplVar("AppPool")]
        public string AppPool { get; set; }
    }
}
