using GSharp.Support.Base;
using GSharp.Support.Utilities;
using System;
using System.Management;

namespace GSharp.Support
{
    public class VirtualEnvironment : IEnvironment
    {
        #region 상수
        private const string KEY_MSVM = "Virtual";
        private const string KEY_MSCORP = "Microsoft Corporation";
        #endregion

        #region 변수
        VirtualType lastType;
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
        #endregion

        #region 열거형
        public enum VirtualType
        {
            VMWare,
            HyperV,
            Parallels,
            VirtualBox
        }
        #endregion

        public bool IsEnvironment
        {
            get
            {
                using (var items = searcher.Get())
                {
                    foreach (var item in items)
                    {
                        var model = item["Model"].ToString();
                        var manufacturer = item["Manufacturer"].ToString();

                        if (manufacturer.StringContains(VirtualType.VMWare.ToString()))
                        {
                            lastType = VirtualType.VMWare;
                            return true;
                        }
                        else if (manufacturer.StringEquals(KEY_MSCORP) && model.StringContains(KEY_MSVM))
                        {
                            lastType = VirtualType.HyperV;
                            return true;
                        }
                        else if (manufacturer.StringContains(VirtualType.Parallels.ToString()) && model.StringContains(VirtualType.Parallels.ToString()))
                        {
                            lastType = VirtualType.Parallels;
                            return true;
                        }
                        else if (model.StringEquals(VirtualType.VirtualBox.ToString()))
                        {
                            lastType = VirtualType.VirtualBox;
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public string Version
        {
            get
            {
                if (IsEnvironment)
                {
                    return lastType.ToString();
                }

                throw new InvalidOperationException();
            }
        }

        public VirtualType Type
        {
            get
            {
                if (IsEnvironment)
                {
                    return lastType;
                }

                throw new InvalidOperationException();
            }
        }
    }
}
