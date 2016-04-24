using System;

namespace GSharp.Extension.Attributes
{
    public class GCommandAttribute : Attribute
    {
        public string Name { get; set; }

        public GCommandAttribute(string name)
        {
            Name = name;
        }
    }
}
