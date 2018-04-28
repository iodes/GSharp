namespace GSharp.Packager
{
    public interface IPackageResolver
    {
        string Signature { get; set; }

        string Extension { get; set; }
    }
}
