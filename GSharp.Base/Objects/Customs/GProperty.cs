using GSharp.Base.Cores;
using GSharp.Base.Utilities;
using GSharp.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects.Customs
{
    public class GProperty : GSettableObject, ICustom
    {
        public override Type SettableType
        {
            get
            {
                return GCommand.ObjectType;
            }
        }

        public GCommand GCommand
        {
            get
            {
                return _GCommand;
            }
        }

        public Type CustomType
        {
            get
            {
                return GCommand.ObjectType;
            }
        }

        private GCommand _GCommand;

        public GProperty(GCommand command)
        {
            _GCommand = command;
        }

        public override string ToSource()
        {
            return GCommand.FullName;
        }
    }
}
