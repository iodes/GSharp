namespace GSharp.Packager
{
    public interface IPackage
    {
        string Title { get; set; }

        string Author { get; set; }

        string Description { get; set; }

        void Install(string path);
    }
}
