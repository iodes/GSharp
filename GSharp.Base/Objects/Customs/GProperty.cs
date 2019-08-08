using System;
using GSharp.Common.Extensions;
using GSharp.Common.Objects;

namespace GSharp.Base.Objects.Customs
{
    public class GProperty : GSettableObject, ICustomObject
    {
        #region Properties
        public IGCommand Command { get; }
        
        public override Type SettableType => Command.ObjectType;
        
        public Type CustomType => Command.ObjectType;
        #endregion
        
        #region Initializer
        public GProperty(IGCommand command)
        {
            Command = command;
        }

        public override string ToSource()
        {
            return Command.FullName;
        }
        #endregion
    }
}
