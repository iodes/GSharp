using System;
using System.Text;

namespace GSharp.Packager.Commons
{
    public abstract class PackageData : IPackageData
    {
        #region 변수
        protected string _name;
        protected long _size;
        protected DateTime _lastWriteTime;
        internal IPackageData _parent;
        #endregion

        #region 속성
        public string Name => _name;

        public string Path => ResolvePath(this);

        public long Size => _size;

        public DateTime LastWriteTime => _lastWriteTime;

        public IPackageData Parent => _parent;
        #endregion

        #region 내부 함수
        private string ResolvePath(IPackageData data)
        {
            var builder = new StringBuilder();

            IPackageData currentData = data;
            while (currentData != null)
            {
                builder.Insert(0, currentData.Name);

                var nextData = currentData.Parent;
                if (nextData != null)
                {
                    builder.Insert(0, "\\");
                }

                currentData = nextData;
            }

            return builder.ToString();
        }
        #endregion

        #region 사용자 함수
        public abstract void Extract(string path);
        #endregion

        #region IDisposable
        public abstract void Dispose();
        #endregion
    }
}
