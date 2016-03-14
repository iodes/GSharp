using System;

namespace GSharp.Extension
{
    public class CommandAttribute : Attribute
    {
        public string Name { get; set; }

        public CommandType Type { get; set; }


        public enum CommandType
        {
            Call,
            Event,
            Return
        }

        public CommandAttribute(string name, CommandType type)
        {
            Name = name;
            Type = type;
        }
    }
}
