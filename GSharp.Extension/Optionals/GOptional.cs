using System;

namespace GSharp.Extension.Optionals
{
    [Serializable]
    public class GOptional
    {
        #region 속성
        public string Name
        {
            get
            {
                return _Name;
            }
        }
        private string _Name;

        public string FullName
        {
            get
            {
                return _FullName;
            }
        }
        private string _FullName;

        public string FriendlyName
        {
            get
            {
                return _FriendlyName;
            }
        }
        private string _FriendlyName;

        public Type ObjectType
        {
            get
            {
                return _ObjectType;
            }
        }
        private Type _ObjectType;
        #endregion

        #region 생성자
        public GOptional(string name, string fullName, string friendlyName, Type objectType)
        {
            _Name = name;
            _FullName = fullName;
            _FriendlyName = friendlyName;
            _ObjectType = objectType;
        }
        #endregion
    }
}
