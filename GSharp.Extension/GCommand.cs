namespace GSharp.Extension
{
    public class GCommand
    {
        public GModule Parent { get; set; }
        public string MethodName { get; set; }
        public string FriendlyName { get; set; }
        public string NamespaceName { get; set; }
        public int ParamCount { get; set; } = 0;

        public CommandAttribute.CommandType MethodType { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0}.{1}", NamespaceName, MethodName);
            }
        }

        public GCommand()
        {
        }

        public GCommand(string methodName, int paramCount = 0)
        {
            MethodName = methodName;
            ParamCount = paramCount;
        }

        public GCommand(string namespaceName, string methodName, int paramCount = 0) : this(methodName, paramCount)
        {
            NamespaceName = namespaceName;
        }

        public GCommand(string namespaceName, string methodName, string friendlyName, int paramCount = 0) : this(namespaceName, methodName, paramCount)
        {
            FriendlyName = friendlyName;
        }

        public GCommand(GModule parent, string methodName, string friendlyName, string namespaceName, int paramCount = 0) : this(namespaceName, methodName, friendlyName, paramCount)
        {
            Parent = parent;
        }
    }
}
