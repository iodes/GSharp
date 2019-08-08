using GSharp.Base.Cores;
using System;
using System.Linq;
using GSharp.Common.Cores;
using GSharp.Common.Extensions;
using GSharp.Common.Objects;

namespace GSharp.Base.Objects.Customs
{
    [Serializable]
    public class GEnumeration : GObject, ICustomObject, IModule
    {
        #region Properties
        public IGCommand Command { get; }
        
        public Type CustomType => Command.ObjectType;

        public int SelectedIndex {
            get => _selectedIndex;
            set => _selectedIndex = value >= Command.Optionals.Count() ? 0 : value;
        }
        private int _selectedIndex;
        #endregion

        #region Initializer
        private GEnumeration(IGCommand command)
        {
            if (command.MethodType != CommandType.Enum)
            {
                throw new Exception("Cannot create GEnum");
            }

            Command = command;
        }

        public GEnumeration(IGCommand command, int index = 0) : this(command)
        {
            SelectedIndex = index;
        }
        #endregion

        public override string ToSource()
        {
            return Command.Optionals.ElementAtOrDefault(SelectedIndex)?.FullName;
        }
    }
}
