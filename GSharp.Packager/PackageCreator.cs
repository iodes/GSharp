using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

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

        public IPackage Create(string path)
        {
            using (var fileStream = File.Open(path, FileMode.Create))
            using (var binaryWriter = new BinaryWriter(fileStream))
            using (var zipStream = new ZipOutputStream(fileStream))
            {
                // 헤더 데이터 작성
                binaryWriter.Write("Title");
                binaryWriter.Write("Author");
                binaryWriter.Write("Version");
                binaryWriter.Write("Description");
                binaryWriter.Write(Resolver?.Signature ?? "DEFAULT");

                // 압축 스트림 설정
                zipStream.SetLevel(3);

                // 압축 데이터 작성
                foreach (var file in Datas)
                {
                    var fileInfo = new FileInfo(file);
                    var entryName = ZipEntry.CleanName(file.Substring(fileInfo.DirectoryName.Length));
                    zipStream.PutNextEntry(new ZipEntry(entryName)
                    {
                        DateTime = fileInfo.LastWriteTime,
                        Size = fileInfo.Length
                    });

                    var buffer = new byte[4096];
                    using (var streamReader = File.OpenRead(file))
                    {
                        StreamUtils.Copy(streamReader, zipStream, buffer);
                    }

                    zipStream.CloseEntry();
                }

                zipStream.IsStreamOwner = false;
                zipStream.Close();
            }

            return null;
        }
        #endregion
    }
}
