using GSharp.Support.Base;
using GSharp.Support.Utilities;
using System;
using System.Runtime.InteropServices;

namespace GSharp.Support
{
    public class WineEnvironment : IEnvironment
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate string wine_get_version();

        public bool IsEnvironment
        {
            get
            {
                if (new NTEnvironment().IsEnvironment && GetWineVersion() != null)
                {
                    return true;
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
                    return GetWineVersion();
                }

                throw new InvalidOperationException();
            }
        }

        private string GetWineVersion()
        {
            var hWineVersion = NativeMethods.GetProcAddress(NativeMethods.GetModuleHandle("ntdll.dll"), "wine_get_version");
            if (hWineVersion != IntPtr.Zero)
            {
                var wineVersion = Marshal.GetDelegateForFunctionPointer(hWineVersion, typeof(wine_get_version)) as wine_get_version;
                return wineVersion();
            }

            return null;
        }
    }
}
