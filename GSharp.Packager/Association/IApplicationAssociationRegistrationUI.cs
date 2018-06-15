using System;
using System.Runtime.InteropServices;

namespace GSharp.Packager.Association
{
    [Guid("1f76a169-f994-40ac-8fc8-0959e8874710")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IApplicationAssociationRegistrationUI
    {
        [PreserveSig]
        int LaunchAdvancedAssociationUI([MarshalAs(UnmanagedType.LPWStr)] string pszAppRegName);
    }
}
