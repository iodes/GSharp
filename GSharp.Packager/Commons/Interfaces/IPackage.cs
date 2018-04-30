using System;
using System.IO;

namespace GSharp.Packager.Commons
{
    public interface IPackage : IPackageHeader, IDisposable
    {
        void Install(string path);
    }
}
