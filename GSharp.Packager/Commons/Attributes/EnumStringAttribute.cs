using System;

namespace GSharp.Packager.Commons
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumStringAttribute : Attribute, IValueAttribute<string>
    {
        public string Value { get; set; }

        public EnumStringAttribute(string value)
        {
            Value = value;
        }
    }
}
