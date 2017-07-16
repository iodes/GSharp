using System;

namespace GSharp.Extension.Attributes
{
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Property, AllowMultiple = false)]
    public class GControlAttribute : Attribute
    {
        public string Name { get; set; }

        public bool Translated { get; set; }

        public GControlAttribute(string name)
        {
            Name = name;
        }

        public GControlAttribute(bool isTranslated = false)
        {
            Translated = isTranslated;
        }
    }
}
