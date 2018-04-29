using System;
using System.Collections.Specialized;
using System.IO;

namespace GSharp.Packager.Commons
{
    public class PackageDirectory : PackageData
    {
        #region 변수
        private PackageDataCollection _children = new PackageDataCollection();
        #endregion

        #region 속성
        public PackageDataCollection Children => _children;
        #endregion

        #region 생성자
        private PackageDirectory()
        {
            Children.CollectionChanged += Children_CollectionChanged;
        }

        public PackageDirectory(string path) : this()
        {
            var directoryInfo = new DirectoryInfo(path);

            _name = directoryInfo.Name;
            _lastWriteTime = directoryInfo.LastWriteTime;

            foreach (string directory in Directory.GetDirectories(path))
            {
                Children.Add(new PackageDirectory(directory));
            }

            foreach (string file in Directory.GetFiles(path))
            {
                Children.Add(new PackageFile(file));
            }
        }

        public PackageDirectory(string name, PackageDataCollection datas) : this()
        {
            _name = name;
            foreach (var data in datas)
            {
                _children.Add(data);
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

        #region IDisposable
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (IDisposable data in Children)
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
