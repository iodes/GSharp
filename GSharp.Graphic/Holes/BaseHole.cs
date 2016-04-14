using GSharp.Graphic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GSharp.Graphic.Holes
{
    public abstract class BaseHole : UserControl
    {
        public abstract BaseBlock Block { get; }

        public delegate void HoleEventArgs(BaseBlock oldBlock, BaseBlock newBlock);

        public virtual event HoleEventArgs BlockChanged;

        public static BaseHole CreateHole(string holeName)
        {   
            switch (holeName)
            {
                case "logic":
                    return new LogicHole();

                case "object":
                    return new ObjectHole();

                case "variable":
                    return new VariableHole();

                default:
                    return null;
            }
        }
    }
}
