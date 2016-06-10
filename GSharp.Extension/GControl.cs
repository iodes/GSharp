using System;
using GSharp.Extension.Abstracts;
using GSharp.Extension.Exports;

namespace GSharp.Extension
{
    [Serializable]
    public class GControl
    {
        #region 속성
        public GExport[] Exports
        {
            get
            {
                return _Exports;
            }
        }
        private GExport[] _Exports;

        public GExtension Parent
        {
            get
            {
                return _Parent;
            }
        }
        private GExtension _Parent;

        public Type Source
        {
            get
            {
                return _Source;
            }
        }
        private Type _Source;

        public string FriendlyName
        {
            get
            {
                return _FriendlyName;
            }
        }
        private string _FriendlyName;

        public string NamespaceName
        {
            get
            {
                return _NamespaceName;
            }
        }
        private string _NamespaceName;
        #endregion

        #region 생성자
        public GControl(Type source, string friendlyName, string namespaceName, GExport[] exports = null)
        {
            _Source = source;
            _FriendlyName = friendlyName;
            _NamespaceName = namespaceName;
            _Exports = exports;
        }

        public GControl(GExtension parent, Type source, string friendlyName, string namespaceName, GExport[] exports = null)
            : this(source, friendlyName, namespaceName, exports)
        {
            _Parent = parent;
        }
        #endregion

        #region 사용자 함수
        public GView LoadView()
        {
            return Activator.CreateInstance(Source) as GView;
        }
        #endregion
    }
}
