using System;

namespace GSharp.Extension.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter, AllowMultiple = false)]
    public class GParameterAttribute : Attribute
    {
        public string Name { get; set; }

        public GParameterAttribute(string name)
        {
            Name = name;
        }
    }
}
