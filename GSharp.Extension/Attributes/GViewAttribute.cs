using System;

namespace GSharp.Extension.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GViewAttribute : Attribute
    {
        public string Name { get; set; }

        public GViewAttribute(string name)
        {
            Name = name;
        }
    }
}
