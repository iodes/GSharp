using GSharp.Packager.Commons;
using GSharp.Packager.Extensions;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace GSharp.Packager
{
    public class PackageCreator : IPackageHeader
    {
        #region 속성
        public string Title { get; set; }

        public string Author { get; set; }

        public string Version { get; set; }

        public string Description { get; set; }

        public string Signature { get; set; }

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
                binaryWriter.Write(SectionType.Header.GetValue<EnumStringAttribute, string>());
                binaryWriter.Write(Title ?? string.Empty);
                binaryWriter.Write(Author ?? string.Empty);
                binaryWriter.Write(Version ?? string.Empty);
                binaryWriter.Write(Description ?? string.Empty);
                binaryWriter.Write(Signature ?? string.Empty);

                // 압축 스트림 설정
                zipStream.SetLevel(3);

                // 압축 데이터 작성
                binaryWriter.Write(SectionType.Content.GetValue<EnumStringAttribute, string>());
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
