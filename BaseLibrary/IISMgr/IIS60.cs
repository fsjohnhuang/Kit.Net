using System; 
using System.Collections.Generic;
using System.Text;

using IISMgr.Base;
using lpp.CMDHelper;
using lpp.TplHelper;
using lpp.CollectionHelper;

namespace IISMgr
{
    /// <summary>
    /// 操作IIS6.0的辅助类
    /// </summary>
    /// <remarks>
    /// 参考网站：http://support.microsoft.com/kb/816568/zh-cn
    /// http://technet.microsoft.com/zh-cn/library/cc780607
    /// </remarks>
    public class IIS60 : IIS
    {
        private const string REMOTING_PARAMS = " @Server @User @Pwd ";
        private const string PARAM_PATTERN = "/{0} {1}";

        private readonly string CREATE_WEBSITE_CMD;
        private readonly string DEL_WEBSITE_CMD;
        private readonly string QUERY_WEBSITE_CMD;
        private readonly string START_WEBSITE_CMD;
        private readonly string STOP_WEBSITE_CMD;
        private readonly string PAUSE_WEBSITE_CMD;

        private readonly string CREATE_VDIR_CMD;
        private readonly string DEL_VDIR_CMD;
        private readonly string QUERY_VDIR_CMD;

        public IIS60()
        {
            base.PATH = @"%systemroot%\system32";

            CREATE_WEBSITE_CMD = string.Format("IIsweb {0} /create @Path @SiteName @IPAddress @HostHeader @AutoStart @AppPool", REMOTING_PARAMS);
            DEL_WEBSITE_CMD = string.Format("IIsweb {0} /delete @SiteNames", REMOTING_PARAMS);
            QUERY_WEBSITE_CMD = string.Format("IIsweb {0} /query @SiteName", REMOTING_PARAMS);
            START_WEBSITE_CMD = string.Format("IIsweb {0} /start @SiteNames", REMOTING_PARAMS);
            STOP_WEBSITE_CMD = string.Format("IIsweb {0} /stop @SiteNames", REMOTING_PARAMS);
            PAUSE_WEBSITE_CMD = string.Format("IIsweb {0} /pause @SiteNames", REMOTING_PARAMS);

            CREATE_VDIR_CMD = string.Format("IIsVDir {0} /create @SiteName@VirtualPath @Name @PhysicalPath", REMOTING_PARAMS);
            DEL_VDIR_CMD = string.Format("IIsVDir {0} /delete @SiteName@VirtualPath @Name", REMOTING_PARAMS); 
            QUERY_VDIR_CMD = string.Format("IIsVDir {0} /query @SiteName@VirtualPath ", REMOTING_PARAMS); 
        }

        private string PrecompileRemotingParams(string cmd, AuthorizationConfig4Remoting config)
        {
            return Compiler.Compile<AuthorizationConfig4Remoting>(cmd, config, (attrName, orig) => { 
                if (string.Equals("Server", attrName)) {
                    return string.Format(PARAM_PATTERN, "s", orig.ToString());
                }
                else if (string.Equals("User", attrName)) {
                    return string.Format(PARAM_PATTERN, "u", orig.ToString());
                }
                else if (string.Equals("Pwd", attrName)) {
                    return string.Format(PARAM_PATTERN, "p", orig.ToString());
                }

                return orig;
            });
        }

        #region 网站操作方法

        /// <summary>
        /// 创建网站
        /// </summary>
        /// <param name="config">创建网站的配置信息</param>
        /// <param name="sysDriver">系统分区卷名</param>
        /// <returns></returns>
        public ExecRsp CreateWebsite(WebsiteConfig4Create config, string sysDriver = "c:")
        {
            if (string.IsNullOrEmpty(config.Path))
            {
                return Util.GetRequiredErrRsp("WebsiteConfig4Create.Path");
            }
            if (string.IsNullOrEmpty(config.SiteName))
            {
                return Util.GetRequiredErrRsp("WebsiteConfig4Create.SiteName");
            }

            string cmd = CREATE_WEBSITE_CMD;
            cmd = PrecompileRemotingParams(cmd, config);
            cmd = Compiler.Compile<WebsiteConfig4Create>(cmd, config, (attrName, orig) => {
                if (string.Equals("AutoStart", attrName))
                {
                    return ((bool)orig ? string.Empty : "/dontstart");
                }
                else if (string.Equals("Port", attrName))
                {
                    return string.Format(PARAM_PATTERN, "b", orig.ToString());
                }
                else if (string.Equals("IPAddress", attrName)) {
                    return string.Format(PARAM_PATTERN, "i", orig.ToString());
                }
                else if (string.Equals("HostHeader", attrName)) {
                    return string.Format(PARAM_PATTERN, "d", orig.ToString());
                }
                else if (string.Equals("AppPool", attrName)) {
                    return string.Format(PARAM_PATTERN, "ap", orig.ToString());
                }

                return string.Format("\"{0}\"", orig);
            });

            return Exec(new string[] { cmd }, sysDriver);
        }

