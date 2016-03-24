using System;

namespace GSharp.Extension
{
    public class GCommand
    {
        #region 속성
        public GModule Parent
        {
            get
            {
                return _Parent;
            }
        }
        private GModule _Parent;

        public Type[] Arguments
        {
            get
            {
                return _Arguments;
            }
        }
        private Type[] _Arguments;

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

        public enum CommandType
        {
            Call,
            Event,
            Logic
        }

        public string FullName
        {
            get
            {
                return string.Format("{0}.{1}", NamespaceName, MethodName);
            }
        }
        #endregion

        #region 생성자
        public GCommand(string methodName, CommandType methodType, Type[] arguments = null)
        {
            _MethodName = methodName;
            _Arguments = arguments;
            _MethodType = methodType;
        }

        public GCommand(string namespaceName, string methodName, CommandType methodType, Type[] arguments = null)
            : this(methodName, methodType, arguments)
        {
            _NamespaceName = namespaceName;
        }

        public GCommand(string namespaceName, string methodName, string friendlyName, CommandType methodType, Type[] arguments = null)
            : this(namespaceName, methodName, methodType, arguments)
        {
            _FriendlyName = friendlyName;
        }

        public GCommand(GModule parent, string namespaceName, string methodName, string friendlyName, CommandType methodType, Type[] arguments = null)
            : this(namespaceName, methodName, friendlyName, methodType, arguments)
        {
            _Parent = parent;
        }
        #endregion
    }
}
