namespace GSharp.Extension
{
    public class GCommand
    {
        public string MethodName { get; set; }

        public string FriendlyName { get; set; }

        public string NamespaceName { get; set; }

        public CommandAttribute.CommandType MethodType { get; set; }
    }
}
