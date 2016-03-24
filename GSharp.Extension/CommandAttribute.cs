using System;

namespace GSharp.Extension
{
    public class CommandAttribute : Attribute
    {
        public string Name { get; set; }

        public CommandAttribute(string name)
        {
            Name = name;
        }
    }
}
