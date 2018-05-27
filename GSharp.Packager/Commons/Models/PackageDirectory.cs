using GSharp.Packager.Extensions;
using GSharp.Packager.Utilities;
using System;
using System.Collections.Specialized;
using System.IO;

namespace GSharp.Packager.Commons
{
    public class PackageDirectory : PackageData
    {
        #region 변수
        internal PackageDataCollection _children = new PackageDataCollection();
        #endregion

        #region 속성
        public ReadOnlyPackageDataCollection Children { get; }
        #endregion

        #region 생성자
        private PackageDirectory()
        {
            Children = new ReadOnlyPackageDataCollection(_children);
            _children.CollectionChanged += Children_CollectionChanged;
        }

        public PackageDirectory(string path) : this()
        {
            var info = new DirectoryInfo(path);

            _name = info.Name;
            _lastWriteTime = info.LastWriteTime;

            _children.AddRange(DirectoryUtility.GetContents(path));
        }

        public PackageDirectory(string name, PackageDataCollection datas) : this()
        {
            _name = name;

            if (datas != null)
            {
                _children.AddRange(datas);
            }
        }
        #endregion

        #region 이벤트
        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (PackageData data in e.NewItems)
            {
                data._parent = this;
            }
        }
        #endregion

        #region 사용자 함수
        public override void Extract(string path)
        {
            CompressUtility.Decompress(path, this);
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
                    foreach (IDisposable data in _children)
                    {
                        data.Dispose();
                    }
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
