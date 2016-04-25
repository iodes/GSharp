using System;

namespace GSharp.Extension.Attributes
{
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public class GCommandAttribute : Attribute
    {
        public string Name { get; set; }

        public GCommandAttribute(string name)
        {
            Name = name;
        }
    }
}
