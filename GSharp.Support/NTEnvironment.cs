using GSharp.Support.Base;
using GSharp.Support.Utilities;
using System;

namespace GSharp.Support
{
    public class NTEnvironment : IEnvironment
    {
        public bool IsEnvironment
        {
            get
            {
                return NativeMethods.GetModuleHandle("ntdll.dll") == IntPtr.Zero;
            }
        }

        public string Version
        {
            get
            {
                if (IsEnvironment)
                {
                    return Environment.OSVersion.VersionString;
                }

                throw new InvalidOperationException();
            }
        }
    }
}
