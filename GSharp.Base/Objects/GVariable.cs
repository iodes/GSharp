using System;

namespace GSharp.Base.Objects
{
    [Serializable]
    public class GVariable : GSettableObject
    {
        #region Properties
        public string Name { get; }
        
        public override Type SettableType => typeof(object);
        #endregion

        #region Initializer
        public GVariable(string name)
        {
            Name = name;
        }
        #endregion

        public override string ToSource()
        {
            return Name;
        }
    }
}
