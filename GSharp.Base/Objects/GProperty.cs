using GSharp.Base.Cores;
using GSharp.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects
{
    public class GProperty : GObject, ISettable
    {
        public GCommand GCommand
        {
            get
            {
                return _GCommand;
            }
        }
        private GCommand _GCommand;

        public GProperty(GCommand command)
        {
            _GCommand = command;
        }

        public override string ToSource()
        {
            return _GCommand.FullName;
        }
    }
}
