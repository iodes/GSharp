namespace GSharp.Packager.Commons
{
    public interface IPackageHeader
    {
        string Title { get; set; }

        string Author { get; set; }

        string Version { get; set; }

        string Description { get; set; }

        string Signature { get; set; }
    }
}
