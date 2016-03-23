using System;

namespace GSharp.Extension
{
    public class GCommand
    {
        public GModule Parent { get; set; }

        public Type[] Arguments { get; set; }

        public string MethodName { get; set; }

        public string FriendlyName { get; set; }

        public string NamespaceName { get; set; }

        public CommandAttribute.CommandType MethodType { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0}.{1}", NamespaceName, MethodName);
            }
        }
    }
}
