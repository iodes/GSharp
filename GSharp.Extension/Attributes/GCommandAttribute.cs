using System;

namespace GSharp.Extension.Attributes
{
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Enum, AllowMultiple = false)]
    public class GCommandAttribute : Attribute
    {
        public string Name { get; set; }

        public bool Translated { get; set; }

        public GCommandAttribute(string name)
        {
            Name = name;
        }

        public GCommandAttribute(bool isTranslated = false)
        {
            Translated = isTranslated;
        }
    }
}
