using GSharp.Packager.Commons;
using GSharp.Packager.Extensions;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace GSharp.Packager
{
    public class PackageLoader
    {
        #region 사용자 함수
        public IPackage Load(string path)
        {
            using (var fileStream = File.Open(path, FileMode.Open))
            using (var binaryReader = new BinaryReader(fileStream))
            {
                // 헤더 데이터 읽기
                if (binaryReader.ReadString() == SectionType.Header.GetValue<EnumStringAttribute, string>())
                {
                    Console.WriteLine("Title : " + binaryReader.ReadString());
                    Console.WriteLine("Author : " + binaryReader.ReadString());
                    Console.WriteLine("Version : " + binaryReader.ReadString());
                    Console.WriteLine("Description : " + binaryReader.ReadString());
                    Console.WriteLine("Signature : " + binaryReader.ReadString());
                }

                // 압축 데이터 읽기
                if (binaryReader.ReadString() == SectionType.Content.GetValue<EnumStringAttribute, string>())
                {
                    using (var zipFile = new ZipFile(fileStream))
                    {
                        foreach (ZipEntry zipEntry in zipFile)
                        {
                            if (!zipEntry.IsFile)
                            {
                                continue;
                            }

                            var buffer = new byte[4096];
                            var zipStream = zipFile.GetInputStream(zipEntry);

                            var fullZipToPath = Path.Combine(Path.GetDirectoryName(path), zipEntry.Name);
                            var directoryName = Path.GetDirectoryName(fullZipToPath);

                            if (directoryName.Length > 0)
                            {
                                Directory.CreateDirectory(directoryName);
                            }

                            using (var streamWriter = File.Create(fullZipToPath))
                            {
                                StreamUtils.Copy(zipStream, streamWriter, buffer);
                            }
                        }
                    }
                }
            }

            return null;
        }
        #endregion
    }
}
