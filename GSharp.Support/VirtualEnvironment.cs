using GSharp.Support.Base;
using GSharp.Support.Utilities;
using System;
using System.Linq;
using System.Management;

namespace GSharp.Support
{
    public class VirtualEnvironment : IEnvironment
    {
        #region 상수
        public const string TYPE_VMWARE = "VMware";
        public const string TYPE_HYPER_V = "Hyper-V";
        public const string TYPE_PARALLELS = "Parallels";
        public const string TYPE_VIRTUALBOX = "VirtualBox";
        public const string TYPE_HYPERVISOR = "Hypervisor";
        private const string KEY_MSCORP = "Microsoft Corporation";
        private const string KEY_MSVM = "Virtual";
        #endregion

        #region 변수
        string lastModel = null;
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
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
                        var isHypervisor = item.Properties.OfType<PropertyData>().FirstOrDefault(x => x.Name.StringEquals("HypervisorPresent"));

                        if (manufacturer.StringContains(TYPE_VMWARE))
                        {
                            lastModel = TYPE_VMWARE;
                            return true;
                        }
                        else if (manufacturer.StringEquals(KEY_MSCORP) && model.StringContains(KEY_MSVM))
                        {
                            lastModel = TYPE_HYPER_V;
                            return true;
                        }
                        else if (manufacturer.StringContains(TYPE_PARALLELS) && model.StringContains(TYPE_PARALLELS))
                        {
                            lastModel = TYPE_PARALLELS;
                            return true;
                        }
                        else if (model.StringEquals(TYPE_VIRTUALBOX))
                        {
                            lastModel = TYPE_VIRTUALBOX;
                            return true;
                        }
                        else if ((bool?)isHypervisor?.Value == true)
                        {
                            lastModel = TYPE_HYPERVISOR;
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
                    return lastModel;
                }

                throw new InvalidOperationException();
            }
        }
    }
}
