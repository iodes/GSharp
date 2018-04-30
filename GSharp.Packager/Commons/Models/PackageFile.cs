using System.IO;

namespace GSharp.Packager.Commons
{
    public class PackageFile : PackageData
    {
        #region 변수
        private Stream _content;
        #endregion

        #region 속성
        public Stream Content => _content;
        #endregion

        #region 생성자
        public PackageFile(string path)
        {
            var info = new FileInfo(path);

            _name = info.Name;
            _size = info.Length;
            _lastWriteTime = info.LastWriteTime;
            _content = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public PackageFile(string name, Stream stream)
        {
            _name = name;
            _content = stream;
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
                    _content.Dispose();
                }

                disposedValue = true;
            }
        }

        public override void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
