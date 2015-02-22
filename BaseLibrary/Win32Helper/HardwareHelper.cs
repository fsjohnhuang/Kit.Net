using lpp.EnumHelper;
using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace lpp.Win32Helper
{
    public sealed class HardwareHelper
    {
        #region CPU
        /// <summary>
        /// 得到cpu信息 
        /// </summary>
        /// <param name="cpuProperty">cpu信息属性</param>
        /// <returns></returns>
        public static string GetCpuInfo(CpuProperty cpuProperty)
        {
            string _cpuInfo = "";
            ManagementClass cimobject = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                _cpuInfo = mo.Properties[cpuProperty.Str].Value.ToString();
            }
            return _cpuInfo;
        }
        #endregion

        #region Memory
        /// <summary>
        /// 得到内存设备信息 
        /// </summary>
        /// <param name="memDeviceProperty">mem设备信息属性</param>
        /// <returns></returns>
        public static string GetMemoryDeviceInfo(MemDeviceProperty memDeviceProperty)
        {
            string _memInfo = "";
            ManagementClass cimobject = new ManagementClass("Win32_MemoryDevice");
            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                _memInfo = mo.Properties[memDeviceProperty.Str].Value.ToString();
            }
            return _memInfo;
        }

        /// <summary>
        /// 获取内存总容量（单位是Byte）
        /// </summary>
        /// <returns></returns>
        public static long GetMemorySize()
        {
            long size = 0;
            StringBuilder s = new StringBuilder();
            ManagementClass cimobject = new ManagementClass("Win32_PhysicalMemory");
            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                size += long.Parse(mo.Properties["Capacity"].Value.ToString());
            }

            return size;
        }

        /// <summary>
        /// 获取物理内存信息
        /// </summary>
        /// <param name="memProperty">物理内存属性</param>
        /// <param name="seperator">分隔符，默认是,</param>
        /// <returns></returns>
        public static string GetPhysicalMemory(MemProperty memProperty, string seperator = ",")
        {
            StringBuilder info = new StringBuilder();
            ManagementClass cimobject = new ManagementClass("Win32_PhysicalMemory");
            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                info.Append(mo.Properties[memProperty.Str].Value.ToString() + seperator);
            }
            if (info.Length > 0)
            {
                info.Remove(info.Length - 1, 1);
            }

            return info.ToString();
        }
        #endregion

        #region Hard Disk
        /// <summary>
        /// 获取磁盘驱动器信息
        /// </summary>
        /// <param name="diskDriverProperty">磁盘驱动器信息属性</param>
        /// <returns></returns>
        public static string GetDiskDriverInfo(DiskDriverProperty diskDriverProperty)
        {
            string _diskDriveInfo = "";
            ManagementClass cimobject = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                _diskDriveInfo = mo.Properties[diskDriverProperty.Str].Value.ToString();
            }
            return _diskDriveInfo;
        }

        // 得到硬盘序列号
        public static string GetHDSerialNumber()
        {
            string _HDInfo = "";
            ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc1 = cimobject1.GetInstances();
            foreach (ManagementObject mo in moc1)
            {
                _HDInfo = (string)mo.Properties["Model"].Value;


            }
            return _HDInfo;
        }

        /// <summary>
        /// 获取分区总容量(单位：byte)
        /// </summary>
        /// <param name="logicalDiskName">分区名称,如c:</param>
        /// <returns></returns>
        public static long GetLogicalDiskSize(string logicalDiskName)
        {
            ManagementObject disk = new ManagementObject(string.Format("win32_logicaldisk.deviceid=\"{0}\"", logicalDiskName));
            disk.Get();
            return Convert.ToInt64(disk["size"]);
        }

        /// <summary>
        /// 获取所有分区信息
        /// </summary>
        /// <returns></returns>
        public static List<DiskInfo> GetDiskInfos()
        {
            List<DiskInfo> diskInfos = new List<DiskInfo>();
            SelectQuery qry = new SelectQuery("SELECT * FROM Win32_LogicalDisk");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(qry);
            foreach (ManagementObject disk in searcher.Get())
            {
                diskInfos.Add(new DiskInfo()
                {
                    Name = disk["Name"].ToString()
                    ,
                    VolName = disk["VolumeName"].ToString()
                    ,
                    HDType = (HDType)Enum.Parse(typeof(HDType), disk["DriveType"].ToString())
                    ,
                    Size = Convert.ToInt64(disk["Size"].ToString())
                });
            }

            return diskInfos;
        }
        #endregion

        #region Mainboard
        // 主板制造商
        public static string GetMainboardManufacturer()
        {
            string manufacturer = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            foreach (ManagementObject share in searcher.Get())
            {
                manufacturer = share["Manufacturer"].ToString();
                break;
            }

            return manufacturer;
        }

        // 主板型号
        public static string GetMainboardProduct()
        {
            string product = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            foreach (ManagementObject share in searcher.Get())
            {
                product = share["Product"].ToString();
                break;
            }

            return product;
        }

        // 主板序列号
        public static string GetMainboardSerialNumber()
        {
            string serialNumber = string.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            foreach (ManagementObject share in searcher.Get())
            {
               serialNumber = share["SerialNumber"].ToString();
               break;
            }

            return serialNumber;
        }
        #endregion

        #region 显卡、声卡
        // 获取显卡名称
        public static string GetVideoName()
        {
            string name = string.Empty;
            ManagementObjectSearcher videoControllers = new ManagementObjectSearcher("select * from Win32_VideoController");
            foreach (ManagementObject videoController in videoControllers.Get())
            {
                name = videoController["Caption"].ToString();
                break;
            }

            return name;
        }

        #endregion

        #region Net
        /// <summary>
        /// 获取IPv4地址
        /// </summary>
        /// <returns></returns>
        public static string[] GetIPv4()
        {
            List<string> IPv4s = new List<string>();
            IPAddress[] ipAddrs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ipAddr in ipAddrs)
            {
                if (ipAddr.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPv4s.Add(ipAddr.ToString());
                }
            }

            return IPv4s.ToArray();
        }

        //获取网卡硬件地址
        public static string GetMacAddr()
        {
            string _MacAddress = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc2 = mc.GetInstances();
            foreach (ManagementObject mo in moc2)
            {
                if ((bool)mo["IPEnabled"] == true)
                    _MacAddress = mo["MacAddress"].ToString();
                mo.Dispose();
            }

            return _MacAddress;
        }
        #endregion

        #region 共享资源
        /// <summary>
        /// 获取共享资源
        /// </summary>
        /// <returns></returns>
        public static List<string> GetShare()
        {
            List<string> shares = new List<string>();
            SelectQuery qry = new SelectQuery("SELECT * FROM Win32_Share");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(qry);
            foreach (ManagementObject share in searcher.Get())
            {
                shares.Add(share.GetText(TextFormat.Mof));
            }

            return shares;
        }

        /// <summary>
        /// 新增共享资源（要有相关权限才能开通）
        /// </summary>
        /// <param name="shares">资源数组，如{"C:\\Temp","我的共享",0,10,"Dot Net 实现的共享",""}</param>
        /// <returns></returns>
        public static bool AddShare(object[] shares)
        {
            bool success = false;
            try
            {
                ManagementClass cls = new ManagementClass(new ManagementPath("Win32_Share"));
                cls.InvokeMethod("create", shares);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return success;
        }
        #endregion
    }

    /// <summary>
    /// 分区种类
    /// </summary>
    public enum HDType : int
    {
        NOT_TYPE = 1,
        FLOPPY_DISK= 2,
        REMOVABLE_DRIVE_OR_NETWORK_DRIVE = 3,
        CD_ROM = 4,
        RAM_DISK = 5
    }

    /// <summary>
    /// 分区信息
    /// </summary>
    public class DiskInfo
    {
        public string Name { get; set; }
        public HDType HDType { get; set; }
        public string VolName { get; set; }
        public long Size { get; set; }
    }

    /// <summary>
    /// cpu信息属性枚举类
    /// </summary>
    public sealed class CpuProperty : BaseEnum
    {
        public readonly static CpuProperty AddressWidth = new CpuProperty(0, "AddressWidth");
        public readonly static CpuProperty Architecture = new CpuProperty(1, "Architecture");
        public readonly static CpuProperty Availability = new CpuProperty(2, "Availability");
        public readonly static CpuProperty Caption = new CpuProperty(3, "Caption");
        public readonly static CpuProperty ConfigManagerErrorCode = new CpuProperty(4, "ConfigManagerErrorCode");
        public readonly static CpuProperty ConfigManagerUserConfig = new CpuProperty(5, "ConfigManagerUserConfig");
        public readonly static CpuProperty CpuStatus = new CpuProperty(6, "CpuStatus");
        public readonly static CpuProperty CreationClassName = new CpuProperty(7, "CreationClassName");
        public readonly static CpuProperty CurrentClockSpeed = new CpuProperty(8, "CurrentClockSpeed");
        public readonly static CpuProperty CurrentVoltage = new CpuProperty(9, "CurrentVoltage");
        public readonly static CpuProperty DataWidth = new CpuProperty(10, "DataWidth");
        public readonly static CpuProperty Description = new CpuProperty(11, "Description");
        public readonly static CpuProperty DeviceID = new CpuProperty(12, "DeviceID");
        public readonly static CpuProperty ErrorCleared = new CpuProperty(13, "ErrorCleared");
        public readonly static CpuProperty ErrorDescription = new CpuProperty(14, "ErrorDescription");
        public readonly static CpuProperty ExtClock = new CpuProperty(15, "ExtClock");
        public readonly static CpuProperty Family = new CpuProperty(16, "Family");
        public readonly static CpuProperty InstallDate = new CpuProperty(17, "InstallDate");
        public readonly static CpuProperty L2CacheSize = new CpuProperty(18, "L2CacheSize");
        public readonly static CpuProperty L2CacheSpeed = new CpuProperty(19, "L2CacheSpeed");
        public readonly static CpuProperty L3CacheSize = new CpuProperty(20, "L3CacheSize");
        public readonly static CpuProperty L3CacheSpeed = new CpuProperty(21, "L3CacheSpeed");
        public readonly static CpuProperty LastErrorCode = new CpuProperty(22, "LastErrorCode");
        public readonly static CpuProperty Level = new CpuProperty(23, "Level");
        public readonly static CpuProperty LoadPercentage = new CpuProperty(24, "LoadPercentage");
        public readonly static CpuProperty Manufacturer = new CpuProperty(25, "Manufacturer");
        public readonly static CpuProperty MaxClockSpeed = new CpuProperty(26, "MaxClockSpeed");
        public readonly static CpuProperty Name = new CpuProperty(27, "Name");
        public readonly static CpuProperty NumberOfCores = new CpuProperty(28, "NumberOfCores");
        public readonly static CpuProperty NumberOfLogicalProcessors = new CpuProperty(29, "NumberOfLogicalProcessors");
        public readonly static CpuProperty OtherFamilyDescription = new CpuProperty(30, "OtherFamilyDescription");
        public readonly static CpuProperty PNPDeviceID = new CpuProperty(31, "PNPDeviceID");
        public readonly static CpuProperty PowerManagementCapabilities = new CpuProperty(32, "PowerManagementCapabilities");
        public readonly static CpuProperty PowerManagementSupported = new CpuProperty(33, "PowerManagementSupported");
        public readonly static CpuProperty ProcessorId = new CpuProperty(34, "ProcessorId");
        public readonly static CpuProperty ProcessorType = new CpuProperty(35, "ProcessorType");
        public readonly static CpuProperty Revision = new CpuProperty(36, "Revision");
        public readonly static CpuProperty Role = new CpuProperty(37, "Role");
        public readonly static CpuProperty SocketDesignation = new CpuProperty(38, "SocketDesignation");
        public readonly static CpuProperty Status = new CpuProperty(39, "Status");
        public readonly static CpuProperty StatusInfo = new CpuProperty(40, "StatusInfo");
        public readonly static CpuProperty Stepping = new CpuProperty(41, "Stepping");
        public readonly static CpuProperty SystemCreationClassName = new CpuProperty(42, "SystemCreationClassName");
        public readonly static CpuProperty SystemName = new CpuProperty(43, "SystemName");
        public readonly static CpuProperty UniqueId = new CpuProperty(44, "UniqueId");
        public readonly static CpuProperty UpgradeMethod = new CpuProperty(45, "UpgradeMethod");
        public readonly static CpuProperty Version = new CpuProperty(46, "Version");
        public readonly static CpuProperty VoltageCaps = new CpuProperty(47, "VoltageCaps");

        private CpuProperty(int value, string str) : base(value, str) { }
    }

    /// <summary>
    /// 内存设备信息属性枚举类
    /// </summary>
    public sealed class MemDeviceProperty : BaseEnum
    {
        public readonly static MemDeviceProperty Access = new MemDeviceProperty(0, "Access");
        public readonly static MemDeviceProperty AdditionalErrorData = new MemDeviceProperty(1, "AdditionalErrorData");
        public readonly static MemDeviceProperty Availability = new MemDeviceProperty(2, "Availability");
        public readonly static MemDeviceProperty BlockSize = new MemDeviceProperty(3, "BlockSize");
        public readonly static MemDeviceProperty Caption = new MemDeviceProperty(4, "Caption");
        public readonly static MemDeviceProperty ConfigManagerErrorCode = new MemDeviceProperty(5, "ConfigManagerErrorCode");
        public readonly static MemDeviceProperty ConfigManagerUserConfig = new MemDeviceProperty(6, "ConfigManagerUserConfig");
        public readonly static MemDeviceProperty CorrectableError = new MemDeviceProperty(7, "CorrectableError");
        public readonly static MemDeviceProperty CreationClassName = new MemDeviceProperty(8, "CreationClassName");
        public readonly static MemDeviceProperty Description = new MemDeviceProperty(9, "Description");
        public readonly static MemDeviceProperty DeviceID = new MemDeviceProperty(10, "DeviceID");
        public readonly static MemDeviceProperty EndingAddress = new MemDeviceProperty(11, "EndingAddress");
        public readonly static MemDeviceProperty ErrorAccess = new MemDeviceProperty(12, "ErrorAccess");
        public readonly static MemDeviceProperty ErrorAddress = new MemDeviceProperty(13, "ErrorAddress");
        public readonly static MemDeviceProperty ErrorCleared = new MemDeviceProperty(14, "ErrorCleared");
        public readonly static MemDeviceProperty ErrorData = new MemDeviceProperty(15, "ErrorData");
        public readonly static MemDeviceProperty ErrorDataOrder = new MemDeviceProperty(16, "ErrorDataOrder");
        public readonly static MemDeviceProperty ErrorDescription = new MemDeviceProperty(17, "ErrorDescription");
        public readonly static MemDeviceProperty ErrorGranularity = new MemDeviceProperty(18, "ErrorGranularity");
        public readonly static MemDeviceProperty ErrorInfo = new MemDeviceProperty(19, "ErrorInfo");
        public readonly static MemDeviceProperty ErrorMethodology = new MemDeviceProperty(20, "ErrorMethodology");
        public readonly static MemDeviceProperty ErrorResolution = new MemDeviceProperty(21, "ErrorResolution");
        public readonly static MemDeviceProperty ErrorTime = new MemDeviceProperty(22, "ErrorTime");
        public readonly static MemDeviceProperty ErrorTransferSize = new MemDeviceProperty(23, "ErrorTransferSize");
        public readonly static MemDeviceProperty InstallDate = new MemDeviceProperty(24, "InstallDate");
        public readonly static MemDeviceProperty LastErrorCode = new MemDeviceProperty(25, "LastErrorCode");
        public readonly static MemDeviceProperty Name = new MemDeviceProperty(26, "Name");
        public readonly static MemDeviceProperty NumberOfBlocks = new MemDeviceProperty(27, "NumberOfBlocks");
        public readonly static MemDeviceProperty OtherErrorDescription = new MemDeviceProperty(28, "OtherErrorDescription");
        public readonly static MemDeviceProperty PNPDeviceID = new MemDeviceProperty(29, "PNPDeviceID");
        public readonly static MemDeviceProperty PowerManagementCapabilities = new MemDeviceProperty(30, "PowerManagementCapabilities");
        public readonly static MemDeviceProperty PowerManagementSupported = new MemDeviceProperty(31, "PowerManagementSupported");
        public readonly static MemDeviceProperty Purpose = new MemDeviceProperty(32, "Purpose");
        public readonly static MemDeviceProperty StartingAddress = new MemDeviceProperty(33, "StartingAddress");
        public readonly static MemDeviceProperty Status = new MemDeviceProperty(34, "Status");
        public readonly static MemDeviceProperty StatusInfo = new MemDeviceProperty(35, "StatusInfo");
        public readonly static MemDeviceProperty SystemCreationClassName = new MemDeviceProperty(36, "SystemCreationClassName");
        public readonly static MemDeviceProperty SystemLevelAddress = new MemDeviceProperty(37, "SystemLevelAddress");
        public readonly static MemDeviceProperty SystemName = new MemDeviceProperty(38, "SystemName");

        private MemDeviceProperty(int value, string str) : base(value, str) { }
    }

    /// <summary>
    /// 磁盘驱动器信息枚举
    /// </summary>
    public sealed class DiskDriverProperty : BaseEnum
    {
        public readonly static DiskDriverProperty Availability = new DiskDriverProperty(0, "Availability");
        public readonly static DiskDriverProperty BytesPerSector = new DiskDriverProperty(0, "BytesPerSector");
        public readonly static DiskDriverProperty Capabilities = new DiskDriverProperty(0, "Capabilities");
        public readonly static DiskDriverProperty CapabilityDescriptions = new DiskDriverProperty(0, "CapabilityDescriptions");
        public readonly static DiskDriverProperty Caption = new DiskDriverProperty(0, "Caption");
        public readonly static DiskDriverProperty CompressionMethod = new DiskDriverProperty(0, "Availability");
        public readonly static DiskDriverProperty ConfigManagerErrorCode = new DiskDriverProperty(0, "ConfigManagerErrorCode");
        public readonly static DiskDriverProperty ConfigManagerUserConfig = new DiskDriverProperty(0, "ConfigManagerUserConfig");
        public readonly static DiskDriverProperty CreationClassName = new DiskDriverProperty(0, "CreationClassName");
        public readonly static DiskDriverProperty DefaultBlockSize = new DiskDriverProperty(0, "DefaultBlockSize");
        public readonly static DiskDriverProperty Description = new DiskDriverProperty(0, "Description");
        public readonly static DiskDriverProperty DeviceID = new DiskDriverProperty(0, "DeviceID");
        public readonly static DiskDriverProperty ErrorCleared = new DiskDriverProperty(0, "ErrorCleared");
        public readonly static DiskDriverProperty ErrorDescription = new DiskDriverProperty(0, "ErrorDescription");
        public readonly static DiskDriverProperty ErrorMethodology = new DiskDriverProperty(0, "ErrorMethodology");
        public readonly static DiskDriverProperty FirmwareRevision = new DiskDriverProperty(0, "FirmwareRevision");
        public readonly static DiskDriverProperty Index = new DiskDriverProperty(0, "Index");
        public readonly static DiskDriverProperty InstallDate = new DiskDriverProperty(0, "InstallDate");
        public readonly static DiskDriverProperty InterfaceType = new DiskDriverProperty(0, "InterfaceType");
        public readonly static DiskDriverProperty LastErrorCode = new DiskDriverProperty(0, "LastErrorCode");
        public readonly static DiskDriverProperty Manufacturer = new DiskDriverProperty(0, "Manufacturer");
        public readonly static DiskDriverProperty MaxBlockSize = new DiskDriverProperty(0, "MaxBlockSize");
        public readonly static DiskDriverProperty MaxMediaSize = new DiskDriverProperty(0, "MaxMediaSize");
        public readonly static DiskDriverProperty MediaLoaded = new DiskDriverProperty(0, "MediaLoaded");
        public readonly static DiskDriverProperty MediaType = new DiskDriverProperty(0, "MediaType");
        public readonly static DiskDriverProperty MinBlockSize = new DiskDriverProperty(0, "MinBlockSize");
        public readonly static DiskDriverProperty Model = new DiskDriverProperty(0, "Model");
        public readonly static DiskDriverProperty Name = new DiskDriverProperty(0, "Name");
        public readonly static DiskDriverProperty NeedsCleaning = new DiskDriverProperty(0, "NeedsCleaning");
        public readonly static DiskDriverProperty NumberOfMediaSupported = new DiskDriverProperty(0, "NumberOfMediaSupported");
        public readonly static DiskDriverProperty Partitions = new DiskDriverProperty(0, "Partitions");
        public readonly static DiskDriverProperty PNPDeviceID = new DiskDriverProperty(0, "PNPDeviceID");
        public readonly static DiskDriverProperty PowerManagementCapabilities = new DiskDriverProperty(0, "PowerManagementCapabilities");
        public readonly static DiskDriverProperty PowerManagementSupported = new DiskDriverProperty(0, "PowerManagementSupported");
        public readonly static DiskDriverProperty SCSIBus = new DiskDriverProperty(0, "SCSIBus");
        public readonly static DiskDriverProperty SCSILogicalUnit = new DiskDriverProperty(0, "SCSILogicalUnit");
        public readonly static DiskDriverProperty SCSIPort = new DiskDriverProperty(0, "SCSIPort");
        public readonly static DiskDriverProperty SCSITargetId = new DiskDriverProperty(0, "SCSITargetId");
        public readonly static DiskDriverProperty SectorsPerTrack = new DiskDriverProperty(0, "SectorsPerTrack");
        public readonly static DiskDriverProperty SerialNumber = new DiskDriverProperty(0, "SerialNumber");
        public readonly static DiskDriverProperty Signature = new DiskDriverProperty(0, "Signature");
        public readonly static DiskDriverProperty Size = new DiskDriverProperty(0, "Size");
        public readonly static DiskDriverProperty Status = new DiskDriverProperty(0, "Status");
        public readonly static DiskDriverProperty StatusInfo = new DiskDriverProperty(0, "StatusInfo");
        public readonly static DiskDriverProperty SystemCreationClassName = new DiskDriverProperty(0, "SystemCreationClassName");
        public readonly static DiskDriverProperty SystemName = new DiskDriverProperty(0, "SystemName");
        public readonly static DiskDriverProperty TotalCylinders = new DiskDriverProperty(0, "TotalCylinders");
        public readonly static DiskDriverProperty TotalHeads = new DiskDriverProperty(0, "TotalHeads");
        public readonly static DiskDriverProperty TotalSectors = new DiskDriverProperty(0, "TotalSectors");
        public readonly static DiskDriverProperty TotalTracks = new DiskDriverProperty(0, "TotalTracks");
        public readonly static DiskDriverProperty TracksPerCylinder = new DiskDriverProperty(0, "TracksPerCylinder");

        private DiskDriverProperty(int value, string str) : base(value, str) { }

    }

    /// <summary>
    /// 物理内存信息属性枚举类
    /// </summary>
    public sealed class MemProperty : BaseEnum
    {
        public readonly static MemProperty BankLabel = new MemProperty(0, "BankLabel");
        public readonly static MemProperty Capacity = new MemProperty(1, "Capacity");
        public readonly static MemProperty Caption = new MemProperty(2, "Caption");
        public readonly static MemProperty CreationClassName = new MemProperty(3, "CreationClassName");
        public readonly static MemProperty DataWidth = new MemProperty(4, "DataWidth");
        public readonly static MemProperty Description = new MemProperty(5, "Description");
        public readonly static MemProperty DeviceLocator = new MemProperty(6, "DeviceLocator");
        public readonly static MemProperty FormFactor = new MemProperty(7, "FormFactor");
        public readonly static MemProperty HotSwappable = new MemProperty(8, "HotSwappable");
        public readonly static MemProperty InstallDate = new MemProperty(9, "InstallDate");
        public readonly static MemProperty InterleaveDataDepth = new MemProperty(10, "InterleaveDataDepth");
        public readonly static MemProperty InterleavePosition = new MemProperty(11, "InterleavePosition");
        public readonly static MemProperty Manufacturer = new MemProperty(12, "Manufacturer");
        public readonly static MemProperty MemoryType = new MemProperty(13, "MemoryType");
        public readonly static MemProperty Model = new MemProperty(14, "Model");
        public readonly static MemProperty Name = new MemProperty(15, "Name");
        public readonly static MemProperty OtherIdentifyingInfo = new MemProperty(16, "OtherIdentifyingInfo");
        public readonly static MemProperty PartNumber = new MemProperty(17, "PartNumber");
        public readonly static MemProperty PositionInRow = new MemProperty(18, "PositionInRow");
        public readonly static MemProperty PoweredOn = new MemProperty(19, "PoweredOn");
        public readonly static MemProperty Removable = new MemProperty(20, "Removable");
        public readonly static MemProperty Replaceable = new MemProperty(21, "Replaceable");
        public readonly static MemProperty SerialNumber = new MemProperty(22, "SerialNumber");
        public readonly static MemProperty SKU = new MemProperty(23, "SKU");
        public readonly static MemProperty Speed = new MemProperty(24, "Speed");
        public readonly static MemProperty Status = new MemProperty(25, "Status");
        public readonly static MemProperty Tag = new MemProperty(26, "Tag");
        public readonly static MemProperty TotalWidth = new MemProperty(27, "TotalWidth");
        public readonly static MemProperty TypeDetail = new MemProperty(28, "TypeDetail");
        public readonly static MemProperty Version = new MemProperty(29, "Version");

        private MemProperty(int value, string str) : base (value, str){}
    }
}
