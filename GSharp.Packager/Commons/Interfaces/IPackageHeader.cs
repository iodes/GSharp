namespace GSharp.Packager.Commons
{
    public interface IPackageHeader
    {
        string Title { get; }

        string Author { get; }

        string Version { get; }

        string Signature { get; }
    }
}
