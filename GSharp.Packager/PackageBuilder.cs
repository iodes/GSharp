using GSharp.Packager.Commons;
using GSharp.Packager.Extensions;
using GSharp.Packager.Utilities;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace GSharp.Packager
{
    public class PackageBuilder : IPackageHeader
    {
        #region 변수
        private PackageDataCollection _datas = new PackageDataCollection();
        #endregion

        #region 속성
        public string Title { get; set; }

        public string Author { get; set; }

        public string Version { get; set; }

        public string Signature { get; set; }

        public ReadOnlyPackageDataCollection Datas { get; }
        #endregion

        #region 생성자
        public PackageBuilder()
        {
            Datas = new ReadOnlyPackageDataCollection(_datas);
        }
        #endregion

        #region 사용자 함수
        public void Add(string path)
        {
            var attributes = File.GetAttributes(path);
            if (attributes.HasFlag(FileAttributes.Directory))
            {
                _datas.AddRange(DirectoryUtility.GetContents(path));
            }
            else
            {
                _datas.Add(new PackageFile(path));
            }
        }

        public void Add(IPackageData data)
        {
            _datas.Add(data);
        }

        public void Remove(IPackageData data)
        {
            _datas.Remove(data);
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
                _readOnlyDatas = new ReadOnlyPackageDataCollection(_datas)
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
                foreach (var data in _datas)
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
