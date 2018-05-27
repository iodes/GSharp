using GSharp.Packager.Utilities;
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

        internal ReadOnlyPackageDataCollection _readOnlyDatas;
        #endregion

        #region 속성
        public string Title => _title;

        public string Author => _author;

        public string Version => _version;

        public string Signature => _signature;

        public ReadOnlyPackageDataCollection Datas => _readOnlyDatas;
        #endregion

        #region 생성자
        internal Package()
        {

        }
        #endregion

        #region 사용자 함수
        public void Install(string path)
        {
            foreach (var data in _readOnlyDatas)
            {
                CompressUtility.Decompress(path, data);
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
                    foreach (IDisposable data in _readOnlyDatas)
                    {
                        data.Dispose();
                    }

                    _streamZip.Close();
                    _streamReader.Dispose();
                    _streamFile.Dispose();
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
