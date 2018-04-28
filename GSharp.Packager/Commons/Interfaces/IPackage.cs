namespace GSharp.Packager.Commons
{
    public interface IPackage : IPackageHeader
    {
        void Install(string path);
    }
}
