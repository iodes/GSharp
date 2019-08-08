using System;

namespace GSharp.Base.Objects
{
    public class GVariable : GSettableObject
    {
        public string Name { get; }
        
        public override Type SettableType => typeof(object);
        
        public GVariable(string name)
        {
            Name = name;
        }

        public override string ToSource()
        {
            return Name;
        }
    }
}
