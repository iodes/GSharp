using System;

namespace GSharp.Packager.Commons
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ValueAttribute : Attribute, IValueAttribute<string>
    {
        public string Value { get; set; }

        public ValueAttribute(string value)
        {
            Value = value;
        }
    }
}
