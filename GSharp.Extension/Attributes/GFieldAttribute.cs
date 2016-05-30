using System;

namespace GSharp.Extension.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class GFieldAttribute : Attribute
    {
        public string Name { get; set; }

        public GFieldAttribute(string name)
        {
            Name = name;
        }
    }
}
