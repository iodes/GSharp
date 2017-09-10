using GSharp.Support.Interfaces;
using System;

namespace GSharp.Support
{
    public static class NTEnvironment : IEnvironment
    {
        public bool IsEnvironment
        {
            get
            {
                return NativeMethods.GetModuleHandle("ntdll.dll") == null;
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
