using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace GSharp.Packager.Commons
{
    public class Package : IPackage
    {
        #region 변수
        internal string _title;
        internal string _author;
        internal string _version;
        internal string _signature;

        internal ZipFile _streamZip;
        internal FileStream _streamFile;
        internal BinaryReader _streamReader;

        internal PackageDataCollection _datas = new PackageDataCollection();
        #endregion

        #region 속성
        public string Title => _title;

        public string Author => _author;

        public string Version => _version;

        public string Signature => _signature;

        public PackageDataCollection Datas => _datas;
        #endregion

        #region 생성자
        internal Package()
        {

        }
        #endregion

        #region 내부 함수
        private void UncompressData(string path, IPackageData data)
        {
            if (data is PackageDirectory directory)
            {
                if (directory.Children.Count > 0)
                {
                    // 폴더 내부 탐색
                    foreach (var subData in directory.Children)
                    {
                        UncompressData(path, subData);
                    }
                }

                // 대상 폴더 생성
                var targetPath = Path.Combine(path, directory.Path);
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
            }

            // 파일 압축 해제
            if (data is PackageFile file)
            {
                var targetPath = Path.Combine(path, file.Path);

                using (var streamWriter = File.Create(targetPath))
                {
                    var buffer = new byte[4096];
                    StreamUtils.Copy(file.Content, streamWriter, buffer);
                }
            }
        }
        #endregion

        #region 사용자 함수
        public void Install(string path)
        {
            foreach (var data in Datas)
            {
                UncompressData(path, data);
            }
        }
        #endregion

        #region IDisposable
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (IDisposable data in Datas)
                    {
                        data.Dispose();
                    }

                    _streamZip.Close();
                    _streamFile.Dispose();
                    _streamReader.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
