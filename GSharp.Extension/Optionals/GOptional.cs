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

        // TODO 번역 존재시 올바른 언어 반환 필요
        public string FriendlyName
        {
            get
            {
                return _FriendlyName;
            }
        }
        private string _FriendlyName;

        public GTranslation[] Translations
        {
            get
            {
                return _Translations;
            }
        }
        private GTranslation[] _Translations;

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
        public GOptional(string name, string fullName, string friendlyName, Type objectType, GTranslation[] translations = null)
        {
            _Name = name;
            _FullName = fullName;
            _FriendlyName = friendlyName;
            _ObjectType = objectType;
            _Translations = translations;
        }
        #endregion
    }
}
