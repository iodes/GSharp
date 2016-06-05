using System;
using GSharp.Extension.Abstracts;

namespace GSharp.Extension
{
    [Serializable]
    public class GControl
    {
        #region 속성
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
        public GControl(Type source, string friendlyName, string namespaceName)
        {
            _Source = source;
            _FriendlyName = friendlyName;
            _NamespaceName = namespaceName;
        }

        public GControl(GExtension parent, Type source, string friendlyName, string namespaceName)
            : this(source, friendlyName, namespaceName)
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
