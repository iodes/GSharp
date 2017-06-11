using System;

namespace GSharp.Extension.Attributes
{
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Enum)]
    public class GTranslationAttribute : Attribute
    {
        public string Name { get; set; }

        public GTranslationAttribute(string name)
        {
            Name = name;
        }
    }
}
