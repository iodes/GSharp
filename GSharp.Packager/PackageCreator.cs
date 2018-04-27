using System.IO;
using System.Linq;

namespace GSharp.Packager
{
    public class PackageCreator
    {
        #region 속성
        public IPackageResolver Resolver { get; set; }

        public PackageDataCollection Datas { get; } = new PackageDataCollection();
        #endregion

        #region 사용자 함수
        public void AddFile(string path)
        {
            Datas.Add(path);
        }

        public void RemoveFile(string path)
        {
            Datas.Remove(path);
        }

        public void AddDirectory(string path)
        {
            Datas.AddRange(Directory.GetFiles(path, "*.*", SearchOption.AllDirectories));
        }

        public void RemoveDirectory(string path)
        {
            Datas.RemoveAll(x => x.StartsWith(path));
        }

        public IPackage Create()
        {
            return null;
        }
        #endregion
    }
}
