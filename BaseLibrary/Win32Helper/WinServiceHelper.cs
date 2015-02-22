using System.ServiceProcess;
using System.Configuration.Install;
using System.Collections;
using Microsoft.Win32;
using System;

namespace lpp.Win32Helper
{
    public static class WinServiceHelper
    {
        /// <summary>
        /// 通过服务名称判断服务是否已安装
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public static bool IsExist(string serviceName)
        {
            bool isExist = false;
            ServiceController[] services = ServiceController.GetServices();
            for (int i = 0, len = services.Length; i < len && !isExist; ++i)
                isExist = string.Equals(services[i].ServiceName, serviceName);

            return isExist;
        }

        /// <summary>
        /// 通过显示名称判断服务是否已安装
        /// </summary>
        /// <param name="displayName">服务名称</param>
        /// <returns></returns>
        public static bool IsExistByDisplayName(string displayName)
        {
            bool isExist = false;
            ServiceController[] services = ServiceController.GetServices();
            for (int i = 0, len = services.Length; i < len && !isExist; ++i)
                isExist = string.Equals(services[i].DisplayName, displayName);

            return isExist;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void Start(string serviceName)
        {
            if (!IsExist(serviceName)) return;

            ServiceController service = new ServiceController(serviceName);
            try
            {
                if (service.Status == ServiceControllerStatus.Stopped || service.Status == ServiceControllerStatus.StopPending)
                {
                    service.Start();
                    service.Refresh();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                service.Dispose();
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void Stop(string serviceName)
        {
            if (!IsExist(serviceName)) return;

            ServiceController service = new ServiceController(serviceName);
            try
            {
                if (service.CanStop && service.Status == ServiceControllerStatus.Running || service.Status == ServiceControllerStatus.Paused
                    || service.Status == ServiceControllerStatus.PausePending || service.Status == ServiceControllerStatus.ContinuePending
                    || service.Status == ServiceControllerStatus.StartPending)
                {
                    service.Stop();
                    service.Refresh();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                service.Dispose();
            }
        }

        /// <summary>
        /// 暂停服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void Pause(string serviceName)
        {
            if (!IsExist(serviceName)) return;

            ServiceController service = new ServiceController(serviceName);
            try
            {
                if (service.CanPauseAndContinue && service.Status == ServiceControllerStatus.Running)
                {
                    service.Pause();
                    service.Refresh();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                service.Dispose();
            }
        }

        /// <summary>
        /// 恢复服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void Continue(string serviceName)
        {
            if (!IsExist(serviceName)) return;

            ServiceController service = new ServiceController(serviceName);
            try
            {
                if (service.CanPauseAndContinue && (service.Status == ServiceControllerStatus.Paused || service.Status == ServiceControllerStatus.PausePending))
                {
                    service.Continue();
                    service.Refresh();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                service.Dispose();
            }
        }

        /// <summary>
        /// 获取服务的程序路径
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public static string GetExePath(string serviceName)
        {
            string exePath = string.Empty;
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\ControlSet001\services\" + serviceName);
            if (key == null) return exePath;

            object imagePath = key.GetValue("ImagePath");
            if (imagePath != null)
            {
                exePath = imagePath.ToString();
                exePath = exePath.Trim('"');
            }
            key.Close();

            return exePath;
        }

        /// <summary>
        /// 安装服务
        /// </summary>
        /// <param name="exePath">程序路径</param>
        /// <param name="serviceName">服务名称</param>
        public static void Install(string exePath, string serviceName)
        {
            if (IsExist(serviceName)) return;

            IDictionary state = new Hashtable();
            AssemblyInstaller installer = new AssemblyInstaller(exePath, new string[] { });
            try
            {
                installer.UseNewContext = true;
                installer.Install(state);
                installer.Commit(state);
            }
            finally
            {
                installer.Dispose();
            }
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void Unintall(string serviceName)
        {
            if (!IsExist(serviceName)) return;

            string exePath = GetExePath(serviceName);
            if (string.IsNullOrEmpty(exePath)) return;
            AssemblyInstaller installer = new AssemblyInstaller();
            try
            {
                IDictionary state = new Hashtable();

                installer.Path = exePath;
                installer.UseNewContext = true;
                installer.Uninstall(state);
                installer.Commit(state);
            }
            catch (Exception e)
            {
            }
            finally
            {
                installer.Dispose();
            }
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <param name="exePath">服务程序路径</param>
        public static void UnistallByPath(string exePath)
        {
            if (string.IsNullOrEmpty(exePath)) return;

            AssemblyInstaller installer = new AssemblyInstaller();
            try
            {
                IDictionary state = new Hashtable();
                installer.Path = exePath;
                installer.UseNewContext = true;
                installer.Uninstall(state);
                installer.Commit(state);
            }
            catch (Exception e)
            {

            }
            finally
            {
                installer.Dispose();
            }
        }

        /// <summary>
        /// 设置服务启动类型
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="winServiceLaunchMode">启动类型</param>
        public static void SetLaunchType(string serviceName, WinServiceLaunchType winServiceLaunchType)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\ControlSet001\services\" + serviceName, true);
            if (winServiceLaunchType == WinServiceLaunchType.NONE)
            {
                return;
            }
            else if (winServiceLaunchType == WinServiceLaunchType.AUTOMATIC_DELAY)
            {
                key.SetValue("Start", 2);
                string[] valueNames = key.GetValueNames();
                string DelayedAutoStart = "DelayedAutoStart";
                key.SetValue(DelayedAutoStart, 1);
            }
            else
            {
                key.SetValue("Start", (int)winServiceLaunchType);
            }
            key.Close();
        }

        /// <summary>
        /// 获取服务启动类型
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static WinServiceLaunchType GetLaunchType(string serviceName)
        {
            if (!IsExist(serviceName)) return WinServiceLaunchType.NONE;

            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\ControlSet001\services\" + serviceName);
            object startObj = key.GetValue("Start");
            key.Close();
            if (null == startObj) return WinServiceLaunchType.NONE;

            WinServiceLaunchType winServiceLaunchType = (WinServiceLaunchType)Enum.Parse(typeof(WinServiceLaunchType), startObj.ToString());

            return winServiceLaunchType;
        }

        /// <summary>
        /// 获取运行状态
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public static ServiceControllerStatus GetStatus(string serviceName)
        {
            return new ServiceController(serviceName).Status;
        }

        /// <summary>
        /// 检查服务是否运行中
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="justStatusisRunning">
        /// true表示服务状态必须为运行时才返回true
        /// false表示服务状态为启动中、恢复中均返回true
        /// </param>
        /// <returns></returns>
        public static bool IsRunning(string serviceName, bool justStatusisRunning = false) 
        {
            ServiceControllerStatus status = GetStatus(serviceName);
            return (justStatusisRunning ? status == ServiceControllerStatus.Running 
                : status == ServiceControllerStatus.Running || status == ServiceControllerStatus.ContinuePending || status == ServiceControllerStatus.StartPending);
        }
    }

    /// <summary>
    /// windows服务启动类型
    /// </summary>
    public enum WinServiceLaunchType : int
    {
        NONE = 0, // 无效值
        AUTOMATIC_DELAY = 1, // 自动（延迟启动）
        AUTOMATIC = 2, // 自动
        MANUL = 3, // 手动
        DISABLE = 4 // 禁用
    }
}
