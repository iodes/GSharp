using GSharp.Extension.Optionals;
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace GSharp.Extension
{
    [Serializable]
    public class GCommand
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

        public GOptional[] Optionals
        {
            get
            {
                return _Optionals;
            }
        }
        private GOptional[] _Optionals;

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

        public string MethodName
        {
            get
            {
                return _MethodName;
            }
        }
        private string _MethodName;

        // TODO 번역 존재시 올바른 언어 반환 필요
        public string FriendlyName
        {
            get
            {
                if (Translations?.Length > 0)
                {
                    return Translations[0].FriendlyName;
                }
                else
                {
                    return _FriendlyName;
                }
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

        public CommandType MethodType
        {
            get
            {
                return _MethodType;
            }
        }
        private CommandType _MethodType;

        public string FullName
        {
            get
            {
                return string.Format("{0}.{1}", NamespaceName, MethodName);
            }
        }

        public Color BlockColor
        {
            get
            {
                return _BlockColor;
            }
        }
        private Color _BlockColor;
        #endregion

        #region 열거형
        public enum CommandType
        {
            Call,
            Enum,
            Event,
            Logic,
            Property
        }
        #endregion

        #region 생성자
        public GCommand(string methodName, Type objectType, CommandType methodType, GOptional[] optionals = null, GTranslation[] translations = null)
        {
            _ObjectType = objectType;
            _MethodName = methodName;
            _MethodType = methodType;
            _Optionals = optionals;
            _Translations = translations;

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(FullName);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                _BlockColor = ColorTranslator.FromHtml("#" + sb.ToString().Substring(0, 6));
            }
        }

        public GCommand(string namespaceName, string methodName, Type objectType, CommandType methodType, GOptional[] optionals = null, GTranslation[] translations = null)
            : this(methodName, objectType, methodType, optionals, translations)
        {
            _NamespaceName = namespaceName;
        }

        public GCommand(string namespaceName, string methodName, string friendlyName, Type objectType, CommandType methodType, GOptional[] optionals = null, GTranslation[] translations = null)
            : this(namespaceName, methodName, objectType, methodType, optionals, translations)
        {
            _FriendlyName = friendlyName;
        }

        public GCommand(GExtension parent, string namespaceName, string methodName, string friendlyName, Type objectType, CommandType methodType, GOptional[] optionals = null, GTranslation[] translations = null)
            : this(namespaceName, methodName, friendlyName, objectType, methodType, optionals, translations)
        {
            _Parent = parent;
        }
        #endregion
    }
}
