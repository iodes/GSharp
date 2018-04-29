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

        public string Signature { get; set; }

        public PackageDataCollection Datas { get; } = new PackageDataCollection();
        #endregion

        #region 내부 함수
        private void CompressData(ZipOutputStream zipStream, IPackageData data)
        {
            if (data is PackageDirectory directory)
            {
                if (directory.Children.Count > 0)
                {
                    // 폴더 내부 탐색
                    foreach (var subData in directory.Children)
                    {
                        CompressData(zipStream, subData);
                    }
                }
                else
                {
                    // 공백 폴더 생성
                    var dirEntryName = $@"{ZipEntry.CleanName(directory.Path)}\";
                    zipStream.PutNextEntry(new ZipEntry(dirEntryName));
                    zipStream.CloseEntry();
                }
            }

            // 파일 데이터 압축
            if (data is PackageFile file)
            {
                var fileEntryName = ZipEntry.CleanName(file.Path);
                zipStream.PutNextEntry(new ZipEntry(fileEntryName)
                {
                    DateTime = file.LastWriteTime,
                    Size = file.Size
                });

                var buffer = new byte[4096];
                StreamUtils.Copy(file.Content, zipStream, buffer);

                zipStream.CloseEntry();
            }
        }
        #endregion

        #region 사용자 함수
        public void Add(IPackageData data)
        {
            Datas.Add(data);
        }

        public void Remove(IPackageData data)
        {
            Datas.Remove(data);
            data.Dispose();
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
                binaryWriter.Write(Signature ?? string.Empty);

                // 압축 스트림 설정
                zipStream.SetLevel(3);

                // 압축 데이터 작성
                binaryWriter.Write(SectionType.Content.GetValue<EnumStringAttribute, string>());
                foreach (var data in Datas)
                {
                    CompressData(zipStream, data);
                }

                zipStream.IsStreamOwner = false;
                zipStream.Close();
            }

            return null;
        }
        #endregion
    }
}
