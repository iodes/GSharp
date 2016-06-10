using System;
using GSharp.Extension.Optionals;

namespace GSharp.Extension.Exports
{
    [Serializable]
    public class GExport
    {
        #region 속성
        public GOptional[] Optionals
        {
            get
            {
                return _Optionals;
            }
        }
        private GOptional[] _Optionals;

        public Type ObjectType
        {
            get
            {
                return _ObjectType;
            }
        }
        private Type _ObjectType;

        public string MethodName
        {
            get
            {
                return _MethodName;
            }
        }
        private string _MethodName;

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

        public string FullName
        {
            get
            {
                return string.Format("{0}.{1}", NamespaceName, MethodName);
            }
        }
        #endregion

        #region 생성자
        public GExport(string namespaceName, string methodName, string friendlyName, Type objectType, GOptional[] optionals = null)
        {
            _NamespaceName = namespaceName;
            _MethodName = methodName;
            _FriendlyName = friendlyName;
            _ObjectType = objectType;
            _Optionals = optionals;
        }
        #endregion
    }
}
