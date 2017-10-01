using GSharp.Support.Base;
using GSharp.Support.Utilities;
using System;
using System.Linq;
using System.Management;

namespace GSharp.Support
{
    public class VirtualEnvironment : IEnvironment
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");

        public bool IsEnvironment
        {
            get
            {
                using (var items = searcher.Get())
                {
                    foreach (var item in items)
                    {
                        var manufacturer = item["Manufacturer"].ToString();
                        if ((manufacturer.StringEquals("Microsoft Corporation") && item["Model"].StringContains("Virtual")) || manufacturer.StringContains("VMware") || item["Model"].StringEquals("VirtualBox"))
                        {
                            return true;
                        }

                        var hypervisor = item.Properties.OfType<PropertyData>().FirstOrDefault(x => x.Name.StringEquals("HypervisorPresent"));
                        if ((bool?)hypervisor?.Value == true)
                        {
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
                throw new NotImplementedException();
            }
        }
    }
}
