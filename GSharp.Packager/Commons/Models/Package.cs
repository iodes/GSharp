using System;

namespace GSharp.Packager.Commons
{
    public class Package : IPackage, IPackageHeader
    {
        #region 변수
        internal string _title;
        internal string _author;
        internal string _version;
        internal string _signature;
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

        #region 사용자 함수
        public void Install(string path)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
