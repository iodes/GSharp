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

        public Type[] Arguments
        {
            get
            {
                return _Arguments;
            }
        }
        private Type[] _Arguments;

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
            Event,
            Logic,
            Property
        }
        #endregion

        #region 생성자
        public GCommand(string methodName, Type objectType, CommandType methodType, Type[] arguments = null)
        {
            _ObjectType = objectType;
            _MethodName = methodName;
            _MethodType = methodType;
            _Arguments = arguments;

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

        public GCommand(string namespaceName, string methodName, Type objectType, CommandType methodType, Type[] arguments = null)
            : this(methodName, objectType, methodType, arguments)
        {
            _NamespaceName = namespaceName;
        }

        public GCommand(string namespaceName, string methodName, string friendlyName, Type objectType, CommandType methodType, Type[] arguments = null)
            : this(namespaceName, methodName, objectType, methodType, arguments)
        {
            _FriendlyName = friendlyName;
        }

        public GCommand(GExtension parent, string namespaceName, string methodName, string friendlyName, Type objectType, CommandType methodType, Type[] arguments = null)
            : this(namespaceName, methodName, friendlyName, objectType, methodType, arguments)
        {
            _Parent = parent;
        }
        #endregion
    }
}
