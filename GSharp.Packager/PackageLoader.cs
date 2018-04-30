using GSharp.Packager.Commons;
using GSharp.Packager.Extensions;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GSharp.Packager
{
    public class PackageLoader
    {
        #region 내부 함수
        private void ResolveDirectory(ref Dictionary<string, IPackageData> dict, Stream stream, string path, PackageDirectory parentDir = null)
        {
            var rootName = GetRootName(path);
            var downPath = path.Substring(Math.Min(path.Length, rootName.Length + 1));
            var pathDictKey = parentDir?.Path + (parentDir?.Path == null ? rootName : $@"\{rootName}");

            if (downPath.Length > 0)
            {
                // 하위 경로 검색
                var findDict = FindDictionary(dict, pathDictKey);
                var currentDir = findDict ?? new PackageDirectory(rootName, null);

                if (findDict == null)
                {
                    if (parentDir != null)
                    {
                        parentDir.Children.Add(currentDir);
                    }

                    dict.Add(pathDictKey, currentDir);
                }

                ResolveDirectory(ref dict, stream, downPath, currentDir);
            }
            else if (parentDir != null)
            {
                // 최하위 파일
                if (IsDirectory(path))
                {
                    parentDir.Children.Add(new PackageDirectory(path, null));
                }
                else
                {
                    parentDir.Children.Add(new PackageFile(path, stream));
                }
            }
            else
            {
                // 단일 파일
                if (IsDirectory(path))
                {
                    dict.Add(pathDictKey, new PackageDirectory(path, null));
                }
                else
                {
                    dict.Add(pathDictKey, new PackageFile(path, stream));
                }
            }
        }

        public PackageDirectory FindDictionary(Dictionary<string, IPackageData> dict, string key)
        {
            if (dict.ContainsKey(key))
            {
                return (PackageDirectory)dict[key];
            }

            return null;
        }

        private string GetRootName(string path)
        {
            return path.Split('/').First();
        }

        private bool IsDirectory(string path)
        {
            return path.EndsWith("\\");
        }
        #endregion

        #region 사용자 함수
        public IPackage Load(string path)
        {
            Package result = new Package();

            using (var fileStream = File.Open(path, FileMode.Open))
            using (var binaryReader = new BinaryReader(fileStream))
            {
                // 헤더 데이터 읽기
                if (binaryReader.ReadString() == SectionType.Header.GetValue<EnumStringAttribute, string>())
                {
                    result._title = binaryReader.ReadString();
                    result._author = binaryReader.ReadString();
                    result._version = binaryReader.ReadString();
                    result._signature = binaryReader.ReadString();
                }

                // 압축 데이터 읽기
                if (binaryReader.ReadString() == SectionType.Content.GetValue<EnumStringAttribute, string>())
                {
                    var dictDir = new Dictionary<string, IPackageData>();

                    using (var zipFile = new ZipFile(fileStream))
                    {
                        foreach (ZipEntry zipEntry in zipFile)
                        {
                            var zipStream = zipFile.GetInputStream(zipEntry);
                            ResolveDirectory(ref dictDir, zipStream, zipEntry.Name);
                        }

                        foreach (var data in dictDir.Values.Where(x => x.Parent == null))
                        {
                            result.Datas.Add(data);
                        }
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
