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
                var findDict = dict.Find(pathDictKey) as PackageDirectory;
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
                    parentDir.Children.Add(new PackageDirectory(CleanName(path), null));
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
                    dict.Add(pathDictKey, new PackageDirectory(CleanName(path), null));
                }
                else
                {
                    dict.Add(pathDictKey, new PackageFile(path, stream));
                }
            }
        }

        private string CleanName(string path)
        {
            return path.Replace("\\", "");
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
            var result = new Package
            {
                _streamFile = File.Open(path, FileMode.Open)
            };

            // 헤더 데이터 읽기
            result._streamReader = new BinaryReader(result._streamFile);
            if (result._streamReader.ReadString() == SectionType.Header.GetValue<ValueAttribute, string>())
            {
                result._title = result._streamReader.ReadString();
                result._author = result._streamReader.ReadString();
                result._version = result._streamReader.ReadString();
                result._signature = result._streamReader.ReadString();
            }

            // 압축 데이터 읽기
            if (result._streamReader.ReadString() == SectionType.Content.GetValue<ValueAttribute, string>())
            {
                var dictDir = new Dictionary<string, IPackageData>();

                result._streamZip = new ZipFile(result._streamFile)
                {
                    IsStreamOwner = false
                };

                foreach (ZipEntry zipEntry in result._streamZip)
                {
                    var zipStream = result._streamZip.GetInputStream(zipEntry);
                    ResolveDirectory(ref dictDir, zipStream, zipEntry.Name);
                }

                result.Datas.AddRange(dictDir.Values.Where(x => x.Parent == null));
            }

            return result;
        }
        #endregion
    }
}
