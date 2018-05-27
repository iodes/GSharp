using GSharp.Packager.Commons;
using GSharp.Packager.Extensions;
using GSharp.Packager.Utilities;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace GSharp.Packager
{
    public class PackageBuilder : IPackageHeader
    {
        #region 속성
        public string Title { get; set; }

        public string Author { get; set; }

        public string Version { get; set; }

        public string Signature { get; set; }

        public PackageDataCollection Datas { get; } = new PackageDataCollection();
        #endregion

        #region 사용자 함수
        public void Add(string path)
        {
            var attributes = File.GetAttributes(path);
            if (attributes.HasFlag(FileAttributes.Directory))
            {
                Datas.AddRange(DirectoryUtility.GetContents(path));
            }
            else
            {
                Datas.Add(new PackageFile(path));
            }
        }

        public void Add(IPackageData data)
        {
            Datas.Add(data);
        }

        public void Remove(IPackageData data)
        {
            Datas.Remove(data);
            data.Dispose();
        }

        public Package Create(string path)
        {
            var result = new Package
            {
                _title = Title,
                _author = Author,
                _version = Version,
                _signature = Signature,
                _datas = Datas
            };

            using (var fileStream = File.Open(path, FileMode.Create))
            using (var binaryWriter = new BinaryWriter(fileStream))
            using (var zipStream = new ZipOutputStream(fileStream))
            {
                // 헤더 데이터 작성
                binaryWriter.Write(SectionType.Header.GetValue<ValueAttribute, string>());
                binaryWriter.Write(result.Title ?? string.Empty);
                binaryWriter.Write(result.Author ?? string.Empty);
                binaryWriter.Write(result.Version ?? string.Empty);
                binaryWriter.Write(result.Signature ?? string.Empty);

                // 압축 스트림 설정
                zipStream.SetLevel(3);

                // 압축 데이터 작성
                binaryWriter.Write(SectionType.Content.GetValue<ValueAttribute, string>());
                foreach (var data in Datas)
                {
                    CompressUtility.Compress(zipStream, data);
                }

                zipStream.IsStreamOwner = false;
                zipStream.Close();
            }

            return result;
        }
        #endregion
    }
}