        /// <summary>
        /// 删除网站
        /// </summary>
        /// <param name="config">删除网站的配置信息</param>
        /// <param name="sysDriver">系统分区卷名</param>
        /// <returns></returns>
        public ExecRsp DelWebsite(WebsiteConfig4SiteNames config, string sysDriver = "c:")
        {
            if (config.SiteNames == null || config.SiteNames.Length == 0)
            {
                return Util.GetRequiredErrRsp("WebsiteConfig4Del.SiteNames");
            }

            string cmd = DEL_WEBSITE_CMD;
            cmd = PrecompileRemotingParams(cmd, config);
            cmd = Compiler.Compile<WebsiteConfig4SiteNames>(cmd, config, (attrName, orig) =>
            {
                if (string.Equals("SiteNames", attrName)){
                    List<string> siteNameLst = new List<string>((string[])orig);
                    siteNameLst = Lst.Map<string>(siteNameLst, (origVal) => {
                        return string.Format("\"{0}\"", origVal);
                    });
                    return Lst.Join<string>(siteNameLst, false, " ");
                }

                return orig;
            });

            return Exec(new string[] { cmd }, sysDriver);
        }

        /// <summary>
        /// 查询网站信息
        /// </summary>
        /// <param name="config">查询网站的配置信息</param>
        /// <param name="sysDriver">系统分区卷名</param>
        /// <returns></returns>
        public ExecRsp QueryWebsite(WebsiteConfig4Query config, string sysDriver = "c:")
        {
            string cmd = DEL_WEBSITE_CMD;
            cmd = PrecompileRemotingParams(cmd, config);
            cmd = Compiler.Compile<WebsiteConfig4Query>(cmd, config, (attrName, orig) =>
            {
                if (string.Equals("SiteName", attrName))
                {
                    return string.Format("\"{0}\"", orig);
                }

                return orig;
            });

            return Exec(new string[] { cmd }, sysDriver);
        }

        /// <summary>
        /// 启动网站
        /// </summary>
        /// <param name="config">启动网站的配置信息</param>
        /// <param name="sysDriver">系统分区卷名</param>
        /// <returns></returns>
        public ExecRsp StartWebsite(WebsiteConfig4SiteNames config, string sysDriver = "c:")
        {
            if (config.SiteNames == null || config.SiteNames.Length == 0)
            {
                return Util.GetRequiredErrRsp("WebsiteConfig4Del.SiteNames");
            }

            string cmd = START_WEBSITE_CMD;
            cmd = PrecompileRemotingParams(cmd, config);
            cmd = Compiler.Compile<WebsiteConfig4SiteNames>(cmd, config, (attrName, orig) =>
            {
                if (string.Equals("SiteNames", attrName))
                {
                    List<string> siteNameLst = new List<string>((string[])orig);
                    siteNameLst = Lst.Map<string>(siteNameLst, (origVal) =>
                    {
                        return string.Format("\"{0}\"", origVal);
                    });
                    return Lst.Join<string>(siteNameLst, false, " ");
                }

                return orig;
            });

            return Exec(new string[] { cmd }, sysDriver);
        }

        /// <summary>
        /// 停止网站
        /// </summary>
        /// <param name="config">停止网站的配置信息</param>
        /// <param name="sysDriver">系统分区卷名</param>
        /// <returns></returns>
        public ExecRsp StopWebsite(WebsiteConfig4SiteNames config, string sysDriver = "c:")
        {
            if (config.SiteNames == null || config.SiteNames.Length == 0)
            {
                return Util.GetRequiredErrRsp("WebsiteConfig4Del.SiteNames");
            }

            string cmd = STOP_WEBSITE_CMD;
            cmd = PrecompileRemotingParams(cmd, config);
            cmd = Compiler.Compile<WebsiteConfig4SiteNames>(cmd, config, (attrName, orig) =>
            {
                if (string.Equals("SiteNames", attrName))
                {
                    List<string> siteNameLst = new List<string>((string[])orig);
                    siteNameLst = Lst.Map<string>(siteNameLst, (origVal) =>
                    {
                        return string.Format("\"{0}\"", origVal);
                    });
                    return Lst.Join<string>(siteNameLst, false, " ");
                }

                return orig;
            });

            return Exec(new string[] { cmd }, sysDriver);
        }

