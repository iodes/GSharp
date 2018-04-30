using GSharp.Packager.Commons;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace GSharp.Packager.Utilities
{
    static class CompressUtility
    {
        public static void Compress(ZipOutputStream zipStream, IPackageData data)
        {
            if (data is PackageDirectory directory)
            {
                if (directory.Children.Count > 0)
                {
                    // 폴더 내부 탐색
                    foreach (var subData in directory.Children)
                    {
                        Compress(zipStream, subData);
                    }
                }
                else
                {
                    // 단일 폴더 생성
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

        public static void Decompress(string path, IPackageData data)
        {
            if (data is PackageDirectory directory)
            {
                if (directory.Children.Count > 0)
                {
                    // 폴더 내부 탐색
                    foreach (var subData in directory.Children)
                    {
                        Decompress(path, subData);
                    }
                }
                else
                {
                    // 단일 폴더 생성
                    CreateDirectory(Path.Combine(path, directory.Path));
                }
            }

            // 파일 데이터 해제
            if (data is PackageFile file)
            {
                var targetPath = Path.Combine(path, file.Path);
                CreateDirectory(Path.GetDirectoryName(targetPath));

                using (var streamWriter = File.Create(targetPath))
                {
                    var buffer = new byte[4096];
                    StreamUtils.Copy(file.Content, streamWriter, buffer);
                }
            }
        }

        private static void CreateDirectory(string path)
        {
            if (path.Length > 0 && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
