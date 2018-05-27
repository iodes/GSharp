using System;

namespace GSharp.Packager.Commons
{
    public interface IPackageData : IDisposable
    {
        string Name { get; }

        string Path { get; }

        long Size { get; }

        DateTime LastWriteTime { get; }

        IPackageData Parent { get; }

        void Extract(string path);
    }
}