        /// <summary>
        /// 暂停网站
        /// </summary>
        /// <param name="config">暂停网站的配置信息</param>
        /// <param name="sysDriver">系统分区卷名</param>
        /// <returns></returns>
        public ExecRsp PauseWebsite(WebsiteConfig4SiteNames config, string sysDriver = "c:")
        {
            if (config.SiteNames == null || config.SiteNames.Length == 0)
            {
                return Util.GetRequiredErrRsp("WebsiteConfig4Del.SiteNames");
            }

            string cmd = PAUSE_WEBSITE_CMD;
            cmd = PrecompileRemotingParams(cmd, config);
            cmd = Compiler.Compile<WebsiteConfig4SiteNames>(cmd, config, (attrName, orig) =>
            {
                if (string.Equals("SiteNames", attrName))
                {
                    List<string> siteNameLst = new List<string>((string[])orig);
                    siteNameLst = Lst.Map<string>(siteNameLst, (origVal) =>
                    {
                        return string.Format("\"{0}\"", origVal);
                    });
                    return Lst.Join<string>(siteNameLst, false, " ");
                }

                return orig;
            });

            return Exec(new string[] { cmd }, sysDriver);
        }

        #endregion

        #region Web虚拟目录操作方法

        /// <summary>
        /// 创建Web虚拟目录
        /// </summary>
        /// <param name="config">创建Web虚拟目录的配置信息</param>
        /// <param name="sysDriver">系统分区卷名</param>
        /// <returns></returns>
        public ExecRsp CreateVDir(VDirConfig4Create config, string sysDriver = "c:")
        {
            if (string.IsNullOrEmpty(config.SiteName))
            {
                return Util.GetRequiredErrRsp("VDirConfig4Create.SiteName");
            }
            if (string.IsNullOrEmpty(config.Name))
            {
                return Util.GetRequiredErrRsp("VDirConfig4Create.Name");
            }
            if (string.IsNullOrEmpty(config.PhysicalPath))
            {
                return Util.GetRequiredErrRsp("VDirConfig4Create.PhysicalPath");
            }

            string cmd = CREATE_VDIR_CMD;
            cmd = PrecompileRemotingParams(cmd, config);
            cmd = Compiler.Compile<VDirConfig4Create>(cmd, config, (attrName, orig) =>
            {
                if (string.Equals("SiteName", attrName))
                {
                    return string.Format("\"{0}\"", orig.ToString());
                }

                return orig;
            });

            return Exec(new string[] { cmd }, sysDriver);
        }

        /// <summary>
        /// 删除Web虚拟目录
        /// </summary>
        /// <param name="config">删除Web虚拟目录的配置信息</param>
        /// <param name="sysDriver">系统分区卷名</param>
        /// <returns></returns>
        public ExecRsp DelVDir(VDirConfig4Del config, string sysDriver = "c:")
        {
            if (string.IsNullOrEmpty(config.SiteName))
            {
                return Util.GetRequiredErrRsp("VDirConfig4Create.SiteName");
            }
            if (string.IsNullOrEmpty(config.Name))
            {
                return Util.GetRequiredErrRsp("VDirConfig4Create.Name");
            }

            string cmd = DEL_VDIR_CMD;
            cmd = PrecompileRemotingParams(cmd, config);
            cmd = Compiler.Compile<VDirConfig4Del>(cmd, config, (attrName, orig) =>
            {
               if (string.Equals("SiteName", attrName))
                {
                    return string.Format("\"{0}\"", orig);
                }

                return orig;
            });

            return Exec(new string[] { cmd }, sysDriver);
        }

        /// <summary>
        /// 查询Web虚拟目录
        /// </summary>
        /// <param name="config">查询Web虚拟目录的配置信息</param>
        /// <param name="sysDriver">系统分区卷名</param>
        /// <returns></returns>
        public ExecRsp QueryVDir(VDirConfig4Query config, string sysDriver = "c:")
        {
            if (string.IsNullOrEmpty(config.SiteName))
            {
                return Util.GetRequiredErrRsp("VDirConfig4Create.SiteName");
            }

            string cmd = QUERY_VDIR_CMD;
            cmd = PrecompileRemotingParams(cmd, config);
            cmd = Compiler.Compile<VDirConfig4Query>(cmd, config, (attrName, orig) =>
            {
               if (string.Equals("SiteName", attrName))
                {
                    return string.Format("\"{0}\"", orig);
                }

                return orig;
            });

            return Exec(new string[] { cmd }, sysDriver);
        }

        #endregion
    }
}
