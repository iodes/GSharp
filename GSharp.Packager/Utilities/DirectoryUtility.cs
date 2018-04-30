using GSharp.Packager.Commons;
using System.IO;

namespace GSharp.Packager.Utilities
{
    static class DirectoryUtility
    {
        public static PackageDataCollection GetContents(string path)
        {
            var result = new PackageDataCollection();

            foreach (string directory in Directory.GetDirectories(path))
            {
                result.Add(new PackageDirectory(directory));
            }

            foreach (string file in Directory.GetFiles(path))
            {
                result.Add(new PackageFile(file));
            }

            return result;
        }
    }
}
