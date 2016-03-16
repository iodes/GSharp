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

        public static BaseHole CreateHole(String holeName)
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
